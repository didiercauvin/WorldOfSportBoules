namespace WorldOfSportBoules.Application.Rencontre.Domain;

public sealed class MatchResult
{
    public int Team1Score { get; }
    public int Team2Score { get; }

    public bool IsTeam1Victory =>
        Team1Score > Team2Score;

    public MatchResult(
        int team1Score,
        int team2Score)
    {
        if (team1Score < 0)
            throw new ArgumentOutOfRangeException(
                nameof(team1Score));

        if (team2Score < 0)
            throw new ArgumentOutOfRangeException(
                nameof(team2Score));

        if (team1Score == team2Score)
        {
            throw new ArgumentException(
                "Un match ne peut pas se terminer sur une égalité.");
        }

        Team1Score = team1Score;
        Team2Score = team2Score;
    }
}