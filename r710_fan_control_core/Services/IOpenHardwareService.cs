using r710_fan_control_core.Models;
using System;
using System.ComponentModel;

namespace r710_fan_control_core.Services
{
    public interface IOpenHardwareService
    {
        DateTime LastUpdated { get; set; }
        long Latency { get; set; }
        OpenHardwareMonitorSensors Sensors { get; set; }

        void GetSensors(object sender, DoWorkEventArgs e);
    }
}