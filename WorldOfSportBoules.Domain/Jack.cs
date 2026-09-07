using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Domain;

public sealed class Jack
{
    public Position Position { get; private set; }

    public Jack(Position position)
    {
        Position = position;
    }

    public void MoveTo(Position position)
    {
        Position = position;
    }
}
