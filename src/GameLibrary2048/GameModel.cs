namespace GameLibrary2048;

/// <summary>
/// Defines the <see cref="GameGrid"/> game data model.
/// </summary>
public class GameModel
{
    /// <summary>
    /// Gets or sets the row-major order list of the <see cref="GameGrid"/> game cells values.
    /// </summary>
    public int[] Values { get; set; } = new int[16];

    /// <summary>
    /// Gets or sets the game's goal. The initial value is 2048.
    /// </summary>
    public int Goal { get; set; } = 2048;

    /// <summary>
    /// Gets or sets the game's number of moves.
    /// </summary>
    public int Moves { get; set; }

    /// <summary>
    /// Gets or sets the game's score.
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Gets or sets an indicator if the game is won or not. The initial value is false.
    /// </summary>
    public bool IsWon { get; set; }

    /// <summary>
    /// Gets or sets the game time.
    /// </summary>
    public TimeSpan GameTime { get; set; } = new TimeSpan(0);

    /// <summary>
    /// Creates an instance of <see cref="GameModel"/>.
    /// </summary>
    public GameModel() { }

    /// <summary>
    /// Empties the <see cref="Values"/> by assigning 0 to all of its elements.
    /// </summary>
    protected void EmptyValues() => Array.Fill(Values, 0);
}