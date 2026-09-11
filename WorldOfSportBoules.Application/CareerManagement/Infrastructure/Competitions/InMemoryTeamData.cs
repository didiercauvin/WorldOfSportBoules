using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Infrastructure.Competitions;

public static class InMemoryTeamData
{
    private static readonly string[] FirstNames =
    [
        "Jean", "Pierre", "Michel", "Alain", "Philippe", "Laurent", "Christophe", "Nicolas",
        "Julien", "François", "Thomas", "Sébastien", "Olivier", "Pascal", "Patrick", "Didier",
        "David", "Éric", "Stéphane", "Vincent", "Marc", "Antoine", "Bruno", "Gérard",
        "Mathieu", "Romain", "Alexandre", "Fabrice", "Guillaume", "Jérôme"
    ];

    public static IReadOnlyList<Team> Teams { get; } =
    [
        Create("10000000-0000-0000-0000-000000000001", "Martin", 75),
        Create("10000000-0000-0000-0000-000000000002", "Durand", 63),
        Create("10000000-0000-0000-0000-000000000003", "Bernard", 80),
        Create("10000000-0000-0000-0000-000000000004", "Leroy", 68),
        Create("10000000-0000-0000-0000-000000000005", "Morel", 85),
        Create("10000000-0000-0000-0000-000000000006", "Garnier", 73),
        Create("10000000-0000-0000-0000-000000000007", "Roux", 61),
        Create("10000000-0000-0000-0000-000000000008", "Chevalier", 78),
        Create("10000000-0000-0000-0000-000000000009", "Faure", 66),
        Create("10000000-0000-0000-0000-000000000010", "Mercier", 83),
        Create("10000000-0000-0000-0000-000000000011", "Dupont", 71),
        Create("10000000-0000-0000-0000-000000000012", "Lambert", 59),
        Create("10000000-0000-0000-0000-000000000013", "Bonnet", 76),
        Create("10000000-0000-0000-0000-000000000014", "Girard", 64),
        Create("10000000-0000-0000-0000-000000000015", "Fontaine", 81),
        Create("10000000-0000-0000-0000-000000000016", "Rousseau", 69),
        Create("10000000-0000-0000-0000-000000000017", "Blanchard", 86),
        Create("10000000-0000-0000-0000-000000000018", "Giraud", 74),
        Create("10000000-0000-0000-0000-000000000019", "Muller", 62),
        Create("10000000-0000-0000-0000-000000000020", "Henry", 79),
        Create("10000000-0000-0000-0000-000000000021", "Renaud", 67),
        Create("10000000-0000-0000-0000-000000000022", "Legrand", 84),
        Create("10000000-0000-0000-0000-000000000023", "Perrin", 72),
        Create("10000000-0000-0000-0000-000000000024", "Robin", 60),
        Create("10000000-0000-0000-0000-000000000025", "Clément", 77),
        Create("10000000-0000-0000-0000-000000000026", "Morin", 65),
        Create("10000000-0000-0000-0000-000000000027", "Nicolas", 82),
        Create("10000000-0000-0000-0000-000000000028", "Gauthier", 70),
        Create("10000000-0000-0000-0000-000000000029", "Masson", 58),
        Create("10000000-0000-0000-0000-000000000030", "Marchand", 75),
        Create("10000000-0000-0000-0000-000000000031", "Rey", 63),
        Create("10000000-0000-0000-0000-000000000032", "André", 80),
        Create("10000000-0000-0000-0000-000000000033", "Gautier", 68),
        Create("10000000-0000-0000-0000-000000000034", "Lacroix", 85),
        Create("10000000-0000-0000-0000-000000000035", "Caron", 73),
        Create("10000000-0000-0000-0000-000000000036", "Colin", 61),
        Create("10000000-0000-0000-0000-000000000037", "Boucher", 78),
        Create("10000000-0000-0000-0000-000000000038", "Aubert", 66),
        Create("10000000-0000-0000-0000-000000000039", "Leclerc", 83),
        Create("10000000-0000-0000-0000-000000000040", "Renard", 71),
        Create("10000000-0000-0000-0000-000000000041", "Fleury", 59),
        Create("10000000-0000-0000-0000-000000000042", "Barbier", 76),
        Create("10000000-0000-0000-0000-000000000043", "Brun", 64),
        Create("10000000-0000-0000-0000-000000000044", "Gaudin", 81),
        Create("10000000-0000-0000-0000-000000000045", "Pelletier", 69),
        Create("10000000-0000-0000-0000-000000000046", "Guérin", 86),
        Create("10000000-0000-0000-0000-000000000047", "Dufour", 74),
        Create("10000000-0000-0000-0000-000000000048", "Meyer", 62),
        Create("10000000-0000-0000-0000-000000000049", "Olivier", 79),
        Create("10000000-0000-0000-0000-000000000050", "Roy", 67),
        Create("10000000-0000-0000-0000-000000000051", "Benoît", 84),
        Create("10000000-0000-0000-0000-000000000052", "Noël", 72),
        Create("10000000-0000-0000-0000-000000000053", "Perrot", 60),
        Create("10000000-0000-0000-0000-000000000054", "Tessier", 77),
        Create("10000000-0000-0000-0000-000000000055", "Lemoine", 65),
        Create("10000000-0000-0000-0000-000000000056", "Roussel", 82),
        Create("10000000-0000-0000-0000-000000000057", "Vidal", 70),
        Create("10000000-0000-0000-0000-000000000058", "Denis", 58),
        Create("10000000-0000-0000-0000-000000000059", "Arnaud", 75),
        Create("10000000-0000-0000-0000-000000000060", "Delorme", 63),
        Create("10000000-0000-0000-0000-000000000061", "Pichon", 80),
        Create("10000000-0000-0000-0000-000000000062", "Jacquet", 68),
        Create("10000000-0000-0000-0000-000000000063", "Bertin", 85),
        Create("10000000-0000-0000-0000-000000000064", "Charrier", 73),
        Create("10000000-0000-0000-0000-000000000065", "Cousin", 61),
        Create("10000000-0000-0000-0000-000000000066", "Berger", 78),
        Create("10000000-0000-0000-0000-000000000067", "Philippe", 66),
        Create("10000000-0000-0000-0000-000000000068", "Dumont", 83),
        Create("10000000-0000-0000-0000-000000000069", "Meunier", 71),
        Create("10000000-0000-0000-0000-000000000070", "Grondin", 59)
    ];

    private static Team Create(
        string idText,
        string surname,
        int strength)
    {
        var id = Guid.Parse(idText);

        var team = new Team(
            id,
            surname,
            TeamCategory.M4);

        var seedBytes = SHA256.HashData(
            Encoding.UTF8.GetBytes(idText));

        var seed = BitConverter.ToInt32(seedBytes, 0);
        var random = new Random(seed);

        for (var position = 0; position < 4; position++)
        {
            var firstNameIndex =
                ((seed & int.MaxValue) + position * 7) % FirstNames.Length;

            var category = position == 0 || random.Next(100) < 20
                ? PlayerCategory.M3
                : PlayerCategory.M4;

            var variation = random.Next(-10, 11);
            var tir = Math.Clamp(strength + variation, 0, 100);

            variation = random.Next(-10, 11);
            var point = Math.Clamp(strength + variation, 0, 100);

            team.AddPlayer(
                new Player(
                    CreatePlayerId(id, position + 1),
                    FirstNames[firstNameIndex],
                    surname,
                    category,
                    tir,
                    point));
        }

        return team;
    }

    private static Guid CreatePlayerId(
        Guid teamId,
        int position)
    {
        var input = $"{teamId:N}:{position}";
        var hash = SHA256.HashData(
            Encoding.UTF8.GetBytes(input));

        return new Guid(hash[..16]);
    }
}
