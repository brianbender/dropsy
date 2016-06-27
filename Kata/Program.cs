using System;

namespace Kata
{
    internal class Program
    {
        private static void Main()
        {
            IRandomGenerator randomGenerator = new RandomGenerator();
            var game = new GameController(9, randomGenerator, new FileLoggingConsoleWrapper(), 500);
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