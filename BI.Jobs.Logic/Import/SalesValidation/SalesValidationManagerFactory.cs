using BI.Jobs.Logic.Import.StoreValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.SalesValidation
{
    public class SalesValidationManagerFactory
    {
        public ISalesValidationManager GetSalesValidationManager(decimal version)
        {
            return new GeneralSalesValidationManager();
        }
    }
}
