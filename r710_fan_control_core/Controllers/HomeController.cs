using Microsoft.AspNetCore.Mvc;
using r710_fan_control_core.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace r710_fan_control_core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ModeService _modeService;

        public HomeController(ModeService modeService)
        {
            _modeService = modeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public void Auto()
        {
            _modeService.Auto();
        }

        [HttpGet]
        public async Task AutoLow()
        {
            await _modeService.AutoLow();
        }
    }
}
