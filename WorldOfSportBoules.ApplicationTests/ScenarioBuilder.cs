using WorldOfSportBoules.Application.Rencontre.Domain;

namespace WorldOfSportBoules.ApplicationTests;

public sealed class ScenarioBuilder
{
    private readonly List<Player> _players = [];

    public static ScenarioBuilder UnePartie()
    {
        return new ScenarioBuilder();
    }

    public ScenarioBuilder AvecUnJoueur(string name)
    {
        var player = new Player(
            Guid.NewGuid(),
            name);

        _players.Add(player);

        return this;
    }

    public Match Build()
    {
        return new Match(_players);
    }
}
