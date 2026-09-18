using MediatR;
using Tennis.Application.Commands;

namespace Tennis.Api.Controllers;

[ApiController]
[Route("api/players")]
public sealed class PlayersController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<TennisPlayerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<TennisPlayerDto>>> GetTennisJoueurs(
        CancellationToken cancellationToken)
    {
        var joueurs = await sender.Send(new GetListTennisPlayersQuery(), cancellationToken);
        return Ok(joueurs);
    }

    [HttpGet("{idPlayer:int}", Name = nameof(GetTennisJoueur))]
    [ProducesResponseType(typeof(TennisPlayerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TennisPlayerDto>> GetTennisJoueur(
        [FromRoute] int idPlayer,
        CancellationToken cancellationToken)
    {
        var joueur = await sender.Send(new GetTennisPlayerQuery(idPlayer), cancellationToken);
        return Ok(joueur);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TennisPlayerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TennisPlayerDto>> AjouterJoueur(
        [FromBody] TennisPlayerDto joueur,
        CancellationToken cancellationToken)
    {
        var joueurAjoute = await sender.Send(
            new AjouterTennisJoueurCommand(joueur),
            cancellationToken);

        return CreatedAtRoute(
            nameof(GetTennisJoueur),
            new { idPlayer = joueurAjoute.Id },
            joueurAjoute);
    }
}
