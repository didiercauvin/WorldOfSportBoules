using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.ConsoleApp;

public sealed class ViewSeasonScreen
{
    public void Run(Career career)
    {
        AnsiConsole.Clear();

        var season = career.CurrentSeason;

        if (season is null)
        {
            AnsiConsole.MarkupLine(
                "[yellow]Aucune saison n'a encore été préparée.[/]");

            AnsiConsole.Prompt(
                new TextPrompt<string>(
                    "\nAppuyez sur [grey]Entrée[/] pour continuer.")
                    .AllowEmpty());

            return;
        }

        DisplaySeason(career, season);

        AnsiConsole.Prompt(
            new TextPrompt<string>(
                "\nAppuyez sur [grey]Entrée[/] pour continuer.")
                .AllowEmpty());
    }

    private static void DisplaySeason(
        Career career,
        Season season)
    {
        AnsiConsole.Write(
            new Rule(
                $"[bold]Saison {season.Year}-{season.Year + 1}[/]"));

        AnsiConsole.MarkupLine(
            $"\n[bold]Équipe :[/] {career.Team.Name}");

        AnsiConsole.MarkupLine(
            $"[bold]Catégorie :[/] {career.Team.Category}");

        AnsiConsole.MarkupLine(
            $"[bold]Période :[/] " +
            $"{season.StartDate:dd/MM/yyyy} → {season.EndDate:dd/MM/yyyy}");

        AnsiConsole.MarkupLine("\n[bold]Effectif[/]");

        foreach (var player in career.Team.Players)
        {
            AnsiConsole.MarkupLine(
                $"  • {player.FullName} [grey]({player.Category})[/]");
        }

        AnsiConsole.MarkupLine("\n[bold]Concours[/]");

        if (season.Competitions.Count == 0)
        {
            AnsiConsole.MarkupLine(
                "  [grey]Aucun concours sélectionné.[/]");
            return;
        }

        var table = new Table();

        table.AddColumn("Date");
        table.AddColumn("Concours");
        table.AddColumn("Lieu");
        table.AddColumn("Format");
        table.AddColumn("Engagement");

        foreach (var competition in season.Competitions
                     .OrderBy(x => x.StartDate))
        {
            table.AddRow(
                competition.StartDate.ToString("dd/MM/yyyy"),
                competition.Definition.Name,
                competition.Definition.Location,
                competition.Definition.TeamFormat.ToString(),
                competition.EntryFee.ToString("C"));
        }

        AnsiConsole.Write(table);
    }
}
