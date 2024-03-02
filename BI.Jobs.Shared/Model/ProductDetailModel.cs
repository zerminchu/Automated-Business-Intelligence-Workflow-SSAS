using BI.Jobs.Shared.Model.ARMS;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model
{
    public class ProductDetailModel
    {
        // Products
        public string DetailId { get; set; }
        public string HeaderId { get; set; }
        public decimal ProductsCount { get; set; }
        public decimal ProductsAmount { get; set; }
        public decimal ProductsDiscount { get; set; }
        public decimal ProductsSubTotal { get; set; }
        public decimal ProductsGrandTotal { get; set; }
        public decimal ProductsTaxTotal { get; set; }

        public string product_name { get; set; }
        public string product_id { get; set; }
        public string product_third_party_id { get; set; }
        public string product_price { get; set; }
        public string product_alacarte { get; set; }
        public string product_mode { get; set; }



        public ProductDetailModel(string headerId, string product_id, string product_third_party_id, string product_name, decimal products_count, decimal products_amount,
           decimal products_discount, decimal products_subtotal, decimal products_grandtotal, decimal products_taxtotal, string product_price, string product_alacarte, 
           string product_mode)
        {
            DetailId = String.Empty;
            HeaderId = headerId;
            ProductsCount = products_count;
            ProductsAmount = products_amount;
            ProductsDiscount = products_discount;
            ProductsSubTotal = products_subtotal;
            ProductsGrandTotal = products_grandtotal;
            ProductsTaxTotal = products_taxtotal;

            this.product_name = product_name;
            this.product_id = product_id;
            this.product_third_party_id = product_third_party_id;
            this.product_price = product_price;
            this.product_alacarte = product_alacarte;
            this.product_mode = product_mode;


        }

    }
}