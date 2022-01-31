using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace r710_fan_control_core.Services
{
    public class ModeService
    {
        private bool _autoLowRunning;
        private readonly decimal _cutoff = 45;

        //private readonly decimal _curve1 = 0;
        //private readonly decimal _curve2 = 20;
        //private readonly decimal _curve3 = 40;
        //private readonly decimal _curve4 = 60;

        public void Auto()
        {
            FanService.SwitchToAutomatic();
            _autoLowRunning = false;
        }

        public async Task AutoLow(int speedPercent)
        {
            if (!_autoLowRunning)
            {
                FanService.SwitchToManual(speedPercent);
                _autoLowRunning = true;

                while (true)
                {
                    if (!_autoLowRunning) break;

                    decimal maxTemp = 0;

                    try
                    {
                        maxTemp = (await TemperatureService.GetTemperatures()).Max(t => t.Value);

                        if (maxTemp > _cutoff)
                        {
                            FanService.SwitchToAutomatic();
                        }
                        else
                        {
                            FanService.SwitchToManual(speedPercent);
                        }
                    }
                    catch (Exception)
                    {
                        FanService.SwitchToAutomatic();
                    }

                    Thread.Sleep(10000);
                }
            }
        }

        //public class FanCurve
        //{
        //    public ICollection<Entry> Entries { get; set; }

        //    public IEnumerable<Entry> BaseEntries => new List<Entry>() { 
        //        new Entry { Temperature = 30, FanSpeed = 22 },
        //        new Entry { Temperature = 40, FanSpeed = 33 },
        //        new Entry { Temperature = 50, FanSpeed = 44 },
        //        new Entry { Temperature = 60, FanSpeed = 55 }
        //    };

        //    public class Entry
        //    {
        //        public decimal Temperature { get; set; }
        //        public int FanSpeed { get; set; }
        //    }

        //    public int GetFanSpeed(decimal temperature)
        //    {
        //        var startingTemp = 0;
        //        var startingFanSpeed = 0;

        //        foreach (var baseEntry in BaseEntries)
        //        {
        //            var fanSpeedDifference = baseEntry.FanSpeed - startingFanSpeed;

        //            for (int i = startingTemp; i < baseEntry.Temperature; i++)
        //            {

        //            }
        //        }
        //    }
        //}
    }
}
