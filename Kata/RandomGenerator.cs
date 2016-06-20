using System;

namespace Kata
{
    public class RandomGenerator : IRandomGenerator
    {
        private readonly Random _random;

        public RandomGenerator()
        {
            _random = new Random();
        }

        public string GetRandom(int maxValue)
        {
            return _random.Next(1, maxValue + 1).ToString();
        }

    }
}