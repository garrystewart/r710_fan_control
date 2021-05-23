using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using r710_fan_control.Models;
using r710_fan_control.Services;

namespace r710_fan_control.Controllers
{
    public class FanController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetFans()
        {
            FanService.GetFanSensors();

            return View();
        }

        [HttpPost]
        public ActionResult SwitchToAutomatic()
        {
            FanService.SwitchToAutomatic();

            return RedirectToAction("Control");
        }

        [HttpPost]
        public ActionResult SwitchToManual(Fan model)
        {
            FanService.SwitchToManual(model.Speed);

            return RedirectToAction("Control");
        }
    }
}