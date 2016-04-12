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

        [Test]
        public void AddingChipToOneByOneMeansGameOver()
        {
            var testObj = new GridDisplayer(new Board(1, _fakeRandomGenerator));
            Assert.False(testObj.GameIsOver);
            testObj.SelectColumn("1");
            Assert.True(testObj.GameIsOver);
        }

        [Test]
        public void AddingChipToTwoByTwoDoesNotMeanGameOver()
        {
            var testObj = new GridDisplayer(new Board(2, _fakeRandomGenerator));
            testObj.SelectColumn("1");
            Assert.False(testObj.GameIsOver);
        }

        [Test]
        public void CallingDisplayTwiceDisplaysTheSameBoard()
        {
            _fakeRandomGenerator.NumberToReturn = 2;
            var testObj = new GridDisplayer(new Board(2, _fakeRandomGenerator));
            var firstBoard = testObj.DisplayBoard();
            _fakeRandomGenerator.NumberToReturn = 1;
            var secondBoard = testObj.DisplayBoard();
            Assert.That(firstBoard, Is.EqualTo(secondBoard));
        }

        [Test]
        public void Display1DisplaysA1X1Board()
        {
            var testObj = new GridDisplayer(new Board(1, _fakeRandomGenerator));
            var output = testObj.DisplayBoard();
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
            var testObj = new GridDisplayer(new Board(2, _fakeRandomGenerator));
            var output = testObj.DisplayBoard();
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
            var testObj = new GridDisplayer(new Board(9, _fakeRandomGenerator));
            var output = testObj.DisplayBoard();

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
        public void SelectColumn_DoesNotChangeBoardIfSelectedColumnFull()
        {
            _fakeRandomGenerator.NumberToReturn = 1;
            var testObj = new GridDisplayer(new Board(2, _fakeRandomGenerator));
            testObj.SelectColumn("2");
            _fakeRandomGenerator.NumberToReturn = 2;
            testObj.SelectColumn("2");
            var first = testObj.DisplayBoard();
            _fakeRandomGenerator.NumberToReturn = 1;
            testObj.SelectColumn("2");
            var second = testObj.DisplayBoard();
            Assert.That(first, Is.EqualTo(second));
        }

        [Test]
        public void SelectColumn_PutsPiecesOnTheBoard()
        {
            _fakeRandomGenerator.NumberToReturn = 2;
            var testObj = new GridDisplayer(new Board(2, _fakeRandomGenerator));
            testObj.DisplayBoard();
            _fakeRandomGenerator.NumberToReturn = 1;
            testObj.SelectColumn("2");
            var output = testObj.DisplayBoard();
            var expected = "   1    " + Environment.NewLine +
                           "┌──────┐" + Environment.NewLine +
                           "│      │" + Environment.NewLine +
                           "│    2 │" + Environment.NewLine +
                           "└──────┘" + Environment.NewLine +
                           "  1  2  " + Environment.NewLine;

            Assert.That(output, Is.EqualTo(expected));
            testObj.SelectColumn("2");
            output = testObj.DisplayBoard();

            expected = "   1    " + Environment.NewLine +
                       "┌──────┐" + Environment.NewLine +
                       "│    1 │" + Environment.NewLine +
                       "│    2 │" + Environment.NewLine +
                       "└──────┘" + Environment.NewLine +
                       "  1  2  " + Environment.NewLine;

            Assert.That(output, Is.EqualTo(expected));
        }
    }
}