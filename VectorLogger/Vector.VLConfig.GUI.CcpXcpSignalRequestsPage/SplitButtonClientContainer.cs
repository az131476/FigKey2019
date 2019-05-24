using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common.EventConditions;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.CcpXcpSignalRequestsPage
{
	internal class SplitButtonClientContainer : ISplitButtonExClient
	{
		public delegate void EventChangedDelegate();

		private SplitButton splitButton;

		private readonly SplitButtonEx splitButtonEx;

		public SplitButtonClientContainer.EventChangedDelegate EventChanged;

		private IModelValidator ModelValidator
		{
			get;
			set;
		}

		private IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
		}

		public Event ResultEvent
		{
			get;
			set;
		}

		public bool IsStopOnTimerEvent
		{
			get
			{
				return this.splitButton.Text == Resources.Timer;
			}
		}

		private bool IsStartButton
		{
			get;
			set;
		}

		public SplitButton SplitButton
		{
			get
			{
				return this.splitButton;
			}
		}

		public string SplitButtonEmptyDefault
		{
			get
			{
				return Resources.SplitButtonEmptyDefault;
			}
		}

		public SplitButtonClientContainer(ActionCcpXcp prevAction, SplitButton splitButton, bool isStartButton, IModelValidator modelValidator, IApplicationDatabaseManager applicationDatabaseManager) : this(splitButton, isStartButton, modelValidator, applicationDatabaseManager)
		{
			if (prevAction != null)
			{
				string text = "";
				if (isStartButton)
				{
					text = this.GetEventName(prevAction.Event);
				}
				else if (prevAction.StopType is StopOnEvent)
				{
					text = this.GetEventName((prevAction.StopType as StopOnEvent).Event);
				}
				else if (prevAction.StopType is StopOnTimer)
				{
					text = Resources.Timer;
				}
				if (!string.IsNullOrEmpty(text))
				{
					splitButton.Text = text;
				}
			}
		}

		public SplitButtonClientContainer(SplitButton splitButton, bool isStartButton, IModelValidator modelValidator, IApplicationDatabaseManager applicationDatabaseManager)
		{
			this.splitButton = splitButton;
			this.ModelValidator = modelValidator;
			this.ApplicationDatabaseManager = applicationDatabaseManager;
			this.IsStartButton = isStartButton;
			this.splitButton.AutoSize = false;
			this.splitButtonEx = new SplitButtonEx(this);
			GUIUtil.InitSplitButtonMenuEventTypes(this.splitButtonEx);
			if (!this.IsStartButton)
			{
				this.splitButton.SplitMenuStrip.Items.Add(new ToolStripMenuItem(Resources.Timer, Resources.ImageNone, new EventHandler(this.OnTimerClick)));
			}
		}

		public bool IsItemVisible(ToolStripItem item)
		{
			string text = item.Text;
			if (text == Vocabulary.TriggerTypeNameColOnStart || text == Resources_Trigger.TriggerTypeNameColOnCycTimer)
			{
				return this.IsStartButton;
			}
			if (text == Resources.Timer)
			{
				return !this.IsStartButton;
			}
			if (text == Resources_Trigger.TriggerTypeNameColCANData || text == Vocabulary.TriggerTypeNameColCANId || text == Resources_Trigger.TriggerTypeNameColCANMsgTimeout || text == Resources_Trigger.TriggerTypeNameColSymbolicCAN || text == Resources_Trigger.TriggerTypeNameColSymbolicSigCAN)
			{
				return this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColLINData || text == Vocabulary.TriggerTypeNameColLINId || text == Resources_Trigger.TriggerTypeNameColLINMsgTimeout || text == Resources_Trigger.TriggerTypeNameColSymbolicLIN || text == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN)
			{
				return this.ModelValidator.LoggerSpecifics.LIN.NumberOfChannels > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColFlexray || text == Resources_Trigger.TriggerTypeNameColSymbolicFlexray || text == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray)
			{
				return this.ModelValidator.LoggerSpecifics.Flexray.NumberOfChannels > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColKey)
			{
				return this.ModelValidator.LoggerSpecifics.IO.NumberOfKeys > 0u || this.ModelValidator.LoggerSpecifics.IO.NumberOfPanelKeys > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColAnalogInput)
			{
				return this.ModelValidator.LoggerSpecifics.IO.NumberOfAnalogInputs > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColDigitalInput)
			{
				return this.ModelValidator.LoggerSpecifics.IO.NumberOfDigitalInputs > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColIgnition)
			{
				return this.ModelValidator.LoggerSpecifics.Recording.IsIgnitionEventSupported;
			}
			return !(text == Resources_Trigger.TriggerTypeNameColVoCanRecording) && text == Resources_Trigger.TriggerTypeNameColCcpXcpSignal && this.ModelValidator.LoggerSpecifics.Recording.IsCCPXCPSignalEventSupported;
		}

		public void ItemClicked(ToolStripItem item)
		{
			this.AddItem(item.Text);
		}

		public void DefaultActionClicked()
		{
			this.AddItem(this.splitButtonEx.DefaultAction);
		}

		private void AddItem(string itemText)
		{
			Event @event = this.CreateActionEvent(itemText);
			if (@event != null)
			{
				this.ResultEvent = @event;
			}
			if (this.EventChanged != null)
			{
				this.EventChanged();
			}
		}

		protected virtual void OnTimerClick(object sender, EventArgs e)
		{
			this.splitButton.Text = Resources.Timer;
			this.ItemClicked((ToolStripItem)sender);
		}

		private Event CreateActionEvent(string eventTypeName)
		{
			if (eventTypeName == Vocabulary.TriggerTypeNameColOnStart)
			{
				return this.CreateOnStartEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColOnCycTimer)
			{
				return this.CreateCyclicTimerEvent();
			}
			if (eventTypeName == Vocabulary.TriggerTypeNameColCANId)
			{
				return this.CreateCANIdEvent();
			}
			if (eventTypeName == Vocabulary.TriggerTypeNameColLINId)
			{
				return this.CreateLINIdEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColCANData)
			{
				return this.CreateCANDataEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColLINData)
			{
				return this.CreateLINDataEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColFlexray)
			{
				return this.CreateFlexrayIdEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicCAN)
			{
				return this.CreateSymMessageEvent(BusType.Bt_CAN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicLIN)
			{
				return this.CreateSymMessageEvent(BusType.Bt_LIN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicFlexray)
			{
				return this.CreateSymMessageEvent(BusType.Bt_FlexRay);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicSigCAN)
			{
				return this.CreateSymSignalEvent(BusType.Bt_CAN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN)
			{
				return this.CreateSymSignalEvent(BusType.Bt_LIN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray)
			{
				return this.CreateSymSignalEvent(BusType.Bt_FlexRay);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColCANMsgTimeout)
			{
				return this.CreateMsgTimeoutEvent(BusType.Bt_CAN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColLINMsgTimeout)
			{
				return this.CreateMsgTimeoutEvent(BusType.Bt_LIN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColDigitalInput)
			{
				return this.CreateDigitalInputEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColAnalogInput)
			{
				return this.CreateAnalogInputEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColKey)
			{
				return this.CreateKeyEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColIgnition)
			{
				return this.CreateIgnitionEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColCcpXcpSignal)
			{
				return this.CreateCcpXcpSignalEvent();
			}
			if (eventTypeName == Resources.Timer)
			{
				return null;
			}
			return null;
		}

		private Event CreateOnStartEvent()
		{
			OnStartCondition onStartCondition = new OnStartCondition();
			onStartCondition.ResetToDefaults();
			if (DialogResult.OK == onStartCondition.ShowDialog())
			{
				return new OnStartEvent
				{
					Delay = 
					{
						Value = onStartCondition.Delay
					}
				};
			}
			return null;
		}

		private Event CreateCyclicTimerEvent()
		{
			CyclicTimerCondition cyclicTimerCondition = new CyclicTimerCondition();
			cyclicTimerCondition.ResetToDefaults();
			cyclicTimerCondition.IncreaseMinimum();
			if (DialogResult.OK == cyclicTimerCondition.ShowDialog())
			{
				return new CyclicTimerEvent(cyclicTimerCondition.CyclicTimerEvent);
			}
			return null;
		}

		private Event CreateCANIdEvent()
		{
			CANIdCondition cANIdCondition = new CANIdCondition(this.ModelValidator);
			cANIdCondition.ResetToDefaults();
			if (DialogResult.OK == cANIdCondition.ShowDialog())
			{
				return new CANIdEvent(cANIdCondition.CANIdEvent);
			}
			return null;
		}

		private Event CreateLINIdEvent()
		{
			LINIdCondition lINIdCondition = new LINIdCondition(this.ModelValidator);
			lINIdCondition.ResetToDefaults();
			if (DialogResult.OK == lINIdCondition.ShowDialog())
			{
				return new LINIdEvent(lINIdCondition.LINIdEvent);
			}
			return null;
		}

		private Event CreateFlexrayIdEvent()
		{
			FlexrayIdCondition flexrayIdCondition = new FlexrayIdCondition(this.ModelValidator);
			flexrayIdCondition.ResetToDefaults();
			if (DialogResult.OK == flexrayIdCondition.ShowDialog())
			{
				return new FlexrayIdEvent(flexrayIdCondition.FlexrayIdEvent);
			}
			return null;
		}

		private Event CreateCANDataEvent()
		{
			CANDataCondition cANDataCondition = new CANDataCondition(this.ModelValidator, null);
			cANDataCondition.InitDefaultValues();
			if (DialogResult.OK == cANDataCondition.ShowDialog())
			{
				return new CANDataEvent(cANDataCondition.CANDataEvent);
			}
			return null;
		}

		private Event CreateLINDataEvent()
		{
			LINDataCondition lINDataCondition = new LINDataCondition(this.ModelValidator, null);
			lINDataCondition.InitDefaultValues();
			if (DialogResult.OK == lINDataCondition.ShowDialog())
			{
				return new LINDataEvent(lINDataCondition.LINDataEvent);
			}
			return null;
		}

		private Event CreateSymMessageEvent(BusType busType)
		{
			if (!this.ModelValidator.DatabaseServices.HasDatabasesConfiguredFor(busType))
			{
				string arg = Vocabulary.CAN;
				if (busType == BusType.Bt_LIN)
				{
					arg = Vocabulary.LIN;
				}
				else if (busType == BusType.Bt_FlexRay)
				{
					arg = Vocabulary.Flexray;
				}
				InformMessageBox.Info(string.Format(Resources.NoDatabaseAvailableForBustype, arg));
				return null;
			}
			string text = "";
			string text2 = "";
			string value = "";
			string text3 = "";
			bool value2 = false;
			if (!this.ApplicationDatabaseManager.SelectMessageInDatabase(ref text, ref value, ref text2, ref text3, ref busType, ref value2))
			{
				return null;
			}
			string message;
			if (!this.ModelValidator.DatabaseServices.IsSymbolicMessageInsertAllowed(text, text3, text2, busType, out message))
			{
				InformMessageBox.Error(message);
				return null;
			}
			SymbolicMessageEvent symbolicMessageEvent = new SymbolicMessageEvent();
			symbolicMessageEvent.MessageName.Value = text;
			symbolicMessageEvent.DatabaseName.Value = value;
			symbolicMessageEvent.DatabasePath.Value = this.ModelValidator.GetFilePathRelativeToConfiguration(text2);
			symbolicMessageEvent.NetworkName.Value = text3;
			symbolicMessageEvent.BusType.Value = busType;
			symbolicMessageEvent.IsFlexrayPDU.Value = value2;
			symbolicMessageEvent.ChannelNumber.Value = 1u;
			IList<uint> channelAssignmentOfDatabase = this.ModelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(symbolicMessageEvent.DatabasePath.Value, text3);
			if (channelAssignmentOfDatabase.Count > 0)
			{
				symbolicMessageEvent.ChannelNumber.Value = channelAssignmentOfDatabase[0];
				if (symbolicMessageEvent.ChannelNumber.Value == Database.ChannelNumber_FlexrayAB && BusType.Bt_FlexRay == busType)
				{
					symbolicMessageEvent.ChannelNumber.Value = 1u;
					if (text.EndsWith(Constants.FlexrayChannelB_Postfix))
					{
						symbolicMessageEvent.ChannelNumber.Value = 2u;
					}
				}
			}
			return symbolicMessageEvent;
		}

		private Event CreateSymSignalEvent(BusType busType)
		{
			if (!this.ModelValidator.DatabaseServices.HasDatabasesConfiguredFor(busType))
			{
				string arg = Vocabulary.CAN;
				if (busType == BusType.Bt_LIN)
				{
					arg = Vocabulary.LIN;
				}
				else if (busType == BusType.Bt_FlexRay)
				{
					arg = Vocabulary.Flexray;
				}
				InformMessageBox.Info(string.Format(Resources.NoDatabaseAvailableForBustype, arg));
				return null;
			}
			string text = "";
			string text2 = "";
			string text3 = "";
			string value = "";
			string text4 = "";
			bool value2 = false;
			if (this.ApplicationDatabaseManager.SelectSignalInDatabase(ref text, ref text2, ref value, ref text3, ref text4, ref busType, ref value2))
			{
				string message;
				if (!this.ModelValidator.DatabaseServices.IsSymbolicSignalInsertAllowed(text2, text, text4, text3, busType, out message))
				{
					InformMessageBox.Error(message);
					return null;
				}
				SymbolicSignalEvent symbolicSignalEvent = new SymbolicSignalEvent();
				symbolicSignalEvent.MessageName.Value = text;
				symbolicSignalEvent.SignalName.Value = text2;
				symbolicSignalEvent.DatabaseName.Value = value;
				symbolicSignalEvent.DatabasePath.Value = this.ModelValidator.GetFilePathRelativeToConfiguration(text3);
				symbolicSignalEvent.NetworkName.Value = text4;
				symbolicSignalEvent.ChannelNumber.Value = 1u;
				IList<uint> channelAssignmentOfDatabase = this.ModelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(symbolicSignalEvent.DatabasePath.Value, text4);
				if (channelAssignmentOfDatabase.Count > 0)
				{
					symbolicSignalEvent.ChannelNumber.Value = channelAssignmentOfDatabase[0];
					if (symbolicSignalEvent.ChannelNumber.Value == Database.ChannelNumber_FlexrayAB && BusType.Bt_FlexRay == busType)
					{
						symbolicSignalEvent.ChannelNumber.Value = 1u;
						if (text.EndsWith(Constants.FlexrayChannelB_Postfix))
						{
							symbolicSignalEvent.ChannelNumber.Value = 2u;
						}
					}
				}
				symbolicSignalEvent.BusType.Value = busType;
				symbolicSignalEvent.IsFlexrayPDU.Value = value2;
				symbolicSignalEvent.LowValue.Value = 0.0;
				symbolicSignalEvent.HighValue.Value = 0.0;
				symbolicSignalEvent.Relation.Value = CondRelation.Equal;
				using (SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(this.ModelValidator, this.ApplicationDatabaseManager, null))
				{
					symbolicSignalCondition.SignalEvent = new SymbolicSignalEvent(symbolicSignalEvent);
					if (DialogResult.OK == symbolicSignalCondition.ShowDialog())
					{
						symbolicSignalEvent.Assign(symbolicSignalCondition.SignalEvent);
						return symbolicSignalEvent;
					}
				}
			}
			return null;
		}

		private Event CreateMsgTimeoutEvent(BusType busType)
		{
			MsgTimeoutCondition msgTimeoutCondition = new MsgTimeoutCondition(this.ModelValidator, this.ApplicationDatabaseManager);
			msgTimeoutCondition.InitDefaultValues(busType);
			if (DialogResult.OK == msgTimeoutCondition.ShowDialog())
			{
				return new MsgTimeoutEvent(msgTimeoutCondition.MsgTimeoutEvent);
			}
			return null;
		}

		private Event CreateDigitalInputEvent()
		{
			DigitalInputCondition digitalInputCondition = new DigitalInputCondition(this.ModelValidator);
			digitalInputCondition.ResetToDefaults();
			if (DialogResult.OK == digitalInputCondition.ShowDialog())
			{
				return new DigitalInputEvent(digitalInputCondition.DigitalInputEvent);
			}
			return null;
		}

		private Event CreateAnalogInputEvent()
		{
			AnalogInputCondition analogInputCondition = new AnalogInputCondition(this.ModelValidator);
			analogInputCondition.ResetToDefaults();
			if (DialogResult.OK == analogInputCondition.ShowDialog())
			{
				return new AnalogInputEvent(analogInputCondition.AnalogInputEvent);
			}
			return null;
		}

		private Event CreateKeyEvent()
		{
			KeyCondition keyCondition = new KeyCondition(this.ModelValidator);
			keyCondition.ResetToDefaults();
			if (DialogResult.OK == keyCondition.ShowDialog())
			{
				return new KeyEvent(keyCondition.KeyEvent);
			}
			return null;
		}

		private Event CreateIgnitionEvent()
		{
			IgnitionCondition ignitionCondition = new IgnitionCondition();
			ignitionCondition.ResetToDefaults();
			if (DialogResult.OK == ignitionCondition.ShowDialog())
			{
				return new IgnitionEvent(ignitionCondition.IgnitionEvent);
			}
			return null;
		}

		private Event CreateCcpXcpSignalEvent()
		{
			if (!CcpXcpManager.Instance().CheckTriggerSignalEvents())
			{
				return null;
			}
			RecordTrigger recordTrigger = RecordTrigger.CreateCcpXcpSignalTrigger();
			CcpXcpSignalEvent ccpXcpSignalEvent = recordTrigger.Event as CcpXcpSignalEvent;
			if (ccpXcpSignalEvent == null)
			{
				return null;
			}
			ccpXcpSignalEvent.SignalName.Value = string.Empty;
			ccpXcpSignalEvent.LowValue.Value = 0.0;
			ccpXcpSignalEvent.HighValue.Value = 0.0;
			ccpXcpSignalEvent.Relation.Value = CondRelation.Equal;
			ccpXcpSignalEvent.CcpXcpEcuName.Value = string.Empty;
			using (SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(this.ModelValidator, this.ApplicationDatabaseManager, null))
			{
				symbolicSignalCondition.SignalEvent = new CcpXcpSignalEvent(ccpXcpSignalEvent);
				symbolicSignalCondition.CcpXcpSignalConfiguration = CcpXcpManager.Instance().ConfigurationManagerService.CcpXcpSignalConfiguration;
				if (DialogResult.OK == symbolicSignalCondition.ShowDialog())
				{
					ccpXcpSignalEvent.Assign(symbolicSignalCondition.SignalEvent);
					return ccpXcpSignalEvent;
				}
			}
			return null;
		}

		public void EditEventCondition()
		{
			if (this.EditEventCondition(this.ResultEvent) && this.EventChanged != null)
			{
				this.EventChanged();
			}
		}

		private bool EditEventCondition(Event ev)
		{
			if (ev is OnStartEvent)
			{
				return this.EditOnStartEventCondition(ev as OnStartEvent);
			}
			if (ev is CyclicTimerEvent)
			{
				return this.EditCyclicTimerEventCondition(ev as CyclicTimerEvent);
			}
			if (ev is CANIdEvent)
			{
				return this.EditCANIdEventCondition(ev as CANIdEvent);
			}
			if (ev is LINIdEvent)
			{
				return this.EditLINIdEventCondition(ev as LINIdEvent);
			}
			if (ev is FlexrayIdEvent)
			{
				return this.EditFlexrayIdEventCondition(ev as FlexrayIdEvent);
			}
			if (ev is CANDataEvent)
			{
				return this.EditCANDataEventCondition(ev as CANDataEvent);
			}
			if (ev is LINDataEvent)
			{
				return this.EditLINDataEventCondition(ev as LINDataEvent);
			}
			if (ev is SymbolicMessageEvent)
			{
				return this.EditSymbolicMessageEventCondition(ev as SymbolicMessageEvent);
			}
			if (ev is ISymbolicSignalEvent)
			{
				return this.EditSymbolicSignalEventCondition(ev as ISymbolicSignalEvent);
			}
			if (ev is MsgTimeoutEvent)
			{
				return this.EditMsgTimeoutEventCondition(ev as MsgTimeoutEvent);
			}
			if (ev is DigitalInputEvent)
			{
				return this.EditDigitalInputEventCondition(ev as DigitalInputEvent);
			}
			if (ev is AnalogInputEvent)
			{
				return this.EditAnalogInputEventCondition(ev as AnalogInputEvent);
			}
			if (ev is KeyEvent)
			{
				return this.EditKeyEventCondition(ev as KeyEvent);
			}
			return ev is IgnitionEvent && this.EditIgnitionEventCondition(ev as IgnitionEvent);
		}

		private bool EditOnStartEventCondition(OnStartEvent onStartEvent)
		{
			OnStartCondition onStartCondition = new OnStartCondition();
			onStartCondition.Delay = onStartEvent.Delay.Value;
			if (DialogResult.OK == onStartCondition.ShowDialog() && onStartEvent.Delay.Value != onStartCondition.Delay)
			{
				onStartEvent.Delay.Value = onStartCondition.Delay;
				return true;
			}
			return false;
		}

		private bool EditCyclicTimerEventCondition(CyclicTimerEvent cyclicTimerEvent)
		{
			if (cyclicTimerEvent == null)
			{
				return false;
			}
			CyclicTimerCondition cyclicTimerCondition = new CyclicTimerCondition();
			cyclicTimerCondition.CyclicTimerEvent = new CyclicTimerEvent(cyclicTimerEvent);
			cyclicTimerCondition.IncreaseMinimum();
			if (DialogResult.OK == cyclicTimerCondition.ShowDialog() && !cyclicTimerEvent.Equals(cyclicTimerCondition.CyclicTimerEvent))
			{
				cyclicTimerEvent.Assign(cyclicTimerCondition.CyclicTimerEvent);
				return true;
			}
			return false;
		}

		private bool EditCANIdEventCondition(CANIdEvent canIdEvent)
		{
			if (canIdEvent == null)
			{
				return false;
			}
			CANIdCondition cANIdCondition = new CANIdCondition(this.ModelValidator);
			cANIdCondition.CANIdEvent = new CANIdEvent(canIdEvent);
			if (DialogResult.OK == cANIdCondition.ShowDialog() && !canIdEvent.Equals(cANIdCondition.CANIdEvent))
			{
				canIdEvent.Assign(cANIdCondition.CANIdEvent);
				return true;
			}
			return false;
		}

		private bool EditLINIdEventCondition(LINIdEvent linIdEvent)
		{
			if (linIdEvent == null)
			{
				return false;
			}
			LINIdCondition lINIdCondition = new LINIdCondition(this.ModelValidator);
			lINIdCondition.LINIdEvent = new LINIdEvent(linIdEvent);
			if (DialogResult.OK == lINIdCondition.ShowDialog() && !linIdEvent.Equals(lINIdCondition.LINIdEvent))
			{
				linIdEvent.Assign(lINIdCondition.LINIdEvent);
				return true;
			}
			return false;
		}

		private bool EditFlexrayIdEventCondition(FlexrayIdEvent frIdEvent)
		{
			if (frIdEvent == null)
			{
				return false;
			}
			FlexrayIdCondition flexrayIdCondition = new FlexrayIdCondition(this.ModelValidator);
			flexrayIdCondition.FlexrayIdEvent = new FlexrayIdEvent(frIdEvent);
			if (DialogResult.OK == flexrayIdCondition.ShowDialog() && !frIdEvent.Equals(flexrayIdCondition.FlexrayIdEvent))
			{
				frIdEvent.Assign(flexrayIdCondition.FlexrayIdEvent);
				return true;
			}
			return false;
		}

		private bool EditCANDataEventCondition(CANDataEvent canDataEvent)
		{
			if (canDataEvent == null)
			{
				return false;
			}
			CANDataCondition cANDataCondition = new CANDataCondition(this.ModelValidator, null);
			cANDataCondition.CANDataEvent = new CANDataEvent(canDataEvent);
			if (DialogResult.OK == cANDataCondition.ShowDialog() && !canDataEvent.Equals(cANDataCondition.CANDataEvent))
			{
				canDataEvent.Assign(cANDataCondition.CANDataEvent);
				return true;
			}
			return false;
		}

		private bool EditLINDataEventCondition(LINDataEvent linDataEvent)
		{
			if (linDataEvent == null)
			{
				return false;
			}
			LINDataCondition lINDataCondition = new LINDataCondition(this.ModelValidator, null);
			lINDataCondition.LINDataEvent = new LINDataEvent(linDataEvent);
			if (DialogResult.OK == lINDataCondition.ShowDialog() && !linDataEvent.Equals(lINDataCondition.LINDataEvent))
			{
				linDataEvent.Assign(lINDataCondition.LINDataEvent);
				return true;
			}
			return false;
		}

		private bool EditSymbolicMessageEventCondition(SymbolicMessageEvent symbolicMsgEvent)
		{
			if (symbolicMsgEvent == null)
			{
				return false;
			}
			SymbolicMessageCondition symbolicMessageCondition = new SymbolicMessageCondition(this.ModelValidator, this.ApplicationDatabaseManager);
			symbolicMessageCondition.SymbolicMessageEvent = new SymbolicMessageEvent(symbolicMsgEvent);
			if (DialogResult.OK == symbolicMessageCondition.ShowDialog() && !symbolicMsgEvent.Equals(symbolicMessageCondition.SymbolicMessageEvent))
			{
				symbolicMsgEvent.Assign(symbolicMessageCondition.SymbolicMessageEvent);
				return true;
			}
			return false;
		}

		private bool EditSymbolicSignalEventCondition(ISymbolicSignalEvent symbolicSigEvent)
		{
			if (symbolicSigEvent == null)
			{
				return false;
			}
			using (SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(this.ModelValidator, this.ApplicationDatabaseManager, null))
			{
				symbolicSignalCondition.CcpXcpSignalConfiguration = CcpXcpManager.Instance().ConfigurationManagerService.CcpXcpSignalConfiguration;
				if (symbolicSigEvent is SymbolicSignalEvent)
				{
					symbolicSignalCondition.SignalEvent = new SymbolicSignalEvent(symbolicSigEvent);
				}
				else
				{
					if (!(symbolicSigEvent is CcpXcpSignalEvent))
					{
						bool result = false;
						return result;
					}
					symbolicSignalCondition.SignalEvent = new CcpXcpSignalEvent(symbolicSigEvent);
				}
				if (DialogResult.OK == symbolicSignalCondition.ShowDialog() && !symbolicSigEvent.Equals(symbolicSignalCondition.SignalEvent))
				{
					symbolicSigEvent.Assign(symbolicSignalCondition.SignalEvent);
					bool result = true;
					return result;
				}
			}
			return false;
		}

		private bool EditMsgTimeoutEventCondition(MsgTimeoutEvent msgTimeoutEvent)
		{
			if (msgTimeoutEvent == null)
			{
				return false;
			}
			MsgTimeoutCondition msgTimeoutCondition = new MsgTimeoutCondition(this.ModelValidator, this.ApplicationDatabaseManager);
			msgTimeoutCondition.MsgTimeoutEvent = new MsgTimeoutEvent(msgTimeoutEvent);
			if (DialogResult.OK == msgTimeoutCondition.ShowDialog() && !msgTimeoutEvent.Equals(msgTimeoutCondition.MsgTimeoutEvent))
			{
				msgTimeoutEvent.Assign(msgTimeoutCondition.MsgTimeoutEvent);
				return true;
			}
			return false;
		}

		private bool EditDigitalInputEventCondition(DigitalInputEvent digInEvent)
		{
			if (digInEvent == null)
			{
				return false;
			}
			DigitalInputCondition digitalInputCondition = new DigitalInputCondition(this.ModelValidator);
			digitalInputCondition.DigitalInputEvent = new DigitalInputEvent(digInEvent);
			if (DialogResult.OK == digitalInputCondition.ShowDialog() && !digInEvent.Equals(digitalInputCondition.DigitalInputEvent))
			{
				digInEvent.Assign(digitalInputCondition.DigitalInputEvent);
				return true;
			}
			return false;
		}

		private bool EditAnalogInputEventCondition(AnalogInputEvent analogInEvent)
		{
			if (analogInEvent == null)
			{
				return false;
			}
			AnalogInputCondition analogInputCondition = new AnalogInputCondition(this.ModelValidator);
			analogInputCondition.AnalogInputEvent = new AnalogInputEvent(analogInEvent);
			if (DialogResult.OK == analogInputCondition.ShowDialog() && !analogInEvent.Equals(analogInputCondition.AnalogInputEvent))
			{
				analogInEvent.Assign(analogInputCondition.AnalogInputEvent);
				return true;
			}
			return false;
		}

		private bool EditKeyEventCondition(KeyEvent keyEvent)
		{
			if (keyEvent == null)
			{
				return false;
			}
			KeyCondition keyCondition = new KeyCondition(this.ModelValidator);
			keyCondition.KeyEvent = new KeyEvent(keyEvent);
			if (DialogResult.OK == keyCondition.ShowDialog() && !keyEvent.Equals(keyCondition.KeyEvent))
			{
				keyEvent.Assign(keyCondition.KeyEvent);
				return true;
			}
			return false;
		}

		private bool EditIgnitionEventCondition(IgnitionEvent ignitionEvent)
		{
			if (ignitionEvent == null)
			{
				return false;
			}
			IgnitionCondition ignitionCondition = new IgnitionCondition();
			ignitionCondition.IgnitionEvent = new IgnitionEvent(ignitionEvent);
			if (DialogResult.OK == ignitionCondition.ShowDialog() && !ignitionEvent.Equals(ignitionCondition.IgnitionEvent))
			{
				ignitionEvent.Assign(ignitionCondition.IgnitionEvent);
				return true;
			}
			return false;
		}

		private string GetEventName(Event ev)
		{
			if (ev is OnStartEvent)
			{
				return Vocabulary.TriggerTypeNameColOnStart;
			}
			if (ev is CyclicTimerEvent)
			{
				return Resources_Trigger.TriggerTypeNameColOnCycTimer;
			}
			if (ev is CANIdEvent)
			{
				return Vocabulary.TriggerTypeNameColCANId;
			}
			if (ev is LINIdEvent)
			{
				return Vocabulary.TriggerTypeNameColLINId;
			}
			if (ev is CANDataEvent)
			{
				return Resources_Trigger.TriggerTypeNameColCANData;
			}
			if (ev is LINDataEvent)
			{
				return Resources_Trigger.TriggerTypeNameColLINData;
			}
			if (ev is FlexrayIdEvent)
			{
				return Resources_Trigger.TriggerTypeNameColFlexray;
			}
			if (ev is SymbolicMessageEvent)
			{
				SymbolicMessageEvent symbolicMessageEvent = ev as SymbolicMessageEvent;
				if (symbolicMessageEvent.BusType.Value == BusType.Bt_CAN)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicCAN;
				}
				if (symbolicMessageEvent.BusType.Value == BusType.Bt_LIN)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicLIN;
				}
				if (symbolicMessageEvent.BusType.Value == BusType.Bt_FlexRay)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicFlexray;
				}
			}
			else if (ev is SymbolicSignalEvent)
			{
				SymbolicSignalEvent symbolicSignalEvent = ev as SymbolicSignalEvent;
				if (symbolicSignalEvent.BusType.Value == BusType.Bt_CAN)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicSigCAN;
				}
				if (symbolicSignalEvent.BusType.Value == BusType.Bt_LIN)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicSigLIN;
				}
				if (symbolicSignalEvent.BusType.Value == BusType.Bt_FlexRay)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray;
				}
			}
			else if (ev is MsgTimeoutEvent)
			{
				MsgTimeoutEvent msgTimeoutEvent = ev as MsgTimeoutEvent;
				if (msgTimeoutEvent.BusType.Value == BusType.Bt_CAN)
				{
					return Resources_Trigger.TriggerTypeNameColCANMsgTimeout;
				}
				if (msgTimeoutEvent.BusType.Value == BusType.Bt_LIN)
				{
					return Resources_Trigger.TriggerTypeNameColLINMsgTimeout;
				}
			}
			else
			{
				if (ev is DigitalInputEvent)
				{
					return Resources_Trigger.TriggerTypeNameColDigitalInput;
				}
				if (ev is AnalogInputEvent)
				{
					return Resources_Trigger.TriggerTypeNameColAnalogInput;
				}
				if (ev is KeyEvent)
				{
					return Resources_Trigger.TriggerTypeNameColKey;
				}
				if (ev is IgnitionEvent)
				{
					return Resources_Trigger.TriggerTypeNameColIgnition;
				}
				if (ev is CcpXcpSignalEvent)
				{
					return Resources_Trigger.TriggerTypeNameColCcpXcpSignal;
				}
			}
			return string.Empty;
		}
	}
}
