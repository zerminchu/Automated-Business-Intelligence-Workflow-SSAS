using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.SalesImport
{
    public class ImportComponentFactory
    {
        public IImportComponent GetSalesImportComponent(decimal version)
        {
            return new GeneralSalesImportComponent();
        }
    }
}
