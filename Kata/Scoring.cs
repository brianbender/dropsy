using System;

namespace Kata
{
    public class Scoring
    {
        private readonly int _boardSize;
        public double CurrentScore;
        public double TotalScore;
        private int _cascadeCount = 0;

        public Scoring(int boardSize)
        {
            _boardSize = boardSize;
            CurrentScore = 0;
            TotalScore = 0;
        }

        public void AddPoints(int countOfClearedCells)
        {
            _cascadeCount++;
            var addedScore = Math.Floor(countOfClearedCells*_boardSize* Math.Pow(_cascadeCount, 2.5));
            CurrentScore += addedScore;
            TotalScore += addedScore;
        }

        public string GetScoreDisplay()
        {
            return $"{TotalScore,-10}         {CurrentScore,10}";
        }

        public void Reset()
        {
            CurrentScore = 0;
            _cascadeCount = 0;
        }
    }
}