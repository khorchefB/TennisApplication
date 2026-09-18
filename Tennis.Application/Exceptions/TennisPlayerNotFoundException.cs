namespace Tennis.Application.Exceptions;

public sealed class TennisPlayerNotFoundException(int idPlayer)
    : Exception($"Le joueur de tennis avec l'identifiant {idPlayer} est introuvable.")
{
    public int IdPlayer { get; } = idPlayer;
}
