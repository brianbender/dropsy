using System;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Kata
{
    public class GameController
    {
        private readonly Board _board;
        private readonly ConsoleWrapper _consoleWrapper;
        private int _movesTaken;
        private readonly int _sleepTime;

        public GameController(Board board, ConsoleWrapper consoleWrapper, int sleepTime = 0)
        {
            _board = board;
            _consoleWrapper = consoleWrapper;
            _movesTaken = 0;
            CanAcceptInput = true;
            _sleepTime = sleepTime;

        }

        public bool GameIsOver { get; set; }
        public bool CanAcceptInput { get; set; }

        public void DoMove(string input)
        {
            if (string.IsNullOrEmpty(input))
                return;
            CanAcceptInput = false;
            var column = GetColumnIndex(input);
            _board.PlaceChip(column);
            UpdateGameState();
        }

        public void DisplayBoard()
        {
            _consoleWrapper.Clear();
            _consoleWrapper.Write(_board.Display());
        }

        private void UpdateGameState()
        {
            _movesTaken++;
            if (_movesTaken%5 == 0)
            {
                _board.AddBlockRow();
            }
            var clearedCells = _board.ClearNumbers();
            DisplayBoard();
            Thread.Sleep(_sleepTime);

            //var timer = new Timer(500);
            //timer.Start();
            //timer.Elapsed += delegate
            //{
            _board.ClearPoppedCells(clearedCells);
            CanAcceptInput = true;
            //};
            if (_board.TopRowIsFilled() || _board.ColumnOverFlowed())
                GameIsOver = true;
        }


        private static int GetColumnIndex(string input)
        {
            return int.Parse(input) - 1;
        }
    }
}