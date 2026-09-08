using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailableCompetitions;
using WorldOfSportBoules.Application.CareerManagement.Application.GettingAvailablePlayers;
using WorldOfSportBoules.Application.CareerManagement.Application.PreparingSeason;
using WorldOfSportBoules.Application.CareerManagement.Domain;

namespace WorldOfSportBoules.ConsoleApp;

public sealed class PrepareSeasonScreen
{
    private readonly GetAvailableCompetitionsHandler
        _getAvailableCompetitionsHandler;

    private readonly PrepareSeasonHandler
        _prepareSeasonHandler;

    private readonly CompetitionSelectionView
        _competitionSelectionView;

    public PrepareSeasonScreen(
        GetAvailableCompetitionsHandler getAvailableCompetitionsHandler,
        PrepareSeasonHandler prepareSeasonHandler,
        CompetitionSelectionView competitionSelectionView)
    {
        _getAvailableCompetitionsHandler =
            getAvailableCompetitionsHandler;

        _prepareSeasonHandler =
            prepareSeasonHandler;

        _competitionSelectionView =
            competitionSelectionView;
    }

    public Season? Run(
        Career career,
        int seasonYear)
    {
        var availableCompetitions =
            _getAvailableCompetitionsHandler.Handle(
                new GetAvailableCompetitionsQuery(seasonYear));

        var selectedCompetitions =
            _competitionSelectionView.Run(
                availableCompetitions,
                seasonYear);

        if (selectedCompetitions is null)
        {
            return null;
        }

        var command = new PrepareSeasonCommand(
            seasonYear,
            selectedCompetitions
                .Select(x => x.Id)
                .ToList());

        return _prepareSeasonHandler.Handle(
            command,
            career);
    }
}