using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PlanMyRun.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PlanMyRun.MVC.Controllers
{
    public class ForecastController : Controller
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
        // GET: Forecast
        public async Task<ActionResult> Index()
        {
            var service = CreateForecastService();
            var model = await service.GetForecastAsync();
            model.Days.ToList();
            return View(model.Days);
        }

        private ForecastService CreateForecastService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());

            var user = UserManager.FindById(userId.ToString());
            var zipCode = user.ZipCode;
            var service = new ForecastService(userId, zipCode);
            return service;
        }
    }
}