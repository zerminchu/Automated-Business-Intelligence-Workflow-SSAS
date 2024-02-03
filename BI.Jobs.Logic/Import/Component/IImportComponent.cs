using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.Component
{
    public interface IImportComponent
    {
        public void CreateImportJob();
        public void ProcessImportJob();
    }
}
