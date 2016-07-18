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
            Assert.That(testObj.CurrentScore, Is.EqualTo(expectedScore));
        }

        [Test]
        public void CascadingAddPointsDoesNewCalculation()
        {
            _testObj.AddPoints(1);
            _testObj.AddPoints(2);
            _testObj.AddPoints(3);
            Assert.That(_testObj.TotalScore, Is.EqualTo(530));
            Assert.That(_testObj.CurrentScore, Is.EqualTo(530));
        }

        [Test]
        public void DisplayScore_ShowsTheTotalScoreOnTheBottomLeftAndCurrentOnTheBottomRight()
        {
            _testObj.AddPoints(5);
            var output = _testObj.GetScoreDisplay();
            Assert.That(output, Is.EqualTo("45                         45"));
        }

        [Test]
        public void DisplayScore_StaysAt30Characters()
        {
            _testObj.AddPoints(500);
            _testObj.Reset();
            _testObj.AddPoints(5);
            _testObj.AddPoints(5);

            var output = _testObj.GetScoreDisplay();
            Assert.That(output, Is.EqualTo("4799                      299"));
        }


        [Test]
        public void NextCard()
        {
            Assert.Fail("Every time you add a new block row, add 17000 points");
        }

        [Test]
        public void Reset_ClearsCurrentScore()
        {
            _testObj.AddPoints(1);
            Assert.That(_testObj.CurrentScore, Is.EqualTo(9));
            _testObj.Reset();
            Assert.That(_testObj.CurrentScore, Is.EqualTo(0));
            Assert.That(_testObj.TotalScore, Is.EqualTo(9));
        }

        [Test]
        public void Reset_ResetsCascadeCounter()
        {
            _testObj.AddPoints(1);
            _testObj.Reset();
            _testObj.AddPoints(1);
            Assert.That(_testObj.TotalScore, Is.EqualTo(18));
            Assert.That(_testObj.CurrentScore, Is.EqualTo(9));
        }
    }
}