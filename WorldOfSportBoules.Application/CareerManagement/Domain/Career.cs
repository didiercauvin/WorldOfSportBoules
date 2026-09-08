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

    public Career(
        Guid id,
        string managerName,
        Team team)
    {
        Id = id;
        ManagerName = managerName;
        Team = team;
    }
}
