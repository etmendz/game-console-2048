﻿/*
* GameLibrary2048.Tests (c) Mendz, etmendz. All rights reserved. 
* Part of GameConsole2048
* SPDX-License-Identifier: GPL-3.0-or-later 
*/
namespace GameLibrary2048.Tests;

[TestClass()]
public class GameModelTests : GameModel
{
    private static readonly int[] _expected = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    [TestMethod()]
    public void GameModelTest()
    {
        Assert.AreEqual(16, Values.Length);
        Assert.AreEqual(2048, Goal);
        Assert.AreEqual(0, Moves);
        Assert.AreEqual(0, Score);
        Assert.AreEqual(false, IsWon);
        Assert.AreEqual(new TimeSpan(0), GameTime);
    }

    [TestMethod()]
    public void EmptyValuesTest()
    {
        EmptyValues();
        CollectionAssert.AllItemsAreNotNull(Values);
        CollectionAssert.AreEqual(_expected, Values);
    }
}