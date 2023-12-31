﻿/*
* GameConsole2048 (c) Mendz, etmendz. All rights reserved. 
* SPDX-License-Identifier: GPL-3.0-or-later 
*/
namespace GameConsole2048;

/// <summary>
/// Specifies constants that define valid game moves.
/// </summary>
public enum GameMove
{
    /// <summary>
    /// Not a valid game move.
    /// </summary>
    None,
    /// <summary>
    /// Move up.
    /// </summary>
    Up,
    /// <summary>
    /// Move right.
    /// </summary>
    Right,
    /// <summary>
    /// Move left.
    /// </summary>
    Left,
    /// <summary>
    /// Move down.
    /// </summary>
    Down
}