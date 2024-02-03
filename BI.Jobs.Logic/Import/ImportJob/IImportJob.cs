using BI.Jobs.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.ImportJob
{
    public interface IImportJob
    {
        public void Import(List<ProcessingJob> jobs);
    }
}
