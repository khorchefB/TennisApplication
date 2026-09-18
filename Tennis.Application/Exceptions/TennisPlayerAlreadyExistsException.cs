namespace Tennis.Application.Exceptions;

public sealed class TennisPlayerAlreadyExistsException(int idPlayer)
    : Exception($"Un joueur de tennis avec l'identifiant {idPlayer} existe déjà.")
{
    public int IdPlayer { get; } = idPlayer;
}
