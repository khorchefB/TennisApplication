namespace Tennis.Application.Queries;

public sealed class GetListTennisPlayersQueryHandler(ITennisPlayerRepository tennisPlayerRepository)
    : IQueryHandler<GetListTennisPlayersQuery, IReadOnlyCollection<JoueurTennisDto>>
{
    public async Task<IReadOnlyCollection<JoueurTennisDto>> Handle(
        GetListTennisPlayersQuery request,
        CancellationToken cancellationToken)
        => (await tennisPlayerRepository.GetTennisJoueurs(cancellationToken))
            .OrderBy(player => player.Donnees.Rang)
            .Select(player => player.Adapt<JoueurTennisDto>())
            .ToArray();
}
