using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailableCompetitions;
using WorldOfSportBoules.Application.CareerManagement.Application.PreparingSeason;
using WorldOfSportBoules.Application.CareerManagement.Domain;
using static System.Net.Mime.MediaTypeNames;

namespace WorldOfSportBoules.ConsoleApp.CareerScreens;

public sealed class CareerScreen
{
    private readonly GetAvailableCompetitionsHandler
        _getAvailableCompetitionsHandler;

    private readonly PrepareSeasonHandler
        _prepareSeasonHandler;

    public CareerScreen(
        GetAvailableCompetitionsHandler getAvailableCompetitionsHandler,
        PrepareSeasonHandler prepareSeasonHandler)
    {
        _getAvailableCompetitionsHandler =
            getAvailableCompetitionsHandler;

        _prepareSeasonHandler =
            prepareSeasonHandler;
    }

    public void Run(Career career)
    {
        var menuItems = new[]
        {
            "Ma saison",
            "Mon équipe",
            "Préparer la saison",
            "Budget",
            "Recrutement",
            "Quitter"
        };

        var selectedMenuIndex = 0;

        while (true)
        {
            RenderMenu(
                career,
                menuItems,
                selectedMenuIndex);

            var key = System.Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedMenuIndex =
                        (selectedMenuIndex - 1 + menuItems.Length)
                        % menuItems.Length;
                    break;

                case ConsoleKey.DownArrow:
                    selectedMenuIndex =
                        (selectedMenuIndex + 1)
                        % menuItems.Length;
                    break;

                case ConsoleKey.Enter:
                    if (menuItems[selectedMenuIndex] == "Quitter")
                    {
                        return;
                    }

                    HandleMenuSelection(
                        career,
                        menuItems[selectedMenuIndex]);

                    break;
            }
        }
    }

    private void RenderMenu(
        Career career,
        string[] menuItems,
        int selectedMenuIndex)
    {
        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("Menu")
                    .Size(28),
                new Layout("Content"));

        UpdateMenu(
            layout,
            career,
            menuItems,
            selectedMenuIndex);

        UpdateContent(
            layout,
            career,
            menuItems[selectedMenuIndex]);

        AnsiConsole.Clear();
        AnsiConsole.Write(layout);
    }

    private static void UpdateMenu(
        Layout layout,
        Career career,
        string[] menuItems,
        int selectedMenuIndex)
    {
        var lines = new List<string>
        {
            $"[bold]{career.Team.Name}[/]",
            $"[grey]Manager : {career.ManagerName}[/]",
            ""
        };

        for (var i = 0; i < menuItems.Length; i++)
        {
            var selected = i == selectedMenuIndex;

            var prefix = selected
                ? "[bold]❯[/] "
                : "  ";

            var item = selected
                ? $"[bold]{menuItems[i]}[/]"
                : menuItems[i];

            lines.Add(prefix + item);
        }

        var panel = new Panel(
            new Markup(string.Join("\n", lines)))
        {
            Header = new PanelHeader("[bold]Carrière[/]"),
            Border = BoxBorder.Rounded
        };

        layout["Menu"].Update(panel);
    }

    private void UpdateContent(
    Layout layout,
    Career career,
    string selectedItem)
    {
        var content = selectedItem switch
        {
            "Ma saison" =>
                CreateSeasonContent(career),

            "Mon équipe" =>
                CreateTeamContent(career),

            "Préparer la saison" =>
                CreatePrepareSeasonContent(career),

            "Budget" =>
                CreateComingSoonContent("Budget"),

            "Recrutement" =>
                CreateComingSoonContent("Recrutement"),

            "Quitter" =>
                CreateComingSoonContent("Quitter"),

            _ =>
                CreateComingSoonContent("")
        };

        layout["Content"].Update(content);
    }

    private void HandleMenuSelection(
        Career career,
        string selectedItem)
    {
        switch (selectedItem)
        {
            case "Préparer la saison":
                PrepareSeason(career);
                break;

            case "Ma saison":
                break;

            case "Mon équipe":
                break;

            case "Budget":
                break;

            case "Recrutement":
                break;
        }
    }

    private void PrepareSeason(Career career)
    {
        var seasonYear = DateTime.Now.Year;

        var competitions =
            _getAvailableCompetitionsHandler.Handle(
                new GetAvailableCompetitionsQuery(
                    seasonYear));

        var selected = SelectCompetitions(
            competitions,
            seasonYear);

        if (selected is null)
        {
            return;
        }

        var command = new PrepareSeasonCommand(
            seasonYear,
            selected
                .Select(x => x.Id)
                .ToList());

        _prepareSeasonHandler.Handle(
            command,
            career);
    }

    private static IReadOnlyList<ScheduledCompetition>?
        SelectCompetitions(
            IReadOnlyList<ScheduledCompetition> competitions,
            int seasonYear)
    {
        if (competitions.Count == 0)
        {
            AnsiConsole.Clear();

            AnsiConsole.MarkupLine(
                "[yellow]Aucun concours disponible.[/]");

            System.Console.ReadKey(true);

            return null;
        }

        var selected = new HashSet<Guid>();
        var index = 0;

        while (true)
        {
            var layout = new Layout("Root")
                .SplitColumns(
                    new Layout("Menu")
                        .Size(28),
                    new Layout("Content"));

            layout["Menu"].Update(
                new Panel(
                    new Markup(
                        "[bold]Préparation de la saison[/]\n\n" +
                        "[grey]↑ ↓[/] Déplacer\n" +
                        "[grey]Espace[/] Sélectionner\n" +
                        "[grey]Entrée[/] Valider\n" +
                        "[grey]Échap[/] Annuler"))
                {
                    Header = new PanelHeader("[bold]Navigation[/]"),
                    Border = BoxBorder.Rounded
                });

            layout["Content"].Update(
                CreateCompetitionSelectionContent(
                    competitions,
                    seasonYear,
                    selected,
                    index));

            AnsiConsole.Clear();
            AnsiConsole.Write(layout);

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

    private static Panel CreateCompetitionSelectionContent(
        IReadOnlyList<ScheduledCompetition> competitions,
        int seasonYear,
        HashSet<Guid> selected,
        int index)
    {
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
                : "";

            var checkbox = selected.Contains(competition.Id)
                ? "[green][[x]][/]"
                : "[[ ]]";

            var name = i == index
                ? $"[bold]{competition.Definition.Name}[/]"
                : competition.Definition.Name;

            table.AddRow(
                cursor,
                checkbox,
                competition.StartDate.ToString("dd/MM/yyyy"),
                name,
                competition.Definition.Location,
                competition.Definition.TeamFormat.ToString(),
                competition.EntryFee.ToString("C"));
        }

        var content = new Rows(
            new Markup(
                $"[bold]Saison {seasonYear}-{seasonYear + 1}[/]\n\n" +
                "Sélectionnez les concours auxquels " +
                "vous souhaitez participer.\n\n" +
                "[grey]↑ ↓ : déplacer   " +
                "Espace : sélectionner   " +
                "Entrée : valider   " +
                "Échap : annuler[/]\n"),
            table,
            new Markup(
                $"\n[bold]{selected.Count}[/] " +
                "concours sélectionné(s)")
        );

        return new Panel(content)
        {
            Header = new PanelHeader(
                "[bold]Calendrier[/]"),
            Border = BoxBorder.Rounded
        };
    }

    private static Panel CreateSeasonContent(
        Career career)
    {
        if (career.CurrentSeason is null)
        {
            return new Panel(
                new Markup(
                    "[bold]Aucune saison préparée.[/]\n\n" +
                    "Utilisez [bold]Préparer la saison[/] " +
                    "pour construire votre calendrier."));
        }

        var season = career.CurrentSeason;

        var lines = new List<string>
        {
            $"[bold]Saison {season.Year}-{season.Year + 1}[/]",
            "",
            $"Période : " +
            $"{season.StartDate:dd/MM/yyyy} → " +
            $"{season.EndDate:dd/MM/yyyy}",
            "",
            $"Concours inscrits : " +
            $"[bold]{season.Competitions.Count}[/]",
            ""
        };

        foreach (var competition in season.Competitions
                     .OrderBy(x => x.StartDate))
        {
            lines.Add(
                $"• {competition.StartDate:dd/MM/yyyy} — " +
                $"{competition.Definition.Name}");
        }

        return new Panel(
            new Markup(string.Join("\n", lines)));
    }

    private static Panel CreateTeamContent(
        Career career)
    {
        var lines = new List<string>
        {
            $"[bold]{career.Team.Name}[/]",
            "",
            $"Catégorie : [bold]{career.Team.Category}[/]",
            "",
            "[bold]Effectif[/]",
            ""
        };

        foreach (var player in career.Team.Players)
        {
            lines.Add(
                $"• {player.FullName} " +
                $"[grey]({player.Category})[/]");
        }

        return new Panel(
            new Markup(string.Join("\n", lines)));
    }

    private static Panel CreateComingSoonContent(
        string title)
    {
        return new Panel(
            new Markup(
                $"[bold]{title}[/]\n\n" +
                "[grey]Cet écran sera implémenté prochainement.[/]"));
    }

    private static Panel CreatePrepareSeasonContent(
    Career career)
    {
        var seasonYear = DateTime.Now.Year;

        var content = new Markup(
            $"[bold]Préparer la saison {seasonYear}-{seasonYear + 1}[/]\n\n" +
            "Construisez votre calendrier en sélectionnant " +
            "les concours auxquels vous souhaitez participer.\n\n" +
            "[grey]Appuyez sur Entrée pour commencer.[/]");

        return new Panel(content)
        {
            Header = new PanelHeader("[bold]Préparation[/]"),
            Border = BoxBorder.Rounded
        };
    }
}
