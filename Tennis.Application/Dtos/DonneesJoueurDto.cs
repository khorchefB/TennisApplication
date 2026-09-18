using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tennis.Application.Dtos;

public class DonneesJoueurDto
{
    [Range(1, int.MaxValue)]
    [JsonPropertyName("rank")]
    public int Rang { get; set; }

    [Range(0, int.MaxValue)]
    [JsonPropertyName("points")]
    public int Points { get; set; }

    [Range(1, int.MaxValue)]
    [JsonPropertyName("weight")]
    public int Poids { get; set; }

    [Range(1, int.MaxValue)]
    [JsonPropertyName("height")]
    public int Taille { get; set; }

    [Range(1, 120)]
    [JsonPropertyName("age")]
    public int Age { get; set; }

    [Required]
    [MinLength(1)]
    [JsonPropertyName("last")]
    public List<int> DerniersResultats { get; set; } = [];
}
