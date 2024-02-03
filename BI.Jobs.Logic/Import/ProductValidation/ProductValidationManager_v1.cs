using BI.Jobs.Logic.Import.ProductValidation;
using BI.Jobs.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.ProductValidation
{
    public class ProductValidationManager_v1 : IProductValidationManager
    {
        public List<string> ValidateModel(SKUModel model)
        {
            List<string> errors = new List<string>();

            //if (String.IsNullOrWhiteSpace(model.sku_item_code))
            //{
            //    errors.Add("SKU Id cannot be empty. ");
            //}

            if (String.IsNullOrWhiteSpace(model.sku_item_code))
            {
                errors.Add("SKU Code cannot be empty. ");
            }

            if (String.IsNullOrWhiteSpace(model.product_description))
            {
                errors.Add("SKU Name cannot be empty. ");
            }

            return errors;
        }
    }
}
