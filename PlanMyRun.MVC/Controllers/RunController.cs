using Microsoft.AspNet.Identity;
using PlanMyRun.Models.RunModels;
using PlanMyRun.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PlanMyRun.MVC.Controllers
{
    public class RunController : Controller
    {
        // GET: Run
        public async Task<ActionResult> Index()
        {
            var service = CreateRunService();
            var model = await service.GetRunsAsync();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create (RunCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var service = CreateRunService();
            if (await service.CreateRunAsync(model))
                return RedirectToAction("Index");
            ModelState.AddModelError("", "Run could not be created.");
            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var service = CreateRunService();
            var model = await service.GetRunByIdAsync(id);
            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var service = CreateRunService();
            var detail = await service.GetRunByIdAsync(id);
            var model = new RunEdit
            {
                Id = detail.Id,
                RacePlanId=detail.RacePlanId,
                PlannedDistance=detail.PlannedDistance,
                EstimatedTime=detail.EstimatedTime,
                ScheduleDateTime=detail.ScheduleDateTime,
                LocationId=detail.LocationId,
                ActualDistance=detail.ActualDistance,
                ActualTime=detail.ActualTime
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RunEdit model)
        {
            var service = CreateRunService();
            if (await service.EditRunAsync(model))
                return RedirectToAction("Index");
            ModelState.AddModelError("", "Plan could not be updated.");
            return View(model);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            var service = CreateRunService();
            var model = await service.GetRunByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRun(int id)
        {
            var service = CreateRunService();
            await service.DeleteRunAsync(id);
            return RedirectToAction("Index");
        }


        private RunService CreateRunService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new RunService(userId);
            return service;
        }
    }
}