using Kata;

namespace Tests
{
    public class FakeRandomGenerator : IRandomGenerator
    {
        public int NumberToReturn;

        public FakeRandomGenerator(int numberToReturn)
        {
            NumberToReturn = numberToReturn;
        }

        public string GetRandom(int maxValue)
        {
            return NumberToReturn.ToString();
        }
    }
}