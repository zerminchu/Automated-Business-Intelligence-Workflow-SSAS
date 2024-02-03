using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model
{
    public class StoreMasterModel
    {
        public int? result { get; set; }
        public List<StoreModel> branch_data { get; set; }
    }

    // standard database model
    public class StoreModel
    {
        public string code { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string branch_group_code { get; set; }
        public string phone_1 { get; set; }
        public string phone_2 { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string address_3 { get; set; }
        public string zip_code { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string region { get; set; }
        public string opening_date { get; set; }
        public string status { get; set; }

        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }
        public string CustomField4 { get; set; }
        public string CustomField5 { get; set; }
        public string CustomField6 { get; set; }
    }

}
