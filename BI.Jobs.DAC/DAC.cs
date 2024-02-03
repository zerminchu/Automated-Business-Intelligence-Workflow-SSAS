using BI.Jobs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.DAC
{
    public abstract class DAC
    {
        private const string SQL_CONNETION_NAME = "SQL";

        private const string MDX_CONNECTION_NAME = "MDX";

        protected string SQLConnectionString =>
            AppSettingsUtil.GetConnectionString(SQL_CONNETION_NAME);

        protected string MDXConnectionString =>
           AppSettingsUtil.GetConnectionString(MDX_CONNECTION_NAME);

        protected static T GetDataValue<T>(IDataReader dr, string columnName)
        {
            int i = dr.GetOrdinal(columnName);

            if (!dr.IsDBNull(i))
                return (T)dr.GetValue(i);
            else
                return default(T);
        }
    }
}
