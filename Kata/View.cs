using System;

namespace Kata
{
    public class View
    {
        public virtual void Clear()
        {
            Console.Clear();
        }

        public virtual void Write(string output)
        {
            Console.Write(output);
        }
    }
}
