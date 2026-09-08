using Spectre.Console;
using WorldOfSportBoules.Application.CareerManagement.Application.CreatingCareer;
using WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailableCompetitions;
using WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailablePlayers;
using WorldOfSportBoules.Application.CareerManagement.Application.PreparingSeason;
using WorldOfSportBoules.Application.CareerManagement.Infrastructure.Competitions;
using WorldOfSportBoules.Application.CareerManagement.Infrastructure.Players;
using WorldOfSportBoules.ConsoleApp;
using WorldOfSportBoules.ConsoleApp.CareerScreens;

var competitionProvider = new InMemoryCompetitionProvider();
var playerProvider = new InMemoryPlayerProvider();

var getAvailablePlayersHandler =
    new GetAvailablePlayersHandler(
        playerProvider);

var getAvailableCompetitionsHandler =
    new GetAvailableCompetitionsHandler(
        competitionProvider);

var prepareSeasonHandler =
    new PrepareSeasonHandler(
        competitionProvider);

var createCareerHandler =
    new CreateCareerHandler();

var prepareSeasonScreen =
    new PrepareSeasonScreen(
        getAvailableCompetitionsHandler);

var createCareerScreen =
    new CreateCareerScreen(
        createCareerHandler);

var manageTeamScreen =
    new ManageTeamScreen(
        getAvailablePlayersHandler);

var selectCompetitionScreen = new SelectCompetitionScreen();
var tournamentScreen = new TournamentScreen();

var careerScreen =
    new CareerScreen(
        getAvailableCompetitionsHandler,
        prepareSeasonHandler,
        manageTeamScreen,
        prepareSeasonScreen,
        selectCompetitionScreen,
        tournamentScreen);

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

                careerScreen.Run(career);

                break;
            }

        case "Quitter":
            return;
    }
}
