using System;
using Kata;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void SetUp()
        {
            _fakeRandomGenerator = new FakeRandomGenerator(1);
        }

        private FakeRandomGenerator _fakeRandomGenerator;
        private readonly FakeConsoleWrapper _fakeConsoleWrapper = new FakeConsoleWrapper();

        [Test]
        public void AddingChipToOneByOneMeansGameOver()
        {
            _fakeRandomGenerator.NumberToReturn = 7;
            var testObj = new GameController(new Board(1, _fakeRandomGenerator), _fakeConsoleWrapper);
            Assert.False(testObj.GameIsOver);
            testObj.DoMove("1");
            Assert.True(testObj.GameIsOver);
        }

        [Test]
        public void AddingChipToTwoByTwoDoesNotMeanGameOver()
        {
            var testObj = new GameController(new Board(2, _fakeRandomGenerator), _fakeConsoleWrapper);
            testObj.DoMove("1");
            Assert.False(testObj.GameIsOver);
        }

        [Test]
        public void AfterPlacingFivePiecesMakeARowOfBlocks()
        {
            _fakeRandomGenerator.NumberToReturn = 9;
            var testObj = new GameController(new Board(3, _fakeRandomGenerator), _fakeConsoleWrapper);
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DoMove("2");
            testObj.DoMove("2");
            testObj.DoMove("3");
            testObj.DisplayBoard();
            Assert.That(_fakeConsoleWrapper.LastWrite, Is.EqualTo(
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
        public void CallingDisplayTwiceDisplaysTheSameBoard()
        {
            _fakeRandomGenerator.NumberToReturn = 2;
            var testObj = new GameController(new Board(2, _fakeRandomGenerator), _fakeConsoleWrapper);
            testObj.DisplayBoard();
            var firstBoard = _fakeConsoleWrapper.LastWrite;
            _fakeRandomGenerator.NumberToReturn = 1;
            testObj.DisplayBoard();
            var secondBoard = _fakeConsoleWrapper.LastWrite;
            Assert.That(firstBoard, Is.EqualTo(secondBoard));
        }

        [Test]
        public void Display1DisplaysA1X1Board()
        {
            var testObj = new GameController(new Board(1, _fakeRandomGenerator), _fakeConsoleWrapper);
            testObj.DisplayBoard();
            var output = _fakeConsoleWrapper.LastWrite;
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
            var testObj = new GameController(new Board(2, _fakeRandomGenerator), _fakeConsoleWrapper);
            testObj.DisplayBoard();
            var output = _fakeConsoleWrapper.LastWrite;
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
            var testObj = new GameController(new Board(9, _fakeRandomGenerator), _fakeConsoleWrapper);
            testObj.DisplayBoard();
            var output = _fakeConsoleWrapper.LastWrite;

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
        public void GameOverIfPieceGoesOffEdge()
        {
            _fakeRandomGenerator.NumberToReturn = 7;
            var testObj = new GameController(new Board(3, _fakeRandomGenerator), _fakeConsoleWrapper);
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DoMove("2");
            Assert.That(testObj.GameIsOver, Is.False);
            testObj.DoMove("3");
            Assert.That(testObj.GameIsOver, Is.True);
        }

        [Test]
        public void SelectColumn_DoesNotChangeBoardIfSelectedColumnFull()
        {
            _fakeRandomGenerator.NumberToReturn = 7;
            var testObj = new GameController(new Board(2, _fakeRandomGenerator), _fakeConsoleWrapper);
            testObj.DoMove("2");
            _fakeRandomGenerator.NumberToReturn = 6;
            testObj.DoMove("2");
            testObj.DisplayBoard();

            var first = _fakeConsoleWrapper.LastWrite;
            _fakeRandomGenerator.NumberToReturn = 7;
            testObj.DoMove("2");
            testObj.DisplayBoard();
            var second = _fakeConsoleWrapper.LastWrite;
            Assert.That(first, Is.EqualTo(second));
        }

        [Test]
        public void SelectColumn_PutsPiecesOnTheBoard()
        {
            _fakeRandomGenerator.NumberToReturn = 6;
            var testObj = new GameController(new Board(2, _fakeRandomGenerator), _fakeConsoleWrapper);
            testObj.DisplayBoard();
            _fakeRandomGenerator.NumberToReturn = 5;
            testObj.DoMove("2");
            testObj.DisplayBoard();
            var output = _fakeConsoleWrapper.LastWrite;
            var expected = "   5    " + Environment.NewLine +
                           "┌──────┐" + Environment.NewLine +
                           "│      │" + Environment.NewLine +
                           "│    6 │" + Environment.NewLine +
                           "└──────┘" + Environment.NewLine +
                           "  1  2  " + Environment.NewLine;

            Assert.That(output, Is.EqualTo(expected));
            testObj.DoMove("2");
            testObj.DisplayBoard();

            output = _fakeConsoleWrapper.LastWrite;
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