using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisAgreeMent.Model.XCP
{
    public class XcpOnCan
    {
        ///XCP有效
        ///
        public string CurrentSelectItem { get; set; }
        public VehicleAppl VehicleApplData { get; set; }

        public VehicleApplRam VehicleApplRamData { get; set; }

        public CalibrationLe CalibrationLeData { get; set; }

        public CalibrationLeRam CalibrationLeRamData { get; set; }

        public class VehicleAppl
        {
            public string Version { get; set; }

            public string MasterID { get; set; }

            public string SlaveID { get; set; }

            public string Baudrate { get; set; }

            public string CanName { get; set; }
        }
        public class VehicleApplRam
        {
            public string Version { get; set; }

            public string MasterID { get; set; }

            public string SlaveID { get; set; }

            public string Baudrate { get; set; }

            public string CanName { get; set; }
        }
        public class CalibrationLe
        {
            public string Version { get; set; }

            public string MasterID { get; set; }

            public string SlaveID { get; set; }

            public string Baudrate { get; set; }

            public string CanName { get; set; }
        }
        public class CalibrationLeRam
        {
            public string Version { get; set; }

            public string MasterID { get; set; }

            public string SlaveID { get; set; }

            public string Baudrate { get; set; }

            public string CanName { get; set; }
        }
    }
}

