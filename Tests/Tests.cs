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
        }

        [Test]
        public void CallingDisplayTwiceDisplaysTheSameBoard()
        {
            _fakeRandomGenerator.NumberToReturn = 2;
            var testObj = new GridDisplayer(_fakeRandomGenerator, 2);
            var firstBoard = testObj.DisplayBoard();
            _fakeRandomGenerator.NumberToReturn = 1;
            var secondBoard = testObj.DisplayBoard();
            Assert.That(firstBoard, Is.EqualTo(secondBoard));
        }

        [Test]
        public void Display1DisplaysA1X1Board()
        {
            var testObj = new GridDisplayer(_fakeRandomGenerator, 1);
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
            var testObj = new GridDisplayer(_fakeRandomGenerator, 2);
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
            var testObj = new GridDisplayer(_fakeRandomGenerator, 9);
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
        public void SelectColumn_PutsPiecesOnTheBoard()
        {
            _fakeRandomGenerator.NumberToReturn = 2;
            var testObj = new GridDisplayer(_fakeRandomGenerator, 2);
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