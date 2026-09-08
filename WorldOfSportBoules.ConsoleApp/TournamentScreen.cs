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
        CompetitionParticipation participation)
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
            var status = GetStatus(
                round,
                tournament);

            var result = round.Result is null
                ? ""
                : $"{round.Result.TeamScore} - " +
                  $"{round.Result.OpponentScore}";

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
        TournamentRound round,
        Tournament tournament)
    {
        if (round.Result is not null)
        {
            return round.Result.IsVictory
                ? "[green]✓ Victoire[/]"
                : "[red]✗ Défaite[/]";
        }

        if (tournament.NextRound == round)
        {
            return "[yellow]▶ À jouer[/]";
        }

        return "[grey]○[/]";
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
