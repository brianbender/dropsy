namespace Kata
{
    public class Board
    {
        private readonly int _size;
        private readonly IRandomGenerator _randomGenerator;
        private string[,] _cellContents;
        private string _randomPiece;

        public Board(int size, IRandomGenerator randomGenerator)
        {
            _size = size;
            _randomGenerator = randomGenerator;
            CreateCells();
            _randomPiece = _randomGenerator.GetRandom(_size);
        }

        private void CreateCells()
        {
            _cellContents = new string[_size, _size];
            for (var col = 0; col < _size; col++)
            {
                for (var row = 0; row < _size; row++)
                {
                    SetCellContent(row, col, " ");
                }
            }
        }
        public bool TopRowIsFilled()
        {
            for (var col = 0; col < _size; col++)
            {
                if (CellIsEmpty(0, col))
                    return false;
            }
            return true;
        }
        private void SetCellContent(int row, int col, string content)
        {
            _cellContents[row, col] = content;
        }
        private string GetCellContent(int row, int column)
        {
            return _cellContents[row, column];
        }

        public string DrawCell(int column, int row)
        {
            return $" {GetCellContent(row, column)} ";
        }

        private bool CellIsEmpty(int row, int column)
        {
            return GetCellContent(row, column).Equals(" ");
        }
        public void PlaceChip(int column)
        {
            for (var row = _size - 1; row >= 0; row--)
            {
                if (!CellIsEmpty(row, column)) continue;
                SetCellContent(row, column, _randomPiece);
                _randomPiece = _randomGenerator.GetRandom(_size);
                return;
            }
        }

        public string GetNextPiece()
        {
            return _randomPiece;
        }
    }
}