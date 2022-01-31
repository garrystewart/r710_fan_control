using System.Collections.Generic;
using System.Diagnostics;
using r710_fan_control_core.Models;

namespace r710_fan_control_core.Services
{
    public static class IPMIService
    {
        private const string _hostname = "192.168.18.61";
        private const string _user = "root";
        private const string _password = "calvin";

        private static readonly string _baseArguments = $@"C:\ipmitool_1.8.18-dellemc_p001\ipmitool -I lanplus -H {_hostname} -U {_user} -P {_password}";

        public static readonly string rawArgument = $"{_baseArguments} raw";
        private static readonly string _sensorList = $"{_baseArguments} sensor list";

        public static string Command(string arguments)
        {
            Process process = new();

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
            return ConvertSensorOutputToModel(GetSensorList());
        }

        private static string GetSensorList()
        {
            Stopwatch stopwatch = new();
            Debug.WriteLine("Getting Sensor List...");
            stopwatch.Start();
            var sensorsList = Command(_sensorList);
            stopwatch.Stop();
            Debug.WriteLine($"Finished getting sensor list. Took {stopwatch.ElapsedMilliseconds}ms");
            return sensorsList;
        }

        private static IEnumerable<Sensor> ConvertSensorOutputToModel(string output)
        {
            ICollection<Sensor> sensors = new List<Sensor>();

            string[] lines = output.Split('\n');

            for (int i = 0; i < lines.Length - 1; i++)
            {
                string[] items = lines[i].Split('|');

                Sensor sensor = new();

                sensor.ProbeName = items[0].Trim();
                sensor.Reading = items[1].Trim();
                sensor.SetMeasurement(items[2].Trim());
                sensor.Status = items[3].Trim();

                ushort.TryParse(items[5].Trim(), out ushort warningMin);
                ushort.TryParse(items[6].Trim(), out ushort warningMax);
                ushort.TryParse(items[7].Trim(), out ushort failureMin);
                ushort.TryParse(items[8].Trim(), out ushort failureMax);

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
    }
}
