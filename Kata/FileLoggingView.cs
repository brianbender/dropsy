using System;
using System.IO;
using System.Text;

namespace Kata
{
    public class FileLoggingView : View, IDisposable
    {
        private readonly FileStream _file;

        public FileLoggingView()
        {
            _file = File.Open("last_game.txt", FileMode.Create);
        }

        public void Dispose()
        {
            _file.Close();
            _file.Dispose();
        }

        public override void Write(string output)
        {
            base.Write(output);
            var bytes = Encoding.UTF8.GetBytes(output);
            _file.Write(bytes, 0, bytes.Length);
        }
    }
}
