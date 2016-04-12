using System;

namespace Kata
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IRandomGenerator randomGenerator = new RandomGenerator();
            var board = new Board(9, randomGenerator);
            var game = new GridDisplayer(board);
            while (!game.GameIsOver)
            {
                Console.Write(game.DisplayBoard());
                var input = Console.ReadKey().KeyChar.ToString();
                game.DoMove(input);
                Console.Clear();
            }
            Console.Write(game.DisplayBoard());
        }
    }
}