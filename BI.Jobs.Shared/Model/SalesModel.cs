using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model
{
    public class OrderDiscount
    {
        public string discount_id { get; set; }
        public string third_party_id { get; set; }
        public string discount_reference_uid { get; set; }
        public string discount_name { get; set; }
        public string count { get; set; }
        public string amount { get; set; }
        public string mode { get; set; }
    }

    public class Params
    {
        public string restaurant_id { get; set; }
        public string store_code { get; set; }
        public List<Tlog> tlogs { get; set; }
    }

    public class Product
    {
        public string product_id { get; set; }
        public string third_party_id { get; set; }
        public string product_name { get; set; }
        public string count { get; set; }
        public string amount { get; set; }
        public string discount { get; set; }
        public string tax { get; set; }
        public string total_amount { get; set; }
        public string price { get; set; }
        public string a_la_carte_price { get; set; }
        public string mode { get; set; }
        public List<object> product_qualifiers { get; set; }
        public string sub_total { get; set; }
        public string savings { get; set; }


    }

    public class SalesModel
    {
        public string api_version { get; set; }
        public string provider { get; set; }
        public string password { get; set; }
        public string method { get; set; }
        public Params @params { get; set; }
    }

    public class Tax
    {
        public string tax_id { get; set; }
        public string third_party_tax_id { get; set; }
        public string tax_reference_uid { get; set; }
        public string tax_name { get; set; }
        public string count { get; set; }
        public string amount { get; set; }
        public string is_change { get; set; }
        public string mode { get; set; }
        public string principalAmount { get; set; }
        public string principalRate { get; set; }
    }

    public class Tender
    {
        public string tender_id { get; set; }
        public string third_party_id { get; set; }
        public string tender_reference_uid { get; set; }
        public string tender_name { get; set; }
        public string count { get; set; }
        public string amount { get; set; }
        public string is_change { get; set; }
        public string mode { get; set; }
        public string principalAmount { get; set; }
        public string principalRate { get; set; }
    }

    public class Tlog
    {
        public string sale_id { get; set; }
        public string order_number { get; set; }
        public string customer_id { get; set; }
        public string date { get; set; }
        public string transaction_start_datetime { get; set; }
        public string transaction_end_datetime { get; set; }
        public string destination { get; set; }
        public string is_overring { get; set; }
        public string deleted_items { get; set; }
        public string order_grand_total { get; set; }
        public string order_sub_total { get; set; }
        public string total_rounding { get; set; }





        public List<Valuemeal> valuemeals { get; set; }
        public List<Product> products { get; set; }
        public List<Tender> tenders { get; set; }
        public List<Tax> tax { get; set; }
        public List<OrderDiscount> order_discounts { get; set; }
    }

    public class Valuemeal
    {
        public string valuemeal_id { get; set; }
        public string third_party_id { get; set; }
        public string valuemeal_name { get; set; }
        public string count { get; set; }
        public string amount { get; set; }
        public string discount { get; set; }
        public string sub_total { get; set; }
        public string tax { get; set; }
        public string total_amount { get; set; }
        public string savings { get; set; }
        public string mode { get; set; }
        public List<ValuemealProduct> valuemeal_products { get; set; }
    }

    public class ValuemealProduct
    {
        public string product_id { get; set; }
        public string third_party_id { get; set; }
        public string product_name { get; set; }
        public string count { get; set; }
        public string amount { get; set; }
        public string price { get; set; }
        public string a_la_carte_price { get; set; }
        public string mode { get; set; }
        public List<ValuemealProductQualifier> valuemeal_product_qualifiers { get; set; }
    }

    public class ValuemealProductQualifier
    {
        public string modifier_id { get; set; }
        public string modifier_third_party_id { get; set; }
        public string modifier_name { get; set; }
        public string qualifier_id { get; set; }
        public string qualifier_third_party_id { get; set; }
        public string qualifier_name { get; set; }
        public string count { get; set; }
        public string amount { get; set; }
        public string discount { get; set; }
        public string sub_total { get; set; }
        public string tax { get; set; }
        public string total_amount { get; set; }
        public string price { get; set; }
        public string a_la_carte_price { get; set; }
        public string mode { get; set; }
    }
}
