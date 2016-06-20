using System.Collections.Generic;
using System.Linq;
using Kata;

namespace Tests
{
    public class FakeRandomGenerator : IRandomGenerator
    {
        public Queue<int> NumbersList = new Queue<int>();
        public int NumberToReturn;

        public FakeRandomGenerator(int numberToReturn)
        {
            NumberToReturn = numberToReturn;
        }

        public string GetRandom(int maxValue)
        {
            return (NumbersList.Any() ? NumbersList.Dequeue() : NumberToReturn).ToString();
        }

        public void SetRandomNumbers(params int[] numbers)
        {
            foreach (var number in numbers)
                NumbersList.Enqueue(number);
        }

        public string GetRandoms(int size)
        {
            return NumberToReturn.ToString();
        }
    }
}