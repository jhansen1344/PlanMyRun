using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
        // GET: RacePlan
        public async Task<ActionResult> Index()
        {
            RacePlanService service = CreateRacePlanService();
            var model = await service.GetRacePlansAsync();

            return View(model);
        }

        //Get: Templates
        public async Task<ActionResult> GetTemplates()
        {
            var service = CreateRacePlanService();
            var model = await service.GetTemplates();
            return View(model);
        }

        public ActionResult CreateFromTemplate(int id)
        {
            var model = new RacePlanTemplate()
            {
                ExistingPlanId = id
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateFromTemplate(RacePlanTemplate model)
        {
            var service = CreateRacePlanService();
            if (await service.CreateFromTemplateAsync(model))
                return RedirectToAction("Index");
            ModelState.AddModelError("", "Plan could not be created.");
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
            if (await service.CreateRacePlanAsync(model))
            {
                TempData["SaveResult"] = "Plan successfully created.";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Plan could not be created.");
            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var service = CreateRacePlanService();
            var model = await service.GetPlanByIdAsync(id);
            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var service = CreateRacePlanService();
            var detail = await service.GetPlanByIdAsync(id);
            var model = new RacePlanEdit
            {
                Id = detail.Id,
                RaceName = detail.RaceName,
                RaceDate = detail.RaceDate,
                IsPublic = detail.IsPublic,
                RaceLength = detail.RaceLength,
                GoalTime = detail.GoalTime,
                Description = detail.Description
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RacePlanEdit model)
        {
            var service = CreateRacePlanService();
            if (await service.EditRacePlanAsync(model))
            {
                TempData["SaveResult"] = "Plan successfully updated.";
                return RedirectToAction("Index");
            }
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
            TempData["SaveResult"] = "Plan successfully deleted.";
            return RedirectToAction("Index");
        }

        private RacePlanService CreateRacePlanService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var user = UserManager.FindById(userId.ToString());
            string zipCode = user.ZipCode;
            var userPace = user.Pace;

            var service = new RacePlanService(userId, zipCode, userPace);
            return service;
        }
    }
}