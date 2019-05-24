using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FigKeySerialPort.Controls;

namespace FigKeySerialPort.Model
{
    class SentConfig
    {
        public LocalConfig.Storage_Data_Type StorageDataType { get; set; }

        public enum QuickDataOrder
        {
            CHECKED,
            UNCHECKED
        }

        public class BaseSigConfig
        {
            public string DataType { get; set; }

            public string BatteryState { get; set; }

            public string SerialMsg { get; set; }

            public string TimeLong { get; set; }
        }
        public class SlowSigConfig
        {
            //十六进制或十进制数据
            public string GroupCount { get; set; }

            public string GroupOrder { get; set; }

            public string GroupSerialID { get; set; }

            public string GroupData { get; set; }

        }

        public class QuickSigConfig
        {
            public string QuickSigType { get; set; }

            public string QuickSigData1 { get; set; }

            public string QuickSigData2 { get; set; }

            public QuickDataOrder QuickDataCheck { get; set; }
        }
    }
}
