using System.Collections.Generic;
using System.IO;

namespace StudentsList.UI
{
    class Printer
    {
        private TextWriter writer;
        public Printer(TextWriter writer)
        {
            this.writer = writer;
        }
        public void Print(string text)
        {
            this.writer.Write(text);
        }

        public void PrintLine()
        {
            PrintLine(string.Empty);
        }

        public void PrintLine(string text)
        {
            this.writer.WriteLine(text);
        }

        public void Print(List<string> list)
        {
            foreach(var item in list)
                Print(item);
        }

        public void PrintLine(List<string> list)
        {
            foreach(var item in list)
                PrintLine(item);
        }
    }
}