using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Application.StartingCompetition;

public sealed record StartCompetitionCommand(
    Guid CompetitionId);

public sealed class StartCompetitionHandler
{
    private readonly IProvideTeam _teamProvider;

    public StartCompetitionHandler(
        IProvideTeam teamProvider)
    {
        _teamProvider = teamProvider;
    }

    public CompetitionParticipation Handle(
        StartCompetitionCommand command,
        Career career)
    {
        ArgumentNullException.ThrowIfNull(career);

        var season = career.CurrentSeason
            ?? throw new InvalidOperationException(
                "La carrière ne possède pas de saison en cours.");

        var competition = season.Competitions
            .FirstOrDefault(x =>
                x.Id == command.CompetitionId);

        if (competition is null)
        {
            throw new InvalidOperationException(
                "Le concours n'est pas inscrit au calendrier de la saison.");
        }

        var teams = _teamProvider
            .GetForCompetition(competition.Definition)
            .ToList();

        if (teams.Any(x => x.Id == career.Team.Id))
        {
            teams.RemoveAll(x => x.Id == career.Team.Id);
        }

        if (competition.Definition.Format ==
            CompetitionFormat.EliminationDirecte)
        {
            var requiredTeamCount =
                GetRequiredTeamCount(
                    competition.Definition.FirstRound!.Value);

            var requiredOpponentCount =
                requiredTeamCount - 1;

            if (teams.Count < requiredOpponentCount)
            {
                throw new InvalidOperationException(
                    $"Il n'y a pas assez d'équipes pour organiser " +
                    $"le {competition.Definition.FirstRound}.");
            }

            teams = teams
                .OrderBy(_ => Random.Shared.Next())
                .Take(requiredOpponentCount)
                .ToList();

            teams.Add(career.Team);
        }

        return season.StartCompetition(
            competition,
            career.Team,
            teams);
    }

    private static int GetRequiredTeamCount(
        CompetitionRound round)
    {
        return round switch
        {
            CompetitionRound.TrenteDeuxiemeDeFinale => 64,
            CompetitionRound.SeiziemeDeFinale => 32,
            CompetitionRound.HuitiemeDeFinale => 16,
            CompetitionRound.QuartDeFinale => 8,
            CompetitionRound.DemiFinale => 4,
            CompetitionRound.Finale => 2,

            _ => throw new ArgumentOutOfRangeException(
                nameof(round))
        };
    }
}
