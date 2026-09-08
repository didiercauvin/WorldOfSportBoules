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

    public bool IsFinished =>
        _rounds.Last().IsPlayed;

    public bool HasLost =>
        _rounds.Any(x =>
            x.IsPlayed &&
            !x.Result!.IsVictory);

    public TournamentRound? NextRound =>
        _rounds.FirstOrDefault(x => !x.IsPlayed);

    public Tournament(
        CompetitionRound firstRound)
    {
        var rounds = GetRoundsFrom(firstRound);

        _rounds.AddRange(
            rounds.Select(x => new TournamentRound(x)));
    }

    public void PlayNextRound(MatchResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (IsFinished)
        {
            throw new InvalidOperationException(
                "Le tournoi est déjà terminé.");
        }

        if (HasLost)
        {
            throw new InvalidOperationException(
                "L'équipe a été éliminée.");
        }

        var nextRound = NextRound;

        if (nextRound is null)
        {
            throw new InvalidOperationException(
                "Il n'y a plus de tour à jouer.");
        }

        nextRound.SetResult(result);
    }

    private static IReadOnlyList<CompetitionRound> GetRoundsFrom(
        CompetitionRound firstRound)
    {
        var allRounds = new[]
        {
            CompetitionRound.TrenteDeuxiemeDeFinale,
            CompetitionRound.SeiziemeDeFinale,
            CompetitionRound.HuitiemeDeFinale,
            CompetitionRound.QuartDeFinale,
            CompetitionRound.DemiFinale,
            CompetitionRound.Finale
        };

        var index = Array.IndexOf(allRounds, firstRound);

        if (index < 0)
        {
            throw new ArgumentException(
                "Le tour initial est invalide.",
                nameof(firstRound));
        }

        return allRounds[index..];
    }
}
