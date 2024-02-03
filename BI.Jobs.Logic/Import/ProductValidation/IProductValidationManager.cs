using BI.Jobs.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.ProductValidation
{
    public interface IProductValidationManager
    {
        public List<string> ValidateModel(SKUModel model);
    }
}
