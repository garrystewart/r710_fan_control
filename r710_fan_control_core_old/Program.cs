using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using r710_fan_control_core.Services;
using static r710_fan_control_core.Services.TemperatureService;

namespace r710_fan_control_core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Task task = Task.Run(() => Test());

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private async static Task Test()
        {
            decimal cutoff = 50;

            FanService.SwitchToManual(0);

            while (true)
            {
                decimal currentMaximumTemperature = (await GetTemperatures()).Max(t => t.Value);

                System.Diagnostics.Debug.WriteLine(currentMaximumTemperature);

                if (currentMaximumTemperature > cutoff)
                {
                    FanService.SwitchToAutomatic();
                    Thread.Sleep(60000);
                }
                else
                {
                    FanService.SwitchToManual(0);
                    Thread.Sleep(10000);
                }                
            }
        }
    }
}
