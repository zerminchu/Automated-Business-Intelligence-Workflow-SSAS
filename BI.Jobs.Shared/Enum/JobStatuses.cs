using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Enum
{
    public enum JobStatuses
    {
        New = 0,
        Error = 1,
        Completed = 2,
        Aborted = -1,
        SummarizedMeta = 3
    }
}
