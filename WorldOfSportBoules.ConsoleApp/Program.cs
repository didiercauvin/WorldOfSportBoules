using Spectre.Console;
using WorldOfSportBoules.Application.CareerManagement.Application.CreatingCareer;
using WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailableCompetitions;
using WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailablePlayers;
using WorldOfSportBoules.Application.CareerManagement.Application.PreparingSeason;
using WorldOfSportBoules.Application.CareerManagement.Infrastructure;
using WorldOfSportBoules.Application.CareerManagement.Infrastructure.Competitions;
using WorldOfSportBoules.Application.CareerManagement.Infrastructure.Players;
using WorldOfSportBoules.ConsoleApp;
using WorldOfSportBoules.ConsoleApp.CareerScreens;

var competitionProvider = new InMemoryCompetitionProvider();
var playerProvider = new InMemoryPlayerProvider();

var getAvailablePlayersHandler =
    new GetAvailablePlayersHandler(playerProvider);

var getAvailableCompetitionsHandler =
    new GetAvailableCompetitionsHandler(competitionProvider);

var prepareSeasonHandler =
    new PrepareSeasonHandler(competitionProvider);

var prepareSeasonScreen =
    new PrepareSeasonScreen(
        getAvailableCompetitionsHandler,
        prepareSeasonHandler);

var createCareerHandler =
    new CreateCareerHandler();

var createCareerScreen =
    new CreateCareerScreen(createCareerHandler, getAvailablePlayersHandler);

while (true)
{
    AnsiConsole.Clear();

    var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[bold]Sport Boules Manager[/]")
            .AddChoices(
                "Nouvelle carrière",
                "Quitter"));

    switch (choice)
    {
        case "Nouvelle carrière":
            {
                var career = createCareerScreen.Run();

                // Pour l'instant :
                var seasonYear = DateTime.Now.Year;

                prepareSeasonScreen.Run(
                    career,
                    seasonYear);

                break;
            }

        case "Quitter":
            return;
    }
}