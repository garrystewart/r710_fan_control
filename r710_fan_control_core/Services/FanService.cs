using r710_fan_control_core.Models;

namespace r710_fan_control_core.Services
{
    public static class FanService
    {
        private static readonly string _rawArgument = IPMIService.rawArgument;
        private static void SwitchToManual() => IPMIService.Command($"{_rawArgument} 0x30 0x30 0x01 0x00");
        public static void SwitchToAutomatic() => IPMIService.Command($"{_rawArgument} 0x30 0x30 0x01 0x01");

        public static void SwitchToManual(string speedPercent)
        {
            SwitchToManual();
            IPMIService.Command($"{_rawArgument} 0x30 0x30 0x02 0xff 0x{ConvertSpeedToHex(speedPercent)}");
        }

        public static void SwitchToManual(int speedPercent)
        {
            SwitchToManual();
            IPMIService.Command($"{_rawArgument} 0x30 0x30 0x02 0xff 0x{ConvertSpeedToHex(speedPercent)}");
        }

        private static string ConvertSpeedToHex(string speed) => int.Parse(speed).ToString("x");
        private static string ConvertSpeedToHex(int speed) => speed.ToString("x");

        public static IEnumerable<Sensor> GetFanSensors()
        {
            IEnumerable<Sensor> sensors = IPMIService.GetSensors();
            return sensors.Where(s => s.Measurement == Measurement.RPM);
        }
    }
}
