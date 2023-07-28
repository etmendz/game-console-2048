namespace GameLibrary2048;

/// <summary>
/// Defines the game randomizer.
/// </summary>
internal static class GameRandomizer
{
    /// <summary>
    /// Gets a random number less than the maximum limit.
    /// </summary>
    /// <param name="limit">The maximum limit.</param>
    /// <returns>A random number less than the maximum limit.</returns>
    public static int Next(int limit) => Random.Shared.Next(limit);

    /// <summary>
    /// Randomly gets 2 or 4, calibrated so there's a 90% chance that 2 is returned.
    /// </summary>
    /// <returns>Returns 2, else 4.</returns>
    public static int Next() => Random.Shared.NextDouble() < 0.9 ? 2 : 4;
}