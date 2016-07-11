namespace Kata
{
    public class Scoring
    {
        private readonly int _boardSize;
        public int CurrentScore;
        public int TotalScore;

        public Scoring(int boardSize)
        {
            _boardSize = boardSize;
            CurrentScore = 0;
            TotalScore = 0;
        }

        public void AddPoints(int countOfClearedCells)
        {
            CurrentScore += countOfClearedCells * _boardSize;
            TotalScore += countOfClearedCells * _boardSize;
        }

        public string GetScoreDisplay()
        {
            return $"{TotalScore,-10}         {CurrentScore,10}";
        }

        public void Reset()
        {
            CurrentScore = 0;
        }
    }
}