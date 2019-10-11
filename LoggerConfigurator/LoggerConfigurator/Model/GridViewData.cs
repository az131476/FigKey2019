using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;
using AnalysisAgreeMent.Model;
using AnalysisAgreeMent.Model.XCP;
using AnalysisAgreeMent.Model.DBC;

namespace FigKeyLoggerConfigurator.Model
{
    public class GridViewData
    {
        public class GridViewColumns
        {
            //名称+描述+单位+数据类型+数据长度+是否摩托罗拉+开始地址+截取长度+数据地址+系数+偏移量
            public const string ORDER = "序号";
            public const string NAME = "名称";
            public const string DESCRIBLE = "描述";
            public const string UNIT = "单位";
            public const string TYPE = "类型";
            public const string DATA_LEN = "数据长度";
            public const string BYTE_ORDER = "字节顺序";//motorola  or inter
            public const string START_INDEX = "开始地址";
            public const string BITDATA_LEN = "截取长度";
            public const string DATA_ADDRESS = "数据地址";
            public const string FACTOR = "系数";
            public const string OFF_SET = "偏移量";

            /// <summary>
            /// 非必须字段
            /// </summary>
            public const string ECU_NAME = "ECU名称";
            public const string BYTES_LEN = "长度";
            public const string MEASUREMENT_MODE = "模型尺寸";
            public const string CYLE = "圈数";

        }
        public List<int> LimitTimeListSegMent { get; set; }

        public List<int> LimitTimeList10ms { get; set; }

        public List<int> LimitTimeList100ms { get; set; }

        public List<int> DbcCheckIndex { get; set; }
    }

    public enum TimeLimitType
    {
        _segMent,
        _10ms,
        _100ms
    }

    public class LimitTimeCfg
    {

        public int RowIndex { get; set; }

        public TimeLimitType LimitType { get; set; }
    }

    public class GridExportContent
    {
        public AgreementType AgreementTypeCan1 { get; set; }

        public AgreementType AgreementTypeCan2 { get; set; }

        public CurrentCanType CurrentSelectCanType { get; set; }

        public RadGridView GridViewCan1 { get; set; }

        public RadGridView GridViewCan2 { get; set; }

        public GridViewData GridDataA2lSelectRow { get; set; }

        public GridViewData GridDataDbcSelectRow { get; set; }

        public AnalysisData AnalysisDataCan1 { get; set; }

        public AnalysisData AnalysisDataCan2 { get; set; }

        public XcpData XcpDataCan1 { get; set; }

        public XcpData XcpDataCan2 { get; set; }
    }
}
