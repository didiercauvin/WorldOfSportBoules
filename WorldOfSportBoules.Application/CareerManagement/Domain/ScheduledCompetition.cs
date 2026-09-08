using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Application.CareerManagement.Domain;

public sealed class ScheduledCompetition
{
    public Guid Id { get; }

    public CompetitionDefinition Definition { get; }

    public int SeasonYear { get; }

    public DateOnly StartDate { get; }

    public DateOnly EndDate { get; }

    public decimal EntryFee { get; }

    public ScheduledCompetition(
        Guid id,
        CompetitionDefinition definition,
        int seasonYear,
        DateOnly startDate,
        DateOnly endDate,
        decimal entryFee)
    {
        if (endDate < startDate)
        {
            throw new ArgumentException(
                "La date de fin doit être supérieure ou égale à la date de début.",
                nameof(endDate));
        }

        Id = id;
        Definition = definition;
        SeasonYear = seasonYear;
        StartDate = startDate;
        EndDate = endDate;
        EntryFee = entryFee;
    }

    public int DurationInDays =>
        EndDate.DayNumber - StartDate.DayNumber + 1;
}
