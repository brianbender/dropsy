using System;
using System.Collections.Generic;
using System.Linq;

namespace Kata
{
    public class Board
    {
        private const string UpperLeft = "┌";
        private const string UpperRight = "┐";
        private const string LowerLeft = "└";
        private const string LowerRight = "┘";
        private const string HorizontalBorder = "───";
        private const string VerticalBorder = "│";
        private const string LabelFiller = "  ";
        private const string EmptySpace = " ";
        private const string Block = "█";
        private const string Pop = "*";
        private readonly IRandomGenerator _randomGenerator;
        private readonly int _size;
        private string _bottomDisplay;
        private string[,] _cellContents;
        private bool _columnOverFlowed;
        private string _randomPiece;
        private string _topDisplay;

        public Board(int size, IRandomGenerator randomGenerator)
        {
            _size = size;
            _randomGenerator = randomGenerator;
            CreateCells();
            CreateTopAndBottom();
            _randomPiece = _randomGenerator.GetRandom(_size);
        }

        public string Display()
        {
            var middle = DrawInside();
            return DisplayNextMove(_size) + _topDisplay + middle +
                   _bottomDisplay;
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

        private string MakeLabel()
        {
            var output = LabelFiller;
            for (var i = 0; i < _size; i++)
                output += i + 1 + LabelFiller;
            return output + Environment.NewLine;
        }

        private void CreateTopAndBottom()
        {
            var output = "";
            for (var i = 0; i < _size; i++)
                output += HorizontalBorder;
            var horizontalEdge = output;
            _bottomDisplay = LowerLeft + horizontalEdge + LowerRight + Environment.NewLine + MakeLabel();
            _topDisplay = UpperLeft + horizontalEdge + UpperRight + Environment.NewLine;
        }

        private string DisplayNextMove(int size)
        {
            var chars = size*3 + 2;
            var top = Enumerable.Repeat(EmptySpace, chars).ToArray();
            top[(chars - 1)/2] = _randomPiece;
            return string.Join("", top) + Environment.NewLine;
        }

        private string DrawInside()
        {
            var middle = "";
            for (var row = 0; row < _size; row++)
            {
                var rowText = "";
                for (var column = 0; column < _size; column++)
                {
                    rowText += DrawCell(column, row);
                }
                middle += string.Format("{0}{1}{0}{2}", VerticalBorder, rowText, Environment.NewLine);
            }
            return middle;
        }

        private void SetCellContent(int row, int col, string content)
        {
            _cellContents[row, col] = content;
        }

        private string GetCellContent(int row, int column)
        {
            return _cellContents[row, column];
        }

        private string DrawCell(int column, int row)
        {
            return $" {GetCellContent(row, column)} ";
        }

        private bool CellIsEmpty(int row, int column)
        {
            return GetCellContent(row, column).Equals(" ");
        }

        public void AddBlockRow()
        {
            for (var col = 0; col < _size; ++col)
            {
                if (!CellIsEmpty(0, col))
                    _columnOverFlowed = true;
            }
            for (var row = 1; row < _size; ++row)
            {
                for (var col = 0; col < _size; ++col)
                {
                    SetCellContent(row - 1, col, GetCellContent(row, col));
                    if (row == _size - 1)
                        SetCellContent(_size - 1, col, Block);
                }
            }
        }

        public bool ColumnOverFlowed()
        {
            return _columnOverFlowed;
        }

        public void ClearNumbers()
        {
            var numbersToClear = new Dictionary<int, HashSet<int>>();


            for (var col = 0; col < _size; col++)
            {
                numbersToClear.Add(col, DoColumnWork(col, 0));
            }

            foreach (var kvp in numbersToClear)
                foreach (var row in kvp.Value)
                    SetCellContent(row, kvp.Key, Pop);
        }

        private HashSet<int> DoColumnWork(int col, int startingRow)
        {
            var numberInSeries = 0;
            var columnsToClear = new HashSet<int>();
            for (var row = startingRow; row < _size; row++)
            {
                if (CellIsEmpty(row, col))
                {
                    columnsToClear.UnionWith(DoColumnWork(col, startingRow + 1));
                    break;
                }
                numberInSeries++;
            }

            //set of numbers to check is start to end
            for (var row = startingRow; row < startingRow + numberInSeries; row++)
            {
                var cellContent = GetCellContent(row, col);
                if (cellContent == numberInSeries.ToString())
                    columnsToClear.Add(row);
            }
            return columnsToClear;
        }
    }
}