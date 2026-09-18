
namespace Tennis.Application.Queries;

public class GetListTennisPlayersQueryHandler(ITennisPlayerRepository tennisPlayerRepository) : IQueryHandler<GetListTennisPlayersQuery, IEnumerable<TennisPlayerDto>?>
{
    public async Task<IEnumerable<TennisPlayerDto>?> Handle(GetListTennisPlayersQuery request, CancellationToken cancellationToken)
     => (await tennisPlayerRepository.GetTennisJoueurs()).OrderByDescending(player => player.Data.Rank).Adapt<IEnumerable<TennisPlayerDto>>();
}
