using WorldOfSportBoules.Application.Rencontre.Domain;
using WorldOfSportBoules.Application.Rencontre.Engine;
using WorldOfSportBoules.ConsoleApp;

var terrain = new Terrain();

var pierre = new Player(
    Guid.NewGuid(),
    "Pierre");

var match = new Match([pierre]);

var engine = new MatchEngine(
    new ButEngine());

var renderer = new ConsoleTerrainRenderer();

Console.Clear();

Console.WriteLine(
    renderer.Render(terrain, match));

Console.WriteLine();
Console.WriteLine("LANCER DU BUT");
Console.WriteLine();

Console.Write(
    "Distance souhaitée (12,5 à 19,5 m) : ");

var y = double.Parse(Console.ReadLine()!);

Console.Write(
    "Position latérale souhaitée (0 à 3,5 m) : ");

var x = double.Parse(
    Console.ReadLine()!);

var cible = new Position(x, y);

engine.LancerLeBut(
    match,
    "Pierre",
    cible);

Console.Clear();

Console.WriteLine(
    renderer.Render(terrain, match));

Console.WriteLine();

Console.WriteLine(
    $"But : X={match.But!.Position.X:F2} m, " +
    $"Y={match.But.Position.Y:F2} m");

Console.ReadKey();