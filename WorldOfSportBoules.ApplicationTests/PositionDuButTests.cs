using WorldOfSportBoules.Domain;
using WorldOfSportBoules.Engine;

namespace WorldOfSportBoules.ApplicationTests;

[TestClass]
public sealed class PositionDuButTests
{
    [TestMethod]
    public void Le_joueur_peut_choisir_la_position_du_but()
    {
        var match = ScenarioBuilder
        .UnePartie()
        .AvecUnJoueur("Pierre")
        .Build();

        var engine = new MatchEngine(new ButEngine());

        var cible = new Position(1.2, 15.5);

        engine.LancerLeBut(match, "Pierre", cible);

        Assert.AreEqual(cible, match.But!.Position);
    }
}
