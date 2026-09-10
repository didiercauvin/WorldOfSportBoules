using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Domain;

public interface IProvideTeam
{
    IReadOnlyList<Team> GetForCompetition(
        CompetitionDefinition competition);

    IReadOnlyList<Team> GetForCategory(
        TeamCategory category);
}
