using BI.Jobs.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.SalesImport
{
    public interface IImportComponent
    {
        public void Import(SalesModel model, string requestId);
    }
}
