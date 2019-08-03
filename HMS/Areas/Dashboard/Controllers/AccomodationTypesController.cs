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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Listing()
        {
            AccomodationTypesViewModel viewModel = new AccomodationTypesViewModel();
            viewModel.AccomodationTypes = accomodationTypesService.GetAllAccomodationTypes().ToList();
            return PartialView("_Listing",viewModel);
        }
        [HttpGet]
        public ActionResult Action()
        {
            AccomodationTypeActionModel model = new AccomodationTypeActionModel();
            return PartialView("_Action",model);
        }
        [HttpPost]
        public JsonResult Action(AccomodationTypeActionModel viewModel)
        {
            JsonResult json = new JsonResult();
            AccomodationType model = new AccomodationType();
            model.Id = Guid.NewGuid().ToString();
            model.Name = viewModel.Name;
            model.Description = viewModel.Description;

            var result = accomodationTypesService.SaveAccomodationType(model);
            if(result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false,Message="Unable To Add Accomodation Type" };
            }
            return json;
        }

    }
}