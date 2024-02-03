using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.SalesValidation
{
    public class SalesTransactionValidatorFactory
    {
        public ISalesTransactionValidator GetSalesTransactionValidator(decimal version)
        {
            return new GeneralSalesTransactionValidator();
        }
    }
}
