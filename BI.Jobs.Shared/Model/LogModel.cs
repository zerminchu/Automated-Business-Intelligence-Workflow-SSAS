using BI.Jobs.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model
{
    public class LogModel
    {
        public string RequestId { get; set; }
        public string Layer { get; set; }
        public LogSeverity Severity { get; set; }
        public string Info { get; set; }
        public string Detail { get; set; }
        public string Performer { get; set; }

        public LogModel(string requestId, string layer, LogSeverity severity, string info, string detail, string performer)
        {
            RequestId = requestId;
            Layer = layer;
            Severity = severity;
            Info = info;
            Detail = detail;
            Performer = performer;
        }
    }
}
