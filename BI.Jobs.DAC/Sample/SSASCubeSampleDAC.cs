using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AnalysisServices.AdomdClient;

namespace BI.Jobs.DAC.Sample
{
    public class SSASCubeSampleDAC : DAC
    {
        public List<KeyValuePair<string, string>> GetSampleData(int month, int year)
        {
            var today = DateTime.Today;

            string mdxQuery
                = @" SELECT NON EMPTY { [Measures].[Total Amount Include Tax] } ON COLUMNS, 
NON EMPTY { 
([Fact Transaction Details].[Transaction Header Id].[Transaction Header Id].ALLMEMBERS * 
[Fact Transaction Details].[Transaction Detail Id].[Transaction Detail Id].ALLMEMBERS * 
[Date].[Year].[Year].ALLMEMBERS * [Date].[Month].[Month].ALLMEMBERS * 
[Transaction Header].[SKU Id].[SKU Id].ALLMEMBERS * 
[Fact Transaction Details].[SKU Description].[SKU Description].ALLMEMBERS ) 
} DIMENSION PROPERTIES MEMBER_CAPTION, MEMBER_UNIQUE_NAME ON ROWS 
FROM ( SELECT ( { [Date].[Month].&[" + month.ToString() + @"] } ) ON COLUMNS 
FROM ( SELECT ( { [Date].[Year].&[" + year.ToString() + @"] } ) ON COLUMNS FROM [BI Data Dev])) 
CELL PROPERTIES VALUE, BACK_COLOR, FORE_COLOR, FORMATTED_VALUE, FORMAT_STRING, FONT_NAME, FONT_SIZE, FONT_FLAGS ";

            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            using (var connection = new AdomdConnection(MDXConnectionString))
            {
                using (AdomdCommand cmd = new AdomdCommand(mdxQuery, connection))
                {
                    connection.Open();
                    using (AdomdDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            string k = (GetDataValue<double>(dr, "[Measures].[Total Amount Include Tax]")).ToString();
                            string v = GetDataValue<string>(dr, "[Transaction Header].[SKU Id].[SKU Id].[MEMBER_CAPTION]");

                            KeyValuePair<string, string> kv = new KeyValuePair<string, string>(k, v);
                            result.Add(kv);
                        }

                        return result;
                    }
                }
            }
        }
    }
}
