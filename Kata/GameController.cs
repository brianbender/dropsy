using System.Threading;

namespace Kata
{
    public class GameController
    {
        private readonly Board _board;
        private readonly ConsoleWrapper _consoleWrapper;
        private readonly int _sleepTime;
        private int _movesTaken;

        public GameController(Board board, ConsoleWrapper consoleWrapper, int sleepTime = 0)
        {
            _board = board;
            _consoleWrapper = consoleWrapper;
            _movesTaken = 0;
            CanAcceptInput = true;
            _sleepTime = sleepTime;
        }

        public bool GameIsOver { get; set; }
        public bool CanAcceptInput { get; set; }

        public void DoMove(string input)
        {
            if (string.IsNullOrEmpty(input))
                return;
            CanAcceptInput = false;
            var column = GetColumnIndex(input);
            _board.PlaceChip(column);
            UpdateGameState();
        }

        public void DisplayBoard()
        {
            _consoleWrapper.Clear();
            _consoleWrapper.Write(_board.Display());
        }

        private void UpdateGameState()
        {
            _movesTaken++;
            if (_movesTaken%5 == 0)
            {
                _board.AddBlockRow();
            }
            var clearedCells = _board.ClearNumbers();

            while (clearedCells.Count != 0)
            {
                DisplayBoard();
                Thread.Sleep(_sleepTime);
                _board.ClearPoppedCells(clearedCells);
                clearedCells = _board.ClearNumbers();
            }
            CanAcceptInput = true;

            if (_board.TopRowIsFilled() || _board.ColumnOverFlowed())
                GameIsOver = true;
        }


        private static int GetColumnIndex(string input)
        {
            return int.Parse(input) - 1;
        }
    }
}