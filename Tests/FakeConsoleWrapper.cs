using Kata;

namespace Tests
{
    public class FakeConsoleWrapper : ConsoleWrapper
    {
        public bool ClearCalled { get; set; }

        public string LastWrite { get; set; }

        public override void Write(string output)
        {
            LastWrite = output;
        }

        public override void Clear()
        {
            ClearCalled = true;
        }
    }
}