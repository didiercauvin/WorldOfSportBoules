namespace WorldOfSportBoules.Application.Rencontre.Domain;

public sealed class MatchResult
{
    public int TeamScore { get; }
    public int OpponentScore { get; }

    public bool IsVictory =>
        TeamScore > OpponentScore;

    public MatchResult(
        int teamScore,
        int opponentScore)
    {
        if (teamScore < 0)
            throw new ArgumentOutOfRangeException(nameof(teamScore));

        if (opponentScore < 0)
            throw new ArgumentOutOfRangeException(nameof(opponentScore));

        if (teamScore == opponentScore)
            throw new ArgumentException(
                "Un match ne peut pas se terminer sur une égalité.");

        TeamScore = teamScore;
        OpponentScore = opponentScore;
    }
}