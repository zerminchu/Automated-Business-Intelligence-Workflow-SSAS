using BI.Jobs.DAC.API;
using BI.Jobs.DAC.Job;
using BI.Jobs.Logic.Import.Model;
using BI.Jobs.Logic.Import.SalesImport;
using BI.Jobs.Logic.Import.SalesValidation;
using BI.Jobs.Shared;
using BI.Jobs.Shared.Enum;
using BI.Jobs.Shared.Model;
using BI.Jobs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.ImportJob
{
    public class SalesImportJob: IImportJob
    {
        protected const string targetDirectoryFolderDateFormat = GeneralConfig.targetDirectoryFolderDateFormat;


        protected ImportParam _Param;
        protected ILogComponent _Logger;
        protected int _MaxFileSize;
        protected string _JobType;

        public SalesImportJob(ImportParam param, string jobType, int maxFileSize, ILogComponent logger)
        {
            _Param = param;
            _Logger = logger;
            _MaxFileSize = maxFileSize;
            _JobType = jobType;
        }

        public void ProcessImportSalesJob(List<ProcessingJob> jobs)
        {
            var apiDAC = new ApiDAC();
            var jobDAC = new JobDAC();

            foreach (var j in jobs)
            {
                Console.WriteLine($"Processing {j.RequestId} - {j.JobId} with file {j.FilePath}");
                FileInfo info = new FileInfo(j.FilePath);
                Console.WriteLine(info.Length);
                if (info.Length > _MaxFileSize)
                {
                    LogInfo(LogSeverity.info, $"Processing {j.RequestId} - {j.JobId} with file {j.FilePath}", $"File size {info.Length} exceed {_MaxFileSize}");
                    jobDAC.UpdateProcessingJob(j.JobId, JobStatuses.Error, _Param.Performer);
                    continue;
                }

                //if (info.Name.Contains("Sales_20220918.txt"))
                //{
                //    var debug = true;
                //}

                string content = string.Empty;
                using (StreamReader streamReader = new StreamReader(j.FilePath, Encoding.UTF8))
                {
                    content = streamReader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        LogInfo(LogSeverity.info, $"Processing {j.RequestId} - {j.JobId} with file {j.FilePath}", $"No Content");
                        jobDAC.UpdateProcessingJob(j.JobId, JobStatuses.Error, _Param.Performer);
                        continue;
                    }
                }

                try
                {
                    SalesModel model = JsonSerializer.Deserialize<SalesModel>(content);

                    //check if api version ok
                    //decimal apiVersion = decimal.Parse(model.api_version);
                    //var apis = apiDAC.GetApi(ApiType.Sales, apiVersion);
                    //if (apis == null)
                    //{
                    //    LogInfo(LogSeverity.audit, $"Processing {j.RequestId} - {j.JobId} with file {j.FilePath}", $"API type and version unavailable");
                    //    continue;
                    //}
                    decimal apiVersion = 1;


                    if (model.@params.tlogs.Count <= 0)
                    {
                        LogInfo(LogSeverity.info, $"Processing {j.RequestId} - {j.JobId} with file {j.FilePath}", $"No transaction record");
                        jobDAC.UpdateProcessingJob(j.JobId, JobStatuses.Error, _Param.Performer);
                        continue;
                    }

                    //select validation rule according to api
                    var SalesValidationManager = new SalesValidationManagerFactory().GetSalesValidationManager(apiVersion);
                    List<string> errors = SalesValidationManager.ValidateSalesModel(model);
                    if (errors.Count > 0)
                    {
                        LogInfo(LogSeverity.audit, $"Processing {j.RequestId} - {j.JobId} with file {j.FilePath}", $"Validation errors: {errors.Count}");
                        Console.WriteLine(JsonSerializer.Serialize(errors));
                        jobDAC.UpdateProcessingJob(j.JobId, JobStatuses.Error, _Param.Performer);

                        continue;
                    }

                    LogInfo(LogSeverity.info, $"Processing {j.RequestId} - {j.JobId} with file {j.FilePath}", $"Ok to insert");

                    var SalesImportComponent = new ImportComponentFactory().GetSalesImportComponent(apiVersion);
                    SalesImportComponent.Import(model, _Param.RequestId);

                    //job import success. move file
                    LogInfo(LogSeverity.info, $"Processing {j.RequestId} - {j.JobId} with file {j.FilePath}", $"Import success");
                    jobDAC.UpdateProcessingJob(j.JobId, JobStatuses.Completed, _Param.Performer);


                }
                catch (Exception ex)
                {
                    LogModel l = new LogModel(_Param.RequestId,
                        "Business", LogSeverity.error, ex.Message, ex.ToString(), _Param.Performer);
                    _Logger.CreateSysLog(l);

                    jobDAC.UpdateProcessingJob(j.JobId, JobStatuses.Error, _Param.Performer);
                }
            }
        }

        public void Import(List<ProcessingJob> jobs)
        {
            try
            {
                LogInfo(LogSeverity.info, $"ProcessImportJob for {_JobType}", "");
                //List<ProcessingJob> jobs = GetNewJob();
                if (jobs.Count <= 0)
                {
                    LogInfo(LogSeverity.info, $"ProcessImportJob for {_JobType}", "No new job");
                    return;
                }
                ProcessImportSalesJob(jobs);
            }
            catch (Exception ex)
            {
                LogModel l = new LogModel(_Param.RequestId,
                   "Business", LogSeverity.error, ex.Message, ex.ToString(), _Param.Performer);
                _Logger.CreateSysLog(l);
            }
        }

        #region log
        public void LogInfo(LogSeverity severity, string action, string message)
        {
            LogModel l = new LogModel(_Param.RequestId,
                    "Business", severity, $"{_Param.FileCode} - {_Param.JobDate.ToString(targetDirectoryFolderDateFormat)} - {_Param.RequestId} - {action}", message, _Param.Performer);
            _Logger.CreateSysLog(l);
        }
        #endregion
    }
}
