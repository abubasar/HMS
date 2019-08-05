using HMS.Areas.Dashboard.ViewModels;
using HMS.Entities;
using HMS.Services;
using HMS.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class RolesController : Controller
    {

        private HMSSignInManager _signInManager;
        private HMSUserManager _userManager;
        private HMSRoleManager _roleManager;
        public RolesController()
        {
        }

        public RolesController(HMSUserManager userManager, HMSSignInManager signInManager,HMSRoleManager roleManager)
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

        public ActionResult Index(string searchTerm, int? page)
        {
            int recordSize = 3;
            page = page ?? 1;//page null hole 1 assign kor dia 
            RolesListingModel viewModel = new RolesListingModel();
            viewModel.SearchTerm = searchTerm;
            viewModel.Roles = SearchRoles(searchTerm, page.Value, recordSize);
            var totalRecords = SearchRolesCount(searchTerm);
            viewModel.Pager = new Pager(totalRecords, page, recordSize);
            return View(viewModel);
        }


        public IEnumerable<IdentityRole> SearchRoles(string searchTerm, int page, int recordSize)
        {
            var queryable = RoleManager.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));

            }
         
            return queryable.OrderBy(x => x.Name).Skip((page - 1) * recordSize).Take(recordSize).AsEnumerable();

        }

        public int SearchRolesCount(string searchTerm)
        {
            var queryable = RoleManager.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));

            }
         
            return queryable.Count();

        }



        [HttpGet]
        public async Task<ActionResult> Action(string id)
        {

            RolesActionModel model = new RolesActionModel();
           
            if (!string.IsNullOrEmpty(id))//editing 
            {
                var role = await RoleManager.FindByIdAsync(id);
                model.Id = role.Id;
                model.Name = role.Name;

            }

            return PartialView("_Action", model);
        }
        [HttpPost]
        public async Task<JsonResult> Action(RolesActionModel viewModel)
        {
            IdentityResult result = null;
            JsonResult json = new JsonResult();
            if (!string.IsNullOrEmpty(viewModel.Id))//editing
            {
                var role = await RoleManager.FindByIdAsync(viewModel.Id);
                role.Id = viewModel.Id;
                role.Name = viewModel.Name;
                result = await RoleManager.UpdateAsync(role);

            }
            else//adding
            {
                var role = new IdentityRole();
                role.Name = viewModel.Name;
                result = await RoleManager.CreateAsync(role);

            }

            json.Data = new { Success = result.Succeeded, Message = string.Join(",  ", result.Errors) };
            return json;
        }


        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            RolesActionModel model = new RolesActionModel();
            var role= await RoleManager.FindByIdAsync(id);
            model.Id = role.Id;

            return PartialView("_Delete", model);
        }


        [HttpPost]
        public async Task<JsonResult> Delete(RolesActionModel viewModel)
        {
            IdentityResult result = null;
            JsonResult json = new JsonResult();
            if (!string.IsNullOrEmpty(viewModel.Id))
            {
                var role = await RoleManager.FindByIdAsync(viewModel.Id);
                result = await RoleManager.DeleteAsync(role);
                json.Data = new { Success = result.Succeeded, Message = string.Join(",  ", result.Errors) };
            }
            else
            {
                json.Data = new { Success = false, Message = "Invalid Role/No Role Found " };
            }

            return json;
        }

    }

}