using System.Collections.Generic;
using System.Linq;
using static r710_fan_control.Services.IPMIService;

namespace r710_fan_control.Services
{
    public static class FanService
    {
        private static readonly string _rawArgument = IPMIService.rawArgument;
        private static void SwitchToManual()
        {
            IPMIService.Command($"{_rawArgument} 0x30 0x30 0x01 0x00");
        }

        public static void SwitchToAutomatic()
        {
            IPMIService.Command($"{_rawArgument} 0x30 0x30 0x01 0x01");
        }

        public static void SwitchToManual(string speed)
        {
            SwitchToManual();
            IPMIService.Command($"{_rawArgument} 0x30 0x30 0x02 0xff 0x{ConvertSpeedToHex(speed)}");
        }

        private static string ConvertSpeedToHex(string speed)
        {
            return int.Parse(speed).ToString("x");
        }

        public static IEnumerable<Sensor> GetFanSensors()
        {
            IEnumerable<Sensor> sensors = IPMIService.GetSensors();
            return sensors.Where(s => s.Measurement == IPMIService.Measurement.RPM);
        }

        public class Fan
        {
            public string Display { get; set; }
            public ushort Value { get; set; }
        }
    }
}