namespace GameLibrary2048;

/// <summary>
/// Defines a game cell.
/// </summary>
public class GameCell
{
    /// <summary>
    /// Gets the game cell's N (north) neighbor. Null if the game cell is on the top row of the game grid.
    /// </summary>
    public GameCell? N { get; internal set; }

    /// <summary>
    /// Gets the game cell's E (east) neighbor. Null if the game cell is on the rightmost row of the game grid.
    /// </summary>
    public GameCell? E { get; internal set; }

    /// <summary>
    /// Gets the game cell's W (west) neighbor. Null if the game cell is on the leftmost row of the game grid.
    /// </summary>
    public GameCell? W { get; internal set; }

    /// <summary>
    /// Gets the game cell's S (south) neighbor. Null if the game cell is on the bottom row of the game grid.
    /// </summary>
    public GameCell? S { get; internal set; }

    /// <summary>
    /// Gets the game cell's value.
    /// </summary>
    public int Value { get; internal set; }
}