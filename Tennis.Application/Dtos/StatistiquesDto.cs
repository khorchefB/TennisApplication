using System.Text.Json.Serialization;

namespace Tennis.Application.Dtos;

public class StatistiquesDto
{
    [JsonPropertyName("country")]
    public string Pays { get; set; } = string.Empty;

    [JsonPropertyName("imc")]
    public double IMC { get; set; }

    [JsonPropertyName("medianne")]
    public double Medianne { get; set; }
}
