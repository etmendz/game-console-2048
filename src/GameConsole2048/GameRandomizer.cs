/*
* GameConsole2048 (c) Mendz, etmendz. All rights reserved. 
* SPDX-License-Identifier: GPL-3.0-or-later 
*/
namespace GameConsole2048;

/// <summary>
/// Defines the game randomizer.
/// </summary>
internal static class GameRandomizer
{
    /// <summary>
    /// Randomly gets 2 or 4, calibrated so there's a 90% chance that 2 is returned.
    /// </summary>
    /// <returns>Returns 2, else 4.</returns>
    public static int Next() => Random.Shared.NextDouble() < 0.9 ? 2 : 4;
}