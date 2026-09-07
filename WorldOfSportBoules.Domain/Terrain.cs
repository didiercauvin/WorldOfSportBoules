namespace WorldOfSportBoules.Domain;

public sealed record TerrainZone(
    string Name,
    double StartY,
    double EndY);

public sealed class Terrain
{
    public IReadOnlyList<TerrainZone> Zones { get; }

    public Terrain()
    {
        Zones =
        [
            new TerrainZone(
                "Zone de jeu",
                0,
                5),

            new TerrainZone(
                "Zone des 2 mètres",
                5,
                7)
        ];
    }
}
