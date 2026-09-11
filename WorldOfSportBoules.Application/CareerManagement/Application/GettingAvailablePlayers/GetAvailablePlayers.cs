using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailablePlayers;

public sealed record GetAvailablePlayersQuery(TeamCategory PlayerCategory);

public sealed class GetAvailablePlayersHandler
{
    private readonly IProvidePlayer _playerProvider;

    public GetAvailablePlayersHandler(
        IProvidePlayer playerProvider)
    {
        _playerProvider = playerProvider;
    }

    public IReadOnlyList<Player> Handle(
        GetAvailablePlayersQuery query)
    {
        var players = _playerProvider.GetForCategory(query.PlayerCategory);

        return query.PlayerCategory switch
        {
            TeamCategory.M4 =>
                players
                    .Where(x =>
                        x.Category is PlayerCategory.M4 or PlayerCategory.M3)
                    .ToList(),

            _ => throw new NotSupportedException(
                $"La catégorie {query.PlayerCategory} n'est pas encore supportée.")
        };
    }
}