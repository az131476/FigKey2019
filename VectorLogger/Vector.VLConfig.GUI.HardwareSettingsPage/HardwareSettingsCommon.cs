using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common.EventConditions;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.HardwareSettingsPage
{
	public class HardwareSettingsCommon
	{
		public static bool CreateStopCycDiagCommEvent(string eventTypeName, IModelValidator modelValidator, IApplicationDatabaseManager dbManager, out Event stopCycDiagCommEvent)
		{
			stopCycDiagCommEvent = null;
			string text = "";
			string text2 = "";
			string text3 = "";
			string value = "";
			string text4 = "";
			bool result;
			if (eventTypeName == Resources.StopCycDiagCommWhileSymCANSignal || eventTypeName == Resources.StopCycDiagCommWhileSymLINSignal || eventTypeName == Resources.StopCycDiagCommWhileSymFRSignal)
			{
				BusType busType = BusType.Bt_CAN;
				string arg = Vocabulary.CAN;
				bool value2 = false;
				if (eventTypeName == Resources.StopCycDiagCommWhileSymLINSignal)
				{
					busType = BusType.Bt_LIN;
					arg = Vocabulary.LIN;
				}
				else if (eventTypeName == Resources.StopCycDiagCommWhileSymFRSignal)
				{
					busType = BusType.Bt_FlexRay;
					arg = Vocabulary.Flexray;
				}
				if (!modelValidator.DatabaseServices.HasDatabasesConfiguredFor(busType))
				{
					InformMessageBox.Info(string.Format(Resources.NoDatabaseAvailableForBustype, arg));
					return false;
				}
				if (dbManager.SelectSignalInDatabase(ref text, ref text2, ref value, ref text3, ref text4, ref busType, ref value2))
				{
					string message;
					if (!modelValidator.DatabaseServices.IsSymbolicSignalInsertAllowed(text2, text, text4, text3, busType, out message))
					{
						InformMessageBox.Error(message);
						return false;
					}
					SymbolicSignalEvent symbolicSignalEvent = new SymbolicSignalEvent();
					symbolicSignalEvent.MessageName.Value = text;
					symbolicSignalEvent.SignalName.Value = text2;
					symbolicSignalEvent.DatabaseName.Value = value;
					symbolicSignalEvent.DatabasePath.Value = modelValidator.GetFilePathRelativeToConfiguration(text3);
					symbolicSignalEvent.NetworkName.Value = text4;
					IList<uint> channelAssignmentOfDatabase = modelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(symbolicSignalEvent.DatabasePath.Value, text4);
					symbolicSignalEvent.ChannelNumber.Value = channelAssignmentOfDatabase[0];
					if (symbolicSignalEvent.ChannelNumber.Value == Database.ChannelNumber_FlexrayAB && BusType.Bt_FlexRay == busType)
					{
						symbolicSignalEvent.ChannelNumber.Value = 1u;
						if (text.EndsWith(Constants.FlexrayChannelB_Postfix))
						{
							symbolicSignalEvent.ChannelNumber.Value = 2u;
						}
					}
					symbolicSignalEvent.BusType.Value = busType;
					symbolicSignalEvent.IsFlexrayPDU.Value = value2;
					symbolicSignalEvent.LowValue.Value = 0.0;
					symbolicSignalEvent.HighValue.Value = 0.0;
					symbolicSignalEvent.Relation.Value = CondRelation.Equal;
					using (SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(modelValidator, dbManager, new List<CondRelation>
					{
						CondRelation.OnChange
					}))
					{
						symbolicSignalCondition.SignalEvent = new SymbolicSignalEvent(symbolicSignalEvent);
						if (DialogResult.OK == symbolicSignalCondition.ShowDialog())
						{
							symbolicSignalEvent.Assign(symbolicSignalCondition.SignalEvent);
							stopCycDiagCommEvent = symbolicSignalEvent;
							result = true;
							return result;
						}
					}
				}
				return false;
			}
			else
			{
				if (eventTypeName == Resources.StopCycDiagCommWhileCANData)
				{
					using (CANDataCondition cANDataCondition = new CANDataCondition(modelValidator, new List<CondRelation>
					{
						CondRelation.OnChange
					}))
					{
						cANDataCondition.InitDefaultValues();
						if (DialogResult.OK == cANDataCondition.ShowDialog())
						{
							stopCycDiagCommEvent = new CANDataEvent(cANDataCondition.CANDataEvent);
							result = true;
							return result;
						}
					}
					return false;
				}
				if (eventTypeName == Resources.StopCycDiagCommWhileLINData)
				{
					using (LINDataCondition lINDataCondition = new LINDataCondition(modelValidator, new List<CondRelation>
					{
						CondRelation.OnChange
					}))
					{
						lINDataCondition.InitDefaultValues();
						if (DialogResult.OK == lINDataCondition.ShowDialog())
						{
							stopCycDiagCommEvent = new LINDataEvent(lINDataCondition.LINDataEvent);
							result = true;
							return result;
						}
					}
					return false;
				}
				if (eventTypeName == Resources.StopCycDiagCommWhileCANMsgTimeout)
				{
					using (MsgTimeoutCondition msgTimeoutCondition = new MsgTimeoutCondition(modelValidator, dbManager))
					{
						msgTimeoutCondition.InitDefaultValues(BusType.Bt_CAN);
						if (DialogResult.OK == msgTimeoutCondition.ShowDialog())
						{
							stopCycDiagCommEvent = new MsgTimeoutEvent(msgTimeoutCondition.MsgTimeoutEvent);
							result = true;
							return result;
						}
					}
					return false;
				}
				if (eventTypeName == Resources.StopCycDiagCommWhileLINMsgTimeout)
				{
					using (MsgTimeoutCondition msgTimeoutCondition2 = new MsgTimeoutCondition(modelValidator, dbManager))
					{
						msgTimeoutCondition2.InitDefaultValues(BusType.Bt_LIN);
						if (DialogResult.OK == msgTimeoutCondition2.ShowDialog())
						{
							stopCycDiagCommEvent = new MsgTimeoutEvent(msgTimeoutCondition2.MsgTimeoutEvent);
							result = true;
							return result;
						}
					}
					return false;
				}
				if (eventTypeName == Resources.StopCycDiagCommWhileDigIn)
				{
					using (DigitalInputCondition digitalInputCondition = new DigitalInputCondition(modelValidator, true))
					{
						if (DialogResult.OK == digitalInputCondition.ShowDialog())
						{
							stopCycDiagCommEvent = new DigitalInputEvent(digitalInputCondition.DigitalInputEvent);
							result = true;
							return result;
						}
					}
					return false;
				}
				if (eventTypeName == Resources.StopCycDiagCommWhileIgnition)
				{
					using (IgnitionCondition ignitionCondition = new IgnitionCondition())
					{
						ignitionCondition.IgnitionEvent.IsOn.Value = false;
						if (DialogResult.OK == ignitionCondition.ShowDialog())
						{
							stopCycDiagCommEvent = new IgnitionEvent(ignitionCondition.IgnitionEvent);
							result = true;
							return result;
						}
					}
					return false;
				}
				if (eventTypeName == Resources.StopCycDiagCommNever)
				{
					return true;
				}
				if (!(eventTypeName == Resources.StopCycDiagCommWhileCcpXcpSignal))
				{
					return false;
				}
				if (!CcpXcpManager.Instance().CheckTriggerSignalEvents())
				{
					return false;
				}
				CcpXcpSignalEvent ccpXcpSignalEvent = new CcpXcpSignalEvent();
				ccpXcpSignalEvent.SignalName.Value = string.Empty;
				ccpXcpSignalEvent.LowValue.Value = 0.0;
				ccpXcpSignalEvent.HighValue.Value = 0.0;
				ccpXcpSignalEvent.Relation.Value = CondRelation.Equal;
				ccpXcpSignalEvent.CcpXcpEcuName.Value = string.Empty;
				using (SymbolicSignalCondition symbolicSignalCondition2 = new SymbolicSignalCondition(modelValidator, dbManager, new List<CondRelation>
				{
					CondRelation.OnChange
				}))
				{
					symbolicSignalCondition2.SignalEvent = new CcpXcpSignalEvent(ccpXcpSignalEvent);
					symbolicSignalCondition2.CcpXcpSignalConfiguration = CcpXcpManager.Instance().ConfigurationManagerService.CcpXcpSignalConfiguration;
					if (DialogResult.OK == symbolicSignalCondition2.ShowDialog())
					{
						ccpXcpSignalEvent.Assign(symbolicSignalCondition2.SignalEvent);
						stopCycDiagCommEvent = ccpXcpSignalEvent;
						result = true;
						return result;
					}
				}
				return false;
			}
			return result;
		}

		public static bool EditStopCycDiagCommEvent(IModelValidator modelValidator, IApplicationDatabaseManager dbManager, ref Event stopCycDiagCommEvent)
		{
			if (stopCycDiagCommEvent is ISymbolicSignalEvent)
			{
				using (SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(modelValidator, dbManager, new List<CondRelation>
				{
					CondRelation.OnChange
				}))
				{
					symbolicSignalCondition.CcpXcpSignalConfiguration = CcpXcpManager.Instance().ConfigurationManagerService.CcpXcpSignalConfiguration;
					bool result;
					if (stopCycDiagCommEvent is SymbolicSignalEvent)
					{
						symbolicSignalCondition.SignalEvent = new SymbolicSignalEvent(stopCycDiagCommEvent as SymbolicSignalEvent);
					}
					else
					{
						if (!(stopCycDiagCommEvent is CcpXcpSignalEvent))
						{
							result = false;
							return result;
						}
						symbolicSignalCondition.SignalEvent = new CcpXcpSignalEvent(stopCycDiagCommEvent as CcpXcpSignalEvent);
					}
					if (DialogResult.OK == symbolicSignalCondition.ShowDialog())
					{
						ISymbolicSignalEvent symbolicSignalEvent = stopCycDiagCommEvent as ISymbolicSignalEvent;
						if (!symbolicSignalEvent.Equals(symbolicSignalCondition.SignalEvent))
						{
							symbolicSignalEvent.Assign(symbolicSignalCondition.SignalEvent);
							result = true;
							return result;
						}
					}
					result = false;
					return result;
				}
			}
			if (stopCycDiagCommEvent is CANDataEvent)
			{
				using (CANDataCondition cANDataCondition = new CANDataCondition(modelValidator, new List<CondRelation>
				{
					CondRelation.OnChange
				}))
				{
					cANDataCondition.CANDataEvent = new CANDataEvent(stopCycDiagCommEvent as CANDataEvent);
					bool result;
					if (DialogResult.OK == cANDataCondition.ShowDialog())
					{
						CANDataEvent cANDataEvent = stopCycDiagCommEvent as CANDataEvent;
						if (!cANDataEvent.Equals(cANDataCondition.CANDataEvent))
						{
							cANDataEvent.Assign(cANDataCondition.CANDataEvent);
							result = true;
							return result;
						}
					}
					result = false;
					return result;
				}
			}
			if (stopCycDiagCommEvent is LINDataEvent)
			{
				using (LINDataCondition lINDataCondition = new LINDataCondition(modelValidator, new List<CondRelation>
				{
					CondRelation.OnChange
				}))
				{
					lINDataCondition.LINDataEvent = new LINDataEvent(stopCycDiagCommEvent as LINDataEvent);
					bool result;
					if (DialogResult.OK == lINDataCondition.ShowDialog())
					{
						LINDataEvent lINDataEvent = stopCycDiagCommEvent as LINDataEvent;
						if (!lINDataEvent.Equals(lINDataCondition.LINDataEvent))
						{
							lINDataEvent.Assign(lINDataCondition.LINDataEvent);
							result = true;
							return result;
						}
					}
					result = false;
					return result;
				}
			}
			if (stopCycDiagCommEvent is MsgTimeoutEvent)
			{
				using (MsgTimeoutCondition msgTimeoutCondition = new MsgTimeoutCondition(modelValidator, dbManager))
				{
					msgTimeoutCondition.MsgTimeoutEvent = new MsgTimeoutEvent(stopCycDiagCommEvent as MsgTimeoutEvent);
					bool result;
					if (DialogResult.OK == msgTimeoutCondition.ShowDialog())
					{
						MsgTimeoutEvent msgTimeoutEvent = stopCycDiagCommEvent as MsgTimeoutEvent;
						if (!msgTimeoutEvent.Equals(msgTimeoutCondition.MsgTimeoutEvent))
						{
							msgTimeoutEvent.Assign(msgTimeoutCondition.MsgTimeoutEvent);
							result = true;
							return result;
						}
					}
					result = false;
					return result;
				}
			}
			if (stopCycDiagCommEvent is DigitalInputEvent)
			{
				using (DigitalInputCondition digitalInputCondition = new DigitalInputCondition(modelValidator, true))
				{
					digitalInputCondition.DigitalInputEvent = new DigitalInputEvent(stopCycDiagCommEvent as DigitalInputEvent);
					bool result;
					if (DialogResult.OK == digitalInputCondition.ShowDialog())
					{
						DigitalInputEvent digitalInputEvent = stopCycDiagCommEvent as DigitalInputEvent;
						if (!digitalInputEvent.Equals(digitalInputCondition.DigitalInputEvent))
						{
							digitalInputEvent.Assign(digitalInputCondition.DigitalInputEvent);
							result = true;
							return result;
						}
					}
					result = false;
					return result;
				}
			}
			if (stopCycDiagCommEvent is IgnitionEvent)
			{
				using (IgnitionCondition ignitionCondition = new IgnitionCondition())
				{
					ignitionCondition.IgnitionEvent = new IgnitionEvent(stopCycDiagCommEvent as IgnitionEvent);
					bool result;
					if (DialogResult.OK == ignitionCondition.ShowDialog())
					{
						IgnitionEvent ignitionEvent = stopCycDiagCommEvent as IgnitionEvent;
						if (!ignitionEvent.Equals(ignitionCondition.IgnitionEvent))
						{
							ignitionEvent.Assign(ignitionCondition.IgnitionEvent);
							result = true;
							return result;
						}
					}
					result = false;
					return result;
				}
			}
			return false;
		}

		public static void DisplayStopCycDiagCommEventCondition(Event stopCyclicCommunicationEvent, IDatabaseServices databaseServices, ILoggerSpecifics loggerSpecifics, ref TextBox textBoxEventType, ref TextBox textBoxCondition, ref Button buttonChangeCondition)
		{
			textBoxEventType.Text = "";
			textBoxCondition.Text = "";
			textBoxCondition.Visible = true;
			buttonChangeCondition.Visible = true;
			if (stopCyclicCommunicationEvent == null)
			{
				textBoxEventType.Text = Resources.StopCycDiagCommNever;
				textBoxCondition.Visible = false;
				buttonChangeCondition.Visible = false;
				return;
			}
			if (stopCyclicCommunicationEvent is SymbolicSignalEvent)
			{
				SymbolicSignalEvent symbolicSignalEvent = stopCyclicCommunicationEvent as SymbolicSignalEvent;
				string arg;
				if (symbolicSignalEvent.BusType.Value == BusType.Bt_LIN)
				{
					textBoxEventType.Text = Resources.StopCycDiagCommWhileSymLINSignal;
					arg = GUIUtil.MapLINChannelNumber2String(symbolicSignalEvent.ChannelNumber.Value, loggerSpecifics);
				}
				else if (symbolicSignalEvent.BusType.Value == BusType.Bt_FlexRay)
				{
					textBoxEventType.Text = Resources.StopCycDiagCommWhileSymFRSignal;
					arg = GUIUtil.MapFlexrayChannelNumber2String(symbolicSignalEvent.ChannelNumber.Value);
				}
				else
				{
					textBoxEventType.Text = Resources.StopCycDiagCommWhileSymCANSignal;
					arg = GUIUtil.MapCANChannelNumber2String(symbolicSignalEvent.ChannelNumber.Value);
				}
				textBoxCondition.Text = string.Format(Resources.ChannelAndEventFormat, arg, GUIUtil.MapEventCondition2String(symbolicSignalEvent, databaseServices));
				return;
			}
			if (stopCyclicCommunicationEvent is CcpXcpSignalEvent)
			{
				CcpXcpSignalEvent symSigEvent = stopCyclicCommunicationEvent as CcpXcpSignalEvent;
				textBoxEventType.Text = Resources.StopCycDiagCommWhileCcpXcpSignal;
				textBoxCondition.Text = GUIUtil.MapEventCondition2String(symSigEvent, databaseServices);
				return;
			}
			if (stopCyclicCommunicationEvent is CANDataEvent)
			{
				textBoxEventType.Text = Resources.StopCycDiagCommWhileCANData;
				textBoxCondition.Text = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String((stopCyclicCommunicationEvent as CANDataEvent).ChannelNumber.Value), GUIUtil.MapEventCondition2String(stopCyclicCommunicationEvent as CANDataEvent));
				return;
			}
			if (stopCyclicCommunicationEvent is LINDataEvent)
			{
				textBoxEventType.Text = Resources.StopCycDiagCommWhileLINData;
				textBoxCondition.Text = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String((stopCyclicCommunicationEvent as LINDataEvent).ChannelNumber.Value, loggerSpecifics), GUIUtil.MapEventCondition2String(stopCyclicCommunicationEvent as LINDataEvent));
				return;
			}
			if (stopCyclicCommunicationEvent is MsgTimeoutEvent)
			{
				MsgTimeoutEvent msgTimeoutEvent = stopCyclicCommunicationEvent as MsgTimeoutEvent;
				if (msgTimeoutEvent.BusType.Value == BusType.Bt_CAN)
				{
					textBoxEventType.Text = Resources.StopCycDiagCommWhileCANMsgTimeout;
					textBoxCondition.Text = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String(msgTimeoutEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(msgTimeoutEvent, databaseServices));
					return;
				}
				if (msgTimeoutEvent.BusType.Value == BusType.Bt_LIN)
				{
					textBoxEventType.Text = Resources.StopCycDiagCommWhileLINMsgTimeout;
					textBoxCondition.Text = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String(msgTimeoutEvent.ChannelNumber.Value, loggerSpecifics), GUIUtil.MapEventCondition2String(msgTimeoutEvent, databaseServices));
					return;
				}
			}
			else
			{
				if (stopCyclicCommunicationEvent is DigitalInputEvent)
				{
					textBoxEventType.Text = Resources.StopCycDiagCommWhileDigIn;
					textBoxCondition.Text = string.Format("{0} == {1}", GUIUtil.MapDigitalInputNumber2String((stopCyclicCommunicationEvent as DigitalInputEvent).DigitalInput.Value), GUIUtil.MapEventCondition2String(stopCyclicCommunicationEvent as DigitalInputEvent, true));
					return;
				}
				if (stopCyclicCommunicationEvent is IgnitionEvent)
				{
					textBoxEventType.Text = Resources.StopCycDiagCommWhileIgnition;
					textBoxCondition.Text = GUIUtil.MapEventCondition2String(stopCyclicCommunicationEvent as IgnitionEvent);
				}
			}
		}
	}
}
