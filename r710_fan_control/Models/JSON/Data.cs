using System.Collections.Generic;
using Newtonsoft.Json;
using r710_fan_control.Classes;

namespace r710_fan_control.Models
{
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
}