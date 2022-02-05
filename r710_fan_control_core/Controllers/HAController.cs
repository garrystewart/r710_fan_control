using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using r710_fan_control_core.Models;
using r710_fan_control_core.Models.API;
using r710_fan_control_core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace r710_fan_control_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HAController : ControllerBase
    {
        private readonly IIpmiService _ipmiService;
        private readonly ITemperatureService _temperatureService;
        private readonly IOpenHardwareService _openHardwareService;

        public HAController(IIpmiService ipmiService, ITemperatureService temperatureService, IOpenHardwareService openHardwareService)
        {
            _ipmiService = ipmiService;
            _temperatureService = temperatureService;
            _openHardwareService = openHardwareService;
        }

        [Route("test")]
        [HttpGet]
        public void Test()
        {
            _temperatureService.GetMaxTemperature();
        }

        [HttpGet]
        public HomeAssistant Get()
        {
            while (_ipmiService.Sensors == null)
            {
                Thread.Sleep(1000);
            }

            var sensors = _ipmiService.Sensors;

            var power = new HomeAssistant.PowerType();

            var current = sensors.Single(s => s.ProbeName == "Current" && s.Measurement != Measurement.None);
            var voltage = sensors.Single(s => s.ProbeName == "Voltage" && s.Measurement != Measurement.None);
            var system = sensors.Single(s => s.ProbeName == "System Level");

            var openHardwareMonitor = new HomeAssistant.OpenHardwareMonitorType
            {
                Memory = new HomeAssistant.OpenHardwareMonitorType.MemoryType
                {
                    Load = _openHardwareService.Sensors.Memory.Load,
                    UsedMemory = _openHardwareService.Sensors.Memory.UsedMemory,
                    AvailableMemory = _openHardwareService.Sensors.Memory.AvailableMemory
                },
                SolidStateDrive = new HomeAssistant.OpenHardwareMonitorType.SolidStateDriveType
                {
                    Temperature = _openHardwareService.Sensors.SolidStateDrive.Temperature,
                    UsedSpace = _openHardwareService.Sensors.SolidStateDrive.UsedSpace,
                    RemainingLife = _openHardwareService.Sensors.SolidStateDrive.RemainingLife,
                    WriteAmplification = _openHardwareService.Sensors.SolidStateDrive.WriteAmplification,
                    TotalBytesWritten = _openHardwareService.Sensors.SolidStateDrive.TotalBytesWritten
                },
                LastUpdated = _openHardwareService.LastUpdated,
                Latency = _openHardwareService.Latency
            };

            foreach (var processor in _openHardwareService.Sensors.Processors)
            {
                foreach (var core in processor.Cores)
                {
                    openHardwareMonitor.Cores.Add(new HomeAssistant.OpenHardwareMonitorType.Core
                    {
                        Clock = core.Clock,
                        Temperature = core.Temperature,
                        Load = core.Load
                    });
                }
            }

            return new HomeAssistant()
            {
                Fans = new HomeAssistant.FanType
                {
                    FansList = sensors.Where(s => s.ProbeName.Contains("FAN"))
                        .Select(s => new HomeAssistant.Fan
                        {
                            Reading = Convert.ToInt32(Convert.ToDecimal(s.Reading))
                        })
                        .ToList(),
                    FansModeAverage = new HomeAssistant.Fan
                    {
                        Reading = sensors
                        .Where(s => s.ProbeName.Contains("FAN"))
                        .GroupBy(n => Convert.ToInt32(Convert.ToDecimal(n.Reading)))
                        .OrderByDescending(g => g.Count())
                        .Select(g => g.Key)
                        .FirstOrDefault()
                    }
                },
                Power = new HomeAssistant.PowerType
                {
                    Current = Convert.ToDecimal(current.Reading),
                    Voltage = Convert.ToDecimal(voltage.Reading),
                    Watts = Convert.ToDecimal(system.Reading)
                },
                Ipmi = new HomeAssistant.IpmiType
                {
                    LastUpdated = _ipmiService.LastUpdated,
                    Latency = _ipmiService.Latency
                },
                OpenHardwareMonitor = openHardwareMonitor
            };
        }
    }
}
