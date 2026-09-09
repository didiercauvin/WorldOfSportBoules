using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.Rencontre.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Domain;

public sealed class TournamentMatch
{
    public Team Team1 { get; }
    public Team Team2 { get; }

    public MatchResult? Result { get; private set; }

    public Team? Winner { get; private set; }

    public bool IsPlayed =>
        Result is not null;

    public TournamentMatch(
        Team team1,
        Team team2)
    {
        ArgumentNullException.ThrowIfNull(team1);
        ArgumentNullException.ThrowIfNull(team2);

        if (team1.Id == team2.Id)
        {
            throw new ArgumentException(
                "Une équipe ne peut pas être son propre adversaire.");
        }

        Team1 = team1;
        Team2 = team2;
    }

    public void SetResult(MatchResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (IsPlayed)
        {
            throw new InvalidOperationException(
                "Ce match a déjà été joué.");
        }

        Result = result;

        Winner = result.IsTeam1Victory
            ? Team1
            : Team2;
    }
}
