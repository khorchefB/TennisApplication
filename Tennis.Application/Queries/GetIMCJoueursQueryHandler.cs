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
        if (joueur.Donnees.Taille <= 0 || joueur.Donnees.Poids <= 0)
        {
            throw new TennisStatisticsUnavailableException(
                $"Les données de taille ou de poids du joueur {joueur.Id} sont invalides.");
        }

        var poidsKg = joueur.Donnees.Poids / 1000d;
        var tailleMetres = joueur.Donnees.Taille / 100d;
        return poidsKg / Math.Pow(tailleMetres, 2);
    }
}
