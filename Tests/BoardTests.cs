using System;
using Kata;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class BoardTests
    {
        private FakeRandomGenerator _fakeRandomGenerator;

        [SetUp]
        public void SetUp()
        {
            _fakeRandomGenerator = new FakeRandomGenerator(1);
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

    }
}