using r710_fan_control_core.Models;
using r710_fan_control_core.Models.JSON;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace r710_fan_control_core.Services
{
    public class TemperatureService : ITemperatureService
    {
        private readonly IOpenHardwareService _openHardwareService;

        public TemperatureService(IOpenHardwareService openHardwareService)
        {
            _openHardwareService = openHardwareService;
        }

        public int GetMaxTemperature()
        {
            var temperatures = new List<int>();

            foreach (var processor in _openHardwareService.Sensors.Processors)
            {
                temperatures.AddRange(processor.Cores.Select(c => c.Temperature));
            }

            return temperatures.Max();
        }
    }
}
