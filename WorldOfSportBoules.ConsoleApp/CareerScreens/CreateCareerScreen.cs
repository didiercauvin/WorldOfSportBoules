using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Application.CreatingCareer;
using WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailablePlayers;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.ConsoleApp.CareerScreens;

public sealed class CreateCareerScreen
{
    private readonly CreateCareerHandler _handler;
    private readonly GetAvailablePlayersHandler _getAvailablePlayersHandler;

    public CreateCareerScreen(
        CreateCareerHandler handler,
        GetAvailablePlayersHandler getAvailablePlayersHandler)
    {
        _handler = handler;
        _getAvailablePlayersHandler = getAvailablePlayersHandler;
    }

    public Career Run()
    {
        AnsiConsole.Clear();

        var managerName = AnsiConsole.Ask<string>(
            "Nom du manager :");

        var teamName = AnsiConsole.Ask<string>(
            "Nom de l'équipe :");

        var category = AnsiConsole.Prompt(
            new SelectionPrompt<TeamCategory>()
                .Title("Catégorie de départ :")
                .AddChoices(TeamCategory.M4));

        var career = _handler.Handle(
            new CreateCareerCommand(
                managerName,
                teamName,
                category));

        AddPlayers(career.Team);

        return career;
    }

    private void AddPlayers(Team team)
    {
        var availablePlayers =
            _getAvailablePlayersHandler.Handle(
                new GetAvailablePlayersQuery(
                    team.Category));

        var selectedPlayers = AnsiConsole.Prompt(
            new MultiSelectionPrompt<Player>()
                .Title(
                    $"[bold]Sélectionnez les joueurs de l'équipe {team.Category}[/]")
                .InstructionsText(
                    "[grey](Espace pour sélectionner, Entrée pour valider)[/]")
                .PageSize(10)
                .AddChoices(availablePlayers)
                .UseConverter(player =>
                    $"{player.FullName} [grey]({player.Category})[/]"));

        if (selectedPlayers.Count != 4)
        {
            AnsiConsole.MarkupLine(
                "\n[red]Vous devez sélectionner exactement 4 joueurs.[/]");

            AnsiConsole.Prompt(
                new TextPrompt<string>(
                    "Appuyez sur [grey]Entrée[/] pour recommencer.")
                    .AllowEmpty());

            AddPlayers(team);

            return;
        }

        foreach (var player in selectedPlayers)
        {
            team.AddPlayer(player);
        }
    }
}