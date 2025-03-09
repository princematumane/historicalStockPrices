namespace StockPricesDashboardAPI.Domain.Model
{
    public class Stock
    {
        public string Symbol { get; set; }
        public List<StockPrice> Prices { get; set; }
    }
}
