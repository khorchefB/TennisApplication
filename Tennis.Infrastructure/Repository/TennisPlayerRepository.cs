
namespace Tennis.Infrastructure.Repository;

public sealed class TennisPlayerRepository : ITennisPlayerRepository, IHostedService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly List<TennisJoueur> _players = [];
    private readonly SemaphoreSlim _playersLock = new(1, 1);

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
            "Data",
            "headtohead.json");

        try
        {
            var json = await File.ReadAllTextAsync(path, cancellationToken);
            var players = JsonSerializer.Deserialize<JoueursTennis>(json, JsonOptions)
                ?? throw new JsonException("Le fichier des joueurs ne contient aucune donnée exploitable.");

            await _playersLock.WaitAsync(cancellationToken);
            try
            {
                _players.Clear();
                _players.AddRange(players.Joueurs);
            }
            finally
            {
                _playersLock.Release();
            }
        }
        catch (Exception exception) when (exception is IOException or JsonException)
        {
            throw new TennisDataInitializationException(
                $"Impossible de charger les données de tennis depuis '{path}'.",
                exception);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task<TennisJoueur?> GetTennisPlayer(
        int idPlayer,
        CancellationToken cancellationToken = default)
    {
        await _playersLock.WaitAsync(cancellationToken);
        try
        {
            return _players.FirstOrDefault(player => player.Id == idPlayer);
        }
        finally
        {
            _playersLock.Release();
        }
    }

    public async Task<IReadOnlyCollection<TennisJoueur>> GetTennisJoueurs(
        CancellationToken cancellationToken = default)
    {
        await _playersLock.WaitAsync(cancellationToken);
        try
        {
            return _players.ToArray();
        }
        finally
        {
            _playersLock.Release();
        }
    }

    public async Task<TennisJoueur> AjouterTennisJoueur(
        TennisJoueur joueur,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(joueur);

        await _playersLock.WaitAsync(cancellationToken);
        try
        {
            if (_players.Any(player => player.Id == joueur.Id))
            {
                throw new TennisPlayerAlreadyExistsException(joueur.Id);
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
