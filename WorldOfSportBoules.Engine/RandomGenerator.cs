namespace WorldOfSportBoules.Engine;

public interface IRandomGenerator
{
    double NextDouble();
}

public sealed class RandomGenerator : IRandomGenerator
{
    private readonly Random _random = new();

    public double NextDouble()
    {
        return _random.NextDouble();
    }
}
