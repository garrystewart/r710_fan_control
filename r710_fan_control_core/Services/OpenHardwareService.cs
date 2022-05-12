using Microsoft.Extensions.Configuration;
using r710_fan_control_core.Models;
using r710_fan_control_core.Models.JSON;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace r710_fan_control_core.Services
{
    public class OpenHardwareService : IOpenHardwareService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _url;

        public OpenHardwareService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _url = $"http://{_configuration.GetValue<string>("OpenHardwareMonitor:Hostname")}:{_configuration.GetValue<string>("OpenHardwareMonitor:Port")}/data.json";

            BackgroundWorker backgroundWorker = new();
            backgroundWorker.DoWork += GetSensors;
            backgroundWorker.RunWorkerAsync();
        }

        public OpenHardwareMonitorSensors Sensors { get; set; }
        public long Latency { get; set; }
        public DateTime LastUpdated { get; set; }

        public void GetSensors(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Stopwatch stopwatch = new();

                stopwatch.Start();

                var data = Data.FromJson(_httpClient.GetStringAsync(_url).Result);

                var processorsList = new List<OpenHardwareMonitorSensors.Processor>();
                var processors = data.Children.First().Children.Where(c => c.Text == "Intel Xeon L5640");

                foreach (var processor in processors)
                {
                    var coreList = new List<OpenHardwareMonitorSensors.Processor.Core>();

                    var coreClocks = processor.Children
                        .Where(c => c.Text == "Clocks")
                        .SelectMany(t => t.Children)
                        .Reverse()
                        .ToList();

                    var coreTemperatures = processor.Children
                        .Where(c => c.Text == "Temperatures")
                        .SelectMany(t => t.Children);

                    var coreLoad = processor.Children
                        .Where(c => c.Text == "Load")
                        .Reverse()
                        .SelectMany(t => t.Children);

                    for (int i = 0; i < coreTemperatures.Count(); i++)
                    {
                        coreList.Add(new OpenHardwareMonitorSensors.Processor.Core
                        {
                            Clock = Convert.ToDecimal(coreClocks.ElementAt(i).Value.Replace(" MHz", "")),
                            Temperature = Convert.ToInt32(Convert.ToDecimal(coreTemperatures.ElementAt(i).Value.Replace(" °C", ""))),
                            Load = Convert.ToDecimal(coreLoad.ElementAt(i).Value.Replace(" %", ""))
                        });
                    }

                    processorsList.Add(new OpenHardwareMonitorSensors.Processor
                    {
                        Cores = coreList
                    });
                }

                Sensors = new OpenHardwareMonitorSensors
                {
                    Memory = new OpenHardwareMonitorSensors.MemoryType
                    {
                        Load = Convert.ToDecimal(data.Children.First().Children
                            .Single(c => c.Text == "Generic Memory").Children
                            .Single(c => c.Text == "Load").Children
                            .Single(c => c.Text == "Memory").Value.Replace(" %", "")),
                        UsedMemory = Convert.ToDecimal(data.Children.First().Children
                            .Single(c => c.Text == "Generic Memory").Children
                            .Single(c => c.Text == "Data").Children
                            .Single(c => c.Text == "Used Memory").Value.Replace(" GB", "")),
                        AvailableMemory = Convert.ToDecimal(data.Children.First().Children
                            .Single(c => c.Text == "Generic Memory").Children
                            .Single(c => c.Text == "Data").Children
                            .Single(c => c.Text == "Available Memory").Value.Replace(" GB", ""))
                    },

                    SolidStateDrive = new OpenHardwareMonitorSensors.SolidStateDriveType
                    {
                        Temperature = Convert.ToInt32(Convert.ToDecimal(data.Children.First().Children
                            .Single(c => c.Text == "CT500MX500SSD1").Children
                            .Single(c => c.Text == "Temperatures").Children
                            .Single(c => c.Text == "Temperature").Value.Replace(" °C", ""))),
                        UsedSpace = Convert.ToDecimal(data.Children.First().Children
                            .Single(c => c.Text == "CT500MX500SSD1").Children
                            .Single(c => c.Text == "Load").Children
                            .Single(c => c.Text == "Used Space").Value.Replace(" %", "")),
                        RemainingLife = Convert.ToDecimal(data.Children.First().Children
                            .Single(c => c.Text == "CT500MX500SSD1").Children
                            .Single(c => c.Text == "Levels").Children
                            .Single(c => c.Text == "Remaining Life").Value.Replace(" %", "")),
                        WriteAmplification = Convert.ToDecimal(data.Children.First().Children
                            .Single(c => c.Text == "CT500MX500SSD1").Children
                            .Single(c => c.Text == "Factors").Children
                            .Single(c => c.Text == "Write Amplification").Value),
                        TotalBytesWritten = Convert.ToDecimal(data.Children.First().Children
                            .Single(c => c.Text == "CT500MX500SSD1").Children
                            .Single(c => c.Text == "Data").Children
                            .Single(c => c.Text == "Total Bytes Written").Value.Replace(" GB", ""))
                    },

                    Processors = processorsList
                };

                stopwatch.Stop();

                Latency = stopwatch.ElapsedMilliseconds;
                LastUpdated = DateTime.Now;

                Thread.Sleep(1000);
            }
        }
    }
}
