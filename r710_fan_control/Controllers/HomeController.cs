using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using r710_fan_control.Models;

namespace r710_fan_control.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient _client = new HttpClient();
        private const string _url = "http://192.168.18.45:8085/data.json";
        private const string _ipmiToolPath = @"C:\ipmitool_1.8.18-dellemc_p001\";
        private const string _hostname = "192.168.18.61";
        private const string _user = "root";
        private const string _password = "calvin";
        private static readonly string _baseCommand = $"{_ipmiToolPath}ipmitool -I lanplus -H {_hostname} -U {_user} -P {_password}";
        private static readonly string _raw = $"{_baseCommand} raw";

        public async Task<ActionResult> Index()
        {
            //string json = await _client.GetStringAsync(_url);

            //Data data = Data.FromJson(json);

            //Stats stats = new Stats();

            //ICollection<string> temperatureList = new List<string>();

            //IEnumerable<Data> processors = data.Children.Where(c => c.Text == "WIN-USMAMP0H8B8").Single().Children
            //    .Where(c => c.Text == "Intel Xeon L5640").ToList();

            //foreach (var processor in processors)
            //{
            //    var processorToAdd = new Stats.Processor { Name  =processor.Text };

            //    foreach (var temperature in processors
            //        .SelectMany(p => p.Children
            //        .Where(c => c.Text == "Temperatures")
            //        .SelectMany(t => t.Children)))
            //    {
            //        processorToAdd.Cores.Add(temperature.Value);
            //    }

            //    stats.Processors.Add(processorToAdd);
            //}

            //foreach (var temperature in processors
            //    .SelectMany(processor => processor.Children
            //    .Where(c => c.Text == "Temperatures")
            //    .SelectMany(t => t.Children)))
            //{
            //    temperatureList.Add(temperature.Value);
            //}

            return View(GetHighestTemperature(await GetTemperatures()));
        }

        public decimal GetHighestTemperature(IEnumerable<decimal> temperatures)
        {
            return temperatures.Max();
        }

        public async Task<IEnumerable<decimal>> GetTemperatures()
        {
            string json = await _client.GetStringAsync(_url);

            Data data = Data.FromJson(json);

            ICollection<decimal> temperatures = new List<decimal>();

            IEnumerable<Data> processors = data.Children.Where(c => c.Text == "WIN-USMAMP0H8B8").Single().Children
                .Where(c => c.Text == "Intel Xeon L5640").ToList();

            foreach (var temperature in processors
                .SelectMany(processor => processor.Children
                .Where(c => c.Text == "Temperatures")
                .SelectMany(t => t.Children)))
            {
                temperatures.Add(decimal.Parse(temperature.Value.Replace(" °C", "")));
            }

            return temperatures;
        }

        [HttpGet]
        public ActionResult Fan()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Fan(Fan model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            string hexSpeed = int.Parse(model.Speed).ToString("x");

            if (MvcApplication.State != State.Manual)
            {
                Command($"{_raw} 0x30 0x30 0x01 0x00");
                MvcApplication.State = State.Manual;
            }

            Command($"{_raw} 0x30 0x30 0x02 0xff 0x{hexSpeed}");

            return View();
        }

        [HttpPost]
        public ActionResult FanAuto()
        {
            if (MvcApplication.State != State.Auto)
            {
                Command($"{_raw} 0x30 0x30 0x01 0x01");
                MvcApplication.State = State.Auto;
            }

            return RedirectToAction("Fan");
        }

        private static void Command(string command)
        {
            Process.Start("cmd.exe", $"/C {command}");
        }
    }

    public enum State
    {
        None,
        Auto,
        Manual
    }

    public partial class Data
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("Text")]
        public string Text { get; set; }

        [JsonProperty("Children")]
        public List<Data> Children { get; set; }

        [JsonProperty("Min")]
        public string Min { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Max")]
        public string Max { get; set; }

        [JsonProperty("ImageURL")]
        public string ImageUrl { get; set; }
    }

    public partial class Data
    {
        public static Data FromJson(string json) => JsonConvert.DeserializeObject<Data>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Data self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}