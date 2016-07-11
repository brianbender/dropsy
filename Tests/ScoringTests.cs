using Kata;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ScoringTests
    {
        private Scoring _testObj;

        [SetUp]
        public void SetUp()
        {
            _testObj = new Scoring(9);
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
            Assert.That(output, Is.EqualTo("4590                       90"));
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

        [TestCase(9, 18)]
        [TestCase(5, 10)]
        public void ScoringIsBasedOnBoardSize(int boardSize, int expectedScore)
        {
            var testObj = new Scoring(boardSize);
            testObj.AddPoints(2);
            Assert.That(testObj.CurrentScore, Is.EqualTo(expectedScore));
        }

    }
}