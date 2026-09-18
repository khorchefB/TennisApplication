namespace Tennis.Application.Queries;

public sealed class GetListTennisPlayersQueryHandler(ITennisPlayerRepository tennisPlayerRepository)
    : IQueryHandler<GetListTennisPlayersQuery, IReadOnlyCollection<TennisPlayerDto>>
{
    public async Task<IReadOnlyCollection<TennisPlayerDto>> Handle(
        GetListTennisPlayersQuery request,
        CancellationToken cancellationToken)
        => (await tennisPlayerRepository.GetTennisJoueurs(cancellationToken))
            .OrderBy(player => player.Data.Rank)
            .Select(player => player.Adapt<TennisPlayerDto>())
            .ToArray();
}
