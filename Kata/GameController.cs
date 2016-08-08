using System;
using System.Collections.Generic;
using System.Threading;

namespace Kata
{
    public class GameController
    {
        private readonly ConsoleWrapper _consoleWrapper;
        private readonly int _sleepTime;
        protected readonly Board Board;
        private int _movesTaken;

        public GameController(Board board, ConsoleWrapper consoleWrapper, int sleepTime = 0)
        {
            Board = board;
            _consoleWrapper = consoleWrapper;
            _movesTaken = 0;
            CanAcceptInput = true;
            _sleepTime = sleepTime;

        }

        public GameController(int boardSize, IRandomGenerator randomGenerator, ConsoleWrapper consoleWrapper,
            int sleepTime = 0)
            : this(new Board(boardSize, randomGenerator), consoleWrapper, sleepTime)
        {
        }

        public bool GameIsOver { get; set; }
        public bool CanAcceptInput { get; set; }

        public void DoMove(string input)
        {
            if (string.IsNullOrEmpty(input))
                return;
            CanAcceptInput = false;
            var column = GetColumnIndex(input);
            Board.PlaceChip(column);
            UpdateGameState();
        }

        public void DisplayBoard()
        {
            _consoleWrapper.Clear();
            _consoleWrapper.Write(Board.Display());
        }

        public void Draw()
        {
            DisplayBoard();
            DisplayScore();
            Thread.Sleep(_sleepTime);
        }

        private void UpdateGameState()
        {
            _movesTaken++;
            Board.ResetScore();

            ProcessBoardChanges();
            if (_movesTaken % 5 == 0)
            {
                Board.AddBlockRow();
                ProcessBoardChanges();
            }
            CanAcceptInput = true;
            if (Board.TopRowIsFilled())
                GameIsOver = true;
        }

        private bool ColumnOverflowed()
        {
            if (!Board.ColumnOverFlowed()) return false;
            GameIsOver = true;
            Draw();
            return true;
        }

        private void ProcessBoardChanges()
        {
            if (ColumnOverflowed())
            {
                GameIsOver = true;
                return;
            }
            var clearedCells = new List<Tuple<int, int>>();
            do
            {
                Draw();
                PopAndSleep(clearedCells);
                clearedCells = Board.ClearNumbers();
                Board.AddPoints(clearedCells.Count);
            } while (clearedCells.Count != 0);
        }

        private void PopAndSleep(List<Tuple<int, int>> clearedCells)
        {
            if (clearedCells.Count == 0) return;
            Board.ClearPoppedCells(clearedCells);
            Draw();
        }

        private static int GetColumnIndex(string input)
        {
            return int.Parse(input) - 1;
        }

        public void DisplayScore()
        {
            var score = Board.GetScore();
            var output = $"{score.Item1,-10}         {score.Item2,10}" + Environment.NewLine;
            _consoleWrapper.Write(output);
        }
    }
}