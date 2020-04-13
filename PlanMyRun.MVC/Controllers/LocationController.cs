using Microsoft.AspNet.Identity;
using PlanMyRun.Models.LocationModels;
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
    public class LocationController : Controller
    {
        // GET: Location
        public async Task<ActionResult> Index()
        {
            var service = CreateLocationService();
            var model = await service.GetLocationsAsync();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LocationCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var service = CreateLocationService();
            if (await service.CreateLocationAsync(model))
            {
                TempData["SaveResult"] = "Location successfully created.";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Location could not be created.");
            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var service = CreateLocationService();
            var model = await service.GetLocationByIdAsync(id);
            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var service = CreateLocationService();
            var detail = await service.GetLocationByIdAsync(id);
            var model = new LocationEdit
            {
                Id = detail.Id,
                Name=detail.Name,
                MaxDistance=detail.MaxDistance,
                HasLoops=detail.HasLoops,
                Address=detail.Address,
                PathType=detail.PathType
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LocationEdit model)
        {
            var service = CreateLocationService();
            if (await service.EditLocationAsync(model))
            {
                TempData["SaveResult"] = "Location successfully updated.";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Location information could not be updated.");
            return View(model);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            var service = CreateLocationService();
            var model = await service.GetLocationByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteLocation(int id)
        {
            var service = CreateLocationService();
            await service.DeleteLocationAsync(id);
            TempData["SaveResult"] = "Location successfully deleted.";
            return RedirectToAction("Index");
        }

        private LocationService CreateLocationService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new LocationService(userId);
            return service;
        }
    }
}