using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.Rencontre.Domain;

namespace WorldOfSportBoules.ApplicationTests;

public sealed class ScenarioBuilder
{
    private readonly List<Player> _players = [];

    public static ScenarioBuilder UnePartie()
    {
        return new ScenarioBuilder();
    }

    public ScenarioBuilder AvecUnJoueur(string firstname, string lastname)
    {
        var player = new Player(
            Guid.NewGuid(),
            firstname,
            lastname,
            PlayerCategory.M4,
            10,
            10);

        _players.Add(player);

        return this;
    }

    public Match Build()
    {
        return new Match(_players);
    }
}
