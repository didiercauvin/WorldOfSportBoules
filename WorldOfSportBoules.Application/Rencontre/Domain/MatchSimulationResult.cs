using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfSportBoules.Application.Rencontre.Domain;

public sealed class MatchSimulationResult
{
    public MatchResult Result { get; }

    public IReadOnlyList<MeneResult> Menes { get; }

    public MatchSimulationResult(
        MatchResult result,
        IReadOnlyList<MeneResult> menes)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(menes);

        Result = result;
        Menes = menes;
    }
}
