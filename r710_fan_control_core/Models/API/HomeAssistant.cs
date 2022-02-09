using System;
using System.Collections.Generic;

namespace r710_fan_control_core.Models.API
{
    public class HomeAssistant
    {
        public IpmiType Ipmi { get; set; }
        public OpenHardwareMonitorType OpenHardwareMonitor { get; set; }  

        public class IpmiType
        {
            public class FanType
            {
                public IEnumerable<int> Readings { get; set; }
                public int ModeAverage { get; set; }
            }

            public class PowerType
            {
                public decimal Current { get; set; }
                public decimal Voltage { get; set; }
                public decimal Watts { get; set; }
            }

            public FanType Fans { get; set; }
            public PowerType Power { get; set; }
            public DateTime LastUpdated { get; set; }
            public long Latency { get; set; }
        }

        public class OpenHardwareMonitorType
        {
            public OpenHardwareMonitorType()
            {
                Cores = new HashSet<Core>();
            }

            public ICollection<Core> Cores { get; set; }
            public MemoryType Memory { get; set; }
            //public SolidStateDriveType SolidStateDrive { get; set; }
            public DateTime LastUpdated { get; set; }
            public long Latency { get; set; }

            public class Core
            {
                public decimal Clock { get; set; }
                public int Temperature { get; set; }
                public decimal Load { get; set; }
            }

            public class MemoryType
            {
                public decimal Load { get; set; }
                public decimal UsedMemory { get; set; }
                public decimal AvailableMemory { get; set; }
            }

            public class SolidStateDriveType
            {
                public int Temperature { get; set; }
                public decimal UsedSpace { get; set; }
                public decimal RemainingLife { get; set; }
                public decimal WriteAmplification { get; set; }
                public decimal TotalBytesWritten { get; set; }
            }
        }
    }
}
