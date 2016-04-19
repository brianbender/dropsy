using System;
using Kata;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class BoardTests
    {
        [SetUp]
        public void SetUp()
        {
            _fakeRandomGenerator = new FakeRandomGenerator(1);
        }

        private FakeRandomGenerator _fakeRandomGenerator;

        [Test]
        public void AddBlockRow_PlacesBlockRowOnBottom()
        {
            var testObj = new Board(2, _fakeRandomGenerator);
            testObj.AddBlockRow();
            var expected = "   1    " + Environment.NewLine +
                           "┌──────┐" + Environment.NewLine +
                           "│      │" + Environment.NewLine +
                           "│ █  █ │" + Environment.NewLine +
                           "└──────┘" + Environment.NewLine +
                           "  1  2  " + Environment.NewLine;

            Assert.That(testObj.Display(), Is.EqualTo(expected));
        }

        [Test]
        public void AddBlockRow_ShiftsExistingCellsUp()
        {
            var testObj = new Board(2, _fakeRandomGenerator);
            testObj.AddBlockRow();
            testObj.AddBlockRow();
            var expected = "   1    " + Environment.NewLine +
                           "┌──────┐" + Environment.NewLine +
                           "│ █  █ │" + Environment.NewLine +
                           "│ █  █ │" + Environment.NewLine +
                           "└──────┘" + Environment.NewLine +
                           "  1  2  " + Environment.NewLine;

            Assert.That(testObj.Display(), Is.EqualTo(expected));
        }

        [Test]
        public void ChipRemoval_DoesNotClearForColumnOfTwoNumbersWithNoTwos()
        {
            _fakeRandomGenerator.NumberToReturn = 3;
            var testObj = new Board(2, _fakeRandomGenerator);
            testObj.PlaceChip(0);
            testObj.PlaceChip(0);
            var expected = testObj.Display();
            testObj.ClearNumbers();
            var result = testObj.Display();
            Assert.That(result, Is.EqualTo(expected));
        }


        [Test]
        public void ChipRemoval_DoesClearForColumnOfTwoNumbersWithTwos()
        {
            _fakeRandomGenerator.NumberToReturn = 3;
            var testObj = new Board(3, _fakeRandomGenerator);
            _fakeRandomGenerator.NumberToReturn = 4;
            testObj.PlaceChip(0);
            _fakeRandomGenerator.NumberToReturn = 3;
            testObj.PlaceChip(0);
            testObj.PlaceChip(0);
            testObj.ClearNumbers();
            var result = testObj.Display();
            var expected = "     3     \r\n┌─────────┐\r\n│ *       │\r\n│ 4       │\r\n│ *       │\r\n└─────────┘\r\n  1  2  3  \r\n";
            Assert.That(result, Is.EqualTo(expected));
        }


        [Test]
        public void ChipRemoval_TestOne()
        {
            var testObj = new Board(1, _fakeRandomGenerator);
            testObj.PlaceChip(0);
            testObj.ClearNumbers();
            var expected = "  1  \r\n┌───┐\r\n│ * │\r\n└───┘\r\n  1  \r\n";
            Assert.That(testObj.Display(), Is.EqualTo(expected));
        }

        [Test]
        public void ColumnOverFlowed_ReturnsFalseOnEmptyBoard()
        {
            var testObj = new Board(2, _fakeRandomGenerator);
            Assert.False(testObj.ColumnOverFlowed());
        }

        [Test]
        public void ColumnOverFlowed_ReturnsTrueIfColumnOverflowed()
        {
            var testObj = new Board(2, _fakeRandomGenerator);
            testObj.PlaceChip(1);
            testObj.PlaceChip(1);
            testObj.AddBlockRow();
            Assert.True(testObj.ColumnOverFlowed());
        }
    }
}