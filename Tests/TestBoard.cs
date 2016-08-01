using Kata;

namespace Tests
{
    public class TestBoard : Board
    {
        public TestBoard(int size, IRandomGenerator randomGenerator) : base(size, randomGenerator)
        {
        }

        public void OverrideCellContent(int row, int col, string content)
        {
            SetCellContent(row, col, content);
        }

        public string GetCellContentForTest(int row, int col)
        {
            return GetCellContent(row, col);
        }
    }
}