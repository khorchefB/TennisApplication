using System.Text.Json.Serialization;

namespace Tennis.Domain.Models;

public class JoueursTennis
{
    [JsonPropertyName("players")]
    public IEnumerable<TennisJoueur> Joueurs { get; set; } = [];
}
