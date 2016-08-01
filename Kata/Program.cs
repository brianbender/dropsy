using System;

namespace Kata
{
    internal class Program
    {
        private static void Main()
        {
            IRandomGenerator randomGenerator = new RandomGenerator();
            var consoleWrapper = new FileLoggingConsoleWrapper();
            const int boardSize = 9;
            const int sleepTime = 500;
            var game = new GameController(boardSize, randomGenerator, consoleWrapper, sleepTime);
            while (!game.GameIsOver)
            {
                game.Draw();

                if (game.CanAcceptInput)
                {
                    var input = Console.ReadKey().KeyChar.ToString();
                    game.DoMove(input);
                }
            }
            game.Draw();
        }
    }
}