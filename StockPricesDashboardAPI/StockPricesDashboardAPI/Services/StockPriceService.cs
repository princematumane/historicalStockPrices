using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using StockPricesDashboardAPI.Domain.Model;
using StockPricesDashboardAPI.Exceptions;
using System.Globalization;
using System.Net;

namespace StockPricesDashboardAPI.Services
{
    public class StockPriceService
    {
        private readonly HttpClient _httpClient;
        private readonly RedisService _redisService;
        private readonly AlphaVantageConfig _alphaVantageConfig;

        private static int _apiCallCount = 0;
        private static DateTime _lastResetTime = DateTime.UtcNow;

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
            if (string.IsNullOrEmpty(symbol))
            {
                throw new ArgumentNullException(nameof(symbol), "Stock symbol cannot be null or empty.");
            }

            string cacheKey = $"stock:{symbol}";
            //var cachedData = await _redisService.GetAsync<Stock>(cacheKey);
            //if (cachedData != null)
            //{
            //    return cachedData;
            //}

            if (_apiCallCount >= 5 && (DateTime.UtcNow - _lastResetTime).TotalMinutes < 1)
            {
                throw new ApiRateLimitExceededException();
            }

            _apiCallCount++;

            if ((DateTime.UtcNow - _lastResetTime).TotalMinutes >= 1)
            {
                _apiCallCount = 0;
                _lastResetTime = DateTime.UtcNow;
            }

                var url = $"query?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=compact&datatype=json";
                var response = await _httpClient.GetAsync(url);

                if (response.StatusCode == (HttpStatusCode)429)
                {
                    throw new ApiRateLimitExceededException();
                }

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

                await _redisService.SetAsync(cacheKey, stock, TimeSpan.FromMinutes(10));

                return stock;

        }
    }
}
