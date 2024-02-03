using BI.Jobs.Logic.Import.StoreValidation;
using BI.Jobs.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.StoreValidation
{
    public class StoreValidationManager : IStoreValidationManager
    {
        public List<string> ValidateModel(StoreModel model)
        {
            List<string> errors = new List<string>();

            if (String.IsNullOrWhiteSpace(model.code))
            {
                errors.Add("Store Code cannot be empty. ");
            }

            if (String.IsNullOrWhiteSpace(model.description))
            {
                errors.Add("Store Description cannot be empty. ");
            }

            return errors;
        }
    }
}
