/*
* GameConsole2048 (c) Mendz, etmendz. All rights reserved. 
* SPDX-License-Identifier: GPL-3.0-or-later 
*/
using GameLibrary;

namespace GameConsole2048;

internal static class Program
{
    private static void Main() => new GameConsole<GameUI, GameGrid, GameMove, bool>(
        "GameConsole2048",
        "Mendz, etmendz. All rights reserved.",
        "A simple console app version of 2048 -- https://github.com/etmendz/game-console-2048",
        "Use the arrow keys to move, fill and merge the cell values in the grid." + Environment.NewLine + Environment.NewLine + "Press [Esc] anytime to exit the app.",
        GamePlayReadyMode.WhileReady
        ).Play(); // Play the game!
}