using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Application.CareerManagement.Domain;

public interface IProvideCompetition
{
    IReadOnlyList<ScheduledCompetition> GetForSeason(int seasonYear);
}
