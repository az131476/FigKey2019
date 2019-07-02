using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisAgreeMent.Model.XCP
{
    struct IF_DATA_ASAP1B_CCP
    {
        public string DAQ_NAME { get; set; }

        public string CAN_ID_FIXED { get; set; }

        public string FIRST_PID { get; set; }

        public string Raster { get; set; }

        public struct TP_BLOB
        {
            public string CCP_VERSION { get; set; }

            public string BLOB_VERSION { get; set; }

            public string CAN_MSG_ID_SEND { get; set; }

            public string CAN_MSG_ID_RECE { get; set; }

            public string STATION_ADDRESS { get; set; }

            public string BYTE_ORDER { get; set; }
        }
    }
}
