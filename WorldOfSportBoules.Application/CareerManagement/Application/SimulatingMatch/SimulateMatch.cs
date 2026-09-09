using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.Rencontre.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Application.SimulatingMatch;

public sealed record SimulateMatchCommand(
    Guid Team1Id,
    Guid Team2Id);

public sealed class SimulateMatchHandler
{
    public MatchResult Handle(
        SimulateMatchCommand command)
    {
        var team1Score = Random.Shared.Next(0, 14);
        var team2Score = Random.Shared.Next(0, 14);

        // Une égalité n'est pas possible.
        if (team1Score == team2Score)
        {
            if (Random.Shared.Next(2) == 0)
                team1Score++;
            else
                team2Score++;
        }

        return new MatchResult(
            team1Score,
            team2Score);
    }
}
