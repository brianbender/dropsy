using System.Timers;

namespace Kata
{
    public class GameController
    {
        private readonly Board _board;
        private int _movesTaken;

        public GameController(Board board)
        {
            _board = board;
            _movesTaken = 0;
        }

        public bool GameIsOver { get; set; }

        public void DoMove(string input)
        {
            var column = GetColumnIndex(input);
            _board.PlaceChip(column);
            UpdateGameState();
        }

        public string DisplayBoard()
        {
            return _board.Display();
        }

        private void UpdateGameState()
        {
            _movesTaken++;
            if (_movesTaken%5 == 0)
            {
                _board.AddBlockRow();
            }
            var clearedCells = _board.ClearNumbers();
            var timer = new Timer(500);
            timer.Elapsed += delegate { _board.ClearPoppedCells(clearedCells); };
            timer.Start();
            if (_board.TopRowIsFilled() || _board.ColumnOverFlowed())
                GameIsOver = true;
        }


        private static int GetColumnIndex(string input)
        {
            return int.Parse(input) - 1;
        }
    }
}