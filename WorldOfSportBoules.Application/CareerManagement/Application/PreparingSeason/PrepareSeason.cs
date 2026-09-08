using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Application.PreparingSeason;

public sealed record PrepareSeasonCommand(
    int SeasonYear,
    IReadOnlyList<Guid> CompetitionIds);


public sealed class PrepareSeasonHandler
{
    private readonly IProvideCompetition _competitionProvider;

    public PrepareSeasonHandler(
        IProvideCompetition competitionProvider)
    {
        _competitionProvider = competitionProvider;
    }

    public Season Handle(
        PrepareSeasonCommand command,
        Career career)
    {
        var availableCompetitions =
            _competitionProvider.GetForSeason(command.SeasonYear);

        var selectedCompetitions =
            availableCompetitions
                .Where(x => command.CompetitionIds.Contains(x.Id))
                .ToList();

        if (selectedCompetitions.Count != command.CompetitionIds.Count)
        {
            throw new InvalidOperationException(
                "Un ou plusieurs concours sélectionnés ne sont pas disponibles pour cette saison.");
        }

        var season = new Season(command.SeasonYear);

        foreach (var competition in selectedCompetitions)
        {
            season.RegisterForCompetition(competition);
        }

        career.SetCurrentSeason(season);

        return season;
    }
}