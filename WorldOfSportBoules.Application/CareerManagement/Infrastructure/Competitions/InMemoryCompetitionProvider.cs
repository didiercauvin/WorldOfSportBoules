using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Infrastructure.Competitions;

public sealed class InMemoryCompetitionProvider : IProvideCompetition
{
    private IReadOnlyList<ScheduledCompetition> _cache;

    public IReadOnlyList<ScheduledCompetition> GetForSeason(
        int seasonYear)
    {
        if (_cache is not null)
        {
            return _cache;
        }

        var definitions = InMemoryCompetitionData.Definitions
            .ToDictionary(x => x.Id);

        _cache = InMemoryCompetitionData.Editions
            .Where(x => x.StartDate.Year == seasonYear)
            .Select(x => new ScheduledCompetition(
                Guid.NewGuid(),
                definitions[x.CompetitionDefinitionId],
                seasonYear,
                x.StartDate,
                x.EndDate,
                x.EntryFee))
            .ToList();

        return _cache;
    }
}

internal sealed record CompetitionEditionData(
    Guid CompetitionDefinitionId,
    DateOnly StartDate,
    DateOnly EndDate,
    decimal EntryFee);
