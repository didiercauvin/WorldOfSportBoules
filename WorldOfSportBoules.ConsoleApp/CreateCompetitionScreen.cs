using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Domain;
using WorldOfSportBoules.Application.Competition.CreatingCompetition;

namespace WorldOfSportBoules.ConsoleApp;

public sealed class CreateCompetitionScreen
{
    private readonly CreateCompetitionHandler _handler;
    private readonly IProvideTeam _teamProvider;

    public CreateCompetitionScreen(
        CreateCompetitionHandler handler,
        IProvideTeam teamProvider)
    {
        _handler = handler;
        _teamProvider = teamProvider;
    }

    public CreatedCompetition? Run()
    {
        var category = SelectCategory();

        if (category is null)
            return null;

        var firstRound = SelectFirstRound();

        if (firstRound is null)
            return null;

        var teams = SelectTeams(
            category.Value,
            firstRound.Value);

        if (teams is null)
            return null;

        var playerTeam = SelectPlayerTeam(teams);

        if (playerTeam is null)
            return null;

        try
        {
            var tournament =
                _handler.Handle(
                    new CreateCompetitionCommand(
                        category.Value,
                        firstRound.Value,
                        teams.Select(x => x.Id).ToList()));

            return new CreatedCompetition(
                tournament,
                playerTeam);
        }
        catch (Exception exception)
        {
            AnsiConsole.Clear();

            AnsiConsole.MarkupLine(
                $"[red]{exception.Message}[/]");

            AnsiConsole.MarkupLine(
                "\n[grey]Entrée : continuer[/]");

            System.Console.ReadKey(true);

            return null;
        }
    }

    private static Team? SelectPlayerTeam(
    IReadOnlyList<Team> teams)
    {
        var selectedIndex = 0;

        while (true)
        {
            AnsiConsole.Clear();

            AnsiConsole.MarkupLine(
                "[bold]Créer son concours[/]\n");

            AnsiConsole.MarkupLine(
                "[bold]Votre équipe[/]\n");

            AnsiConsole.MarkupLine(
                "[grey]Quelle équipe voulez-vous contrôler ?[/]\n");

            for (var i = 0; i < teams.Count; i++)
            {
                var cursor =
                    i == selectedIndex
                        ? "[yellow]❯[/]"
                        : " ";

                AnsiConsole.MarkupLine(
                    $"{cursor} {teams[i].Name}");
            }

            AnsiConsole.MarkupLine(
                "\n[grey]↑ ↓ : déplacer   " +
                "Entrée : choisir   " +
                "Échap : retour[/]");

            var key =
                System.Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex =
                        Math.Max(
                            0,
                            selectedIndex - 1);
                    break;

                case ConsoleKey.DownArrow:
                    selectedIndex =
                        Math.Min(
                            teams.Count - 1,
                            selectedIndex + 1);
                    break;

                case ConsoleKey.Enter:
                    return teams[selectedIndex];

                case ConsoleKey.Escape:
                    return null;
            }
        }
    }

    private static TeamCategory? SelectCategory()
    {
        while (true)
        {
            AnsiConsole.Clear();

            AnsiConsole.MarkupLine(
                "[bold]Créer son concours[/]\n");

            AnsiConsole.MarkupLine(
                "[bold]Catégorie[/]\n");

            AnsiConsole.MarkupLine(
                "[yellow]❯ M4[/]");

            AnsiConsole.MarkupLine(
                "\n[grey]Entrée : continuer   " +
                "Échap : retour[/]");

            var key =
                System.Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.Enter:
                    return TeamCategory.M4;

                case ConsoleKey.Escape:
                    return null;
            }
        }
    }

    private static CompetitionRound? SelectFirstRound()
    {
        var rounds = new[]
        {
            CompetitionRound.TrenteDeuxiemeDeFinale,
            CompetitionRound.SeiziemeDeFinale,
            CompetitionRound.HuitiemeDeFinale,
            CompetitionRound.QuartDeFinale,
            CompetitionRound.DemiFinale,
            CompetitionRound.Finale
        };

        var selectedIndex = 0;

        while (true)
        {
            AnsiConsole.Clear();

            AnsiConsole.MarkupLine(
                "[bold]Créer son concours[/]\n");

            AnsiConsole.MarkupLine(
                "[bold]Premier tour[/]\n");

            for (var i = 0; i < rounds.Length; i++)
            {
                var prefix =
                    i == selectedIndex
                        ? "[yellow]❯[/]"
                        : " ";

                AnsiConsole.MarkupLine(
                    $"{prefix} {GetRoundName(rounds[i])} " +
                    $"[grey]({GetTeamCount(rounds[i])} équipes)[/]");
            }

            AnsiConsole.MarkupLine(
                "\n[grey]↑ ↓ : déplacer   " +
                "Entrée : continuer   " +
                "Échap : retour[/]");

            var key =
                System.Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex =
                        Math.Max(0, selectedIndex - 1);
                    break;

                case ConsoleKey.DownArrow:
                    selectedIndex =
                        Math.Min(
                            rounds.Length - 1,
                            selectedIndex + 1);
                    break;

                case ConsoleKey.Enter:
                    return rounds[selectedIndex];

                case ConsoleKey.Escape:
                    return null;
            }
        }
    }

    private List<Team>? SelectTeams(
    TeamCategory category,
    CompetitionRound firstRound)
    {
        var teams =
            _teamProvider
                .GetForCategory(category)
                .ToList();

        var requiredCount =
            GetTeamCount(firstRound);

        if (teams.Count < requiredCount)
        {
            AnsiConsole.Clear();

            AnsiConsole.MarkupLine(
                $"[red]Il n'y a que {teams.Count} équipes " +
                $"disponibles pour {requiredCount} nécessaires.[/]");

            AnsiConsole.MarkupLine(
                "\n[grey]Entrée : continuer[/]");

            System.Console.ReadKey(true);

            return null;
        }

        const int columns = 3;
        const int rows = 5;
        const int pageSize = columns * rows;

        var selected = new HashSet<Guid>();
        var selectedIndex = 0;

        while (true)
        {
            var currentPage =
                selectedIndex / pageSize;

            var pageStart =
                currentPage * pageSize;

            var pageEnd =
                Math.Min(
                    pageStart + pageSize,
                    teams.Count);

            AnsiConsole.Clear();

            AnsiConsole.MarkupLine(
                $"[bold]Sélection des équipes[/]\n" +
                $"Catégorie : {category}\n" +
                $"Premier tour : {GetRoundName(firstRound)}\n" +
                $"Équipes : {selected.Count}/{requiredCount}\n");

            // Grille : 5 lignes × 3 colonnes
            for (var row = 0; row < rows; row++)
            {
                var line = "";

                for (var column = 0; column < columns; column++)
                {
                    var index =
                        pageStart +
                        row * columns +
                        column;

                    if (index >= pageEnd)
                        continue;

                    var team = teams[index];

                    var cursor =
                        index == selectedIndex
                            ? "[yellow]❯[/]"
                            : " ";

                    var checkbox =
                        selected.Contains(team.Id)
                            ? "[green]X[/]"
                            : "[grey] [/]";

                    var text =
                        $"{cursor} {checkbox} {team.Name}";

                    line += text.PadRight(28);
                }

                AnsiConsole.MarkupLine(line);
            }

            var totalPages =
                (int)Math.Ceiling(
                    teams.Count / (double)pageSize);

            AnsiConsole.MarkupLine(
                $"\n[grey]Page {currentPage + 1}/{totalPages}[/]");

            AnsiConsole.MarkupLine(
                "\n[grey]↑ ↓ ← → : déplacer   " +
                "Espace : sélectionner   " +
                "R : sélection aléatoire   " +
                "PgUp/PgDn : changer de page   " +
                "Entrée : valider   " +
                "Échap : retour[/]");

            var key =
                System.Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    {
                        var positionInPage =
                            selectedIndex - pageStart;

                        var row =
                            positionInPage / columns;

                        var column =
                            positionInPage % columns;

                        // Monter normalement
                        if (row > 0)
                        {
                            var newIndex =
                                selectedIndex - columns;

                            if (newIndex >= pageStart)
                            {
                                selectedIndex = newIndex;
                            }
                        }
                        // Première ligne : page précédente
                        else if (currentPage > 0)
                        {
                            var previousPageStart =
                                pageStart - pageSize;

                            var previousPageEnd =
                                Math.Min(
                                    previousPageStart + pageSize,
                                    teams.Count);

                            var previousPageItemCount =
                                previousPageEnd - previousPageStart;

                            var previousColumn =
                                Math.Min(
                                    column,
                                    (previousPageItemCount - 1) % columns);

                            var previousRow =
                                (previousPageItemCount - 1 - previousColumn)
                                / columns;

                            selectedIndex =
                                previousPageStart +
                                previousRow * columns +
                                previousColumn;
                        }

                        break;
                    }

                case ConsoleKey.DownArrow:
                    {
                        var positionInPage =
                            selectedIndex - pageStart;

                        var row =
                            positionInPage / columns;

                        var column =
                            positionInPage % columns;

                        // Descendre normalement
                        var newIndex =
                            selectedIndex + columns;

                        if (newIndex < pageEnd)
                        {
                            selectedIndex = newIndex;
                        }
                        // Dernière ligne : page suivante
                        else if (pageEnd < teams.Count)
                        {
                            var nextPageStart =
                                pageEnd;

                            var nextPageItemCount =
                                Math.Min(
                                    pageSize,
                                    teams.Count - nextPageStart);

                            var nextColumn =
                                Math.Min(
                                    column,
                                    nextPageItemCount - 1);

                            selectedIndex =
                                nextPageStart + nextColumn;
                        }

                        break;
                    }

                case ConsoleKey.LeftArrow:
                    {
                        var positionInPage =
                            selectedIndex - pageStart;

                        var column =
                            positionInPage % columns;

                        if (column > 0)
                        {
                            selectedIndex--;
                        }

                        break;
                    }

                case ConsoleKey.RightArrow:
                    {
                        var positionInPage =
                            selectedIndex - pageStart;

                        var column =
                            positionInPage % columns;

                        var newIndex =
                            selectedIndex + 1;

                        if (column < columns - 1 &&
                            newIndex < pageEnd)
                        {
                            selectedIndex = newIndex;
                        }

                        break;
                    }

                case ConsoleKey.PageUp:
                    {
                        if (currentPage > 0)
                        {
                            selectedIndex =
                                Math.Max(
                                    0,
                                    selectedIndex - pageSize);
                        }

                        break;
                    }

                case ConsoleKey.PageDown:
                    {
                        if (pageEnd < teams.Count)
                        {
                            selectedIndex =
                                Math.Min(
                                    teams.Count - 1,
                                    selectedIndex + pageSize);
                        }

                        break;
                    }

                case ConsoleKey.Spacebar:
                    {
                        var teamId =
                            teams[selectedIndex].Id;

                        if (selected.Contains(teamId))
                        {
                            selected.Remove(teamId);
                        }
                        else if (selected.Count < requiredCount)
                        {
                            selected.Add(teamId);
                        }

                        break;
                    }

                case ConsoleKey.Enter:
                    {
                        if (selected.Count == requiredCount)
                        {
                            return teams
                                .Where(x => selected.Contains(x.Id))
                                .ToList();
                        }

                        break;
                    }

                case ConsoleKey.R:
                    {
                        selected.Clear();

                        var randomTeams = teams
                            .OrderBy(_ => Random.Shared.Next())
                            .Take(requiredCount);

                        foreach (var team in randomTeams)
                        {
                            selected.Add(team.Id);
                        }

                        break;
                    }

                case ConsoleKey.Escape:
                    return null;
            }
        }
    }

    private static int GetTeamCount(
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

    private static string GetRoundName(
        CompetitionRound round)
    {
        return round switch
        {
            CompetitionRound.TrenteDeuxiemeDeFinale =>
                "1/32ème",

            CompetitionRound.SeiziemeDeFinale =>
                "1/16ème",

            CompetitionRound.HuitiemeDeFinale =>
                "1/8ème",

            CompetitionRound.QuartDeFinale =>
                "1/4",

            CompetitionRound.DemiFinale =>
                "1/2",

            CompetitionRound.Finale =>
                "Finale",

            _ => throw new ArgumentOutOfRangeException(
                nameof(round))
        };
    }
}
