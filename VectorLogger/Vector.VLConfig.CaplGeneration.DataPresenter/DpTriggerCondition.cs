using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal abstract class DpTriggerCondition
	{
		private readonly Event mEvent;

		protected readonly List<DpTriggerCondition> mChildConditionListFlat = new List<DpTriggerCondition>();

		public uint Ident
		{
			get;
			private set;
		}

		public uint TriggerIdent
		{
			get;
			private set;
		}

		public bool IsPointInTime
		{
			get
			{
				return this.mEvent.IsPointInTime.Value;
			}
		}

		public bool IsSingleCondition
		{
			get;
			set;
		}

		public bool IsCombinedConditionRoot
		{
			get;
			protected set;
		}

		public string CombinedCondition
		{
			get;
			protected set;
		}

		public ReadOnlyCollection<DpTriggerCondition> ChildConditionListFlat
		{
			get
			{
				return new ReadOnlyCollection<DpTriggerCondition>(this.mChildConditionListFlat);
			}
		}

		public abstract string ChannelString
		{
			get;
		}

		public abstract string NameOfCaplEventHandler
		{
			get;
		}

		public virtual string TemplateVariables
		{
			get
			{
				return "TVariables_Condition";
			}
		}

		public abstract string TemplateEventHandler
		{
			get;
		}

		public virtual bool TemplateCallsProcessTriggerInEventHandler
		{
			get
			{
				return false;
			}
		}

		public virtual string TemplateAdditionalFunction
		{
			get
			{
				return null;
			}
		}

		public virtual string TemplateStartupCode
		{
			get
			{
				return null;
			}
		}

		public virtual string ORCondOfPointInTimeConditions
		{
			get
			{
				return string.Empty;
			}
		}

		public virtual bool SimulateGlBehaviour
		{
			get
			{
				return false;
			}
		}

		public virtual string SimulateGlBehaviourCondition
		{
			get
			{
				return string.Empty;
			}
		}

		protected DpTriggerCondition(uint triggerIdent, uint conditionIdent, Event conditionEvent)
		{
			this.TriggerIdent = triggerIdent;
			this.Ident = conditionIdent;
			this.mEvent = conditionEvent;
			this.IsSingleCondition = true;
			this.IsCombinedConditionRoot = false;
			this.CombinedCondition = "gCond" + this.Ident;
		}

		public static DpTriggerCondition Create(uint triggerIdent, uint conditionIdent, Event conditionEvent, bool isTrigger)
		{
			DpTriggerCondition result = null;
			if (conditionEvent is CombinedEvent)
			{
				result = new DpTriggerConditionCombined(triggerIdent, conditionIdent, conditionEvent, isTrigger);
			}
			else if (conditionEvent is CANIdEvent)
			{
				result = new DpTriggerConditionCanId(triggerIdent, conditionIdent, conditionEvent);
			}
			else if (conditionEvent is LINIdEvent)
			{
				result = new DpTriggerConditionLinId(triggerIdent, conditionIdent, conditionEvent);
			}
			else if (conditionEvent is CANDataEvent)
			{
				result = new DpTriggerConditionCanData(triggerIdent, conditionIdent, conditionEvent);
			}
			else if (conditionEvent is LINDataEvent)
			{
				result = new DpTriggerConditionLinData(triggerIdent, conditionIdent, conditionEvent);
			}
			else if (conditionEvent is SymbolicMessageEvent)
			{
				SymbolicMessageEvent symbolicMessageEvent = conditionEvent as SymbolicMessageEvent;
				if (symbolicMessageEvent.BusType.Value == BusType.Bt_CAN)
				{
					result = new DpTriggerConditionSymbolicMessageCan(triggerIdent, conditionIdent, conditionEvent);
				}
				else if (symbolicMessageEvent.BusType.Value == BusType.Bt_LIN)
				{
					result = new DpTriggerConditionSymbolicMessageLin(triggerIdent, conditionIdent, conditionEvent);
				}
			}
			else if (conditionEvent is SymbolicSignalEvent)
			{
				SymbolicSignalEvent symbolicSignalEvent = conditionEvent as SymbolicSignalEvent;
				if (symbolicSignalEvent.BusType.Value == BusType.Bt_CAN)
				{
					result = new DpTriggerConditionSymbolicSignalCan(triggerIdent, conditionIdent, conditionEvent);
				}
				else if (symbolicSignalEvent.BusType.Value == BusType.Bt_LIN)
				{
					result = new DpTriggerConditionSymbolicSignalLin(triggerIdent, conditionIdent, conditionEvent);
				}
			}
			else if (conditionEvent is MsgTimeoutEvent)
			{
				MsgTimeoutEvent msgTimeoutEvent = conditionEvent as MsgTimeoutEvent;
				if (msgTimeoutEvent.BusType.Value == BusType.Bt_CAN)
				{
					result = new DpTriggerConditionMsgTimeoutCan(triggerIdent, conditionIdent, conditionEvent);
				}
				else if (msgTimeoutEvent.BusType.Value == BusType.Bt_LIN)
				{
					result = new DpTriggerConditionMsgTimeoutLin(triggerIdent, conditionIdent, conditionEvent);
				}
			}
			else if (conditionEvent is AnalogInputEvent)
			{
				result = new DpTriggerConditionAnalogInput(triggerIdent, conditionIdent, conditionEvent);
			}
			else if (conditionEvent is DigitalInputEvent)
			{
				result = new DpTriggerConditionDigitalInput(triggerIdent, conditionIdent, conditionEvent);
			}
			return result;
		}

		public virtual bool Validate(IApplicationDatabaseManager dbManager, string configurationFolderPath)
		{
			return true;
		}
	}
}
