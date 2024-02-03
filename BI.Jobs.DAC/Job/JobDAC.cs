using BI.Jobs.Shared.Enum;
using BI.Jobs.Shared.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.DAC.Job
{
    public class JobDAC: DAC
    {
        public void CreateProcessingJob(string requestId, string jobType, DateTime dt, string filePath, string performer)
        {
            const string SQL_QUERY = @"INSERT INTO [dbo].[ProcessingJob]
           ([RequestId], [JobType], [JobDate], [FilePath],[JobStatus],[CreatedDate],[CreatedBy],[UpdatedDate],[UpdatedBy])
     VALUES
           (@RequestId,@JobType, @JobDate,@FilePath,@JobStatus,@CreatedDate,@CreatedBy,@UpdatedDate,@UpdatedBy)";

            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (var cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@RequestId", requestId);
                    cmd.Parameters.AddWithValue("@JobType", jobType);
                    cmd.Parameters.AddWithValue("@JobDate", dt);
                    cmd.Parameters.AddWithValue("@FilePath", filePath);
                    cmd.Parameters.AddWithValue("@JobStatus", JobStatuses.New);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CreatedBy", performer);
                    cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdatedBy", performer);

                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<ProcessingJob> GetNewProcessingJob(string jobType)
        {
            const string SQL_QUERY =
               @" SELECT [JobId], [JobType]
      ,[RequestId]
      ,[FilePath]
      ,[JobStatus]
      ,[Remarks]
      ,[CreatedDate]
      ,[CreatedBy]
      ,[UpdatedDate]
      ,[UpdatedBy]
  FROM [dbo].[ProcessingJob]
  WHERE JobStatus = @JobStatus AND JobType = @JobType ORDER BY RequestId ";

            var result = new List<ProcessingJob>();
            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@JobStatus", JobStatuses.New);
                    cmd.Parameters.AddWithValue("@JobType", jobType);

                    sqlConnection.Open();

                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var job = new ProcessingJob();
                            job.JobId = GetDataValue<long>(dr, "JobId");
                            job.RequestId = GetDataValue<string>(dr, "RequestId");
                            job.JobType = GetDataValue<string>(dr, "JobType");
                            job.FilePath = GetDataValue<string>(dr, "FilePath");
                            job.JobStatus = GetDataValue<JobStatuses>(dr, "JobStatus");
                            job.Remarks = GetDataValue<string>(dr, "Remarks");
                            job.CreatedDate = GetDataValue<DateTime>(dr, "CreatedDate");
                            job.CreatedBy = GetDataValue<string>(dr, "CreatedBy");
                            job.UpdatedDate = GetDataValue<DateTime>(dr, "UpdatedDate");
                            job.UpdatedBy = GetDataValue<string>(dr, "UpdatedBy");

                            result.Add(job);
                        }

                        return result;
                    }

                }
            }
        }

        public void UpdateProcessingJob(long jobId, JobStatuses status, string performer)
        {
            const string SQL_QUERY = @"UPDATE ProcessingJob SET JobStatus = @JobStatus, UpdatedDate = @dt, UpdatedBy = @performer WHERE JobId = @JobId";

            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (var cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@JobId", jobId);
                    cmd.Parameters.AddWithValue("@performer", performer);
                    cmd.Parameters.AddWithValue("@dt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@JobStatus", status);
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
