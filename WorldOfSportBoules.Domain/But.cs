using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Domain;

public sealed class But
{
    public Position Position { get; private set; }

    public But(Position position)
    {
        Position = position;
    }

    public void MoveTo(Position position)
    {
        Position = position;
    }
}
