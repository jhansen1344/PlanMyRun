﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanMyRun.MVC.Controllers
{
    public class RunController : Controller
    {
        // GET: Run
        public ActionResult Index()
        {
            return View();
        }
    }
}