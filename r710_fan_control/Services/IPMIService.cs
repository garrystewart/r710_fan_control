using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace r710_fan_control.Services
{
    public static class IPMIService
    {
        private const string _hostname = "192.168.18.61";
        private const string _user = "root";
        private const string _password = "calvin";

        private static readonly string _baseArguments = $@"C:\ipmitool_1.8.18-dellemc_p001\ipmitool -I lanplus -H {_hostname} -U {_user} -P {_password}";

        public static readonly string rawArgument = $"{_baseArguments} raw";
        public static readonly string sensorList = $"{_baseArguments} sensor list";

        public static string Command(string arguments)
        {
            Process process = new Process();

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/c {arguments}";
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }

        public static IEnumerable<Sensor> GetSensors()
        {
            Debug.WriteLine($"[{DateTime.Now}] Getting sensors...");

            ICollection<Sensor> sensors = new List<Sensor>();

            string output = Command(sensorList);

            Debug.WriteLine($"[{DateTime.Now}] Got sensors");

            string[] lines = output.Split('\n');

            for (int i = 0; i < lines.Length - 1; i++)
            {
                string[] items = lines[i].Split('|');

                Sensor sensor = new Sensor();

                sensor.ProbeName = items[0].Trim();
                sensor.Reading = items[1].Trim();
                sensor.Measurement = GetMeasurement(items[2].Trim());
                sensor.Status = items[3].Trim();

                ushort warningMin;
                ushort warningMax;
                ushort failureMin;
                ushort failureMax;

                ushort.TryParse(items[5].Trim(), out warningMin);
                ushort.TryParse(items[6].Trim(), out warningMax);
                ushort.TryParse(items[7].Trim(), out failureMin);
                ushort.TryParse(items[8].Trim(), out failureMax);

                sensor.Thresholds = new Thresholds
                {
                    Warning = new Warning
                    {
                        Min = warningMin,
                        Max = warningMax
                    },
                    Failure = new Failure
                    {
                        Min = failureMin,
                        Max = failureMax
                    }
                };

                sensors.Add(sensor);
            }

            return sensors;
        }

        public class Sensor
        {
            public string ProbeName { get; set; }
            public string Reading { get; set; }
            public Measurement Measurement { get; set; }
            public string Status { get; set; }
            public Thresholds Thresholds { get; set; }

        }
        public class Thresholds
        {
            public Warning Warning { get; set; }
            public Failure Failure { get; set; }
        }
        public class Warning
        {
            public ushort? Min { get; set; }
            public ushort? Max { get; set; }
        }
        public class Failure
        {
            public ushort? Min { get; set; }
            public ushort? Max { get; set; }
        }

        private static Measurement GetMeasurement(string measurement)
        {
            switch (measurement)
            {
                case "Amps":
                    return Measurement.Amps;
                case "degrees C":
                    return Measurement.DegreesC;
                case "discrete":
                    return Measurement.Discrete;
                case "RPM":
                    return Measurement.RPM;
                case "Volts":
                    return Measurement.Volts;
                case "Watts":
                    return Measurement.Watts;
                default:
                    return Measurement.None;
            }
        }

        public enum Measurement
        {
            None,
            Amps,
            DegreesC,
            Discrete,
            RPM,
            Volts,
            Watts
        }
    }
}