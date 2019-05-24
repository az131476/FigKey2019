using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "Database")]
	public class Database : IFile, ITransmitMessageChannel
	{
		public static readonly uint ChannelNumber_FlexrayAB = 256u;

		private DatabaseFileType mDatabaseFileType;

		[DataMember(Name = "DatabaseCpProtectionList")]
		private List<CPProtection> cpProtectionList;

		private ValidatedProperty<string> seedAndKeyPath;

		private ValidatedProperty<string> cpEcuDisplayName;

		private ValidatedProperty<string> cpIsSeedAndKeyUsedValidatedProperty;

		private ValidatedProperty<string> cpUseDbParamsValidatedProperty;

		[DataMember(Name = "DatabaseFilePath")]
		public ValidatedProperty<string> FilePath
		{
			get;
			set;
		}

		[DataMember(Name = "DatabaseNetworkName")]
		public ValidatedProperty<string> NetworkName
		{
			get;
			set;
		}

		[DataMember(Name = "DatabaseBusType")]
		public ValidatedProperty<BusType> BusType
		{
			get;
			set;
		}

		[DataMember(Name = "DatabaseChannelNumber")]
		public ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		[DataMember(Name = "DatabaseCPType")]
		public ValidatedProperty<CPType> CPType
		{
			get;
			set;
		}

		[DataMember(Name = "DatabaseIsCPActive")]
		public ValidatedProperty<bool> IsCPActive
		{
			get;
			set;
		}

		[DataMember(Name = "DatabaseCcpXcpEcuList")]
		private List<CcpXcpEcu> ccpXcpEcuList
		{
			get;
			set;
		}

		[XmlIgnore]
		public ReadOnlyCollection<CcpXcpEcu> CcpXcpEcuList
		{
			get
			{
				return new ReadOnlyCollection<CcpXcpEcu>(this.ccpXcpEcuList);
			}
		}

		[XmlIgnore]
		public ReadOnlyCollection<CPProtection> CpProtections
		{
			get
			{
				return new ReadOnlyCollection<CPProtection>(this.cpProtectionList);
			}
		}

		[XmlIgnore]
		public ReadOnlyCollection<CPProtection> CpProtectionsWithSeedAndKeyRequired
		{
			get
			{
				return new ReadOnlyCollection<CPProtection>((from CPProtection prot in this.cpProtectionList
				where prot.HasSeedAndKey.Value
				select prot).ToList<CPProtection>());
			}
		}

		public ValidatedProperty<string> SeedAndKeyPath
		{
			get
			{
				if (this.seedAndKeyPath == null)
				{
					this.seedAndKeyPath = new ValidatedProperty<string>(string.Empty);
				}
				if (this.CpProtectionsWithSeedAndKeyRequired.Count > 0 && this.CpProtectionsWithSeedAndKeyRequired[0].SeedAndKeyFilePath != null)
				{
					this.seedAndKeyPath.Value = this.CpProtectionsWithSeedAndKeyRequired[0].SeedAndKeyFilePath.Value;
				}
				else if (this.FileType == DatabaseFileType.A2L && this.CpProtections.Count > 0 && this.CpProtections[0].SeedAndKeyFilePath != null)
				{
					this.seedAndKeyPath.Value = this.CpProtections[0].SeedAndKeyFilePath.Value;
				}
				return this.seedAndKeyPath;
			}
		}

		public uint ExtraCPChannel
		{
			get;
			set;
		}

		public ValidatedProperty<string> CcpXcpEcuDisplayName
		{
			get
			{
				if (this.FileType == DatabaseFileType.A2L)
				{
					return this.CcpXcpEcuList[0].CcpXcpEcuDisplayNameValidatedProperty;
				}
				return this.cpEcuDisplayName;
			}
		}

		public ValidatedProperty<string> CcpXcpIsSeedAndKeyUsedValidatedProperty
		{
			get
			{
				if (this.FileType == DatabaseFileType.A2L)
				{
					return this.CcpXcpEcuList[0].IsSeedAndKeyUsedValidatedProperty;
				}
				return this.cpIsSeedAndKeyUsedValidatedProperty;
			}
		}

		public bool CcpXcpIsSeedAndKeyUsed
		{
			get
			{
				if (this.FileType == DatabaseFileType.A2L)
				{
					return this.CcpXcpEcuList[0].IsSeedAndKeyUsed;
				}
				return this.cpIsSeedAndKeyUsedValidatedProperty.Value == bool.TrueString;
			}
			set
			{
				if (this.FileType == DatabaseFileType.A2L)
				{
					this.CcpXcpEcuList[0].IsSeedAndKeyUsed = value;
					return;
				}
				this.cpIsSeedAndKeyUsedValidatedProperty.Value = value.ToString();
			}
		}

		public ValidatedProperty<string> CcpXcpUseDbParamsValidatedProperty
		{
			get
			{
				if (this.FileType == DatabaseFileType.A2L)
				{
					return this.CcpXcpEcuList[0].UseDbParamsValidatedProperty;
				}
				return this.cpUseDbParamsValidatedProperty;
			}
		}

		public bool CcpXcpUseDbParams
		{
			get
			{
				if (this.FileType == DatabaseFileType.A2L)
				{
					return this.CcpXcpEcuList[0].UseDbParams;
				}
				return this.cpUseDbParamsValidatedProperty.Value == bool.TrueString;
			}
			set
			{
				if (this.FileType == DatabaseFileType.A2L)
				{
					this.CcpXcpEcuList[0].UseDbParams = value;
					return;
				}
				this.cpUseDbParamsValidatedProperty.Value = value.ToString();
			}
		}

		public bool IsBusDatabase
		{
			get
			{
				return this.CPType.Value == Vector.VLConfig.Data.ConfigurationDataModel.CPType.None && this.BusType.Value != Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_None;
			}
		}

		public bool IsAUTOSARFile
		{
			get;
			set;
		}

		public bool IsFileNotFound
		{
			get;
			set;
		}

		public bool IsInconsistent
		{
			get;
			set;
		}

		public DatabaseFileType FileType
		{
			get
			{
				if (this.mDatabaseFileType == DatabaseFileType.Unspecified)
				{
					return this.mDatabaseFileType = this.DetermineDatabaseFileType();
				}
				return this.mDatabaseFileType;
			}
		}

		public string CcpXcpSlaveNamePrefix
		{
			get;
			set;
		}

		public string AliasName
		{
			get
			{
				if (string.IsNullOrEmpty(this.NetworkName.Value))
				{
					return Path.GetFileNameWithoutExtension(this.FilePath.Value);
				}
				return this.NetworkName.Value;
			}
		}

		public Database()
		{
			this.FilePath = new ValidatedProperty<string>(string.Empty);
			this.NetworkName = new ValidatedProperty<string>(string.Empty);
			this.BusType = new ValidatedProperty<BusType>(Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_Wildcard);
			this.ChannelNumber = new ValidatedProperty<uint>(1u);
			this.CPType = new ValidatedProperty<CPType>(Vector.VLConfig.Data.ConfigurationDataModel.CPType.None);
			this.IsCPActive = new ValidatedProperty<bool>(false);
			this.cpProtectionList = new List<CPProtection>();
			this.ExtraCPChannel = 0u;
			this.IsAUTOSARFile = false;
			this.IsFileNotFound = false;
			this.IsInconsistent = false;
			this.mDatabaseFileType = DatabaseFileType.Unspecified;
			this.CcpXcpSlaveNamePrefix = string.Empty;
			this.ccpXcpEcuList = new List<CcpXcpEcu>();
			this.InitializeValidatedRuntimeProperties();
		}

		public Database(Database other)
		{
			this.FilePath = new ValidatedProperty<string>(other.FilePath.Value);
			this.NetworkName = new ValidatedProperty<string>(other.NetworkName.Value);
			this.BusType = new ValidatedProperty<BusType>(other.BusType.Value);
			this.ChannelNumber = new ValidatedProperty<uint>(other.ChannelNumber.Value);
			this.CPType = new ValidatedProperty<CPType>(other.CPType.Value);
			this.IsCPActive = new ValidatedProperty<bool>(other.IsCPActive.Value);
			this.cpProtectionList = new List<CPProtection>();
			foreach (CPProtection current in other.cpProtectionList)
			{
				this.AddCpProtection(new CPProtection(current));
			}
			this.ExtraCPChannel = other.ExtraCPChannel;
			this.IsAUTOSARFile = other.IsAUTOSARFile;
			this.IsFileNotFound = other.IsFileNotFound;
			this.IsInconsistent = other.IsInconsistent;
			this.mDatabaseFileType = other.FileType;
			this.CcpXcpSlaveNamePrefix = other.CcpXcpSlaveNamePrefix;
			this.ccpXcpEcuList = new List<CcpXcpEcu>();
			foreach (CcpXcpEcu current2 in other.ccpXcpEcuList)
			{
				this.ccpXcpEcuList.Add(new CcpXcpEcu(current2));
			}
			this.InitializeValidatedRuntimeProperties();
			this.cpEcuDisplayName.Value = other.cpEcuDisplayName.Value;
			this.cpIsSeedAndKeyUsedValidatedProperty.Value = other.cpIsSeedAndKeyUsedValidatedProperty.Value;
			this.cpUseDbParamsValidatedProperty.Value = other.cpUseDbParamsValidatedProperty.Value;
		}

		public void InitializeValidatedRuntimeProperties()
		{
			this.cpEcuDisplayName = new ValidatedProperty<string>();
			this.cpIsSeedAndKeyUsedValidatedProperty = new ValidatedProperty<string>();
			this.cpUseDbParamsValidatedProperty = new ValidatedProperty<string>(bool.TrueString);
		}

		public void AddCpProtection(CPProtection prot)
		{
			this.cpProtectionList.Add(prot);
		}

		public void ClearCpProtections()
		{
			this.cpProtectionList.Clear();
		}

		public void AddCcpXcpEcu(CcpXcpEcu ecu)
		{
			this.ccpXcpEcuList.Add(ecu);
		}

		public void SetCcpXcpProtocol(CcpXcpProtocol xcpProtocol, string transportLayerInstanceName, bool useVxModule)
		{
			this.CPType.Value = Vector.VLConfig.Data.ConfigurationDataModel.CPType.XCP;
			this.CcpXcpEcuList[0].TransportLayerInstanceName = transportLayerInstanceName;
			this.CcpXcpEcuList[0].UseVxModule = useVxModule;
			if (useVxModule)
			{
				this.BusType.Value = Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_Ethernet;
				this.CcpXcpEcuList[0].EthernetProtocol = EthernetProtocol.UDP;
				return;
			}
			switch (xcpProtocol)
			{
			case CcpXcpProtocol.CcpXcpPr_CAN:
				this.BusType.Value = Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN;
				return;
			case CcpXcpProtocol.CcpXcpPr_FlexRay:
				this.BusType.Value = Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay;
				return;
			case CcpXcpProtocol.CcpXcpPr_UDP:
				this.BusType.Value = Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_Ethernet;
				this.CcpXcpEcuList[0].EthernetProtocol = EthernetProtocol.UDP;
				return;
			case CcpXcpProtocol.CcpXcpPr_TCP:
				this.BusType.Value = Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_Ethernet;
				this.CcpXcpEcuList[0].EthernetProtocol = EthernetProtocol.TCP;
				return;
			case CcpXcpProtocol.CcpXcpPr_CCP:
				this.BusType.Value = Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN;
				this.CPType.Value = Vector.VLConfig.Data.ConfigurationDataModel.CPType.CCP;
				return;
			default:
				return;
			}
		}

		public void UpdateSeedAndKeyUsedStatus(bool hasSeedAndKey)
		{
			if (hasSeedAndKey)
			{
				if (this.cpProtectionList.Count > 0)
				{
					this.cpProtectionList[0].HasSeedAndKey.Value = true;
					return;
				}
			}
			else
			{
				foreach (CPProtection current in this.CpProtectionsWithSeedAndKeyRequired)
				{
					current.HasSeedAndKey.Value = false;
				}
			}
		}

		public static string MakeCpEcuDisplayName(ICollection<string> names)
		{
			string text = string.Empty;
			foreach (string current in names)
			{
				text += current;
				if (current != names.Last<string>())
				{
					text += ", ";
				}
			}
			return text;
		}

		private DatabaseFileType DetermineDatabaseFileType()
		{
			string extension = Path.GetExtension(this.FilePath.Value);
			if (!string.IsNullOrEmpty(extension))
			{
				string strA = extension.Trim(new char[]
				{
					'.'
				});
				if (string.Compare(strA, "a2l", true, CultureInfo.InvariantCulture) == 0)
				{
					return DatabaseFileType.A2L;
				}
				if (string.Compare(strA, "dbc", true, CultureInfo.InvariantCulture) == 0)
				{
					return DatabaseFileType.DBC;
				}
				if (string.Compare(strA, "xml", true, CultureInfo.InvariantCulture) == 0)
				{
					return DatabaseFileType.XML;
				}
			}
			return DatabaseFileType.Unspecified;
		}
	}
}
