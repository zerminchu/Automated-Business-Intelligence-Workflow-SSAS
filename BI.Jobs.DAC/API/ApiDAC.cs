using BI.Jobs.Shared.Collection;
using BI.Jobs.Shared.Enum;
using BI.Jobs.Shared.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.DAC.API
{
    public class ApiDAC : DAC
    {
        public List<ApiModel> GetApi(string type, decimal version)
        {
            const string SQL_QUERY =
               @" SELECT [Id]
      ,[ApiType]
      ,[ApiVersion]
      ,[ApiStatus]
      ,[CreatedDate]
      ,[UpdatedDate]
      ,[CreatedBy]
      ,[UpdatedBy]
      ,[Version]
  FROM [dbo].[ApiManager] WHERE ApiType = @type AND ApiStatus = @status AND ApiVersion  = @version ";

            var result = new List<ApiModel>();
            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@version", version);
                    cmd.Parameters.AddWithValue("@status", Statuses.Active);

                    sqlConnection.Open();

                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var job = new ApiModel();
                            job.Id = GetDataValue<int>(dr, "Id");
                            job.ApiType = GetDataValue<string>(dr, "ApiType");
                            job.ApiVersion = GetDataValue<decimal>(dr, "ApiVersion");
                            job.ApiStatus = GetDataValue<Statuses>(dr, "ApiStatus");;
                            job.CreatedDate = GetDataValue<DateTime>(dr, "CreatedDate");
                            job.CreatedBy = GetDataValue<string>(dr, "CreatedBy");
                            job.UpdatedDate = GetDataValue<DateTime>(dr, "UpdatedDate");
                            job.UpdatedBy = GetDataValue<string>(dr, "UpdatedBy");
                            job.Version = GetDataValue<string>(dr, "Version");

                            result.Add(job);
                        }

                        return result;
                    }

                }
            }
        }
    }
}
