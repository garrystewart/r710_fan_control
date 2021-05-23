using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using r710_fan_control.Models;
using r710_fan_control_core.Services;

namespace r710_fan_control_core.Controllers
{
    public class FansController : Controller
    {
        public IActionResult Live()
        {
            return View();
        }

        public IActionResult Control()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SwitchToAutomatic()
        {
            FanService.SwitchToAutomatic();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SwitchToManual(Fan model)
        {
            FanService.SwitchToManual(model.Speed);

            return RedirectToAction("Index");
        }
    }
}
