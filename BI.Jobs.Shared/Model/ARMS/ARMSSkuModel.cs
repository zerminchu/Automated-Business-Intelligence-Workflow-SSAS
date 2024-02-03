using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model.ARMS
{
    public class ARMSSkuModel
    {
        public int result { get; set; }
        public List<ProductsDatum> products_data { get; set; }
    }
    public class ProductsDatum
    {
        public string sku_item_id { get; set; }
        public string sku_item_code { get; set; }
        public string mcode { get; set; }
        public string artno { get; set; }
        public string link_code { get; set; }
        public string product_description { get; set; }
        public string receipt_description { get; set; }
        public string active { get; set; }
        public string pos_photo_url { get; set; }
        public string selling_price_before_tax { get; set; }
        public string selling_price_inclusive_tax { get; set; }
        public string group_id { get; set; }
        public string group_code { get; set; }
        public string group_description { get; set; }
        public string division_id { get; set; }
        public string division_code { get; set; }
        public string division_description { get; set; }
        public string department_id { get; set; }
        public string department_code { get; set; }
        public string department_description { get; set; }
        public string category_id { get; set; }
        public string category_code { get; set; }
        public string category_description { get; set; }
        public string changes_row_index { get; set; }
    }

    


}
