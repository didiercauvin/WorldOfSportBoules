using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.ConsoleApp;

public sealed class SelectCompetitionScreen
{
    private int _selectedIndex;

    public void Initialize(Career career)
    {
        _selectedIndex = 0;
    }

    public Panel Render(Career career)
    {
        var competitions = career.CurrentSeason?.Competitions
            ?? [];

        if (competitions.Count == 0)
        {
            return new Panel(
                new Markup(
                    "[yellow]Aucun concours n'est prévu " +
                    "pour cette saison.[/]"));
        }

        if (_selectedIndex >= competitions.Count)
        {
            _selectedIndex = competitions.Count - 1;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("")
            .AddColumn("Date")
            .AddColumn("Concours")
            .AddColumn("Lieu")
            .AddColumn("Format")
            .AddColumn("Engagement");

        for (var i = 0; i < competitions.Count; i++)
        {
            var competition = competitions[i];

            var cursor = i == _selectedIndex
                ? "[bold]❯[/]"
                : "";

            var name = i == _selectedIndex
                ? $"[bold]{competition.Definition.Name}[/]"
                : competition.Definition.Name;

            table.AddRow(
                cursor,
                competition.StartDate.ToString("dd/MM/yyyy"),
                name,
                competition.Definition.Location,
                competition.Definition.Format.ToString(),
                competition.EntryFee.ToString("C"));
        }

        var content = new Rows(
            new Markup(
                $"[bold]{career.Team.Name}[/]\n\n" +
                "Sélectionnez le concours à jouer.\n"),
            table,
            new Markup(
                "\n[grey]↑ ↓ : déplacer   " +
                "Entrée : sélectionner   " +
                "Échap : retour[/]"));

        return new Panel(content)
        {
            Header = new PanelHeader("[bold]Concours[/]"),
            Border = BoxBorder.Rounded
        };
    }

    public void MoveUp(Career career)
    {
        var competitions = career.CurrentSeason?.Competitions;

        if (competitions is null || competitions.Count == 0)
            return;

        _selectedIndex = Math.Max(
            0,
            _selectedIndex - 1);
    }

    public void MoveDown(Career career)
    {
        var competitions = career.CurrentSeason?.Competitions;

        if (competitions is null || competitions.Count == 0)
            return;

        _selectedIndex = Math.Min(
            competitions.Count - 1,
            _selectedIndex + 1);
    }

    public ScheduledCompetition? GetSelectedCompetition(
        Career career)
    {
        var competitions = career.CurrentSeason?.Competitions;

        if (competitions is null ||
            competitions.Count == 0)
        {
            return null;
        }

        return competitions[_selectedIndex];
    }
}
