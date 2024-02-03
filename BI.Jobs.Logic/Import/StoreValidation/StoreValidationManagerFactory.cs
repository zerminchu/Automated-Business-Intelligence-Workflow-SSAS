
using BI.Jobs.Logic.Import.StoreValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.StoreValidation
{
    public class StoreValidationManagerFactory
    {
        public IStoreValidationManager GetStoreValidationManager(decimal version)
        {
            return new StoreValidationManager();
        }
    }
}
