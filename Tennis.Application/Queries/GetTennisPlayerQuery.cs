namespace Tennis.Application.Queries;

public sealed record GetTennisPlayerQuery(int IdPlayer) : IQuery<TennisPlayerDto>;
