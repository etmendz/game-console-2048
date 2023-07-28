using GameLibrary2048;

namespace GameConsole2048;

/// <summary>
/// Provides the game UX capabilities for game flow and game play interactions.
/// </summary>
internal static class GameUX
{
    /// <summary>
    /// Gets a key input.
    /// </summary>
    /// <param name="check">The key input to check for.</param>
    /// <returns>The key input.</returns>
    public static ConsoleKey GetKey(ConsoleKey check)
    {
        ConsoleKey key;
        do
        {
            key = Console.ReadKey(true).Key;
            // Start conditional section for check values with special rules.
            if (check == ConsoleKey.Y)
            {
                if (key == ConsoleKey.N) break; // Check for N
            }
            // End conditional section for check values with special rules.
            if (key == ConsoleKey.Escape) Environment.Exit(0); // [Esc] exits the app.
        } while (key != check); // Loop until the user presses the check key.
        return key;
    }

    /// <summary>
    /// Gets a game move.
    /// </summary>
    /// <returns>The <see cref="GameMove"/>.</returns>
    public static GameMove GetMove()
    {
        GameMove move = GameMove.None;
        do
        {
            // Evaluate a key stroke in to a game move.
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow: move = GameMove.Up; break;
                case ConsoleKey.RightArrow: move = GameMove.Right; break;
                case ConsoleKey.LeftArrow: move = GameMove.Left; break;
                case ConsoleKey.DownArrow: move = GameMove.Down; break;
                case ConsoleKey.Escape: Environment.Exit(0); break; // [Esc] exits the app.
                default: break;
            }
        } while (move == GameMove.None); // Loop until a valid game move is evaluated.
        return move;
    }
}