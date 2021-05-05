using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace r710_fan_control.Models
{
    public class Stats
    {
        public ICollection<Processor> Processors { get; set; }
        public class Processor
        {
            public string Name { get; set; }
            public ICollection<Core> Cores { get; set; }
            public class Core
            {
                public decimal Clock { get; set; }
                public decimal Temperature { get; set; }
                public decimal Load { get; set; }
            }
        }
    }
}