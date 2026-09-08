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

        if (Category == TeamCategory.M4 &&
            player.Category is not (PlayerCategory.M3 or PlayerCategory.M4))
        {
            throw new InvalidOperationException(
                $"Un joueur {player.Category} ne peut pas intégrer une équipe M4.");
        }

        if (_players.Any(x => x.Id == player.Id))
        {
            throw new InvalidOperationException(
                "Ce joueur est déjà dans l'équipe.");
        }

        _players.Add(player);
    }
}
