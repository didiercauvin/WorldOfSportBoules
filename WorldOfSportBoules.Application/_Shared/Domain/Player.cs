using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Application._Shared.Domain;

public sealed class Player
{
    public Guid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public PlayerCategory Category { get; }

    public Player(
        Guid id,
        string firstName,
        string lastName,
        PlayerCategory category)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Category = category;
    }

    public string FullName =>
        $"{FirstName} {LastName}";
}
