
using BI.Jobs.Logic.Import.ProductValidation;
using BI.Jobs.Logic.Import.StoreValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.ProductValidation
{
    public class ProductValidationManagerFactory
    {
        public IProductValidationManager GetProductValidationManager(decimal version)
        {
            return new ProductValidationManager_v1();
        }
    }
}
