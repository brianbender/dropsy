using System;

namespace Kata
{
    public class RandomGenerator : IRandomGenerator
    {
        public string GetRandom(int maxValue)
        {
            return new Random().Next(1, maxValue + 1).ToString();
        }
    }
}