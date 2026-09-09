using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.ConsoleApp;

public sealed class CompetitionPathScreen
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
            .AddColumn("Adversaire")
            .AddColumn("Score")
            .AddColumn("Résultat");

        foreach (var round in tournament.Rounds)
        {
            var match = round.Matches
                .FirstOrDefault(x =>
                    x.Team1.Id == team.Id ||
                    x.Team2.Id == team.Id);

            if (match is null)
                continue;

            var opponent =
                match.Team1.Id == team.Id
                    ? match.Team2
                    : match.Team1;

            var score = "";

            if (match.Result is not null)
            {
                score = match.Team1.Id == team.Id
                    ? $"{match.Result.Team1Score} - " +
                      $"{match.Result.Team2Score}"
                    : $"{match.Result.Team2Score} - " +
                      $"{match.Result.Team1Score}";
            }

            var result = !match.IsPlayed
                ? "[yellow]À jouer[/]"
                : match.Winner?.Id == team.Id
                    ? "[green]Victoire[/]"
                    : "[red]Défaite[/]";

            table.AddRow(
                GetRoundName(round.Round),
                opponent.Name,
                score,
                result);
        }

        var content = new Rows(
            new Markup(
                $"[bold]{participation.Competition.Definition.Name}[/]\n" +
                $"{participation.Competition.StartDate:dd/MM/yyyy}\n\n"),
            table,
            new Markup(
                "\n[grey]Échap : retour[/]"));

        return new Panel(content)
        {
            Header = new PanelHeader("[bold]Parcours[/]"),
            Border = BoxBorder.Rounded
        };
    }

    private static string GetRoundName(
        CompetitionRound round)
    {
        return round switch
        {
            CompetitionRound.TrenteDeuxiemeDeFinale =>
                "1/32ème de finale",

            CompetitionRound.SeiziemeDeFinale =>
                "1/16ème de finale",

            CompetitionRound.HuitiemeDeFinale =>
                "1/8ème de finale",

            CompetitionRound.QuartDeFinale =>
                "1/4 de finale",

            CompetitionRound.DemiFinale =>
                "1/2 finale",

            CompetitionRound.Finale =>
                "Finale",

            _ => throw new ArgumentOutOfRangeException(
                nameof(round))
        };
    }
}
