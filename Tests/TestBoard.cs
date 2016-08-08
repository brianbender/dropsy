using Kata;

namespace Tests
{
    public class TestBoard : Board
    {
        public TestBoard(int size, IRandomGenerator randomGenerator) : base(size, randomGenerator)
        {
        }

        public string GetCellContentForTest(int row, int col)
        {
            return GetCellContent(row, col);
        }

        public void OverrideCellContent(int row, int col, string content)
        {
            SetCellContent(row, col, content);
        }
    }
}
