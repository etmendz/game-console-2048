namespace GameLibrary2048.Tests;

[TestClass()]
public class GameGridTests
{
    // These control game data sets focus on representing key turning points during game play,
    // while also creating and capturing basic and common scenarios to test.
    private static readonly int[] zeroes = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private static readonly int[] winrow = new int[16] { 1024, 1024, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private static readonly int[] wonrow = new int[16] { 2048, 4, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private static readonly int[] wincolumn = new int[16] { 1024, 0, 0, 0, 1024, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0 };
    private static readonly int[] woncolumn = new int[16] { 2048, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0 };
    private static readonly int[] gameover = new int[16] { 2, 4, 8, 16, 32, 64, 128, 256, 256, 128, 64, 32, 16, 8, 4, 2 };
    // A challenge is how to come up with combinations and permutations of values to cover as many scenarios possible,
    // while also considering the fact that the game grid game cells are randomly filled during game play.

    [TestMethod()]
    public void GameGridTest()
    {
        GameGrid gameGrid = new();
        Assert.AreEqual(16, gameGrid.Values.Length);
        CollectionAssert.AllItemsAreNotNull(gameGrid.Values);
        CollectionAssert.AreEqual(zeroes, gameGrid.Values);
        GameCell gameCell;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                gameCell = gameGrid.GetGameCell(i, j);
                if (i == 0) Assert.IsNull(gameCell.N, "A top row game cell cannot have an N neighbor.");
                if (j == 3) Assert.IsNull(gameCell.E, "A rightmost column game cell cannot have an E neighbor.");
                if (j == 0) Assert.IsNull(gameCell.W, "A leftmost column game cell cannot have a W neighbor.");
                if (i == 3) Assert.IsNull(gameCell.S, "A bottom row game cell cannot have an S neighbor.");
            }
        }
        Assert.AreEqual(2048, gameGrid.Goal);
        Assert.AreEqual(0, gameGrid.Moves);
        Assert.AreEqual(0, gameGrid.Score);
        Assert.AreEqual(false, gameGrid.IsWon);
        Assert.AreEqual(new TimeSpan(0), gameGrid.GameTime);
    }

    [TestMethod()]
    public void StartTest()
    {
        GameGrid gameGrid = new();
        gameGrid.Start();
        CollectionAssert.AreNotEqual(zeroes, gameGrid.Values);
        Assert.IsTrue(gameGrid.Values.Count((value) => value == 2 || value == 4) == 2, "Expecting two game cells to be filled/valued with either 2 or 4.");
        Assert.IsTrue(gameGrid.Values.Count((value) => value == 0) == 14);
    }

    [TestMethod()]
    public void LoadTest()
    {
        GameGrid gameGrid = new();
        gameGrid.Load(winrow, 2048, 1, 1, false, new TimeSpan(0, 15, 0));
        LoadTest_Asserts(gameGrid);
    }

    [TestMethod()]
    public void LoadTest_GameModel()
    {
        GameGrid gameGrid = new();
        gameGrid.Load(new GameModel() { Values = winrow, Moves = 1, Score = 1, IsWon = false, GameTime = new TimeSpan(0, 15, 0) });
        LoadTest_Asserts(gameGrid);
    }

    private static void LoadTest_Asserts(GameGrid gameGrid)
    {
        Assert.AreEqual(2048, gameGrid.Goal);
        Assert.AreEqual(1, gameGrid.Moves);
        Assert.AreEqual(1, gameGrid.Score);
        Assert.AreEqual(false, gameGrid.IsWon);
        Assert.AreEqual(new TimeSpan(0, 15, 0), gameGrid.GameTime);
        SyncValuesTest_Loop(gameGrid);
    }

    [TestMethod()]
    public void SyncValuesTest()
    {
        GameGrid gameGrid = new();
        gameGrid.Start();
        gameGrid.SyncValues();
        SyncValuesTest_Loop(gameGrid);
    }

    private static void SyncValuesTest_Loop(GameGrid gameGrid)
    {
        // Values is a 1D array. Game cells is a 2D array (4x4 matrix).
        // This loop aligns and compares each element in Values to its counterpart in game cells.
        for (int i = 0; i < zeroes.Length; i++)
        {
            int value = gameGrid.Values[i];
            int r = i / 4;
            int c = i % 4;
            int v = gameGrid.GetGameCell(r, c).Value;
            if (value != v) Assert.Fail($"SyncValuesTest failed: {value} at {i} != {v} at ({r}, {c}).");
        }
    }

    [TestMethod()]
    public void GetGameCellTest()
    {
        GameGrid gameGrid = new();
        gameGrid.Start();
        Assert.IsNotNull(gameGrid.GetGameCell(0, 0));
        Assert.IsInstanceOfType(gameGrid.GetGameCell(0, 0), typeof(GameCell));
    }

    [TestMethod()]
    public void MoveTest()
    {
        GameGrid gameGrid = new();
        gameGrid.Start();
        int moves = gameGrid.Moves;
        void move(GameMove gameMove)
        {
            if (gameGrid.Move(gameMove))
            {
                Assert.IsTrue(gameGrid.Moves == moves + 1);
                moves++;
            }
        }
        // Because the game grid's game cells are randomly filled, 
        // this loop is an attempt to increase the chance that all game moves will actually make a move that counts.
        for (int i = 0; i <= 4; i++)
        {
            move(GameMove.Up);
            move(GameMove.Right);
            move(GameMove.Left);
            move(GameMove.Down);
        }
    }

    [TestMethod()]
    public void MoveTest_Up()
    {
        GameGrid gameGrid = new();
        gameGrid.Load(wincolumn, 2048, 1, 1, false, new TimeSpan(0, 15, 0));
        Assert.IsTrue(gameGrid.Move(GameMove.Up));
        Assert.AreEqual(2048, gameGrid.GetGameCell(0, 0).Value);
        Assert.AreEqual(4, gameGrid.GetGameCell(1, 0).Value);
        MoveTest_Asserts(gameGrid);
    }

    [TestMethod()]
    public void MoveTest_Right()
    {
        GameGrid gameGrid = new();
        gameGrid.Load(winrow, 2048, 1, 1, false, new TimeSpan(0, 15, 0));
        Assert.IsTrue(gameGrid.Move(GameMove.Right));
        Assert.AreEqual(2048, gameGrid.GetGameCell(0, 2).Value);
        Assert.AreEqual(4, gameGrid.GetGameCell(0, 3).Value);
        MoveTest_Asserts(gameGrid);
    }

    [TestMethod()]
    public void MoveTest_Left()
    {
        GameGrid gameGrid = new();
        gameGrid.Load(winrow, 2048, 1, 1, false, new TimeSpan(0, 15, 0));
        Assert.IsTrue(gameGrid.Move(GameMove.Left));
        Assert.AreEqual(2048, gameGrid.GetGameCell(0, 0).Value);
        Assert.AreEqual(4, gameGrid.GetGameCell(0, 1).Value);
        MoveTest_Asserts(gameGrid);
    }

    [TestMethod()]
    public void MoveTest_Down()
    {
        GameGrid gameGrid = new();
        gameGrid.Load(wincolumn, 2048, 1, 1, false, new TimeSpan(0, 15, 0));
        Assert.IsTrue(gameGrid.Move(GameMove.Down));
        Assert.AreEqual(2048, gameGrid.GetGameCell(2, 0).Value);
        Assert.AreEqual(4, gameGrid.GetGameCell(3, 0).Value);
        MoveTest_Asserts(gameGrid);
    }

    private static void MoveTest_Asserts(GameGrid gameGrid)
    {
        Assert.AreEqual(2, gameGrid.Moves);
        Assert.AreEqual(2053, gameGrid.Score);
        Assert.IsTrue(gameGrid.IsWon);
        Assert.IsTrue(gameGrid.Values.Count((value) => value == 0) == 13);
    }

    [TestMethod()]
    public void MoveTest_None()
    {
        GameGrid gameGrid = new();
        gameGrid.Load(gameover, 2048, 1, 1, false, new TimeSpan(0));
        MoveTest_None_Asserts(gameGrid);
    }

    private static void MoveTest_None_Asserts(GameGrid gameGrid)
    {
        Assert.IsFalse(gameGrid.Move(GameMove.Up));
        Assert.IsFalse(gameGrid.Move(GameMove.Right));
        Assert.IsFalse(gameGrid.Move(GameMove.Left));
        Assert.IsFalse(gameGrid.Move(GameMove.Down));
    }

    [TestMethod()]
    public void ContinueTest()
    {
        GameGrid gameGrid = new();
        gameGrid.Start();
        gameGrid.Continue();
        Assert.AreEqual(2048, gameGrid.Goal);
        Assert.AreEqual(false, gameGrid.IsWon);
    }

    [TestMethod()]
    public void ContinueTest_IsWon()
    {
        GameGrid gameGrid = new();
        gameGrid.Load(wonrow, 2048, 1, 1, true, new TimeSpan(0, 15, 0));
        gameGrid.Continue();
        Assert.AreNotEqual(2048, gameGrid.Goal);
        Assert.AreEqual(4096, gameGrid.Goal);
        Assert.AreEqual(false, gameGrid.IsWon);
    }

    [TestMethod()]
    public void GameOverTest_NotGameOver()
    {
        GameGrid gameGrid = new();
        gameGrid.Start();
        Assert.IsFalse(gameGrid.GameOver());
    }

    [TestMethod()]
    public void GameOverTest_GameOver()
    {
        GameGrid gameGrid = new();
        gameGrid.Load(gameover, 2048, 1, 1, false, new TimeSpan(0));
        MoveTest_None_Asserts(gameGrid);
        Assert.IsTrue(gameGrid.GameOver());
    }

    [TestMethod()]
    public void CalculateGameTimeTest()
    {
        GameGrid gameGrid = new();
        gameGrid.Load(woncolumn, 2048, 1, 1, true, new TimeSpan(0));
        gameGrid.StartTime = DateTime.Now;
        Thread.Sleep(1000);
        gameGrid.Continue();
        gameGrid.CalculateGameTime();
        Assert.IsTrue(gameGrid.GameTime > new TimeSpan(0));
        Assert.AreEqual(default, gameGrid.StartTime);
    }

    [TestMethod()]
    public void EndTest()
    {
        GameGrid gameGrid = new();
        gameGrid.Start();
        gameGrid.End();
        CollectionAssert.AreEqual(zeroes, gameGrid.Values);
    }
}