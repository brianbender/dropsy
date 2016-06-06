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
        private readonly FakeConsoleWrapper _consoleWrapper = new FakeConsoleWrapper();

        [Test]
        public void AddingChipToOneByOneMeansGameOver()
        {
            _fakeRandomGenerator.NumberToReturn = 7;
            var testObj = new GameController(new Board(1, _fakeRandomGenerator), _consoleWrapper);
            Assert.False(testObj.GameIsOver);
            testObj.DoMove("1");
            Assert.True(testObj.GameIsOver);
        }

        [Test]
        public void AddingChipToTwoByTwoDoesNotMeanGameOver()
        {
            var testObj = new GameController(new Board(2, _fakeRandomGenerator), _consoleWrapper);
            testObj.DoMove("1");
            Assert.False(testObj.GameIsOver);
        }

        [Test]
        public void AfterPlacingFivePiecesMakeARowOfBlocks()
        {
            _fakeRandomGenerator.NumberToReturn = 9;
            var testObj = new GameController(new Board(3, _fakeRandomGenerator), _consoleWrapper);
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
        public void PopingNumberInTopOfBlockCracksBlock()
        {
            _fakeRandomGenerator.NumberToReturn = 3;
            var testObj = new GameController(new Board(3, _fakeRandomGenerator), _consoleWrapper);
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
                "│         │" + Environment.NewLine +
                "│ ▓  ▓  ▓ │" + Environment.NewLine +
                "└─────────┘" + Environment.NewLine +
                "  1  2  3  " + Environment.NewLine
                ));
        }

        [Test]
        public void PopingNumberInTopOfCrackedBlockRevealsNumber()
        {
            _fakeRandomGenerator.NumberToReturn = 3;
            var testObj = new GameController(new Board(3, _fakeRandomGenerator), _consoleWrapper);
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DoMove("2");
            testObj.DoMove("2");
            _fakeRandomGenerator.NumberToReturn = 2;
            testObj.DoMove("3");
            testObj.DoMove("1");
            testObj.DisplayBoard();
            Assert.That(_consoleWrapper.LastWrite, Is.EqualTo(
               "     2     " + Environment.NewLine +
               "┌─────────┐" + Environment.NewLine +
               "│         │" + Environment.NewLine +
               "│         │" + Environment.NewLine +
               "│ 2  ▓  ▓ │" + Environment.NewLine +
               "└─────────┘" + Environment.NewLine +
               "  1  2  3  " + Environment.NewLine
               ));
        }

        [Test]
        public void CallingDisplayTwiceDisplaysTheSameBoard()
        {
            _fakeRandomGenerator.NumberToReturn = 2;
            var testObj = new GameController(new Board(2, _fakeRandomGenerator), _consoleWrapper);
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
            var board = new Board(3, _fakeRandomGenerator);
            var testObj = new GameController(board, _consoleWrapper);
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
            var board = new Board(3, _fakeRandomGenerator);
            var testObj = new GameController(board, _consoleWrapper);
            testObj.DoMove("2");
            testObj.DoMove("1");
            _fakeRandomGenerator.NumberToReturn = 2;
            testObj.DoMove("1");
            testObj.DoMove("1");
            testObj.DisplayBoard();
            var expected = "     2     \r\n┌─────────┐\r\n│         │\r\n│         │\r\n│    3    │\r\n└─────────┘\r\n  1  2  3  \r\n";
            Assert.That(_consoleWrapper.LastWrite, Is.EqualTo(expected));
        }


        [Test]
        public void Display1DisplaysA1X1Board()
        {
            var testObj = new GameController(new Board(1, _fakeRandomGenerator), _consoleWrapper);
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
            var testObj = new GameController(new Board(2, _fakeRandomGenerator), _consoleWrapper);
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
            var testObj = new GameController(new Board(9, _fakeRandomGenerator), _consoleWrapper);
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
        public void GameOverIfPieceGoesOffEdge()
        {
            _fakeRandomGenerator.NumberToReturn = 7;
            var testObj = new GameController(new Board(3, _fakeRandomGenerator), _consoleWrapper);
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
            var testObj = new GameController(new Board(2, _fakeRandomGenerator), _consoleWrapper);
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
            var testObj = new GameController(new Board(2, _fakeRandomGenerator), _consoleWrapper);
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