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
        public void AfterPlacingFivePiecesMakeARowOfBlocks()
        {
            _fakeRandomGenerator.NumberToReturn = 1;
            var testObj = new Board(3, _fakeRandomGenerator);
            testObj.PlaceChip(0);
            testObj.PlaceChip(0);
            testObj.PlaceChip(1);
            testObj.PlaceChip(1);
            testObj.PlaceChip(2);
            Assert.That(testObj.Display(), Is.EqualTo(
                "     1     " + Environment.NewLine +
                "┌─────────┐" + Environment.NewLine +
                "│ 1  1    │" + Environment.NewLine +
                "│ 1  1  1 │" + Environment.NewLine +
                "│ █  █  █ │" + Environment.NewLine +
                "└─────────┘" + Environment.NewLine +
                "  1  2  3  "
                ));
        }
    }
}