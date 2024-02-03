using BI.Jobs.Logic.Import.Model;
using BI.Jobs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.ImportJob
{
    public static class JobType
    {
        public const string IMPORTMASTERSTORE = "IMPORTMASTERSTORE";
        public const string IMPORTMASTERPRODUCT = "IMPORTMASTERPRODUCT";
        public const string IMPORTSALES = "IMPORTSALES";

        public const string SUMMARIZEDSALES_GRAND = "SUMMARIZEDSALES_GRAND";
    }

    public class ImportJobFactory
    {
        //ImportParam param, string jobType, int maxFileSize, ILogComponent logger
        public IImportJob GetImportJob(ImportParam param, string jobType, int maxFileSize, 
            ILogComponent logger, decimal version)
        {
            if (jobType.Equals(JobType.IMPORTSALES))
            {
                return new SalesImportJob(param, jobType, maxFileSize, logger);
            }
            else if(jobType.Equals(JobType.IMPORTMASTERSTORE))
            {
                return new StoreImportJob(param, jobType, maxFileSize, logger);
            }
            else if (jobType.Equals(JobType.IMPORTMASTERPRODUCT))
            {
                return new ProductImportJob(param, jobType, maxFileSize, logger);
                //return new ProductImportJob(param, jobType, maxFileSize, logger);
            }
            else
            {
                throw new Exception("Invalid Job Type");
            }
        }
    }
}
