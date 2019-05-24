using System;
using System.Collections.Generic;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	internal class GiNToolErrorCode
	{
		private Dictionary<int, string> ErrorString = new Dictionary<int, string>();

		public GiNToolErrorCode()
		{
			this.SetGinErrorCodeDictionary();
		}

		private void SetGinErrorCodeDictionary()
		{
			this.ErrorString[0] = "";
			this.ErrorString[1] = Resources.GiN_EC_NoRequest;
			this.ErrorString[20] = Resources.GiN_EC_Memory;
			this.ErrorString[21] = Resources.GiN_EC_MissingDLL;
			this.ErrorString[22] = Resources.GiN_EC_Phys;
			this.ErrorString[23] = Resources.GiN_EC_MissingReg;
			this.ErrorString[24] = Resources.GiN_EC_USBeject;
			this.ErrorString[30] = Resources.GiN_EC_Arg;
			this.ErrorString[31] = Resources.GiN_EC_FilFind;
			this.ErrorString[32] = Resources.GiN_EC_FilForm;
			this.ErrorString[33] = Resources.GiN_EC_FilVer;
			this.ErrorString[34] = Resources.GiN_EC_FilWrite;
			this.ErrorString[35] = Resources.GiN_EC_RealTime;
			this.ErrorString[40] = Resources.GiN_EC_NoConn;
			this.ErrorString[41] = Resources.GiN_EC_Comm;
			this.ErrorString[42] = Resources.GiN_EC_Timeout;
			this.ErrorString[50] = Resources.GiN_EC_Intern;
			this.ErrorString[51] = Resources.GiN_EC_IllDev;
			this.ErrorString[52] = Resources.GiN_EC_DevSW;
			this.ErrorString[53] = Resources.GiN_EC_DevVer;
			this.ErrorString[54] = Resources.GiN_EC_NoData;
			this.ErrorString[55] = Resources.GiN_EC_Conf;
			this.ErrorString[56] = Resources.GiN_EC_Compile;
		}

		public string GetErrorString(int errorCode)
		{
			if (!this.ErrorString.ContainsKey(errorCode))
			{
				return Resources.GiN_EC_UnknownError;
			}
			string result;
			try
			{
				result = this.ErrorString[errorCode];
			}
			catch
			{
				result = Resources.GiN_EC_UnknownError;
			}
			return result;
		}

		public string GetErrorString(int errorCode, string errorText)
		{
			if (errorText.Contains("is too large to fit into the device."))
			{
				return Resources.GIN_EC_File2Large4UserData;
			}
			return this.GetErrorString(errorCode);
		}
	}
}
