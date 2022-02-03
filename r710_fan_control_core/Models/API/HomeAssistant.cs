using System.Collections.Generic;

namespace r710_fan_control_core.Models.API
{
    public class HomeAssistant
    {
        public IEnumerable<Fan> Fans { get; set; }
        public IEnumerable<PowerType> Power { get; set; }

        public class Fan
        {
            public int Reading { get; set; }
            public string Measurement { get; set; }
        }

        public class PowerType
        {
            public string Name { get; set; }
            public decimal Reading { get; set; }
            public string Measurement { get; set; }
        }
    }
}
