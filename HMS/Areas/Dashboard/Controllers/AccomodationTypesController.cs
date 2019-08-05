using HMS.Areas.Dashboard.ViewModels;
using HMS.Entities;
using HMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class AccomodationTypesController : Controller
    {
        private readonly AccomodationTypesService accomodationTypesService;
        public AccomodationTypesController()
        {
            this.accomodationTypesService = new AccomodationTypesService();
        }
        // GET: Dashboard/AccomodationTypes
        public ActionResult Index(string searchTerm)
        {
            AccomodationTypeListingModel viewModel = new AccomodationTypeListingModel();
            viewModel.SearchTerm = searchTerm;
            viewModel.AccomodationTypes = accomodationTypesService.SearchAccomodationTypes(searchTerm).ToList();
           
            return View(viewModel);
        }
       
        [HttpGet]
        public ActionResult Action(string id)
        {
            AccomodationTypeActionModel model = new AccomodationTypeActionModel();
            if (id != null)//editing
            {
                var accomodationType = accomodationTypesService.GetAccomodationTypeBYId(id);
                model.Id = accomodationType.Id;
                model.Name = accomodationType.Name;
                model.Description = accomodationType.Description;
            }

            return PartialView("_Action", model);
        }
        [HttpPost]
        public JsonResult Action(AccomodationTypeActionModel viewModel)
        {
            var result = false;
            JsonResult json = new JsonResult();
            if (viewModel.Id != null)//editing
            {
                var accomodationType = accomodationTypesService.GetAccomodationTypeBYId(viewModel.Id);
                accomodationType.Id = viewModel.Id;
                accomodationType.Name = viewModel.Name;
                accomodationType.Description = viewModel.Description;
                result = accomodationTypesService.UpdateAccomodationType(accomodationType);
            }
            else//adding
            {
                AccomodationType model = new AccomodationType();
                model.Id = Guid.NewGuid().ToString();
                model.Name = viewModel.Name;
                model.Description = viewModel.Description;
                result = accomodationTypesService.SaveAccomodationType(model);
            }


            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable To Perform Action on Accomodation Type" };
            }
            return json;
        }


        [HttpGet]
        public ActionResult Delete(string id)
        {
            AccomodationTypeActionModel model = new AccomodationTypeActionModel();

            var accomodationType = accomodationTypesService.GetAccomodationTypeBYId(id);
            model.Id = accomodationType.Id;


            return PartialView("_Delete", model);
        }


        [HttpPost]
        public JsonResult Delete(AccomodationTypeActionModel viewModel)
        { 
            var result = false;
            JsonResult json = new JsonResult();

            var accomodationType = accomodationTypesService.GetAccomodationTypeBYId(viewModel.Id);
            result = accomodationTypesService.DeleteAccomodationType(accomodationType);

            if (result)
            { 
                json.Data = new { Success = true };
            }
            else 
            {
                json.Data = new { Success = false, Message = "Unable To Perform Action on Accomodation Type" };
            }
            return json;
        }

    }

}
