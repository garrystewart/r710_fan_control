using r710_fan_control_core.Models;
using System;
using System.Collections.Generic;

namespace r710_fan_control_core.Services
{
    public interface IIpmiService
    {
        DateTime LastUpdated { get; set; }
        long Latency { get; set; }
        string RawArgument { get; }
        IEnumerable<IpmiSensor> Sensors { get; set; }

        string Command(string arguments);
    }
}