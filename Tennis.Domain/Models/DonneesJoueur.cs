using System.Text.Json.Serialization;

namespace Tennis.Domain.Models;

public class DonneesJoueur
{
    [JsonPropertyName("rank")]
    public int Rang { get; set; }

    [JsonPropertyName("points")]
    public int Points { get; set; }

    [JsonPropertyName("weight")]
    public int Poids { get; set; }

    [JsonPropertyName("height")]
    public int Taille { get; set; }

    [JsonPropertyName("age")]
    public int Age { get; set; }

    [JsonPropertyName("last")]
    public List<int> DerniersResultats { get; set; } = [];
}
