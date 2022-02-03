using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using r710_fan_control_core.Models;
using r710_fan_control_core.Models.API;
using r710_fan_control_core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace r710_fan_control_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HAController : ControllerBase
    {
        private readonly IpmiService _ipmiService;

        public HAController(IpmiService ipmiService)
        {
            _ipmiService = ipmiService;
        }

        [HttpGet]
        public HomeAssistant Get()
        {
            while (_ipmiService.Sensors == null)
            {

            }

            var sensors = _ipmiService.Sensors;

            var power = new List<HomeAssistant.PowerType>();

            var current = sensors.Single(s =>
                s.ProbeName == "Current" && s.Measurement != Measurement.None);

            power.Add(new HomeAssistant.PowerType
            {
                Name = "Current",
                Reading = Convert.ToDecimal(current.Reading),
                Measurement = "Amps"
            });


            var voltage = sensors.Single(s =>
                s.ProbeName == "Voltage" && s.Measurement != Measurement.None);

            power.Add(new HomeAssistant.PowerType
            {
                Name = "Voltage",
                Reading = Convert.ToDecimal(voltage.Reading),
                Measurement = "Volts"
            });


            var system = sensors.Single(s => s.ProbeName == "System Level");

            power.Add(new HomeAssistant.PowerType
            {
                Name = "Watts",
                Reading = Convert.ToDecimal(system.Reading),
                Measurement = "Watts"
            });

            return new HomeAssistant()
            {
                Fans = new HomeAssistant.FanType
                {
                    FansList = sensors.Where(s => s.ProbeName.Contains("FAN"))
                        .Select(s => new HomeAssistant.Fan
                        {
                            Reading = Convert.ToInt32(Convert.ToDecimal(s.Reading)),
                            Measurement = Enum.GetName(s.Measurement)
                        })
                        .ToList(),
                    FansModeAverage = new HomeAssistant.Fan
                    {
                        Reading = sensors
                        .Where(s => s.ProbeName.Contains("FAN"))
                        .GroupBy(n => Convert.ToInt32(Convert.ToDecimal(n.Reading)))
                        .OrderByDescending(g => g.Count())
                        .Select(g => g.Key)
                        .FirstOrDefault(),
                        Measurement = "RPM"
                    }
                },
                Power = power,
                IpmiLastUpdated = _ipmiService.LastUpdated,
                IpmiLatency = _ipmiService.Latency,
            };
        }
    }
}
