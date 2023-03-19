using System.Text.Json.Serialization;

namespace EquitiesApi.Services.Private
{
    public class ReturnDTO
    {
        [JsonPropertyName("priceDate")]
        public string Date { get; set; }

        //using fclose and fopen from IEX as they are fully adjusted for historical dates
        [JsonPropertyName("fclose")]
        public double Close { get; set; }

        [JsonPropertyName("fopen")]
        public double Open { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

    }
}
