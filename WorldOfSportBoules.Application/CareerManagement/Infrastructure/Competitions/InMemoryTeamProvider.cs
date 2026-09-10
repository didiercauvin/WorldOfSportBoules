using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Infrastructure.Competitions;

public sealed class InMemoryTeamProvider : IProvideTeam
{
    public IReadOnlyList<Team> GetForCompetition(
        CompetitionDefinition competition)
    {
        return InMemoryTeamData.Teams
            .Where(x => x.Category == TeamCategory.M4)
            .ToList();
    }

    public IReadOnlyList<Team> GetForCategory(
    TeamCategory category)
    {
        return InMemoryTeamData.Teams
            .Where(x => x.Category == category)
            .ToList();
    }
}
