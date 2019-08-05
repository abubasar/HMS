using HMS.Areas.Dashboard.ViewModels;
using HMS.Entities;
using HMS.Services;
using HMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class AccomodationPackagesController : Controller
    {
        private readonly AccomodationTypesService accomodationTypesService;
        private readonly AccomodationPackagesService accomodationPackagesService;

        public AccomodationPackagesController()
        {
            this.accomodationTypesService = new AccomodationTypesService();
            this.accomodationPackagesService = new AccomodationPackagesService();
        }
       
        public ActionResult Index(string searchTerm,string AccomodationTypeId,int? page)
        {
            int recordSize = 3;
            page = page ?? 1;//page null hole 1 assign kor dia.
            AccomodationPackageListingModel viewModel = new AccomodationPackageListingModel();
            viewModel.SearchTerm = searchTerm;
            viewModel.AccomodationTypeId = AccomodationTypeId;
            viewModel.AccomodationTypes = accomodationTypesService.GetAllAccomodationTypes().ToList();
            viewModel.AccomodationPackages = accomodationPackagesService.SearchAccomodationPackages(searchTerm, AccomodationTypeId,page.Value,recordSize).ToList();
            var totalRecords = accomodationPackagesService.SearchAccomodationPackagesCount(searchTerm, AccomodationTypeId);
            viewModel.Pager = new Pager(totalRecords, page, recordSize);
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Action(string id) { 
       
            AccomodationPackageActionModel model = new AccomodationPackageActionModel();
            model.AccomodationTypes = accomodationTypesService.GetAllAccomodationTypes();
            if (id != null)//editing
            {
                var accomodationPackage = accomodationPackagesService.GetAccomodationPackageBYId(id);
                model.Id = accomodationPackage.Id;
                model.Name = accomodationPackage.Name;
                model.NoOfRoom = accomodationPackage.NoOfRoom;
                model.FeePerNight = accomodationPackage.FeePerNight;
                model.AccomodationTypeId = accomodationPackage.AccomodationTypeId;
            }

            return PartialView("_Action", model);
        }
        [HttpPost]
        public JsonResult Action(AccomodationPackageActionModel viewModel)
        {
            var result = false;
            JsonResult json = new JsonResult();
            if (viewModel.Id != null)//editing
            {
                var accomodationPackage = accomodationPackagesService.GetAccomodationPackageBYId(viewModel.Id);
                accomodationPackage.Id = viewModel.Id;
                accomodationPackage.Name = viewModel.Name;
                accomodationPackage.NoOfRoom = viewModel.NoOfRoom;
                accomodationPackage.FeePerNight = viewModel.FeePerNight;
                accomodationPackage.AccomodationTypeId = viewModel.AccomodationTypeId;
                result = accomodationPackagesService.UpdateAccomodationPackage(accomodationPackage);
            }
            else//adding
            {
                AccomodationPackage model = new AccomodationPackage();
                model.Id = Guid.NewGuid().ToString();
                model.Name = viewModel.Name;
                model.NoOfRoom = viewModel.NoOfRoom;
                model.FeePerNight = viewModel.FeePerNight;
                model.AccomodationTypeId = viewModel.AccomodationTypeId;
                result = accomodationPackagesService.SaveAccomodationPackage(model);
            }


            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable To Perform Action on Accomodation Package" };
            }
            return json;
        }


        [HttpGet]
        public ActionResult Delete(string id)
        {
            AccomodationPackageActionModel model = new AccomodationPackageActionModel();

            var accomodationPackage = accomodationPackagesService.GetAccomodationPackageBYId(id);
            model.Id = accomodationPackage.Id;


            return PartialView("_Delete", model);
        }


        [HttpPost]
        public JsonResult Delete(AccomodationPackageActionModel viewModel)
        {
            var result = false;
            JsonResult json = new JsonResult();

            var accomodationPackage = accomodationPackagesService.GetAccomodationPackageBYId(viewModel.Id);
            result = accomodationPackagesService.DeleteAccomodationPackage(accomodationPackage);

            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable To Perform Action on Accomodation Package" };
            }
            return json;
        }

    }

}
