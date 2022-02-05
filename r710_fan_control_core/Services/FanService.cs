using r710_fan_control_core.Models;
using System.Collections.Generic;
using System.Linq;

namespace r710_fan_control_core.Services
{
    public class FanService : IFanService
    {
        private readonly IIpmiService _ipmiService;

        public FanService(IIpmiService ipmiService)
        {
            _ipmiService = ipmiService;
        }

        public void SwitchToAutomatic() => _ipmiService.Command($"{_ipmiService.RawArgument} 0x30 0x30 0x01 0x01");

        public void SwitchToManual(int speedPercent)
        {
            _ipmiService.Command($"{_ipmiService.RawArgument} 0x30 0x30 0x01 0x00");
            _ipmiService.Command($"{_ipmiService.RawArgument} 0x30 0x30 0x02 0xff 0x{speedPercent:x}");
        }

        public IEnumerable<IpmiSensor> GetFanSensors()
        {
            return _ipmiService.Sensors.Where(s => s.Measurement == Measurement.RPM);
        }
    }
}
