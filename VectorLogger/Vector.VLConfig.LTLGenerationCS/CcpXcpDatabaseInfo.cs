using System;
using System.Collections.Generic;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS
{
	public class CcpXcpDatabaseInfo
	{
		public List<CcpXcpEcuInfo> EcuInfoList
		{
			get;
			private set;
		}

		public string DestinationFolderPath
		{
			get;
			private set;
		}

		public uint ChannelNumber
		{
			get;
			set;
		}

		public string DestinationFilePath
		{
			get;
			set;
		}

		public BusType BusType
		{
			get;
			set;
		}

		public uint FlexRayXcpChannel
		{
			get;
			set;
		}

		public string FlexRayNetworkName
		{
			get;
			set;
		}

		public string ErrorText
		{
			get;
			private set;
		}

		public CcpXcpEcuInfo ErrorEcuInfo
		{
			get;
			private set;
		}

		public Database FlexRayDatabase
		{
			get;
			set;
		}

		public CcpXcpIncludeFileInfo IncludeFileInfo
		{
			get;
			set;
		}

		public CcpXcpDatabaseInfo(Database database, string destinationFolderPath)
		{
			this.EcuInfoList = new List<CcpXcpEcuInfo>();
			this.DestinationFolderPath = destinationFolderPath;
			this.ErrorText = string.Empty;
			this.ErrorEcuInfo = null;
			this.DestinationFilePath = string.Empty;
			this.BusType = database.BusType.Value;
			this.ChannelNumber = database.ChannelNumber.Value;
			this.FlexRayXcpChannel = 0u;
			this.FlexRayNetworkName = string.Empty;
			this.FlexRayDatabase = null;
		}

		public void SetErrorText(string aErrorText, CcpXcpEcuInfo aErrorEcuInfo)
		{
			this.ErrorText = aErrorText;
			this.ErrorEcuInfo = aErrorEcuInfo;
		}
	}
}
