using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Application.CareerManagement.Domain;

public sealed class Career
{
    public Guid Id { get; }
    public string ManagerName { get; }
    public Team Team { get; }

    public Season? CurrentSeason { get; private set; }

    public Career(
        Guid id,
        string managerName,
        Team team)
    {
        Id = id;
        ManagerName = managerName;
        Team = team;
    }

    public void SetCurrentSeason(Season season)
    {
        ArgumentNullException.ThrowIfNull(season);

        CurrentSeason = season;
    }
}
