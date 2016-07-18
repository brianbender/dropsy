using System;

namespace Kata
{
    public class ConsoleWrapper
    {
        public virtual void Write(string output)
        {
            Console.Write(output);
        }

        public virtual void Clear()
        {
            Console.Clear();
        }
    }
}