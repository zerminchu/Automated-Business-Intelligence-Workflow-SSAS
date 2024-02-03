using BI.Jobs.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model
{
    public class ProcessingJob
    {
        public long JobId { get; set; }
        public string JobType { get; set; }
        public string RequestId { get; set; }
        public string FilePath { get; set; }
        public JobStatuses JobStatus { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
