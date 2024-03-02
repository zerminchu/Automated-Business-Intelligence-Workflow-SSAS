using BI.Jobs.Shared.Model.ARMS;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model
{
    public class SalesDetailModel
    {
        public string DetailId { get; set; }
        public string HeaderId { get; set; }
        public decimal ValueMealCount { get; set; }
        public decimal ValueMealAmount { get; set; }
        public decimal ValueMealDiscount { get; set; }
        public decimal ValueMealSubTotal{ get; set; }
        public decimal ValueMealGrandTotal { get; set; }
        public decimal ValueMealTaxTotal { get; set; }

        public string transaction_start_datetime { get; set; }
        public string valuemeal_savings { get; set; }
        public string valuemeal_name { get; set; }
        public string valuemeal_id { get; set; }
        public string valuemeal_third_party_id { get; set; }
        public string valuemeal_mode { get; set; }


        public SalesDetailModel(string headerId, string transaction_start_datetime, string valuemeal_id, string valuemeal_third_party_id, string valuemeal_name, 
            decimal valuemeal_count, decimal valuemeal_amount, string valuemeal_savings, decimal valuemeal_discount, decimal valuemeal_subtotal, decimal valuemeal_grandtotal, decimal valuemeal_taxtotal,
            string valuemeal_mode)
        {
            DetailId = String.Empty;
            HeaderId = headerId;
            ValueMealAmount = valuemeal_amount;
            ValueMealCount = valuemeal_count;
            ValueMealDiscount = valuemeal_discount;
            ValueMealSubTotal = valuemeal_subtotal;
            ValueMealGrandTotal = valuemeal_grandtotal;
            ValueMealTaxTotal = valuemeal_taxtotal;



            this.transaction_start_datetime = transaction_start_datetime;
            this.valuemeal_savings = valuemeal_savings;
            this.valuemeal_name = valuemeal_name;
            this.valuemeal_id = valuemeal_id;
            this.valuemeal_third_party_id = valuemeal_third_party_id;
            this.valuemeal_mode = valuemeal_mode;


        }

    }
}