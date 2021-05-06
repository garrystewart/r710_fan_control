using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using Microsoft.AspNet.SignalR;
using static r710_fan_control.Services.TemperatureService;

namespace r710_fan_control.SignalR
{
    public class TemperatureHub : Hub
    {
        private readonly Timer _timer;

        public TemperatureHub()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += BroadcastTemperatures;
            _timer.AutoReset = true;            
        }

        public override Task OnConnected()
        {
            if (!_timer.Enabled) _timer.Enabled = true;

            return base.OnConnected();
        }

        public void BroadcastTemperatures(Object source, ElapsedEventArgs e)
        {
            Clients.All.updateTemperatures(GetTemperatures());
        }
    }
}