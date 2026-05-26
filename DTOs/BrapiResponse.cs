using System.Text.Json.Serialization;

public class BrapiResponse {
    [JsonPropertyName("stocks")]
    public List<BrapiStock> Stocks { get; set; } = new();
}

public class BrapiStock {
    [JsonPropertyName("stock")]
    public string Stock { get; set; } = string.Empty;

    [JsonPropertyName("close")]
    public decimal? Close { get; set; }
}

