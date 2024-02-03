using BI.Jobs.DAC.Dev;
using BI.Jobs.DAC.Job;
using BI.Jobs.Shared.Collection;
using BI.Jobs.Shared.Library;
using BI.Jobs.Shared.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.SalesValidation
{
    public class GeneralSalesTransactionValidator : ISalesTransactionValidator
    {
        public List<string> Validate(Tlog model)
        {
            List<string> errors = new List<string>();

            if (String.IsNullOrWhiteSpace(model.sale_id))
                errors.Add("Sales Id is empty");

            CultureInfo enUS = new CultureInfo("en-US");
            DateTime dateValue;

            if (String.IsNullOrWhiteSpace(model.date))
                errors.Add($"{model.sale_id} - date is empty");
            else
            {
                
                if (!DateTime.TryParseExact(model.date, MiscFormat.DT_FORMAT, enUS,
                                 DateTimeStyles.None, out dateValue))
                    errors.Add($"{model.sale_id} - date is in invalid format");
            }

            if (!String.IsNullOrWhiteSpace(model.transaction_start_datetime))
            {
                if (!DateTime.TryParseExact(model.transaction_start_datetime, MiscFormat.TRANSDT_FORMAT, enUS,
                                    DateTimeStyles.None, out dateValue))
                    errors.Add($"{model.sale_id} - transaction_start_datetime is in invalid format");
            }

            if (!String.IsNullOrWhiteSpace(model.transaction_end_datetime))
            {
                if (!DateTime.TryParseExact(model.transaction_end_datetime, MiscFormat.TRANSDT_FORMAT, enUS,
                                    DateTimeStyles.None, out dateValue))
                    errors.Add($"{model.sale_id} - transaction_end_datetime is in invalid format");
            }

            if ((model.products == null || model.products.Count <= 0)
                && (model.valuemeals == null || model.valuemeals.Count <= 0))
            {

                errors.Add($"{model.sale_id} - No product or valuemeal for this transaction");
            }

            if (errors.Count > 0)
                return errors;




            #region for dev
            Boolean createIfProductNotExisted = ConfigManager.GetCreateProductIfNotExisted();
            #endregion


            ProductDAC pDac = new ProductDAC();
            if (model.valuemeals != null && model.valuemeals.Count > 0)
            {
                foreach (var vm in model.valuemeals)
                {
                    #region dev
                    if (createIfProductNotExisted)
                    {
                        var mockDAC = new MockDataDAC();
                        mockDAC.CreateProduct(vm.valuemeal_id, vm.valuemeal_name);
                    }
                    #endregion


                    int existed = pDac.GetProductWithId(vm.valuemeal_id);
                    if (existed == 0)
                    {
                        errors.Add($"{model.sale_id} - vm {vm.valuemeal_id} not existed. ");
                        break;
                    }

                    if (vm.valuemeal_products != null && vm.valuemeal_products.Count > 0)
                    {
                        foreach (var vmp in vm.valuemeal_products)
                        {
                            #region dev
                            if (createIfProductNotExisted)
                            {
                                var mockDAC = new MockDataDAC();
                                mockDAC.CreateProduct(vmp.product_id, vmp.product_name);
                            }
                            #endregion

                            existed = pDac.GetProductWithId(vmp.product_id);
                            if (existed == 0)
                            {
                                errors.Add($"{model.sale_id} - vm {vm.valuemeal_id} - vmp {vmp.product_id} not existed. ");
                                break;
                            }

                            //ignore qualifier first
                            //if (vmp.valuemeal_product_qualifiers != null && vmp.valuemeal_product_qualifiers.Count > 0)
                            //{
                            //    foreach (var pd in vmp.valuemeal_product_qualifiers)
                            //    {
                            //        existed = pDac.GetProductWithId(pd.id);
                            //        if (existed == 0)
                            //        {
                            //            errors.Add($"{model.sale_id} - vm {vm.valuemeal_id} - vmp {vmp.product_id} not existed. ");
                            //            break;
                            //        }
                            //    }
                            //}
                        }
                    }

                }
            }

            if (model.products != null && model.products.Count > 0)
            {
                foreach (var p in model.products)
                {
                    #region dev
                    if (createIfProductNotExisted)
                    {
                        var mockDAC = new MockDataDAC();
                        mockDAC.CreateProduct(p.product_id, p.product_name);
                    }
                    #endregion

                    int existed = pDac.GetProductWithId(p.product_id);
                    if (existed == 0)
                    {
                        errors.Add($"{model.sale_id} - product {p.product_id} not existed. ");
                        break;
                    }
                }
            }

            //if ((model.tax == null || model.tax.Count <= 0)
            //    && model.tenders == null || model.tenders.Count <= 0)
            //{
            //    errors.Add($"{model.sale_id} - tax not existed. ");
            //}
            //else
            //{
            //    if (model.tax == null || model.tax.Count <= 0)
            //    {
            //        var taxTender = model.tenders.Where(x => x.tender_name.Equals(MiscFormat.TENDER_TAX_KEYWORD));
            //        if (taxTender == null || taxTender.Count() <= 0)
            //        {
            //            errors.Add($"{model.sale_id} - tax not existed. ");
            //        }
            //    }   
            //}

            return errors;
        }
    }
}
