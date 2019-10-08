using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalysisAgreeMent.Model.DBC;
using AnalysisAgreeMent.Model.XCP;
using AnalysisAgreeMent.Model;
using AnalysisAgreeMent.Analysis;
using AnalysisAgreeMent;

namespace LoggerConfigurator.Model
{
    class CanProtocolDataEntity
    {
        public XcpData CanXcpData { get; set; }

        public XcpHelper CanXcpHelper { get; set; }

        public DBCData CanDbcData { get; set; }

        public DbcHelper CanDbcHelper { get; set; }

        public AnalysisData CanAnalysisData { get; set; }

        public AgreementType AgreementType { get; set; }
    }
}
