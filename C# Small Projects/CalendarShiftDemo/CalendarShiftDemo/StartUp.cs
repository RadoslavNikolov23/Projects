using CalendarShiftDemo.Core;
using CalendarShiftDemo.Core.Contacts;

namespace CalendarShiftDemo;

public class StartUp
{
    static void Main()
    {
        IEngine engine = new Engine();
        engine.Run();
    }
}
