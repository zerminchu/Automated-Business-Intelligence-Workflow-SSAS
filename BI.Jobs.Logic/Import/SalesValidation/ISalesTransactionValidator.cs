using BI.Jobs.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Import.SalesValidation
{
    public interface ISalesTransactionValidator
    {
        public List<string> Validate(Tlog model);
    }
}
