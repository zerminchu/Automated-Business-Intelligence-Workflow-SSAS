using BI.Jobs.Shared.Model.ARMS;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model
{
    public class TenderDetailModel
    {

        public string DetailId { get; set; }
        public string HeaderId { get; set; }
        public decimal TenderCount { get; set; }
        public decimal TenderAmount { get; set; }


        public string tender_name { get; set; }
        public string tender_id { get; set; }
        public string tender_is_change { get; set; }
        public string tender_mode { get; set; }
        public string tender_third_party_id{ get; set; }




        public TenderDetailModel(string headerId, string tender_id, string tender_third_party_id, string tender_name, decimal tender_count, decimal tender_amount,
           string tender_is_change, string tender_mode)
        {
            DetailId = String.Empty;
            HeaderId = headerId;
            TenderCount = tender_count;
            TenderAmount = tender_amount;
         

            this.tender_name = tender_name;
            this.tender_id = tender_id;
            this.tender_third_party_id = tender_third_party_id;
            this.tender_is_change = tender_is_change;
            this.tender_mode = tender_mode;


        }

    }
}