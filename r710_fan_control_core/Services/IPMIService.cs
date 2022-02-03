using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using r710_fan_control_core.Models;

namespace r710_fan_control_core.Services
{
    public class IpmiService
    {
        private const string _hostname = "192.168.18.61";
        private const string _user = "root";
        private const string _password = "calvin";

        private readonly string _baseArguments;

        public readonly string _rawArgument;
        private readonly string _sensorList;

        public IpmiService()
        {
            _baseArguments = $@"C:\ipmitool_1.8.18-dellemc_p001\ipmitool -I lanplus -H {_hostname} -U {_user} -P {_password}";

            _rawArgument = $"{_baseArguments} raw";
            _sensorList = $"{_baseArguments} sensor list";

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += GetSensors;
            backgroundWorker.RunWorkerAsync();
        }

        public IEnumerable<Sensor> Sensors { get; set; }
        public long Latency { get; set; }
        public DateTime LastUpdated { get; set; }

        public string Command(string arguments)
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

        private void GetSensors(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Sensors = ConvertSensorOutputToModel(GetSensorList());
            }             
        }

        private string GetSensorList()
        {
            Stopwatch stopwatch = new();

            stopwatch.Start();
            var sensorsList = Command(_sensorList);
            stopwatch.Stop();

            Latency = stopwatch.ElapsedMilliseconds;
            LastUpdated = DateTime.Now;

            return sensorsList;
        }

        private IEnumerable<Sensor> ConvertSensorOutputToModel(string output)
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
