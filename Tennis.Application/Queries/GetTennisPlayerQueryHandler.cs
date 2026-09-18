
namespace Tennis.Application.Queries;

public class GetTennisPlayerQueryHandler(ITennisPlayerRepository tennisPlayerRepository) : IQueryHandler<GetTennisPlayerQuery, TennisPlayerDto?>
{
    public async Task<TennisPlayerDto?> Handle(GetTennisPlayerQuery request, CancellationToken cancellationToken)
        => (await tennisPlayerRepository.GetTennisPlayer(request.IdPlayer))?.Adapt<TennisPlayerDto>();
}
