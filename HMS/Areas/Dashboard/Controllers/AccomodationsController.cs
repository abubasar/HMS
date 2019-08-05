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
    public class AccomodationsController : Controller
    { 
        private readonly AccomodationPackagesService accomodationPackagesService;
        private readonly AccomodationsService accomodationsService;
        public AccomodationsController()
        {
            
            this.accomodationPackagesService = new AccomodationPackagesService();
            this.accomodationsService = new AccomodationsService();
        }

        public ActionResult Index(string searchTerm, string AccomodationPackageId, int? page)
        {
            int recordSize = 3;
            page = page ?? 1;//page null hole 1 assign kor dia.
            AccomodationsListingModel viewModel = new AccomodationsListingModel();
            viewModel.SearchTerm = searchTerm;
            viewModel.AccomodationPackageId = AccomodationPackageId;
            viewModel.AccomodationPackages = accomodationPackagesService.GetAllAccomodationPackages().ToList();
            viewModel.Accomodations = accomodationsService.SearchAccomodations(searchTerm, AccomodationPackageId, page.Value, recordSize).ToList();
            var totalRecords = accomodationsService.SearchAccomodationsCount(searchTerm, AccomodationPackageId);
            viewModel.Pager = new Pager(totalRecords, page, recordSize);
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Action(string id)
        {

            AccomodationsActionModel model = new AccomodationsActionModel();
            model.AccomodationPackages = accomodationPackagesService.GetAllAccomodationPackages();
            if (id != null)//editing
            {
                var accomodation = accomodationsService.GetAccomodationById(id);
                model.Id = accomodation.Id;
                model.Name = accomodation.Name;
                model.Description = accomodation.Description;
                model.AccomodationPackageId = accomodation.AccomodationPackageId;
            }

            return PartialView("_Action", model);
        }
        [HttpPost]
        public JsonResult Action(AccomodationsActionModel viewModel)
        {
            var result = false;
            JsonResult json = new JsonResult();
            if (viewModel.Id != null)//editing
            {
                var accomodation = accomodationsService.GetAccomodationById(viewModel.Id);
                accomodation.Id = viewModel.Id;
                accomodation.Name = viewModel.Name;
                accomodation.Description= viewModel.Description;
                accomodation.AccomodationPackageId = viewModel.AccomodationPackageId;
                result = accomodationsService.UpdateAccomodation(accomodation);
            }
            else//adding
            {
                Accomodation model = new Accomodation();
                model.Id = Guid.NewGuid().ToString();
                model.Name = viewModel.Name;
                model.Description = viewModel.Description;
                model.AccomodationPackageId = viewModel.AccomodationPackageId;
                result = accomodationsService.SaveAccomodation(model);
            }


            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable To Perform Action on Accomodation" };
            }
            return json;
        }


        [HttpGet]
        public ActionResult Delete(string id)
        {
            AccomodationsActionModel model = new AccomodationsActionModel();

            var accomodation = accomodationsService.GetAccomodationById(id);
            model.Id = accomodation.Id;


            return PartialView("_Delete", model);
        }


        [HttpPost]
        public JsonResult Delete(AccomodationsActionModel viewModel)
        {
            var result = false;
            JsonResult json = new JsonResult();

            var accomodation = accomodationsService.GetAccomodationById(viewModel.Id);
            result = accomodationsService.DeleteAccomodation(accomodation);

            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable To Perform Action on Accomodation" };
            }
            return json;
        }

    }

}