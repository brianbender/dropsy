using System;
using System.Collections.Generic;
using System.Threading;

namespace Kata
{
    public class GameController
    {
        private readonly int _boardSize;
        private readonly ConsoleWrapper _consoleWrapper;
        private readonly int _sleepTime;
        protected readonly Board Board;
        private int _movesTaken;
        public int CurrentScore;
        public int TotalScore;

        private GameController(Board board, ConsoleWrapper consoleWrapper, int sleepTime = 0)
        {
            Board = board;
            _consoleWrapper = consoleWrapper;
            _movesTaken = 0;
            CanAcceptInput = true;
            _sleepTime = sleepTime;
            CurrentScore = 0;
            TotalScore = 0;
        }

        public GameController(int board, IRandomGenerator randomGenerator, ConsoleWrapper consoleWrapper,
            int sleepTime = 0)
            : this(new Board(board, randomGenerator), consoleWrapper, sleepTime)
        {
            _boardSize = board;
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

        public void DrawGame()
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
            CurrentScore = 0;
            var clearedCells = new List<Tuple<int, int>>();
            do
            {
                DisplayBoard();
                Thread.Sleep(_sleepTime);
                Board.ClearPoppedCells(clearedCells);
                DisplayBoard();
                Thread.Sleep(_sleepTime);
                clearedCells = Board.ClearNumbers();
                CurrentScore += clearedCells.Count*_boardSize;
                TotalScore += clearedCells.Count*_boardSize;
            } while (clearedCells.Count != 0);
        }


        private static int GetColumnIndex(string input)
        {
            return int.Parse(input) - 1;
        }

        public void DisplayScore()
        {
            _consoleWrapper.Write(TotalScore + "                           " + CurrentScore);
        }
    }
}