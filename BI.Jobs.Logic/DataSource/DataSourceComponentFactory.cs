using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.DataSource
{
    public static class DataSourceType
    {
        public const string SalesVsStore = "SalesVsStore";
        public const string SalesVsProduct = "SalesVsProduct";
    }

    public class DataSourceComponentFactory
    {
        public IDataSourceComponent GetDataSource(string type, string clientCode)
        {
            if (type.Equals(DataSourceType.SalesVsStore))
            {
                return new SalesVsStore();
            }
            else if (type.Equals(DataSourceType.SalesVsProduct))
            {
                return new SalesVsProduct();
            }
            else
            {
                throw new Exception("Data Source type invalid");
            }
            
        }
    }
}
