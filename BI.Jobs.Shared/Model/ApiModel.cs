using BI.Jobs.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model
{
    public class ApiModel
    {
        public int Id { get; set; }
        public string ApiType { get; set; }
        public decimal ApiVersion { get; set; }
        public Statuses ApiStatus { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Version { get; set; }
    }
}
