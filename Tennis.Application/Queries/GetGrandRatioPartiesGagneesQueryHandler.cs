using Tennis.Application.Exceptions;

namespace Tennis.Application.Queries;

public sealed class GetGrandRatioPartiesGagneesQueryHandler(ITennisPlayerRepository tennisPlayerRepository)
    : IQueryHandler<GetGrandRatioPartiesGagneesQuery, string>
{
    public async Task<string> Handle(
        GetGrandRatioPartiesGagneesQuery request,
        CancellationToken cancellationToken)
    {
        var joueurs = await tennisPlayerRepository.GetTennisJoueurs(cancellationToken);

        var meilleurPays = joueurs
            .Where(joueur => !string.IsNullOrWhiteSpace(joueur.Country.Code) && joueur.Data.Last.Count > 0)
            .GroupBy(joueur => joueur.Country.Code)
            .Select(groupe => new
            {
                Country = groupe.Key,
                Victoires = groupe.Sum(joueur => joueur.Data.Last.Count(resultat => resultat == 1)),
                Parties = groupe.Sum(joueur => joueur.Data.Last.Count)
            })
            .Where(statistique => statistique.Parties > 0)
            .OrderByDescending(statistique => (double)statistique.Victoires / statistique.Parties)
            .ThenBy(statistique => statistique.Country)
            .FirstOrDefault();

        return meilleurPays?.Country
            ?? throw new TennisStatisticsUnavailableException(
                "Impossible de déterminer le pays avec le meilleur ratio de victoires.");
    }
}
