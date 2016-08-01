using Kata;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ScoringTests
    {
        [SetUp]
        public void SetUp()
        {
            _testObj = new Scoring(9);
        }

        private Scoring _testObj;

        [TestCase(9, 18)]
        [TestCase(5, 10)]
        public void ScoringIsBasedOnBoardSize(int boardSize, int expectedScore)
        {
            var testObj = new Scoring(boardSize);
            testObj.AddPoints(2);
            var score = testObj.GetScore();
            Assert.That(score.Item1, Is.EqualTo(expectedScore));
            Assert.That(score.Item2, Is.EqualTo(expectedScore));
        }

        private void AssertScore(int expectedTotal, int expectedCurrent)
        {
            var score = _testObj.GetScore();
            Assert.That(score.Item1, Is.EqualTo(expectedTotal));
            Assert.That(score.Item2, Is.EqualTo(expectedCurrent));
        }


        [Test]
        public void AddBlockRow_Adds17000Points()
        {
            _testObj.AddBlockRow();
            AssertScore(17000, 17000);
        }

        [Test]
        public void CascadingAddPointsDoesNewCalculation()
        {
            _testObj.AddPoints(1);
            _testObj.AddPoints(2);
            _testObj.AddPoints(3);
            AssertScore(530, 420);
        }

        [Test]
        public void DisplayScore_ShowsTheTotalScoreOnTheBottomLeftAndCurrentOnTheBottomRight()
        {
            _testObj.AddPoints(5);
            AssertScore(45, 45);
        }

        [Test]
        public void DisplayScore_StaysAt30Characters()
        {
            _testObj.AddPoints(500);
            _testObj.Reset();
            _testObj.AddPoints(5);
            _testObj.AddPoints(5);

            AssertScore(4799, 254);
        }

        [Test]
        public void Reset_ResetsCascadeCounter()
        {
            _testObj.AddPoints(1);
            _testObj.Reset();
            _testObj.AddPoints(1);
            AssertScore(18, 9);
        }
    }
}