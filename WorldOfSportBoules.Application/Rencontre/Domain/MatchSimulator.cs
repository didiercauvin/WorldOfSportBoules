using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.Application.Rencontre.Domain;

public sealed class MatchSimulator
{
    private const int WinningScore = 13;

    public MatchSimulationResult Simulate(
        Team team1,
        Team team2)
    {
        ArgumentNullException.ThrowIfNull(team1);
        ArgumentNullException.ThrowIfNull(team2);

        var team1Strength =
            CalculateTeamStrength(team1);

        var team2Strength =
            CalculateTeamStrength(team2);

        var team1Score = 0;
        var team2Score = 0;

        var menes = new List<MeneResult>();

        while (team1Score < WinningScore &&
               team2Score < WinningScore)
        {
            var mene = SimulateMene(
                team1Strength,
                team2Strength);

            menes.Add(mene);

            team1Score += mene.Team1Points;
            team2Score += mene.Team2Points;

            if (team1Score > WinningScore)
            {
                team1Score = WinningScore;
            }

            if (team2Score > WinningScore)
            {
                team2Score = WinningScore;
            }
        }

        var result =
            new MatchResult(
                team1Score,
                team2Score);

        return new MatchSimulationResult(
            result,
            menes);
    }

    private static double CalculateTeamStrength(
        Team team)
    {
        if (team.Players.Count == 0)
            return 0;

        return team.Players
            .Select(CalculatePlayerStrength)
            .Average();
    }

    private static double CalculatePlayerStrength(
        Player player)
    {
        return player.Tir * 0.5 +
               player.Point * 0.5;
    }

    private static MeneResult SimulateMene(
        double team1Strength,
        double team2Strength)
    {
        var team1Probability =
            team1Strength /
            (team1Strength + team2Strength);

        var team1Wins =
            Random.Shared.NextDouble() <
            team1Probability;

        var points =
            Random.Shared.Next(1, 4);

        return team1Wins
            ? new MeneResult(points, 0)
            : new MeneResult(0, points);
    }
}
