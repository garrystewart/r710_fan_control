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
        private readonly decimal _cutoff = 80;
        private readonly int _stepDownTime = 30;

        public void Auto()
        {
            FanService.SwitchToAutomatic();
            _autoLowRunning = false;
        }

        public async Task AutoLow()
        {
            if (!_autoLowRunning)
            {
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
                            System.Diagnostics.Debug.WriteLine($"maxTemp: {maxTemp} | FanCurve: {FanCurve.GetFanSpeed(maxTemp)}");
                            FanService.SwitchToManual(FanCurve.GetFanSpeed(maxTemp));
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

        public static class FanCurve
        {
            public static IEnumerable<Entry> ReferencePoints => new List<Entry>() {
                new Entry { Temperature = 40, FanSpeed = 0 },
                new Entry { Temperature = 60, FanSpeed = 22 },
                new Entry { Temperature = 80, FanSpeed = 100 }
            };

            public class Entry
            {
                public decimal Temperature { get; set; }
                public int FanSpeed { get; set; }
            }

            public static int GetFanSpeed(decimal temperature)
            {
                if (temperature < ReferencePoints.First().Temperature)
                {
                    var fanSpeedUnit = ReferencePoints.First().FanSpeed / ReferencePoints.First().Temperature;

                    return Convert.ToInt32(Math.Floor(temperature * fanSpeedUnit));
                }
                else if (temperature > ReferencePoints.Last().Temperature)
                {
                    var fanSpeedUnit = ReferencePoints.Last().FanSpeed / (100 - ReferencePoints.Last().Temperature);

                    return Convert.ToInt32(Math.Floor(temperature * fanSpeedUnit));
                }
                else
                {
                    for (int i = 0; i < ReferencePoints.Count(); i++)
                    {
                        if (temperature >= ReferencePoints.ElementAt(i).Temperature && temperature < ReferencePoints.ElementAt(i + 1).Temperature)
                        {
                            var temperatureDifference = ReferencePoints.ElementAt(i + 1).Temperature - ReferencePoints.ElementAt(i).Temperature;
                            var fanSpeedDifference = ReferencePoints.ElementAt(i + 1).FanSpeed - ReferencePoints.ElementAt(i).FanSpeed;

                            var fanSpeedUnit = fanSpeedDifference / temperatureDifference;

                            return Convert.ToInt32(Math.Floor(((temperature - ReferencePoints.ElementAt(i).Temperature) * fanSpeedUnit) + ReferencePoints.ElementAt(i).FanSpeed));
                            
                        }
                    }

                    throw new Exception();
                }
            }
        }
    }
}
