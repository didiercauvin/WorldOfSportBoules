using WorldOfSportBoules.Application._Shared.Domain;
using WorldOfSportBoules.Application.Rencontre.Domain;

namespace WorldOfSportBoules.Application.Rencontre.Engine;

public sealed class ButEngine
{

    public Position Lancer(Player player, Position cible)
    {
        // Pour l'instant : lancement parfaitement précis.
        return cible;
    }
}