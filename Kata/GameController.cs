using System;
using System.Collections.Generic;
using System.Threading;

namespace Kata
{
    public class GameController
    {
        protected readonly Board Board;
        private readonly int _sleepTime;
        private readonly View _view;
        private int _movesTaken;

        public GameController(Board board, View view, int sleepTime = 0)
        {
            Board = board;
            _view = view;
            _movesTaken = 0;
            CanAcceptInput = true;
            _sleepTime = sleepTime;
        }

        public GameController(int boardSize, IRandomGenerator randomGenerator, View view, int sleepTime = 0)
            : this(new Board(boardSize, randomGenerator), view, sleepTime)
        {
        }

        public bool CanAcceptInput { get; set; }

        public bool GameIsOver { get; set; }

        public void DisplayBoard()
        {
            _view.Clear();
            _view.Write(Board.Display());
        }

        public void DisplayScore()
        {
            var score = Board.GetScore();
            var output = $"{score.Item1,-10}         {score.Item2,10}" + Environment.NewLine;
            _view.Write(output);
        }

        public void DoMove(string input)
        {
            if (string.IsNullOrEmpty(input))
                return;
            CanAcceptInput = false;
            var column = GetColumnIndex(input);
            Board.PlaceChip(column);
            UpdateGameState();
        }

        public void Draw()
        {
            DisplayBoard();
            DisplayScore();
            Thread.Sleep(_sleepTime);
        }

        private bool ColumnOverflowed()
        {
            if (!Board.ColumnOverFlowed())
                return false;
            GameIsOver = true;
            Draw();
            return true;
        }

        private void PopAndSleep(List<Tuple<int, int>> clearedCells)
        {
            if (clearedCells.Count == 0)
                return;
            Board.ClearPoppedCells(clearedCells);
            Draw();
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

        private void UpdateGameState()
        {
            _movesTaken++;
            Board.ResetScore();

            ProcessBoardChanges();
            if (_movesTaken%5 == 0)
            {
                Board.AddBlockRow();
                ProcessBoardChanges();
            }
            CanAcceptInput = true;
            if (Board.TopRowIsFilled())
                GameIsOver = true;
        }

        private static int GetColumnIndex(string input)
        {
            return int.Parse(input) - 1;
        }
    }
}
