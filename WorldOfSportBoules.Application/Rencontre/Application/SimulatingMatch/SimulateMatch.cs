using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Application.CareerManagement.Domain;
using WorldOfSportBoules.Application.Rencontre.Domain;

namespace WorldOfSportBoules.Application.Rencontre.Application.SimulatingMatch;

public sealed record SimulateMatchCommand(
    Team Team1,
    Team Team2);

public sealed class SimulateMatchHandler
{
    private readonly MatchSimulator _simulator;

    public SimulateMatchHandler(MatchSimulator simulator)
    {
        _simulator = simulator;
    }

    public MatchSimulationResult Handle(
        SimulateMatchCommand command)
    {
        return _simulator.Simulate(
        command.Team1,
        command.Team2);
    }
}
