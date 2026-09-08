using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailableCompetitions;

public sealed record GetAvailableCompetitionsQuery(int SeasonYear);

public sealed class GetAvailableCompetitionsHandler
{
    private readonly IProvideCompetition _competitionProvider;

    public GetAvailableCompetitionsHandler(
        IProvideCompetition competitionProvider)
    {
        _competitionProvider = competitionProvider;
    }

    public IReadOnlyList<ScheduledCompetition> Handle(
        GetAvailableCompetitionsQuery query)
    {
        return _competitionProvider.GetForSeason(query.SeasonYear);
    }
}
