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

        public GridDisplayer(IRandomGenerator randomGenerator)
        {
            _randomGenerator = randomGenerator;
        }

        public string DisplayBoard(int size)
        {
            var horizontalEdge = RepeatString(size, HorizontalBorder);
            var middle = RepeatString(size, DisplayMiddle(size));
            return DisplayNextMove(size) + DisplayTop(horizontalEdge) + middle +
                   DisplayBottom(horizontalEdge) + MakeLabel(size);
        }

        private static string DisplayBottom(string horizontalEdge)
        {
            return LowerLeft + horizontalEdge + LowerRight + Environment.NewLine;
        }

        private static string DisplayMiddle(int size)
        {
            return VerticalBorder + RepeatString(size, HorizontalFiller) + VerticalBorder + Environment.NewLine;
        }

        private static string DisplayTop(string horizontalEdge)
        {
            return UpperLeft + horizontalEdge + UpperRight + Environment.NewLine;
        }

        private string DisplayNextMove(int size)
        {
            var randomPiece = _randomGenerator.GetRandom(size);
            var chars = size*3 + 2;
            var top = Enumerable.Repeat(EmptySpace, chars).ToArray();
            top[(chars - 1)/2] = randomPiece;
            return string.Join("", top) + Environment.NewLine;
        }

        private static string MakeLabel(int size)
        {
            var output = LabelFiller;
            for (var i = 0; i < size; i++)
                output += i + 1 + LabelFiller;
            return output + Environment.NewLine;
        }

        private static string RepeatString(int size, string stringToRepeat)
        {
            var output = "";
            for (var i = 0; i < size; i++)
                output += stringToRepeat;
            return output;
        }
    }
}