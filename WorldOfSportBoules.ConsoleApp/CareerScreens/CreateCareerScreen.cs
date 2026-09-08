using Spectre.Console;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Application.CreatingCareer;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.ConsoleApp.CareerScreens;

public sealed class CreateCareerScreen
{
    private readonly CreateCareerHandler _handler;

    public CreateCareerScreen(
        CreateCareerHandler handler)
    {
        _handler = handler;
    }

    public Career Run()
    {
        AnsiConsole.Clear();

        var managerName = AnsiConsole.Ask<string>(
            "Nom du manager :");

        var teamName = AnsiConsole.Ask<string>(
            "Nom de l'équipe :");

        var category = AnsiConsole.Prompt(
            new SelectionPrompt<TeamCategory>()
                .Title("Catégorie de départ :")
                .AddChoices(TeamCategory.M4));

        return _handler.Handle(
            new CreateCareerCommand(
                managerName,
                teamName,
                category));
    }
}