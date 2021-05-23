using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using r710_fan_control.Services;
using r710_fan_control.ViewModels;

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

        public ActionResult Index()
        {
            //HomeIndexVM model = new HomeIndexVM
            //{
            //    Temperatures = await TemperatureService.GetTemperatures(),
            //    FanSpeeds = FanService.GetFanSensors()
            //};

            return View();
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