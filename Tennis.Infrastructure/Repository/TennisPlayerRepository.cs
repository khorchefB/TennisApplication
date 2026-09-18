namespace Tennis.Infrastructure.Repository;

public class TennisPlayerRepository : ITennisPlayerRepository, IHostedService
{
    private readonly List<TennisJoueur> _players = [];
    private readonly SemaphoreSlim _playersLock = new(1, 1);

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
            "Data",
            "headtohead.json");

        var json = await File.ReadAllTextAsync(path, cancellationToken);
        var players = JsonSerializer.Deserialize<TennisPlayers>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        await _playersLock.WaitAsync(cancellationToken);
        try
        {
            _players.Clear();
            _players.AddRange(players?.Players ?? []);
        }
        finally
        {
            _playersLock.Release();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task<TennisJoueur?> GetTennisPlayer(int idPlayer)
    {
        await _playersLock.WaitAsync();
        try
        {
            return _players.FirstOrDefault(player => player.Id == idPlayer);
        }
        finally
        {
            _playersLock.Release();
        }
    }

    public async Task<IEnumerable<TennisJoueur>> GetTennisJoueurs()
    {
        await _playersLock.WaitAsync();
        try
        {
            return _players.ToList();
        }
        finally
        {
            _playersLock.Release();
        }
    }

    public async Task<TennisJoueur> AjouterTennisJoueur(TennisJoueur joueur)
    {
        await _playersLock.WaitAsync();
        try
        {
            if (_players.Any(player => player.Id == joueur.Id))
            {
                throw new InvalidOperationException($"Un joueur avec l'identifiant {joueur.Id} existe déjà.");
            }

            _players.Add(joueur);
            return joueur;
        }
        finally
        {
            _playersLock.Release();
        }
    }
}
