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
            .Where(joueur => !string.IsNullOrWhiteSpace(joueur.Pays.Code) && joueur.Donnees.DerniersResultats.Count > 0)
            .GroupBy(joueur => joueur.Pays.Code)
            .Select(groupe => new
            {
                Pays = groupe.Key,
                Victoires = groupe.Sum(joueur => joueur.Donnees.DerniersResultats.Count(resultat => resultat == 1)),
                Parties = groupe.Sum(joueur => joueur.Donnees.DerniersResultats.Count)
            })
            .Where(statistique => statistique.Parties > 0)
            .OrderByDescending(statistique => (double)statistique.Victoires / statistique.Parties)
            .ThenBy(statistique => statistique.Pays)
            .FirstOrDefault();

        return meilleurPays?.Pays
            ?? throw new TennisStatisticsUnavailableException(
                "Impossible de déterminer le pays avec le meilleur ratio de victoires.");
    }
}
