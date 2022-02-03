using Microsoft.AspNetCore.Mvc;
using r710_fan_control_core.Models;
using r710_fan_control_core.Models.API;
using r710_fan_control_core.Services;
using System.Collections.Generic;
using System.Linq;

namespace r710_fan_control_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        [Route("fans")]
        [HttpGet]
        public Fans Get()
        {
            var sensors = IPMIService.GetSensors();

            foreach (var sensor in sensors.Where(s => s.Reading != "na" && s.Reading != "0x0"))
            {
                System.Diagnostics.Debug.WriteLine($"sensor: {sensor.ProbeName} reading: {sensor.Reading} measurement: {sensor.Measurement}");
            }

            var model = new Fans
            {
                Fan1 = sensors.Single(s => s.ProbeName == "FAN 1 RPM").Reading,
                Fan2 = sensors.Single(s => s.ProbeName == "FAN 2 RPM").Reading,
                Fan3 = sensors.Single(s => s.ProbeName == "FAN 3 RPM").Reading,
                Fan4 = sensors.Single(s => s.ProbeName == "FAN 4 RPM").Reading,
                Fan5 = sensors.Single(s => s.ProbeName == "FAN 5 RPM").Reading
            };

            return model;
        }
    }
}
