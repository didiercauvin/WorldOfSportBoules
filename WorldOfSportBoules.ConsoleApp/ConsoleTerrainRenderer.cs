using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfSportBoules.Domain;

namespace WorldOfSportBoules.ConsoleApp;

public sealed class ConsoleTerrainRenderer
{
    private const int TerrainWidth = 31;

    private const int Zone2mHeight = 6;
    private const int Zone5mHeight = 15;

    public string Render(Terrain terrain)
    {
        var lines = new List<string>();

        lines.Add($"╔{new string('═', TerrainWidth)}╗");

        // Zone des 2 mètres
        AddEmptyLines(lines, Zone2mHeight / 2);
        lines.Add(Center("ZONE DES 2 MÈTRES"));
        AddEmptyLines(lines, Zone2mHeight - (Zone2mHeight / 2) - 1);

        // Séparation entre les zones
        lines.Add($"╠{new string('═', TerrainWidth)}╣");

        // Zone des 5 mètres
        AddEmptyLines(lines, Zone5mHeight / 2);
        lines.Add(Center("ZONE DES 5 MÈTRES"));
        AddEmptyLines(lines, Zone5mHeight - (Zone5mHeight / 2) - 1);

        lines.Add($"╚{new string('═', TerrainWidth)}╝");

        lines.Add(Center("Ligne des 12,5 m"));

        return string.Join(Environment.NewLine, lines);
    }

    private static void AddEmptyLines(
        List<string> lines,
        int count)
    {
        for (var i = 0; i < count; i++)
        {
            lines.Add($"║{new string(' ', TerrainWidth)}║");
        }
    }

    private static string Center(string text)
    {
        var padding = Math.Max(0, (TerrainWidth - text.Length) / 2);

        return $"║{new string(' ', padding)}{text}{new string(' ', TerrainWidth - padding - text.Length)}║";
    }
}