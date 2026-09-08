using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Application.Rencontre.Domain;

public sealed class Player
{
    public Guid Id { get; }
    public string Name { get; }

    public Player(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
