namespace WorldOfSportBoules.Application.Rencontre.Domain;

public sealed record TerrainZone(
    string Name,
    double StartY,
    double EndY);

public sealed class Terrain
{
    public double Width { get; } = 3.5;

    public double LigneDesBut { get; } = 12.5;

    public double FinZoneDesCinqMetres { get; } = 17.5;

    public double LigneDesPied { get; } = 19.5;
}
