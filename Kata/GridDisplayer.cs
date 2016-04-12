using System.Runtime.Serialization.Formatters;

namespace Kata
{
    public class GridDisplayer
    {
        private readonly Board _board;
        private int _movesTaken;

        public GridDisplayer(Board board)
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

            if (_board.TopRowIsFilled())
                GameIsOver = true;
        }


        private static int GetColumnIndex(string input)
        {
            return int.Parse(input) - 1;
        }
    }
}