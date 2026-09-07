using System.Numerics;
using WorldOfSportBoules.Domain;

namespace WorldOfSportBoules.Engine;

public sealed class ButEngine
{

    public Position Lancer(Player player, Position cible)
    {
        // Pour l'instant : lancement parfaitement précis.
        return cible;
    }
}