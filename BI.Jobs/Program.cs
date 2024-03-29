﻿// See https://aka.ms/new-console-template for more information
using BI.Jobs;
using BI.Jobs.Logic.DataSource;
using BI.Jobs.Logic.Import;
using BI.Jobs.Logic.Import.Component;
using BI.Jobs.Logic.Import.ImportJob;
using BI.Jobs.Logic.Import.Model;
using BI.Jobs.Logic.Sample;
using BI.Jobs.Logic.Summarized;
using BI.Jobs.Shared;
using BI.Jobs.Shared.Library;
using BI.Jobs.Shared.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

string key = "ASPNETCORE_ENVIRONMENT";
var cm = new ConsoleManager(true);

Console.WriteLine("Step 1 - Start Read environmental setting");
#region step 1
var env = Environment.GetEnvironmentVariable(key);
AppSettingsUtil.Register(env);

var builder = new HostBuilder();
builder
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        config.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((context, s) =>
    {
      
        s.BuildServiceProvider();
    });
#endregion
Console.WriteLine("Step 1 - Finish Read environmental setting");


#region call API to get the source file from UNICON web service
//call web service, drop the file we want into the GeneralConfig:targetDirectory
//so this application will go to that directly and look for the file
#endregion


//client code
string clientCode = AppSettingsUtil.GetValue<string>("GeneralConfig:ClientCode");
if (String.IsNullOrWhiteSpace(clientCode))
{
    throw new Exception("Client Code is not being setup properly. ");
}

//file type naming pattern
const string masterStoreFileCode = "Store*.txt";
const string masterProductFileCode = "Product*.txt";
const string salesFileCode = "*Sales*";

//directory to read the files
string targetDirectory = AppSettingsUtil.GetValue<string>("GeneralConfig:targetDirectory");


 DateTime dt = DateTime.Now;

//generic parameters to identify the instances of job
string currentRunValueAsJobId = Guid.NewGuid().ToString();
LogManagerType type = LogManagerType.db;
ILogComponentFactory logFactory = new LogComponentFactory(type);
ILogComponent logger = logFactory.GetLogComponent();

//import master data
cm.Print($"Step 2 - Start import Store Master Data");
#region step 2
#region create job
string importMasterStoreDataFileCode = masterStoreFileCode;
string importMasterStoreDataJobType = JobType.IMPORTMASTERSTORE;
string importMasterStoreDataJobId = currentRunValueAsJobId;
string importMasterStoreDataJobPerformer = ConfigManager.GetRunner();
ImportParam importMasterStoreDataJobParams = new ImportParam(clientCode, importMasterStoreDataFileCode, 
    dt, targetDirectory, importMasterStoreDataJobId, importMasterStoreDataJobPerformer);


IImportComponent createImportStoreJobComponent = new GeneralImportComponent(importMasterStoreDataJobParams,
    importMasterStoreDataJobType, logger);
createImportStoreJobComponent.CreateImportJob();
#endregion

#region process job
string processMasterStoreJobFileCode = masterStoreFileCode;
string processMasterStoreJobType = JobType.IMPORTMASTERSTORE;
string processMasterStoreJobId = currentRunValueAsJobId;
string processMasterStoreJobPerformer = ConfigManager.GetRunner();
ImportParam processMasterStoreJobParams = new ImportParam(clientCode, processMasterStoreJobFileCode, 
    dt, targetDirectory, processMasterStoreJobId, processMasterStoreJobPerformer);

IImportComponent importMasterStoreComponent = new GeneralImportComponent(processMasterStoreJobParams,
    processMasterStoreJobType, logger);
importMasterStoreComponent.ProcessImportJob();

#endregion
#endregion
cm.Print($"Step 2 - Store Master Data Imported Successfully");



cm.Print($"Step 3 - Start import Product Master Data");
#region step 3
#region create job
string importMasterProductDataFileCode = masterProductFileCode;
string importMasterProductDataJobType = JobType.IMPORTMASTERPRODUCT;
string importMasterProductDataJobId = currentRunValueAsJobId;
string importMasterProductDataJobPerformer = ConfigManager.GetRunner();
ImportParam importMasterProductDataJobParams = new ImportParam(clientCode, importMasterProductDataFileCode,
    dt, targetDirectory, importMasterProductDataJobId, importMasterProductDataJobPerformer);

IImportComponent createImportProductJobComponent = new GeneralImportComponent(importMasterProductDataJobParams,
    importMasterProductDataJobType, logger);
createImportProductJobComponent.CreateImportJob();
#endregion
#region process job
string processMasterProductJobFileCode = masterProductFileCode;
string processMasterProductJobType = JobType.IMPORTMASTERPRODUCT;
string processMasterProductJobId = currentRunValueAsJobId;
string processMasterProductJobPerformer = ConfigManager.GetRunner();
ImportParam processMasterProductJobParams = new ImportParam(clientCode, processMasterProductJobFileCode,
    dt, targetDirectory, processMasterProductJobId, processMasterProductJobPerformer);

IImportComponent importMasterProductComponent = new GeneralImportComponent(processMasterProductJobParams,
    processMasterProductJobType, logger);
importMasterProductComponent.ProcessImportJob();
#endregion
#endregion
cm.Print($"Step 3 - Product Master Data Imported Successfully");


cm.Print($"Step 4 - Start Import Sales Data");
#region step 4
#region create job
string createSalesJobFileCode = salesFileCode;
string createSalesJobType = JobType.IMPORTSALES;
string createSalesJobId = currentRunValueAsJobId;
string createSalesJobPerformer = ConfigManager.GetRunner();
ImportParam createSalesJobParams = new ImportParam(clientCode, createSalesJobFileCode, dt, targetDirectory, createSalesJobId, createSalesJobPerformer);

IImportComponent createJobComponent = new GeneralImportComponent(createSalesJobParams, createSalesJobType, logger);
createJobComponent.CreateImportJob();
#endregion
#region process job
string processSalesJobFileCode = salesFileCode;
string processSalesJobType = JobType.IMPORTSALES;
string processSalesJobId = currentRunValueAsJobId;
string processSalesJobPerformer = ConfigManager.GetRunner();
ImportParam processSalesJobParams = new ImportParam(clientCode, processSalesJobFileCode, dt, targetDirectory, processSalesJobId, processSalesJobPerformer);

IImportComponent importJobComponent = new GeneralImportComponent(processSalesJobParams, processSalesJobType, logger);
importJobComponent.ProcessImportJob();
#endregion
#endregion
cm.Print($"Step 4 - Finish Import Sales Data");   



cm.Print($"Step 5 - Start Summarized Sales Data"); 
#region step 5
string sumRequestId = currentRunValueAsJobId;
string sumPerformer = ConfigManager.GetRunner();
var salesSummarizedComponent = new SummarizedComponentFactory()
    .GetSummarziedComponent(SummarizedComponentType.SALES, sumRequestId, sumPerformer, logger);
salesSummarizedComponent.Summarized(dt.ToString("yyyyMMdd"));
#endregion
cm.Print($"Step 5 - Summarized Sales Data Imported Successfully");


cm.Print($"Step 6 - Start Generate Data source file");
#region step 6
DataSourceManager SalesVsProductManager = new DataSourceManager(clientCode, DataSourceType.SalesVsProduct);
SalesVsProductManager.Generate();
SalesVsProductManager.HouseKeeping();

DataSourceManager SalesVsStoreManager = new DataSourceManager(clientCode, DataSourceType.SalesVsStore);
SalesVsStoreManager.Generate();
SalesVsStoreManager.HouseKeeping();

#endregion
cm.Print($"Step 6 - Data source file generated successfully");

