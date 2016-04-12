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
        private const string LabelFiller = "  ";
        private const string EmptySpace = " ";
        private const string Block = "█";
        private readonly int _size;
        private string _bottomDisplay;
        private string _topDisplay;
        private readonly Board _board;

        public GridDisplayer(int size, Board board)
        {
            _size = size;
            CreateTopAndBottom();
            _board = board;
        }

        public bool GameIsOver { get; set; }


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

            var column = GetColumnIndex(input);
            _board.PlaceChip(column);
            UpdateGameState();
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
                    rowText += _board.DrawCell(column, row);
                }
                middle += string.Format("{0}{1}{0}{2}", VerticalBorder, rowText, Environment.NewLine);
            }
            return middle;
        }

    
        private string DisplayNextMove(int size)
        {
            var chars = size*3 + 2;
            var top = Enumerable.Repeat(EmptySpace, chars).ToArray();
            top[(chars - 1)/2] = _board.GetNextPiece();
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