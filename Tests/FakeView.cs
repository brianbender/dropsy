using System.Collections.Generic;
using System.Linq;
using Kata;

namespace Tests
{
    public class FakeView : View
    {
        public List<string> AllWrites { get; set; } = new List<string>();
        public bool ClearCalled { get; set; }

        public string LastWrite => AllWrites.Last();

        public override void Clear()
        {
            ClearCalled = true;
        }

        public override void Write(string output)
        {
            AllWrites.Add(output);
        }
    }
}
