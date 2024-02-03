using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model
{
    public class SummarizedStore
    {
        public string Year { get; set; }
        public string FiscalYear { get; set; }
        public string Month { get; set; }
        public string FiscalMonth { get; set; }
        public string DayOfMonth { get; set; }
        public string Quarter { get; set; }
        public string FiscalQuarter { get; set; }
        public string DayOfYear { get; set; }
        public string WeekOfMonth { get; set; }
        public string FiscalWeekOfYear { get; set; }
        public int DateKey { get; set; }
        public string RegionName { get; set; }
        public string CountryName { get; set; }
        public string AreaName { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public int TotalTransaction { get; set; }
        public decimal TotalAmount { get; set; }
        public string BasketSize { get; set; }
        //public string x { get; set; }
    }
}
