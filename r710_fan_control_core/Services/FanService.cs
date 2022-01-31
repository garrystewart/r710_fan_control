using r710_fan_control_core.Models;
using System.Collections.Generic;
using System.Linq;

namespace r710_fan_control_core.Services
{
    public static class FanService
    {
        private static readonly string _rawArgument = IPMIService.rawArgument;
        public static void SwitchToAutomatic() => IPMIService.Command($"{_rawArgument} 0x30 0x30 0x01 0x01");

        public static void SwitchToManual(int speedPercent)
        {
            IPMIService.Command($"{_rawArgument} 0x30 0x30 0x01 0x00");
            IPMIService.Command($"{_rawArgument} 0x30 0x30 0x02 0xff 0x{speedPercent:x}");
        }

        public static IEnumerable<Sensor> GetFanSensors()
        {
            IEnumerable<Sensor> sensors = IPMIService.GetSensors();
            return sensors.Where(s => s.Measurement == Measurement.RPM);
        }
    }
}
