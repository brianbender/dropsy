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
        private int _selectedColumn;
        private string _randomPiece;

        public GridDisplayer(IRandomGenerator randomGenerator, int size)
        {
            _randomGenerator = randomGenerator;
            _size = size;
            _randomPiece = _randomGenerator.GetRandom(size);
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
            var output = "";
            for (var i = 0; i < _size; i++)
                output += HorizontalBorder;
            var horizontalEdge = output;


            var middle = "";
            for (var row = 0; row < _size; row++)
            {
                var output2 = "";
                for (var column = 0; column < _size; column++)
                {
                    if (column == (_selectedColumn - 1) && (row == _size-1))
                    {
                        output2 += $" {_randomPiece} ";
                        _randomPiece = " ";
                    }
                    else
                    {
                        output2 += HorizontalFiller;
                    }
                }
                middle += VerticalBorder + output2 + VerticalBorder + Environment.NewLine;
            }

            return DisplayNextMove(_size) + DisplayTop(horizontalEdge) + middle +
                   DisplayBottom(horizontalEdge) + MakeLabel(_size);
        }

        private static string DisplayBottom(string horizontalEdge)
        {
            return LowerLeft + horizontalEdge + LowerRight + Environment.NewLine;
        }

        private static string DisplayTop(string horizontalEdge)
        {
            return UpperLeft + horizontalEdge + UpperRight + Environment.NewLine;
        }

        private string DisplayNextMove(int size)
        {
            var chars = size*3 + 2;
            var top = Enumerable.Repeat(EmptySpace, chars).ToArray();
            top[(chars - 1)/2] = _randomPiece;
            return string.Join("", top) + Environment.NewLine;
        }

        private static string MakeLabel(int size)
        {
            var output = LabelFiller;
            for (var i = 0; i < size; i++)
                output += i + 1 + LabelFiller;
            return output + Environment.NewLine;
        }
    }
}