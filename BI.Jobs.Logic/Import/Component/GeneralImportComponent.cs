using BI.Jobs.DAC.API;
using BI.Jobs.DAC.Job;
using BI.Jobs.Logic.Import.ImportJob;
using BI.Jobs.Logic.Import.Model;
using BI.Jobs.Logic.Import.SalesImport;
using BI.Jobs.Logic.Import.SalesValidation;
using BI.Jobs.Shared;
using BI.Jobs.Shared.Collection;
using BI.Jobs.Shared.Enum;
using BI.Jobs.Shared.Library;
using BI.Jobs.Shared.Model;
using BI.Jobs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.Component
{
    public class GeneralImportComponent : IImportComponent
    {
        protected const string targetDirectoryFolderDateFormat = GeneralConfig.targetDirectoryFolderDateFormat;

        protected ImportParam _Param;
        protected ILogComponent _Logger;
        protected int _MaxFileSize;
        protected string _JobType;


        public GeneralImportComponent(ImportParam param, string jobType, ILogComponent logger)
        //public GeneralImportComponent(ImportParam param, string jobType)
        {
            _Param = param;
            _Logger = new LogComponent();
            _MaxFileSize = ConfigManager.GetMaxFileSizeInMB();
            _JobType = jobType;
        }

        

        public void CreateImportJobWithFile()
        {
            string targetDirectoryWithDate = _Param.TargetDirectory + "\\" + _Param.JobDate.ToString(targetDirectoryFolderDateFormat);
            if (!Directory.Exists(targetDirectoryWithDate))
                Directory.CreateDirectory(targetDirectoryWithDate);

            DirectoryInfo d = new DirectoryInfo(targetDirectoryWithDate);

            FileInfo[] Files = d.GetFiles($"{_Param.FileCode}");
            //FileInfo[] Files = d.GetFiles($"{_Param.FileCode}*.txt");
            if (Files.Length <= 0)
            {
                LogModel l = new LogModel(_Param.RequestId,
                    "Business", LogSeverity.info, $"No files for {_Param.FileCode}", "", _Param.Performer);
                _Logger.CreateSysLog(l);
                return;
            }

            string targetDirectoryForProcessing = targetDirectoryWithDate + $"\\{_Param.RequestId}\\Processing";
            if (!Directory.Exists(targetDirectoryForProcessing))
                Directory.CreateDirectory(targetDirectoryForProcessing);

            var jobDAC = new JobDAC();
            foreach (var f in Files)
            {
                try
                {
                    string finalFilePath = $@"{targetDirectoryForProcessing}\\{f.Name}";
                    f.MoveTo(finalFilePath);
                    jobDAC.CreateProcessingJob(_Param.RequestId, _JobType, _Param.JobDate, finalFilePath, _Param.Performer);
                }
                catch (Exception)
                {
                    //possibly dirty read due to other thread grab the file
                    //ignore this error and continue
                }
            }
        }

        public void MoveFinishedJob(string targerFile)
        {
            string targetDirectoryWithDate = _Param.TargetDirectory + "\\" + _Param.JobDate.ToString(targetDirectoryFolderDateFormat);
            
            //string targetDirectoryForProcessing = targetDirectoryWithDate + $"\\{_Param.RequestId}\\Processing";
            //if (!Directory.Exists(targetDirectoryForProcessing))
            //    Directory.CreateDirectory(targetDirectoryForProcessing);

            //DirectoryInfo d = new DirectoryInfo(targetDirectoryForProcessing);
            //FileInfo[] Files = d.GetFiles($"{_Param.FileCode}*.txt");
            //if (Files.Length <= 0)
            //{
            //    LogModel l = new LogModel(_Param.RequestId,
            //            "Business", LogSeverity.info, $"No files for {_Param.FileCode} for finished", "", _Param.Performer);
            //    _Logger.CreateSysLog(l);
            //    return;
            //}

            //string targetDirectoryForFinished = targetDirectoryWithDate + $"\\{_Param.RequestId}\\Finished";
            //if (!Directory.Exists(targetDirectoryForFinished))
            //    Directory.CreateDirectory(targetDirectoryForFinished);

            //var jobDAC = new JobDAC();

            //foreach (var f in Files)
            //{
            //    try
            //    {
            //        string finalFilePath = $@"{targetDirectoryForFinished}\\{f.Name}";
            //        f.MoveTo(finalFilePath);
            //    }
            //    catch (Exception)
            //    {
            //        //possibly dirty read due to other thread grab the file
            //        //ignore this error and continue
            //    }
            //}
        }

        public void CreateImportJob()
        {
            try
            {
                LogInfo(LogSeverity.info, "CreateImportJob", "");
                CreateImportJobWithFile();
            }
            catch (Exception ex)
            {
                LogModel l = new LogModel(_Param.RequestId,
                   "Business", LogSeverity.error, ex.Message, ex.ToString(), _Param.Performer);
                _Logger.CreateSysLog(l);
            }
        }

        public void ProcessImportJob()
        {
            try
            {
                var jobs = GetNewJob();
                if (jobs.Count <= 0)
                {
                    LogInfo(LogSeverity.info, "ProcessImportJob", "No New job for process");
                    return;
                }
                ImportJobFactory factory = new ImportJobFactory();
                IImportJob importJob = factory.GetImportJob(_Param, _JobType, _MaxFileSize, _Logger, 1);
                importJob.Import(jobs);
            }
            catch (Exception ex)
            {
                LogModel l = new LogModel(_Param.RequestId,
                   "Business", LogSeverity.error, ex.Message, ex.ToString(), _Param.Performer);
                _Logger.CreateSysLog(l);
            }
        }


        public List<ProcessingJob> GetNewJob()
        {
            return new JobDAC().GetNewProcessingJob(_JobType);
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
