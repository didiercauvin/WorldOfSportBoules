using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Infrastructure.Competitions;

internal static class InMemoryCompetitionData
{
    public static IReadOnlyList<CompetitionDefinition> Definitions =>
    [
        new(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "Concours de Saint-Martin",
            "Saint-Martin",
            35,
            CompetitionType.Loisir, 
            CompetitionFormat.Poules,
            CompetitionTeamFormat.Quadrette,
            CompetitionCategory.M3M4),

        new(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "Concours de Thouars",
            "Thouars",
            55,
            CompetitionType.Loisir,
            CompetitionFormat.EliminationDirecte,
            CompetitionTeamFormat.Quadrette,
            CompetitionCategory.M3M4),

        new(
            Guid.Parse("33333333-3333-3333-3333-333333333333"),
            "Grand Prix de Tours",
            "Tours",
            220,
            CompetitionType.Promotion,
            CompetitionFormat.EliminationDirecte,
            CompetitionTeamFormat.Quadrette,
            CompetitionCategory.M3M4),

        new(
            Guid.Parse("44444444-4444-4444-4444-444444444444"),
            "Grand Prix de Limoges",
            "Limoges",
            350,
            CompetitionType.Promotion,
            CompetitionFormat.Poules,
            CompetitionTeamFormat.Quadrette,
            CompetitionCategory.M3M4),

        new(
            Guid.Parse("55555555-5555-5555-5555-555555555555"),
            "Régional de Poitiers",
            "Poitiers",
            180,
            CompetitionType.Promotion,
            CompetitionFormat.EliminationDirecte,
            CompetitionTeamFormat.Quadrette,
            CompetitionCategory.M2)
    ];

    public static IReadOnlyList<CompetitionEditionData> Editions =>
    [
        new(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            new DateOnly(2026, 9, 6),
            new DateOnly(2026, 9, 6),
            10m),

        new(
            Guid.Parse("33333333-3333-3333-3333-333333333333"),
            new DateOnly(2026, 9, 26),
            new DateOnly(2026, 9, 27),
            30m),

        new(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            new DateOnly(2026, 10, 4),
            new DateOnly(2026, 10, 4),
            12m),

        new(
            Guid.Parse("55555555-5555-5555-5555-555555555555"),
            new DateOnly(2026, 10, 17),
            new DateOnly(2026, 10, 18),
            20m),

        new(
            Guid.Parse("44444444-4444-4444-4444-444444444444"),
            new DateOnly(2026, 11, 7),
            new DateOnly(2026, 11, 8),
            35m)
    ];
}
