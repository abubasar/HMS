using HMS.Areas.Dashboard.ViewModels;
using HMS.Data;
using HMS.Entities;
using HMS.Services;
using HMS.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class UsersController : Controller
    {

        private HMSSignInManager _signInManager;
        private HMSUserManager _userManager;
        private HMSRoleManager _roleManager;

        public UsersController()
        {
        }

        public UsersController(HMSUserManager userManager, HMSSignInManager signInManager, HMSRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public HMSSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<HMSSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public HMSUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<HMSUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public HMSRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<HMSRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public async Task<ActionResult> Index(string searchTerm, string roleId, int? page)
        {
            int recordSize = 3;
            page = page ?? 1;//page null hole 1 assign kor dia 
            UsersListingModel viewModel = new UsersListingModel();
            viewModel.SearchTerm = searchTerm;
            viewModel.RoleId = roleId;
            viewModel.Roles = RoleManager.Roles.ToList();
            viewModel.Users =await SearchUsers(searchTerm, roleId, page.Value, recordSize);
            var totalRecords = await SearchUsersCount(searchTerm, roleId);
            viewModel.Pager =  new Pager(totalRecords, page, recordSize);
            return View(viewModel);
        }


        public async Task<IEnumerable<HMSUser>> SearchUsers(string searchTerm, string roleId, int page, int recordSize)
        { 
                var queryable =UserManager.Users.AsQueryable();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    queryable = queryable.Where(x => x.UserName.ToLower().Contains(searchTerm.ToLower())

                    || x.Email.ToLower().ToString().Contains(searchTerm.ToLower()));
                }
                if (!string.IsNullOrEmpty(roleId))
                {
                //  queryable = queryable.Where(x => x.Roles.Select(y => y.RoleId).Contains(roleId));
                var role = await RoleManager.FindByIdAsync(roleId);
                var userIds = role.Users.Select(x => x.UserId).ToList();
                queryable = queryable.Where(x => userIds.Contains(x.Id));
            }

                return queryable.OrderBy(x => x.Email).Skip((page - 1) * recordSize).Take(recordSize).AsEnumerable();
      
        }
         
        public async Task<int> SearchUsersCount(string searchTerm, string roleId)
        {
            var queryable = UserManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                queryable = queryable.Where(x => x.UserName.ToLower().Contains(searchTerm.ToLower())

                || x.Email.ToLower().ToString().Contains(searchTerm.ToLower()));
            }
            if (!string.IsNullOrEmpty(roleId))
            {
                var role = await RoleManager.FindByIdAsync(roleId);
                var userIds = role.Users.Select(x => x.UserId).ToList();
                queryable = queryable.Where(x => userIds.Contains(x.Id));
            }
              
            return queryable.Count();

        }



        [HttpGet]
        public async Task<ActionResult> Action(string id)
        {

          UsersActionModel model = new UsersActionModel();
           // model.Roles = accomodationPackagesService.GetAllAccomodationPackages();
            if (!string.IsNullOrEmpty(id))//editing 
            {
                var user =await UserManager.FindByIdAsync(id);
                model.Id = user.Id;
                model.FullName = user.FullName;
                model.Email = user.Email;
                model.UserName = user.UserName;
                model.Country = user.Country;
                model.City = user.City;
                model.Address = user.Address;
               
            }

            return PartialView("_Action", model);
        }
        [HttpPost]
        public async Task<JsonResult> Action(UsersActionModel viewModel)
        {
            IdentityResult result = null;
            JsonResult json = new JsonResult();
            if (!string.IsNullOrEmpty(viewModel.Id))//editing
            {
                var user = await UserManager.FindByIdAsync(viewModel.Id);
                user.Id = viewModel.Id;
                user.FullName = viewModel.FullName;
                user.Email = viewModel.Email;
                user.UserName = viewModel.UserName;
                user.Country = viewModel.Country;
                user.City = viewModel.City;
                user.Address = viewModel.Address;
                result=await UserManager.UpdateAsync(user);
              
            }
            else//adding
            {
                var user = new HMSUser();
                user.FullName = viewModel.FullName;
                user.Email = viewModel.Email;
                user.UserName = viewModel.UserName;
                user.Country = viewModel.Country;
                user.City = viewModel.City;
                user.Address = viewModel.Address;
                result = await UserManager.CreateAsync(user);

            }

            json.Data = new { Success = result.Succeeded, Message = string.Join(",  ", result.Errors) };
            return json;
        }


        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            UsersActionModel model = new UsersActionModel();
            var user = await UserManager.FindByIdAsync(id);
            model.Id = user.Id;

            return PartialView("_Delete", model);
        }


        [HttpPost]
        public async Task<JsonResult> Delete(UsersActionModel viewModel)
        {
            IdentityResult result = null;
            JsonResult json = new JsonResult();
            if (!string.IsNullOrEmpty(viewModel.Id))
            {
                var user = await UserManager.FindByIdAsync(viewModel.Id);
                result = await UserManager.DeleteAsync(user);
                json.Data = new { Success = result.Succeeded, Message = string.Join(",  ", result.Errors) };
            }
            else
            {
                json.Data = new { Success = false, Message = "Invalid User/No User Found " };
            }
           
            return json;
        }




        [HttpGet]
        public async Task<ActionResult> UserRoles(string id) 
        {
            var user = await UserManager.FindByIdAsync(id);
            UserRolesModel model = new UserRolesModel();
            model.UserId = id;
            model.Roles = RoleManager.Roles.ToList();
            var userRoleIds = user.Roles.Select(x => x.RoleId).ToList();
            model.UserRoles = RoleManager.Roles.Where(x => userRoleIds.Contains(x.Id)).ToList();
            model.Roles = RoleManager.Roles.Where(x => !userRoleIds.Contains(x.Id)).ToList();
            return PartialView("_UserRoles", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation">Type of operation Assign or delete Role</param>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UserRoleOperation(string userId,string roleId,bool isDelete=false)
        {
         
            JsonResult json = new JsonResult();
            var user =await UserManager.FindByIdAsync(userId);

            var role = await RoleManager.FindByIdAsync(roleId);

            if(user!=null && role != null)
            {
                IdentityResult result = null;
                if (!isDelete)
                {
                    result = await UserManager.AddToRoleAsync(userId, role.Name);
                }
                else 
                {
                    result = await UserManager.RemoveFromRoleAsync(userId, role.Name);
                }
            
                json.Data = new { Success = result.Succeeded, Message = string.Join(", ", result.Errors) };
            }
            else
            {
                json.Data = new { Success = false, Message = "Invalid Operation" };
            }

            return json;
        }


    }

}