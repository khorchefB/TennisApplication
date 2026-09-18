
namespace Tennis.Application.Queries;

public class GetGrandRatioPartiesGagneesQueryHandler(ITennisPlayerRepository tennisPlayerRepository) : IQueryHandler<GetGrandRatioPartiesGagneesQuery, int>
{
    public async Task<int> Handle(GetGrandRatioPartiesGagneesQuery request, CancellationToken cancellationToken)
    {
        var tennisjoueurs = await tennisPlayerRepository.GetTennisJoueurs();

        return 0;
    }
}
