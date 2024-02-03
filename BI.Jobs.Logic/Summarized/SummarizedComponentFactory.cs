using BI.Jobs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Summarized
{
    public static class SummarizedComponentType
    {
        public const string SALES = "SALES";
    }

    public class SummarizedComponentFactory
    {
        

        public ISummarizedComponent GetSummarziedComponent(string type, string requestId, string performer, ILogComponent logger)
        {
            if (type.Equals(SummarizedComponentType.SALES))
            {
                return new SummarizedSalesProductComponent(requestId,performer, logger);
            }
            else
                throw new Exception("Invalid Summarized Component type");

        }
    }
}
