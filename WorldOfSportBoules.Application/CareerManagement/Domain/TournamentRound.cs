using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.Rencontre.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Domain;

public sealed class TournamentRound
{
    public CompetitionRound Round { get; }
    public MatchResult? Result { get; private set; }

    public bool IsPlayed =>
        Result is not null;

    public TournamentRound(CompetitionRound round)
    {
        Round = round;
    }

    public void SetResult(MatchResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (IsPlayed)
        {
            throw new InvalidOperationException(
                "Ce tour a déjà été joué.");
        }

        Result = result;
    }
}

public enum CompetitionRound
{
    TrenteDeuxiemeDeFinale,
    SeiziemeDeFinale,
    HuitiemeDeFinale,
    QuartDeFinale,
    DemiFinale,
    Finale
}