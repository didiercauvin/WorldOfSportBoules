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

    public int Tir { get; }
    public int Point { get; }

    public Player(
        Guid id,
        string firstName,
        string lastName,
        PlayerCategory category,
        int tir,
        int point)
    {
        if (tir < 0 || tir > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(tir),
                "La compétence de tir doit être comprise entre 0 et 100.");
        }

        if (point < 0 || point > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(point),
                "La compétence de point doit être comprise entre 0 et 100.");
        }

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Category = category;
        Tir = tir;
        Point = point;
    }

    public string FullName =>
        $"{FirstName} {LastName}";
}
