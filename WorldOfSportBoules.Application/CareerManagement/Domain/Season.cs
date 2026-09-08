using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Application.CareerManagement.Domain;

public sealed class Season
{
    private readonly List<ScheduledCompetition> _competitions = [];

    public int Year { get; }

    public DateOnly StartDate { get; }

    public DateOnly EndDate { get; }

    public IReadOnlyList<ScheduledCompetition> Competitions =>
        _competitions;

    public Season(int year)
    {
        Year = year;
        StartDate = new DateOnly(year, 9, 1);
        EndDate = new DateOnly(year + 1, 8, 31);
    }

    public void RegisterForCompetition(
        ScheduledCompetition competition)
    {
        ArgumentNullException.ThrowIfNull(competition);

        if (competition.StartDate < StartDate ||
            competition.EndDate > EndDate)
        {
            throw new InvalidOperationException(
                "Le concours doit avoir lieu pendant la saison.");
        }

        if (_competitions.Any(x => x.Id == competition.Id))
        {
            throw new InvalidOperationException(
                "L'équipe est déjà inscrite à ce concours.");
        }

        if (_competitions.Any(x =>
            x.StartDate <= competition.EndDate &&
            competition.StartDate <= x.EndDate))
        {
            throw new InvalidOperationException(
                "Le concours chevauche un concours auquel l'équipe est déjà inscrite.");
        }

        _competitions.Add(competition);
    }
}