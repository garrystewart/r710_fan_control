using r710_fan_control_core.Models;
using System.Collections.Generic;
using System.Linq;

namespace r710_fan_control_core.Services
{
    public class FanService
    {
        private readonly IpmiService _ipmiService;

        public FanService(IpmiService ipmiService)
        {
            _ipmiService = ipmiService;
        }

        public void SwitchToAutomatic() => _ipmiService.Command($"{_ipmiService._rawArgument} 0x30 0x30 0x01 0x01");

        public void SwitchToManual(int speedPercent)
        {
            _ipmiService.Command($"{_ipmiService._rawArgument} 0x30 0x30 0x01 0x00");
            _ipmiService.Command($"{_ipmiService._rawArgument} 0x30 0x30 0x02 0xff 0x{speedPercent:x}");
        }

        public IEnumerable<Sensor> GetFanSensors()
        {
            return _ipmiService.Sensors.Where(s => s.Measurement == Measurement.RPM);
        }
    }
}
