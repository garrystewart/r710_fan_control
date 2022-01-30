using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace r710_fan_control_core.Controllers
{
    public class TemperaturesController : Controller
    {
        public IActionResult Live()
        {
            return View();
        }

        public IActionResult Control()
        {
            return View();
        }
    }
}
