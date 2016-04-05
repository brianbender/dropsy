using System;

namespace Kata
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var consoleWrapper = new GridDisplayer(new RandomGenerator(), 9);
            while (true)
            {
                Console.Write(consoleWrapper.DisplayBoard());
                var input = Console.ReadKey().KeyChar.ToString();
                consoleWrapper.SelectColumn(input);
                Console.Clear();
            }
        }
    }
}