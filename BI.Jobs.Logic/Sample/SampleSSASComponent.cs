using BI.Jobs.DAC.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Sample
{
    public class SampleSSASComponent
    {
        public List<KeyValuePair<string, string>> GetSampleValue(int month, int year)
        {
            return new SSASCubeSampleDAC().GetSampleData(month, year);
        }
    }
}
