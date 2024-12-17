using CalendarShiftDemo.IO.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarShiftDemo.IO
{
    public class Writer : IWriter
    {
        private readonly string filePath;

        public Writer()
        {
            string path = @"../../../" + @"Results/";
            filePath = Path.Combine(path, "Results.txt");

            File.WriteAllText(filePath, string.Empty);
        }
        public void Write(string text) => Console.Write(text);

        public void WriteLine() => Console.WriteLine();
        public void WriteLine(string text) => Console.WriteLine(text);

        public void WriteText(string text) => File.WriteAllText(filePath, text.Trim()+Environment.NewLine);
    }
}
