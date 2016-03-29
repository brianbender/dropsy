using System;

namespace Kata
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var consoleWrapper = new GridDisplayer(new RandomGenerator());
            Console.Write(consoleWrapper.DisplayBoard(9));
            Console.ReadKey();
        }
    }
}