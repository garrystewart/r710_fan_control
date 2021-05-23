using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using Microsoft.AspNet.SignalR;
using r710_fan_control.Services;
using static r710_fan_control.Services.IPMIService;
using static r710_fan_control.Services.TemperatureService;
using static r710_fan_control.Services.FanService;

namespace r710_fan_control.SignalR
{
    public class StatsHub : Hub
    {
        //private bool _broadcastTemperaturesRunning = false;
        //private bool _broadcastFanSpeedsRunning = false;

        public override Task OnConnected()
        {
            ////if (!_broadcastTemperaturesRunning) BroadcastTemperatures();
            //if (!_broadcastFanSpeedsRunning) BroadcastFanSpeeds();

            Test();

            return base.OnConnected();
        }

        //private void BroadcastTemperatures()
        //{
        //    _broadcastTemperaturesRunning = true;

        //    Clients.All.updateTemperatures(GetTemperatures().Result);
            
        //    BroadcastTemperatures();
        //}

        //private void BroadcastFanSpeeds()
        //{
        //    _broadcastFanSpeedsRunning = true;

        //    while (_broadcastFanSpeedsRunning)
        //    {
        //        IEnumerable<Sensor> fanSensors = GetFanSensors();
        //        Clients.All.updateFanSpeeds(fanSensors);
        //    }
        //}

        private void Test()
        {
            while (true) {
                Clients.All.test();
            }
        }
    }
}