using Microsoft.AspNet.Identity;
using PlanMyRun.Models.RacePlanModels;
using PlanMyRun.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanMyRun.MVC.Controllers
{
    public class RacePlanController : Controller
    {
        // GET: RacePlan
        public ActionResult Index()
        {
            RacePlanService service = CreateRacePlanService();
            var model = service.GetRacePlans();

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RacePlanCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            RacePlanService service = CreateRacePlanService();
            service.CreateRacePlan(model);

            return RedirectToAction("Index");

        }

        private RacePlanService CreateRacePlanService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new RacePlanService(userId);
            return service;
        }
    }
}