using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Application.CareerManagement.Domain;

public sealed class CompetitionParticipation
{
    public ScheduledCompetition Competition { get; }

    public Team Team { get; }

    public Tournament? Tournament { get; }

    public CompetitionParticipation(
        ScheduledCompetition competition,
        Team team,
        IReadOnlyList<Team> teams)
    {
        ArgumentNullException.ThrowIfNull(competition);
        ArgumentNullException.ThrowIfNull(team);
        ArgumentNullException.ThrowIfNull(teams);

        Competition = competition;
        Team = team;

        if (competition.Definition.Format ==
            CompetitionFormat.EliminationDirecte)
        {
            Tournament = new Tournament(
                competition.Definition.FirstRound!.Value,
                teams);
        }
    }

    public bool IsFinished
    {
        get
        {
            if (Tournament is null)
                return true;

            return Tournament.IsFinished ||
                   Tournament.HasLost(Team);
        }
    }

    public CompetitionRound? EliminatedAt
    {
        get
        {
            if (Tournament is null)
                return null;

            var match = Tournament.Rounds
                .SelectMany(x => x.Matches)
                .FirstOrDefault(x =>
                    x.IsPlayed &&
                    (x.Team1.Id == Team.Id ||
                     x.Team2.Id == Team.Id) &&
                    x.Winner?.Id != Team.Id);

            return match is null
                ? null
                : Tournament.Rounds
                    .First(x => x.Matches.Contains(match))
                    .Round;
        }
    }
}