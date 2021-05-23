using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static r710_fan_control.Services.IPMIService;
using static r710_fan_control.Services.TemperatureService;

namespace r710_fan_control.ViewModels
{
    public class HomeIndexVM
    {
        public IEnumerable<Temperature> Temperatures { get; set; }
        public IEnumerable<Sensor> FanSpeeds { get; set; }
    }
}