using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model
{
    public class SalesDetailModel
    {
        public string DetailId { get; set; }
        public string HeaderId { get; set; }
        public string SkuId { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmountWithoutTax { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmountWithTax { get; set; }
        public string transaction_start_datetime { get; set; }
        public string savings { get; set; }




        public SalesDetailModel(string headerId, string skuId, decimal qty, decimal unitPrice,
            decimal subtotal, decimal discount, decimal totalAmountWithoutTax, decimal tax, decimal totalAmountWithTax, string transaction_start_datetime, string savings)
        {
            DetailId = String.Empty;
            HeaderId = headerId;
            SkuId = skuId;
            Qty = qty;
            UnitPrice = unitPrice;
            SubTotal = subtotal;
            Discount = discount;
            TotalAmountWithoutTax = totalAmountWithoutTax;
            Tax = tax;
            TotalAmountWithTax = totalAmountWithTax;
            this.transaction_start_datetime = transaction_start_datetime;
            this.savings = savings;


        }
    }
}
