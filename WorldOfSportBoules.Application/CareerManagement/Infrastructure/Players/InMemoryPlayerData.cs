using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Infrastructure.Players;

internal static class InMemoryPlayerData
{
    public static IReadOnlyList<Player> Players { get; } =
    [
        Create("Jean", "Martin", PlayerCategory.M3, 82, 78),
        Create("Pierre", "Durand", PlayerCategory.M3, 75, 84),
        Create("Michel", "Bernard", PlayerCategory.M4, 68, 81),
        Create("Alain", "Leroy", PlayerCategory.M4, 79, 65),

        Create("Philippe", "Morel", PlayerCategory.M3, 88, 76),
        Create("Laurent", "Garnier", PlayerCategory.M4, 71, 73),
        Create("Christophe", "Roux", PlayerCategory.M4, 64, 86),
        Create("Nicolas", "Chevalier", PlayerCategory.M3, 81, 82),

        Create("Julien", "Faure", PlayerCategory.M4, 76, 69),
        Create("François", "Mercier", PlayerCategory.M4, 62, 78),
        Create("Thomas", "Dupont", PlayerCategory.M3, 85, 80),
        Create("Sébastien", "Lambert", PlayerCategory.M4, 73, 72),

        Create("Olivier", "Bonnet", PlayerCategory.M3, 79, 88),
        Create("Pascal", "Girard", PlayerCategory.M4, 67, 75),
        Create("Patrick", "Fontaine", PlayerCategory.M4, 82, 63),
        Create("Didier", "Rousseau", PlayerCategory.M3, 86, 84),

        Create("David", "Blanchard", PlayerCategory.M4, 69, 79),
        Create("Éric", "Giraud", PlayerCategory.M4, 74, 70),
        Create("Stéphane", "Muller", PlayerCategory.M3, 83, 77),
        Create("Vincent", "Henry", PlayerCategory.M4, 65, 85),

        Create("Marc", "Renaud", PlayerCategory.M3, 80, 81),
        Create("Antoine", "Legrand", PlayerCategory.M4, 72, 76),
        Create("Bruno", "Perrin", PlayerCategory.M4, 77, 68),
        Create("Gérard", "Robin", PlayerCategory.M3, 89, 74),

        Create("Mathieu", "Clément", PlayerCategory.M4, 70, 82),
        Create("Romain", "Morin", PlayerCategory.M4, 63, 77),
        Create("Alexandre", "Nicolas", PlayerCategory.M3, 84, 86),
        Create("Fabrice", "Gauthier", PlayerCategory.M4, 78, 71),

        Create("Guillaume", "Masson", PlayerCategory.M3, 81, 79),
        Create("Jérôme", "Marchand", PlayerCategory.M4, 66, 83),
        Create("Loïc", "Rey", PlayerCategory.M4, 75, 74),
        Create("Arnaud", "André", PlayerCategory.M3, 87, 81),

        Create("Benoît", "Gautier", PlayerCategory.M4, 71, 80),
        Create("Rémi", "Lacroix", PlayerCategory.M4, 68, 73),
        Create("Florian", "Caron", PlayerCategory.M3, 85, 78),
        Create("Xavier", "Colin", PlayerCategory.M4, 73, 85),

        Create("Cédric", "Boucher", PlayerCategory.M4, 77, 69),
        Create("Damien", "Aubert", PlayerCategory.M3, 82, 83),
        Create("Franck", "Leclerc", PlayerCategory.M4, 64, 80),
        Create("Jérémy", "Renard", PlayerCategory.M4, 79, 76)
    ];

    private static Player Create(
    string firstName,
    string lastName,
    PlayerCategory category,
    int tir,
    int point)
    {
        var input =
            $"{firstName}:{lastName}:{category}";

        var hash =
            System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(input));

        return new Player(
            new Guid(hash[..16]),
            firstName,
            lastName,
            category,
            tir,
            point);
    }
}
