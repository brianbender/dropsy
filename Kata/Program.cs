using System;

namespace Kata
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var consoleWrapper = new GridDisplayer(new RandomGenerator(), 9);
            Console.Write(consoleWrapper.DisplayBoard());
            Console.ReadKey();
        }
    }
}