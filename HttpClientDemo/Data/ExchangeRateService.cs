using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HttpClientDemo.Models;

namespace HttpClientDemo.Data
{
    public class ExchangeRateService
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ExchangeRate>> GetExchangeRatesAsync()
        {
            var response = await _httpClient.GetStringAsync("https://api.cnb.cz//cnbapi/exrates/daily");
            var data = JsonSerializer.Deserialize<ExchangeRateResponse>(response);
            var rates = data?.Rates;

            if (rates != null)
            {
                foreach (var rate in rates)
                {
                    if (rate.Amount != 1)
                    {
                        rate.Rate /= rate.Amount;
                        rate.Amount = 1;
                    }
                }
            }

            return rates;
        }
    }

    public class ExchangeRateResponse
    {
        [JsonPropertyName("rates")]
        public List<ExchangeRate> Rates { get; set; }
    }
}