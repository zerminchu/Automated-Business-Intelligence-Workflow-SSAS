using BI.Jobs.DAC.Dev;
using BI.Jobs.DAC.Job;
using BI.Jobs.Logic.Import.StoreValidation;
using BI.Jobs.Shared.Library;
using BI.Jobs.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.SalesValidation
{
    public class GeneralSalesValidationManager : ISalesValidationManager
    {
        public List<string> ValidateSalesModel(SalesModel model)
        {
            List<string> errors = new List<string>();

            Console.WriteLine($"Total of {model.@params.tlogs.Count} records found in file");
            //validate

            //string storeId = model.@params.store_id.ToString();
            string storeId = "TestID";
            if (string.IsNullOrWhiteSpace(storeId))
            {
                errors.Add("Store Id is required");
                return errors;
            }


            #region for dev
            Boolean createIfStoreNotExisted = ConfigManager.GetCreateStoreIfNotExisted();
            if (createIfStoreNotExisted)
            {
                var mockDAC = new MockDataDAC();
                mockDAC.CreateStore(storeId, storeId);
            }
            #endregion


            //check if store id existed
            var storeDAC = new StoreDAC();
            int existed = storeDAC.GetStoreWithId(storeId);
            if (existed != 1)
            {
                errors.Add("Store Id not existed");
                return errors;
            }

            var validator = new SalesTransactionValidatorFactory().GetSalesTransactionValidator(1);
            foreach (var t in model.@params.tlogs)
            {
                errors = validator.Validate(t);
            }

            return errors;
        }
    }
}
