﻿using System;
using Kata;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class GameControllerTests
    {
        [SetUp]
        public void SetUp()
        {
            _fakeRandomGenerator = new FakeRandomGenerator(1);
            _consoleWrapper = new FakeConsoleWrapper();
        }

        private FakeRandomGenerator _fakeRandomGenerator;
        private FakeConsoleWrapper _consoleWrapper;

        private class BrokenEncapsulationGameController : GameController
        {
            public BrokenEncapsulationGameController(int boardSize, IRandomGenerator randomGenerator,
                ConsoleWrapper consoleWrapper, int sleepTime = 0)
                : base(boardSize, randomGenerator, consoleWrapper, sleepTime)
            {
            }

            public Board GetBoard()
            {
                return Board;
            }
        }

        [Test]
        public void AddBlockRowDoesNotDrawTwiceWhenAddingTheBlockRow()
        {
            _fakeRandomGenerator.NumberToReturn = 9;
            var testObj = new GameController(5, _fakeRandomGenerator, _consoleWrapper);
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DoMove("2");
            testObj.DoMove("2");

            var writes = _consoleWrapper.AllWrites.Count;
            Assert.That(_consoleWrapper.AllWrites[writes - 1], Is.Not.EqualTo(_consoleWrapper.AllWrites[writes - 3]));
            Assert.That(_consoleWrapper.AllWrites[writes - 2], Is.Not.EqualTo(_consoleWrapper.AllWrites[writes - 4]));
        }

        [Test]
        public void AddingABlockRow_AdjustsScoring()
        {
            _fakeRandomGenerator.NumberToReturn = 9;

            var testObj = new GameController(5, _fakeRandomGenerator, _consoleWrapper);
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DoMove("2");
            testObj.DoMove("2");

            Assert.That(_consoleWrapper.LastWrite, Is.EqualTo("17000                   17000\r\n"));
        }

        [Test]
        public void AddingChipToTwoByTwoDoesNotMeanGameOver()
        {
            var testObj = new GameController(2, _fakeRandomGenerator, _consoleWrapper);
            testObj.DoMove("1");
            Assert.False(testObj.GameIsOver);
        }


        [Test]
        public void AfterPlacingFivePiecesMakeARowOfBlocks()
        {
            _fakeRandomGenerator.NumberToReturn = 9;
            var testObj = new GameController(3, _fakeRandomGenerator, _consoleWrapper);
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DoMove("2");
            testObj.DoMove("2");
            testObj.DoMove("3");
            testObj.DisplayBoard();
            Assert.That(_consoleWrapper.LastWrite, Is.EqualTo(
                "     9     " + Environment.NewLine +
                "┌─────────┐" + Environment.NewLine +
                "│ 9  9    │" + Environment.NewLine +
                "│ 9  9  9 │" + Environment.NewLine +
                "│ █  █  █ │" + Environment.NewLine +
                "└─────────┘" + Environment.NewLine +
                "  1  2  3  " + Environment.NewLine
                ));
        }

        [Test]
        public void BlockRowsAreAddedAfterPoppingNumbers()
        {
            _fakeRandomGenerator.NumberToReturn = 3;
            var testObj = new GameController(3, _fakeRandomGenerator, _consoleWrapper);
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DoMove("2");
            testObj.DoMove("2");
            testObj.DoMove("3");
            testObj.DisplayBoard();
            Assert.That(_consoleWrapper.LastWrite, Is.EqualTo(
                "     3     " + Environment.NewLine +
                "┌─────────┐" + Environment.NewLine +
                "│         │" + Environment.NewLine +
                "│ 3  3    │" + Environment.NewLine +
                "│ █  █  █ │" + Environment.NewLine +
                "└─────────┘" + Environment.NewLine +
                "  1  2  3  " + Environment.NewLine
                ));
        }

        [Test]
        public void CallingDisplayTwiceDisplaysTheSameBoard()
        {
            _fakeRandomGenerator.NumberToReturn = 2;
            var testObj = new GameController(2, _fakeRandomGenerator, _consoleWrapper);
            testObj.DisplayBoard();
            var firstBoard = _consoleWrapper.LastWrite;
            _fakeRandomGenerator.NumberToReturn = 1;
            testObj.DisplayBoard();
            var secondBoard = _consoleWrapper.LastWrite;
            Assert.That(firstBoard, Is.EqualTo(secondBoard));
        }

        [Test]
        public void CascadeBlockPop()
        {
            _fakeRandomGenerator.NumberToReturn = 2;
            var testObj = new GameController(3, _fakeRandomGenerator, _consoleWrapper);
            testObj.DisplayBoard();
            var emptyBoard = _consoleWrapper.LastWrite;

            _fakeRandomGenerator.NumberToReturn = 3;
            testObj.DoMove("1");
            _fakeRandomGenerator.NumberToReturn = 2;
            testObj.DoMove("3");
            testObj.DoMove("2");

            testObj.DisplayBoard();
            Assert.That(_consoleWrapper.LastWrite, Is.EqualTo(emptyBoard));
        }

        [Test]
        public void CascadeBlockPop2()
        {
            _fakeRandomGenerator.NumberToReturn = 3;
            var testObj = new GameController(3, _fakeRandomGenerator, _consoleWrapper);
            testObj.DoMove("2");
            testObj.DoMove("1");
            _fakeRandomGenerator.NumberToReturn = 2;
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DisplayBoard();
            var expected =
                "     2     \r\n┌─────────┐\r\n│         │\r\n│         │\r\n│    3    │\r\n└─────────┘\r\n  1  2  3  \r\n";
            Assert.That(_consoleWrapper.LastWrite, Is.EqualTo(expected));
        }

        [Test]
        public void CrackingBlocksReavealsDifferentRandomNumbers()
        {
            _fakeRandomGenerator.NumberToReturn = 3;
            var testObj = new GameController(3, _fakeRandomGenerator, _consoleWrapper);
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DoMove("2");
            testObj.DoMove("2");
            testObj.DoMove("3");
            testObj.DoMove("3");
            testObj.DoMove("1");
            testObj.DoMove("2");
            _fakeRandomGenerator.SetRandomNumbers(1, 4, 5, 6);
            testObj.DoMove("3");
            testObj.DisplayBoard();
            Assert.That(_consoleWrapper.LastWrite, Is.EqualTo(
                "     1     " + Environment.NewLine +
                "┌─────────┐" + Environment.NewLine +
                "│         │" + Environment.NewLine +
                "│         │" + Environment.NewLine +
                "│ 4  5  6 │" + Environment.NewLine +
                "└─────────┘" + Environment.NewLine +
                "  1  2  3  " + Environment.NewLine
                ));
        }


        [Test]
        public void Display1DisplaysA1X1Board()
        {
            var testObj = new GameController(1, _fakeRandomGenerator, _consoleWrapper);
            testObj.DisplayBoard();
            var output = _consoleWrapper.LastWrite;
            var expected = "  1  " + Environment.NewLine +
                           "┌───┐" + Environment.NewLine +
                           "│   │" + Environment.NewLine +
                           "└───┘" + Environment.NewLine +
                           "  1  " + Environment.NewLine;

            Assert.That(output, Is.EqualTo(expected));
        }

        [Test]
        public void Display2Displaysa2X2Board()
        {
            var testObj = new GameController(2, _fakeRandomGenerator, _consoleWrapper);
            testObj.DisplayBoard();
            var output = _consoleWrapper.LastWrite;
            var expected = "   1    " + Environment.NewLine +
                           "┌──────┐" + Environment.NewLine +
                           "│      │" + Environment.NewLine +
                           "│      │" + Environment.NewLine +
                           "└──────┘" + Environment.NewLine +
                           "  1  2  " + Environment.NewLine;

            Assert.That(output, Is.EqualTo(expected));
        }

        [Test]
        public void Display9DisplaysA9X9Board()
        {
            _fakeRandomGenerator.NumberToReturn = 4;
            var testObj = new GameController(9, _fakeRandomGenerator, _consoleWrapper);
            testObj.DisplayBoard();
            var output = _consoleWrapper.LastWrite;

            var expected = "              4              " + Environment.NewLine +
                           "┌───────────────────────────┐" + Environment.NewLine +
                           "│                           │" + Environment.NewLine +
                           "│                           │" + Environment.NewLine +
                           "│                           │" + Environment.NewLine +
                           "│                           │" + Environment.NewLine +
                           "│                           │" + Environment.NewLine +
                           "│                           │" + Environment.NewLine +
                           "│                           │" + Environment.NewLine +
                           "│                           │" + Environment.NewLine +
                           "│                           │" + Environment.NewLine +
                           "└───────────────────────────┘" + Environment.NewLine +
                           "  1  2  3  4  5  6  7  8  9  " + Environment.NewLine;

            Assert.That(output, Is.EqualTo(expected));
        }

        [Test]
        public void Draw_DisplaysTheBoardAndTheScore()
        {
            var testObj = new GameController(1, _fakeRandomGenerator, _consoleWrapper);
            testObj.Draw();

            var otherConsoleWrapper = new FakeConsoleWrapper();
            var otherTestObj = new GameController(1, _fakeRandomGenerator, otherConsoleWrapper);
            otherTestObj.DisplayBoard();
            otherTestObj.DisplayScore();

            Assert.That(_consoleWrapper.AllWrites, Is.EqualTo(otherConsoleWrapper.AllWrites));
        }

        [Test]
        public void GameOverIfPieceGoesOffEdge()
        {
            _fakeRandomGenerator.NumberToReturn = 7;
            var testObj = new GameController(3, _fakeRandomGenerator, _consoleWrapper);
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DoMove("2");
            Assert.That(testObj.GameIsOver, Is.False);
            testObj.DoMove("3");
            Assert.That(testObj.GameIsOver, Is.True);
        }

        [Test]
        public void LosingByBlockRowCanNeverPop()
        {
            _fakeRandomGenerator.SetRandomNumbers(2, 4, 5, 7, 2);
            var testObj = new BrokenEncapsulationGameController(4, _fakeRandomGenerator, _consoleWrapper);
            testObj.DoMove("1");
            testObj.DoMove("3");
            testObj.DoMove("2");
            testObj.GetBoard().AddBlockRow();
            testObj.GetBoard().AddBlockRow();
            testObj.DoMove("1");
            testObj.DoMove("4");

            Assert.That(testObj.GameIsOver, Is.True);
            Assert.That(_consoleWrapper.LastWrite, Is.EqualTo("17000                17000\r\n"));
            //Assert.Fail("This test is wrong. we need to verify that if a block row comes in and a pop could have saved you, you are not saved.");
        }

        [Test]
        public void PopAndDropHappenInDiscreteSteps()
        {
            _fakeRandomGenerator.NumberToReturn = 9;
            _fakeRandomGenerator.SetRandomNumbers(1, 2, 4, 3, 2);
            var testObj = new BrokenEncapsulationGameController(3, _fakeRandomGenerator, _consoleWrapper);
            testObj.GetBoard().AddBlockRow();
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DoMove("1");
            var expected =
                "     4     " + Environment.NewLine +
                "┌─────────┐" + Environment.NewLine +
                "│         │" + Environment.NewLine +
                "│         │" + Environment.NewLine +
                "│ 3  █  █ │" + Environment.NewLine +
                "└─────────┘" + Environment.NewLine +
                "  1  2  3  " + Environment.NewLine;

            Assert.That(_consoleWrapper.AllWrites.Contains(expected), Is.True);
        }

        [Test]
        public void PopingNumberInTopOfCrackedBlockRevealsNumber()
        {
            _fakeRandomGenerator.NumberToReturn = 3;
            var testObj = new GameController(3, _fakeRandomGenerator, _consoleWrapper);
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DoMove("2");
            testObj.DoMove("2");
            _fakeRandomGenerator.NumberToReturn = 2;
            testObj.DoMove("3");
            _fakeRandomGenerator.NumberToReturn = 5;
            testObj.DoMove("1");
            testObj.DisplayBoard();
            Assert.That(_consoleWrapper.LastWrite, Is.EqualTo(
                "     5     " + Environment.NewLine +
                "┌─────────┐" + Environment.NewLine +
                "│         │" + Environment.NewLine +
                "│    3    │" + Environment.NewLine +
                "│ 5  █  █ │" + Environment.NewLine +
                "└─────────┘" + Environment.NewLine +
                "  1  2  3  " + Environment.NewLine
                ));
        }

        [Test]
        public void PoppingACellAddsScore()
        {
            _fakeRandomGenerator.NumberToReturn = 1;
            var testObj = new GameController(1, _fakeRandomGenerator, _consoleWrapper);
            testObj.DoMove("1");

            Assert.That(_consoleWrapper.LastWrite, Is.EqualTo("1                           1\r\n"));
        }

        [Test]
        public void SelectColumn_DoesNotChangeBoardIfSelectedColumnFull()
        {
            _fakeRandomGenerator.NumberToReturn = 7;
            var testObj = new GameController(2, _fakeRandomGenerator, _consoleWrapper);
            testObj.DoMove("2");
            _fakeRandomGenerator.NumberToReturn = 6;
            testObj.DoMove("2");
            testObj.DisplayBoard();

            var first = _consoleWrapper.LastWrite;
            _fakeRandomGenerator.NumberToReturn = 7;
            testObj.DoMove("2");
            testObj.DisplayBoard();
            var second = _consoleWrapper.LastWrite;
            Assert.That(first, Is.EqualTo(second));
        }

        [Test]
        public void SelectColumn_PutsPiecesOnTheBoard()
        {
            _fakeRandomGenerator.NumberToReturn = 6;
            var testObj = new GameController(2, _fakeRandomGenerator, _consoleWrapper);
            testObj.DisplayBoard();
            _fakeRandomGenerator.NumberToReturn = 5;
            testObj.DoMove("2");
            testObj.DisplayBoard();
            var output = _consoleWrapper.LastWrite;
            var expected = "   5    " + Environment.NewLine +
                           "┌──────┐" + Environment.NewLine +
                           "│      │" + Environment.NewLine +
                           "│    6 │" + Environment.NewLine +
                           "└──────┘" + Environment.NewLine +
                           "  1  2  " + Environment.NewLine;

            Assert.That(output, Is.EqualTo(expected));
            testObj.DoMove("2");
            testObj.DisplayBoard();

            output = _consoleWrapper.LastWrite;
            expected = "   5    " + Environment.NewLine +
                       "┌──────┐" + Environment.NewLine +
                       "│    5 │" + Environment.NewLine +
                       "│    6 │" + Environment.NewLine +
                       "└──────┘" + Environment.NewLine +
                       "  1  2  " + Environment.NewLine;

            Assert.That(output, Is.EqualTo(expected));
        }
    }
}