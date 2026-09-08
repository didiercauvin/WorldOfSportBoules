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
    private readonly GetAvailableCompetitionsHandler
        _getAvailableCompetitionsHandler;

    private IReadOnlyList<ScheduledCompetition> _competitions = [];
    private readonly HashSet<Guid> _selectedCompetitions = [];
    private int _selectedIndex;

    public PrepareSeasonScreen(
        GetAvailableCompetitionsHandler getAvailableCompetitionsHandler)
    {
        _getAvailableCompetitionsHandler =
            getAvailableCompetitionsHandler;
    }

    public void Initialize(int seasonYear)
    {
        _competitions =
            _getAvailableCompetitionsHandler.Handle(
                new GetAvailableCompetitionsQuery(
                    seasonYear));

        _selectedCompetitions.Clear();
        _selectedIndex = 0;
    }

    public Panel Render(int seasonYear)
    {
        if (_competitions.Count == 0)
        {
            return new Panel(
                new Markup(
                    "[yellow]Aucun concours disponible " +
                    "pour cette saison.[/]"));
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("")
            .AddColumn("")
            .AddColumn("Date")
            .AddColumn("Concours")
            .AddColumn("Lieu")
            .AddColumn("Format")
            .AddColumn("Engagement");

        for (var i = 0; i < _competitions.Count; i++)
        {
            var competition = _competitions[i];

            var cursor = i == _selectedIndex
                ? "[bold]❯[/]"
                : "";

            var checkbox = _selectedCompetitions.Contains(
                competition.Id)
                ? "[green][[x]][/]"
                : "[[ ]]";

            var name = i == _selectedIndex
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
                "vous souhaitez participer.\n"),
            table,
            new Markup(
                $"\n[bold]{_selectedCompetitions.Count}[/] " +
                "concours sélectionné(s)\n\n" +
                "[grey]↑ ↓ : déplacer   " +
                "Espace : sélectionner   " +
                "Entrée : valider   " +
                "Échap : annuler[/]"));

        return new Panel(content)
        {
            Header = new PanelHeader("[bold]Calendrier[/]"),
            Border = BoxBorder.Rounded
        };
    }

    public void MoveUp()
    {
        if (_competitions.Count == 0)
            return;

        _selectedIndex = Math.Max(
            0,
            _selectedIndex - 1);
    }

    public void MoveDown()
    {
        if (_competitions.Count == 0)
            return;

        _selectedIndex = Math.Min(
            _competitions.Count - 1,
            _selectedIndex + 1);
    }

    public void ToggleSelectedCompetition()
    {
        if (_competitions.Count == 0)
            return;

        var competition = _competitions[_selectedIndex];

        if (!_selectedCompetitions.Add(competition.Id))
        {
            _selectedCompetitions.Remove(competition.Id);
        }
    }

    public IReadOnlyList<Guid> GetSelectedCompetitionIds()
    {
        return _selectedCompetitions.ToList();
    }
}