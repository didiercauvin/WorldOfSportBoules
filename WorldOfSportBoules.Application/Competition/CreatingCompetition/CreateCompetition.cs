using System;
using System.Collections.Generic;
using System.Text;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.Application.Competition.CreatingCompetition;

public sealed record CreateCompetitionCommand(
    TeamCategory Category,
    CompetitionRound FirstRound,
    IReadOnlyList<Guid> TeamIds,
    Team PlayerTeam);

public sealed record CreatedCompetition(
    Tournament Tournament,
    Team PlayerTeam);

public sealed class CreateCompetitionHandler
{
    private readonly IProvideTeam _teamProvider;

    public CreateCompetitionHandler(
        IProvideTeam teamProvider)
    {
        _teamProvider = teamProvider;
    }

    public Tournament Handle(
        CreateCompetitionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command.TeamIds);

        var availableTeams =
            _teamProvider.GetForCategory(
                command.Category);

        var selectedTeams =
            availableTeams
                .Where(x => command.TeamIds.Contains(x.Id))
                .ToList();

        selectedTeams.Add(command.PlayerTeam);

        var requiredTeamCount =
            GetRequiredTeamCount(
                command.FirstRound);

        if (selectedTeams.Count != requiredTeamCount)
        {
            throw new InvalidOperationException(
                $"Il faut sélectionner exactement " +
                $"{requiredTeamCount} équipes.");
        }

        return new Tournament(
            command.FirstRound,
            selectedTeams);
    }

    private static int GetRequiredTeamCount(
        CompetitionRound round)
    {
        return round switch
        {
            CompetitionRound.TrenteDeuxiemeDeFinale => 64,
            CompetitionRound.SeiziemeDeFinale => 32,
            CompetitionRound.HuitiemeDeFinale => 16,
            CompetitionRound.QuartDeFinale => 8,
            CompetitionRound.DemiFinale => 4,
            CompetitionRound.Finale => 2,

            _ => throw new ArgumentOutOfRangeException(
                nameof(round))
        };
    }
}
