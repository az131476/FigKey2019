using DevExpress.Data;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Vector.McModule;
using Vector.UtilityFunctions.XtraGrid;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.CcpXcpDescriptionsPage
{
	public class ProtocolSettingsDialog : Form
	{
		internal interface ISetting
		{
			string Name
			{
				get;
			}

			string Value
			{
				get;
				set;
			}

			ProtocolSettingsDialog.SettingsGroup Group
			{
				get;
			}

			bool IsGlobalSetting
			{
				get;
			}

			bool HasDbParamOrDefaultValue
			{
				get;
			}

			bool IsValueReadOnly
			{
				get;
			}

			ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			IValidatedProperty ValidatedProperty
			{
				get;
			}

			ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get;
			}

			List<string> RepositoryItems
			{
				get;
			}

			void OnRepositoryItemButtonClick();

			string ApplyDbParamOrDefaultValue();

			string GetStoredValueFromConfig();

			void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged);
		}

		internal enum SettingsGroup
		{
			Basic,
			Advanced
		}

		internal enum RepositoryItemType
		{
			TextEdit,
			ComboBox,
			ButtonEdit
		}

		private class CcpVersion : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Resources_CcpXcp.CcpXcpEcuSettingCcpVersion;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Advanced;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].UseCcpVersion2_0ValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.ComboBox;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return new List<string>
					{
						"2.0",
						"2.1"
					};
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				ICcpDeviceConfig ccpDeviceConfig = this.SettingsDialog.DeviceConfig as ICcpDeviceConfig;
				if (ccpDeviceConfig == null)
				{
					return null;
				}
				string b = ccpDeviceConfig.MajorVersion + "." + ccpDeviceConfig.MinorVersion;
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].UseCcpVersion2_0 = (this.GetCcpVersionString(true) == b);
				return this.GetCcpVersionString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].UseCcpVersion2_0);
			}

			public string GetStoredValueFromConfig()
			{
				return this.GetCcpVersionString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].UseCcpVersion2_0);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				bool flag = false;
				if (this.Value == this.GetCcpVersionString(true))
				{
					flag = true;
				}
				bool flag2;
				result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(flag.ToString(CultureInfo.InvariantCulture), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].UseCcpVersion2_0ValidatedProperty, sourceGUIElement, out flag2);
				valueChanged |= flag2;
			}

			private string GetCcpVersionString(bool useCcpVersion2_0)
			{
				if (useCcpVersion2_0)
				{
					return "2.0";
				}
				return "2.1";
			}
		}

		private class RequestId : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return string.Format(Vocabulary.CcpXcpEcuSettingRequestId, this.SettingsDialog.ModeStr);
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Basic;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanRequestIDValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.TextEdit;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return null;
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				IXcpDeviceConfig xcpDeviceConfig = this.SettingsDialog.DeviceConfig as IXcpDeviceConfig;
				IXcpCanConfig xcpCanConfig = null;
				if (xcpDeviceConfig != null)
				{
					xcpCanConfig = (xcpDeviceConfig.TransportConfig as IXcpCanConfig);
				}
				uint num;
				if (xcpCanConfig != null)
				{
					num = xcpCanConfig.IdMaster;
				}
				else
				{
					ICcpDeviceConfig ccpDeviceConfig = this.SettingsDialog.DeviceConfig as ICcpDeviceConfig;
					if (ccpDeviceConfig == null)
					{
						return string.Empty;
					}
					num = ccpDeviceConfig.CanIdMaster;
				}
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsCanRequestIdExtended = ((num & Constants.CANDbIsExtendedIdMask) == Constants.CANDbIsExtendedIdMask);
				uint num2 = num & ~Constants.CANDbIsExtendedIdMask;
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanRequestID = num2;
				return GUIUtil.CANIdToDisplayString(num2, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsCanRequestIdExtended);
			}

			public string GetStoredValueFromConfig()
			{
				return GUIUtil.CANIdToDisplayString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanRequestID, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsCanRequestIdExtended);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				uint num;
				bool flag;
				if (GUIUtil.DisplayStringToCANId(this.Value, out num, out flag))
				{
					bool flag2;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(num.ToString(CultureInfo.InvariantCulture), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanRequestIDValidatedProperty, sourceGUIElement, out flag2);
					valueChanged |= flag2;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(flag.ToString(CultureInfo.InvariantCulture), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsCanRequestIdExtendedValidatedProperty, sourceGUIElement, out flag2);
					valueChanged |= flag2;
					return;
				}
				this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanRequestIDValidatedProperty, Resources.ErrorCANIdExpected);
				result = false;
			}
		}

		private class ResponseId : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return string.Format(Vocabulary.CcpXcpEcuSettingResponseId, this.SettingsDialog.ModeStr);
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Basic;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanResponseIDValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.TextEdit;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return null;
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				IXcpDeviceConfig xcpDeviceConfig = this.SettingsDialog.DeviceConfig as IXcpDeviceConfig;
				IXcpCanConfig xcpCanConfig = null;
				if (xcpDeviceConfig != null)
				{
					xcpCanConfig = (xcpDeviceConfig.TransportConfig as IXcpCanConfig);
				}
				uint num;
				if (xcpCanConfig != null)
				{
					num = xcpCanConfig.IdSlave;
				}
				else
				{
					ICcpDeviceConfig ccpDeviceConfig = this.SettingsDialog.DeviceConfig as ICcpDeviceConfig;
					if (ccpDeviceConfig == null)
					{
						return string.Empty;
					}
					num = ccpDeviceConfig.CanIdSlave;
				}
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsCanResponseIdExtended = ((num & Constants.CANDbIsExtendedIdMask) == Constants.CANDbIsExtendedIdMask);
				uint num2 = num & ~Constants.CANDbIsExtendedIdMask;
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanResponseID = num2;
				return GUIUtil.CANIdToDisplayString(num2, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsCanResponseIdExtended);
			}

			public string GetStoredValueFromConfig()
			{
				return GUIUtil.CANIdToDisplayString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanResponseID, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsCanResponseIdExtended);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				uint num;
				bool flag;
				if (GUIUtil.DisplayStringToCANId(this.Value, out num, out flag))
				{
					bool flag2;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(num.ToString(CultureInfo.InvariantCulture), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanResponseIDValidatedProperty, sourceGUIElement, out flag2);
					valueChanged |= flag2;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(flag.ToString(CultureInfo.InvariantCulture), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsCanResponseIdExtendedValidatedProperty, sourceGUIElement, out flag2);
					valueChanged |= flag2;
					return;
				}
				this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanResponseIDValidatedProperty, Resources.ErrorCANIdExpected);
				result = false;
			}
		}

		private class FirstId : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return string.Format(Vocabulary.CcpXcpEcuSettingFirstId, this.SettingsDialog.ModeStr);
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Basic;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanFirstIDValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.TextEdit;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return null;
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				uint canFirstID = 0u;
				if (CcpXcpManager.Instance().GetCanFirstID(this.SettingsDialog.DatabaseWorkingCopy, out canFirstID, true))
				{
					this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsCanFirstIdExtended = true;
					this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanFirstID = canFirstID;
				}
				return GUIUtil.CANIdToDisplayString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanFirstID, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsCanFirstIdExtended);
			}

			public string GetStoredValueFromConfig()
			{
				return GUIUtil.CANIdToDisplayString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanFirstID, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsCanFirstIdExtended);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				uint num;
				bool flag;
				if (GUIUtil.DisplayStringToCANId(this.Value, out num, out flag))
				{
					bool flag2;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(num.ToString(CultureInfo.InvariantCulture), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanFirstIDValidatedProperty, sourceGUIElement, out flag2);
					valueChanged |= flag2;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(flag.ToString(CultureInfo.InvariantCulture), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsCanFirstIdExtendedValidatedProperty, sourceGUIElement, out flag2);
					valueChanged |= flag2;
					return;
				}
				this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].CanFirstIDValidatedProperty, Resources.ErrorCANIdExpected);
				result = false;
			}
		}

		private class Host : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					if (!this.SettingsDialog.UsingVxModule())
					{
						return Vocabulary.CcpXcpEcuSettingHost;
					}
					return Resources_CcpXcp.CcpXcpEcuSettingVxIp;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Basic;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetHostValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.TextEdit;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return null;
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				IXcpDeviceConfig xcpDeviceConfig = this.SettingsDialog.DeviceConfig as IXcpDeviceConfig;
				if (xcpDeviceConfig == null)
				{
					return string.Empty;
				}
				IXcpIpConfig xcpIpConfig = xcpDeviceConfig.TransportConfig as IXcpIpConfig;
				if (xcpIpConfig == null)
				{
					return string.Empty;
				}
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetHost = xcpIpConfig.Host;
				return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetHost;
			}

			public string GetStoredValueFromConfig()
			{
				return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetHost;
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				if (ProtocolSettingsDialog.Host.ValidateIpAdress(this.Value))
				{
					bool flag;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(this.Value, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetHostValidatedProperty, sourceGUIElement, out flag);
					valueChanged |= flag;
					return;
				}
				this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetHostValidatedProperty, Resources.ErrorInvalidIpAdress);
				result = false;
			}

			private static bool ValidateIpAdress(string aValue)
			{
				IPAddress iPAddress;
				return IPAddress.TryParse(aValue, out iPAddress);
			}
		}

		private class FlexRayEcuTxQueue : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Resources_CcpXcp.CcpXcpEcuSettingFlexRayEcuTxQueue;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Advanced;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].FlexRayEcuTxQueueValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.ComboBox;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return new List<string>
					{
						Resources.Yes,
						Resources.No
					};
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				IXcpDeviceConfig xcpDeviceConfig = this.SettingsDialog.DeviceConfig as IXcpDeviceConfig;
				if (xcpDeviceConfig == null)
				{
					return string.Empty;
				}
				IXcpFlexRayConfig xcpFlexRayConfig = xcpDeviceConfig.TransportConfig as IXcpFlexRayConfig;
				if (xcpFlexRayConfig == null)
				{
					return string.Empty;
				}
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].FlexRayEcuTxQueue = xcpFlexRayConfig.DtoBuffering;
				return GUIUtil.ConvertBool2YesNoString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].FlexRayEcuTxQueue);
			}

			public string GetStoredValueFromConfig()
			{
				return GUIUtil.ConvertBool2YesNoString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].FlexRayEcuTxQueue);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				bool flag = false;
				if (GUIUtil.ConvertYesNoString2Bool(this.Value, ref flag))
				{
					bool flag2;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(flag.ToString(), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].FlexRayEcuTxQueueValidatedProperty, sourceGUIElement, out flag2);
					valueChanged |= flag2;
				}
			}
		}

		private class MaxCTO : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Vocabulary.CcpXcpEcuSettingMaxCTO;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Advanced;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].MaxCTOValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.TextEdit;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return null;
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				IXcpDeviceConfig xcpDeviceConfig = this.SettingsDialog.DeviceConfig as IXcpDeviceConfig;
				if (xcpDeviceConfig == null)
				{
					return string.Empty;
				}
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].MaxCTO = xcpDeviceConfig.ProtocolConfig.MaxCto;
				return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].MaxCTO.ToString(CultureInfo.InvariantCulture);
			}

			public string GetStoredValueFromConfig()
			{
				return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].MaxCTO.ToString(CultureInfo.InvariantCulture);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				uint num;
				if (uint.TryParse(this.Value, out num))
				{
					bool flag;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(num.ToString(CultureInfo.InvariantCulture), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].MaxCTOValidatedProperty, sourceGUIElement, out flag);
					valueChanged |= flag;
					return;
				}
				this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].MaxCTOValidatedProperty, Resources.InvalidValueInteger);
				result = false;
			}
		}

		private class MaxDTO : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Vocabulary.CcpXcpEcuSettingMaxDTO;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Advanced;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].MaxDTOValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.TextEdit;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return null;
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				IXcpDeviceConfig xcpDeviceConfig = this.SettingsDialog.DeviceConfig as IXcpDeviceConfig;
				if (xcpDeviceConfig == null)
				{
					return string.Empty;
				}
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].MaxDTO = xcpDeviceConfig.ProtocolConfig.MaxDto;
				return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].MaxDTO.ToString(CultureInfo.InvariantCulture);
			}

			public string GetStoredValueFromConfig()
			{
				return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].MaxDTO.ToString(CultureInfo.InvariantCulture);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				uint num;
				if (uint.TryParse(this.Value, out num))
				{
					bool flag;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(num.ToString(CultureInfo.InvariantCulture), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].MaxDTOValidatedProperty, sourceGUIElement, out flag);
					valueChanged |= flag;
					return;
				}
				this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].MaxDTOValidatedProperty, Resources.InvalidValueInteger);
				result = false;
			}
		}

		private class NodeAddress : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return string.Format(Resources_CcpXcp.CcpXcpEcuSettingNodeAdress, this.SettingsDialog.ModeStr);
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Basic;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].FlexRayXcpNodeAdressValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.TextEdit;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return null;
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].FlexRayXcpNodeAdress = CcpXcpManager.GetNodeAddressDbParamOrDefaultValue(this.SettingsDialog.DeviceConfig);
				return GUIUtil.NumberToDisplayString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].FlexRayXcpNodeAdress);
			}

			public string GetStoredValueFromConfig()
			{
				return GUIUtil.NumberToDisplayString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].FlexRayXcpNodeAdress);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				uint num;
				if (!GUIUtil.DisplayStringToNumber(this.Value, out num))
				{
					this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].FlexRayXcpNodeAdressValidatedProperty, Resources.ErrorUIntExpected);
					result = false;
					return;
				}
				if (num < Constants.MinCcpXcpFlexRayNodeAdress || num > Constants.MaxCcpXcpFlexRayNodeAdress)
				{
					this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].FlexRayXcpNodeAdressValidatedProperty, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, GUIUtil.NumberToDisplayString(Constants.MinCcpXcpFlexRayNodeAdress), GUIUtil.NumberToDisplayString(Constants.MaxCcpXcpFlexRayNodeAdress)));
					result = false;
					return;
				}
				bool flag;
				result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(num.ToString(CultureInfo.InvariantCulture), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].FlexRayXcpNodeAdressValidatedProperty, sourceGUIElement, out flag);
				valueChanged |= flag;
			}
		}

		private class Port : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					if (!this.SettingsDialog.UsingVxModule())
					{
						return Vocabulary.CcpXcpEcuSettingPort;
					}
					return Resources_CcpXcp.CcpXcpEcuSettingVxPort1;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Basic;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetPortValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.TextEdit;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return null;
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				IXcpDeviceConfig xcpDeviceConfig = this.SettingsDialog.DeviceConfig as IXcpDeviceConfig;
				if (xcpDeviceConfig == null)
				{
					return string.Empty;
				}
				IXcpIpConfig xcpIpConfig = xcpDeviceConfig.TransportConfig as IXcpIpConfig;
				if (xcpIpConfig == null)
				{
					return string.Empty;
				}
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetPort = (uint)xcpIpConfig.Port;
				return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetPort.ToString(CultureInfo.InvariantCulture);
			}

			public string GetStoredValueFromConfig()
			{
				return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetPort.ToString(CultureInfo.InvariantCulture);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				uint num;
				if (!uint.TryParse(this.Value, out num))
				{
					this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetPortValidatedProperty, Resources.ErrorUIntExpected);
					result = false;
					return;
				}
				if (num < Constants.MinCcpXcpEthernetPort || num > Constants.MaxCcpXcpEthernetPort)
				{
					this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetPortValidatedProperty, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinCcpXcpEthernetPort, Constants.MaxCcpXcpEthernetPort));
					result = false;
					return;
				}
				bool flag;
				result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(num.ToString(CultureInfo.InvariantCulture), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetPortValidatedProperty, sourceGUIElement, out flag);
				valueChanged |= flag;
			}
		}

		private class Port2 : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Resources_CcpXcp.CcpXcpEcuSettingVxPort2;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Basic;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetPort2ValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.TextEdit;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return null;
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				IXcpDeviceConfig xcpDeviceConfig = this.SettingsDialog.DeviceConfig as IXcpDeviceConfig;
				if (xcpDeviceConfig == null)
				{
					return string.Empty;
				}
				if (!(xcpDeviceConfig.TransportConfig is IXcpIpConfig))
				{
					return string.Empty;
				}
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetPort2 = 5554u;
				return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetPort2.ToString(CultureInfo.InvariantCulture);
			}

			public string GetStoredValueFromConfig()
			{
				return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetPort2.ToString(CultureInfo.InvariantCulture);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				uint num;
				if (!uint.TryParse(this.Value, out num))
				{
					this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetPort2ValidatedProperty, Resources.ErrorUIntExpected);
					result = false;
					return;
				}
				if (num < Constants.MinCcpXcpEthernetPort || num > Constants.MaxCcpXcpEthernetPort)
				{
					this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetPort2ValidatedProperty, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinCcpXcpEthernetPort, Constants.MaxCcpXcpEthernetPort));
					result = false;
					return;
				}
				bool flag;
				result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(num.ToString(CultureInfo.InvariantCulture), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EthernetPort2ValidatedProperty, sourceGUIElement, out flag);
				valueChanged |= flag;
			}
		}

		private class LoggerEth1Ip : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Vocabulary.CcpXcpEcuSettingEth1Ip;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Basic;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return true;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return false;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.EthernetConfigWorkingCopy.Eth1Ip;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.TextEdit;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return null;
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				return null;
			}

			public string GetStoredValueFromConfig()
			{
				return this.SettingsDialog.EthernetConfigWorkingCopy.Eth1Ip.Value;
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				if (this.ValidateIpAdress(this.Value))
				{
					bool flag;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(this.Value, this.SettingsDialog.EthernetConfigWorkingCopy.Eth1Ip, sourceGUIElement, out flag);
					valueChanged |= flag;
					return;
				}
				this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.EthernetConfigWorkingCopy.Eth1Ip, Resources.ErrorInvalidIpAdress);
				result = false;
			}

			private bool ValidateIpAdress(string aValue)
			{
				IPAddress iPAddress;
				return IPAddress.TryParse(aValue, out iPAddress);
			}
		}

		private class LoggerEth1KeepAwake : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Resources_CcpXcp.CcpXcpEcuSettingEth1KeepAwake;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Basic;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return true;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return false;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.EthernetConfigWorkingCopy.Eth1KeepAwake;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.ComboBox;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return new List<string>
					{
						Resources.Yes,
						Resources.No
					};
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				return null;
			}

			public string GetStoredValueFromConfig()
			{
				return GUIUtil.ConvertBool2YesNoString(this.SettingsDialog.EthernetConfigWorkingCopy.Eth1KeepAwake.Value);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				bool flag = false;
				if (GUIUtil.ConvertYesNoString2Bool(this.Value, ref flag))
				{
					bool flag2;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<bool>(flag, this.SettingsDialog.EthernetConfigWorkingCopy.Eth1KeepAwake, sourceGUIElement, out flag2);
					valueChanged |= flag2;
				}
			}
		}

		private class UseSeedAndKey : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Resources_CcpXcp.CcpXcpEcuSettingUseSeedAndKey;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Basic;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return this.SettingsDialog.UsingVxModule() || this.SettingsDialog.DatabaseWorkingCopy.FileType != DatabaseFileType.A2L;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpIsSeedAndKeyUsedValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.ComboBox;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return new List<string>
					{
						Resources.Yes,
						Resources.No
					};
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpIsSeedAndKeyUsed = (this.SettingsDialog.DatabaseWorkingCopy.CpProtectionsWithSeedAndKeyRequired.Count > 0);
				return GUIUtil.ConvertBool2YesNoString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpIsSeedAndKeyUsed);
			}

			public string GetStoredValueFromConfig()
			{
				return GUIUtil.ConvertBool2YesNoString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpIsSeedAndKeyUsed);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				bool flag = false;
				if (GUIUtil.ConvertYesNoString2Bool(this.Value, ref flag))
				{
					bool flag2;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(flag.ToString(), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpIsSeedAndKeyUsedValidatedProperty, sourceGUIElement, out flag2);
					valueChanged |= flag2;
				}
			}
		}

		private class ExchangeIdHandling : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Resources.ExchangeIdHandling;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Advanced;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return true;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return false;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.EnableExchangeIdHandling;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.ComboBox;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return new List<string>
					{
						Resources.On,
						Resources.Off
					};
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				return null;
			}

			public string GetStoredValueFromConfig()
			{
				return GUIUtil.ConvertBool2OnOffString(this.SettingsDialog.EnableExchangeIdHandling.Value);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				bool flag = false;
				if (GUIUtil.ConvertOnOffString2Bool(this.Value, ref flag))
				{
					bool flag2;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<bool>(flag, this.SettingsDialog.EnableExchangeIdHandling, sourceGUIElement, out flag2);
					valueChanged |= flag2;
				}
			}
		}

		private class SeedAndKeyPath : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Vocabulary.CcpXcpEcuSettingSeedAndKeyPath;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Basic;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return false;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return !this.SettingsDialog.DatabaseWorkingCopy.CcpXcpIsSeedAndKeyUsed;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.SeedAndKeyPath;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.ButtonEdit;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return null;
				}
			}

			public void OnRepositoryItemButtonClick()
			{
				GenericOpenFileDialog.FileName = "";
				string titleText = string.Format(Resources.TitleSelectSKBFileForDb, Path.GetFileName(this.SettingsDialog.DatabaseWorkingCopy.FilePath.Value));
				string text = string.Empty;
				CPProtection cPProtection;
				if (this.SettingsDialog.DatabaseWorkingCopy.CpProtectionsWithSeedAndKeyRequired.Count > 0)
				{
					cPProtection = this.SettingsDialog.DatabaseWorkingCopy.CpProtectionsWithSeedAndKeyRequired[0];
				}
				else
				{
					if (this.SettingsDialog.DatabaseWorkingCopy.FileType != DatabaseFileType.A2L || this.SettingsDialog.DatabaseWorkingCopy.CpProtections.Count <= 0)
					{
						return;
					}
					cPProtection = this.SettingsDialog.DatabaseWorkingCopy.CpProtections[0];
				}
				if (!string.IsNullOrEmpty(cPProtection.SeedAndKeyFilePath.Value))
				{
					text = this.SettingsDialog.ModelValidator.GetAbsoluteFilePath(cPProtection.SeedAndKeyFilePath.Value);
					FileInfo fileInfo = new FileInfo(text);
					if (fileInfo.Exists)
					{
						GenericOpenFileDialog.FileName = fileInfo.Name;
						GenericOpenFileDialog.InitialDirectory = fileInfo.DirectoryName;
					}
				}
				if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(titleText, FileType.SKBFile))
				{
					string fileName = GenericOpenFileDialog.FileName;
					cPProtection.SeedAndKeyFilePath.Value = this.SettingsDialog.ModelValidator.GetFilePathRelativeToConfiguration(fileName);
					if (text != GenericOpenFileDialog.FileName)
					{
						this.value = this.SettingsDialog.DatabaseWorkingCopy.SeedAndKeyPath.Value;
					}
				}
			}

			public string ApplyDbParamOrDefaultValue()
			{
				return null;
			}

			public string GetStoredValueFromConfig()
			{
				if (this.SettingsDialog.DatabaseWorkingCopy.SeedAndKeyPath != null)
				{
					return this.SettingsDialog.DatabaseWorkingCopy.SeedAndKeyPath.Value;
				}
				return string.Empty;
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
			}
		}

		private class DaqTimeout : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Resources_CcpXcp.CcpXcpEcuSettingDaqTimeout;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Advanced;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].DaqTimeoutValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.TextEdit;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return null;
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				uint daqTimeout = Constants.DefaultDaqTimeout;
				if (this.SettingsDialog.A2LDatabase != null && this.SettingsDialog.A2LDatabase.LoggerEcu != null && this.SettingsDialog.A2LDatabase.LoggerEcu.Data.DaqTimeout >= Constants.MinDaqTimeout)
				{
					daqTimeout = this.SettingsDialog.A2LDatabase.LoggerEcu.Data.DaqTimeout;
				}
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].DaqTimeout = daqTimeout;
				return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].DaqTimeout.ToString(CultureInfo.InvariantCulture);
			}

			public string GetStoredValueFromConfig()
			{
				return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].DaqTimeout.ToString(CultureInfo.InvariantCulture);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				uint num;
				if (!uint.TryParse(this.Value, out num))
				{
					this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].DaqTimeoutValidatedProperty, Resources.ErrorUIntExpected);
					result = false;
					return;
				}
				if (num < Constants.MinDaqTimeout || num > Constants.MaxDaqTimeout)
				{
					this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].DaqTimeoutValidatedProperty, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinDaqTimeout, Constants.MaxDaqTimeout));
					result = false;
					return;
				}
				bool flag;
				result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(num.ToString(CultureInfo.InvariantCulture), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].DaqTimeoutValidatedProperty, sourceGUIElement, out flag);
				valueChanged |= flag;
			}
		}

		private class SendSetSessionStatus : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Resources_CcpXcp.CcpXcpEcuSettingSendSetSessionStatus;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Advanced;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return true;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].SendSetSessionStatusValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.ComboBox;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return new List<string>
					{
						Resources.Yes,
						Resources.No
					};
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].SendSetSessionStatus = CcpXcpManager.GetSendSetSessionStatusDbParamOrDefaultValue(this.SettingsDialog.DeviceConfig);
				return GUIUtil.ConvertBool2YesNoString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].SendSetSessionStatus);
			}

			public string GetStoredValueFromConfig()
			{
				return GUIUtil.ConvertBool2YesNoString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].SendSetSessionStatus);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				bool flag = false;
				if (GUIUtil.ConvertYesNoString2Bool(this.Value, ref flag))
				{
					bool flag2;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(flag.ToString(), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].SendSetSessionStatusValidatedProperty, sourceGUIElement, out flag2);
					valueChanged |= flag2;
				}
			}
		}

		private class ExtendStaticDaqListToMaxSize : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Resources_CcpXcp.DaqListSize;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Advanced;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].ExtendStaticDaqListToMaxSizeValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.ComboBox;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return new List<string>
					{
						ProtocolSettingsDialog.ExtendStaticDaqListToMaxSize.GetValueString(false),
						ProtocolSettingsDialog.ExtendStaticDaqListToMaxSize.GetValueString(true)
					};
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].ExtendStaticDaqListToMaxSize = false;
				return ProtocolSettingsDialog.ExtendStaticDaqListToMaxSize.GetValueString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].ExtendStaticDaqListToMaxSize);
			}

			public string GetStoredValueFromConfig()
			{
				return ProtocolSettingsDialog.ExtendStaticDaqListToMaxSize.GetValueString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].ExtendStaticDaqListToMaxSize);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				bool flag = ProtocolSettingsDialog.ExtendStaticDaqListToMaxSize.GetValueString(true) == this.Value;
				bool flag2;
				result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(flag.ToString(), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].ExtendStaticDaqListToMaxSizeValidatedProperty, sourceGUIElement, out flag2);
				valueChanged |= flag2;
			}

			private static string GetValueString(bool aValue)
			{
				if (!aValue)
				{
					return Resources.FromDescription;
				}
				return Resources.ExpandStaticListsToMaxSize;
			}
		}

		private class IsProtocolByteOrderIntel : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Resources.ProtocolByteOrder;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Advanced;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return false;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsProtocolByteOrderIntelValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.ComboBox;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return new List<string>
					{
						ProtocolSettingsDialog.IsProtocolByteOrderIntel.GetValueString(true),
						ProtocolSettingsDialog.IsProtocolByteOrderIntel.GetValueString(false)
					};
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				if (this.SettingsDialog.UsingVxModule())
				{
					return ProtocolSettingsDialog.IsProtocolByteOrderIntel.GetValueString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsProtocolByteOrderIntel = true);
				}
				IXcpDeviceConfig xcpDeviceConfig = this.SettingsDialog.DeviceConfig as IXcpDeviceConfig;
				ICcpDeviceConfig ccpDeviceConfig = this.SettingsDialog.DeviceConfig as ICcpDeviceConfig;
				if (xcpDeviceConfig != null && xcpDeviceConfig.ProtocolConfig != null)
				{
					this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsProtocolByteOrderIntel = xcpDeviceConfig.ProtocolConfig.ByteOrderIntel;
				}
				else if (ccpDeviceConfig != null)
				{
					this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsProtocolByteOrderIntel = !ccpDeviceConfig.ByteOrderMotorola;
				}
				else
				{
					this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsProtocolByteOrderIntel = true;
				}
				return ProtocolSettingsDialog.IsProtocolByteOrderIntel.GetValueString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsProtocolByteOrderIntel);
			}

			public string GetStoredValueFromConfig()
			{
				return ProtocolSettingsDialog.IsProtocolByteOrderIntel.GetValueString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsProtocolByteOrderIntel);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				bool flag = ProtocolSettingsDialog.IsProtocolByteOrderIntel.GetValueString(true) == this.Value;
				if (this.SettingsDialog.UsingVxModule() && !flag)
				{
					this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.LocalModelError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsProtocolByteOrderIntelValidatedProperty, Resources.ErrorVxModuleNeedsIntelByteOrder);
					result = false;
					return;
				}
				bool flag2;
				result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(flag.ToString(), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].IsProtocolByteOrderIntelValidatedProperty, sourceGUIElement, out flag2);
				valueChanged |= flag2;
			}

			private static string GetValueString(bool aValue)
			{
				if (!aValue)
				{
					return Resources.Motorola;
				}
				return Resources.Intel;
			}
		}

		private class UseEcuTimestamp : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Resources.UseEcuTimestamp;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Advanced;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return this.SettingsDialog.UsingVxModule() || this.SettingsDialog.DatabaseWorkingCopy.FileType != DatabaseFileType.A2L;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].UseEcuTimestampValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.ComboBox;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return new List<string>
					{
						Resources.Yes,
						Resources.No
					};
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				bool useEcuTimestampDbParamOrDefaultValue = CcpXcpManager.GetUseEcuTimestampDbParamOrDefaultValue(this.SettingsDialog.DeviceConfig, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].UseVxModule);
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].UseEcuTimestamp = useEcuTimestampDbParamOrDefaultValue;
				return GUIUtil.ConvertBool2YesNoString(useEcuTimestampDbParamOrDefaultValue);
			}

			public string GetStoredValueFromConfig()
			{
				return GUIUtil.ConvertBool2YesNoString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].UseEcuTimestamp);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				bool flag = false;
				if (GUIUtil.ConvertYesNoString2Bool(this.Value, ref flag))
				{
					bool flag2;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(flag.ToString(), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].UseEcuTimestampValidatedProperty, sourceGUIElement, out flag2);
					valueChanged |= flag2;
				}
			}
		}

		private class EcuTimestampWidth : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Resources.EcuTimestamp + ": " + Resources.Width;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Advanced;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return !this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].UseEcuTimestamp;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampWidthValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.ComboBox;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return new List<string>
					{
						"BYTE",
						"WORD",
						"DWORD"
					};
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampWidth = CcpXcpManager.GetEcuTimestampWidthDbParamOrDefaultValue(this.SettingsDialog.DeviceConfig);
				return this.GetValueString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampWidth);
			}

			public string GetStoredValueFromConfig()
			{
				return this.GetValueString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampWidth);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				uint num;
				if (ProtocolSettingsDialog.EcuTimestampWidth.IsValidValue(this.Value, out num))
				{
					bool flag;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(num.ToString(), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampWidthValidatedProperty, sourceGUIElement, out flag);
					valueChanged |= flag;
					return;
				}
				if (this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].UseEcuTimestamp)
				{
					this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampWidthValidatedProperty, Resources.ErrorGenValueOutOfRange);
					result = false;
				}
			}

			private string GetValueString(uint width)
			{
				switch (width)
				{
				case 1u:
					return "BYTE";
				case 2u:
					return "WORD";
				case 4u:
					return "DWORD";
				}
				return string.Format(Resources.SizeInBytes, width);
			}

			private static bool IsValidValue(string valueStr, out uint aValue)
			{
				aValue = 0u;
				if (valueStr == "BYTE")
				{
					aValue = 1u;
					return true;
				}
				if (valueStr == "WORD")
				{
					aValue = 2u;
					return true;
				}
				if (valueStr == "DWORD")
				{
					aValue = 4u;
					return true;
				}
				return false;
			}
		}

		private class EcuTimestampUnit : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Resources.EcuTimestamp + ": " + Resources.Unit;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Advanced;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return !this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].UseEcuTimestamp;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampUnitValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.ComboBox;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return new List<string>
					{
						this.GetValueString(CcpXcpEcuTimestampUnit.TU_1Seconds),
						this.GetValueString(CcpXcpEcuTimestampUnit.TU_100Milliseconds),
						this.GetValueString(CcpXcpEcuTimestampUnit.TU_10Milliseconds),
						this.GetValueString(CcpXcpEcuTimestampUnit.TU_1Milliseconds),
						this.GetValueString(CcpXcpEcuTimestampUnit.TU_100Microseconds),
						this.GetValueString(CcpXcpEcuTimestampUnit.TU_10Microseconds),
						this.GetValueString(CcpXcpEcuTimestampUnit.TU_1Microseconds),
						this.GetValueString(CcpXcpEcuTimestampUnit.TU_100Nanoseconds),
						this.GetValueString(CcpXcpEcuTimestampUnit.TU_10Nanoseconds),
						this.GetValueString(CcpXcpEcuTimestampUnit.TU_1Nanoseconds),
						this.GetValueString(CcpXcpEcuTimestampUnit.TU_100Picoseconds),
						this.GetValueString(CcpXcpEcuTimestampUnit.TU_10Picoseconds),
						this.GetValueString(CcpXcpEcuTimestampUnit.TU_1Picoseconds)
					};
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampUnit = CcpXcpManager.GetEcuTimestampUnitDbParamOrDefaultValue(this.SettingsDialog.DeviceConfig);
				return this.GetValueString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampUnit);
			}

			public string GetStoredValueFromConfig()
			{
				return this.GetValueString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampUnit);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				bool flag;
				result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(this.GetValueFromString(this.Value).ToString(), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampUnitValidatedProperty, sourceGUIElement, out flag);
				valueChanged |= flag;
			}

			private string GetValueString(CcpXcpEcuTimestampUnit val)
			{
				switch (val)
				{
				case CcpXcpEcuTimestampUnit.TU_1Picoseconds:
					return "1 " + Resources.TimeUnitPicosec;
				case CcpXcpEcuTimestampUnit.TU_10Picoseconds:
					return "10 " + Resources.TimeUnitPicosecs;
				case CcpXcpEcuTimestampUnit.TU_100Picoseconds:
					return "100 " + Resources.TimeUnitPicosecs;
				case CcpXcpEcuTimestampUnit.TU_1Nanoseconds:
					return "1 " + Resources.TimeUnitNanosec;
				case CcpXcpEcuTimestampUnit.TU_10Nanoseconds:
					return "10 " + Resources.TimeUnitNanosecs;
				case CcpXcpEcuTimestampUnit.TU_100Nanoseconds:
					return "100 " + Resources.TimeUnitNanosecs;
				case CcpXcpEcuTimestampUnit.TU_1Microseconds:
					return "1 " + Resources.TimeUnitMicrosec;
				case CcpXcpEcuTimestampUnit.TU_10Microseconds:
					return "10 " + Resources.TimeUnitMicrosecs;
				case CcpXcpEcuTimestampUnit.TU_100Microseconds:
					return "100 " + Resources.TimeUnitMicrosecs;
				case CcpXcpEcuTimestampUnit.TU_1Milliseconds:
					return "1 " + Resources.TimeUnitMillisec;
				case CcpXcpEcuTimestampUnit.TU_10Milliseconds:
					return "10 " + Resources.TimeUnitMillisecs;
				case CcpXcpEcuTimestampUnit.TU_100Milliseconds:
					return "100 " + Resources.TimeUnitMillisecs;
				case CcpXcpEcuTimestampUnit.TU_1Seconds:
					return "1 " + Resources.TimeUnitSec;
				default:
					return Resources.Unknown;
				}
			}

			private CcpXcpEcuTimestampUnit GetValueFromString(string valueStr)
			{
				foreach (CcpXcpEcuTimestampUnit ccpXcpEcuTimestampUnit in Enum.GetValues(typeof(CcpXcpEcuTimestampUnit)))
				{
					if (valueStr == this.GetValueString(ccpXcpEcuTimestampUnit))
					{
						return ccpXcpEcuTimestampUnit;
					}
				}
				return CcpXcpEcuTimestampUnit.TU_Unspecified;
			}
		}

		private class EcuTimestampTicks : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Resources.EcuTimestamp + ": " + Resources.Ticks;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Advanced;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return !this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].UseEcuTimestamp;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampTicksValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.TextEdit;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return null;
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampTicks = CcpXcpManager.GetEcuTimestampTicksDbParamOrDefaultValue(this.SettingsDialog.DeviceConfig);
				return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampTicks.ToString(CultureInfo.InvariantCulture);
			}

			public string GetStoredValueFromConfig()
			{
				return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampTicks.ToString(CultureInfo.InvariantCulture);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				ulong num;
				if (ulong.TryParse(this.Value, out num))
				{
					bool flag;
					result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(num.ToString(CultureInfo.InvariantCulture), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampTicksValidatedProperty, sourceGUIElement, out flag);
					valueChanged |= flag;
					return;
				}
				this.SettingsDialog.PageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampTicksValidatedProperty, Resources.InvalidValueInteger);
				result = false;
			}
		}

		private class EcuTimestampCalculationMethod : ProtocolSettingsDialog.ISetting
		{
			private string value;

			public string Name
			{
				get
				{
					return Resources.EcuTimestamp + ": " + Resources.CalcMethod;
				}
			}

			public string Value
			{
				get
				{
					if (this.value != null)
					{
						return this.value;
					}
					return this.value = this.GetStoredValueFromConfig();
				}
				set
				{
					if (value != null)
					{
						this.value = value;
					}
				}
			}

			public ProtocolSettingsDialog.SettingsGroup Group
			{
				get
				{
					return ProtocolSettingsDialog.SettingsGroup.Advanced;
				}
			}

			public bool IsGlobalSetting
			{
				get
				{
					return false;
				}
			}

			public bool HasDbParamOrDefaultValue
			{
				get
				{
					return true;
				}
			}

			public bool IsValueReadOnly
			{
				get
				{
					return !this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].UseEcuTimestamp;
				}
			}

			public ProtocolSettingsDialog SettingsDialog
			{
				get;
				set;
			}

			public IValidatedProperty ValidatedProperty
			{
				get
				{
					return this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampCalculationMethodValidatedProperty;
				}
			}

			public ProtocolSettingsDialog.RepositoryItemType RepositoryItemType
			{
				get
				{
					return ProtocolSettingsDialog.RepositoryItemType.ComboBox;
				}
			}

			public List<string> RepositoryItems
			{
				get
				{
					return new List<string>
					{
						this.GetValueString(CcpXcpEcuTimestampCalculationMethod.Multiplication),
						this.GetValueString(CcpXcpEcuTimestampCalculationMethod.Division)
					};
				}
			}

			public void OnRepositoryItemButtonClick()
			{
			}

			public string ApplyDbParamOrDefaultValue()
			{
				this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampCalculationMethod = CcpXcpEcuTimestampCalculationMethod.Multiplication;
				return this.GetValueString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampCalculationMethod);
			}

			public string GetStoredValueFromConfig()
			{
				return this.GetValueString(this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampCalculationMethod);
			}

			public void Validate(IValidatedGUIElement sourceGUIElement, ref bool result, ref bool valueChanged)
			{
				bool flag;
				result &= this.SettingsDialog.PageValidator.Control.UpdateModel<string>(this.GetValueFromString(this.Value).ToString(), this.SettingsDialog.DatabaseWorkingCopy.CcpXcpEcuList[0].EcuTimestampCalculationMethodValidatedProperty, sourceGUIElement, out flag);
				valueChanged |= flag;
			}

			private string GetValueString(CcpXcpEcuTimestampCalculationMethod val)
			{
				switch (val)
				{
				case CcpXcpEcuTimestampCalculationMethod.Multiplication:
					return Resources.Multiplication;
				case CcpXcpEcuTimestampCalculationMethod.Division:
					return Resources.Division;
				default:
					return Resources.Unknown;
				}
			}

			private CcpXcpEcuTimestampCalculationMethod GetValueFromString(string displayStr)
			{
				foreach (CcpXcpEcuTimestampCalculationMethod ccpXcpEcuTimestampCalculationMethod in Enum.GetValues(typeof(CcpXcpEcuTimestampCalculationMethod)))
				{
					if (displayStr == this.GetValueString(ccpXcpEcuTimestampCalculationMethod))
					{
						return ccpXcpEcuTimestampCalculationMethod;
					}
				}
				return CcpXcpEcuTimestampCalculationMethod.Unspecified;
			}
		}

		private readonly int SettingColumnIndex;

		private readonly int ValueColumnIndex = 1;

		private GUIElementManager_Control guiElementManager;

		private GUIElementManager_ControlGridTree guiGridElementManager;

		private CustomErrorProvider customErrorProvider;

		private GeneralService gridGeneralService;

		private KeyboardNavigationService gridKeyboardNavigationService;

		private BindingList<ProtocolSettingsDialog.ISetting> settings;

		private Database mDatabaseWorkingCopy;

		private EthernetConfiguration mEthernetConfigWorkingCopy;

		private PageValidator pageValidator;

		private IModelValidator modelValidator;

		private IContainer components;

		private Button mButtonOK;

		private Button mButtonCancel;

		private Button mButtonHelp;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private GridControl mGridSettings;

		private GridView mGridViewSettings;

		private RepositoryItemButtonEdit repositoryItemButtonEdit;

		private RepositoryItemCheckEdit repositoryItemCheckEditIsEnabled;

		private RepositoryItemTextEdit repositoryItemTextEditDummy;

		private RepositoryItemTextEdit repositoryItemTextEditIp;

		private CheckBox checkBoxUseDbParams;

		private GridColumn gridColumnGroup;

		private GridColumn gridColumnParameter;

		private GridColumn gridColumnValue;

		private RepositoryItemComboBox repositoryItemComboBox;

		internal ReadOnlyCollection<ProtocolSettingsDialog.ISetting> Settings
		{
			get
			{
				return new ReadOnlyCollection<ProtocolSettingsDialog.ISetting>(this.settings);
			}
		}

		internal Database Database
		{
			get;
			set;
		}

		internal Database DatabaseWorkingCopy
		{
			get
			{
				return this.mDatabaseWorkingCopy;
			}
		}

		internal ValidatedProperty<bool> EnableExchangeIdHandling
		{
			get;
			set;
		}

		internal EthernetConfiguration EthernetConfiguration
		{
			get;
			set;
		}

		internal EthernetConfiguration EthernetConfigWorkingCopy
		{
			get
			{
				return this.mEthernetConfigWorkingCopy;
			}
		}

		internal string ConfigurationFolderPath
		{
			get;
			set;
		}

		internal string ModeStr
		{
			get;
			private set;
		}

		internal A2LDatabase A2LDatabase
		{
			get;
			private set;
		}

		internal IDeviceConfig DeviceConfig
		{
			get;
			private set;
		}

		internal PageValidator PageValidator
		{
			get
			{
				return this.pageValidator;
			}
		}

		internal IModelValidator ModelValidator
		{
			get
			{
				return this.modelValidator;
			}
		}

		public ProtocolSettingsDialog(IModelValidator modelVal)
		{
			this.InitializeComponent();
			this.settings = new BindingList<ProtocolSettingsDialog.ISetting>();
			this.modelValidator = modelVal;
			this.guiElementManager = new GUIElementManager_Control();
			this.guiGridElementManager = new GUIElementManager_ControlGridTree();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.gridGeneralService = new GeneralService(this.mGridViewSettings);
			this.gridKeyboardNavigationService = new KeyboardNavigationService(this.mGridViewSettings);
		}

		private void ProtocolSettingsDialog_Load(object sender, EventArgs e)
		{
			this.gridGeneralService.InitAppearance();
		}

		private void ProtocolSettingsDialog_Shown(object sender, EventArgs e)
		{
			if (this.EthernetConfiguration == null)
			{
				return;
			}
			this.settings = new BindingList<ProtocolSettingsDialog.ISetting>();
			this.mDatabaseWorkingCopy = new Database(this.Database);
			this.mEthernetConfigWorkingCopy = new EthernetConfiguration(this.EthernetConfiguration);
			this.ModeStr = Resources.DisplayModeDec;
			if (GUIUtil.IsHexadecimal)
			{
				this.ModeStr = Resources.DisplayModeHex;
			}
			this.checkBoxUseDbParams.Checked = this.mDatabaseWorkingCopy.CcpXcpUseDbParams;
			this.checkBoxUseDbParams.Enabled = (this.mDatabaseWorkingCopy.FileType == DatabaseFileType.A2L);
			this.PopulateSettingsGrid();
			this.UseDbParams();
			this.ValidateInput();
		}

		private void PopulateSettingsGrid()
		{
			this.mGridSettings.DataSource = null;
			if ((this.mDatabaseWorkingCopy.FileType == DatabaseFileType.A2L && this.mDatabaseWorkingCopy.CPType.Value == CPType.XCP && this.mDatabaseWorkingCopy.BusType.Value == BusType.Bt_CAN) || (this.mDatabaseWorkingCopy.FileType == DatabaseFileType.A2L && (this.mDatabaseWorkingCopy.CPType.Value == CPType.CCP || this.mDatabaseWorkingCopy.CPType.Value == CPType.CCP101)))
			{
				this.AddSetting(new ProtocolSettingsDialog.RequestId());
				this.AddSetting(new ProtocolSettingsDialog.ResponseId());
				this.AddSetting(new ProtocolSettingsDialog.FirstId());
			}
			if (this.mDatabaseWorkingCopy.FileType == DatabaseFileType.A2L && this.mDatabaseWorkingCopy.CPType.Value == CPType.CCP)
			{
				this.AddSetting(new ProtocolSettingsDialog.CcpVersion());
			}
			if (this.mDatabaseWorkingCopy.FileType == DatabaseFileType.A2L && this.mDatabaseWorkingCopy.CPType.Value == CPType.XCP && this.mDatabaseWorkingCopy.BusType.Value == BusType.Bt_FlexRay)
			{
				this.AddSetting(new ProtocolSettingsDialog.FlexRayEcuTxQueue());
				this.AddSetting(new ProtocolSettingsDialog.NodeAddress());
				this.AddSetting(new ProtocolSettingsDialog.MaxCTO());
				this.AddSetting(new ProtocolSettingsDialog.MaxDTO());
			}
			if (this.mDatabaseWorkingCopy.FileType == DatabaseFileType.A2L && this.mDatabaseWorkingCopy.CPType.Value == CPType.XCP && this.mDatabaseWorkingCopy.BusType.Value == BusType.Bt_Ethernet)
			{
				this.AddSetting(new ProtocolSettingsDialog.Host());
				this.AddSetting(new ProtocolSettingsDialog.Port());
				if (this.UsingVxModule())
				{
					this.AddSetting(new ProtocolSettingsDialog.Port2());
				}
				else
				{
					this.AddSetting(new ProtocolSettingsDialog.MaxCTO());
				}
				this.AddSetting(new ProtocolSettingsDialog.MaxDTO());
			}
			if (this.mDatabaseWorkingCopy.FileType == DatabaseFileType.A2L)
			{
				this.AddSetting(new ProtocolSettingsDialog.DaqTimeout());
			}
			if (!this.UsingVxModule())
			{
				this.AddSetting(new ProtocolSettingsDialog.UseSeedAndKey());
				this.AddSetting(new ProtocolSettingsDialog.SeedAndKeyPath());
			}
			if (this.mDatabaseWorkingCopy.CPType.Value == CPType.CCP || this.mDatabaseWorkingCopy.CPType.Value == CPType.CCP101)
			{
				this.AddSetting(new ProtocolSettingsDialog.ExchangeIdHandling());
			}
			if (this.mDatabaseWorkingCopy.FileType == DatabaseFileType.A2L && this.mDatabaseWorkingCopy.CPType.Value == CPType.CCP && this.mDatabaseWorkingCopy.BusType.Value == BusType.Bt_CAN)
			{
				this.AddSetting(new ProtocolSettingsDialog.SendSetSessionStatus());
			}
			if (this.mDatabaseWorkingCopy.FileType == DatabaseFileType.A2L && this.mDatabaseWorkingCopy.CPType.Value == CPType.XCP && this.mDatabaseWorkingCopy.BusType.Value == BusType.Bt_Ethernet)
			{
				this.AddSetting(new ProtocolSettingsDialog.LoggerEth1Ip());
				this.AddSetting(new ProtocolSettingsDialog.LoggerEth1KeepAwake());
			}
			if (this.mDatabaseWorkingCopy.FileType == DatabaseFileType.A2L && this.mDatabaseWorkingCopy.CcpXcpEcuList.Any<CcpXcpEcu>() && !this.mDatabaseWorkingCopy.CcpXcpEcuList[0].UseVxModule)
			{
				this.AddSetting(new ProtocolSettingsDialog.ExtendStaticDaqListToMaxSize());
			}
			if (this.mDatabaseWorkingCopy.FileType == DatabaseFileType.A2L)
			{
				this.AddSetting(new ProtocolSettingsDialog.IsProtocolByteOrderIntel());
			}
			if (this.mDatabaseWorkingCopy.FileType == DatabaseFileType.A2L && this.mDatabaseWorkingCopy.CPType.Value == CPType.XCP)
			{
				this.AddSetting(new ProtocolSettingsDialog.UseEcuTimestamp());
				this.AddSetting(new ProtocolSettingsDialog.EcuTimestampWidth());
				this.AddSetting(new ProtocolSettingsDialog.EcuTimestampUnit());
				this.AddSetting(new ProtocolSettingsDialog.EcuTimestampTicks());
				this.AddSetting(new ProtocolSettingsDialog.EcuTimestampCalculationMethod());
			}
			this.mGridSettings.DataSource = this.settings;
			if (this.mGridViewSettings.Columns.Count > 0 && this.mGridViewSettings.RowCount > 0)
			{
				this.mGridViewSettings.FocusedColumn = this.mGridViewSettings.Columns[this.SettingColumnIndex];
			}
			this.mGridViewSettings.RefreshData();
		}

		private void mButtonOK_Click(object sender, EventArgs e)
		{
			if (!this.HasFormatErrors() && !this.HasLocalErrors())
			{
				this.Database = this.mDatabaseWorkingCopy;
				this.EthernetConfiguration = this.mEthernetConfigWorkingCopy;
				base.DialogResult = DialogResult.OK;
				return;
			}
			InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
			base.DialogResult = DialogResult.None;
			this.mGridViewSettings.RefreshData();
		}

		private void checkBoxUseDbParams_CheckedChanged(object sender, EventArgs e)
		{
			this.UseDbParams();
			this.ValidateInput();
		}

		private void ProtocolSettingsDialog_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void mButtonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void mGridViewSettings_ShowingEditor(object sender, CancelEventArgs e)
		{
			this.gridGeneralService.ShowingEditor<DataRow>(e, this.mGridViewSettings.GetDataRow(this.mGridViewSettings.FocusedRowHandle), this.mGridViewSettings.FocusedColumn, new GeneralService.IsReadOnlyAtAll<DataRow>(this.IsCellReadOnlyAtAll), new GeneralService.IsReadOnlyByCellContent<DataRow>(this.IsCellReadOnlyByCellContent));
			ProtocolSettingsDialog.ISetting setting;
			if (this.GetSetting(this.mGridViewSettings.FocusedRowHandle, out setting))
			{
				ProtocolSettingsDialog.RepositoryItemType repositoryItemType = setting.RepositoryItemType;
				if (repositoryItemType == ProtocolSettingsDialog.RepositoryItemType.ComboBox)
				{
					this.repositoryItemComboBox.Items.Clear();
					foreach (string current in setting.RepositoryItems)
					{
						this.repositoryItemComboBox.Items.Add(current);
					}
				}
			}
			if (setting.IsValueReadOnly)
			{
				e.Cancel = true;
			}
			if (setting.HasDbParamOrDefaultValue && this.DatabaseWorkingCopy.CcpXcpUseDbParams)
			{
				e.Cancel = true;
			}
		}

		private bool IsCellReadOnlyAtAll(object boundObject, GridColumn column)
		{
			return column == this.mGridViewSettings.Columns[this.SettingColumnIndex];
		}

		private bool IsCellReadOnlyByCellContent(object boundObject, GridColumn column)
		{
			if (column != this.mGridViewSettings.Columns[this.ValueColumnIndex])
			{
				return false;
			}
			ProtocolSettingsDialog.ISetting setting = boundObject as ProtocolSettingsDialog.ISetting;
			return setting != null && ((setting.HasDbParamOrDefaultValue && this.DatabaseWorkingCopy.CcpXcpUseDbParams) || setting.IsValueReadOnly);
		}

		private void mGridViewSettings_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			e.RepositoryItem = this.repositoryItemTextEditDummy;
			if (e.Column != this.mGridViewSettings.Columns[this.ValueColumnIndex])
			{
				return;
			}
			if (e.RowHandle != this.mGridViewSettings.FocusedRowHandle)
			{
				return;
			}
			object dataRow = this.mGridViewSettings.GetDataRow(e.RowHandle);
			if (this.IsCellReadOnlyAtAll(dataRow, e.Column) || this.IsCellReadOnlyByCellContent(dataRow, e.Column))
			{
				return;
			}
			ProtocolSettingsDialog.ISetting setting;
			if (this.GetSetting(e.RowHandle, out setting))
			{
				switch (setting.RepositoryItemType)
				{
				case ProtocolSettingsDialog.RepositoryItemType.ComboBox:
					this.repositoryItemComboBox.ReadOnly = setting.IsValueReadOnly;
					e.RepositoryItem = this.repositoryItemComboBox;
					return;
				case ProtocolSettingsDialog.RepositoryItemType.ButtonEdit:
					this.repositoryItemButtonEdit.ReadOnly = setting.IsValueReadOnly;
					this.repositoryItemButtonEdit.Buttons[0].Enabled = !setting.IsValueReadOnly;
					e.RepositoryItem = this.repositoryItemButtonEdit;
					return;
				default:
					e.RepositoryItem = this.repositoryItemTextEditDummy;
					break;
				}
			}
		}

		private void mGridViewSettings_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			this.gridGeneralService.CustomDrawCell<object>(e, new GeneralService.IsReadOnlyAtAll<object>(this.IsCellReadOnlyAtAll), new GeneralService.IsReadOnlyByCellContent<object>(this.IsCellReadOnlyByCellContent));
			IValidatedGUIElement gUIElement = this.guiGridElementManager.GetGUIElement(e.Column, this.mGridViewSettings.GetDataSourceRowIndex(e.RowHandle));
			this.customErrorProvider.Grid.DisplayError(gUIElement, e);
		}

		private void repositoryItemButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			ProtocolSettingsDialog.ISetting setting;
			if (this.GetSetting(this.mGridViewSettings.FocusedRowHandle, out setting))
			{
				setting.OnRepositoryItemButtonClick();
			}
			this.ValidateInput();
			this.mGridViewSettings.RefreshEditor(true);
		}

		private void repositoryItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.mGridViewSettings.PostEditor();
			this.mGridViewSettings.RefreshData();
			this.ValidateInput();
		}

		private void mGridViewSettings_ShownEditor(object sender, EventArgs e)
		{
			this.gridKeyboardNavigationService.GridViewShownEditor();
		}

		private void mGridViewSettings_HiddenEditor(object sender, EventArgs e)
		{
			this.ValidateInput();
		}

		private void mGridViewSettings_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
		{
			this.gridGeneralService.FocusedColumnChanged(e);
		}

		private void mGridViewSettings_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
		{
			this.gridGeneralService.FocusedRowChanged(e);
			this.mGridViewSettings.Invalidate();
		}

		private void mGridViewSettings_MouseDown(object sender, MouseEventArgs e)
		{
			this.gridGeneralService.MouseDown(e);
		}

		private void mGridViewSettings_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			this.gridGeneralService.PopupMenuShowing(e);
		}

		private void mGridSettings_ProcessGridKey(object sender, KeyEventArgs e)
		{
			this.gridKeyboardNavigationService.GridControlProcessGridKey(e);
		}

		private void mGridSettings_EditorKeyDown(object sender, KeyEventArgs e)
		{
			this.gridKeyboardNavigationService.GridControlEditorKeyDown(e);
		}

		private void mGridViewSettings_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
		{
			GridView gridView = sender as GridView;
			if (gridView == null)
			{
				return;
			}
			gridView.GetGroupRowValue(e.RowHandle);
			GridGroupRowInfo gridGroupRowInfo = e.Info as GridGroupRowInfo;
			if (gridGroupRowInfo != null && gridGroupRowInfo.Column.FieldName == "Group")
			{
				object groupRowValue = gridView.GetGroupRowValue(e.RowHandle);
				if (groupRowValue is ProtocolSettingsDialog.SettingsGroup)
				{
					ProtocolSettingsDialog.SettingsGroup settingsGroup = (ProtocolSettingsDialog.SettingsGroup)groupRowValue;
					if (settingsGroup == ProtocolSettingsDialog.SettingsGroup.Basic)
					{
						gridGroupRowInfo.GroupText = Resources.General;
						return;
					}
					if (settingsGroup == ProtocolSettingsDialog.SettingsGroup.Advanced)
					{
						gridGroupRowInfo.GroupText = Resources.Advanced;
						return;
					}
					gridGroupRowInfo.GroupText = gridView.GetGroupRowValue(e.RowHandle).ToString();
				}
			}
		}

		private void mGridViewSettings_TopRowChanged(object sender, EventArgs e)
		{
			this.StoreMapping4VisibleCells();
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
		}

		private bool ValidateInput()
		{
			this.ResetValidationFramework();
			this.StoreMapping4VisibleCells();
			bool flag = true;
			bool flag2 = false;
			bool flag3;
			flag &= this.pageValidator.Control.UpdateModel<string>(this.checkBoxUseDbParams.Checked.ToString(), this.mDatabaseWorkingCopy.CcpXcpUseDbParamsValidatedProperty, this.guiElementManager.GetGUIElement(this.checkBoxUseDbParams), out flag3);
			flag2 |= flag3;
			foreach (ProtocolSettingsDialog.ISetting current in this.Settings)
			{
				current.Validate(this.guiGridElementManager.GetGUIElement(this.mGridViewSettings.Columns[this.ValueColumnIndex], this.Settings.IndexOf(current)), ref flag, ref flag2);
			}
			flag &= this.modelValidator.ValidateCcpXcpDatabaseSettings(this.mDatabaseWorkingCopy, this.mEthernetConfigWorkingCopy, this.pageValidator);
			flag &= this.modelValidator.ValidateCcpXcpEcuIdSettings(this.mDatabaseWorkingCopy, this.pageValidator);
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			return flag;
		}

		private void StoreMapping4VisibleCells()
		{
			PageValidatorGridUtil.StoreRowMappingForVisibleRows(this.mGridViewSettings, new PageValidatorGridUtil.StoreRowMappingHandler(this.OnPageValidatorGridUtilStoreRowMapping));
		}

		private void OnPageValidatorGridUtilStoreRowMapping(int dataSourceRowIdx)
		{
			if (this.mGridViewSettings.RowCount < 1)
			{
				return;
			}
			this.StoreMapping4VisibleColumns(dataSourceRowIdx);
		}

		private void StoreMapping4VisibleColumns(int dataSourceIdx)
		{
			if (PageValidatorGridUtil.IsColumnVisible(this.mGridViewSettings.Columns[this.ValueColumnIndex], this.mGridViewSettings))
			{
				IValidatedGUIElement gUIElement = this.guiGridElementManager.GetGUIElement(this.mGridViewSettings.Columns[this.ValueColumnIndex], dataSourceIdx);
				if (dataSourceIdx < this.settings.Count && this.settings[dataSourceIdx] != null)
				{
					this.pageValidator.Grid.StoreMapping(this.settings[dataSourceIdx].ValidatedProperty, gUIElement);
				}
			}
		}

		public void Reset()
		{
			this.ResetValidationFramework();
		}

		public bool HasErrors()
		{
			return this.pageValidator.General.HasErrors(Enum.GetValues(typeof(ValidationErrorClass)) as ValidationErrorClass[]);
		}

		public bool HasLocalErrors()
		{
			return this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.FormatError,
				ValidationErrorClass.LocalModelError
			});
		}

		public bool HasFormatErrors()
		{
			IPageValidatorGeneral arg_13_0 = this.pageValidator.General;
			ValidationErrorClass[] errorClasses = new ValidationErrorClass[1];
			return arg_13_0.HasErrors(errorClasses);
		}

		private void ResetValidationFramework()
		{
			this.pageValidator.General.Reset();
			this.guiElementManager.Reset();
		}

		private void AddSetting(ProtocolSettingsDialog.ISetting setting)
		{
			setting.SettingsDialog = this;
			this.settings.Add(setting);
		}

		private bool GetSetting(int rowHandle, out ProtocolSettingsDialog.ISetting setting)
		{
			setting = null;
			if (this.Settings.Count <= rowHandle || rowHandle < 0)
			{
				return false;
			}
			int dataSourceRowIndex = this.mGridViewSettings.GetDataSourceRowIndex(rowHandle);
			if (this.settings.Count <= dataSourceRowIndex || dataSourceRowIndex < 0)
			{
				return false;
			}
			setting = this.settings[dataSourceRowIndex];
			return true;
		}

		private void UseDbParams()
		{
			if (!this.checkBoxUseDbParams.Checked)
			{
				this.mGridViewSettings.RefreshData();
				return;
			}
			A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(this.mDatabaseWorkingCopy);
			if (a2LDatabase == null)
			{
				Cursor.Current = Cursors.WaitCursor;
				if (CcpXcpManager.Instance().LoadA2LDatabase(this.mDatabaseWorkingCopy) != Vector.McModule.Result.kOk)
				{
					Cursor.Current = Cursors.Default;
					return;
				}
				a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(this.mDatabaseWorkingCopy);
				Cursor.Current = Cursors.Default;
			}
			string text;
			if (!a2LDatabase.CreateDeviceConfig(this.mDatabaseWorkingCopy, out text, false))
			{
				return;
			}
			if (a2LDatabase.DeviceConfig == null)
			{
				return;
			}
			this.A2LDatabase = a2LDatabase;
			this.DeviceConfig = a2LDatabase.DeviceConfig;
			foreach (ProtocolSettingsDialog.ISetting current in this.Settings)
			{
				if (current.HasDbParamOrDefaultValue)
				{
					current.Value = current.ApplyDbParamOrDefaultValue();
				}
			}
			this.mGridViewSettings.RefreshData();
		}

		public bool UsingVxModule()
		{
			return this.DatabaseWorkingCopy.CcpXcpEcuList.Any<CcpXcpEcu>() && this.DatabaseWorkingCopy.CcpXcpEcuList[0].UseVxModule;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ProtocolSettingsDialog));
			this.mButtonOK = new Button();
			this.mButtonCancel = new Button();
			this.mButtonHelp = new Button();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.mGridSettings = new GridControl();
			this.mGridViewSettings = new GridView();
			this.gridColumnParameter = new GridColumn();
			this.gridColumnValue = new GridColumn();
			this.gridColumnGroup = new GridColumn();
			this.repositoryItemButtonEdit = new RepositoryItemButtonEdit();
			this.repositoryItemCheckEditIsEnabled = new RepositoryItemCheckEdit();
			this.repositoryItemTextEditDummy = new RepositoryItemTextEdit();
			this.repositoryItemTextEditIp = new RepositoryItemTextEdit();
			this.repositoryItemComboBox = new RepositoryItemComboBox();
			this.checkBoxUseDbParams = new CheckBox();
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.mGridSettings).BeginInit();
			((ISupportInitialize)this.mGridViewSettings).BeginInit();
			((ISupportInitialize)this.repositoryItemButtonEdit).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsEnabled).BeginInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).BeginInit();
			((ISupportInitialize)this.repositoryItemTextEditIp).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBox).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.mButtonOK, "mButtonOK");
			this.mButtonOK.DialogResult = DialogResult.OK;
			this.mButtonOK.Name = "mButtonOK";
			this.mButtonOK.UseVisualStyleBackColor = true;
			this.mButtonOK.Click += new EventHandler(this.mButtonOK_Click);
			componentResourceManager.ApplyResources(this.mButtonCancel, "mButtonCancel");
			this.mButtonCancel.DialogResult = DialogResult.Cancel;
			this.mButtonCancel.Name = "mButtonCancel";
			this.mButtonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mButtonHelp, "mButtonHelp");
			this.mButtonHelp.Name = "mButtonHelp";
			this.mButtonHelp.UseVisualStyleBackColor = true;
			this.mButtonHelp.Click += new EventHandler(this.mButtonHelp_Click);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.mGridSettings, "mGridSettings");
			this.mGridSettings.LookAndFeel.Style = LookAndFeelStyle.Style3D;
			this.mGridSettings.LookAndFeel.UseDefaultLookAndFeel = false;
			this.mGridSettings.LookAndFeel.UseWindowsXPTheme = true;
			this.mGridSettings.MainView = this.mGridViewSettings;
			this.mGridSettings.Name = "mGridSettings";
			this.mGridSettings.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemButtonEdit,
				this.repositoryItemCheckEditIsEnabled,
				this.repositoryItemTextEditDummy,
				this.repositoryItemTextEditIp,
				this.repositoryItemComboBox
			});
			this.mGridSettings.ViewCollection.AddRange(new BaseView[]
			{
				this.mGridViewSettings
			});
			this.mGridSettings.ProcessGridKey += new KeyEventHandler(this.mGridSettings_ProcessGridKey);
			this.mGridSettings.EditorKeyDown += new KeyEventHandler(this.mGridSettings_EditorKeyDown);
			this.mGridViewSettings.Columns.AddRange(new GridColumn[]
			{
				this.gridColumnParameter,
				this.gridColumnValue,
				this.gridColumnGroup
			});
			this.mGridViewSettings.GridControl = this.mGridSettings;
			this.mGridViewSettings.GroupCount = 1;
			this.mGridViewSettings.Name = "mGridViewSettings";
			this.mGridViewSettings.OptionsBehavior.AllowAddRows = DefaultBoolean.True;
			this.mGridViewSettings.OptionsBehavior.AutoExpandAllGroups = true;
			this.mGridViewSettings.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
			this.mGridViewSettings.OptionsBehavior.ImmediateUpdateRowPosition = false;
			this.mGridViewSettings.OptionsCustomization.AllowColumnMoving = false;
			this.mGridViewSettings.OptionsCustomization.AllowFilter = false;
			this.mGridViewSettings.OptionsCustomization.AllowGroup = false;
			this.mGridViewSettings.OptionsCustomization.AllowQuickHideColumns = false;
			this.mGridViewSettings.OptionsCustomization.AllowSort = false;
			this.mGridViewSettings.OptionsDetail.EnableMasterViewMode = false;
			this.mGridViewSettings.OptionsFilter.AllowFilterEditor = false;
			this.mGridViewSettings.OptionsFind.AllowFindPanel = false;
			this.mGridViewSettings.OptionsNavigation.UseTabKey = false;
			this.mGridViewSettings.OptionsView.ShowColumnHeaders = false;
			this.mGridViewSettings.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
			this.mGridViewSettings.OptionsView.ShowGroupPanel = false;
			this.mGridViewSettings.OptionsView.ShowIndicator = false;
			this.mGridViewSettings.PaintStyleName = "WindowsXP";
			this.mGridViewSettings.RowHeight = 20;
			this.mGridViewSettings.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.gridColumnGroup, ColumnSortOrder.Ascending)
			});
			this.mGridViewSettings.CustomDrawCell += new RowCellCustomDrawEventHandler(this.mGridViewSettings_CustomDrawCell);
			this.mGridViewSettings.CustomDrawGroupRow += new RowObjectCustomDrawEventHandler(this.mGridViewSettings_CustomDrawGroupRow);
			this.mGridViewSettings.CustomRowCellEdit += new CustomRowCellEditEventHandler(this.mGridViewSettings_CustomRowCellEdit);
			this.mGridViewSettings.TopRowChanged += new EventHandler(this.mGridViewSettings_TopRowChanged);
			this.mGridViewSettings.PopupMenuShowing += new PopupMenuShowingEventHandler(this.mGridViewSettings_PopupMenuShowing);
			this.mGridViewSettings.ShowingEditor += new CancelEventHandler(this.mGridViewSettings_ShowingEditor);
			this.mGridViewSettings.HiddenEditor += new EventHandler(this.mGridViewSettings_HiddenEditor);
			this.mGridViewSettings.ShownEditor += new EventHandler(this.mGridViewSettings_ShownEditor);
			this.mGridViewSettings.FocusedRowChanged += new FocusedRowChangedEventHandler(this.mGridViewSettings_FocusedRowChanged);
			this.mGridViewSettings.FocusedColumnChanged += new FocusedColumnChangedEventHandler(this.mGridViewSettings_FocusedColumnChanged);
			this.mGridViewSettings.MouseDown += new MouseEventHandler(this.mGridViewSettings_MouseDown);
			componentResourceManager.ApplyResources(this.gridColumnParameter, "gridColumnParameter");
			this.gridColumnParameter.FieldName = "Name";
			this.gridColumnParameter.Name = "gridColumnParameter";
			this.gridColumnParameter.OptionsColumn.AllowEdit = false;
			this.gridColumnParameter.OptionsColumn.ReadOnly = true;
			componentResourceManager.ApplyResources(this.gridColumnValue, "gridColumnValue");
			this.gridColumnValue.FieldName = "Value";
			this.gridColumnValue.Name = "gridColumnValue";
			componentResourceManager.ApplyResources(this.gridColumnGroup, "gridColumnGroup");
			this.gridColumnGroup.FieldName = "Group";
			this.gridColumnGroup.Name = "gridColumnGroup";
			componentResourceManager.ApplyResources(this.repositoryItemButtonEdit, "repositoryItemButtonEdit");
			this.repositoryItemButtonEdit.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemButtonEdit.Name = "repositoryItemButtonEdit";
			this.repositoryItemButtonEdit.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemButtonEdit.ButtonClick += new ButtonPressedEventHandler(this.repositoryItemButtonEdit_ButtonClick);
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditIsEnabled, "repositoryItemCheckEditIsEnabled");
			this.repositoryItemCheckEditIsEnabled.Name = "repositoryItemCheckEditIsEnabled";
			componentResourceManager.ApplyResources(this.repositoryItemTextEditDummy, "repositoryItemTextEditDummy");
			this.repositoryItemTextEditDummy.Name = "repositoryItemTextEditDummy";
			componentResourceManager.ApplyResources(this.repositoryItemTextEditIp, "repositoryItemTextEditIp");
			this.repositoryItemTextEditIp.Mask.EditMask = componentResourceManager.GetString("repositoryItemTextEditIp.Mask.EditMask");
			this.repositoryItemTextEditIp.Mask.PlaceHolder = (char)componentResourceManager.GetObject("repositoryItemTextEditIp.Mask.PlaceHolder");
			this.repositoryItemTextEditIp.Name = "repositoryItemTextEditIp";
			componentResourceManager.ApplyResources(this.repositoryItemComboBox, "repositoryItemComboBox");
			this.repositoryItemComboBox.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemComboBox.Buttons"))
			});
			this.repositoryItemComboBox.Name = "repositoryItemComboBox";
			this.repositoryItemComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemComboBox.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBox_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxUseDbParams, "checkBoxUseDbParams");
			this.checkBoxUseDbParams.Name = "checkBoxUseDbParams";
			this.checkBoxUseDbParams.UseVisualStyleBackColor = true;
			this.checkBoxUseDbParams.CheckedChanged += new EventHandler(this.checkBoxUseDbParams_CheckedChanged);
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AcceptButton = this.mButtonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.mButtonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.checkBoxUseDbParams);
			base.Controls.Add(this.mGridSettings);
			base.Controls.Add(this.mButtonOK);
			base.Controls.Add(this.mButtonCancel);
			base.Controls.Add(this.mButtonHelp);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ProtocolSettingsDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.Load += new EventHandler(this.ProtocolSettingsDialog_Load);
			base.Shown += new EventHandler(this.ProtocolSettingsDialog_Shown);
			base.HelpRequested += new HelpEventHandler(this.ProtocolSettingsDialog_HelpRequested);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.mGridSettings).EndInit();
			((ISupportInitialize)this.mGridViewSettings).EndInit();
			((ISupportInitialize)this.repositoryItemButtonEdit).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsEnabled).EndInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).EndInit();
			((ISupportInitialize)this.repositoryItemTextEditIp).EndInit();
			((ISupportInitialize)this.repositoryItemComboBox).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
