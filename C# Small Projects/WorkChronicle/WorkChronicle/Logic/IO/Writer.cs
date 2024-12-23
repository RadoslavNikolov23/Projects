using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkChronicle.Logic.IO.Contacts;

namespace WorkChronicle.Logic.IO
{
    public class Writer : IWriter
    {
        private readonly string filePath;

        public Writer()
        {
            try
            {
                string path = @"C:\Users\PC\source\repos\WorkChronicle\WorkChronicle\Logic\Results\";
                filePath = Path.Combine(path, "Results.txt");

                // Ensure directory exists
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                File.WriteAllText(filePath, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        public void Write(string text) => Console.Write(text);

        public void WriteLine() => Console.WriteLine();
        public void WriteLine(string text) => Console.WriteLine(text);

        public void WriteText(string text) => File.WriteAllText(filePath, text.Trim() + Environment.NewLine);
    }
}
