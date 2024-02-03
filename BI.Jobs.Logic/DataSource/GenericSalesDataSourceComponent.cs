using BI.Jobs.DAC.Job;
using BI.Jobs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.DataSource
{
    public class GenericSalesDataSourceComponent: IDataSourceComponent
    {
        //private const string HEADER = "Unit Price,Quantity,Total Amount,Region,Branch,Department,Product,Category,Cashier,Receipt No,GroupCode,Group,Year,Month,Day,Transaction Date,SKU,Basket Size,Divison,Route,Customer";

        //private const string FILENAME = "bi_general";
        //private const string FILEEXTENSION = "csv";

        public void Generate(string fileName)
        {
            ////find target filepath to keep the result
            //string targetFile = FileUtilities.GetStaticFilePath(FILENAME, FILEEXTENSION);

            ////high chances that there are existing file. 
            ////do not touch the existing file until we confirmed we can generate new file
            ////get a temp name to generate new file first
            //string tempFile = FileUtilities.CreateTempFile(targetFile);

            ////write the header to the newly created temp file
            //FileUtilities.WriteToFile(FILENAME, FILEEXTENSION, HEADER);

            ////get the database record via DAC
            ////convert to csv string
            ////append to file
            //var stringBuilder = new StringBuilder();
            //Boolean withHeader = false;

            //var x = new SalesDAC();
            //var summarizedProducts = x.GetSummarizedProduct();
            //if (summarizedProducts != null && summarizedProducts.Count > 0)
            //{
            //    string csvLine = string.Empty;
            //    //convert each row to csv string and append to files
            //    foreach (var line in summarizedProducts.ToCsv(header: withHeader))
            //        stringBuilder.AppendLine(line);

            //    FileUtilities.WriteToFile(FILENAME, FILEEXTENSION, stringBuilder.ToString());
            //}

            ////completed.
            ////this mean we can now rename the temp file to the targetfile
            ////but there are chances that there are existing target file being used by IIS
            ////iis reset to release the file
            //IISComponent.DoIISReset();

            ////then rename existing file to a obsolete file if existing file existed
            //int counter = 0;
            //while (true)
            //{
            //    counter++;
            //    string overwirttenFile = FileUtilities.GetOverwrittenFile(targetFile, counter);
            //    if (!File.Exists(overwirttenFile))
            //    {
            //        FileUtilities.RenameFile(targetFile, overwirttenFile);
            //        break;
            //    }
            //}

            ////then rename the temp file to target file
            //FileUtilities.RenameFile(tempFile, targetFile);
        }

       

        
    }
}
