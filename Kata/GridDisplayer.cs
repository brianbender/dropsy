using System;
using System.Linq;

namespace Kata
{
    public class GridDisplayer
    {
        private const string UpperLeft = "┌";
        private const string UpperRight = "┐";
        private const string LowerLeft = "└";
        private const string LowerRight = "┘";
        private const string HorizontalBorder = "───";
        private const string VerticalBorder = "│";
        private const string HorizontalFiller = "   ";
        private const string LabelFiller = "  ";
        private const string EmptySpace = " ";
        private readonly IRandomGenerator _randomGenerator;
        private readonly int _size;
        private string _bottomDisplay;
        private string _randomPiece;
        private int _selectedColumn;
        private string _topDisplay;

        public GridDisplayer(IRandomGenerator randomGenerator, int size)
        {
            _randomGenerator = randomGenerator;
            _size = size;
            CreateTopAndBottom();
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

        public void SelectColumn(string input)
        {
            var column = GetColumn(input);
            _selectedColumn = column;
        }

        private static int GetColumn(string input)
        {
            return int.Parse(input);
        }

        public string DisplayBoard()
        {
            var middle = DrawInside();
            return DisplayNextMove(_size) + _topDisplay + middle +
                   _bottomDisplay;
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

        private string DrawCell(int column, int row)
        {
            string cellText;
            if (column == _selectedColumn - 1 && (row == _size - 1))
            {
                cellText = $" {_randomPiece} ";
                _randomPiece = " ";
            }
            else
            {
                cellText = HorizontalFiller;
            }
            return cellText;
        }

        private string DisplayNextMove(int size)
        {
            _randomPiece = _randomGenerator.GetRandom(size);
            var chars = size*3 + 2;
            var top = Enumerable.Repeat(EmptySpace, chars).ToArray();
            top[(chars - 1)/2] = _randomPiece;
            return string.Join("", top) + Environment.NewLine;
        }

        private string MakeLabel()
        {
            var output = LabelFiller;
            for (var i = 0; i < _size; i++)
                output += i + 1 + LabelFiller;
            return output + Environment.NewLine;
        }
    }
}