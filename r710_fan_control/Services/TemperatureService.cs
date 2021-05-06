using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using r710_fan_control.Models;

namespace r710_fan_control.Services
{
    public static class TemperatureService
    {
        private static readonly HttpClient _client = new HttpClient();
        private const string _url = "http://192.168.18.45:8085/data.json";

        public static async Task<IEnumerable<Temperature>> GetTemperatures()
        {
            string json = await _client.GetStringAsync(_url);

            Data data = Data.FromJson(json);

            ICollection<Temperature> temperatures = new List<Temperature>();

            IEnumerable<Data> processors = data.Children.Where(c => c.Text == "WIN-USMAMP0H8B8").Single().Children
                .Where(c => c.Text == "Intel Xeon L5640").ToList();

            foreach (var temperature in processors
                .SelectMany(processor => processor.Children
                .Where(c => c.Text == "Temperatures")
                .SelectMany(t => t.Children)))
            {
                temperatures.Add(new Temperature
                {
                    Display = temperature.Value,
                    Value = decimal.Parse(temperature.Value.Replace(" °C", ""))
                });
            }

            return temperatures;
        }

        public class Temperature
        {
            public decimal Value { get; set; }
            public string Display { get; set; }
        }
    }
}