using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Model
{
    public class SummarizedProduct
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
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string DepartmentName { get; set; }
        public string GroupName { get; set; }
        public string DivisionName { get; set; }
        public decimal TotalTransaction { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal BasketSize { get; set; }
    }
}
