using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model
{
    public class DataSourceSettingModel
    {
        public string fileName { get; set; }
        public string fileExtension { get; set; }
        public string header { get; set; }
        public int KeepNonLiveFileForXMinutes { get; set; }
        public string StaticFileLocation { get; set; }
    }
}
