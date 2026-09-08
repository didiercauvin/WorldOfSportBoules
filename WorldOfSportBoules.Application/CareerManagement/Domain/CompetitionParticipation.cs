using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Application.CareerManagement.Domain;

public sealed class CompetitionParticipation
{
    public ScheduledCompetition Competition { get; }
    public Tournament? Tournament { get; }

    public CompetitionParticipation(
        ScheduledCompetition competition)
    {
        ArgumentNullException.ThrowIfNull(competition);

        Competition = competition;

        if (competition.Definition.Format ==
            CompetitionFormat.EliminationDirecte)
        {
            Tournament = new Tournament(
                competition.Definition.FirstRound!.Value);
        }
    }

    public bool IsFinished =>
        Tournament is not null &&
        (Tournament.IsFinished || Tournament.HasLost);

    public bool IsVictory =>
        Tournament is not null &&
        Tournament.IsFinished &&
        !Tournament.HasLost;
}
