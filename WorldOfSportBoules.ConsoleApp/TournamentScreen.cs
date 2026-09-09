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
        var competition = participation.Competition;
        var tournament = participation.Tournament;

        if (tournament is null)
        {
            return new Panel(
                new Markup(
                    "[yellow]Le format de cette compétition " +
                    "n'est pas encore disponible.[/]"))
            {
                Header = new PanelHeader(
                    $"[bold]{competition.Definition.Name}[/]"),
                Border = BoxBorder.Rounded
            };
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("Tour")
            .AddColumn("Statut")
            .AddColumn("Résultat");

        foreach (var round in tournament.Rounds)
        {
            var match = round.Matches
                .FirstOrDefault(x =>
                    x.Team1.Id == team.Id ||
                    x.Team2.Id == team.Id);

            var status = GetStatus(
                match,
                team);

            var result = match?.Result is null
                ? ""
                : $"{match.Result.Team1Score} - " +
                  $"{match.Result.Team2Score}";

            table.AddRow(
                GetRoundName(round.Round),
                status,
                result);
        }

        var content = new Rows(
            new Markup(
                $"[bold]{competition.Definition.Name}[/]\n" +
                $"{competition.Definition.Location}\n"),
            table,
            new Markup(
                "\n[grey]↑ ↓ : déplacer   " +
                "Entrée : sélectionner   " +
                "Échap : retour[/]"));

        return new Panel(content)
        {
            Header = new PanelHeader("[bold]Tournoi[/]"),
            Border = BoxBorder.Rounded
        };
    }

    private static string GetStatus(
    TournamentMatch? match,
    Team team)
    {
        if (match is null)
        {
            return "[grey]○[/]";
        }

        if (!match.IsPlayed)
        {
            return "[yellow]▶ À jouer[/]";
        }

        return match.Winner?.Id == team.Id
            ? "[green]✓ Victoire[/]"
            : "[red]✗ Défaite[/]";
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

            _ => round.ToString()
        };
    }
}
