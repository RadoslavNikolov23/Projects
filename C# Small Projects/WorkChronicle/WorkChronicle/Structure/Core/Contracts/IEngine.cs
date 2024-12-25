using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkChronicle.Structure.Core.Contracts
{
    public interface IEngine
    {
        List<string> CalculateShifts(DateTime startDate, string[] cycle);
        int CalculateTotalHours(List<string> shifts);
    }
}
