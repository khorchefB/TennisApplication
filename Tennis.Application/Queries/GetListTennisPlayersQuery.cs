namespace Tennis.Application.Queries;

public sealed record GetListTennisPlayersQuery : IQuery<IReadOnlyCollection<TennisPlayerDto>>;
