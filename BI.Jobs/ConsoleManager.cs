using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs
{
    public class ConsoleManager
    {
        private Boolean _appendTime;

        public ConsoleManager(Boolean appendTime)
        {
            this._appendTime = appendTime;
        }

        public void Print(string s)
        {
            if (_appendTime)
                Console.WriteLine($"{s} - {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
            else
                Console.WriteLine(s);
        }
    }
}
