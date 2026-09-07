using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WorldOfSportBoules.Domain;
using Match = WorldOfSportBoules.Domain.Match;

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
