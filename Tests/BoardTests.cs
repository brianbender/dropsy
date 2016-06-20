using System;
using System.Collections;
using System.Collections.Generic;
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

        public class TestBoard : Board
        {
            public TestBoard(int size, IRandomGenerator randomGenerator) : base(size, randomGenerator)
            {
            }

            public void OverrideCellContent(int row, int col, string content)
            {
                SetCellContent(row, col, content);
            }

            public string GetCellContentForTest(int row, int col)
            {
                return GetCellContent(row, col);
            }
        }

        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        public void BlocksCrackInAllDirections(int rowOffset, int colOffset)
        {
            var testObj = new TestBoard(4, _fakeRandomGenerator);
            testObj.OverrideCellContent(2, 2, Board.Block);
            testObj.OverrideCellContent(2 + rowOffset, 2 + colOffset, "2");
            testObj.ClearNumbers();
            Assert.That(testObj.GetCellContentForTest(2, 2), Is.EqualTo(Board.CrackedBlock));
        }

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
            var expected =
                "     3     \r\n┌─────────┐\r\n│ *       │\r\n│ 4       │\r\n│ *       │\r\n└─────────┘\r\n  1  2  3  \r\n";
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ChipRemoval_DoesClearForRowOfTwoNumbersWithTwos()
        {
            _fakeRandomGenerator.NumberToReturn = 2;
            var testObj = new Board(2, _fakeRandomGenerator);
            testObj.PlaceChip(0);
            testObj.PlaceChip(1);
            var clearedNumbers = testObj.ClearNumbers();
            IEnumerable expected = new List<Tuple<int, int>> {new Tuple<int, int>(1, 1), new Tuple<int, int>(1, 0)};
            CollectionAssert.AreEquivalent(expected, clearedNumbers);
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
        public void ChipRemovalDoesNotClearNumbersThatAreNotMatchedToTheNumberInSequence()
        {
            _fakeRandomGenerator.NumberToReturn = 5;
            var testObj = new Board(2, _fakeRandomGenerator);
            testObj.PlaceChip(0);
            var clearedNumbers = testObj.ClearNumbers();
            Assert.That(clearedNumbers, Is.Empty);
        }

        [Test]
        public void ClearNumbers_PopsRowsWithGapsBecauseOfCascade()
        {
            _fakeRandomGenerator.NumberToReturn = 5;
            var testObj = new Board(5, _fakeRandomGenerator);
            _fakeRandomGenerator.NumberToReturn = 2;
            testObj.PlaceChip(2);
            testObj.PlaceChip(0);
            testObj.PlaceChip(1);
            testObj.PlaceChip(3);
            testObj.PlaceChip(4);
            var cells = testObj.ClearNumbers();
            testObj.ClearPoppedCells(cells);
            testObj.ClearNumbers();
            var expected =
                "        2        \r\n" +
                "┌───────────────┐\r\n" +
                "│               │\r\n" +
                "│               │\r\n" +
                "│               │\r\n" +
                "│               │\r\n" +
                "│ *  *     *  * │\r\n" +
                "└───────────────┘\r\n" +
                "  1  2  3  4  5  \r\n";
            Assert.That(testObj.Display(), Is.EqualTo(expected));
        }

        [Test]
        public void ClearNumbers_RemovesAllIn2Steps()
        {
            _fakeRandomGenerator.NumberToReturn = 3;
            var testObj = new Board(3, _fakeRandomGenerator);
            _fakeRandomGenerator.NumberToReturn = 2;

            testObj.PlaceChip(0);
            testObj.PlaceChip(2);
            testObj.PlaceChip(1);
            var cells = testObj.ClearNumbers();
            var expected =
                "     2     \r\n┌─────────┐\r\n│         │\r\n│         │\r\n│ *  2  2 │\r\n└─────────┘\r\n  1  2  3  \r\n";
            Assert.That(testObj.Display(), Is.EqualTo(expected));
            testObj.ClearPoppedCells(cells);
            testObj.ClearNumbers();
            expected =
                "     2     \r\n┌─────────┐\r\n│         │\r\n│         │\r\n│    *  * │\r\n└─────────┘\r\n  1  2  3  \r\n";
            Assert.That(testObj.Display(), Is.EqualTo(expected));
        }

        [Test]
        public void ClearNumbers_RemovesBothInOnce()
        {
            _fakeRandomGenerator.NumberToReturn = 2;
            var testObj = new Board(3, _fakeRandomGenerator);
            _fakeRandomGenerator.NumberToReturn = 1;

            testObj.PlaceChip(0);
            testObj.PlaceChip(1);
            testObj.ClearNumbers();
            var expected =
                "     1     \r\n┌─────────┐\r\n│         │\r\n│         │\r\n│ *  *    │\r\n└─────────┘\r\n  1  2  3  \r\n";
            var result = testObj.Display();

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ClearPoppedCellClearsTwoCollsAndDropsNumber()
        {
            _fakeRandomGenerator.NumberToReturn = 3;
            var testObj = new Board(3, _fakeRandomGenerator);
            testObj.PlaceChip(0);
            _fakeRandomGenerator.NumberToReturn = 2;
            testObj.PlaceChip(0);
            testObj.PlaceChip(0);


            testObj.ClearPoppedCells(new List<Tuple<int, int>> {new Tuple<int, int>(1, 0), new Tuple<int, int>(2, 0)});
            const string expected =
                "     2     \r\n┌─────────┐\r\n│         │\r\n│         │\r\n│ 2       │\r\n└─────────┘\r\n  1  2  3  \r\n";
            Assert.That(testObj.Display(), Is.EqualTo(expected));
        }


        [Test]
        public void ClearPoppedCellsDoesNotClearBlocks()
        {
            var testObj = new Board(2, _fakeRandomGenerator);
            testObj.AddBlockRow();
            var expected = testObj.Display();
            testObj.ClearPoppedCells(new List<Tuple<int, int>> {new Tuple<int, int>(1, 1)});
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

        [Test]
        public void DisconnectedRowsWillPop()
        {
            var testObj = new Board(3, _fakeRandomGenerator);
            testObj.PlaceChip(0);
            testObj.PlaceChip(2);
            var clearedNumbers = testObj.ClearNumbers();
            IEnumerable expected = new List<Tuple<int, int>> {new Tuple<int, int>(2, 0), new Tuple<int, int>(2, 2)};
            CollectionAssert.AreEquivalent(expected, clearedNumbers);
        }

        [Test]
        public void PopAndClearClearsCells()
        {
            var testObj = new Board(1, _fakeRandomGenerator);
            var expected = testObj.Display();
            testObj.PlaceChip(0);
            var clearedCells = testObj.ClearNumbers();
            testObj.ClearPoppedCells(clearedCells);
            Assert.That(testObj.Display(), Is.EqualTo(expected));
        }

        [Test]
        public void PopAndClearClearsCellsAndMovesThemDown()
        {
            _fakeRandomGenerator.NumberToReturn = 2;
            var testObj = new Board(3, _fakeRandomGenerator);
            _fakeRandomGenerator.NumberToReturn = 3;
            testObj.PlaceChip(0);
            testObj.PlaceChip(0);
            var clearedCells = testObj.ClearNumbers();
            testObj.ClearPoppedCells(clearedCells);
            var expected =
                "     3     \r\n┌─────────┐\r\n│         │\r\n│         │\r\n│ 3       │\r\n└─────────┘\r\n  1  2  3  \r\n";
            Assert.That(testObj.Display(), Is.EqualTo(expected));
        }

        [Test]
        public void RowsPopInTwoSteps()
        {
            _fakeRandomGenerator.NumberToReturn = 4;
            var testObj = new Board(6, _fakeRandomGenerator);
            _fakeRandomGenerator.NumberToReturn = 5;
            testObj.PlaceChip(0);
            _fakeRandomGenerator.NumberToReturn = 7;
            testObj.PlaceChip(1);
            _fakeRandomGenerator.NumberToReturn = 3;
            testObj.PlaceChip(2);
            testObj.PlaceChip(3);

            testObj.ClearNumbers();
            var expected =
                "         3          \r\n" +
                "┌──────────────────┐\r\n" +
                "│                  │\r\n" +
                "│                  │\r\n" +
                "│                  │\r\n" +
                "│                  │\r\n" +
                "│                  │\r\n" +
                "│ *  5  7  3       │\r\n" +
                "└──────────────────┘\r\n" +
                "  1  2  3  4  5  6  \r\n";
            var display = testObj.Display();
            Assert.That(display, Is.EqualTo(expected));
        }
    }
}