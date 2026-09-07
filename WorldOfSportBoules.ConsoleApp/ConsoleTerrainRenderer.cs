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

    private const int Zone5mHeight = 15;
    private const int SeparationHeight = 1;
    private const int Zone2mHeight = 6;

    private const int TerrainHeight =
        Zone5mHeight +
        SeparationHeight +
        Zone2mHeight;

    public string Render(Terrain terrain, Match match)
    {
        var grid = CreateGrid();

        // La grille est organisée du haut vers le bas :
        //
        //   zone 2 m
        //   séparation
        //   zone 5 m
        //
        // Le bas correspond à la ligne des 12,5 m.

        var separationRow = Zone2mHeight;

        DrawZoneLabel(
            grid,
            Zone2mHeight / 2,
            "ZONE DES 2 MÈTRES");

        DrawZoneSeparator(
            grid,
            separationRow);

        DrawZoneLabel(
            grid,
            separationRow + 1 + Zone5mHeight / 2,
            "ZONE DES 5 MÈTRES");

        if (match.But is not null)
        {
            DrawBut(grid, terrain, match.But);
        }

        return RenderGrid(grid);
    }

    private static char[,] CreateGrid()
    {
        var grid = new char[TerrainHeight, TerrainWidth];

        for (var row = 0; row < TerrainHeight; row++)
        {
            for (var column = 0; column < TerrainWidth; column++)
            {
                grid[row, column] = ' ';
            }
        }

        return grid;
    }

    private static void DrawZoneSeparator(
        char[,] grid,
        int row)
    {
        for (var column = 0; column < TerrainWidth; column++)
        {
            grid[row, column] = '─';
        }
    }

    private static void DrawZoneLabel(
        char[,] grid,
        int row,
        string label)
    {
        var startColumn =
            (TerrainWidth - label.Length) / 2;

        for (var i = 0; i < label.Length; i++)
        {
            grid[row, startColumn + i] = label[i];
        }
    }

    private static void DrawBut(
        char[,] grid,
        Terrain terrain,
        But but)
    {
        var column = PositionToColumn(
            but.Position.X,
            terrain);

        var row = PositionToRow(
            but.Position.Y,
            terrain);

        grid[row, column] = '●';
    }

    private static int PositionToColumn(
        double x,
        Terrain terrain)
    {
        var ratio = x / terrain.Width;

        var column = (int)Math.Round(
            ratio * (TerrainWidth - 1));

        return Math.Clamp(
            column,
            0,
            TerrainWidth - 1);
    }

    private static int PositionToRow(
        double y,
        Terrain terrain)
    {
        var ratio =
            (y - terrain.LigneDesBut) /
            (terrain.LigneDesPied - terrain.LigneDesBut);

        // 12,5 m = bas de l'écran
        // 19,5 m = haut de l'écran
        var row = (int)Math.Round(
            (1 - ratio) * (TerrainHeight - 1));

        return Math.Clamp(
            row,
            0,
            TerrainHeight - 1);
    }

    private static string RenderGrid(char[,] grid)
    {
        var lines = new List<string>
        {
            $"╔{new string('═', TerrainWidth)}╗"
        };

        for (var row = 0; row < TerrainHeight; row++)
        {
            var content = new string(
                Enumerable
                    .Range(0, TerrainWidth)
                    .Select(column => grid[row, column])
                    .ToArray());

            lines.Add($"║{content}║");
        }

        lines.Add(
            $"╚{new string('═', TerrainWidth)}╝");

        lines.Add(
            Center("Ligne des 12,5 m"));

        return string.Join(
            Environment.NewLine,
            lines);
    }

    private static string Center(string text)
    {
        var padding =
            Math.Max(0, (TerrainWidth - text.Length) / 2);

        return new string(' ', padding) + text;
    }
}