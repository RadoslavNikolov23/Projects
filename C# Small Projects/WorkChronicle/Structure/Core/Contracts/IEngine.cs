using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkChronicle.Core.Models.Contracts;
using WorkChronicle.Core.Repository;
using WorkChronicle.Core.Repository.Contracts;

namespace WorkChronicle.Structure.Core.Contracts
{
    public interface IEngine
    {
        Schedule CalculateShifts(DateTime startDate, string[] cycle);

        int CalculateTotalHours(Schedule schedule);
    }
}
