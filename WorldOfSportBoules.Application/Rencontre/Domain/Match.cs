using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Application.Rencontre.Domain;

public sealed class Match
{
    private readonly List<Player> _players;

    public But? But { get; private set; }

    public IReadOnlyList<Player> Players => new ReadOnlyCollection<Player>(_players);

    public Match(IEnumerable<Player> players)
    {
        _players = players.ToList();
    }

    public void PlacerLeBut(But but)
    {
        But = but;
    }
}
