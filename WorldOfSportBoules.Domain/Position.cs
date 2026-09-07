using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Domain;

public readonly record struct Position(double X, double Y)
{
    public double DistanceTo(Position other)
    {
        var dx = X - other.X;
        var dy = Y - other.Y;

        return Math.Sqrt(dx * dx + dy * dy);
    }
}