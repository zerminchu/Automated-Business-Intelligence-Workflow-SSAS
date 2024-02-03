using BI.Jobs.DAC.Job;
using BI.Jobs.Shared;
using BI.Jobs.Shared.Enum;
using BI.Jobs.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Logic.Summarized
{

    public class SummarizedSalesProductComponent: ISummarizedComponent
    {
        private string _requestId;
        private string _performer;
        private ILogComponent _logger;

        public SummarizedSalesProductComponent(string requestId, string performer, ILogComponent logger)
        {
            _requestId = requestId;
            _performer = performer;
            _logger = logger;
        }

        public void Summarized(string datekey)
        {
            try
            {
                var sDAC = new SalesDAC();
                sDAC.SummarizedProduct(_requestId, datekey);
            }
            catch (Exception ex)
            {
                LogModel l = new LogModel(_requestId,
                    "Business", LogSeverity.error, ex.Message, ex.ToString(), _performer);
                _logger.CreateSysLog(l);
            }
        }
    }
}
