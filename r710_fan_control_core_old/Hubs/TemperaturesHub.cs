using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using r710_fan_control_core.Services;

namespace r710_fan_control_core.Hubs
{
    public class TemperaturesHub : Hub
    {
        public async Task UpdateTemperatures()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var temperatures = await TemperatureService.GetTemperatures();
            stopwatch.Stop();

            await Clients.Caller.SendAsync("UpdateTemperatures", temperatures, stopwatch.ElapsedMilliseconds, DateTime.Now);
        }
    }
}