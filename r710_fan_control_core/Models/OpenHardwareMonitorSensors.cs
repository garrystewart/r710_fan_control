using System.Collections;
using System.Collections.Generic;

namespace r710_fan_control_core.Models
{
    public class OpenHardwareMonitorSensors
    {
        public OpenHardwareMonitorSensors()
        {
            Processors = new HashSet<Processor>();
        }

        public ICollection<Processor> Processors { get; set; }
        public MemoryType Memory { get; set; }
        public SolidStateDriveType SolidStateDrive { get; set; }


        public class Processor
        {
            public class Core
            {
                public decimal Clock { get; set; }
                public int Temperature { get; set; }
                public decimal Load { get; set; }
            }

            public Processor()
            {
                Cores = new HashSet<Core>();
            }

            public ICollection<Core> Cores { get; set; }
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
