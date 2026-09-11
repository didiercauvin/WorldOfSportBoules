using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Infrastructure.Players;

public sealed class InMemoryPlayerProvider : IProvidePlayer
{
    public IReadOnlyList<Player> GetForCategory(TeamCategory category)
    {
        return category switch
        {
            TeamCategory.M4 =>
                InMemoryPlayerData.Players
                    .Where(x =>
                        x.Category is
                            PlayerCategory.M3 or
                            PlayerCategory.M4)
                    .ToList(),

            _ =>
                InMemoryPlayerData.Players.ToList()
        };
    }
}