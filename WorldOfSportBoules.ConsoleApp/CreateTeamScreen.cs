using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Domain;
using WorldOfSportBoules.Application.Competition.CreatingTeam;

namespace WorldOfSportBoules.ConsoleApp;

public sealed class CreateTeamScreen
{
    private readonly CreateTeamHandler _handler;
    private readonly IProvidePlayer _playerProvider;

    public CreateTeamScreen(
        CreateTeamHandler handler,
        IProvidePlayer playerProvider)
    {
        _handler = handler;
        _playerProvider = playerProvider;
    }

    public Team? Run(TeamCategory category)
    {
        var players =
            _playerProvider
                .GetForCategory(category)
                .ToList();

        const int requiredCount = 4;

        var selected = new HashSet<Guid>();
        var selectedIndex = 0;

        while (true)
        {
            AnsiConsole.Clear();

            var currentPlayer =
                players[selectedIndex];

            var selectionTable =
                new Table()
                    .NoBorder()
                    .AddColumn("")
                    .AddColumn("Joueur")
                    .AddColumn("Cat.")
                    .AddColumn("Tir")
                    .AddColumn("Point");

            foreach (var player in players)
            {
                var isCurrent =
                    player.Id == currentPlayer.Id;

                var cursor =
                    isCurrent
                        ? "[yellow]❯[/]"
                        : " ";

                var checkbox =
                    selected.Contains(player.Id)
                        ? "[green]X[/]"
                        : "[grey] [/]";

                selectionTable.AddRow(
                    $"{cursor} {checkbox}",
                    player.FullName,
                    player.Category.ToString(),
                    player.Tir.ToString(),
                    player.Point.ToString());
            }

            var selectedPlayers =
                players
                    .Where(x => selected.Contains(x.Id))
                    .ToList();

            var teamTable =
                new Table()
                    .NoBorder()
                    .AddColumn("Joueur")
                    .AddColumn("Cat.")
                    .AddColumn("Tir")
                    .AddColumn("Point");

            foreach (var player in selectedPlayers)
            {
                teamTable.AddRow(
                    player.FullName,
                    player.Category.ToString(),
                    player.Tir.ToString(),
                    player.Point.ToString());
            }

            var teamPanel =
                new Panel(teamTable)
                {
                    Header = new PanelHeader(
                        $"[bold]Mon équipe ({selected.Count}/4)[/]"),
                    Border = BoxBorder.Rounded
                };

            var content =
                new Rows(
                    new Markup(
                        "[bold]Constituer mon équipe[/]\n"),
                    selectionTable,
                    teamPanel,
                    new Markup(
                        "\n[grey]↑ ↓ : déplacer   " +
                        "Espace : sélectionner   " +
                        "R : tirage aléatoire   " +
                        "Entrée : valider   " +
                        "Échap : retour[/]")
                );

            AnsiConsole.Write(content);

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
                            players.Count - 1,
                            selectedIndex + 1);
                    break;

                case ConsoleKey.Spacebar:
                    {
                        var player =
                            players[selectedIndex];

                        if (selected.Contains(player.Id))
                        {
                            selected.Remove(player.Id);
                        }
                        else if (selected.Count < requiredCount)
                        {
                            selected.Add(player.Id);
                        }

                        break;
                    }

                case ConsoleKey.R:
                    {
                        selected.Clear();

                        foreach (var player in players
                            .OrderBy(_ => Random.Shared.Next())
                            .Take(requiredCount))
                        {
                            selected.Add(player.Id);
                        }

                        break;
                    }

                case ConsoleKey.Enter:
                    {
                        if (selected.Count != requiredCount)
                        {
                            break;
                        }

                        var name =
                            AnsiConsole.Ask<string>(
                                "[bold]Nom de l'équipe :[/]");

                        try
                        {
                            return _handler.Handle(
                                new CreateTeamCommand(
                                    name,
                                    category,
                                    selected.ToList()));
                        }
                        catch (Exception exception)
                        {
                            AnsiConsole.MarkupLine(
                                $"\n[red]{exception.Message}[/]");

                            System.Console.ReadKey(true);
                        }

                        break;
                    }

                case ConsoleKey.Escape:
                    return null;
            }
        }
    }
}
