using BI.Jobs.DAC.Job;
using BI.Jobs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.DataSource
{
    public class SalesVsProduct: IDataSourceComponent
    {
        public void Generate(string filePath)
        {
            try
            {
                var stringBuilder = new StringBuilder();
                Boolean withHeader = false;
                var x = new SalesDAC();
                var summarizedProducts = x.GetSummarizedProduct();
                if (summarizedProducts != null && summarizedProducts.Count > 0)
                {
                    string csvLine = string.Empty;
                    //convert each row to csv string and append to files
                    foreach (var line in summarizedProducts.ToCsv(header: withHeader))
                        stringBuilder.AppendLine(line);

                    FileUtilities.WriteToFileWithPath(filePath, stringBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                //do not interrupt whole process even if error
                string m = ex.Message;
            }
        }
    }
}
