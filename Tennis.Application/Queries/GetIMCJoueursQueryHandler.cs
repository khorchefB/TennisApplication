using Tennis.Application.Exceptions;
using Tennis.Domain.Models;

namespace Tennis.Application.Queries;

public sealed class GetIMCJoueursQueryHandler(ITennisPlayerRepository tennisPlayerRepository)
    : IQueryHandler<GetIMCJoueursQuery, double>
{
    public async Task<double> Handle(GetIMCJoueursQuery request, CancellationToken cancellationToken)
    {
        var joueurs = await tennisPlayerRepository.GetTennisJoueurs(cancellationToken);

        if (joueurs.Count == 0)
        {
            throw new TennisStatisticsUnavailableException("Impossible de calculer l'IMC moyen : aucun joueur n'est disponible.");
        }

        return joueurs.Average(CalculerIMC);
    }

    private static double CalculerIMC(TennisJoueur joueur)
    {
        if (joueur.Data.Height <= 0 || joueur.Data.Weight <= 0)
        {
            throw new TennisStatisticsUnavailableException(
                $"Les données de taille ou de poids du joueur {joueur.Id} sont invalides.");
        }

        var poidsKg = joueur.Data.Weight / 1000d;
        var tailleMetres = joueur.Data.Height / 100d;
        return poidsKg / Math.Pow(tailleMetres, 2);
    }
}
