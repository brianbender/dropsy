using System;

namespace Kata
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var game = new GridDisplayer(new RandomGenerator(), 9);
            while (!game.GameIsOver)
            {
                Console.Write(game.DisplayBoard());
                var input = Console.ReadKey().KeyChar.ToString();
                game.SelectColumn(input);
                Console.Clear();
            }
            Console.Write(game.DisplayBoard());
        }
    }
}