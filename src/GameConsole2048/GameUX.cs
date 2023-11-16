/*
* GameConsole2048 (c) Mendz, etmendz. All rights reserved. 
* SPDX-License-Identifier: GPL-3.0-or-later 
*/
using GameLibrary;

namespace GameConsole2048;

/// <summary>
/// Provides the game UX capabilities for game flow and game play interactions.
/// </summary>
internal static class GameUX
{
    private static readonly GameConsoleUX _gameConsoleUX = new();

    /// <summary>
    /// Gets a game move.
    /// </summary>
    /// <returns>The <see cref="GameMove"/>.</returns>
    public static GameMove GetMove()
    {
        GameMove move = GameMove.None;
        // Evaluate a key stroke in to a game move.
        switch (_gameConsoleUX.GetMove())
        {
            case ConsoleKey.UpArrow: move = GameMove.Up; break;
            case ConsoleKey.RightArrow: move = GameMove.Right; break;
            case ConsoleKey.LeftArrow: move = GameMove.Left; break;
            case ConsoleKey.DownArrow: move = GameMove.Down; break;
            default: break;
        }
        return move;
    }
}