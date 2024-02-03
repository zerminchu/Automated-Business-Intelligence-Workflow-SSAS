using BI.Jobs.DAC.API;
using BI.Jobs.DAC.Job;
using BI.Jobs.Logic.Import.Model;
using BI.Jobs.Logic.Import.ProductValidation;
using BI.Jobs.Shared;
using BI.Jobs.Shared.Enum;
using BI.Jobs.Shared.Model;
using BI.Jobs.Shared.Model.ARMS;
using BI.Jobs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.ImportJob
{
    public class ProductImportJob : IImportJob
    {
        protected const string ARMS = "ARMS";

        protected const string targetDirectoryFolderDateFormat = GeneralConfig.targetDirectoryFolderDateFormat;

        protected ImportParam _Param;
        protected ILogComponent _Logger;
        protected int _MaxFileSize;
        protected string _JobType;

        public ProductImportJob(ImportParam param, string jobType, int maxFileSize, ILogComponent logger)
        {
            _Param = param;
            _Logger = logger;
            _MaxFileSize = maxFileSize;
            _JobType = jobType;
        }

        public void ProcessImportMasterProductJob(List<ProcessingJob> jobs)
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
                    SKUMasterModel model = GetProductMasterModel(content);
                    decimal apiVersion = 1;

                    if (model == null)
                    {
                        LogInfo(LogSeverity.info, $"Processing {j.RequestId} - {j.JobId} with file {j.FilePath}", $"Model is empty or invalid");
                        jobDAC.UpdateProcessingJob(j.JobId, JobStatuses.Error, _Param.Performer);
                        continue;
                    }

                    var productdata = model.product_data;
                    var ProductValidator = new ProductValidationManagerFactory().GetProductValidationManager(apiVersion);

                    foreach (var p in productdata)
                    {
                        var errors = ProductValidator.ValidateModel(p);
                        if (errors.Count > 0)
                        {
                            string error = JsonSerializer.Serialize(errors)!;

                            LogInfo(LogSeverity.error, $"Processing {j.RequestId} - {j.JobId} with file {j.FilePath}", $"Validation Failed. " + error);
                            jobDAC.UpdateProcessingJob(j.JobId, JobStatuses.Error, _Param.Performer);
                            break;
                        }
                    }

                    LogInfo(LogSeverity.info, $"Processing {j.RequestId} - {j.JobId} with file {j.FilePath}", $"Ok to insert");

                    var productDAC = new ProductDAC();
                    foreach (var p in productdata)
                    {
                        productDAC.UpsertProduct(p);
                    }

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
                ProcessImportMasterProductJob(jobs);
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



        public SKUMasterModel GetProductMasterModel(string content)
        {
            if (_Param.ClientCode.Equals(ARMS))
            {
                ARMSSkuModel armsModel = JsonSerializer.Deserialize<ARMSSkuModel>(content)!;
                SKUMasterModel model = new SKUMasterModel();

                if (armsModel != null && armsModel.products_data != null && armsModel.products_data.Count > 0)
                {
                    model.product_data = new List<SKUModel>();
                    foreach (var x in armsModel.products_data)
                    {
                        SKUModel s = new SKUModel();
                        s.sku_item_code = x.sku_item_code;
                        s.product_description = x.product_description;
                        model.product_data.Add(s);
                    }
                }

                return model;
            }
            else
            {
                SKUMasterModel model = JsonSerializer.Deserialize<SKUMasterModel>(content)!;
                return model;
            }
        }
    }
}
