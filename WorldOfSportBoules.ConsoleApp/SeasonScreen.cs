using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.ConsoleApp;

public sealed class SeasonScreen
{
    private int _selectedIndex;

    public void Initialize()
    {
        _selectedIndex = 0;
    }

    public Panel Render(Career career)
    {
        var season = career.CurrentSeason;

        if (season is null)
        {
            return new Panel(
                new Markup(
                    "[yellow]Aucune saison en cours.[/]"));
        }

        var competitions = season.Competitions
            .OrderBy(x => x.StartDate)
            .ToList();

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
            .AddColumn("Parcours")
            .AddColumn("Statut");

        foreach (var competition in competitions
                     .Select((competition, index) =>
                         new { competition, index }))
        {
            var scheduledCompetition = competition.competition;

            var participation = season.Participations
                .FirstOrDefault(x =>
                    x.Competition.Id ==
                    scheduledCompetition.Id);

            var cursor =
                competition.index == _selectedIndex
                    ? "[bold]❯[/]"
                    : "";

            var name =
                competition.index == _selectedIndex
                    ? $"[bold]{scheduledCompetition.Definition.Name}[/]"
                    : scheduledCompetition.Definition.Name;

            table.AddRow(
                cursor,
                scheduledCompetition.StartDate
                    .ToString("dd/MM/yyyy"),
                name,
                GetPath(
                    participation,
                    career.Team),
                GetStatus(
                    participation,
                    career.Team));
        }

        var content = new Rows(
            new Markup(
                $"[bold]{career.Team.Name}[/]\n" +
                $"Saison {season.Year}-{season.Year + 1}\n"),
            table,
            new Markup(
                "\n[grey]↑ ↓ : déplacer   " +
                "Entrée : voir le parcours   " +
                "Échap : retour[/]"));

        return new Panel(content)
        {
            Header = new PanelHeader("[bold]Ma saison[/]"),
            Border = BoxBorder.Rounded
        };
    }

    public void MoveUp(Career career)
    {
        var count =
            career.CurrentSeason?.Competitions.Count ?? 0;

        if (count == 0)
            return;

        _selectedIndex = Math.Max(
            0,
            _selectedIndex - 1);
    }

    public void MoveDown(Career career)
    {
        var count =
            career.CurrentSeason?.Competitions.Count ?? 0;

        if (count == 0)
            return;

        _selectedIndex = Math.Min(
            count - 1,
            _selectedIndex + 1);
    }

    public ScheduledCompetition? GetSelectedCompetition(
        Career career)
    {
        var competitions = career.CurrentSeason?
            .Competitions
            .OrderBy(x => x.StartDate)
            .ToList();

        if (competitions is null ||
            competitions.Count == 0)
        {
            return null;
        }

        return competitions[_selectedIndex];
    }

    private static string GetPath(
        CompetitionParticipation? participation,
        Team team)
    {
        if (participation?.Tournament is null)
            return "[grey]—[/]";

        var matches = participation.Tournament
            .GetTeamMatches(team);

        if (matches.Count == 0)
            return "[grey]—[/]";

        return string.Join(
            "  ",
            matches.Select(match =>
            {
                var round =
                    GetShortRoundName(
                        participation.Tournament
                            .Rounds
                            .First(x => x.Matches.Contains(match))
                            .Round);

                if (!match.IsPlayed)
                    return $"[yellow]{round}[/]";

                return match.Winner?.Id == team.Id
                    ? $"[green]✓ {round}[/]"
                    : $"[red]✗ {round}[/]";
            }));
    }

    private static string GetStatus(
        CompetitionParticipation? participation,
        Team team)
    {
        if (participation is null)
            return "[grey]À venir[/]";

        if (participation.IsFinished)
        {
            if (participation.Tournament?.HasLost(team) == true)
                return "[red]Éliminé[/]";

            return "[green]Vainqueur[/]";
        }

        return "[yellow]En cours[/]";
    }

    private static string GetShortRoundName(
        CompetitionRound round)
    {
        return round switch
        {
            CompetitionRound.TrenteDeuxiemeDeFinale => "1/32",
            CompetitionRound.SeiziemeDeFinale => "1/16",
            CompetitionRound.HuitiemeDeFinale => "1/8",
            CompetitionRound.QuartDeFinale => "1/4",
            CompetitionRound.DemiFinale => "1/2",
            CompetitionRound.Finale => "Finale",

            _ => throw new ArgumentOutOfRangeException(
                nameof(round))
        };
    }
}
