using BI.Jobs.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.StoreValidation
{
    public interface IStoreValidationManager
    {
        public List<string> ValidateModel(StoreModel model);
    }
}
