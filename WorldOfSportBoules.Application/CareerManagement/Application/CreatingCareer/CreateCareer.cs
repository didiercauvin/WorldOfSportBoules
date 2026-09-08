using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Application.CreatingCareer;

public sealed record CreateCareerCommand(
    string ManagerName,
    string TeamName,
    TeamCategory Category);

public sealed class CreateCareerHandler
{
    public Career Handle(CreateCareerCommand command)
    {
        var team = new Team(
            Guid.NewGuid(),
            command.TeamName,
            command.Category);

        return new Career(
            Guid.NewGuid(),
            command.ManagerName,
            team);
    }
}