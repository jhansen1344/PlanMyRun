using Microsoft.AspNet.Identity;
using PlanMyRun.Models.RacePlanModels;
using PlanMyRun.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PlanMyRun.MVC.Controllers
{
    public class RacePlanController : Controller
    {
        // GET: RacePlan
        public async Task<ActionResult> Index()
        {
            RacePlanService service = CreateRacePlanService();
            var model = await service.GetRacePlansAsync();

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RacePlanCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            RacePlanService service = CreateRacePlanService();
            if(await service.CreateRacePlanAsync(model))
                return RedirectToAction("Index");
            ModelState.AddModelError("", "Note could not be created.");
            return View(model);
        }

        private RacePlanService CreateRacePlanService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new RacePlanService(userId);
            return service;
        }
    }
}