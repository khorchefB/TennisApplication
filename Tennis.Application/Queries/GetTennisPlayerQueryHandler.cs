using Tennis.Application.Exceptions;

namespace Tennis.Application.Queries;

public sealed class GetTennisPlayerQueryHandler(ITennisPlayerRepository tennisPlayerRepository)
    : IQueryHandler<GetTennisPlayerQuery, TennisPlayerDto>
{
    public async Task<TennisPlayerDto> Handle(
        GetTennisPlayerQuery request,
        CancellationToken cancellationToken)
    {
        var joueur = await tennisPlayerRepository.GetTennisPlayer(request.IdPlayer, cancellationToken)
            ?? throw new TennisPlayerNotFoundException(request.IdPlayer);

        return joueur.Adapt<TennisPlayerDto>();
    }
}
