using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Domain;

public sealed class Ball
{
    public Guid Id { get; } = Guid.NewGuid();

    public string PlayerName { get; }

    public Position Position { get; private set; }

    public Ball(string playerName, Position position)
    {
        PlayerName = playerName;
        Position = position;
    }

    public void MoveTo(Position position)
    {
        Position = position;
    }
}
