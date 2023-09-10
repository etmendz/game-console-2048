/*
* GameConsole2048 (c) Mendz, etmendz. All rights reserved. 
* SPDX-License-Identifier: GPL-3.0-or-later 
*/
using CommunityToolkit.HighPerformance;
using GameLibrary;

namespace GameConsole2048;

/// <summary>
/// Defines a game grid.
/// </summary>
public class GameGrid : GameModel, IGamePlay<GameMove, bool>
{
    // The game cells...
    private GameCell[,] _gameCells = new GameCell[4, 4];

    // The Memory2D{T} wrapper for the game cells...
    private Memory2D<GameCell> _memory2D;

    /// <summary>
    /// Gets a game cell from the game grid's game cells at the given coordinates.
    /// </summary>
    /// <param name="row">The zero-based row coordinate.</param>
    /// <param name="column">The zero-based column coordinate.</param>
    /// <returns>The <see cref="GameCell"/> from the <see cref="GameGrid"/> game cells at the given coordinates.</returns>
    public GameCell this[int row, int column] { get => _gameCells[row, column]; }

    /// <summary>
    /// Gets or sets the start time.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Creates an instance of the <see cref="GameGrid"/>.
    /// </summary>
    public GameGrid() => Initialize();

    /// <summary>
    /// Initializes the game state.
    /// </summary>
    private void Initialize()
    {
        // Fill game cells with new GameCell instances.
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++) _gameCells[i, j] = new();
        }
        // Set each game cell's references to its NEWS neighbors.
        GameCell gameCell;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                gameCell = _gameCells[i, j];
                if (i > 0) gameCell.N = _gameCells[i - 1, j];
                if (j < 3) gameCell.E = _gameCells[i, j + 1];
                if (j > 0) gameCell.W = _gameCells[i, j - 1];
                if (i < 3) gameCell.S = _gameCells[i + 1, j];
            }
        }
        _memory2D = _gameCells; // Wrap the game cells...
        EmptyValues();
        Goal = 2048;
        Moves = 0;
        Score = 0;
        IsWon = false;
        GameTime = new TimeSpan(0);
        StartTime = default;
    }

    /// <summary>
    /// Starts the game by filling two game cells in the <see cref="GameGrid"/>.
    /// </summary>
    public bool Start()
    {
        if (_gameCells[0,0] is null) Initialize();
        Fill(2); // Fill a couple of random game cells.
        return true;
    }

    /// <summary>
    /// Loads a game using the given game data.
    /// </summary>
    /// <param name="values">The row-major order list of the <see cref="GameGrid"/> game cells values.</param>
    /// <param name="goal">The <see cref="Goal"/>.</param>
    /// <param name="moves">The number of <see cref="Moves"/>.</param>
    /// <param name="score">The <see cref="Score"/>.</param>
    /// <param name="isWon">The <see cref="IsWon"/> indicator.</param>
    /// <param name="gameTime">The <see cref="GameTime"/>.</param>
    public void Load(int[] values, int goal, int moves, int score, bool isWon, TimeSpan gameTime)
    {
        if (_gameCells[0, 0] is null) Initialize();
        int index = 0;
        foreach (GameCell gameCell in _memory2D.Span)
        {
            int value = values[index];
            gameCell.Value = value;
            Values[index] = value; // Take advantage of this loop to force a deep sync to the base game model.
            index++;
        }
        Goal = goal;
        Moves = moves;
        Score = score;
        IsWon = isWon;
        GameTime = gameTime;
    }

    /// <summary>
    /// Loads a game using the given game data.
    /// </summary>
    /// <param name="gameData">The game data as an instance of <see cref="GameModel"/>.</param>
    public void Load(GameModel gameData) => Load(gameData.Values, gameData.Goal, gameData.Moves, gameData.Score, gameData.IsWon, gameData.GameTime);

    /// <summary>
    /// Randomly finds in the <see cref="GameGrid"/> a 0-valued <see cref="GameCell"/>, and randomly sets its value to either 2 or 4.
    /// </summary>
    /// <param name="count">The number of 0-valued game cells to fill. Default is 1.</param>
    private void Fill(int count = 1)
    {
        List<GameCell> zeroes = new();
        foreach (GameCell gameCell in _memory2D.Span)
        {
            // List all 0-valued game cells in the game grid.
            if (gameCell.Value == 0) zeroes.Add(gameCell);
        }
        if (zeroes.Count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                // Randomly find a 0-valued game cell in the list, and randomly set its value to either 2 or 4.
                int z = GameLibrary.GameRandomizer.Next(zeroes.Count);
                zeroes[z].Value = GameRandomizer.Next();
                zeroes.RemoveAt(z);
                if (zeroes.Count == 0) break;
            }
        }
        SyncValues(); // Force a deep sync to the base game model.
    }

    /// <summary>
    /// Syncs the <see cref="GameGrid"/> game cells values to the base <see cref="GameModel.Values"/>.
    /// </summary>
    /// <remarks>
    /// <para>Internally, the game grid manages its game cells, a 4x4 matrix (2D array).</para>
    /// <para>There are codes in place to sync the game cells values to the base game model values.</para>
    /// <para>Regardless, it is recommended to call this method before casting a game grid instance to <see cref="GameModel"/>.</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// gameGrid.SyncValues(); // Force a deep sync to the base game model.
    /// GameModel gameData = (GameModel)gameGrid;
    /// </code>
    /// </example>
    public void SyncValues()
    {
        int index = 0;
        foreach (GameCell gameCell in _memory2D.Span)
        {
            base.Values[index++] = gameCell.Value;
        }
    }

    /// <summary>
    /// Evaluate the game move to update the game's state.
    /// </summary>
    /// <param name="gameMove">The <see cref="GameMove"/> to evaluate.</param>
    /// <returns>True if a game cell value was moved, else false.</returns>
    /// <remarks>If a <see cref="GameCell"/> value was moved, increments <see cref="Moves"/> and calls <see cref="Fill"/>.</remarks>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="gameMove"/> is invalid or not supported.</exception>
    public bool Action(GameMove gameMove)
    {
        bool moved = false;
        Span2D<GameCell> span2D = _memory2D.Span;
        for (int i = 0; i < 4; i++)
        {
            // Wishing RefEnumerable has a Reverse() method...
            GameCell[] slice = gameMove switch
            {
                GameMove.None => throw new ArgumentException("Invalid or unsupported game move.", nameof(gameMove)),
                GameMove.Up => span2D.GetColumn(i).ToArray(),
                GameMove.Right => span2D.GetRow(i).ToArray().Reverse().ToArray(),
                GameMove.Left => span2D.GetRow(i).ToArray(),
                GameMove.Down => span2D.GetColumn(i).ToArray().Reverse().ToArray(),
                _ => throw new ArgumentException("Invalid or unsupported game move.", nameof(gameMove))
            };
            for (int j = 0; j < 4; j++)
            {
                // Move values together to fill zeroes
                int z = j;
                for (int k = j; k < 4; k++)
                {
                    int value = slice[k].Value;
                    if (value == 0) continue;
                    if (z < k)
                    {
                        slice[z].Value = value;
                        slice[k].Value = 0;
                        moved = true;
                    }
                    z++;
                }
                // Merge matching values
                if (j < 3)
                {
                    GameCell target = slice[j];
                    GameCell source = slice[j + 1];
                    int t = target.Value;
                    if (t != 0)
                    {
                        int s = source.Value;
                        if (t == s)
                        {
                            target.Value += s; // Increment the target game cell value with the source game cell value.
                            Score += target.Value; // Increment the Score with the target game cell's new value.
                            if (target.Value == Goal)
                            {
                                IsWon = true; // If the Goal is reached, the game is won!
                                CalculateGameTime();
                            }
                            source.Value = 0; // Set the source game cell value to 0.
                            moved = true; // Flag a successful move.
                        }
                    }
                }
            }
        }
        if (moved)
        {
            Moves++; // Make the move count.
            Fill(); // Fill a random game cell.
        }
        return moved;
    }

    /// <summary>
    /// If <see cref="IsWon"/> is true, continues the game by doubling the <see cref="Goal"/> and resetting <see cref="IsWon"/> to false.
    /// </summary>
    public bool Continue()
    {
        if (IsWon)
        {
            Goal *= 2;
            IsWon = false;
        }
        return true;
    }

    /// <summary>
    /// Checks if the game is over.
    /// </summary>
    /// <returns>True if game over, else false.</returns>
    /// <remarks>
    /// <para>It is not game over when a <see cref="GameCell"/> value is 0 or equal to a NEWS neighbor cell value.</para>
    /// <para>If it's game over, <see cref="CalculateGameTime"/> is called.</para>
    /// </remarks>
    public bool GameOver()
    {
        bool isGameOver = true; // Temporarily assume it's game over.
        bool check(GameCell? neighbor, int value)
        {
            if (neighbor is not null)
            {
                int v = neighbor.Value;
                if (v == 0 || v == value) isGameOver = false;
            }
            return isGameOver;
        };
        foreach (GameCell gameCell in _memory2D.Span)
        {
            int value = gameCell.Value;
            if (value == 0 || 
                (!check(gameCell.N, value) 
                    || !check(gameCell.E, value) 
                    || !check(gameCell.W, value) 
                    || !check(gameCell.S, value)))
            {
                isGameOver = false;
                break; // Stop checking if it's not game over.
            }
        }
        if (isGameOver) CalculateGameTime();
        return isGameOver;
    }

    /// <summary>
    /// Calculates the <see cref="GameTime"/> and resets the <see cref="StartTime"/>.
    /// </summary>
    public void CalculateGameTime()
    {
        static DateTime removeMilliseconds(DateTime dateTime) => dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));
        DateTime startTime = StartTime;
        if (startTime != default) GameTime += removeMilliseconds(DateTime.Now) - removeMilliseconds(startTime);
        StartTime = default;
    }

    /// <summary>
    /// Ends the game.
    /// </summary>
    public void End()
    {
        // Free up some memory...
        EmptyValues();
        _memory2D = null;
        _gameCells = new GameCell[4, 4];
    }
}