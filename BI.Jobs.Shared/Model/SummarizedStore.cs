using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model
{
    public class SummarizedStore
    {
   
        public string ValueMealName { get; set; }
        public string ValueMealId { get; set; }
        public string ValueMealThirdPartyId { get; set; }
        public decimal ValueMealCount { get; set; }
        public decimal ValueMealAmount { get; set; }
        public string ValueMealSavings { get; set; }
        public decimal ValueMealDiscount { get; set; }
        public decimal ValueMealSubTotal { get; set; }
        public decimal ValueMealGrandTotal { get; set; }
        public decimal ValueMealTaxTotal { get; set; }
        public string ValueMealMode { get; set; }

        public string TenderName { get; set; }
        public decimal TenderCount { get; set; }
        public decimal TenderAmount { get; set; }
        public bool TenderIsChange { get; set; }
        public string TenderMode { get; set; }

        public string ProductsName { get; set; }
        public decimal ProductsCount { get; set; }
        public decimal ProductsAmount { get; set; }
        public decimal ProductsDiscount { get; set; }
        public decimal ProductsSubTotal { get; set; }
        public decimal ProductsGrandTotal { get; set; }
        public decimal ProductsTaxTotal { get; set; }
        public string ProductsPrice { get; set; }
        public string ProductsAlacarte { get; set; }
        public string ProductsMode { get; set; }


    }
}
