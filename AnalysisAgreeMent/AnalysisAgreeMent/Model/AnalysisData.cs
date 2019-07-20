using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisAgreeMent.Model
{
    public class AnalysisData
    {
        public AgreementType AgreeMentXCP { get; set; }

        public string BaudRateDbc { get; set; }
        public List<AnalysisSignal> AnalysisDbcDataList { get; set; }

        public List<AnalysisSignal> AnalysisiXcpDataList { get; set; }
    }
}
