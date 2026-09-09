using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.Rencontre.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Domain;

public sealed class TournamentRound
{
    private readonly List<TournamentMatch> _matches = [];

    public CompetitionRound Round { get; }

    public IReadOnlyList<TournamentMatch> Matches =>
        _matches;

    public TournamentRound(
        CompetitionRound round)
    {
        Round = round;
    }

    public void AddMatch(
        TournamentMatch match)
    {
        ArgumentNullException.ThrowIfNull(match);

        _matches.Add(match);
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