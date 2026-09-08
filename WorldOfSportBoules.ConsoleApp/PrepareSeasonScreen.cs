using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailableCompetitions;
using WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailablePlayers;
using WorldOfSportBoules.Application.CareerManagement.Application.PreparingSeason;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.ConsoleApp;

public sealed class PrepareSeasonScreen
{
    private readonly GetAvailableCompetitionsHandler _getAvailableCompetitionsHandler;
    private readonly PrepareSeasonHandler _prepareSeasonHandler;

    public PrepareSeasonScreen(
        GetAvailableCompetitionsHandler getAvailableCompetitionsHandler,
        PrepareSeasonHandler prepareSeasonHandler)
    {
        _getAvailableCompetitionsHandler = getAvailableCompetitionsHandler;
        _prepareSeasonHandler = prepareSeasonHandler;
    }

    public Season Run(
        Career career,
        int seasonYear)
    {
        AnsiConsole.Clear();

        var competitions = SelectCompetitions(seasonYear);

        AnsiConsole.Clear();

        var command = new PrepareSeasonCommand(
            seasonYear,
            career.Team.Players.Select(x => x.Id).ToList(),
            competitions.Select(x => x.Id).ToList());

        var season = _prepareSeasonHandler.Handle(
            command,
            career);

        DisplaySummary(career, season);

        AnsiConsole.Prompt(
            new TextPrompt<string>(
                "\nAppuyez sur [grey]Entrée[/] pour continuer.")
                .AllowEmpty());

        return season;
    }

    private IReadOnlyList<ScheduledCompetition> SelectCompetitions(
        int seasonYear)
    {
        var availableCompetitions =
            _getAvailableCompetitionsHandler.Handle(
                new GetAvailableCompetitionsQuery(seasonYear));

        return AnsiConsole.Prompt(
            new MultiSelectionPrompt<ScheduledCompetition>()
                .Title(
                    $"[bold]Sélectionnez les concours de la saison {seasonYear}-{seasonYear + 1}[/]")
                .InstructionsText(
                    "[grey](Espace pour sélectionner, Entrée pour valider)[/]")
                .PageSize(10)
                .AddChoices(availableCompetitions)
                .UseConverter(competition =>
                    $"{competition.Definition.Name} " +
                    $"[grey]({competition.StartDate:dd/MM/yyyy} · " +
                    $"{competition.Definition.Location} · " +
                    $"{competition.EntryFee:C})[/]"));
    }

    private static void DisplaySummary(
        Career career,
        Season season)
    {
        AnsiConsole.Write(
            new Rule("[bold]Préparation de la saison[/]"));

        AnsiConsole.MarkupLine(
            $"\n[bold]Équipe :[/] {career.Team.Name}");

        AnsiConsole.MarkupLine(
            $"[bold]Catégorie :[/] {career.Team.Category}");

        AnsiConsole.MarkupLine("\n[bold]Joueurs :[/]");

        foreach (var player in career.Team.Players)
        {
            AnsiConsole.MarkupLine(
                $"  • {player.FullName} [grey]({player.Category})[/]");
        }

        AnsiConsole.MarkupLine("\n[bold]Concours :[/]");

        foreach (var competition in season.Competitions)
        {
            AnsiConsole.MarkupLine(
                $"  • {competition.Definition.Name} " +
                $"[grey]({competition.StartDate:dd/MM/yyyy})[/]");
        }
    }
}