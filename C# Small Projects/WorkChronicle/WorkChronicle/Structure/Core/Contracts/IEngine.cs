using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkChronicle.Core.Models.Contracts;
using WorkChronicle.Core.Repository.Contracts;

namespace WorkChronicle.Structure.Core.Contracts
{
    public interface IEngine
    {
        ISchedule<IShift> CalculateShifts(DateTime startDate, string[] cycle);

        List<string> PrintShifts(ISchedule<IShift> schedule);

        int CalculateTotalHours(ISchedule<IShift> schedule);
    }
}
