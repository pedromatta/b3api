public class B3IntegrationService
{
    private readonly HttpClient _httpClient;
    private readonly string _brapiToken;

    public B3IntegrationService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _brapiToken = configuration.GetValue<string>("BRAPI_TOKEN")
            ?? throw new ArgumentNullException("BRAPI_TOKEN not configured");
    }

    public async Task<List<AssetQuote>> FetchDailyQuotesAsync()
    {
        var url = $"https://brapi.dev/api/quote/list?token={_brapiToken}";

        var response = await _httpClient.GetFromJsonAsync<BrapiResponse>(url);
        var quotesList = new List<AssetQuote>();

        if (response?.Stocks != null)
        {
            foreach (var item in response.Stocks)
            {
                if (!item.Close.HasValue) continue;
                quotesList.Add(new AssetQuote
                {
                    Ticker = item.Stock,
                    Price = item.Close.Value,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }
        return quotesList;
    }
}
