using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Application.CareerManagement.Domain;

public sealed class CompetitionDefinition
{
    public Guid Id { get; }

    public string Name { get; }

    public string Location { get; }

    public int DistanceKm { get; }
    public CompetitionRound? FirstRound { get; }

    public CompetitionType Type { get; }
    public CompetitionFormat Format { get; }

    public CompetitionTeamFormat TeamFormat { get; }
    public CompetitionCategory Category { get; }

    public CompetitionDefinition(
        Guid id,
        string name,
        string location,
        int distanceKm,
        CompetitionType type,
        CompetitionFormat format,
        CompetitionTeamFormat teamFormat,
        CompetitionCategory category,
        CompetitionRound firstRound)
    {
        Id = id;
        Name = name;
        Location = location;
        DistanceKm = distanceKm;
        Type = type;
        Format = format;
        TeamFormat = teamFormat;
        Category = category;
        FirstRound = firstRound;
    }
}

public enum CompetitionType
{
    Loisir,
    Promotion
}

public enum CompetitionFormat
{
    Poules,
    EliminationDirecte
}

public enum CompetitionTeamFormat
{
    Quadrette
}

public enum CompetitionCategory
{
    M1,
    M2,
    M3M4,
    ToutesCategories
}