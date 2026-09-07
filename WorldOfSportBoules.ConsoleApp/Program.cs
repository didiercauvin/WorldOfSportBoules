using WorldOfSportBoules.ConsoleApp;
using WorldOfSportBoules.Domain;

var terrain = new Terrain();

var renderer = new ConsoleTerrainRenderer();

Console.WriteLine(renderer.Render(terrain));

Console.ReadKey();