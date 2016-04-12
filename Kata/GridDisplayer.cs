namespace Kata
{
    public class GridDisplayer
    {
        private readonly Board _board;

        public GridDisplayer(Board board)
        {
            _board = board;
        }

        public bool GameIsOver { get; set; }

        public void SelectColumn(string input)
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
            if (_board.TopRowIsFilled())
                GameIsOver = true;
        }

        private static int GetColumnIndex(string input)
        {
            return int.Parse(input) - 1;
        }
    }
}