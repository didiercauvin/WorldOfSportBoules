using Spectre.Console;
using WorldOfSportBoules.Application.CareerManagement.Application.CreatingCareer;
using WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailableCompetitions;
using WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailablePlayers;
using WorldOfSportBoules.Application.CareerManagement.Application.PreparingSeason;
using WorldOfSportBoules.Application.CareerManagement.Application.SimulatingMatch;
using WorldOfSportBoules.Application.CareerManagement.Application.StartingCompetition;
using WorldOfSportBoules.Application.CareerManagement.Infrastructure.Competitions;
using WorldOfSportBoules.Application.CareerManagement.Infrastructure.Players;
using WorldOfSportBoules.Application.Competition.CreatingCompetition;
using WorldOfSportBoules.ConsoleApp;
using WorldOfSportBoules.ConsoleApp.CareerScreens;

var competitionProvider = new InMemoryCompetitionProvider();
var playerProvider = new InMemoryPlayerProvider();
var teamProvider =  new InMemoryTeamProvider();

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
var startCompetitionHandler = new StartCompetitionHandler(teamProvider);
var simulateMatchHandler = new SimulateMatchHandler();
var seasonScreen =
    new SeasonScreen();

var competitionPathScreen =
    new CompetitionPathScreen();

var createCompetitionHandler =
    new CreateCompetitionHandler(teamProvider);

var teamDetailsRenderer =
    new TeamDetailsRenderer();

var createCompetitionScreen =
    new CreateCompetitionScreen(
        createCompetitionHandler,
        teamProvider,
        teamDetailsRenderer);

var careerScreen =
    new CareerScreen(
        getAvailableCompetitionsHandler,
        prepareSeasonHandler,
        manageTeamScreen,
        prepareSeasonScreen,
        selectCompetitionScreen,
        tournamentScreen,
        startCompetitionHandler,
        simulateMatchHandler,
        seasonScreen,
        competitionPathScreen);

while (true)
{
    AnsiConsole.Clear();

    var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[bold]Sport Boules Manager[/]")
            .AddChoices(
                "Nouvelle carrière",
                "Créer son concours",
                "Quitter"));

    switch (choice)
    {
        case "Nouvelle carrière":
            {
                var career = createCareerScreen.Run();

                careerScreen.Run(career);

                break;
            }

        case "Créer son concours":
            {
                var competition =
                    createCompetitionScreen.Run();

                if (competition is not null)
                {
                    careerScreen.RunTournament(
                        competition.Tournament,
                        competition.PlayerTeam);
                }

                break;
            }

        case "Quitter":
            return;
    }
}
