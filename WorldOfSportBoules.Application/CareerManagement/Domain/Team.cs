using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Domain;

public sealed class Team
{
    private readonly List<Player> _players = [];

    public Guid Id { get; }
    public string Name { get; }
    public TeamCategory Category { get; }

    public IReadOnlyList<Player> Players => _players;

    public Team(
        Guid id,
        string name,
        TeamCategory category)
    {
        Id = id;
        Name = name;
        Category = category;
    }

    public void AddPlayer(Player player)
    {
        ArgumentNullException.ThrowIfNull(player);

        _players.Add(player);
    }
}
