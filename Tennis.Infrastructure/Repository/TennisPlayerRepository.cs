namespace Tennis.Infrastructure.Repository;

public class TennisPlayerRepository : ITennisPlayerRepository
{
    public async Task<TennisJoueur?> GetTennisPlayer(int idPlayer)
    {
        var players = await GetDeserializedPlayers();
        if (players is null || players.Count() == 0) return default;
        return players.FirstOrDefault(player => player.Id == idPlayer);
    }

    public async Task<IEnumerable<TennisJoueur>> GetTennisJoueurs()
        => await GetDeserializedPlayers() ?? new List<TennisJoueur>();

    private async Task<IEnumerable<TennisJoueur>> GetDeserializedPlayers()
    {
        var path = Path.Combine(
          Path.GetDirectoryName(
    Assembly.GetExecutingAssembly().Location
        ),
           "Data",
           "headtohead.json"
       );

        var json = await File.ReadAllTextAsync(path);

        var players = JsonSerializer.Deserialize<TennisPlayers>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );
        return players!.Players;
    }

}
