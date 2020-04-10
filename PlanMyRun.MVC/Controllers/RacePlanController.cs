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
    [Authorize]
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

        public async Task<ActionResult> Details(int id)
        {
            var service = CreateRacePlanService();
            var model = await service.GetPlanByIdAsync(id);
            return View(model);
        }

        public async Task<ActionResult> Edit (int id)
        {
            var service = CreateRacePlanService();
            var detail = await service.GetPlanByIdAsync(id);
            var model = new RacePlanEdit
            {
                Id=detail.Id,
                RaceName=detail.RaceName,
                RaceDate=detail.RaceDate,
                IsPublic=detail.IsPublic,
                RaceLength=detail.RaceLength,
                GoalTime=detail.GoalTime,
                Description=detail.Description
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit (RacePlanEdit model)
        {
            var service = CreateRacePlanService();
            if (await service.EditRacePlanAsync(model))
                return RedirectToAction("Index");
            ModelState.AddModelError("", "Plan could not be updated.");
            return View(model);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            var service = CreateRacePlanService();
            var model = await service.GetPlanByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePlan(int id)
        {
            var service = CreateRacePlanService();
            await service.DeletePlanAsync(id);
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