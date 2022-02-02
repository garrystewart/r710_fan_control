using Microsoft.AspNetCore.Mvc;
using r710_fan_control_core.Models;
using r710_fan_control_core.Services;
using System.Collections.Generic;

namespace r710_fan_control_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        // GET: api/<SensorsController>
        [HttpGet]
        public IEnumerable<Sensor> Get()
        {
            return IPMIService.GetSensors();
        }
    }
}
