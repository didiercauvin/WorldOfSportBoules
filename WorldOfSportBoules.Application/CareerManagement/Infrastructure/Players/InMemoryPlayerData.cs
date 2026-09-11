using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Infrastructure.Players;

internal static class InMemoryPlayerData
{
    public static IReadOnlyList<Player> Players =>
    [
        new(
            Guid.Parse("10000000-0000-0000-0000-000000000001"),
            "Jean",
            "Dupont",
            PlayerCategory.M4,
            50,
            68),

        new(
            Guid.Parse("10000000-0000-0000-0000-000000000002"),
            "Michel",
            "Martin",
            PlayerCategory.M4,
            50,
            68),

        new(
            Guid.Parse("10000000-0000-0000-0000-000000000003"),
            "Philippe",
            "Durand",
            PlayerCategory.M4,
            50,
            68),

        new(
            Guid.Parse("10000000-0000-0000-0000-000000000004"),
            "Christophe",
            "Bernard",
            PlayerCategory.M4,
            50,
            68),

        new(
            Guid.Parse("10000000-0000-0000-0000-000000000005"),
            "Laurent",
            "Moreau",
            PlayerCategory.M4,
            50,
            68),

        new(
            Guid.Parse("10000000-0000-0000-0000-000000000006"),
            "Patrick",
            "Robert",
            PlayerCategory.M3,
            50,
            68),

        new(
            Guid.Parse("10000000-0000-0000-0000-000000000007"),
            "Alain",
            "Petit",
            PlayerCategory.M2,
            50,
            68),

        new(
            Guid.Parse("10000000-0000-0000-0000-000000000008"),
            "Frédéric",
            "Richard",
            PlayerCategory.M2,
            50,
            68)
    ];
}
