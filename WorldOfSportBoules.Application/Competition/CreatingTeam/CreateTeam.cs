using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.Application.Competition.CreatingTeam;

public sealed record CreateTeamCommand(
    string Name,
    TeamCategory Category,
    IReadOnlyList<Guid> PlayerIds);

public sealed class CreateTeamHandler
{
    private readonly IProvidePlayer _playerProvider;

    public CreateTeamHandler(
        IProvidePlayer playerProvider)
    {
        _playerProvider = playerProvider;
    }

    public Team Handle(
        CreateTeamCommand command)
    {
        ArgumentNullException.ThrowIfNull(command.PlayerIds);

        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new ArgumentException(
                "Le nom de l'équipe est obligatoire.",
                nameof(command.Name));
        }

        var availablePlayers =
            _playerProvider.GetForCategory(
                command.Category);

        var players =
            availablePlayers
                .Where(x => command.PlayerIds.Contains(x.Id))
                .ToList();

        var team = new Team(
            Guid.CreateVersion7(),
            command.Name,
            command.Category);

        foreach (var player in players)
        {
            team.AddPlayer(player);
        }

        //team.ValidateComposition();

        return team;
    }
}