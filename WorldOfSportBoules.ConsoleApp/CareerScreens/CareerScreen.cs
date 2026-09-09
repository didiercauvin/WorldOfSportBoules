using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailableCompetitions;
using WorldOfSportBoules.Application.CareerManagement.Application.PreparingSeason;
using WorldOfSportBoules.Application.CareerManagement.Application.SimulatingMatch;
using WorldOfSportBoules.Application.CareerManagement.Application.StartingCompetition;
using WorldOfSportBoules.Application.CareerManagement.Domain;
using static System.Net.Mime.MediaTypeNames;

namespace WorldOfSportBoules.ConsoleApp.CareerScreens;

public sealed class CareerScreen
{
    private string _currentScreen = "Ma saison";

    private readonly GetAvailableCompetitionsHandler
        _getAvailableCompetitionsHandler;

    private readonly ManageTeamScreen _manageTeamScreen;
    private readonly PrepareSeasonScreen _prepareSeasonScreen;
    private readonly SelectCompetitionScreen _selectCompetitionScreen;

    private readonly TournamentScreen _tournamentScreen;
    private CompetitionParticipation? _currentParticipation;

    private readonly PrepareSeasonHandler
        _prepareSeasonHandler;
    private readonly StartCompetitionHandler _startCompetitionHandler;
    private readonly SimulateMatchHandler _simulateMatchHandler;

    public CareerScreen(
        GetAvailableCompetitionsHandler getAvailableCompetitionsHandler,
        PrepareSeasonHandler prepareSeasonHandler,
        ManageTeamScreen manageTeamScreen,
        PrepareSeasonScreen prepareSeasonScreen,
        SelectCompetitionScreen selectCompetitionScreen,
        TournamentScreen tournamentScreen,
        StartCompetitionHandler startCompetitionHandler,
        SimulateMatchHandler simulateMatchHandler)
    {
        _getAvailableCompetitionsHandler =
            getAvailableCompetitionsHandler;

        _prepareSeasonHandler =
            prepareSeasonHandler;

        _manageTeamScreen = manageTeamScreen;
        _prepareSeasonScreen = prepareSeasonScreen;
        _selectCompetitionScreen = selectCompetitionScreen;
        _tournamentScreen = tournamentScreen;
        _startCompetitionHandler = startCompetitionHandler;
        _simulateMatchHandler = simulateMatchHandler;
    }

    public void Run(Career career)
    {
        var seasonYear = DateTime.Now.Year;

        _prepareSeasonScreen.Initialize(seasonYear);
        _manageTeamScreen.Initialize(career);

        var menuItems = new[]
        {
            "Ma saison",
            "Mon équipe",
            "Préparer la saison",
            "Concours",
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


            if (_currentScreen == "Préparer la saison")
            {
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        _prepareSeasonScreen.MoveUp();
                        continue;

                    case ConsoleKey.DownArrow:
                        _prepareSeasonScreen.MoveDown();
                        continue;

                    case ConsoleKey.Spacebar:
                        _prepareSeasonScreen.ToggleSelectedCompetition();
                        continue;

                    case ConsoleKey.Enter:
                        {
                            var competitionIds =
                                _prepareSeasonScreen
                                    .GetSelectedCompetitionIds();

                            if (competitionIds.Count == 0)
                            {
                                continue;
                            }

                            _prepareSeasonHandler.Handle(
                                new PrepareSeasonCommand(
                                    seasonYear,
                                    competitionIds),
                                career);

                            _currentScreen = "Ma saison";
                            continue;
                        }

                    case ConsoleKey.Escape:
                        _currentScreen = "Ma saison";
                        continue;
                }
            }

            if (_currentScreen == "Concours")
            {
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        _selectCompetitionScreen.MoveUp(career);
                        break;

                    case ConsoleKey.DownArrow:
                        _selectCompetitionScreen.MoveDown(career);
                        break;

                    case ConsoleKey.Enter:
                        {
                            var competition =
                                _selectCompetitionScreen
                                    .GetSelectedCompetition(career);

                            if (competition is not null)
                            {
                                // Prochaine étape :
                                // créer la CompetitionParticipation
                            }

                            break;
                        }

                    case ConsoleKey.Escape:
                        _currentScreen = "Ma saison";
                        break;
                }

                continue;
            }

            if (_currentScreen == "Tournoi")
            {
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        // Pour l'instant, rien.
                        // La prochaine étape ouvrira la simulation
                        // du tour sélectionné.
                        break;

                    case ConsoleKey.Escape:
                        _currentScreen = "Concours";
                        break;
                }

                continue;
            }

            // Navigation spécifique à l'écran "Mon équipe"
            if (_currentScreen == "Mon équipe")
            {
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        _manageTeamScreen.MoveUp();
                        continue;

                    case ConsoleKey.DownArrow:
                        _manageTeamScreen.MoveDown();
                        continue;

                    case ConsoleKey.Spacebar:
                        _manageTeamScreen.ToggleSelectedPlayer(career);
                        continue;

                    case ConsoleKey.Enter:
                    case ConsoleKey.Escape:
                        _currentScreen = "Ma saison";
                        continue;
                }
            }

            // Navigation du menu principal
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
                _manageTeamScreen.Render(career),

            "Préparer la saison" =>
                _prepareSeasonScreen.Render(
                    DateTime.Now.Year),

            "Concours" =>
                _selectCompetitionScreen.Render(career),

            "Tournoi" =>
                _currentParticipation is null
                    ? CreateComingSoonContent("Aucun tournoi.")
                    : _tournamentScreen.Render(
                        _currentParticipation,
                        career.Team),

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
                var seasonYear = DateTime.Now.Year;

                _prepareSeasonScreen.Initialize(seasonYear);
                _currentScreen = "Préparer la saison";
                break;

            case "Mon équipe":
                _currentScreen = "Mon équipe";
                _manageTeamScreen.Initialize(career);
                break;

            case "Ma saison":
                _currentScreen = "Ma saison";
                break;

            case "Concours":
                
                    _selectCompetitionScreen.Initialize(career);

                    while (true)
                    {
                        AnsiConsole.Clear();

                        AnsiConsole.Write(
                            _selectCompetitionScreen.Render(career));

                        var key = Console.ReadKey(true).Key;

                        switch (key)
                        {
                            case ConsoleKey.UpArrow:
                                _selectCompetitionScreen.MoveUp(career);
                                break;

                            case ConsoleKey.DownArrow:
                                _selectCompetitionScreen.MoveDown(career);
                                break;

                            case ConsoleKey.Enter:
                                {
                                    var competition =
                                        _selectCompetitionScreen
                                            .GetSelectedCompetition(career);

                                    if (competition is null)
                                        break;

                                    try
                                    {
                                        var participation =
                                            _startCompetitionHandler.Handle(
                                                new StartCompetitionCommand(
                                                    competition.Id),
                                                career);

                                        _currentParticipation = participation;

                                        RunTournament(
                                            participation,
                                            career);
                                    }
                                    catch (InvalidOperationException ex)
                                    {
                                        AnsiConsole.MarkupLine(
                                            $"[red]{ex.Message}[/]");

                                        Console.ReadKey(true);
                                    }

                                    break;
                                }

                            case ConsoleKey.Escape:
                                return;
                        }

                        // Après le retour du tournoi, on revient
                        // à la sélection des concours.
                    }

            case "Budget":
                break;

            case "Recrutement":
                break;
        }
    }

    private void RunTournament(
    CompetitionParticipation participation,
    Career career)
    {
        while (true)
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
                _tournamentScreen.Render(
                    participation,
                    career.Team));

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Escape)
            {
                if (participation.IsFinished)
                {
                    return;
                }

                // Concours en cours : impossible de quitter.
                continue;
            }

            if (key != ConsoleKey.Enter)
            {
                continue;
            }

            var tournament = participation.Tournament;

            if (tournament is null ||
                tournament.IsCurrentRoundFinished)
            {
                continue;
            }

            SimulateCurrentRound(
                tournament,
                career.Team);

            ShowMatchResult(
                tournament,
                career.Team);

            if (tournament.HasLost(career.Team))
            {
                continue;
            }

            if (tournament.IsFinished)
            {
                continue;
            }

            tournament.GenerateNextRound();
        }
    }

    private static void ShowMatchResult(
    Tournament tournament,
    Team team)
    {
        var match = tournament.GetTeamMatch(team);

        if (match is null || match.Result is null)
            return;

        var opponent = match.Team1.Id == team.Id
            ? match.Team2
            : match.Team1;

        var score = match.Team1.Id == team.Id
            ? $"{match.Result.Team1Score} - {match.Result.Team2Score}"
            : $"{match.Result.Team2Score} - {match.Result.Team1Score}";

        AnsiConsole.Clear();

        var color = match.Winner?.Id == team.Id
            ? "green"
            : "red";

        AnsiConsole.MarkupLine(
            $"[bold]{team.Name}[/] " +
            $"[grey]vs[/] " +
            $"[bold]{opponent.Name}[/]");

        AnsiConsole.MarkupLine(
            $"[{color}]{score}[/]");

        AnsiConsole.MarkupLine(
            "\n[grey]Entrée : continuer[/]");

        Console.ReadKey(true);
    }

    private void SimulateCurrentRound(
    Tournament tournament,
    Team team)
    {
        foreach (var match in tournament.CurrentRound.Matches)
        {
            var result = _simulateMatchHandler.Handle(
                new SimulateMatchCommand(
                    match.Team1.Id,
                    match.Team2.Id));

            tournament.SetMatchResult(
                match,
                result);
        }
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

    private static Panel CreateComingSoonContent(
        string title)
    {
        return new Panel(
            new Markup(
                $"[bold]{title}[/]\n\n" +
                "[grey]Cet écran sera implémenté prochainement.[/]"));
    }
}
