using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.Rencontre.Domain;

namespace WorldOfSportBoules.Application.CareerManagement.Domain;

public sealed class Tournament
{
    private readonly List<TournamentRound> _rounds = [];

    public IReadOnlyList<TournamentRound> Rounds =>
        _rounds;

    public Tournament(
    CompetitionRound firstRound,
    IReadOnlyList<Team> teams)
    {
        ArgumentNullException.ThrowIfNull(teams);

        var requiredTeamCount =
            GetRequiredTeamCount(firstRound);

        if (teams.Count != requiredTeamCount)
        {
            throw new ArgumentException(
                $"Le tour {firstRound} nécessite " +
                $"{requiredTeamCount} équipes.",
                nameof(teams));
        }

        var drawnTeams = teams.ToList();

        GenerateRound(
            firstRound,
            drawnTeams);
    }

    public TournamentRound CurrentRound =>
        _rounds.Last();

    public TournamentMatch? GetTeamMatch(
        Team team)
    {
        ArgumentNullException.ThrowIfNull(team);

        return CurrentRound.Matches
            .FirstOrDefault(x =>
                x.Team1.Id == team.Id ||
                x.Team2.Id == team.Id);
    }

    public IReadOnlyList<TournamentMatch> GetTeamMatches(
    Team team)
    {
        ArgumentNullException.ThrowIfNull(team);

        return Rounds
            .SelectMany(x => x.Matches)
            .Where(x =>
                x.Team1.Id == team.Id ||
                x.Team2.Id == team.Id)
            .ToList();
    }

    public bool IsCurrentRoundFinished =>
        CurrentRound.Matches.All(x => x.IsPlayed);

    public bool IsFinished =>
        CurrentRound.Round == CompetitionRound.Finale &&
        IsCurrentRoundFinished;

    public bool HasLost(Team team)
    {
        ArgumentNullException.ThrowIfNull(team);

        var match = CurrentRound.Matches
            .FirstOrDefault(x =>
                x.Team1.Id == team.Id ||
                x.Team2.Id == team.Id);

        return match is not null &&
               match.IsPlayed &&
               match.Winner?.Id != team.Id;
    }

    public bool HasWon(Team team)
    {
        ArgumentNullException.ThrowIfNull(team);

        var match = CurrentRound.Matches
            .FirstOrDefault(x =>
                x.Team1.Id == team.Id ||
                x.Team2.Id == team.Id);

        return match is not null &&
               match.IsPlayed &&
               match.Winner?.Id == team.Id;
    }

    public void GenerateNextRound()
    {
        if (!IsCurrentRoundFinished)
        {
            throw new InvalidOperationException(
                "Tous les matchs du tour doivent être terminés.");
        }

        if (IsFinished)
        {
            throw new InvalidOperationException(
                "Le tournoi est déjà terminé.");
        }

        var winners = CurrentRound.Matches
            .Select(x => x.Winner!)
            .ToList();

        GenerateRound(
            GetNextRound(CurrentRound.Round),
            winners);
    }

    public void SetMatchResult(
    TournamentMatch match,
    MatchResult result)
    {
        ArgumentNullException.ThrowIfNull(match);
        ArgumentNullException.ThrowIfNull(result);

        if (!CurrentRound.Matches.Contains(match))
        {
            throw new InvalidOperationException(
                "Ce match n'appartient pas au tour actuel.");
        }

        match.SetResult(result);
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

    private static string GetRoundName(
    CompetitionRound round)
    {
        return round switch
        {
            CompetitionRound.TrenteDeuxiemeDeFinale => "1/32ème",
            CompetitionRound.SeiziemeDeFinale => "1/16ème",
            CompetitionRound.HuitiemeDeFinale => "1/8ème",
            CompetitionRound.QuartDeFinale => "1/4",
            CompetitionRound.DemiFinale => "1/2",
            CompetitionRound.Finale => "finale",

            _ => throw new ArgumentOutOfRangeException(
                nameof(round))
        };
    }

    private static void Shuffle(
    List<Team> teams)
    {
        for (var i = teams.Count - 1; i > 0; i--)
        {
            var j = Random.Shared.Next(i + 1);

            (teams[i], teams[j]) =
                (teams[j], teams[i]);
        }
    }

    private void GenerateRound(
    CompetitionRound round,
    IReadOnlyList<Team> teams)
    {
        var shuffledTeams = teams
            .OrderBy(_ => Random.Shared.Next())
            .ToList();

        var tournamentRound =
            new TournamentRound(round);

        for (var i = 0; i < shuffledTeams.Count; i += 2)
        {
            tournamentRound.AddMatch(
                new TournamentMatch(
                    shuffledTeams[i],
                    shuffledTeams[i + 1]));
        }

        _rounds.Add(tournamentRound);
    }

    private static CompetitionRound GetNextRound(
        CompetitionRound round)
    {
        return round switch
        {
            CompetitionRound.TrenteDeuxiemeDeFinale =>
                CompetitionRound.SeiziemeDeFinale,

            CompetitionRound.SeiziemeDeFinale =>
                CompetitionRound.HuitiemeDeFinale,

            CompetitionRound.HuitiemeDeFinale =>
                CompetitionRound.QuartDeFinale,

            CompetitionRound.QuartDeFinale =>
                CompetitionRound.DemiFinale,

            CompetitionRound.DemiFinale =>
                CompetitionRound.Finale,

            CompetitionRound.Finale =>
                throw new InvalidOperationException(
                    "La finale est le dernier tour."),

            _ => throw new ArgumentOutOfRangeException(
                nameof(round))
        };
    }
}