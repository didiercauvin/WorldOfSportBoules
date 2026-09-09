using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Application.CareerManagement.Domain;

public sealed class Season
{
    private readonly List<ScheduledCompetition> _competitions = [];

    private readonly List<CompetitionParticipation> _participations = [];

    public IReadOnlyList<CompetitionParticipation> Participations =>
        _participations;

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

    public CompetitionParticipation StartCompetition(
    ScheduledCompetition competition,
    Team team,
    IReadOnlyList<Team> teams)
    {
        ArgumentNullException.ThrowIfNull(competition);
        ArgumentNullException.ThrowIfNull(teams);

        if (!_competitions.Any(x => x.Id == competition.Id))
        {
            throw new InvalidOperationException(
                "L'équipe n'est pas inscrite à ce concours.");
        }

        var existingParticipation = _participations
            .FirstOrDefault(x =>
                x.Competition.Id == competition.Id);

        if (existingParticipation is not null)
        {
            return existingParticipation;
        }

        var nextCompetition = NextCompetition;

        if (nextCompetition is null)
        {
            throw new InvalidOperationException(
                "Tous les concours de la saison sont terminés.");
        }

        if (nextCompetition.Id != competition.Id)
        {
            throw new InvalidOperationException(
                $"Le prochain concours est {nextCompetition.Definition.Name}.");
        }

        var participation = new CompetitionParticipation(
            competition,
            team,
            teams);

        _participations.Add(participation);

        return participation;
    }

    public ScheduledCompetition? NextCompetition
    {
        get
        {
            var orderedCompetitions = _competitions
                .OrderBy(x => x.StartDate)
                .ToList();

            foreach (var competition in orderedCompetitions)
            {
                var participation = _participations
                    .FirstOrDefault(x =>
                        x.Competition.Id == competition.Id);

                if (participation is null)
                    return competition;

                if (!participation.IsFinished)
                    return competition;
            }

            return null;
        }
    }
}