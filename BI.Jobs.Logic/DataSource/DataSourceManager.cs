using BI.Jobs.Shared.Model;
using BI.Jobs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.DataSource
{
    public class DataSourceManager
    {
        private string _clientCode { get; set; }
        private string _dataSourceType { get; set; }
        private DataSourceSettingModel _setting;
        private IDataSourceComponent _component;

        public DataSourceManager(string clientCode, string dataSourceType)
        {
            _clientCode = clientCode;
            _dataSourceType = dataSourceType;
            _component = new DataSourceComponentFactory().GetDataSource(dataSourceType, clientCode);

            if (string.IsNullOrWhiteSpace(dataSourceType))
                throw new Exception("Invalid DataSourceType for DataSourceManager");

            _setting = AppSettingsUtil.GetSection<DataSourceSettingModel>($"DataSourceSettings:{dataSourceType}");
            if(_setting == null || String.IsNullOrWhiteSpace(_setting.fileName) 
                || String.IsNullOrWhiteSpace(_setting.fileExtension))
                throw new Exception($"Invalid appsetting config for DataSourceManager {dataSourceType}");
        }

        public void Generate()
        {
            string fileName = _setting.fileName;
            string fileExtension = _setting.fileExtension;
            string header = _setting.header;
            string hostingPath = _setting.StaticFileLocation;

            //find target filepath 
            string targetFile = FileUtilities.GetStaticFilePath(fileName, fileExtension, hostingPath);

            //get a temp name to generate new file first
            string tempFile = FileUtilities.CreateTempFile(targetFile);
            //write the header to the newly created temp file
            FileUtilities.WriteToFileWithPath(tempFile, header);


            //get the database record via DAC
            //convert to csv string
            //append to file
            _component.Generate(tempFile);

            //completed.
            //this mean we can now rename the temp file to the targetfile
            //but there are chances that there are existing target file being used by IIS
            //iis reset to release the file
            IISComponent.DoIISReset();

            //then rename existing file to a obsolete file if existing file existed
           int counter = 0;
            while (true)
           {
                counter++;
               string overwirttenFile = FileUtilities.GetOverwrittenFile(targetFile, counter);
                if (!File.Exists(targetFile))
                {
                    FileUtilities.RenameFile(targetFile, overwirttenFile);
                   break;
                }
               else if (!File.Exists(overwirttenFile))
               {
                    FileUtilities.RenameFile(targetFile, overwirttenFile);
                   break;
                }
            }

            //then rename the temp file to target file
            FileUtilities.RenameFile(tempFile, targetFile);
        }

        public void HouseKeeping()
        {
            string fileName = _setting.fileName;
            string fileExtension = _setting.fileExtension;
            string header = _setting.header;
            string hostingPath = _setting.StaticFileLocation;
            int keepAliveInMinute = _setting.KeepNonLiveFileForXMinutes;

            //find target directory 
            string directory = FileUtilities.GetStaticFileDirectory(hostingPath);
            string overwrittenFileNamePrefix = $"{fileName}.{fileExtension}{FileUtilities.GetOverwrittenModifier()}";
            string tempFileNamePrefix = $"{fileName}.{fileExtension}{FileUtilities.GetTempModifier()}";


            //list out all files
            foreach (var f in Directory.GetFiles(directory))
            {
                string fName = Path.GetFileName(f);
                
                if (fName.StartsWith(overwrittenFileNamePrefix) || fName.StartsWith(tempFileNamePrefix))
                {
                    //file pattern match
                    //check how long since the files is modified
                    DateTime lastModifiedDate = System.IO.File.GetLastWriteTime(f);
                    TimeSpan span = (DateTime.Now).Subtract(lastModifiedDate);
                    double differenceInMinute = span.TotalMinutes;

                    //remove is live longer than allowed duration
                    if (differenceInMinute > keepAliveInMinute)
                        File.Delete(f);

                }
            }
        }
    }
}
