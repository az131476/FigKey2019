using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.PopupList.Data;
using Vector.PopupList.Editors;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.EventConditions
{
	internal class SymbolicSignalCondition : Form
	{
		internal class SignalPopupListDataProvider : PopupListDataProvider
		{
			private List<PopupListGroup> mCommandGroups = new List<PopupListGroup>();

			public SignalPopupListDataProvider(CcpXcpSignalConfiguration signalConfiguration)
			{
				if (signalConfiguration == null)
				{
					return;
				}
				using (IEnumerator<CcpXcpSignal> enumerator = signalConfiguration.ActiveSignals.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CcpXcpSignal signal = enumerator.Current;
						if (CcpXcpManager.Instance().IsCcpXcpTriggerSignalValid(signal))
						{
							if (!(from PopupListGroup gr in this.mCommandGroups
							where string.Compare(gr.Text, signal.EcuName.Value, true) == 0
							select gr).Any<PopupListGroup>())
							{
								this.mCommandGroups.Add(new PopupListGroup(signal.EcuName.Value));
							}
							foreach (PopupListGroup current in this.mCommandGroups)
							{
								if (string.Compare(current.Text, signal.EcuName.Value, true) == 0)
								{
									current.Items.Add(new SymbolicSignalCondition.SignalPopupListItem(signal.Name.Value, signal));
									break;
								}
							}
						}
					}
				}
				base.Groups.AddRange(this.mCommandGroups.ToArray());
			}

			protected override void OnItemToolTipTextNeeded(PopupListItemToolTipTextNeededEventArgs e)
			{
				base.OnItemToolTipTextNeeded(e);
				SymbolicSignalCondition.SignalPopupListItem signalPopupListItem = e.Item as SymbolicSignalCondition.SignalPopupListItem;
				if (signalPopupListItem != null && signalPopupListItem.Signal != null && signalPopupListItem.Signal.EcuName != null && signalPopupListItem.Signal.Name != null)
				{
					e.ToolTipText = string.Format(Resources.SymbolicSignalName, signalPopupListItem.Signal.EcuName, signalPopupListItem.Signal.Name);
				}
			}
		}

		internal class SignalPopupListItem : PopupListItem
		{
			public CcpXcpSignal Signal
			{
				get;
				private set;
			}

			public SignalPopupListItem(string text, CcpXcpSignal signal) : base(text)
			{
				this.Signal = signal;
			}
		}

		internal class DiagnosticSignalPopupListDataProvider : PopupListDataProvider
		{
			private List<PopupListGroup> mCommandGroups = new List<PopupListGroup>();

			private IModelValidator modelValidator;

			public DiagnosticSignalPopupListDataProvider(DiagnosticActionsConfiguration signalConfiguration, IModelValidator modelValidator)
			{
				if (signalConfiguration == null || modelValidator == null)
				{
					return;
				}
				this.modelValidator = modelValidator;
				using (IEnumerator<DiagnosticAction> enumerator = signalConfiguration.DiagnosticActions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DiagnosticAction signal = enumerator.Current;
						if (this.IsDiagnosticTriggerSignalValid(signal))
						{
							if (!(from PopupListGroup gr in this.mCommandGroups
							where string.Compare(gr.Text, signal.EcuQualifier.Value, true) == 0
							select gr).Any<PopupListGroup>())
							{
								this.mCommandGroups.Add(new PopupListGroup(signal.EcuQualifier.Value));
							}
							foreach (PopupListGroup current in this.mCommandGroups)
							{
								if (string.Compare(current.Text, signal.EcuQualifier.Value, true) == 0)
								{
									DiagnosticSignalRequest diagnosticSignalRequest = signal as DiagnosticSignalRequest;
									if (diagnosticSignalRequest != null)
									{
										string value = diagnosticSignalRequest.SignalQualifier.Value;
										current.Items.Add(new SymbolicSignalCondition.DiagnosticSignalPopupListItem(value, signal));
										break;
									}
								}
							}
						}
					}
				}
				base.Groups.AddRange(this.mCommandGroups.ToArray());
			}

			private bool IsDiagnosticTriggerSignalValid(DiagnosticAction signal)
			{
				if (!(signal is DiagnosticSignalRequest))
				{
					return false;
				}
				DiagnosticSignalRequest diagnosticSignalRequest = signal as DiagnosticSignalRequest;
				SignalDefinition signalDefinition = null;
				string variantQualifier;
				if (this.modelValidator.IsDiagECUConfigured(signal.DatabasePath.Value, signal.EcuQualifier.Value, out variantQualifier) && !DiagSymbolsManager.Instance().GetSignalDefinition(diagnosticSignalRequest.EcuQualifier.Value, variantQualifier, diagnosticSignalRequest.DidId.Value, diagnosticSignalRequest.SignalQualifier.Value, out signalDefinition))
				{
					signalDefinition = null;
				}
				bool result = false;
				if (signalDefinition != null)
				{
					result = true;
				}
				return result;
			}

			protected override void OnItemToolTipTextNeeded(PopupListItemToolTipTextNeededEventArgs e)
			{
				base.OnItemToolTipTextNeeded(e);
				SymbolicSignalCondition.DiagnosticSignalPopupListItem diagnosticSignalPopupListItem = e.Item as SymbolicSignalCondition.DiagnosticSignalPopupListItem;
				if (diagnosticSignalPopupListItem != null && diagnosticSignalPopupListItem.Signal != null && diagnosticSignalPopupListItem.Signal.EcuQualifier != null && diagnosticSignalPopupListItem.Signal.ServiceQualifier != null)
				{
					e.ToolTipText = string.Format(Resources.SymbolicSignalName, diagnosticSignalPopupListItem.Signal.EcuQualifier, diagnosticSignalPopupListItem.Signal.ServiceQualifier);
				}
				string parameterDisplayStrings = this.GetParameterDisplayStrings(diagnosticSignalPopupListItem.Signal);
				if (!string.IsNullOrEmpty(parameterDisplayStrings))
				{
					e.ToolTipText = e.ToolTipText + Environment.NewLine + parameterDisplayStrings;
				}
			}

			private string GetParameterDisplayStrings(DiagnosticAction action)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string variantQualifier;
				IList<KeyValuePair<string, string>> list;
				if (this.modelValidator.IsDiagECUConfigured(action.DatabasePath.Value, action.EcuQualifier.Value, out variantQualifier) && DiagSymbolsManager.Instance().GetDisassembledMessageParams(this.modelValidator.GetAbsoluteFilePath(action.DatabasePath.Value), action.EcuQualifier.Value, variantQualifier, action.ServiceQualifier.Value, action.MessageData.Value, out list))
				{
					if (action.HasOnlyConstParams)
					{
						stringBuilder.Append(Resources.ConstantParams);
					}
					int num = 0;
					foreach (KeyValuePair<string, string> current in list)
					{
						if (num != 0)
						{
							if (num > 1)
							{
								stringBuilder.Append("\n");
							}
							stringBuilder.AppendFormat("{0} = {1}", current.Key, current.Value);
						}
						num++;
					}
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		internal class DiagnosticSignalPopupListItem : PopupListItem
		{
			public DiagnosticAction Signal
			{
				get;
				private set;
			}

			public DiagnosticSignalPopupListItem(string text, DiagnosticAction signal) : base(text)
			{
				this.Signal = signal;
			}
		}

		private ISymbolicSignalEvent sigEvent;

		private IModelValidator modelValidator;

		private IApplicationDatabaseManager databaseManager;

		private SignalDefinition signalDefinition;

		private List<CondRelation> hiddenRelations;

		private CcpXcpSignalConfiguration ccpXcpSignalConfiguration;

		private DiagnosticActionsConfiguration diagnosticActionsConfiguration;

		private SignalValueInputMode inputMode;

		private Dictionary<string, double> textEncodedToRawValue;

		private Dictionary<double, string> rawToTextEncodedValue;

		private int invalidChannelIndex;

		private IContainer components;

		private Label labelSignalName;

		private TextBox textBoxSignalName;

		private Button buttonSelectSignal;

		private ComboBox comboBoxConditionType;

		private Label labelConditionType;

		private Label labelValue;

		private TextBox textBoxLowValue;

		private Label labelHighLimitValue;

		private TextBox textBoxHighValue;

		private Button buttonOK;

		private Button buttonCancel;

		private ErrorProvider errorProviderFormat;

		private TextBox textBoxHighValuePhysical;

		private TextBox textBoxLowValuePhysical;

		private RichTextBox richTextBoxInfoPhys;

		private ErrorProvider errorProviderGlobalModel;

		private RichTextBox richTextBoxInfoRaw;

		private Button buttonHelp;

		private ComboBox comboBoxChannel;

		private Label labelChannel;

		private ToolTip toolTip;

		private Label labelHint;

		private PictureBox iconHint;

		private PopupListEdit popupListEditSignals;

		private Label labelSymbolic;

		private ComboBox comboBoxHighValueSymbolic;

		private ComboBox comboBoxLowValueSymbolic;

		private Label labelPhysical;

		private Label labelRaw;

		public ISymbolicSignalEvent SignalEvent
		{
			get
			{
				return this.sigEvent;
			}
			set
			{
				this.sigEvent = value;
				if (this.sigEvent is CcpXcpSignalEvent)
				{
					this.signalDefinition = CcpXcpManager.Instance().GetSignalDefinition(this.sigEvent.SignalName.Value, this.sigEvent.CcpXcpEcuName.Value);
					if ((this.sigEvent.BusType == null || this.sigEvent.BusType.Value == BusType.Bt_None || this.sigEvent.BusType.Value == BusType.Bt_Wildcard) && this.sigEvent.CcpXcpEcuName != null)
					{
						this.sigEvent.BusType = new ValidatedProperty<BusType>(CcpXcpManager.Instance().GetBusType(this.sigEvent.CcpXcpEcuName.Value));
					}
					if ((this.sigEvent.ChannelNumber == null || this.sigEvent.ChannelNumber.Value == 0u) && this.sigEvent.CcpXcpEcuName != null)
					{
						this.sigEvent.ChannelNumber = new ValidatedProperty<uint>(CcpXcpManager.Instance().GetChannelNumber(this.sigEvent.CcpXcpEcuName.Value));
					}
				}
				else if (this.sigEvent is DiagnosticSignalEvent)
				{
					DiagnosticSignalEvent diagnosticSignalEvent = this.sigEvent as DiagnosticSignalEvent;
					DiagnosticsDatabase diagnosticsDatabase;
					DiagnosticsECU diagnosticsECU;
					if (DiagSymbolsManager.Instance().GetSignalDefinition(diagnosticSignalEvent.DiagnosticEcuName.Value, diagnosticSignalEvent.DiagnosticVariant.Value, diagnosticSignalEvent.DiagnosticDid.Value, diagnosticSignalEvent.SignalName.Value, out this.signalDefinition) && this.DiagnosticsDatabaseConfiguration.TryGetDiagnosticsDatabase(diagnosticSignalEvent.DatabasePath.Value, out diagnosticsDatabase) && diagnosticsDatabase.TryGetECU(diagnosticSignalEvent.DiagnosticEcuName.Value, out diagnosticsECU))
					{
						if (diagnosticsECU.BusType != null && (diagnosticSignalEvent.BusType == null || diagnosticSignalEvent.BusType.Value != diagnosticsECU.BusType.Value))
						{
							this.sigEvent.BusType = new ValidatedProperty<BusType>(diagnosticsECU.BusType.Value);
						}
						if (diagnosticsECU.ChannelNumber != null && (diagnosticSignalEvent.ChannelNumber == null || diagnosticSignalEvent.ChannelNumber.Value != diagnosticsECU.ChannelNumber.Value))
						{
							this.sigEvent.ChannelNumber = new ValidatedProperty<uint>(diagnosticsECU.ChannelNumber.Value);
						}
					}
				}
				this.ApplyEventTypeToControls();
				if (this.popupListEditSignals.Properties.DataProvider != null)
				{
					this.popupListEditSignals.Text = this.sigEvent.SignalName.Value;
				}
			}
		}

		public CcpXcpSignalConfiguration CcpXcpSignalConfiguration
		{
			get
			{
				return this.ccpXcpSignalConfiguration;
			}
			set
			{
				this.ccpXcpSignalConfiguration = value;
				this.popupListEditSignals.Properties.DataProvider = new SymbolicSignalCondition.SignalPopupListDataProvider(this.ccpXcpSignalConfiguration);
				if (this.sigEvent != null)
				{
					this.popupListEditSignals.Text = this.sigEvent.SignalName.Value;
				}
			}
		}

		public DiagnosticActionsConfiguration DiagnosticActionsConfiguration
		{
			get
			{
				return this.diagnosticActionsConfiguration;
			}
			set
			{
				this.diagnosticActionsConfiguration = value;
				this.popupListEditSignals.Properties.DataProvider = new SymbolicSignalCondition.DiagnosticSignalPopupListDataProvider(this.diagnosticActionsConfiguration, this.modelValidator);
				if (this.sigEvent != null)
				{
					this.popupListEditSignals.Text = this.sigEvent.SignalName.Value;
				}
			}
		}

		public DiagnosticsDatabaseConfiguration DiagnosticsDatabaseConfiguration
		{
			get;
			set;
		}

		public SymbolicSignalCondition(IModelValidator modelVal, IApplicationDatabaseManager manager, List<CondRelation> relationsToHide)
		{
			this.modelValidator = modelVal;
			this.databaseManager = manager;
			this.signalDefinition = null;
			this.hiddenRelations = relationsToHide;
			this.InitializeComponent();
			this.InitRelationComboBox();
			this.popupListEditSignals.Properties.ShowCustomButton = false;
			this.popupListEditSignals.Properties.ShowItemSelectionButton = false;
			this.iconHint.Image = Resources.IconInfo.ToBitmap();
			this.textEncodedToRawValue = new Dictionary<string, double>();
			this.inputMode = SignalValueInputMode.Raw;
			this.invalidChannelIndex = -1;
		}

		public void ResetToDefaults()
		{
			this.sigEvent.SignalName.Value = "";
			this.sigEvent.MessageName.Value = "";
			this.sigEvent.DatabaseName.Value = "";
			this.sigEvent.DatabasePath.Value = "";
			this.sigEvent.ChannelNumber.Value = 1u;
			this.sigEvent.BusType.Value = BusType.Bt_CAN;
			this.sigEvent.LowValue.Value = 0.0;
			this.sigEvent.HighValue.Value = 0.0;
			this.sigEvent.Relation.Value = CondRelation.Equal;
			this.signalDefinition = null;
			this.inputMode = SignalValueInputMode.Raw;
			this.ApplyValueToControls();
		}

		private void ApplyValueToControls()
		{
			this.ResetErrorProvider();
			this.SubscribeControlEvents(false);
			this.ApplyChannelNumberToControl();
			this.ValidateChannel();
			this.comboBoxConditionType.SelectedItem = GUIUtil.MapTriggerConditionRelation2String(this.sigEvent.Relation.Value);
			bool flag = true;
			if (this.sigEvent is SymbolicSignalEvent)
			{
				if (!this.modelValidator.DatabaseServices.IsSymbolicSignalDefined(this.sigEvent.DatabasePath.Value, this.sigEvent.NetworkName.Value, this.sigEvent.MessageName.Value, this.sigEvent.SignalName.Value, this.sigEvent.ChannelNumber.Value, this.sigEvent.BusType.Value, out this.signalDefinition))
				{
					this.errorProviderGlobalModel.SetError(this.textBoxSignalName, Resources.ErrorUnresolvedSigSymbol);
					flag = false;
				}
				else if (BusType.Bt_FlexRay == this.sigEvent.BusType.Value && !this.modelValidator.ValidateSymbolicMessageChannelFromFlexrayDb(this.sigEvent.DatabasePath.Value, this.sigEvent.NetworkName.Value, this.sigEvent.MessageName.Value, this.sigEvent.ChannelNumber.Value))
				{
					this.errorProviderGlobalModel.SetError(this.textBoxSignalName, Resources.ErrorUnresolvedMsgSymbolAtChn);
					flag = false;
				}
			}
			else if (this.sigEvent is DiagnosticSignalEvent)
			{
				DiagnosticSignalEvent diagnosticSignalEvent = this.sigEvent as DiagnosticSignalEvent;
				if (!DiagSymbolsManager.Instance().GetSignalDefinition(diagnosticSignalEvent.DiagnosticEcuName.Value, diagnosticSignalEvent.DiagnosticVariant.Value, diagnosticSignalEvent.DiagnosticDid.Value, diagnosticSignalEvent.SignalName.Value, out this.signalDefinition))
				{
					this.signalDefinition = null;
					flag = false;
				}
			}
			else
			{
				this.signalDefinition = CcpXcpManager.Instance().GetSignalDefinition(this.sigEvent.SignalName.Value, this.sigEvent.CcpXcpEcuName.Value);
				if (this.signalDefinition == null)
				{
					flag = false;
				}
			}
			this.InitSymbolicValueComboboxes();
			if (flag)
			{
				this.textBoxLowValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.LowValue.Value, this.signalDefinition);
				this.textBoxHighValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.HighValue.Value, this.signalDefinition);
				double physicalValue = this.GetPhysicalValue(this.sigEvent.LowValue.Value);
				this.textBoxLowValuePhysical.Text = this.signalDefinition.PhysicalValueToString(physicalValue);
				double physicalValue2 = this.GetPhysicalValue(this.sigEvent.HighValue.Value);
				this.textBoxHighValuePhysical.Text = this.signalDefinition.PhysicalValueToString(physicalValue2);
				this.SelectTextEncodingForRawValue(this.comboBoxLowValueSymbolic, this.sigEvent.LowValue.Value);
				this.SelectTextEncodingForRawValue(this.comboBoxHighValueSymbolic, this.sigEvent.HighValue.Value);
			}
			else if (this.sigEvent is SymbolicSignalEvent)
			{
				SignalDefinition signalDefinition = new SignalDefinition();
				signalDefinition.SetDummySignal();
				this.textBoxLowValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.LowValue.Value, signalDefinition);
				this.textBoxHighValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.HighValue.Value, signalDefinition);
				this.textBoxLowValuePhysical.Text = Resources.Unknown;
				this.textBoxHighValuePhysical.Text = Resources.Unknown;
				this.signalDefinition = null;
			}
			else
			{
				SignalDefinition signalDefinition2 = new SignalDefinition();
				signalDefinition2.SetSignal(null, 0u, 16u, false, false, true, "", 1.0, 0.0, false, 0, false, false);
				this.textBoxLowValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.LowValue.Value, signalDefinition2);
				this.textBoxHighValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.HighValue.Value, signalDefinition2);
				this.textBoxLowValuePhysical.Text = Resources.Unknown;
				this.textBoxHighValuePhysical.Text = Resources.Unknown;
				this.signalDefinition = null;
			}
			this.DisplayFullSignalInfo();
			this.SubscribeControlEvents(true);
			this.EnableValueControls();
		}

		private void ApplyChannelNumberToControl()
		{
			switch (this.sigEvent.BusType.Value)
			{
			case BusType.Bt_CAN:
				this.comboBoxChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.sigEvent.ChannelNumber.Value);
				return;
			case BusType.Bt_LIN:
			{
				string text = GUIUtil.MapLINChannelNumber2String(this.sigEvent.ChannelNumber.Value, this.modelValidator.LoggerSpecifics);
				if (this.comboBoxChannel.Items.Contains(text))
				{
					this.comboBoxChannel.SelectedItem = text;
					return;
				}
				this.invalidChannelIndex = this.comboBoxChannel.Items.Add(text);
				this.comboBoxChannel.SelectedIndex = this.invalidChannelIndex;
				return;
			}
			case BusType.Bt_FlexRay:
				this.comboBoxChannel.SelectedItem = GUIUtil.MapFlexrayChannelNumber2String(this.sigEvent.ChannelNumber.Value);
				return;
			case BusType.Bt_Ethernet:
				this.comboBoxChannel.SelectedItem = GUIUtil.MapEthernetChannelNumber2String(this.sigEvent.ChannelNumber.Value);
				return;
			}
			SymbolicSignalEvent arg_10D_0 = this.sigEvent as SymbolicSignalEvent;
		}

		private void ApplyEventTypeToControls()
		{
			if (this.sigEvent is CcpXcpSignalEvent)
			{
				this.Text = Resources_Trigger.TriggerTypeNameColCcpXcpSignal;
				this.comboBoxChannel.Enabled = false;
				this.popupListEditSignals.Visible = true;
				this.textBoxSignalName.Visible = false;
				this.buttonSelectSignal.Visible = false;
				return;
			}
			if (this.sigEvent is DiagnosticSignalEvent)
			{
				this.Text = Resources_Trigger.TriggerTypeNameColDiagnosticSignal;
				this.comboBoxChannel.Enabled = false;
				this.popupListEditSignals.Visible = true;
				this.textBoxSignalName.Visible = false;
				this.buttonSelectSignal.Visible = false;
				return;
			}
			this.Text = Resources.SymbolicSignalConditionDlgCaption;
			this.comboBoxChannel.Enabled = true;
			this.popupListEditSignals.Visible = false;
			this.textBoxSignalName.Visible = true;
			this.buttonSelectSignal.Visible = true;
		}

		private void InitChannelComboBox()
		{
			this.comboBoxChannel.SelectedIndexChanged -= new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			this.comboBoxChannel.Items.Clear();
			this.invalidChannelIndex = -1;
			uint totalNumberOfLogicalChannels = this.modelValidator.GetTotalNumberOfLogicalChannels(this.sigEvent.BusType.Value);
			switch (this.sigEvent.BusType.Value)
			{
			case BusType.Bt_CAN:
				for (uint num = 1u; num <= totalNumberOfLogicalChannels; num += 1u)
				{
					this.comboBoxChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
				}
				if (this.sigEvent.ChannelNumber.Value <= totalNumberOfLogicalChannels)
				{
					this.comboBoxChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.sigEvent.ChannelNumber.Value);
					goto IL_2B0;
				}
				this.comboBoxChannel.SelectedIndex = 0;
				goto IL_2B0;
			case BusType.Bt_LIN:
				for (uint num2 = 1u; num2 <= totalNumberOfLogicalChannels; num2 += 1u)
				{
					this.comboBoxChannel.Items.Add(GUIUtil.MapLINChannelNumber2String(num2, this.modelValidator.LoggerSpecifics));
				}
				if (this.sigEvent.ChannelNumber.Value <= totalNumberOfLogicalChannels)
				{
					this.comboBoxChannel.SelectedItem = GUIUtil.MapLINChannelNumber2String(this.sigEvent.ChannelNumber.Value, this.modelValidator.LoggerSpecifics);
					goto IL_2B0;
				}
				this.comboBoxChannel.SelectedIndex = 0;
				goto IL_2B0;
			case BusType.Bt_FlexRay:
				for (uint num3 = 1u; num3 <= totalNumberOfLogicalChannels; num3 += 1u)
				{
					this.comboBoxChannel.Items.Add(GUIUtil.MapFlexrayChannelNumber2String(num3));
				}
				if (this.sigEvent.ChannelNumber.Value <= totalNumberOfLogicalChannels)
				{
					this.comboBoxChannel.Items.Add(GUIUtil.MapFlexrayChannelNumber2String(Database.ChannelNumber_FlexrayAB));
				}
				if (this.sigEvent.ChannelNumber.Value <= this.modelValidator.LoggerSpecifics.Flexray.NumberOfChannels || this.sigEvent.ChannelNumber.Value == Database.ChannelNumber_FlexrayAB)
				{
					this.comboBoxChannel.SelectedItem = GUIUtil.MapFlexrayChannelNumber2String(this.sigEvent.ChannelNumber.Value);
					goto IL_2B0;
				}
				this.comboBoxChannel.SelectedIndex = 0;
				goto IL_2B0;
			case BusType.Bt_Ethernet:
				if (this.modelValidator.LoggerSpecifics.Recording.HasEthernet)
				{
					this.comboBoxChannel.Items.Add(GUIUtil.MapEthernetChannelNumber2String(1u));
					this.comboBoxChannel.SelectedItem = GUIUtil.MapEthernetChannelNumber2String(1u);
					goto IL_2B0;
				}
				this.comboBoxChannel.SelectedIndex = 0;
				goto IL_2B0;
			}
			SymbolicSignalEvent arg_2AF_0 = this.sigEvent as SymbolicSignalEvent;
			IL_2B0:
			this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
		}

		private void InitRelationComboBox()
		{
			this.comboBoxConditionType.SelectedIndexChanged -= new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
			this.comboBoxConditionType.Items.Clear();
			foreach (CondRelation condRelation in Enum.GetValues(typeof(CondRelation)))
			{
				if (this.hiddenRelations == null || !this.hiddenRelations.Contains(condRelation))
				{
					this.comboBoxConditionType.Items.Add(GUIUtil.MapTriggerConditionRelation2String(condRelation));
				}
			}
			if (this.comboBoxConditionType.Items.Count > 0)
			{
				this.comboBoxConditionType.SelectedIndex = 0;
			}
			this.comboBoxConditionType.SelectedIndexChanged += new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
		}

		private void InitSymbolicValueComboboxes()
		{
			this.comboBoxLowValueSymbolic.Items.Clear();
			this.comboBoxHighValueSymbolic.Items.Clear();
			this.textEncodedToRawValue.Clear();
			if (this.signalDefinition != null && this.databaseManager.GetTextEncodedSignalValueTable(this.modelValidator.GetAbsoluteFilePath(this.sigEvent.DatabasePath.Value), this.sigEvent.NetworkName.Value, this.sigEvent.MessageName.Value, this.sigEvent.SignalName.Value, out this.rawToTextEncodedValue))
			{
				int num = 0;
				foreach (double num2 in this.rawToTextEncodedValue.Keys)
				{
					string text = this.rawToTextEncodedValue[num2];
					if (!this.textEncodedToRawValue.ContainsKey(text))
					{
						this.comboBoxLowValueSymbolic.Items.Add(text);
						this.comboBoxHighValueSymbolic.Items.Add(text);
						this.textEncodedToRawValue.Add(text, num2);
						int width = TextRenderer.MeasureText(text, this.comboBoxLowValueSymbolic.Font).Width;
						if (width > num)
						{
							num = width;
						}
					}
				}
				if (num > this.comboBoxLowValueSymbolic.DropDownWidth)
				{
					int num3 = 0;
					if (this.comboBoxLowValueSymbolic.Items.Count > this.comboBoxLowValueSymbolic.MaxDropDownItems)
					{
						num3 = SystemInformation.VerticalScrollBarWidth;
					}
					this.comboBoxLowValueSymbolic.DropDownWidth = num + num3;
					this.comboBoxHighValueSymbolic.DropDownWidth = num + num3;
				}
			}
			if (this.comboBoxLowValueSymbolic.Items.Count > 0)
			{
				this.comboBoxLowValueSymbolic.Items.Add(string.Empty);
				this.comboBoxLowValueSymbolic.Enabled = true;
				this.comboBoxHighValueSymbolic.Items.Add(string.Empty);
				this.comboBoxHighValueSymbolic.Enabled = true;
				this.comboBoxLowValueSymbolic.SelectedIndex = 0;
				this.comboBoxHighValueSymbolic.SelectedIndex = 0;
				return;
			}
			this.comboBoxLowValueSymbolic.Enabled = false;
			this.comboBoxHighValueSymbolic.Enabled = false;
		}

		private bool SelectTextEncodingForRawValue(ComboBox box, double rawValue)
		{
			if (this.signalDefinition != null && this.signalDefinition.HasTextEncodedValueTable)
			{
				long num = Convert.ToInt64(rawValue);
				if (this.rawToTextEncodedValue.ContainsKey((double)num) && box.Items.Contains(this.rawToTextEncodedValue[(double)num]))
				{
					box.SelectedItem = this.rawToTextEncodedValue[(double)num];
					return true;
				}
			}
			box.SelectedItem = "";
			return false;
		}

		private void ResetErrorProvider()
		{
			this.errorProviderFormat.SetError(this.textBoxLowValue, "");
			this.errorProviderFormat.SetError(this.textBoxHighValue, "");
			this.errorProviderFormat.SetError(this.textBoxLowValuePhysical, "");
			this.errorProviderFormat.SetError(this.textBoxHighValuePhysical, "");
			this.errorProviderFormat.SetError(this.comboBoxLowValueSymbolic, "");
			this.errorProviderFormat.SetError(this.comboBoxHighValueSymbolic, "");
			this.errorProviderGlobalModel.SetError(this.textBoxSignalName, "");
		}

		private void SymbolicSignalCondition_Shown(object sender, EventArgs e)
		{
			this.inputMode = SignalValueInputMode.Raw;
			this.InitChannelComboBox();
			this.RenderLabelsForDisplayMode();
			this.ApplyValueToControls();
		}

		private void buttonSelectSignal_Click(object sender, EventArgs e)
		{
			this.inputMode = SignalValueInputMode.Raw;
			string value = this.sigEvent.MessageName.Value;
			string value2 = this.sigEvent.SignalName.Value;
			string value3 = this.sigEvent.DatabaseName.Value;
			string text = this.sigEvent.DatabasePath.Value;
			string value4 = this.sigEvent.NetworkName.Value;
			BusType value5 = this.sigEvent.BusType.Value;
			uint num = this.sigEvent.ChannelNumber.Value;
			bool value6 = this.sigEvent.IsFlexrayPDU.Value;
			if (this.databaseManager.SelectSignalInDatabase(ref value, ref value2, ref value3, ref text, ref value4, ref value5, ref value6))
			{
				string message;
				if (!this.modelValidator.DatabaseServices.IsSymbolicSignalInsertAllowed(value2, value, value4, text, value5, out message))
				{
					InformMessageBox.Error(message);
					return;
				}
				text = this.modelValidator.GetFilePathRelativeToConfiguration(text);
				bool flag = false;
				IList<uint> channelAssignmentOfDatabase = this.modelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(text, value4);
				if (value5 == BusType.Bt_FlexRay && channelAssignmentOfDatabase[0] == Database.ChannelNumber_FlexrayAB)
				{
					if (this.sigEvent.MessageName.Value.EndsWith(Constants.FlexrayChannelA_Postfix) && value.EndsWith(Constants.FlexrayChannelB_Postfix))
					{
						num = 2u;
						flag = true;
					}
					if (this.sigEvent.MessageName.Value.EndsWith(Constants.FlexrayChannelB_Postfix) && value.EndsWith(Constants.FlexrayChannelA_Postfix))
					{
						num = 1u;
						flag = true;
					}
				}
				else if (!channelAssignmentOfDatabase.Contains(num))
				{
					num = channelAssignmentOfDatabase[0];
					flag = true;
				}
				SignalDefinition def;
				this.databaseManager.ResolveSignalSymbolInDatabase(this.modelValidator.GetAbsoluteFilePath(text), value4, value, value2, out def);
				if (!this.SignalDefinitionIsValid(def))
				{
					return;
				}
				this.sigEvent.MessageName.Value = value;
				this.sigEvent.SignalName.Value = value2;
				this.sigEvent.DatabaseName.Value = value3;
				this.sigEvent.DatabasePath.Value = text;
				this.sigEvent.NetworkName.Value = value4;
				this.sigEvent.BusType.Value = value5;
				this.sigEvent.IsFlexrayPDU.Value = value6;
				this.sigEvent.ChannelNumber.Value = num;
				if (flag)
				{
					this.ApplyChannelNumberToControl();
				}
				this.signalDefinition = def;
				this.sigEvent.DatabasePath.Value = this.modelValidator.GetFilePathRelativeToConfiguration(this.sigEvent.DatabasePath.Value);
				if (!string.IsNullOrEmpty(this.errorProviderGlobalModel.GetError(this.textBoxSignalName)))
				{
					this.errorProviderGlobalModel.SetError(this.textBoxSignalName, "");
				}
				if (this.sigEvent.Relation.Value != CondRelation.InRange && this.sigEvent.Relation.Value != CondRelation.NotInRange)
				{
					double physicalValue = this.GetPhysicalValue(this.sigEvent.HighValue.Value);
					this.textBoxHighValuePhysical.Text = this.signalDefinition.PhysicalValueToString(physicalValue);
				}
				this.InitSymbolicValueComboboxes();
				this.DisplayFullSignalInfo();
				this.ValidateInput();
				this.EnableValueControls();
			}
		}

		private void popupListEditSignals_StandaloneValueAccepted(object sender, EventArgs e)
		{
			PopupListEdit popupListEdit = sender as PopupListEdit;
			if (popupListEdit != null && popupListEdit.EditValue != null)
			{
				PopupListEditValue popupListEditValue = popupListEdit.EditValue as PopupListEditValue;
				if (popupListEditValue != null)
				{
					SymbolicSignalCondition.SignalPopupListItem signalPopupListItem = popupListEditValue.Item as SymbolicSignalCondition.SignalPopupListItem;
					if (signalPopupListItem != null)
					{
						this.ApplyCcpXcpSignalToControls(signalPopupListItem.Signal);
						return;
					}
					SymbolicSignalCondition.DiagnosticSignalPopupListItem diagnosticSignalPopupListItem = popupListEditValue.Item as SymbolicSignalCondition.DiagnosticSignalPopupListItem;
					if (diagnosticSignalPopupListItem != null)
					{
						this.ApplyDiagnosticSignalToControls(diagnosticSignalPopupListItem.Signal);
						return;
					}
				}
			}
			this.popupListEditSignals.Text = this.sigEvent.SignalName.Value;
		}

		private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.ValidateChannel();
			if (this.invalidChannelIndex >= 0 && this.comboBoxChannel.SelectedIndex != this.invalidChannelIndex)
			{
				this.comboBoxChannel.Items.RemoveAt(this.invalidChannelIndex);
				this.invalidChannelIndex = -1;
			}
			this.ApplyValueToControls();
		}

		private void comboBoxConditionType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.inputMode = SignalValueInputMode.Raw;
			this.ValidateInput();
			this.EnableValueControls();
		}

		private void textBoxLowValue_Validating(object sender, CancelEventArgs e)
		{
			this.inputMode = SignalValueInputMode.Raw;
			this.ValidateInput();
		}

		private void textBoxHighValue_Validating(object sender, CancelEventArgs e)
		{
			this.inputMode = SignalValueInputMode.Raw;
			this.ValidateInput();
		}

		private void textBoxLowValuePhysical_Validating(object sender, CancelEventArgs e)
		{
			this.inputMode = SignalValueInputMode.Physical;
			this.ValidateInput();
		}

		private void textBoxHighValuePhysical_Validating(object sender, CancelEventArgs e)
		{
			this.inputMode = SignalValueInputMode.Physical;
			this.ValidateInput();
		}

		private void comboBoxLowValueSymbolic_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.comboBoxLowValueSymbolic.SelectedItem.ToString() == string.Empty)
			{
				this.comboBoxLowValueSymbolic.SelectedIndexChanged -= new EventHandler(this.comboBoxLowValueSymbolic_SelectedIndexChanged);
				this.SelectTextEncodingForRawValue(this.comboBoxLowValueSymbolic, this.sigEvent.LowValue.Value);
				this.comboBoxLowValueSymbolic.SelectedIndexChanged += new EventHandler(this.comboBoxLowValueSymbolic_SelectedIndexChanged);
				return;
			}
			this.inputMode = SignalValueInputMode.Symbolic;
			this.ValidateInput();
		}

		private void comboBoxHighValueSymbolic_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.comboBoxHighValueSymbolic.SelectedItem.ToString() == string.Empty)
			{
				this.comboBoxHighValueSymbolic.SelectedIndexChanged -= new EventHandler(this.comboBoxHighValueSymbolic_SelectedIndexChanged);
				this.SelectTextEncodingForRawValue(this.comboBoxHighValueSymbolic, this.sigEvent.HighValue.Value);
				this.comboBoxHighValueSymbolic.SelectedIndexChanged += new EventHandler(this.comboBoxHighValueSymbolic_SelectedIndexChanged);
			}
			this.inputMode = SignalValueInputMode.Symbolic;
			this.ValidateInput();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (!this.ValidateInput())
			{
				InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
				base.DialogResult = DialogResult.None;
				return;
			}
			if (!string.IsNullOrEmpty(this.errorProviderGlobalModel.GetError(this.comboBoxChannel)) && InformMessageBox.Question(Resources.InconsistSettingsCommitCondAnyway) == DialogResult.No)
			{
				return;
			}
			if ((CondRelation.InRange == this.sigEvent.Relation.Value || CondRelation.NotInRange == this.sigEvent.Relation.Value) && this.sigEvent.LowValue.Value > this.sigEvent.HighValue.Value)
			{
				double value = this.sigEvent.HighValue.Value;
				this.sigEvent.HighValue.Value = this.sigEvent.LowValue.Value;
				this.sigEvent.LowValue.Value = value;
			}
			base.DialogResult = DialogResult.OK;
		}

		private void textBoxSignalName_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(this.textBoxSignalName, this.textBoxSignalName.Text);
		}

		private void comboBoxLowValueSymbolic_MouseEnter(object sender, EventArgs e)
		{
			if (this.comboBoxLowValueSymbolic.SelectedItem == null)
			{
				return;
			}
			string text = this.comboBoxLowValueSymbolic.SelectedItem.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				this.toolTip.SetToolTip(this.comboBoxLowValueSymbolic, text);
			}
		}

		private void comboBoxHighValueSymbolic_MouseEnter(object sender, EventArgs e)
		{
			if (this.comboBoxHighValueSymbolic.SelectedItem == null)
			{
				return;
			}
			string text = this.comboBoxHighValueSymbolic.SelectedItem.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				this.toolTip.SetToolTip(this.comboBoxHighValueSymbolic, text);
			}
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void SymbolicSignalCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void ValidateChannel()
		{
			if (this.sigEvent is CcpXcpSignalEvent)
			{
				return;
			}
			if (this.sigEvent is DiagnosticSignalEvent)
			{
				return;
			}
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, "");
			switch (this.sigEvent.BusType.Value)
			{
			case BusType.Bt_CAN:
				this.sigEvent.ChannelNumber.Value = GUIUtil.MapCANChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
				break;
			case BusType.Bt_LIN:
				this.sigEvent.ChannelNumber.Value = GUIUtil.MapLINChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
				break;
			case BusType.Bt_FlexRay:
				this.sigEvent.ChannelNumber.Value = GUIUtil.MapFlexrayChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
				break;
			}
			if (!this.modelValidator.IsHardwareChannelAvailable(this.sigEvent.BusType.Value, this.sigEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelNotAvailable);
				return;
			}
			if (!this.modelValidator.IsHardwareChannelActive(this.sigEvent.BusType.Value, this.sigEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelInactive);
			}
		}

		private bool ValidateInput()
		{
			bool flag = true;
			double physicalValue = 0.0;
			double physicalValue2 = 0.0;
			double value = 0.0;
			double value2 = 0.0;
			bool flag2 = false;
			this.errorProviderFormat.SetError(this.comboBoxLowValueSymbolic, "");
			this.errorProviderFormat.SetError(this.comboBoxHighValueSymbolic, "");
			this.sigEvent.Relation.Value = GUIUtil.MapString2TriggerConditionRelation(this.comboBoxConditionType.SelectedItem.ToString());
			this.SubscribeControlEvents(false);
			if (this.inputMode == SignalValueInputMode.Physical)
			{
				this.ResetFormatErrorAndRestoreRawValueFromModel(this.textBoxLowValue, this.sigEvent.LowValue.Value);
				this.ResetFormatErrorAndRestoreRawValueFromModel(this.textBoxHighValue, this.sigEvent.HighValue.Value);
				if (this.sigEvent.Relation.Value == CondRelation.OnChange)
				{
					this.sigEvent.LowValue.Value = 0.0;
					if (!string.IsNullOrEmpty(this.errorProviderFormat.GetError(this.textBoxLowValuePhysical)))
					{
						this.errorProviderFormat.SetError(this.textBoxLowValuePhysical, "");
					}
					this.textBoxLowValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.LowValue.Value, this.signalDefinition);
					this.SelectTextEncodingForRawValue(this.comboBoxLowValueSymbolic, this.sigEvent.LowValue.Value);
					physicalValue = this.GetPhysicalValue(this.sigEvent.LowValue.Value);
					this.textBoxLowValuePhysical.Text = this.signalDefinition.PhysicalValueToString(physicalValue);
				}
				else if (!GUIUtil.DisplayStringToPhysicalValue(this.textBoxLowValuePhysical.Text, out physicalValue))
				{
					this.errorProviderFormat.SetError(this.textBoxLowValuePhysical, Resources.ErrorNumberExpected);
					flag = false;
				}
				else if (!this.GetRawValue(physicalValue, out value))
				{
					this.errorProviderFormat.SetError(this.textBoxLowValuePhysical, Resources.ErrorGenValueOutOfRange);
					flag = false;
				}
				else
				{
					this.sigEvent.LowValue.Value = value;
					this.textBoxLowValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.LowValue.Value, this.signalDefinition);
					this.SelectTextEncodingForRawValue(this.comboBoxLowValueSymbolic, this.sigEvent.LowValue.Value);
					physicalValue = this.GetPhysicalValue(this.sigEvent.LowValue.Value);
					this.textBoxLowValuePhysical.Text = this.signalDefinition.PhysicalValueToString(physicalValue);
					this.errorProviderFormat.SetError(this.textBoxLowValuePhysical, "");
				}
				if (this.sigEvent.Relation.Value == CondRelation.InRange || this.sigEvent.Relation.Value == CondRelation.NotInRange)
				{
					if (!GUIUtil.DisplayStringToPhysicalValue(this.textBoxHighValuePhysical.Text, out physicalValue2))
					{
						this.errorProviderFormat.SetError(this.textBoxHighValuePhysical, Resources.ErrorNumberExpected);
						flag = false;
					}
					else if (!this.GetRawValue(physicalValue2, out value2))
					{
						this.errorProviderFormat.SetError(this.textBoxHighValuePhysical, Resources.ErrorGenValueOutOfRange);
						flag = false;
					}
					else
					{
						this.sigEvent.HighValue.Value = value2;
						this.textBoxHighValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.HighValue.Value, this.signalDefinition);
						this.SelectTextEncodingForRawValue(this.comboBoxHighValueSymbolic, this.sigEvent.HighValue.Value);
						physicalValue2 = this.GetPhysicalValue(this.sigEvent.HighValue.Value);
						this.textBoxHighValuePhysical.Text = this.signalDefinition.PhysicalValueToString(physicalValue2);
						this.errorProviderFormat.SetError(this.textBoxHighValuePhysical, "");
					}
				}
				else
				{
					this.sigEvent.HighValue.Value = 0.0;
					if (!string.IsNullOrEmpty(this.errorProviderFormat.GetError(this.textBoxHighValuePhysical)))
					{
						this.errorProviderFormat.SetError(this.textBoxHighValuePhysical, "");
					}
					this.textBoxHighValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.HighValue.Value, this.signalDefinition);
					this.SelectTextEncodingForRawValue(this.comboBoxHighValueSymbolic, this.sigEvent.HighValue.Value);
					physicalValue2 = this.GetPhysicalValue(this.sigEvent.HighValue.Value);
					this.textBoxHighValuePhysical.Text = this.signalDefinition.PhysicalValueToString(physicalValue2);
				}
			}
			else if (this.inputMode == SignalValueInputMode.Raw)
			{
				this.ResetFormatErrorAndRestorePhysicalValueFromModel(this.textBoxLowValuePhysical, this.sigEvent.LowValue.Value);
				this.ResetFormatErrorAndRestorePhysicalValueFromModel(this.textBoxHighValuePhysical, this.sigEvent.HighValue.Value);
				if (this.sigEvent.Relation.Value == CondRelation.OnChange)
				{
					this.sigEvent.LowValue.Value = 0.0;
					if (!string.IsNullOrEmpty(this.errorProviderFormat.GetError(this.textBoxLowValue)))
					{
						this.errorProviderFormat.SetError(this.textBoxLowValue, "");
					}
					this.textBoxLowValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.LowValue.Value, this.signalDefinition);
					this.SelectTextEncodingForRawValue(this.comboBoxLowValueSymbolic, this.sigEvent.HighValue.Value);
					physicalValue = this.GetPhysicalValue(this.sigEvent.LowValue.Value);
					this.textBoxLowValuePhysical.Text = ((this.signalDefinition != null) ? this.signalDefinition.PhysicalValueToString(physicalValue) : string.Empty);
				}
				else if (!GUIUtil.DisplayStringToRawSignalValue(this.textBoxLowValue.Text, this.signalDefinition, out value, out flag2))
				{
					if (flag2)
					{
						this.errorProviderFormat.SetError(this.textBoxLowValue, Resources.ErrorGenValueOutOfRange);
					}
					else
					{
						this.errorProviderFormat.SetError(this.textBoxLowValue, Resources.ErrorNumberExpected);
					}
					flag = false;
				}
				else
				{
					this.sigEvent.LowValue.Value = value;
					this.SelectTextEncodingForRawValue(this.comboBoxLowValueSymbolic, this.sigEvent.LowValue.Value);
					this.errorProviderFormat.SetError(this.textBoxLowValue, "");
					if (this.signalDefinition == null)
					{
						this.textBoxLowValuePhysical.Text = Resources.Unknown;
					}
					else
					{
						physicalValue = this.GetPhysicalValue(this.sigEvent.LowValue.Value);
						this.textBoxLowValuePhysical.Text = this.signalDefinition.PhysicalValueToString(physicalValue);
					}
				}
				if (this.sigEvent.Relation.Value == CondRelation.InRange || this.sigEvent.Relation.Value == CondRelation.NotInRange)
				{
					if (!GUIUtil.DisplayStringToRawSignalValue(this.textBoxHighValue.Text, this.signalDefinition, out value2, out flag2))
					{
						if (flag2)
						{
							this.errorProviderFormat.SetError(this.textBoxHighValue, Resources.ErrorGenValueOutOfRange);
						}
						else
						{
							this.errorProviderFormat.SetError(this.textBoxHighValue, Resources.ErrorNumberExpected);
						}
						flag = false;
					}
					else
					{
						this.sigEvent.HighValue.Value = value2;
						this.SelectTextEncodingForRawValue(this.comboBoxHighValueSymbolic, this.sigEvent.HighValue.Value);
						this.errorProviderFormat.SetError(this.textBoxHighValue, "");
						if (this.signalDefinition == null)
						{
							this.textBoxHighValuePhysical.Text = Resources.Unknown;
						}
						else
						{
							physicalValue2 = this.GetPhysicalValue(this.sigEvent.HighValue.Value);
							this.textBoxHighValuePhysical.Text = this.signalDefinition.PhysicalValueToString(physicalValue2);
						}
					}
				}
				else
				{
					this.sigEvent.HighValue.Value = 0.0;
					if (!string.IsNullOrEmpty(this.errorProviderFormat.GetError(this.textBoxHighValue)))
					{
						this.errorProviderFormat.SetError(this.textBoxHighValue, "");
					}
					this.textBoxHighValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.HighValue.Value, this.signalDefinition);
					this.SelectTextEncodingForRawValue(this.comboBoxHighValueSymbolic, this.sigEvent.HighValue.Value);
					physicalValue2 = this.GetPhysicalValue(this.sigEvent.HighValue.Value);
					this.textBoxHighValuePhysical.Text = ((this.signalDefinition != null) ? this.signalDefinition.PhysicalValueToString(physicalValue2) : string.Empty);
				}
			}
			else if (this.inputMode == SignalValueInputMode.Symbolic)
			{
				this.ResetFormatErrorAndRestoreRawValueFromModel(this.textBoxLowValue, this.sigEvent.LowValue.Value);
				this.ResetFormatErrorAndRestoreRawValueFromModel(this.textBoxHighValue, this.sigEvent.HighValue.Value);
				this.ResetFormatErrorAndRestorePhysicalValueFromModel(this.textBoxLowValuePhysical, this.sigEvent.LowValue.Value);
				this.ResetFormatErrorAndRestorePhysicalValueFromModel(this.textBoxHighValuePhysical, this.sigEvent.HighValue.Value);
				if (this.sigEvent.Relation.Value == CondRelation.OnChange)
				{
					this.sigEvent.LowValue.Value = 0.0;
					this.textBoxLowValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.LowValue.Value, this.signalDefinition);
					this.SelectTextEncodingForRawValue(this.comboBoxLowValueSymbolic, this.sigEvent.LowValue.Value);
					physicalValue = this.GetPhysicalValue(this.sigEvent.LowValue.Value);
					this.textBoxLowValuePhysical.Text = this.signalDefinition.PhysicalValueToString(physicalValue);
				}
				else if (this.comboBoxLowValueSymbolic.SelectedItem != null && this.comboBoxLowValueSymbolic.SelectedItem.ToString() != string.Empty)
				{
					string text = this.comboBoxLowValueSymbolic.SelectedItem.ToString();
					if (this.textEncodedToRawValue.ContainsKey(text))
					{
						string b = string.Empty;
						if (this.rawToTextEncodedValue.ContainsKey(this.sigEvent.LowValue.Value))
						{
							b = this.rawToTextEncodedValue[this.sigEvent.LowValue.Value];
						}
						if (text != b)
						{
							this.sigEvent.LowValue.Value = this.textEncodedToRawValue[text];
						}
					}
					this.textBoxLowValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.LowValue.Value, this.signalDefinition);
					physicalValue = this.GetPhysicalValue(this.sigEvent.LowValue.Value);
					this.textBoxLowValuePhysical.Text = this.signalDefinition.PhysicalValueToString(physicalValue);
				}
				if (this.sigEvent.Relation.Value == CondRelation.InRange || this.sigEvent.Relation.Value == CondRelation.NotInRange)
				{
					if (this.comboBoxHighValueSymbolic.SelectedItem != null && this.comboBoxHighValueSymbolic.SelectedItem.ToString() != string.Empty)
					{
						string text2 = this.comboBoxHighValueSymbolic.SelectedItem.ToString();
						if (this.textEncodedToRawValue.ContainsKey(text2))
						{
							string b2 = string.Empty;
							if (this.rawToTextEncodedValue.ContainsKey(this.sigEvent.HighValue.Value))
							{
								b2 = this.rawToTextEncodedValue[this.sigEvent.HighValue.Value];
							}
							if (text2 != b2)
							{
								this.sigEvent.HighValue.Value = this.textEncodedToRawValue[text2];
							}
						}
						this.textBoxHighValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.HighValue.Value, this.signalDefinition);
						physicalValue2 = this.GetPhysicalValue(this.sigEvent.HighValue.Value);
						this.textBoxHighValuePhysical.Text = this.signalDefinition.PhysicalValueToString(physicalValue2);
					}
				}
				else
				{
					this.sigEvent.HighValue.Value = 0.0;
					this.textBoxHighValue.Text = GUIUtil.RawSignalValueToDisplayString(this.sigEvent.HighValue.Value, this.signalDefinition);
					this.SelectTextEncodingForRawValue(this.comboBoxHighValueSymbolic, this.sigEvent.HighValue.Value);
					physicalValue2 = this.GetPhysicalValue(this.sigEvent.HighValue.Value);
					this.textBoxHighValuePhysical.Text = this.signalDefinition.PhysicalValueToString(physicalValue2);
				}
			}
			if (flag && this.signalDefinition != null && ((this.sigEvent.Relation.Value == CondRelation.LessThan && this.sigEvent.LowValue.Value == 0.0 && !this.signalDefinition.IsSigned) || (this.sigEvent.Relation.Value == CondRelation.GreaterThan && this.sigEvent.LowValue.Value == (uint)(Math.Pow(2.0, this.signalDefinition.Length) - 1.0))))
			{
				this.errorProviderFormat.SetError(this.textBoxLowValuePhysical, Resources.ErrorInvalidValueWithRelOp);
				if (this.comboBoxLowValueSymbolic.Enabled && this.comboBoxLowValueSymbolic.SelectedItem != null && this.comboBoxLowValueSymbolic.SelectedItem.ToString() != string.Empty)
				{
					this.errorProviderFormat.SetError(this.comboBoxLowValueSymbolic, Resources.ErrorInvalidValueWithRelOp);
				}
				this.errorProviderFormat.SetError(this.textBoxLowValue, Resources.ErrorInvalidValueWithRelOp);
				flag = false;
			}
			this.SubscribeControlEvents(true);
			return flag;
		}

		private void ResetFormatErrorAndRestoreRawValueFromModel(TextBox rawValueTextBox, double rawModelValue)
		{
			if (!string.IsNullOrEmpty(this.errorProviderFormat.GetError(rawValueTextBox)))
			{
				rawValueTextBox.Text = GUIUtil.RawSignalValueToDisplayString(rawModelValue, this.signalDefinition);
				this.errorProviderFormat.SetError(rawValueTextBox, "");
			}
		}

		private void ResetFormatErrorAndRestorePhysicalValueFromModel(TextBox physValueTextBox, double rawModelValue)
		{
			if (!string.IsNullOrEmpty(this.errorProviderFormat.GetError(physValueTextBox)))
			{
				this.errorProviderFormat.SetError(physValueTextBox, "");
				if (this.signalDefinition == null)
				{
					physValueTextBox.Text = Resources.Unknown;
					return;
				}
				double physicalValue = this.GetPhysicalValue(rawModelValue);
				physValueTextBox.Text = this.signalDefinition.PhysicalValueToString(physicalValue);
			}
		}

		private bool SignalDefinitionIsValid(SignalDefinition def)
		{
			if (def == null)
			{
				return false;
			}
			if (def.Length > this.modelValidator.LoggerSpecifics.Recording.MaximumSignalLength)
			{
				InformMessageBox.Error(string.Format(Resources.ErrorSignalLenExceedsMaximum, this.modelValidator.LoggerSpecifics.Recording.MaximumSignalLength));
				return false;
			}
			if (def.Factor < 0.0)
			{
				InformMessageBox.Error(Resources.ErrorSignalHasNegativeFactor);
				return false;
			}
			return true;
		}

		private double GetPhysicalValue(double rawValue)
		{
			double result = 0.0;
			if (this.sigEvent is SymbolicSignalEvent)
			{
				this.databaseManager.RawSignalValueToPhysicalValue(this.modelValidator.GetAbsoluteFilePath(this.sigEvent.DatabasePath.Value), this.sigEvent.NetworkName.Value, this.sigEvent.MessageName.Value, this.sigEvent.SignalName.Value, rawValue, out result);
			}
			else
			{
				result = CcpXcpManager.Instance().RawSignalValueToPhysicalValue(rawValue, this.signalDefinition);
			}
			return result;
		}

		private bool GetRawValue(double physicalValue, out double rawValue)
		{
			if (this.sigEvent is SymbolicSignalEvent)
			{
				return this.databaseManager.PhysicalSignalValueToRawValue(this.modelValidator.GetAbsoluteFilePath(this.sigEvent.DatabasePath.Value), this.sigEvent.NetworkName.Value, this.sigEvent.MessageName.Value, this.sigEvent.SignalName.Value, physicalValue, out rawValue);
			}
			return CcpXcpManager.Instance().PhysicalSignalValueToRawValue(physicalValue, this.signalDefinition, out rawValue);
		}

		private void RestoreControlsForInvalidSignal()
		{
			this.popupListEditSignals.Text = this.sigEvent.SignalName.Value;
			if (this.buttonOK.Enabled)
			{
				this.buttonOK.Focus();
				return;
			}
			this.buttonCancel.Focus();
		}

		private void ApplyCcpXcpSignalToControls(CcpXcpSignal signal)
		{
			if (signal == null)
			{
				return;
			}
			if (!CcpXcpManager.Instance().IsA2lDatabaseLoaded(signal))
			{
				InformMessageBox.Error(Resources_CcpXcp.ErrorCcpXcpSignalCouldNotLoad);
				this.RestoreControlsForInvalidSignal();
				return;
			}
			CcpXcpManager.EnumValType enumValType;
			uint signalDimensionAndValueType = CcpXcpManager.Instance().GetSignalDimensionAndValueType(signal, out enumValType);
			switch (enumValType)
			{
			case CcpXcpManager.EnumValType.VtUByte:
			case CcpXcpManager.EnumValType.VtSByte:
			case CcpXcpManager.EnumValType.VtUWord:
			case CcpXcpManager.EnumValType.VtSWord:
			case CcpXcpManager.EnumValType.VtULong:
			case CcpXcpManager.EnumValType.VtSLong:
			{
				if (signalDimensionAndValueType > 1u)
				{
					InformMessageBox.Error(Resources_CcpXcp.ErrorCcpXcpTriggerSignalWithMultipleDimensions);
					this.popupListEditSignals.Text = this.sigEvent.SignalName.Value;
					this.RestoreControlsForInvalidSignal();
					return;
				}
				Database database = CcpXcpManager.Instance().GetDatabase(signal);
				if (database != null && database.BusType != null && database.BusType.Value == BusType.Bt_FlexRay)
				{
					InformMessageBox.Error(Resources_CcpXcp.ErrorCcpXcpFlexRaySignalsCannotBeUsedAsTriggerOrMarker);
					this.popupListEditSignals.Text = this.sigEvent.SignalName.Value;
					this.RestoreControlsForInvalidSignal();
					return;
				}
				SignalDefinition signalDefinition = CcpXcpManager.Instance().GetSignalDefinition(signal);
				if (!this.SignalDefinitionIsValid(signalDefinition))
				{
					this.popupListEditSignals.Text = this.sigEvent.SignalName.Value;
					this.RestoreControlsForInvalidSignal();
					return;
				}
				if (signalDefinition.Factor < 0.0)
				{
					InformMessageBox.Error(Resources.ErrorSignalHasNegativeFactor);
					return;
				}
				this.signalDefinition = signalDefinition;
				bool flag = false;
				if (database != null && database.BusType != null)
				{
					this.sigEvent.SignalName.Value = signal.Name.Value;
					this.sigEvent.CcpXcpEcuName.Value = signal.EcuName.Value;
					if (this.sigEvent.ChannelNumber.Value != database.ChannelNumber.Value)
					{
						this.sigEvent.ChannelNumber.Value = database.ChannelNumber.Value;
						flag = true;
					}
					if (this.sigEvent.BusType.Value != database.BusType.Value)
					{
						this.sigEvent.BusType.Value = database.BusType.Value;
						this.InitChannelComboBox();
						flag = true;
					}
				}
				if (flag)
				{
					this.ApplyChannelNumberToControl();
				}
				if (!string.IsNullOrEmpty(this.errorProviderGlobalModel.GetError(this.textBoxSignalName)))
				{
					this.errorProviderGlobalModel.SetError(this.textBoxSignalName, "");
				}
				if (this.sigEvent.Relation.Value != CondRelation.InRange && this.sigEvent.Relation.Value != CondRelation.NotInRange)
				{
					double physicalValue = CcpXcpManager.Instance().RawSignalValueToPhysicalValue(this.sigEvent.HighValue.Value, this.signalDefinition);
					this.textBoxHighValuePhysical.Text = this.signalDefinition.PhysicalValueToString(physicalValue);
				}
				this.DisplayFullSignalInfo();
				this.ValidateInput();
				this.EnableValueControls();
				return;
			}
			default:
				InformMessageBox.Error(Resources_CcpXcp.ErrorCcpXcpTriggerSignalValueType);
				this.popupListEditSignals.Text = this.sigEvent.SignalName.Value;
				this.RestoreControlsForInvalidSignal();
				return;
			}
		}

		private void ApplyDiagnosticSignalToControls(DiagnosticAction signal)
		{
			if (signal == null)
			{
				return;
			}
			DiagnosticSignalRequest diagnosticSignalRequest = signal as DiagnosticSignalRequest;
			if (diagnosticSignalRequest == null)
			{
				return;
			}
			SignalDefinition def = null;
			string text;
			if (this.modelValidator.IsDiagECUConfigured(diagnosticSignalRequest.DatabasePath.Value, diagnosticSignalRequest.EcuQualifier.Value, out text) && !DiagSymbolsManager.Instance().GetSignalDefinition(diagnosticSignalRequest.EcuQualifier.Value, text, diagnosticSignalRequest.DidId.Value, diagnosticSignalRequest.SignalQualifier.Value, out def))
			{
				def = null;
			}
			if (!this.SignalDefinitionIsValid(def))
			{
				this.popupListEditSignals.Text = this.sigEvent.SignalName.Value;
				this.RestoreControlsForInvalidSignal();
				return;
			}
			this.signalDefinition = def;
			this.sigEvent.SignalName.Value = diagnosticSignalRequest.SignalQualifier.Value;
			this.sigEvent.DatabasePath.Value = diagnosticSignalRequest.DatabasePath.Value;
			if (this.sigEvent is DiagnosticSignalEvent)
			{
				DiagnosticSignalEvent diagnosticSignalEvent = this.sigEvent as DiagnosticSignalEvent;
				diagnosticSignalEvent.DiagnosticEcuName.Value = diagnosticSignalRequest.EcuQualifier.Value;
				diagnosticSignalEvent.DiagnosticServiceName.Value = diagnosticSignalRequest.ServiceQualifier.Value;
				diagnosticSignalEvent.DiagnosticVariant.Value = text;
				diagnosticSignalEvent.DiagnosticDid.Value = diagnosticSignalRequest.DidId.Value;
				diagnosticSignalEvent.DiagnosticMessageData.Value = diagnosticSignalRequest.MessageData.Value;
			}
			DiagnosticsDatabase diagnosticsDatabase;
			DiagnosticsECU diagnosticsECU;
			if (this.DiagnosticsDatabaseConfiguration.TryGetDiagnosticsDatabase(diagnosticSignalRequest.DatabasePath.Value, out diagnosticsDatabase) && diagnosticsDatabase.TryGetECU(diagnosticSignalRequest.EcuQualifier.Value, out diagnosticsECU))
			{
				bool flag = false;
				if (this.sigEvent.ChannelNumber.Value != diagnosticsECU.ChannelNumber.Value)
				{
					this.sigEvent.ChannelNumber.Value = diagnosticsECU.ChannelNumber.Value;
					flag = true;
				}
				if (this.sigEvent.BusType.Value != diagnosticsECU.BusType.Value)
				{
					this.sigEvent.BusType.Value = diagnosticsECU.BusType.Value;
					this.InitChannelComboBox();
					flag = true;
				}
				if (flag)
				{
					this.ApplyChannelNumberToControl();
				}
			}
			if (!string.IsNullOrEmpty(this.errorProviderGlobalModel.GetError(this.textBoxSignalName)))
			{
				this.errorProviderGlobalModel.SetError(this.textBoxSignalName, "");
			}
			if (this.sigEvent.Relation.Value != CondRelation.InRange && this.sigEvent.Relation.Value != CondRelation.NotInRange)
			{
				double physicalValue = GUIUtil.RawSignalValueToPhysicalValue(this.sigEvent.HighValue.Value, this.signalDefinition);
				this.textBoxHighValuePhysical.Text = this.signalDefinition.PhysicalValueToString(physicalValue);
			}
			this.DisplayFullSignalInfo();
			this.ValidateInput();
			this.EnableValueControls();
		}

		private void DisplayFullSignalInfo()
		{
			if (this.sigEvent is SymbolicSignalEvent)
			{
				this.textBoxSignalName.Text = string.Format(Resources.SymbolicSignalName, this.sigEvent.MessageName.Value, this.sigEvent.SignalName.Value);
			}
			else
			{
				this.popupListEditSignals.Text = this.sigEvent.SignalName.Value;
			}
			string arg = Vocabulary.CAN;
			if (this.sigEvent.BusType.Value == BusType.Bt_LIN)
			{
				arg = Vocabulary.LIN;
			}
			else if (this.sigEvent.BusType.Value == BusType.Bt_FlexRay)
			{
				arg = Vocabulary.Flexray;
			}
			this.buttonOK.Enabled = true;
			this.labelSignalName.Text = string.Format(Resources.BusTypeSignalName, arg);
			if (this.sigEvent is CcpXcpSignalEvent || this.sigEvent is DiagnosticSignalEvent)
			{
				this.labelSignalName.Text = Resources.CaptionSignalName;
			}
			if (this.signalDefinition != null)
			{
				string text = Resources.No;
				if (this.signalDefinition.IsMultiRange)
				{
					text = Resources.Yes;
				}
				string text2 = Resources.No;
				if (this.signalDefinition.HasLinearConversion)
				{
					text2 = Resources.Yes;
				}
				double num = 0.0;
				double num2 = Math.Pow(2.0, Convert.ToDouble(this.signalDefinition.Length, ProgramUtils.Culture)) - 1.0;
				if (this.signalDefinition.IsSigned)
				{
					num = Math.Pow(2.0, Convert.ToDouble(this.signalDefinition.Length - 1u, ProgramUtils.Culture)) * -1.0;
					num2 = Math.Pow(2.0, Convert.ToDouble(this.signalDefinition.Length - 1u, ProgramUtils.Culture)) - 1.0;
				}
				double physicalValue = this.GetPhysicalValue(num);
				double physicalValue2 = this.GetPhysicalValue(num2);
				string format = Resources.PhysicalSignalInfoInConditionDialog;
				if (this.sigEvent is CcpXcpSignalEvent)
				{
					format = Resources_CcpXcp.PhysicalCcpXcpSignalInfoInConditionDialog;
				}
				this.richTextBoxInfoPhys.Text = string.Format(format, new object[]
				{
					(this.sigEvent is CcpXcpSignalEvent && !this.signalDefinition.HasLinearConversion) ? Resources.Unknown : this.signalDefinition.PhysicalValueToString(physicalValue),
					(this.sigEvent is CcpXcpSignalEvent && !this.signalDefinition.HasLinearConversion) ? Resources.Unknown : this.signalDefinition.PhysicalValueToString(physicalValue2),
					(this.sigEvent is CcpXcpSignalEvent && !this.signalDefinition.HasLinearConversion) ? "-" : this.signalDefinition.Factor.ToString(),
					(this.sigEvent is CcpXcpSignalEvent && !this.signalDefinition.HasLinearConversion) ? "-" : this.signalDefinition.Offset.ToString(),
					this.signalDefinition.Unit,
					text,
					text2
				});
				string text3;
				if (this.signalDefinition.IsSigned)
				{
					text3 = Resources.SignalTypeSigned;
					if (GUIUtil.IsHexadecimal)
					{
						num = 0.0;
						num2 = Math.Pow(2.0, Convert.ToDouble(this.signalDefinition.Length, ProgramUtils.Culture)) - 1.0;
					}
				}
				else
				{
					text3 = Resources.SignalTypeUnsigned;
				}
				string text4 = GUIUtil.RawSignalValueToDisplayString(num, this.signalDefinition);
				string text5 = GUIUtil.RawSignalValueToDisplayString(num2, this.signalDefinition);
				this.richTextBoxInfoRaw.Text = string.Format(Resources.RawSignalInfoInConditionDialog, new object[]
				{
					text4,
					text5,
					text3,
					this.signalDefinition.Length
				});
				return;
			}
			string format2 = Resources.PhysicalSignalInfoInConditionDialog;
			if (this.sigEvent is CcpXcpSignalEvent)
			{
				format2 = Resources_CcpXcp.PhysicalCcpXcpSignalInfoInConditionDialog;
			}
			this.richTextBoxInfoPhys.Text = string.Format(format2, new object[]
			{
				Resources.Unknown,
				Resources.Unknown,
				Resources.Unknown,
				Resources.Unknown,
				Resources.Unknown,
				Resources.Unknown,
				Resources.Unknown
			});
			this.richTextBoxInfoRaw.Text = string.Format(Resources.RawSignalInfoInConditionDialog, new object[]
			{
				Resources.Unknown,
				Resources.Unknown,
				Resources.Unknown,
				Resources.Unknown
			});
			this.buttonOK.Enabled = false;
		}

		private bool HasError(Control control)
		{
			return !string.IsNullOrEmpty(this.errorProviderFormat.GetError(control));
		}

		public bool IsValueInSignalRange(double value)
		{
			if (this.signalDefinition != null)
			{
				double num = Math.Pow(2.0, Convert.ToDouble(this.signalDefinition.Length, ProgramUtils.Culture));
				if (value >= num)
				{
					return false;
				}
			}
			return true;
		}

		private void RenderLabelsForDisplayMode()
		{
			string arg = Resources.DisplayModeDec;
			if (GUIUtil.IsHexadecimal)
			{
				arg = Resources.DisplayModeHex;
			}
			this.labelRaw.Text = string.Format(Resources.RawInputLabelWithMode, arg);
			CcpXcpSignalEvent arg_37_0 = this.sigEvent as CcpXcpSignalEvent;
		}

		private void SubscribeControlEvents(bool doSubscribe)
		{
			if (doSubscribe)
			{
				this.buttonSelectSignal.Click += new EventHandler(this.buttonSelectSignal_Click);
				this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
				this.comboBoxConditionType.SelectedIndexChanged += new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
				this.comboBoxLowValueSymbolic.SelectedIndexChanged += new EventHandler(this.comboBoxLowValueSymbolic_SelectedIndexChanged);
				this.comboBoxHighValueSymbolic.SelectedIndexChanged += new EventHandler(this.comboBoxHighValueSymbolic_SelectedIndexChanged);
				this.textBoxLowValue.Validating += new CancelEventHandler(this.textBoxLowValue_Validating);
				this.textBoxHighValue.Validating += new CancelEventHandler(this.textBoxHighValue_Validating);
				this.textBoxLowValuePhysical.Validating += new CancelEventHandler(this.textBoxLowValuePhysical_Validating);
				this.textBoxHighValuePhysical.Validating += new CancelEventHandler(this.textBoxHighValuePhysical_Validating);
				return;
			}
			this.buttonSelectSignal.Click -= new EventHandler(this.buttonSelectSignal_Click);
			this.comboBoxChannel.SelectedIndexChanged -= new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			this.comboBoxConditionType.SelectedIndexChanged -= new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
			this.comboBoxLowValueSymbolic.SelectedIndexChanged -= new EventHandler(this.comboBoxLowValueSymbolic_SelectedIndexChanged);
			this.comboBoxHighValueSymbolic.SelectedIndexChanged -= new EventHandler(this.comboBoxHighValueSymbolic_SelectedIndexChanged);
			this.textBoxLowValue.Validating -= new CancelEventHandler(this.textBoxLowValue_Validating);
			this.textBoxHighValue.Validating -= new CancelEventHandler(this.textBoxHighValue_Validating);
			this.textBoxLowValuePhysical.Validating -= new CancelEventHandler(this.textBoxLowValuePhysical_Validating);
			this.textBoxHighValuePhysical.Validating -= new CancelEventHandler(this.textBoxHighValuePhysical_Validating);
		}

		private void EnableValueControls()
		{
			this.textBoxLowValue.Enabled = false;
			this.textBoxHighValue.Enabled = false;
			this.textBoxLowValuePhysical.Enabled = false;
			this.textBoxHighValuePhysical.Enabled = false;
			this.comboBoxLowValueSymbolic.Enabled = false;
			this.comboBoxHighValueSymbolic.Enabled = false;
			this.labelHint.Visible = false;
			this.iconHint.Visible = false;
			if (this.signalDefinition == null)
			{
				this.comboBoxConditionType.Enabled = false;
				return;
			}
			this.comboBoxConditionType.Enabled = true;
			this.labelHint.Visible = (this.sigEvent.Relation.Value == CondRelation.OnChange && (this.signalDefinition.Offset != 0.0 || this.signalDefinition.Factor != 1.0));
			this.iconHint.Visible = this.labelHint.Visible;
			if (this.sigEvent.Relation.Value == CondRelation.OnChange)
			{
				if (this.signalDefinition.Offset != 0.0 || this.signalDefinition.Factor != 1.0)
				{
					this.labelHint.Visible = true;
					this.iconHint.Visible = true;
					return;
				}
			}
			else if (this.sigEvent.Relation.Value == CondRelation.InRange || this.sigEvent.Relation.Value == CondRelation.NotInRange)
			{
				this.textBoxLowValue.Enabled = true;
				this.textBoxHighValue.Enabled = true;
				if (this.textEncodedToRawValue.Count > 0)
				{
					this.comboBoxLowValueSymbolic.Enabled = true;
					this.comboBoxHighValueSymbolic.Enabled = true;
				}
				if (this.sigEvent is CcpXcpSignalEvent)
				{
					this.textBoxLowValuePhysical.Enabled = this.signalDefinition.HasLinearConversion;
					this.textBoxHighValuePhysical.Enabled = this.signalDefinition.HasLinearConversion;
					return;
				}
				this.textBoxLowValuePhysical.Enabled = true;
				this.textBoxHighValuePhysical.Enabled = true;
				return;
			}
			else
			{
				this.textBoxLowValue.Enabled = true;
				if (this.textEncodedToRawValue.Count > 0)
				{
					this.comboBoxLowValueSymbolic.Enabled = true;
				}
				if (this.sigEvent is CcpXcpSignalEvent)
				{
					this.textBoxLowValuePhysical.Enabled = this.signalDefinition.HasLinearConversion;
					return;
				}
				this.textBoxLowValuePhysical.Enabled = true;
			}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SymbolicSignalCondition));
			PopupListEditValue editValue = new PopupListEditValue();
			this.labelSignalName = new Label();
			this.textBoxSignalName = new TextBox();
			this.buttonSelectSignal = new Button();
			this.comboBoxConditionType = new ComboBox();
			this.labelConditionType = new Label();
			this.labelValue = new Label();
			this.textBoxLowValue = new TextBox();
			this.labelHighLimitValue = new Label();
			this.textBoxHighValue = new TextBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.textBoxLowValuePhysical = new TextBox();
			this.textBoxHighValuePhysical = new TextBox();
			this.comboBoxChannel = new ComboBox();
			this.comboBoxLowValueSymbolic = new ComboBox();
			this.comboBoxHighValueSymbolic = new ComboBox();
			this.richTextBoxInfoPhys = new RichTextBox();
			this.richTextBoxInfoRaw = new RichTextBox();
			this.buttonHelp = new Button();
			this.labelChannel = new Label();
			this.labelHint = new Label();
			this.iconHint = new PictureBox();
			this.popupListEditSignals = new PopupListEdit();
			this.labelSymbolic = new Label();
			this.labelRaw = new Label();
			this.labelPhysical = new Label();
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.toolTip = new ToolTip(this.components);
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.iconHint).BeginInit();
			((ISupportInitialize)this.popupListEditSignals.Properties).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelSignalName, "labelSignalName");
			this.labelSignalName.Name = "labelSignalName";
			this.errorProviderFormat.SetIconAlignment(this.textBoxSignalName, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxSignalName.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxSignalName, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxSignalName.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxSignalName, "textBoxSignalName");
			this.textBoxSignalName.Name = "textBoxSignalName";
			this.textBoxSignalName.ReadOnly = true;
			this.textBoxSignalName.MouseEnter += new EventHandler(this.textBoxSignalName_MouseEnter);
			componentResourceManager.ApplyResources(this.buttonSelectSignal, "buttonSelectSignal");
			this.buttonSelectSignal.Name = "buttonSelectSignal";
			this.buttonSelectSignal.UseVisualStyleBackColor = true;
			this.buttonSelectSignal.Click += new EventHandler(this.buttonSelectSignal_Click);
			this.comboBoxConditionType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxConditionType.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxConditionType, "comboBoxConditionType");
			this.comboBoxConditionType.Name = "comboBoxConditionType";
			this.comboBoxConditionType.SelectedIndexChanged += new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelConditionType, "labelConditionType");
			this.labelConditionType.Name = "labelConditionType";
			componentResourceManager.ApplyResources(this.labelValue, "labelValue");
			this.labelValue.Name = "labelValue";
			this.errorProviderFormat.SetIconAlignment(this.textBoxLowValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLowValue.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxLowValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLowValue.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxLowValue, "textBoxLowValue");
			this.textBoxLowValue.Name = "textBoxLowValue";
			this.textBoxLowValue.Validating += new CancelEventHandler(this.textBoxLowValue_Validating);
			componentResourceManager.ApplyResources(this.labelHighLimitValue, "labelHighLimitValue");
			this.labelHighLimitValue.Name = "labelHighLimitValue";
			this.errorProviderFormat.SetIconAlignment(this.textBoxHighValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxHighValue.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxHighValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxHighValue.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxHighValue, "textBoxHighValue");
			this.textBoxHighValue.Name = "textBoxHighValue";
			this.textBoxHighValue.Validating += new CancelEventHandler(this.textBoxHighValue_Validating);
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderFormat.SetIconAlignment(this.textBoxLowValuePhysical, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLowValuePhysical.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxLowValuePhysical, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLowValuePhysical.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxLowValuePhysical, "textBoxLowValuePhysical");
			this.textBoxLowValuePhysical.Name = "textBoxLowValuePhysical";
			this.textBoxLowValuePhysical.Validating += new CancelEventHandler(this.textBoxLowValuePhysical_Validating);
			this.errorProviderFormat.SetIconAlignment(this.textBoxHighValuePhysical, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxHighValuePhysical.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxHighValuePhysical, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxHighValuePhysical.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxHighValuePhysical, "textBoxHighValuePhysical");
			this.textBoxHighValuePhysical.Name = "textBoxHighValuePhysical";
			this.textBoxHighValuePhysical.Validating += new CancelEventHandler(this.textBoxHighValuePhysical_Validating);
			this.comboBoxChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxChannel.FormattingEnabled = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment1"));
			componentResourceManager.ApplyResources(this.comboBoxChannel, "comboBoxChannel");
			this.comboBoxChannel.Name = "comboBoxChannel";
			this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			this.comboBoxLowValueSymbolic.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxLowValueSymbolic.FormattingEnabled = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxLowValueSymbolic, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxLowValueSymbolic.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxLowValueSymbolic, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxLowValueSymbolic.IconAlignment1"));
			componentResourceManager.ApplyResources(this.comboBoxLowValueSymbolic, "comboBoxLowValueSymbolic");
			this.comboBoxLowValueSymbolic.Name = "comboBoxLowValueSymbolic";
			this.comboBoxLowValueSymbolic.SelectedIndexChanged += new EventHandler(this.comboBoxLowValueSymbolic_SelectedIndexChanged);
			this.comboBoxLowValueSymbolic.MouseEnter += new EventHandler(this.comboBoxLowValueSymbolic_MouseEnter);
			this.comboBoxHighValueSymbolic.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxHighValueSymbolic.FormattingEnabled = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxHighValueSymbolic, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxHighValueSymbolic.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxHighValueSymbolic, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxHighValueSymbolic.IconAlignment1"));
			componentResourceManager.ApplyResources(this.comboBoxHighValueSymbolic, "comboBoxHighValueSymbolic");
			this.comboBoxHighValueSymbolic.Name = "comboBoxHighValueSymbolic";
			this.comboBoxHighValueSymbolic.SelectedIndexChanged += new EventHandler(this.comboBoxHighValueSymbolic_SelectedIndexChanged);
			this.comboBoxHighValueSymbolic.MouseEnter += new EventHandler(this.comboBoxHighValueSymbolic_MouseEnter);
			this.richTextBoxInfoPhys.BorderStyle = BorderStyle.None;
			componentResourceManager.ApplyResources(this.richTextBoxInfoPhys, "richTextBoxInfoPhys");
			this.richTextBoxInfoPhys.Name = "richTextBoxInfoPhys";
			this.richTextBoxInfoPhys.ReadOnly = true;
			this.richTextBoxInfoPhys.TabStop = false;
			this.richTextBoxInfoRaw.BorderStyle = BorderStyle.None;
			componentResourceManager.ApplyResources(this.richTextBoxInfoRaw, "richTextBoxInfoRaw");
			this.richTextBoxInfoRaw.Name = "richTextBoxInfoRaw";
			this.richTextBoxInfoRaw.ReadOnly = true;
			this.richTextBoxInfoRaw.TabStop = false;
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.labelChannel, "labelChannel");
			this.labelChannel.Name = "labelChannel";
			componentResourceManager.ApplyResources(this.labelHint, "labelHint");
			this.labelHint.Name = "labelHint";
			componentResourceManager.ApplyResources(this.iconHint, "iconHint");
			this.iconHint.Name = "iconHint";
			this.iconHint.TabStop = false;
			this.popupListEditSignals.EditValue = editValue;
			this.popupListEditSignals.Image = null;
			componentResourceManager.ApplyResources(this.popupListEditSignals, "popupListEditSignals");
			this.popupListEditSignals.Name = "popupListEditSignals";
			this.popupListEditSignals.Properties.AllowFreeInputText = false;
			this.popupListEditSignals.Properties.CustomButtonImage = null;
			this.popupListEditSignals.Properties.CustomButtonTooltipText = componentResourceManager.GetString("popupListEditSignals.Properties.CustomButtonTooltipText");
			this.popupListEditSignals.Properties.DropDownArrowTooltipText = componentResourceManager.GetString("popupListEditSignals.Properties.DropDownArrowTooltipText");
			this.popupListEditSignals.Properties.ItemSelectionButtonImage = null;
			this.popupListEditSignals.Properties.ItemSelectionButtonTooltipText = componentResourceManager.GetString("popupListEditSignals.Properties.ItemSelectionButtonTooltipText");
			this.popupListEditSignals.Properties.ShowCustomButton = false;
			this.popupListEditSignals.Properties.ShowDropDownArrow = true;
			this.popupListEditSignals.Properties.ShowItemSelectionButton = true;
			this.popupListEditSignals.StandaloneValueAccepted += new EventHandler(this.popupListEditSignals_StandaloneValueAccepted);
			componentResourceManager.ApplyResources(this.labelSymbolic, "labelSymbolic");
			this.labelSymbolic.Name = "labelSymbolic";
			componentResourceManager.ApplyResources(this.labelRaw, "labelRaw");
			this.labelRaw.Name = "labelRaw";
			componentResourceManager.ApplyResources(this.labelPhysical, "labelPhysical");
			this.labelPhysical.Name = "labelPhysical";
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoValidate = AutoValidate.EnableAllowFocusChange;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.labelPhysical);
			base.Controls.Add(this.labelRaw);
			base.Controls.Add(this.labelSymbolic);
			base.Controls.Add(this.comboBoxHighValueSymbolic);
			base.Controls.Add(this.comboBoxLowValueSymbolic);
			base.Controls.Add(this.popupListEditSignals);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.labelHint);
			base.Controls.Add(this.iconHint);
			base.Controls.Add(this.labelChannel);
			base.Controls.Add(this.comboBoxChannel);
			base.Controls.Add(this.richTextBoxInfoRaw);
			base.Controls.Add(this.richTextBoxInfoPhys);
			base.Controls.Add(this.textBoxHighValuePhysical);
			base.Controls.Add(this.textBoxLowValuePhysical);
			base.Controls.Add(this.textBoxHighValue);
			base.Controls.Add(this.labelHighLimitValue);
			base.Controls.Add(this.textBoxLowValue);
			base.Controls.Add(this.labelValue);
			base.Controls.Add(this.labelConditionType);
			base.Controls.Add(this.comboBoxConditionType);
			base.Controls.Add(this.buttonSelectSignal);
			base.Controls.Add(this.textBoxSignalName);
			base.Controls.Add(this.labelSignalName);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SymbolicSignalCondition";
			base.ShowInTaskbar = false;
			base.Shown += new EventHandler(this.SymbolicSignalCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.SymbolicSignalCondition_HelpRequested);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.iconHint).EndInit();
			((ISupportInitialize)this.popupListEditSignals.Properties).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
