/*
* GameConsole2048 (c) Mendz, etmendz. All rights reserved. 
* SPDX-License-Identifier: GPL-3.0-or-later 
*/
using GameLibrary;
using System.Text;

namespace GameConsole2048;

/// <summary>
/// Defines the game UI.
/// </summary>
internal class GameUI : IGameUI<GameGrid, GameMove, bool>
{
    // Game UI rendering constants...
    const string Row = "---------------------------------";
    const string Filler = "|       |       |       |       |";
    const char Column = '|';
    const int Width = 7;
    const int Capacity = 593;

    // Game IO constants...
    private readonly string _gameDataFile = Path.Combine(AppContext.BaseDirectory, "GameConsole2048.GameData.json");
    private readonly string _gameStatFile = Path.Combine(AppContext.BaseDirectory, "GameConsole2048.GameStat.json");

    // The refresh state...
    private bool _refresh = false;

    // Allocate a string builder once to help optimize UI rendering...
    private readonly StringBuilder _stringBuilder = new(Capacity);

    // Saved game stat...
    private readonly GameStat _savedGameStat;

    /// <summary>
    /// Gets the game stat.
    /// </summary>
    public GameStat GameStat { get; private set; }

    /// <summary>
    /// Gets the game grid.
    /// </summary>
    public GameGrid GamePlay { get; set; }

    /// <summary>
    /// Creates an instance of the <see cref="GameUI"/>.
    /// </summary>
    /// <remarks>Loads the <see cref="GameStat"/>.</remarks>
    public GameUI()
    {
        GameStat = GameStatIO.Read(_gameStatFile) ?? new();
        _savedGameStat = (GameStat)GameStat.Clone();
        GamePlay = new();
    }

    /// <summary>
    /// Starts the game. If saved game data is available, it is loaded. If the loaded game is won, prompts to continue the game.
    /// </summary>
    /// <returns>True if a game is started, else false.</returns>
    public bool Start()
    {
        AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        GameModel? gameData = GameModelIO.Read(_gameDataFile); // Read a saved game data if there's any.
        if (gameData is null) GamePlay.Start(); // Start a new game.
        else
        {
            GamePlay.Load(gameData); // Load a saved game data if there's any.
            File.Delete(_gameDataFile); // There's no need for the saved game data after it's loaded.
        }
        Console.Clear();
        Render();
        GamePlay.StartTime = DateTime.Now; // Start the timer...
        return Continue();
    }

    /// <summary>
    /// Renders the game UI.
    /// </summary>
    /// <remarks>May also update the <see cref="GameStat"/>.</remarks>
    public void Render()
    {
        // Render the header area...
        int score = GamePlay.Score;
        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"Goal: {GamePlay.Goal}\tBest: {GameStat.EvaluateBestScore(score)}" + 
            Environment.NewLine + 
            $"Moves: {GamePlay.Moves}\tScore: {score}");
        Console.WriteLine();
        // Render the game grid area...
        _stringBuilder.Length = 0; // Make sure that the string builder is empty.
        for (int i = 0; i < 4; i++)
        {
            _stringBuilder.AppendLine(Row);
            _stringBuilder.AppendLine(Filler);
            _stringBuilder.Append(Column);
            for (int j = 0; j < 4; j++)
            {
                int value = GamePlay[i, j].Value;
                string v = value == 0 ? " " : value.ToString(); // Don't show 0's.
                int l = v.Length;
                _stringBuilder.Append(v.PadLeft(Convert.ToInt32((Width - l) / 2) + l).PadRight(Width));
                _stringBuilder.Append(Column);
            }
            _stringBuilder.AppendLine();
            _stringBuilder.AppendLine(Filler);
        }
        _stringBuilder.Append(Row);
        Console.WriteLine(_stringBuilder.ToString());
        _stringBuilder.Length = 0; // Make sure that the string builder is empty.
    }

    /// <summary>
    /// Refreshes the game UI.
    /// </summary>
    public void Refresh() => Render();

    /// <summary>
    /// Waits for a game move.
    /// </summary>
    /// <returns>True if a move was made, else false.</returns>
    public bool Action() => GamePlay.Action(GameUX.GetMove());

    /// <summary>
    /// Continues the game. If the game is won, prompts to continue the game.
    /// </summary>
    /// <returns>True if the game is not yet won. Else, true if the player opts to continue, else false.</returns>
    /// <remarks>If the player opts to continue, record breaking <see cref="GameStat"/> changes are saved.</remarks>
    public bool Continue()
    {
        bool isContinue = true;
        if (_refresh) Refresh();
        if (GamePlay.IsWon)
        {
            Console.WriteLine();
            Console.WriteLine($"Goal! {FormatGameTime()}");
            Console.Write("Continue? (Y/N): "); // Prompt to continue or not.
            if (new GameConsoleUX().GetYN() == ConsoleKey.Y)
            {
                SaveGameStat();
                GamePlay.Continue();
                Console.Clear();
                Refresh();
                GamePlay.StartTime = DateTime.Now; // Restart the timer...
            }
            else isContinue = false;
        }
        _refresh = true; // After the first call, refresh state is always true.
        return isContinue;
    }

    /// <summary>
    /// Checks if the game is over.
    /// </summary>
    /// <returns>True if game over, else false.</returns>
    public bool GameOver() => GamePlay.GameOver();

    /// <summary>
    /// Ends the game.
    /// </summary>
    /// <remarks>Record breaking <see cref="GameStat"/> changes are saved and announced.</remarks>
    public void End()
    {
        AppDomain.CurrentDomain.ProcessExit -= CurrentDomain_ProcessExit;
        GamePlay.End();
        Console.WriteLine();
        if (GamePlay.IsWon) Console.WriteLine("You win!");
        else Console.WriteLine($"Game over! {FormatGameTime()}");
        if (SaveGameStat())
        {
            Console.WriteLine();
            Console.WriteLine($"New best score: {GameStat.BestScore}!");
        }
    }

    /// <summary>
    /// Formats the game time message.
    /// </summary>
    /// <returns>A formatted message to show the game time.</returns>
    private string FormatGameTime() => $"In {GamePlay.GameTime:g} of game time.";

    /// <summary>
    /// Saves the game stat if the saved game stat is not equal to the game stat.
    /// </summary>
    /// <returns>True if the <see cref="GameStat"/> is saved, else false.</returns>
    private bool SaveGameStat()
    {
        bool saved = false;
        if (!GameStat.Equals(_savedGameStat))
        {
            GameStatIO.Write(_gameStatFile, GameStat);
            _savedGameStat.BestScore = GameStat.BestScore;
            saved = true;
        }
        return saved;
    }

    /// <summary>
    /// The process exit handler to <see cref="GameGrid.CalculateGameTime"/>, save game data, and <see cref="SaveGameStat"/>.
    /// </summary>
    private void CurrentDomain_ProcessExit(object? sender, EventArgs e)
    {
        AppDomain.CurrentDomain.ProcessExit -= CurrentDomain_ProcessExit;
        if (_refresh) GamePlay.CalculateGameTime();
        GamePlay.SyncValues(); // Force a deep sync to the base game model.
        GameModelIO.Write(_gameDataFile, GamePlay);
        SaveGameStat();
    }
}