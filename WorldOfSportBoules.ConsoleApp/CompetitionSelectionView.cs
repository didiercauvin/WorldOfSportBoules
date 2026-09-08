using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.ConsoleApp;

public sealed class CompetitionSelectionView
{
    public IReadOnlyList<ScheduledCompetition>? Run(
        IReadOnlyList<ScheduledCompetition> competitions,
        int seasonYear)
    {
        var selected = new HashSet<Guid>();
        var index = 0;

        while (true)
        {
            AnsiConsole.Clear();

            Display(
                competitions,
                seasonYear,
                selected,
                index);

            var key = System.Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    index = Math.Max(0, index - 1);
                    break;

                case ConsoleKey.DownArrow:
                    index = Math.Min(
                        competitions.Count - 1,
                        index + 1);
                    break;

                case ConsoleKey.Spacebar:
                    {
                        var competition = competitions[index];

                        if (!selected.Add(competition.Id))
                        {
                            selected.Remove(competition.Id);
                        }

                        break;
                    }

                case ConsoleKey.Enter:
                    return competitions
                        .Where(x => selected.Contains(x.Id))
                        .ToList();

                case ConsoleKey.Escape:
                    return null;
            }
        }
    }

    private static void Display(
        IReadOnlyList<ScheduledCompetition> competitions,
        int seasonYear,
        HashSet<Guid> selected,
        int index)
    {
        AnsiConsole.Write(
            new Rule(
                $"[bold]Préparation de la saison {seasonYear}-{seasonYear + 1}[/]"));

        AnsiConsole.MarkupLine(
            "\nSélectionnez les concours auxquels vous souhaitez participer.");

        AnsiConsole.MarkupLine(
            "[grey]↑ ↓ : déplacer   Espace : sélectionner   " +
            "Entrée : valider   Échap : annuler[/]\n");

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("")
            .AddColumn("")
            .AddColumn("Date")
            .AddColumn("Concours")
            .AddColumn("Lieu")
            .AddColumn("Format")
            .AddColumn("Engagement");

        for (var i = 0; i < competitions.Count; i++)
        {
            var competition = competitions[i];

            var cursor = i == index
                ? "[bold]❯[/]"
                : " ";

            var checkbox = selected.Contains(competition.Id)
                ? "[green][[x]][/]"
                : "[[ ]]";

            var name = i == index
                ? $"[bold]{competition.Definition.Name}[/]"
                : competition.Definition.Name;

            table.AddRow(
                cursor,
                checkbox,
                $"{competition.StartDate:dd/MM/yyyy}",
                name,
                competition.Definition.Location,
                competition.Definition.TeamFormat.ToString(),
                competition.EntryFee.ToString("C"));
        }

        AnsiConsole.Write(table);

        AnsiConsole.MarkupLine(
            $"\n[bold]{selected.Count}[/] concours sélectionné(s)");
    }
}
