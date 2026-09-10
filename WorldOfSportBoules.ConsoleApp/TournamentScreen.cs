using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.ConsoleApp;

public sealed class TournamentScreen
{
    public Panel Render(
        CompetitionParticipation participation,
        Team team)
    {
        var tournament = participation.Tournament;

        if (tournament is null)
        {
            return new Panel(
                new Markup(
                    "[yellow]Cette compétition ne possède " +
                    "pas encore de tournoi.[/]"));
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("Tour")
            .AddColumn("Matchs");

        foreach (var round in tournament.Rounds)
        {
            table.AddRow(
                GetRoundName(round.Round),
                RenderRound(round, team));
        }

        var nextAction =
            GetNextAction(tournament, team);

        var content = new Rows(
            new Markup(
                $"[bold]{participation.Competition.Definition.Name}[/]\n" +
                $"{participation.Competition.StartDate:dd/MM/yyyy}\n"),
            table,
            new Markup(
                $"\n{nextAction}"));

        return new Panel(content)
        {
            Header = new PanelHeader("[bold]Tournoi[/]"),
            Border = BoxBorder.Rounded
        };
    }

    private static string RenderRound(
        TournamentRound round,
        Team team)
    {
        var lines = new List<string>();

        foreach (var match in round.Matches)
        {
            var isOurMatch =
                match.Team1.Id == team.Id ||
                match.Team2.Id == team.Id;

            var team1 =
                FormatTeam(
                    match.Team1,
                    team);

            var team2 =
                FormatTeam(
                    match.Team2,
                    team);

            if (!match.IsPlayed)
            {
                lines.Add(
                    $"{team1} [grey]vs[/] {team2}");

                continue;
            }

            var score =
                $"{match.Result!.Team1Score} - " +
                $"{match.Result.Team2Score}";

            var result =
                match.Winner!.Id == team.Id
                    ? "[green]✓[/]"
                    : isOurMatch
                        ? "[red]✗[/]"
                        : "";

            lines.Add(
                $"{team1} {score} {team2} {result}");
        }

        return string.Join(
            "\n",
            lines);
    }

    private static string FormatTeam(
        Team team,
        Team ourTeam)
    {
        if (team.Id == ourTeam.Id)
            return $"[bold]{team.Name}[/]";

        return team.Name;
    }

    private static string GetNextAction(
        Tournament tournament,
        Team team)
    {
        if (tournament.HasLost(team))
        {
            return "[red]Éliminé[/]\n" +
                   "[grey]Échap : retour[/]";
        }

        if (tournament.IsFinished)
        {
            return "[green]Vainqueur du concours[/]\n" +
                   "[grey]Échap : retour[/]";
        }

        if (!tournament.IsCurrentRoundFinished)
        {
            return "[yellow]Entrée : jouer le " +
                   $"{GetRoundName(tournament.CurrentRound.Round)}[/]";
        }

        return "[grey]Préparation du prochain tour...[/]";
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
