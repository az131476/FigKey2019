using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisAgreeMent.Model.XCP
{
    public class IF_DATA_ASAP1B_CCP
    {
        public CCP_SEG CCP_SEG_DATA { get; set; }

        public CCP_10MS CCP_10MS_DATA { get; set; }

        public CCP_100MS CCP_100MS_DATA { get; set; }

        public class CCP_SEG
        {
            public string DAQ_NAME { get; set; }

            public string CAN_ID_FIXED { get; set; }
        }

        public class CCP_10MS
        {
            public string DAQ_NAME { get; set; }

            public string CAN_ID_FIXED { get; set; }
        }

        public class CCP_100MS
        {
            public string DAQ_NAME { get; set; }

            public string CAN_ID_FIXED { get; set; }
        }
        //master/slver
        public string CCP_VERSION { get; set; }

        public string BLOB_VERSION { get; set; }

        public string CAN_MSG_ID_SEND { get; set; }

        public string CAN_MSG_ID_RECE { get; set; }

        public string STATION_ADDRESS { get; set; }

        public string BYTE_ORDER { get; set; }

        public string BAUDRATE { get; set; }
    }
}
