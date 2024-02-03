using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model
{
    public class SKUMasterModel
    {
        public int result { get; set; }
        public List<SKUModel> product_data { get; set; }
    }

    public class SKUModel
    {
        public string sku_item_code { get; set; }
        public string product_description { get; set; }
        public object article_description { get; set; }
        public object bar_code { get; set; }
        public object supplier_item_code { get; set; }
        public string group_code { get; set; }
        public string group_description { get; set; }
        public string division_code { get; set; }
        public string division_description { get; set; }
        public decimal unit_price { get; set; }
        public string department_code { get; set; }
        public string department_description { get; set; }
        public string category_code { get; set; }
        public string category_description { get; set; }
        public object status { get; set; }
    }

    
}
