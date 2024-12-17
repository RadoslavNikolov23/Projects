using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarShiftDemo.IO.Contacts
{
    public interface IWriter
    {
        void Write(string text);

        void WriteLine();
        void WriteLine(string text);
        void WriteText(string text);
    }
}
