using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using StockPricesDashboardAPI.Domain.Model;
using System.Globalization;

namespace StockPricesDashboardAPI.Services
{
    public class StockPriceService
    {
        private readonly HttpClient _httpClient;
        private readonly RedisService _redisService;
        private readonly AlphaVantageConfig _alphaVantageConfig;

        public StockPriceService(RedisService redisService, HttpClient httpClient,
            IOptions<AlphaVantageConfig> alphaVantageConfig)
        {
            _httpClient = httpClient;
            _redisService = redisService;
            _alphaVantageConfig = alphaVantageConfig.Value;

            _httpClient.BaseAddress = new Uri(_alphaVantageConfig.BaseUrl);
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", _alphaVantageConfig.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", _alphaVantageConfig.Host);
        }

        public async Task<Stock> GetStockPricesAsync(string symbol)
        {
            string cacheKey = $"stock:{symbol}";
            //var cachedData = await _redisService.GetAsync<Stock>(cacheKey);
            //if (cachedData != null)
            //{
            //    return cachedData;
            //}

            var url = $"query?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=compact&datatype=json";
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<AlphaVantageResponse>(content);

            var stock = new Stock
            {
                Symbol = symbol,
                Prices = data.TimeSeries.Select(ts => new StockPrice
                {
                    Symbol = symbol,
                    Date = DateTime.Parse(ts.Key),
                    Open = decimal.Parse(ts.Value.Open, CultureInfo.InvariantCulture),
                    High = decimal.Parse(ts.Value.High, CultureInfo.InvariantCulture),
                    Low = decimal.Parse(ts.Value.Low, CultureInfo.InvariantCulture),
                    Close = decimal.Parse(ts.Value.Close, CultureInfo.InvariantCulture),
                    Volume = long.Parse(ts.Value.Volume)

                }).ToList()
            };

            // await _redisService.SetAsync(cacheKey, stock, TimeSpan.FromMinutes(10));

            return stock;
        }
    }
}
