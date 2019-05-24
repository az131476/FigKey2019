using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CcpXcpEcu")]
	public class CcpXcpEcu
	{
		private enum CcpXcpEcuSettingType
		{
			[EnumMember(Value = "Undefined")]
			Undefined,
			[EnumMember(Value = "EcuDisplayName")]
			EcuDisplayName,
			[EnumMember(Value = "UseDbParams")]
			UseDbParams,
			[EnumMember(Value = "TransportLayerInstanceName")]
			TransportLayerInstanceName,
			[EnumMember(Value = "IsSeedAndKeyUsed")]
			IsSeedAndKeyUsed,
			[EnumMember(Value = "CanRequestID")]
			CanRequestID,
			[EnumMember(Value = "CanResponseID")]
			CanResponseID,
			[EnumMember(Value = "IsCanRequestIdExtended")]
			IsCanRequestIdExtended,
			[EnumMember(Value = "IsCanResponseIdExtended")]
			IsCanResponseIdExtended,
			[EnumMember(Value = "SendSetSessionStatus")]
			SendSetSessionStatus,
			[EnumMember(Value = "IsCanFirstIdExtended")]
			IsCanFirstIdExtended,
			[EnumMember(Value = "UseCcpVersion2_0")]
			UseCcpVersion2_0,
			[EnumMember(Value = "CanFirstID")]
			CanFirstID,
			[EnumMember(Value = "DaqTimeout")]
			DaqTimeout,
			[EnumMember(Value = "FlexRayEcuTxQueue")]
			FlexRayEcuTxQueue,
			[EnumMember(Value = "FlexRayXcpNodeAdress")]
			FlexRayXcpNodeAdress,
			[EnumMember(Value = "EthernetHost")]
			EthernetHost,
			[EnumMember(Value = "EthernetPort")]
			EthernetPort,
			[EnumMember(Value = "EthernetProtocol")]
			EthernetProtocol,
			[EnumMember(Value = "MaxCTO")]
			MaxCTO,
			[EnumMember(Value = "MaxDTO")]
			MaxDTO,
			[EnumMember(Value = "UseVxModule")]
			UseVxModule,
			[EnumMember(Value = "EthernetPort2")]
			EthernetPort2,
			[EnumMember(Value = "Eth1Ip")]
			Eth1Ip,
			[EnumMember(Value = "SeedAndKeyPath")]
			SeedAndKeyPath,
			[EnumMember(Value = "ExchangeIdHandling")]
			ExchangeIdHandling,
			[EnumMember(Value = "Eth1KeepAwake")]
			Eth1KeepAwake,
			[EnumMember(Value = "ExtendStaticDaqListToMaxSize")]
			ExtendStaticDaqListToMaxSize,
			[EnumMember(Value = "IsProtocolByteOrderIntel")]
			IsProtocolByteOrderIntel,
			[EnumMember(Value = "UseEcuTimestamp")]
			UseEcuTimestamp,
			[EnumMember(Value = "EcuTimestampWidth")]
			EcuTimestampWidth,
			[EnumMember(Value = "EcuTimestampUnit")]
			EcuTimestampUnit,
			[EnumMember(Value = "EcuTimestampTicks")]
			EcuTimestampTicks,
			[EnumMember(Value = "EcuTimestampCalculationMethod")]
			EcuTimestampCalculationMethod
		}

		[DataContract(Name = "CcpXcpEcuSetting")]
		private class CcpXcpEcuSetting
		{
			[DataMember(Name = "CcpXcpEcuSettingType")]
			public CcpXcpEcu.CcpXcpEcuSettingType Type
			{
				get;
				set;
			}

			[DataMember(Name = "CcpXcpEcuSettingValue")]
			public ValidatedProperty<string> Value
			{
				get;
				set;
			}

			public CcpXcpEcuSetting(CcpXcpEcu.CcpXcpEcuSettingType type, string value)
			{
				this.Type = type;
				this.Value = new ValidatedProperty<string>(value);
			}
		}

		public const string kDefaultTransportLayerInstanceNameForVx = "---Default---";

		public const string kDefaultIp = "192.168.165.1";

		public const uint kDefaultPort1 = 5555u;

		public const uint kDefaultPort2 = 5554u;

		[DataMember(Name = "CcpXcpEcuSettings")]
		private List<CcpXcpEcu.CcpXcpEcuSetting> settings;

		public ValidatedProperty<string> CcpXcpEcuDisplayNameValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.EcuDisplayName);
			}
		}

		public string CcpXcpEcuDisplayName
		{
			get
			{
				return this.CcpXcpEcuDisplayNameValidatedProperty.Value;
			}
			set
			{
				this.CcpXcpEcuDisplayNameValidatedProperty.Value = value;
			}
		}

		public ValidatedProperty<string> IsSeedAndKeyUsedValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.IsSeedAndKeyUsed);
			}
		}

		public bool IsSeedAndKeyUsed
		{
			get
			{
				return this.GetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.IsSeedAndKeyUsed);
			}
			set
			{
				this.SetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.IsSeedAndKeyUsed, value);
			}
		}

		public ValidatedProperty<string> UseDbParamsValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.UseDbParams);
			}
		}

		public bool UseDbParams
		{
			get
			{
				return this.GetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.UseDbParams);
			}
			set
			{
				this.SetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.UseDbParams, value);
			}
		}

		public ValidatedProperty<string> TransportLayerInstanceNameValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.TransportLayerInstanceName);
			}
		}

		public string TransportLayerInstanceName
		{
			get
			{
				return this.TransportLayerInstanceNameValidatedProperty.Value;
			}
			set
			{
				this.TransportLayerInstanceNameValidatedProperty.Value = value;
			}
		}

		public ValidatedProperty<string> CanRequestIDValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.CanRequestID);
			}
		}

		public uint CanRequestID
		{
			get
			{
				return this.GetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.CanRequestID);
			}
			set
			{
				this.SetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.CanRequestID, value);
			}
		}

		public ValidatedProperty<string> IsCanRequestIdExtendedValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.IsCanRequestIdExtended);
			}
		}

		public bool IsCanRequestIdExtended
		{
			get
			{
				return this.GetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.IsCanRequestIdExtended);
			}
			set
			{
				this.SetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.IsCanRequestIdExtended, value);
			}
		}

		public ValidatedProperty<string> CanResponseIDValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.CanResponseID);
			}
		}

		public uint CanResponseID
		{
			get
			{
				return this.GetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.CanResponseID);
			}
			set
			{
				this.SetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.CanResponseID, value);
			}
		}

		public ValidatedProperty<string> IsCanResponseIdExtendedValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.IsCanResponseIdExtended);
			}
		}

		public bool IsCanResponseIdExtended
		{
			get
			{
				return this.GetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.IsCanResponseIdExtended);
			}
			set
			{
				this.SetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.IsCanResponseIdExtended, value);
			}
		}

		public ValidatedProperty<string> CanFirstIDValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.CanFirstID);
			}
		}

		public uint CanFirstID
		{
			get
			{
				return this.GetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.CanFirstID);
			}
			set
			{
				this.SetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.CanFirstID, value);
			}
		}

		public ValidatedProperty<string> IsCanFirstIdExtendedValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.IsCanFirstIdExtended);
			}
		}

		public bool IsCanFirstIdExtended
		{
			get
			{
				return this.GetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.IsCanFirstIdExtended);
			}
			set
			{
				this.SetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.IsCanFirstIdExtended, value);
			}
		}

		public ValidatedProperty<string> DaqTimeoutValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.DaqTimeout);
			}
		}

		public uint DaqTimeout
		{
			get
			{
				return this.GetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.DaqTimeout);
			}
			set
			{
				this.SetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.DaqTimeout, value);
			}
		}

		public ValidatedProperty<string> SendSetSessionStatusValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.SendSetSessionStatus);
			}
		}

		public bool SendSetSessionStatus
		{
			get
			{
				return this.GetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.SendSetSessionStatus);
			}
			set
			{
				this.SetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.SendSetSessionStatus, value);
			}
		}

		public ValidatedProperty<string> UseCcpVersion2_0ValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.UseCcpVersion2_0);
			}
		}

		public bool UseCcpVersion2_0
		{
			get
			{
				return this.GetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.UseCcpVersion2_0);
			}
			set
			{
				this.SetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.UseCcpVersion2_0, value);
			}
		}

		public ValidatedProperty<string> FlexRayEcuTxQueueValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.FlexRayEcuTxQueue);
			}
		}

		public bool FlexRayEcuTxQueue
		{
			get
			{
				return this.GetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.FlexRayEcuTxQueue);
			}
			set
			{
				this.SetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.FlexRayEcuTxQueue, value);
			}
		}

		public ValidatedProperty<string> FlexRayXcpNodeAdressValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.FlexRayXcpNodeAdress);
			}
		}

		public uint FlexRayXcpNodeAdress
		{
			get
			{
				return this.GetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.FlexRayXcpNodeAdress);
			}
			set
			{
				this.SetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.FlexRayXcpNodeAdress, value);
			}
		}

		public ValidatedProperty<string> EthernetHostValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.EthernetHost);
			}
		}

		public string EthernetHost
		{
			get
			{
				return this.EthernetHostValidatedProperty.Value;
			}
			set
			{
				this.EthernetHostValidatedProperty.Value = value;
			}
		}

		public ValidatedProperty<string> EthernetPortValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.EthernetPort);
			}
		}

		public uint EthernetPort
		{
			get
			{
				return this.GetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.EthernetPort);
			}
			set
			{
				this.SetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.EthernetPort, value);
			}
		}

		public ValidatedProperty<string> EthernetProtocolValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.EthernetProtocol);
			}
		}

		public EthernetProtocol EthernetProtocol
		{
			get
			{
				return this.GetSettingValEthernetProtocol(CcpXcpEcu.CcpXcpEcuSettingType.EthernetProtocol);
			}
			set
			{
				this.SetSettingValEthernetProtocol(CcpXcpEcu.CcpXcpEcuSettingType.EthernetProtocol, value);
			}
		}

		public ValidatedProperty<string> MaxCTOValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.MaxCTO);
			}
		}

		public uint MaxCTO
		{
			get
			{
				return this.GetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.MaxCTO);
			}
			set
			{
				this.SetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.MaxCTO, value);
			}
		}

		public ValidatedProperty<string> MaxDTOValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.MaxDTO);
			}
		}

		public uint MaxDTO
		{
			get
			{
				return this.GetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.MaxDTO);
			}
			set
			{
				this.SetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.MaxDTO, value);
			}
		}

		public ValidatedProperty<string> UseVxModuleValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.UseVxModule);
			}
		}

		public bool UseVxModule
		{
			get
			{
				return this.GetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.UseVxModule);
			}
			set
			{
				this.SetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.UseVxModule, value);
			}
		}

		public ValidatedProperty<string> EthernetPort2ValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.EthernetPort2);
			}
		}

		public uint EthernetPort2
		{
			get
			{
				return this.GetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.EthernetPort2);
			}
			set
			{
				this.SetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.EthernetPort2, value);
			}
		}

		public ValidatedProperty<string> ExtendStaticDaqListToMaxSizeValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.ExtendStaticDaqListToMaxSize);
			}
		}

		public bool ExtendStaticDaqListToMaxSize
		{
			get
			{
				return this.GetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.ExtendStaticDaqListToMaxSize);
			}
			set
			{
				this.SetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.ExtendStaticDaqListToMaxSize, value);
			}
		}

		public ValidatedProperty<string> IsProtocolByteOrderIntelValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.IsProtocolByteOrderIntel);
			}
		}

		public bool IsProtocolByteOrderIntel
		{
			get
			{
				return this.GetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.IsProtocolByteOrderIntel);
			}
			set
			{
				this.SetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.IsProtocolByteOrderIntel, value);
			}
		}

		public ValidatedProperty<string> UseEcuTimestampValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.UseEcuTimestamp);
			}
		}

		public bool UseEcuTimestamp
		{
			get
			{
				return this.GetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.UseEcuTimestamp);
			}
			set
			{
				this.SetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType.UseEcuTimestamp, value);
			}
		}

		public ValidatedProperty<string> EcuTimestampWidthValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampWidth);
			}
		}

		public uint EcuTimestampWidth
		{
			get
			{
				return this.GetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampWidth);
			}
			set
			{
				this.SetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampWidth, value);
			}
		}

		public ValidatedProperty<string> EcuTimestampUnitValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampUnit);
			}
		}

		public CcpXcpEcuTimestampUnit EcuTimestampUnit
		{
			get
			{
				return this.GetSettingValEcuTimestampUnit(CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampUnit);
			}
			set
			{
				this.SetSettingValEcuTimestampUnit(CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampUnit, value);
			}
		}

		public ValidatedProperty<string> EcuTimestampTicksValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampTicks);
			}
		}

		public uint EcuTimestampTicks
		{
			get
			{
				return this.GetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampTicks);
			}
			set
			{
				this.SetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampTicks, value);
			}
		}

		public ValidatedProperty<string> EcuTimestampCalculationMethodValidatedProperty
		{
			get
			{
				return this.GetSetting(CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampCalculationMethod);
			}
		}

		public CcpXcpEcuTimestampCalculationMethod EcuTimestampCalculationMethod
		{
			get
			{
				return this.GetSettingValEcuTimestampCalculationMethod(CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampCalculationMethod);
			}
			set
			{
				this.SetSettingValEcuTimestampCalculationMethod(CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampCalculationMethod, value);
			}
		}

		public string GeneratedDbcOrFibexFile
		{
			get;
			set;
		}

		public string NetworkName
		{
			get;
			set;
		}

		public bool PreventDeconcatinationForFirstXcpOnUdpSlave
		{
			get;
			set;
		}

		public CcpXcpEcu()
		{
			this.settings = new List<CcpXcpEcu.CcpXcpEcuSetting>();
			this.GeneratedDbcOrFibexFile = string.Empty;
			this.NetworkName = string.Empty;
			this.PreventDeconcatinationForFirstXcpOnUdpSlave = false;
		}

		public CcpXcpEcu(CcpXcpEcu other)
		{
			this.settings = new List<CcpXcpEcu.CcpXcpEcuSetting>();
			foreach (CcpXcpEcu.CcpXcpEcuSetting current in other.settings)
			{
				this.settings.Add(new CcpXcpEcu.CcpXcpEcuSetting(current.Type, current.Value.Value));
			}
			this.GeneratedDbcOrFibexFile = other.GeneratedDbcOrFibexFile;
			this.NetworkName = other.NetworkName;
			this.PreventDeconcatinationForFirstXcpOnUdpSlave = other.PreventDeconcatinationForFirstXcpOnUdpSlave;
		}

		public static string DefaultTransportLayerInstanceNameForVx()
		{
			return "---Default---";
		}

		public static bool IsDefaultTransportLayerInstanceForVx(string tlin)
		{
			return string.Compare(tlin, "---Default---", StringComparison.InvariantCulture) == 0;
		}

		private ValidatedProperty<string> GetSetting(CcpXcpEcu.CcpXcpEcuSettingType settingType)
		{
			CcpXcpEcu.CcpXcpEcuSetting ccpXcpEcuSetting = this.settings.Find((CcpXcpEcu.CcpXcpEcuSetting s) => s.Type == settingType);
			if (ccpXcpEcuSetting != null)
			{
				return ccpXcpEcuSetting.Value;
			}
			string value = string.Empty;
			switch (settingType)
			{
			case CcpXcpEcu.CcpXcpEcuSettingType.EcuDisplayName:
				value = string.Empty;
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.UseDbParams:
			case CcpXcpEcu.CcpXcpEcuSettingType.IsProtocolByteOrderIntel:
				value = bool.TrueString;
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.TransportLayerInstanceName:
				value = string.Empty;
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.IsSeedAndKeyUsed:
			case CcpXcpEcu.CcpXcpEcuSettingType.ExtendStaticDaqListToMaxSize:
				value = bool.FalseString;
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.CanRequestID:
			case CcpXcpEcu.CcpXcpEcuSettingType.CanResponseID:
				value = "0";
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.IsCanRequestIdExtended:
			case CcpXcpEcu.CcpXcpEcuSettingType.IsCanResponseIdExtended:
			case CcpXcpEcu.CcpXcpEcuSettingType.SendSetSessionStatus:
				value = bool.FalseString;
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.IsCanFirstIdExtended:
			case CcpXcpEcu.CcpXcpEcuSettingType.UseCcpVersion2_0:
				value = bool.TrueString;
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.CanFirstID:
				value = "268435456";
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.DaqTimeout:
				value = Constants.DefaultDaqTimeout.ToString();
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.FlexRayEcuTxQueue:
				value = bool.FalseString;
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.FlexRayXcpNodeAdress:
				value = Constants.MinCcpXcpFlexRayNodeAdress.ToString();
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.EthernetHost:
				value = "192.168.165.1";
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.EthernetPort:
				value = 5555u.ToString();
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.EthernetProtocol:
				value = EthernetProtocol.UDP.ToString();
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.MaxCTO:
				value = Constants.MinCcpXcpCTO.ToString();
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.MaxDTO:
				value = Constants.MinCcpXcpDTO.ToString();
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.UseVxModule:
				value = bool.FalseString;
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.EthernetPort2:
				value = 5554u.ToString();
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.UseEcuTimestamp:
				value = string.Empty;
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampWidth:
				value = string.Empty;
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampUnit:
				value = string.Empty;
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampTicks:
				value = string.Empty;
				break;
			case CcpXcpEcu.CcpXcpEcuSettingType.EcuTimestampCalculationMethod:
				value = CcpXcpEcuTimestampCalculationMethod.Multiplication.ToString();
				break;
			}
			ccpXcpEcuSetting = new CcpXcpEcu.CcpXcpEcuSetting(settingType, value);
			this.settings.Add(ccpXcpEcuSetting);
			return ccpXcpEcuSetting.Value;
		}

		private bool GetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType settingType)
		{
			return string.Compare(this.GetSetting(settingType).Value, bool.TrueString, true) == 0;
		}

		private void SetSettingValBool(CcpXcpEcu.CcpXcpEcuSettingType settingType, bool value)
		{
			this.GetSetting(settingType).Value = value.ToString();
		}

		private uint GetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType settingType)
		{
			uint result;
			try
			{
				result = Convert.ToUInt32(this.GetSetting(settingType).Value);
			}
			catch (Exception)
			{
				result = 0u;
			}
			return result;
		}

		private void SetSettingValUint(CcpXcpEcu.CcpXcpEcuSettingType settingType, uint value)
		{
			this.GetSetting(settingType).Value = value.ToString(CultureInfo.InvariantCulture);
		}

		private EthernetProtocol GetSettingValEthernetProtocol(CcpXcpEcu.CcpXcpEcuSettingType settingType)
		{
			EthernetProtocol result;
			try
			{
				result = (EthernetProtocol)Enum.Parse(typeof(EthernetProtocol), this.GetSetting(settingType).Value, true);
			}
			catch (Exception)
			{
				result = EthernetProtocol.Unspecified;
			}
			return result;
		}

		private void SetSettingValEthernetProtocol(CcpXcpEcu.CcpXcpEcuSettingType settingType, EthernetProtocol value)
		{
			this.GetSetting(settingType).Value = value.ToString();
		}

		private CcpXcpEcuTimestampUnit GetSettingValEcuTimestampUnit(CcpXcpEcu.CcpXcpEcuSettingType settingsType)
		{
			CcpXcpEcuTimestampUnit result;
			try
			{
				result = (CcpXcpEcuTimestampUnit)Enum.Parse(typeof(CcpXcpEcuTimestampUnit), this.GetSetting(settingsType).Value, true);
			}
			catch
			{
				result = CcpXcpEcuTimestampUnit.TU_Unspecified;
			}
			return result;
		}

		private void SetSettingValEcuTimestampUnit(CcpXcpEcu.CcpXcpEcuSettingType settingsType, CcpXcpEcuTimestampUnit value)
		{
			this.GetSetting(settingsType).Value = value.ToString();
		}

		private CcpXcpEcuTimestampCalculationMethod GetSettingValEcuTimestampCalculationMethod(CcpXcpEcu.CcpXcpEcuSettingType settingType)
		{
			CcpXcpEcuTimestampCalculationMethod result;
			try
			{
				result = (CcpXcpEcuTimestampCalculationMethod)Enum.Parse(typeof(CcpXcpEcuTimestampCalculationMethod), this.GetSetting(settingType).Value, true);
			}
			catch
			{
				result = CcpXcpEcuTimestampCalculationMethod.Unspecified;
			}
			return result;
		}

		private void SetSettingValEcuTimestampCalculationMethod(CcpXcpEcu.CcpXcpEcuSettingType settingType, CcpXcpEcuTimestampCalculationMethod value)
		{
			this.GetSetting(settingType).Value = value.ToString();
		}
	}
}
