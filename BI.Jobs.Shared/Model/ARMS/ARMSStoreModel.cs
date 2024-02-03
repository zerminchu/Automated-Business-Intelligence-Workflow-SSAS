using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model.ARMS
{
    public class ARMSStoreModel
    {
        public int result { get; set; }
        public List<BranchDatum> branch_data { get; set; }
    }


    public class BranchDatum
    {
        public string id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public string phone_1 { get; set; }
        public string phone_2 { get; set; }
        public string phone_3 { get; set; }
        public string contact_email { get; set; }
        public string outlet_photo_url { get; set; }
        public string operation_time { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string active { get; set; }
        public string branch_group_id { get; set; }
        public string branch_group_code { get; set; }
        public string branch_group_desc { get; set; }
        public string changes_row_index { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Accounting
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int age { get; set; }
    }

    public class UNICONCOMPLEXMODEL
    {
        public List<Accounting> accounting { get; set; }
        public List<Sale> sales { get; set; }
    }

    public class Sale
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int age { get; set; }
    }


}
