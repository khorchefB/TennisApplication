using MediatR;

namespace Tennis.Api.Controllers;

[ApiController]
[Route("api/statistics")]
public sealed class StatisticsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(StatistiquesDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<StatistiquesDto>> GetStatistics(CancellationToken cancellationToken)
    {
        var pays = await sender.Send(new GetGrandRatioPartiesGagneesQuery(), cancellationToken);
        var imcMoyen = await sender.Send(new GetIMCJoueursQuery(), cancellationToken);
        var medianne = await sender.Send(new GetMedianneTailleJoueursQuery(), cancellationToken);

        return Ok(new StatistiquesDto
        {
            Pays = pays,
            IMC = imcMoyen,
            Medianne = medianne
        });
    }
}
