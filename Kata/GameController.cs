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
        private readonly Scoring _scoring;

        private GameController(Board board, ConsoleWrapper consoleWrapper, int sleepTime = 0)
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
            _scoring = new Scoring(boardSize);
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
        }

        private void UpdateGameState()
        {
            _movesTaken++;

            ProcessBoardChanges();
            if (_movesTaken%5 == 0)
            {
                Board.AddBlockRow();
                ProcessBoardChanges();
            }
            CanAcceptInput = true;

            if (Board.TopRowIsFilled() || Board.ColumnOverFlowed())
                GameIsOver = true;
        }

        private void ProcessBoardChanges()
        {
            _scoring.Reset();
            var clearedCells = new List<Tuple<int, int>>();
            do
            {
                Draw();
                Thread.Sleep(_sleepTime);
                Board.ClearPoppedCells(clearedCells);
                Draw();
                Thread.Sleep(_sleepTime);
                clearedCells = Board.ClearNumbers();
                _scoring.AddPoints(clearedCells.Count);
            } while (clearedCells.Count != 0);
        }

        private static int GetColumnIndex(string input)
        {
            return int.Parse(input) - 1;
        }

        public void DisplayScore()
        {
            _consoleWrapper.Write(_scoring.GetScoreDisplay() + Environment.NewLine);
        }
    }
}