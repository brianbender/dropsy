using System;

namespace Kata
{
    public class Scoring
    {
        private readonly int _boardSize;
        private int _cascadeCount;
        private double _currentScore;
        private double _totalScore;

        public Scoring(int boardSize)
        {
            _boardSize = boardSize;
            _currentScore = 0;
            _totalScore = 0;
        }

        public void AddPoints(int countOfClearedCells)
        {
            _cascadeCount++;
            var addedScore = Math.Floor(countOfClearedCells*_boardSize*Math.Pow(_cascadeCount, 2.5));
            _currentScore = addedScore;
            _totalScore += addedScore;
        }

        public Tuple<double, double> GetScore()
        {
            return new Tuple<double, double>(_totalScore, _currentScore);
        }

        public void Reset()
        {
            _cascadeCount = 0;
        }

        public void AddBlockRow()
        {
            _currentScore = 17000;
            _totalScore += 17000;
        }
    }
}