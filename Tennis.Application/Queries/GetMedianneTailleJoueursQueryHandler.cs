using Tennis.Application.Exceptions;

namespace Tennis.Application.Queries;

public sealed class GetMedianneTailleJoueursQueryHandler(ITennisPlayerRepository tennisPlayerRepository)
    : IQueryHandler<GetMedianneTailleJoueursQuery, double>
{
    public async Task<double> Handle(
        GetMedianneTailleJoueursQuery request,
        CancellationToken cancellationToken)
    {
        var hauteurs = (await tennisPlayerRepository.GetTennisJoueurs(cancellationToken))
            .Select(joueur => joueur.Donnees.Taille)
            .OrderBy(hauteur => hauteur)
            .ToArray();

        if (hauteurs.Length == 0)
        {
            throw new TennisStatisticsUnavailableException("Impossible de calculer la médiane : aucun joueur n'est disponible.");
        }

        var milieu = hauteurs.Length / 2;
        return hauteurs.Length % 2 == 1
            ? hauteurs[milieu]
            : (hauteurs[milieu - 1] + hauteurs[milieu]) / 2d;
    }
}
