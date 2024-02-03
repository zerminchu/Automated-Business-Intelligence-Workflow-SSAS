using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Utilities
{
    public static class IISComponent
    {
        public static void DoIISReset()
        {
            Process iisReset = new Process();
            iisReset.StartInfo.FileName = "iisreset.exe";
            iisReset.StartInfo.RedirectStandardOutput = true;
            iisReset.StartInfo.UseShellExecute = false;
            iisReset.Start();
            iisReset.WaitForExit();
        }
    }
}
