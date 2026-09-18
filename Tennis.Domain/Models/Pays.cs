using System.Text.Json.Serialization;

namespace Tennis.Domain.Models;

public class Pays
{
    [JsonPropertyName("picture")]
    public string Photo { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;
}
