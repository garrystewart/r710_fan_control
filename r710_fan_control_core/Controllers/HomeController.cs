using Microsoft.AspNetCore.Mvc;
using r710_fan_control_core.Services;

namespace r710_fan_control_core.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public void Auto()
        {
            FanService.SwitchToAutomatic();
        }

        [HttpGet]
        public async Task AutoLow()
        {
            decimal cutoff = 45;

            FanService.SwitchToManual("0");

            // check temps every 10 seconds
            while (true)
            {
                decimal maxTemp = 0;

                try
                {
                    maxTemp = (await TemperatureService.GetTemperatures()).Max(t => t.Value);
                    System.Diagnostics.Debug.WriteLine(maxTemp);

                    if (maxTemp > cutoff)
                    {
                        FanService.SwitchToAutomatic();
                    }
                    else
                    {
                        FanService.SwitchToManual("0");
                    }
                }
                catch (Exception ex)
                {
                    FanService.SwitchToAutomatic();
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }  

                Thread.Sleep(10000);
            }
        }
    }
}
