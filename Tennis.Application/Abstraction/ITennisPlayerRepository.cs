using Tennis.Domain.Models;

namespace Tennis.Application.Abstraction;

public interface ITennisPlayerRepository
{
    Task<IReadOnlyCollection<TennisJoueur>> GetTennisJoueurs(CancellationToken cancellationToken = default);
    Task<TennisJoueur?> GetTennisPlayer(int idPlayer, CancellationToken cancellationToken = default);
    Task<TennisJoueur> AjouterTennisJoueur(TennisJoueur joueur, CancellationToken cancellationToken = default);
}
