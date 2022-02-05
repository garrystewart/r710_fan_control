using r710_fan_control_core.Models;
using System.Collections.Generic;

namespace r710_fan_control_core.Services
{
    public interface IFanService
    {
        IEnumerable<IpmiSensor> GetFanSensors();
        void SwitchToAutomatic();
        void SwitchToManual(int speedPercent);
    }
}