using Tennis.Domain.Models;

namespace Tennis.Application.Abstraction;

public interface ITennisPlayerRepository
{
    Task<IEnumerable<TennisJoueur>> GetTennisJoueurs();
    Task<TennisJoueur?> GetTennisPlayer(int idPlayer);
}
