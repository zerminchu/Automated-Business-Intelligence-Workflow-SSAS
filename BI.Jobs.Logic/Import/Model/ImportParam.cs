using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.Model
{
    public class ImportParam
    {
        public string ClientCode { get; set; }
        public string FileCode { get; set; }
        public DateTime JobDate { get; set; }
        public string TargetDirectory { get; set; }
        public string RequestId { get; set; }
        public string Performer { get; set; }

        public ImportParam(string clientCode, string fileCode, DateTime jobDate, string targerDirectory, string requestId, string performer)
        {
            ClientCode = clientCode;
            FileCode = fileCode;
            JobDate = jobDate;
            TargetDirectory = targerDirectory;
            RequestId = requestId;
            Performer = performer;
        }
    }
}
