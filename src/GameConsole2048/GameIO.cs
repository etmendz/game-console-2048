/*
* GameConsole2048 (c) Mendz, etmendz. All rights reserved. 
* SPDX-License-Identifier: GPL-3.0-or-later 
*/
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameConsole2048;

/// <summary>
/// Source generation context for <see cref="GameLibrary2048.GameModel"/> native AOT compatibility...
/// </summary>
[JsonSerializable(typeof(GameModel))]
internal partial class GameModelContext : JsonSerializerContext
{
}

/// <summary>
/// Provides the game IO capabilities to save to or read from a game data file.
/// </summary>
internal class GameModelIO
{
    /// <summary>
    /// Reads a game data from a game file.
    /// </summary>
    /// <param name="gameDataPath">The path to read the game data from.</param>
    /// <returns>The game data read as <see cref="GameModel"/>.</returns>
    public static GameModel? Read(string gameDataPath) => File.Exists(gameDataPath) ? (GameModel?)JsonSerializer.Deserialize(JsonDocument.Parse(File.ReadAllText(gameDataPath)), typeof(GameModel), GameModelContext.Default) : null;

    /// <summary>
    /// Saves a game data to a game file.
    /// </summary>
    /// <param name="gameDataPath">The path to save the game data to.</param>
    /// <param name="gameData">The game data as <see cref="GameModel"/> to save.</param>
    public static void Write(string gameDataPath, GameModel gameData) => File.WriteAllText(gameDataPath, JsonSerializer.Serialize(gameData!, typeof(GameModel), GameModelContext.Default));
}

/// <summary>
/// Source generation context for <see cref="GameConsole2048.GameStat"/> native AOT compatibility...
/// </summary>
[JsonSerializable(typeof(GameStat))]
internal partial class GameStatContext : JsonSerializerContext
{
}

/// <summary>
/// Provides the game IO capabilities to save to or read from a game stat file.
/// </summary>
internal class GameStatIO
{
    /// <summary>
    /// Reads a game stat from a game file.
    /// </summary>
    /// <param name="gameStatPath">The path to read the game stat from.</param>
    /// <returns>The <see cref="GameStat"/> read.</returns>
    public static GameStat? Read(string gameStatPath) => File.Exists(gameStatPath) ? (GameStat?)JsonSerializer.Deserialize(JsonDocument.Parse(File.ReadAllText(gameStatPath)), typeof(GameStat), GameStatContext.Default) : null;

    /// <summary>
    /// Saves a game stat to a game file.
    /// </summary>
    /// <param name="gameStatPath">The path to save the game stat to.</param>
    /// <param name="gameStat">The <see cref="GameStat"/> to save.</param>
    public static void Write(string gameStatPath, GameStat gameStat) => File.WriteAllText(gameStatPath, JsonSerializer.Serialize(gameStat!, typeof(GameStat), GameStatContext.Default));
}