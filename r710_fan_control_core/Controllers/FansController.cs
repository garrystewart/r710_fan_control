using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        [HttpGet]
        public async Task<IActionResult> SwitchToTest()
        {
            decimal cutoff = 45;

            FanService.SwitchToManual("0");

            // check temps every 10 seconds
            while (true)
            {
                decimal maxTemp = (await TemperatureService.GetTemperatures()).Max(t => t.Value);
                System.Diagnostics.Debug.WriteLine(maxTemp);

                if (maxTemp > cutoff)
                {
                    FanService.SwitchToAutomatic();
                }
                else
                {
                    FanService.SwitchToManual("0");                    
                }

                Thread.Sleep(10000);
            }
        }

        [HttpPost]
        public IActionResult SwitchToAutomatic()
        {
            FanService.SwitchToAutomatic();

            return RedirectToAction("Control");
        }

        [HttpPost]
        public IActionResult SwitchToManual(Fan model)
        {
            FanService.SwitchToManual(model.Speed);

            return RedirectToAction("Control");
        }
    }
}
