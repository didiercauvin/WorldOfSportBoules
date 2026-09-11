using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Application.Rencontre.Domain;

public sealed class MeneResult
{
    public int Team1Points { get; }
    public int Team2Points { get; }

    public MeneResult(
        int team1Points,
        int team2Points)
    {
        if (team1Points < 0)
            throw new ArgumentOutOfRangeException(nameof(team1Points));

        if (team2Points < 0)
            throw new ArgumentOutOfRangeException(nameof(team2Points));

        if (team1Points > 0 && team2Points > 0)
        {
            throw new ArgumentException(
                "Une mène ne peut pas donner des points aux deux équipes.");
        }

        Team1Points = team1Points;
        Team2Points = team2Points;
    }
}
