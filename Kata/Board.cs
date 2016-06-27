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
        private const string Pop = "*";
        public static readonly string Block = "█";
        public static readonly string CrackedBlock = "▓";
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
            _randomPiece = GetRandomChip();
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
                _randomPiece = GetRandomChip();
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

        protected void SetCellContent(int row, int col, string content)
        {
            _cellContents[row, col] = content;
        }

        protected string GetCellContent(int row, int column)
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

        public List<Tuple<int, int>> ClearNumbers()
        {
            var numbersToClear = new List<Tuple<int, int>>();

            for (var col = 0; col < _size; col++)
            {
                var columns = DoColumnWork(col, 0);
                numbersToClear = numbersToClear.Union(columns).ToList();
            }

            for (var row = 0; row < _size; row++)
            {
                var rows = DoRowWork(row, 0);
                numbersToClear = numbersToClear.Union(rows).ToList();
            }

            foreach (var tuple in numbersToClear)
            {
                SetCellContent(tuple.Item1, tuple.Item2, Pop);
                CrackAdjacentBlocks(tuple.Item1, tuple.Item2);
            }
            return numbersToClear.OrderBy(c=>c.Item1).ThenBy(c=>c.Item2).ToList();
        }

        private void CrackAdjacentBlocks(int row, int column)
        {
            CrackBlock(row + 1, column);
            CrackBlock(row - 1, column);
            CrackBlock(row, column + 1);
            CrackBlock(row, column - 1);
        }

        private void CrackBlock(int row, int column)
        {
            if (CellContentIs(row, column, Block))
                SetCellContent(row, column, CrackedBlock);
            else if (CellContentIs(row, column, CrackedBlock))
                SetCellContent(row, column, GetRandomChip());
        }

        private bool CellContentIs(int row, int column, string contents)
        {
            return CellExists(row, column) && GetCellContent(row, column) == contents;
        }

        private bool CellExists(int row, int column)
        {
            return row < _size && column < _size && row >= 0 && column >= 0;
        }

        private string GetRandomChip()
        {
            return _randomGenerator.GetRandom(_size);
        }

        private IEnumerable<Tuple<int, int>> DoRowWork(int row, int startingCol)
        {
            var numberInSeries = 0;
            var columnsToClear = new List<Tuple<int, int>>();
            for (var col = startingCol; col < _size; col++)
            {
                if (CellIsEmpty(row, col))
                {
                    columnsToClear.AddRange(DoRowWork(row, col + 1));
                    break;
                }
                numberInSeries++;
            }

            //set of numbers to check is start to end
            for (var col = startingCol; col < startingCol + numberInSeries; col++)
            {
                var cellContent = GetCellContent(row, col);
                if (cellContent == numberInSeries.ToString())
                    columnsToClear.Add(new Tuple<int, int>(row, col));
            }
            return columnsToClear;
        }

        private IEnumerable<Tuple<int, int>> DoColumnWork(int col, int startingRow)
        {
            var numberInSeries = 0;
            var cellsToClear = new List<Tuple<int, int>>();
            for (var row = startingRow; row < _size; row++)
            {
                if (CellIsEmpty(row, col))
                {
                    cellsToClear.AddRange(DoColumnWork(col, startingRow + 1));
                    break;
                }
                numberInSeries++;
            }

            //set of numbers to check is start to end
            for (var row = startingRow; row < startingRow + numberInSeries; row++)
            {
                var cellContent = GetCellContent(row, col);
                if (cellContent == numberInSeries.ToString())
                    cellsToClear.Add(new Tuple<int, int>(row, col));
            }
            return cellsToClear;
        }

        public void ClearPoppedCells(List<Tuple<int, int>> clearedCells)
        {
            foreach (var tuple in clearedCells)
            {
                if (GetCellContent(tuple.Item1, tuple.Item2) != Block)
                {
                    PopAndDrop(tuple.Item1, tuple.Item2);
                }
            }
        }

        private void PopAndDrop(int row, int col)
        {
            SetCellContent(row, col, EmptySpace);
            for (var i = row; i > 0; --i)
            {
                SetCellContent(i, col, GetCellContent(i - 1, col));
            }
            SetCellContent(0, col, EmptySpace);
        }
    }
}