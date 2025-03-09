namespace StockPricesDashboardAPI.Exceptions
{
    public class ApiRateLimitExceededException : Exception
    {
        public ApiRateLimitExceededException() : base("API rate limit exceeded. Please try again later.") { }
    }
}
