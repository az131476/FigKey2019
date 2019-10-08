using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisAgreeMent.Model.XCP
{
    /// <summary>
    /// 存储解析数据
    /// </summary>
    public class XcpData
    {
        /// <summary>
        /// 数据状态，true-数据存在
        /// </summary>
        public bool DataStatus { get; set; }
        public AgreementType AgreeMentType { get; set; }

        public List<MeasureMent> MeasureData { get; set; }

        public List<Characteristic> CharacterData { get; set; }

        public List<CompuMethod> MetholdData { get; set; }

        public List<TableClass> TableData { get; set; }

        public List<RecordLayoutClass> RecordData { get; set; }

        public List<MemorySegmentClass> MemorySedData { get; set; }

        public List<PropertyClass> PropertyData { get; set; }

        public XcpOnCan XcpOnCanData { get; set; }

        public IF_DATA_ASAP1B_CCP IF_DATA_ASAP1B_CCP_DATA { get; set; }
    }
}
