using BI.Jobs.DAC.Job;
using BI.Jobs.Shared.Collection;
using BI.Jobs.Shared.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.SalesImport
{
    public class GeneralSalesImportComponent: IImportComponent
    {
        
        public void Import(SalesModel model, string requestId)
        {
            //clean prev data for same store and same day
            //string storeId = model.@params.restaurant_id.ToString();

            string storeId = model.@params.restaurant_id == null ? "HardCodedStore": model.@params.restaurant_id.ToString();
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
                    string newHeaderId = Guid.NewGuid().ToString();
                    sDAC.CreateHeader(requestId, newHeaderId, storeId, dateValue, r, model.@params);

                    if (r.valuemeals != null)
                    {

                            foreach (var vm in r.valuemeals)  
                        {
                            decimal valuemeal_count = Convert.ToDecimal(vm.count);
                            decimal valuemeal_amount = Convert.ToDecimal(vm.amount);
                            decimal valuemeal_discount = Convert.ToDecimal(vm.discount);
                            decimal valuemeal_subtotal = Convert.ToDecimal(vm.sub_total);
                            decimal valuemeal_grandtotal = Convert.ToDecimal(vm.grand_total);
                            decimal valuemeal_taxtotal = Convert.ToDecimal(vm.tax_total);




                            SalesDetailModel dm = new SalesDetailModel(newHeaderId, vm.valuemeal_id, r.transaction_start_datetime, vm.third_party_id, vm.valuemeal_name, valuemeal_count, valuemeal_amount, vm.savings,
                                valuemeal_discount, valuemeal_subtotal, valuemeal_grandtotal, valuemeal_taxtotal, vm.mode);

                            sDAC.CreateDetail(requestId, dm);

                            if (vm.valuemeal_products != null)
                            {
                                foreach (var vmp in vm.valuemeal_products)
                                {

                                }
                            }
                        }

                    }


                    if (r.products != null)
                    {
                        foreach (var p in r.products)
                        {

                            decimal products_amount = Convert.ToDecimal(p.amount);
                            decimal products_count = Convert.ToDecimal(p.count);
                            decimal products_discount = Convert.ToDecimal(p.discount);
                            decimal products_subtotal = Convert.ToDecimal(p.sub_total);
                            decimal products_grandtotal = Convert.ToDecimal(p.grand_total);
                            decimal products_taxtotal = Convert.ToDecimal(p.tax_total);




                            ProductDetailModel pdm = new ProductDetailModel(newHeaderId, p.product_id, p.third_party_id, p.product_name, products_count, products_amount,
                                products_discount, products_subtotal, products_grandtotal, products_taxtotal, p.price, p.a_la_carte_price, p.mode);

                            sDAC.CreateDetail2(requestId, pdm);

                        }
                    }


                    if (r.tenders != null)
                    {
                        foreach (var t in r.tenders)
                        {

                            decimal tender_amount = Convert.ToDecimal(t.amount);
                            decimal tender_count = Convert.ToDecimal(t.count);

                            TenderDetailModel tdm = new TenderDetailModel(newHeaderId, t.tender_id, t.third_party_id, t.tender_name, tender_count, tender_amount, t.is_change, t.mode);

                            sDAC.CreateDetail3(requestId, tdm);

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
