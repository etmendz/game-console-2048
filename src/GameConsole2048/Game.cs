/*
* GameConsole2048 (c) Mendz, etmendz. All rights reserved. 
* SPDX-License-Identifier: GPL-3.0-or-later 
*/
using System.Reflection;

namespace GameConsole2048;

/// <summary>
/// Defines a game.
/// </summary>
internal static class Game
{
    // Initially not ready...
    private static bool _ready = false;

    /// <summary>
    /// Plays the game.
    /// </summary>
    public static void Play()
    {
        while (Ready())
        {
            Set();
            Go();
        }
    }

    /// <summary>
    /// Shows the game's splash screen.
    /// </summary>
    private static void Splash()
    {
        Console.CursorVisible = false;
        Console.WriteLine($"GameConsole2048 {Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion} (c) {DateTime.Now.Year} Mendz, etmendz. All rights reserved.");
        Console.WriteLine("A simple console app version of 2048 -- https://github.com/etmendz/game-console-2048");
        Console.WriteLine();
        Console.WriteLine("Use the arrow keys to move, fill and merge the cell values in the grid.");
        Console.WriteLine();
        Console.WriteLine("Press [Esc] anytime to exit the app.");
    }

    /// <summary>
    /// Ready?
    /// </summary>
    /// <returns>True.</returns>
    /// <remarks>When called for the first time, shows the splash screen. Then the game is always ready.</remarks>
    private static bool Ready()
    {
        if (!_ready)
        {
            Splash();
            _ready = true;
        }
        return _ready;
    }

    /// <summary>
    /// Set?
    /// </summary>
    private static void Set()
    {
        Console.WriteLine();
        Console.WriteLine("Press [Enter] to start playing...");
        GameUX.GetKey(ConsoleKey.Enter);
    }

    /// <summary>
    /// Go!
    /// </summary>
    private static void Go()
    {
        GameUI gameUI = new();
        if (gameUI.Start())
        {
            do
            {
                if (gameUI.Move())
                {
                    if (!gameUI.Continue()) break;
                }
            } while (!gameUI.GameOver());
        }
        gameUI.End();
    }
}