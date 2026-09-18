using MediatR;
using Tennis.Application.Commands;

namespace Tennis.Api.Controllers;

[ApiController]
[Route("api/players")]
public sealed class PlayersController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<JoueurTennisDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<JoueurTennisDto>>> GetTennisJoueurs(
        CancellationToken cancellationToken)
    {
        var joueurs = await sender.Send(new GetListTennisPlayersQuery(), cancellationToken);
        return Ok(joueurs);
    }

    [HttpGet("{idPlayer:int}", Name = nameof(GetTennisJoueur))]
    [ProducesResponseType(typeof(JoueurTennisDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JoueurTennisDto>> GetTennisJoueur(
        [FromRoute] int idPlayer,
        CancellationToken cancellationToken)
    {
        var joueur = await sender.Send(new GetTennisPlayerQuery(idPlayer), cancellationToken);
        return Ok(joueur);
    }

    [HttpPost]
    [ProducesResponseType(typeof(JoueurTennisDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<JoueurTennisDto>> AjouterJoueur(
        [FromBody] JoueurTennisDto joueur,
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
