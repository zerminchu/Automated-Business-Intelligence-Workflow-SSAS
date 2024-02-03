﻿using BI.Jobs.DAC.Job;
using BI.Jobs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.DataSource
{
    public class SalesVsStore: IDataSourceComponent
    {
        public void Generate(string filePath)
        {
            var stringBuilder = new StringBuilder();
            Boolean withHeader = false;
            var x = new SalesDAC();
            var summarizedStores = x.GetSummarizedStore();
            if (summarizedStores != null && summarizedStores.Count > 0)
            {
                string csvLine = string.Empty;
                //convert each row to csv string and append to files
                foreach (var line in summarizedStores.ToCsv(header: withHeader))
                    stringBuilder.AppendLine(line);

                FileUtilities.WriteToFileWithPath(filePath, stringBuilder.ToString());
            }
        }
    }
}
