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
        // GET: Forecast
        public async Task<ActionResult> Index()
        {
            var service = new ForecastService();
            var model = await service.GetForecastAsync();
            model.Days.ToList();
            return View(model.Days);
        }
    }
}