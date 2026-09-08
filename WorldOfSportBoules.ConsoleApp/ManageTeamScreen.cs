using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailablePlayers;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.ConsoleApp;

public sealed class ManageTeamScreen
{
    private readonly GetAvailablePlayersHandler
        _getAvailablePlayersHandler;

    private IReadOnlyList<Player>? _players = [];
    private int _selectedIndex;

    public ManageTeamScreen(
        GetAvailablePlayersHandler getAvailablePlayersHandler)
    {
        _getAvailablePlayersHandler =
            getAvailablePlayersHandler;
    }

    public void Initialize(Career career)
    {
        _players = _getAvailablePlayersHandler.Handle(
            new GetAvailablePlayersQuery(
                career.Team.Category));

        _selectedIndex = 0;
    }

    public Panel Render(Career career)
    {
        return CreateTeamContent(
            career,
            _players,
            _selectedIndex);
    }

    public void MoveUp()
    {
        if (_players is null)
            return;

        _selectedIndex =
            Math.Max(0, _selectedIndex - 1);
    }

    public void MoveDown()
    {
        if (_players is null)
            return;

        _selectedIndex =
            Math.Min(
                _players.Count - 1,
                _selectedIndex + 1);
    }

    public void ToggleSelectedPlayer(Career career)
    {
        if (_players is null || _players.Count == 0)
            return;

        var player = _players[_selectedIndex];

        var existingPlayer = career.Team.Players
            .FirstOrDefault(x => x.Id == player.Id);

        if (existingPlayer is not null)
        {
            career.Team.RemovePlayer(player.Id);
        }
        else
        {
            career.Team.AddPlayer(player);
        }
    }

    private static Panel CreateTeamContent(
        Career career,
        IReadOnlyList<Player> players,
        int selectedIndex)
    {
        var team = career.Team;

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("")
            .AddColumn("")
            .AddColumn("Joueur")
            .AddColumn("Catégorie");

        for (var i = 0; i < players.Count; i++)
        {
            var player = players[i];

            var isSelected = team.Players
                .Any(x => x.Id == player.Id);

            var cursor = i == selectedIndex
                ? "[bold]❯[/]"
                : "";

            var checkbox = isSelected
                ? "[green][[x]][/]"
                : "[[ ]]";

            var name = i == selectedIndex
                ? $"[bold]{player.FullName}[/]"
                : player.FullName;

            table.AddRow(
                cursor,
                checkbox,
                name,
                player.Category.ToString());
        }

        var content = new Rows(
            new Markup(
                $"[bold]{team.Name}[/]\n\n" +
                $"Catégorie : [bold]{team.Category}[/]\n" +
                $"Effectif : [bold]{team.Players.Count}[/] / 4\n"),
            table,
            new Markup(
                "\n[grey]↑ ↓ : déplacer   " +
                "Espace : ajouter/retirer   " +
                "Entrée : terminer[/]"));

        return new Panel(content)
        {
            Header = new PanelHeader("[bold]Effectif[/]"),
            Border = BoxBorder.Rounded
        };
    }
}