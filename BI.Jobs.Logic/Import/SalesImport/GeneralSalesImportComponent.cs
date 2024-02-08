using BI.Jobs.DAC.Job;
using BI.Jobs.Shared.Collection;
using BI.Jobs.Shared.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.SalesImport
{
    public class GeneralSalesImportComponent: IImportComponent
    {
        
        public void Import(SalesModel model, string requestId)
        {
            //clean prev data for same store and same day
            //string storeId = model.@params.store_id.ToString();

            string storeId = model.@params.store_id == null ? "HardCodedStore": model.@params.store_id.ToString();
            string transDate = model.@params.tlogs.FirstOrDefault()!.date;

            transDate = "2024-01-01";

            CultureInfo enUS = new CultureInfo("en-US");
            DateTime dateValue;

            if (!DateTime.TryParseExact(transDate, MiscFormat.DT_FORMAT, enUS,
                                 DateTimeStyles.None, out dateValue))
            {
                throw new Exception("tlogs.date is not valid!");
            }


            var sDAC = new SalesDAC();
           sDAC.DeleteSalesDataForStoreDate(storeId, dateValue);

            //import
            try
            {
                foreach (var r in model.@params.tlogs)
                {
                    //string requestId, string headerId, string storeId, DateTime transDt, Tlog data
                    string newHeaderId = Guid.NewGuid().ToString();
                    sDAC.CreateHeader(requestId, newHeaderId, storeId, dateValue, r);

                    if (r.valuemeals != null)
                    {
                        foreach (var vm in r.valuemeals)
                        {
                            decimal qty = Convert.ToDecimal(vm.count);
                            decimal unitPrice = 0;
                            decimal totalQtyWithUnitPrice = Convert.ToDecimal(vm.amount);
                            decimal discount = Convert.ToDecimal(vm.discount);
                            decimal tax = Convert.ToDecimal(vm.tax);
                            decimal TotalAmountWithTax = Convert.ToDecimal(vm.total_amount);

                            SalesDetailModel dm = new SalesDetailModel(newHeaderId, vm.valuemeal_id, qty, unitPrice,
                                totalQtyWithUnitPrice, discount, totalQtyWithUnitPrice - discount, tax, TotalAmountWithTax, r.transaction_start_datetime, vm.savings);

                            sDAC.CreateDetail(requestId, dm);

                            if (vm.valuemeal_products != null)
                            {
                                foreach (var vmp in vm.valuemeal_products)
                                {
                                    decimal vmp_qty = Convert.ToDecimal(vmp.count);
                                    decimal vmp_unitPrice = 0;
                                    decimal vmp_totalQtyWithUnitPrice = Convert.ToDecimal(vmp.amount);
                                    decimal vmp_discount = 0;
                                    decimal vmp_tax = 0;
                                    decimal vmp_TotalAmountWithTax = Convert.ToDecimal(vmp.amount);

                                    SalesDetailModel vmp_dm = new SalesDetailModel(newHeaderId, vmp.product_id, vmp_qty, vmp_unitPrice,
                                        vmp_totalQtyWithUnitPrice, vmp_discount, vmp_totalQtyWithUnitPrice - vmp_discount, vmp_tax, vmp_TotalAmountWithTax, r.transaction_start_datetime, "");
                                }
                            }
                        }

                    }


                    if (r.products != null)
                    {
                        foreach (var p in r.products)
                        {

                            decimal qty = Convert.ToDecimal(p.count);
                            decimal unitPrice = 0;
                            decimal totalQtyWithUnitPrice = Convert.ToDecimal(p.amount);
                            decimal discount = Convert.ToDecimal(p.discount);
                            decimal tax = Convert.ToDecimal(p.tax);
                            decimal TotalAmountWithTax = Convert.ToDecimal(p.total_amount);

                            SalesDetailModel dm = new SalesDetailModel(newHeaderId, p.product_id, qty, unitPrice,
                                totalQtyWithUnitPrice, discount, totalQtyWithUnitPrice - discount, tax, TotalAmountWithTax, r.transaction_start_datetime, "");

                            sDAC.CreateDetail(requestId, dm);
                        }
                    }
                }


                try
                {
                    //summarized 
                    string dateKey = dateValue.ToString("yyyyMMdd");
                    string storeCode = storeId;
                   // sDAC.SummarizedStore(requestId, dateKey, storeCode);
                }
                catch (Exception)
                {
                    //summarized failed should not clean fact data
                }

            }
            catch (Exception ex)
            {
                //delete all record from same store same day same
               sDAC.DeleteSalesDataForStoreDate(storeId, dateValue);
                throw new Exception($"Exception encountered when process store {storeId} for day {transDate}. Delete all record from same combination to prevent error. " + ex.ToString());
            }
        }
    }
}
