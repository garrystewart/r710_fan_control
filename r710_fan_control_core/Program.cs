using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using r710_fan_control_core.Services;

namespace r710_fan_control_core
{
    public class Program
    {   
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
