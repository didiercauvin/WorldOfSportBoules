using WorldOfSportBoules.Application.Rencontre.Domain;

namespace WorldOfSportBoules.Application.Rencontre.Engine;

public sealed class MatchEngine
{
    private readonly ButEngine _butEngine;

    public MatchEngine(ButEngine butEngine)
    {
        _butEngine = butEngine;
    }

    public void LancerLeBut(Match match, string playerName, Position cible)
    {
        var player = match.Players.SingleOrDefault(x => x.Name == playerName);

        if (player is null)
            throw new InvalidOperationException(
                $"Le joueur '{playerName}' ne participe pas à la partie.");

        var position = _butEngine.Lancer(player, cible);

        match.PlacerLeBut(new But(position));
    }
}
