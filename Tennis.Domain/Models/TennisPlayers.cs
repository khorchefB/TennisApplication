using System.Text.Json.Serialization;

namespace Tennis.Domain.Models;

public class TennisPlayers
{
    [JsonPropertyName("players")]
    public IEnumerable<TennisJoueur> Players { get; set; } = [];
}
