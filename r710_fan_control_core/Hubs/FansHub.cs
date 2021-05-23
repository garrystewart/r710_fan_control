using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using r710_fan_control_core.Services;

namespace r710_fan_control_core.Hubs
{
    public class FansHub : Hub
    {
        public async Task UpdateFanSpeeds()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var fanSensors = FanService.GetFanSensors();
            stopwatch.Stop();

            await Clients.Caller.SendAsync("UpdateFanSpeeds", fanSensors, stopwatch.ElapsedMilliseconds, DateTime.Now);
        }
    }
}
