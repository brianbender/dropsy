using System;

namespace Kata
{
    internal class Program
    {
        private static void Main()
        {
            IRandomGenerator randomGenerator = new RandomGenerator();
            var board = new Board(9, randomGenerator);
            var game = new GameController(board, new FileLoggingConsoleWrapper(), 500);
            while (!game.GameIsOver)
            {
                game.DisplayBoard();

                if (game.CanAcceptInput)
                {
                    var input = Console.ReadKey().KeyChar.ToString();
                    game.DoMove(input);
                }
            }
            game.DisplayBoard();
        }
    }
}