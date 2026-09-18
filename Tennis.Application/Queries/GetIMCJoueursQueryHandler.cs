
using Tennis.Domain.Models;

namespace Tennis.Application.Queries;

public class GetIMCJoueursQueryHandler(ITennisPlayerRepository tennisPlayerRepository) : IQueryHandler<GetIMCJoueursQuery, double>
{
    public async Task<double> Handle(GetIMCJoueursQuery request, CancellationToken cancellationToken)
    {
        var tennisjoueurs = await tennisPlayerRepository.GetTennisJoueurs();
        return CalculerMoyenToutJoueurs(tennisjoueurs);
    }

    private double CalculerIMC(TennisJoueur joueur)
        => joueur.Data.Weight / Math.Pow(joueur.Data.Height, 2);

    private double CalculerMoyenToutJoueurs(IEnumerable<TennisJoueur> joueurs)
        => joueurs.Select(joueur => CalculerIMC(joueur)).Sum(imc => imc) / joueurs.Count();
}
