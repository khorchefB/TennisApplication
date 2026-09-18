using Tennis.Domain.Models;

namespace Tennis.Application.Queries;

public class GetMedianneTailleJoueursQueryHandler(ITennisPlayerRepository tennisPlayerRepository) : IQueryHandler<GetMedianneTailleJoueursQuery, double>
{
    public async Task<double> Handle(GetMedianneTailleJoueursQuery request, CancellationToken cancellationToken)
    {
        var tennisjoueurs = await tennisPlayerRepository.GetTennisJoueurs();
        return GetMedianne(tennisjoueurs);
    }

    private double GetMedianne(IEnumerable<TennisJoueur> tennisJoueurs)
    {
        var hauteursTries = GetHauteursTriesGetHauteurs(tennisJoueurs);
        return Mediane(hauteursTries);
    }

    private int[] GetHauteursTriesGetHauteurs(IEnumerable<TennisJoueur> tennisJoueurs)
        => tennisJoueurs.Select(tennisjoueur => tennisjoueur.Data.Height).ToArray();

    private double Mediane(int[] hauteursjoueurs)
    {
        Array.Sort(hauteursjoueurs);

        int n = hauteursjoueurs.Length;

        if (n % 2 == 1)
        {
            return hauteursjoueurs[n / 2];
        }
        else
        {
            return ((double)hauteursjoueurs[n / 2 - 1] + hauteursjoueurs[n / 2]) / 2;
        }
    }
}
