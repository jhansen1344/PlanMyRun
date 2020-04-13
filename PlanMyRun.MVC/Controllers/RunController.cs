using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
    [Authorize]
    public class RunController : Controller
    {
        internal ApplicationUserManager _userManager;
        internal ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Run
        public async Task<ActionResult> Index()
        {
            var service = CreateRunService();
            var model = await service.GetRunsAsync();
            return View(model);
        }

        public async Task<JsonResult> GetRunsInPlan(int id)
        {
            var service = CreateRunService();
            var model = await service.GetRunsInPlanAsync(id);
            var jsonResult = new JsonResult { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return jsonResult;
        }

        public ActionResult EditRunsInPlan(int id)
        {
            var model = new RunDetail()
            {
                Id = id
            };
            return View(model);
        }

        public async Task<JsonResult> GetUnscheduledRuns()
        {
            var service = CreateRunService();
            var model = await service.GetUnscheduledRunsAsync();
            var jsonResult = new JsonResult { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return jsonResult;
        }

        public async Task<ActionResult> GetRunsWithForecast()
        {
            var service = CreateRunService();
            var model = await service.GetRunsWithForecastAsync();
            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            var service = CreateRunService();
            var model = await service.GetCreateSelectListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RunCreate model)
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
            var listModel = await service.GetCreateSelectListAsync();
            var model = new RunEdit
            {
                Id = detail.Id,
                RacePlanId = detail.RacePlanId,
                PlannedDistance = detail.PlannedDistance,
                ScheduleDateTime = detail.ScheduleDateTime.ToString(),
                LocationId = detail.LocationId,
                ActualDistance = detail.ActualDistance,
                ActualTime = detail.ActualTime,
                Description = detail.Description,
                RacePlan=listModel.RacePlan,
                Locations=listModel.Locations
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
            var user = UserManager.FindById(userId.ToString());
            string zipCode = user.ZipCode;
            var userPace = user.Pace;
            
          var service = new RunService(userId, zipCode,userPace);
            return service;
        }
    }
}