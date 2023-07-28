namespace GameConsole2048;

/// <summary>
/// Represents a game stat.
/// </summary>
internal class GameStat : ICloneable
{
    /// <summary>
    /// Gets or sets the best score.
    /// </summary>
    public int BestScore { get; set; }

    /// <summary>
    /// Evaluates the best score. 
    /// </summary>
    /// <param name="score">If greater, replaces the best score.</param>
    /// <returns>The <see cref="BestScore"/>.</returns>
    public int EvaluateBestScore(int score)
    {
        if (BestScore < score) BestScore = score;
        return BestScore;
    }

    #region ICloneable
    /// <summary>
    /// Clones a game stat instance.
    /// </summary>
    /// <returns>A copy of the <see cref="GameStat"/> instance.</returns>
    public object Clone() => new GameStat() { BestScore = this.BestScore };
    #endregion

    #region Overrides
    public override bool Equals(object? obj) => obj is not null && Equals((GameStat)obj);

    public bool Equals(GameStat gameStat) => gameStat.BestScore == this.BestScore;

    public override int GetHashCode() => HashCode.Combine(BestScore);
    #endregion
}