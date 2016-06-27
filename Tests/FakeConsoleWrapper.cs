using System;
using System.Collections.Generic;
using System.Linq;
using Kata;
using NUnit.Framework;

namespace Tests
{
    public class FakeConsoleWrapper : ConsoleWrapper
    {
        public bool ClearCalled { get; set; }

        public string LastWrite => AllWrites.Last();

        public List<string> AllWrites { get; set; } = new List<string>();

        public override void Write(string output)
        {
            AllWrites.Add(output);
        }

        public override void Clear()
        {
            ClearCalled = true;
        }
    }
}