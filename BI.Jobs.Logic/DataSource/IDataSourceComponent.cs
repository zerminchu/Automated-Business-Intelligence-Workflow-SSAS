using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.DataSource
{
    public interface IDataSourceComponent
    {
        public void Generate(string filePath);
    }
}
