using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GUI.XtraTreeListUtils;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.TriggersPage
{
	internal class EventPresenter : ICheckStateDataRecord, IDisposable
	{
		private readonly Dictionary<ulong, EventPresenter> mPresenterMap = new Dictionary<ulong, EventPresenter>();

		private readonly List<EventPresenter> mChildren = new List<EventPresenter>();

		private EventPresenter mDummyChild;

		private readonly Event mEvent;

		private readonly TriggerTree mTriggerTree;

		private MainImageList.IconIndex mBaseImageIndex;

		public event EventHandler DataChanged;

		public ulong Id
		{
			get
			{
				return this.Event.Id;
			}
		}

		public ulong ParentId
		{
			get
			{
				if (this.Parent == null)
				{
					return 0uL;
				}
				return this.Parent.Id;
			}
		}

		public int ImageIndex
		{
			get
			{
				if (this.DrawInactive)
				{
					return MainImageList.Instance.GetImageIndex(this.BaseImageIndex, true);
				}
				string text;
				MainImageList.IconIndex overlayIndex;
				if (this.Event is CombinedEvent && this.GetError(out text, out overlayIndex))
				{
					return MainImageList.Instance.GetImageIndex(this.BaseImageIndex, overlayIndex, ImageUtils.OverlayPos.BottomRight, true);
				}
				return MainImageList.Instance.GetImageIndex(this.BaseImageIndex, false);
			}
		}

		public virtual bool Active
		{
			get
			{
				return this.Parent == null || this.Parent.ChildIsActive(this);
			}
			set
			{
				if (this.Parent != null)
				{
					this.Parent.SetChildActive(this, value);
				}
			}
		}

		public virtual string EventStr
		{
			get
			{
				return this.GetEventString();
			}
		}

		public virtual string Channel
		{
			get
			{
				return this.GetChannelString();
			}
			set
			{
				bool flag = false;
				IEnumerable<IValidatedProperty> validatedPropertyListChannel = this.GetValidatedPropertyListChannel();
				foreach (IValidatedProperty current in validatedPropertyListChannel)
				{
					bool flag2 = false;
					if (current is IValidatedProperty<uint>)
					{
						this.PageValidator.Tree.UpdateModel<uint>(this.GetChannelValue<uint>(current as IValidatedProperty<uint>, value), current as IValidatedProperty<uint>, out flag2);
					}
					else if (current is IValidatedProperty<bool>)
					{
						this.PageValidator.Tree.UpdateModel<bool>(this.GetChannelValue<bool>(current as IValidatedProperty<bool>, value), current as IValidatedProperty<bool>, out flag2);
					}
					flag |= flag2;
				}
				this.Raise_DataChanged(flag);
			}
		}

		public virtual string Condition
		{
			get
			{
				return this.GetConditionString();
			}
		}

		public virtual string Type
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		public virtual string Action
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		public virtual string Name
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		public virtual string Comment
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		public EventPresenter Parent
		{
			get;
			private set;
		}

		private EventPresenter DummyChild
		{
			get
			{
				if (this.mDummyChild == null)
				{
					this.mDummyChild = new EventPresenter(new DummyEvent(), this.mTriggerTree, null);
				}
				this.mDummyChild.Parent = this;
				return this.mDummyChild;
			}
		}

		public bool DrawInactive
		{
			get
			{
				return !this.Active || (this.Parent != null && this.Parent.DrawInactive);
			}
		}

		public Event Event
		{
			get
			{
				return this.mEvent;
			}
		}

		protected IModelValidator ModelValidator
		{
			get
			{
				return this.mTriggerTree.ModelValidator;
			}
		}

		protected IModelEditor ModelEditor
		{
			get
			{
				return this.mTriggerTree.ModelEditor;
			}
		}

		protected PageValidator PageValidator
		{
			get
			{
				return this.mTriggerTree.PageValidator;
			}
		}

		protected TriggerMode TriggerMode
		{
			get
			{
				return this.mTriggerTree.TriggerConfiguration.TriggerMode.Value;
			}
		}

		protected MainImageList.IconIndex BaseImageIndex
		{
			get
			{
				if (this.mBaseImageIndex == MainImageList.IconIndex.NoImage)
				{
					this.mBaseImageIndex = GUIUtil.GetEventTypeIconIndex(this.Event);
				}
				return this.mBaseImageIndex;
			}
		}

		public bool CanCheck
		{
			get
			{
				return !(this.Event is DummyEvent);
			}
		}

		public CheckState CheckState
		{
			get
			{
				if (!this.Active)
				{
					return CheckState.Unchecked;
				}
				return CheckState.Checked;
			}
			set
			{
				this.Active = (value != CheckState.Unchecked);
			}
		}

		public bool GetError(out string text, out MainImageList.IconIndex index)
		{
			text = string.Empty;
			index = MainImageList.IconIndex.NoImage;
			IList<IValidatedProperty> errors = this.GetErrors();
			if (errors == null || !errors.Any<IValidatedProperty>())
			{
				return false;
			}
			IList<IValidatedProperty> source = (from t in errors
			where !string.IsNullOrEmpty(this.PageValidator.ResultCollector.GetErrorText(ValidationErrorClass.LocalModelError, t))
			select t).ToList<IValidatedProperty>();
			if (source.Any<IValidatedProperty>())
			{
				index = MainImageList.IconIndex.Error;
				text = this.PageValidator.ResultCollector.GetErrorText(ValidationErrorClass.LocalModelError, source.First<IValidatedProperty>());
				return true;
			}
			IList<IValidatedProperty> source2 = (from t in errors
			where !string.IsNullOrEmpty(this.PageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, t))
			select t).ToList<IValidatedProperty>();
			if (source2.Any<IValidatedProperty>())
			{
				index = MainImageList.IconIndex.Warning;
				text = this.PageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, source2.First<IValidatedProperty>());
				return true;
			}
			return false;
		}

		protected virtual IList<IValidatedProperty> GetErrors()
		{
			List<IValidatedProperty> list = new List<IValidatedProperty>();
			list.Add(this.Event.IsPointInTime);
			list.AddRange(this.GetValidatedPropertyListChannel());
			list.AddRange(this.GetValidatedPropertyListCondition());
			List<IValidatedProperty> list2 = list.Where(new Func<IValidatedProperty, bool>(this.PageValidator.General.HasError)).ToList<IValidatedProperty>();
			if (list2.Any<IValidatedProperty>())
			{
				return list2;
			}
			foreach (EventPresenter current in this.mChildren)
			{
				IList<IValidatedProperty> errors = current.GetErrors();
				if (errors.Any<IValidatedProperty>())
				{
					return errors;
				}
			}
			return new List<IValidatedProperty>();
		}

		protected void Raise_DataChanged(bool modified)
		{
			if (modified && this.DataChanged != null)
			{
				this.DataChanged(this, EventArgs.Empty);
			}
		}

		public EventPresenter(Event ev, TriggerTree triggerTree, EventPresenter parent = null)
		{
			this.mEvent = ev;
			this.mTriggerTree = triggerTree;
			this.Parent = parent;
			this.UpdateChildren();
		}

		public ReadOnlyCollection<EventPresenter> GetAllChildPresenters(bool recursive = true)
		{
			List<EventPresenter> list = new List<EventPresenter>();
			this.GetAllChildPresentersInternal(ref list, recursive);
			return new ReadOnlyCollection<EventPresenter>(list);
		}

		private void GetAllChildPresentersInternal(ref List<EventPresenter> allChildPresenters, bool recursive)
		{
			foreach (EventPresenter current in this.mChildren)
			{
				allChildPresenters.Add(current);
				if (recursive)
				{
					current.GetAllChildPresentersInternal(ref allChildPresenters, recursive);
				}
			}
		}

		public void UpdateChildren()
		{
			IList<Event> list = this.Event as CombinedEvent;
			if (list == null)
			{
				return;
			}
			List<ulong> dataModelEventIds = (from t in list
			select t.Id).ToList<ulong>();
			this.mChildren.Remove(this.DummyChild);
			List<ulong> list2 = (from t in this.mPresenterMap.Keys
			where !dataModelEventIds.Contains(t)
			select t).ToList<ulong>();
			foreach (ulong current in list2)
			{
				this.RemoveChild(this.mPresenterMap[current]);
			}
			List<Event> list3 = (from t in list
			where !this.mPresenterMap.ContainsKey(t.Id)
			select t).ToList<Event>();
			foreach (Event current2 in list3)
			{
				this.AddChild(new EventPresenter(current2, this.mTriggerTree, this));
			}
			for (int i = 0; i < dataModelEventIds.Count; i++)
			{
				ulong key = dataModelEventIds[i];
				EventPresenter item = this.mPresenterMap[key];
				int num = this.mChildren.IndexOf(item);
				if (num != i)
				{
					this.mChildren.Remove(item);
					this.mChildren.Insert(i, item);
				}
			}
			foreach (EventPresenter current3 in this.mChildren)
			{
				current3.UpdateChildren();
			}
			List<EventPresenter> list4 = this.mChildren.Where(new Func<EventPresenter, bool>(this.ChildIsActive)).ToList<EventPresenter>();
			if (list4.Count < 2)
			{
				this.mChildren.Add(this.DummyChild);
			}
		}

		private void AddChild(EventPresenter pres)
		{
			this.InsertChild(pres, this.mChildren.Count);
		}

		private void InsertChild(EventPresenter pres, int targetIndex)
		{
			if (pres == null || pres.Event == null || this.mPresenterMap.ContainsKey(pres.Id))
			{
				return;
			}
			if (targetIndex < 0 || targetIndex > this.mChildren.Count)
			{
				targetIndex = this.mChildren.Count;
			}
			this.mPresenterMap.Add(pres.Id, pres);
			this.mChildren.Insert(targetIndex, pres);
		}

		private void RemoveChild(EventPresenter pres)
		{
			if (pres == null || pres.Event == null || !this.mPresenterMap.ContainsKey(pres.Id))
			{
				return;
			}
			this.mPresenterMap.Remove(pres.Id);
			this.mChildren.Remove(pres);
			pres.Dispose();
		}

		private bool ChildIsActive(EventPresenter pres)
		{
			CombinedEvent combinedEvent = this.Event as CombinedEvent;
			return pres != null && combinedEvent != null && combinedEvent.ChildIsActive(pres.Event);
		}

		private void SetChildActive(EventPresenter pres, bool active)
		{
			CombinedEvent combinedEvent = this.Event as CombinedEvent;
			if (pres == null || combinedEvent == null)
			{
				return;
			}
			bool active2 = pres.Active;
			combinedEvent.SetChildActive(pres.Event, active);
			this.Raise_DataChanged(pres.Active != active2);
		}

		public int IndexOf(EventPresenter pres)
		{
			return this.mChildren.IndexOf(pres);
		}

		public virtual void Dispose()
		{
			foreach (EventPresenter current in this.mChildren)
			{
				current.Dispose();
			}
			this.mChildren.Clear();
			this.mPresenterMap.Clear();
			this.Parent = null;
		}

		public virtual bool IsReadOnlyAtAll(string fieldName)
		{
			switch (fieldName)
			{
			case "EventStr":
				return true;
			case "Channel":
				return this.Event is IgnitionEvent || this.Event is VoCanRecordingEvent || this.Event is CombinedEvent || this.Event is CcpXcpSignalEvent || this.Event is DiagnosticSignalEvent;
			case "Condition":
				return this.Event is CombinedEvent;
			case "Type":
				return true;
			case "Action":
				return true;
			case "Name":
				return true;
			case "Comment":
				return true;
			}
			return false;
		}

		public virtual bool IsReadOnlyByCellContent(string fieldName)
		{
			return this.Event is DummyEvent;
		}

		public virtual bool IsInactiveButEditable(string fieldName)
		{
			return this.DrawInactive;
		}

		private string GetEventString()
		{
			if (this.Event is CANIdEvent)
			{
				return Vocabulary.TriggerTypeNameColCANId;
			}
			if (this.Event is LINIdEvent)
			{
				return Vocabulary.TriggerTypeNameColLINId;
			}
			if (this.Event is FlexrayIdEvent)
			{
				return Resources_Trigger.TriggerTypeNameColFlexray;
			}
			if (this.Event is SymbolicMessageEvent)
			{
				SymbolicMessageEvent symbolicMessageEvent = this.Event as SymbolicMessageEvent;
				if (BusType.Bt_CAN == symbolicMessageEvent.BusType.Value)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicCAN;
				}
				if (BusType.Bt_LIN == symbolicMessageEvent.BusType.Value)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicLIN;
				}
				if (BusType.Bt_FlexRay == symbolicMessageEvent.BusType.Value)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicFlexray;
				}
			}
			else if (this.Event is SymbolicSignalEvent)
			{
				SymbolicSignalEvent symbolicSignalEvent = this.Event as SymbolicSignalEvent;
				if (BusType.Bt_CAN == symbolicSignalEvent.BusType.Value)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicSigCAN;
				}
				if (BusType.Bt_LIN == symbolicSignalEvent.BusType.Value)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicSigLIN;
				}
				if (BusType.Bt_FlexRay == symbolicSignalEvent.BusType.Value)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray;
				}
			}
			else
			{
				if (this.Event is CANDataEvent)
				{
					return Resources_Trigger.TriggerTypeNameColCANData;
				}
				if (this.Event is LINDataEvent)
				{
					return Resources_Trigger.TriggerTypeNameColLINData;
				}
				if (this.Event is MsgTimeoutEvent)
				{
					MsgTimeoutEvent msgTimeoutEvent = this.Event as MsgTimeoutEvent;
					if (BusType.Bt_CAN == msgTimeoutEvent.BusType.Value)
					{
						return Resources_Trigger.TriggerTypeNameColCANMsgTimeout;
					}
					if (BusType.Bt_LIN == msgTimeoutEvent.BusType.Value)
					{
						return Resources_Trigger.TriggerTypeNameColLINMsgTimeout;
					}
				}
				else
				{
					if (this.Event is DigitalInputEvent)
					{
						return Resources_Trigger.TriggerTypeNameColDigitalInput;
					}
					if (this.Event is AnalogInputEvent)
					{
						return Resources_Trigger.TriggerTypeNameColAnalogInput;
					}
					if (this.Event is KeyEvent)
					{
						return Resources_Trigger.TriggerTypeNameColKey;
					}
					if (this.Event is CANBusStatisticsEvent)
					{
						return Resources_Trigger.TriggerTypeNameColCANBusStatistics;
					}
					if (this.Event is IgnitionEvent)
					{
						return Resources_Trigger.TriggerTypeNameColIgnition;
					}
					if (this.Event is VoCanRecordingEvent)
					{
						return Resources_Trigger.TriggerTypeNameColVoCanRecording;
					}
					if (this.Event is CombinedEvent)
					{
						if (!(this.Event as CombinedEvent).IsConjunction.Value)
						{
							return Resources_Trigger.TriggerTypeNameColORCondition;
						}
						return Resources_Trigger.TriggerTypeNameColANDCondition;
					}
					else
					{
						if (this.Event is CcpXcpSignalEvent)
						{
							return Resources_Trigger.TriggerTypeNameColCcpXcpSignal;
						}
						if (this.Event is DiagnosticSignalEvent)
						{
							return Resources_Trigger.TriggerTypeNameColDiagnosticSignal;
						}
						if (this.Event is DummyEvent)
						{
							return Resources_Trigger.TriggerTypeNameColDummyEvent;
						}
						if (this.Event is IncEvent)
						{
							return Resources_Trigger.TriggerTypeNameColIncEvent;
						}
					}
				}
			}
			return string.Empty;
		}

		private string GetChannelString()
		{
			if (this.Event is CANIdEvent)
			{
				return GUIUtil.MapCANChannelNumber2String((this.Event as CANIdEvent).ChannelNumber.Value);
			}
			if (this.Event is LINIdEvent)
			{
				return GUIUtil.MapLINChannelNumber2String((this.Event as LINIdEvent).ChannelNumber.Value, this.ModelValidator.LoggerSpecifics);
			}
			if (this.Event is FlexrayIdEvent)
			{
				return GUIUtil.MapFlexrayChannelNumber2String((this.Event as FlexrayIdEvent).ChannelNumber.Value);
			}
			if (this.Event is SymbolicMessageEvent)
			{
				SymbolicMessageEvent symbolicMessageEvent = this.Event as SymbolicMessageEvent;
				if (BusType.Bt_CAN == symbolicMessageEvent.BusType.Value)
				{
					return GUIUtil.MapCANChannelNumber2String(symbolicMessageEvent.ChannelNumber.Value);
				}
				if (BusType.Bt_LIN == symbolicMessageEvent.BusType.Value)
				{
					return GUIUtil.MapLINChannelNumber2String(symbolicMessageEvent.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics);
				}
				if (BusType.Bt_FlexRay == symbolicMessageEvent.BusType.Value)
				{
					return GUIUtil.MapFlexrayChannelNumber2String(symbolicMessageEvent.ChannelNumber.Value);
				}
			}
			else if (this.Event is SymbolicSignalEvent)
			{
				ISymbolicSignalEvent symbolicSignalEvent = this.Event as ISymbolicSignalEvent;
				if (BusType.Bt_CAN == symbolicSignalEvent.BusType.Value)
				{
					return GUIUtil.MapCANChannelNumber2String(symbolicSignalEvent.ChannelNumber.Value);
				}
				if (BusType.Bt_LIN == symbolicSignalEvent.BusType.Value)
				{
					return GUIUtil.MapLINChannelNumber2String(symbolicSignalEvent.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics);
				}
				if (BusType.Bt_FlexRay == symbolicSignalEvent.BusType.Value)
				{
					return GUIUtil.MapFlexrayChannelNumber2String(symbolicSignalEvent.ChannelNumber.Value);
				}
			}
			else if (this.Event is MsgTimeoutEvent)
			{
				MsgTimeoutEvent msgTimeoutEvent = this.Event as MsgTimeoutEvent;
				if (BusType.Bt_CAN == msgTimeoutEvent.BusType.Value)
				{
					return GUIUtil.MapCANChannelNumber2String(msgTimeoutEvent.ChannelNumber.Value);
				}
				if (BusType.Bt_LIN == msgTimeoutEvent.BusType.Value)
				{
					return GUIUtil.MapLINChannelNumber2String(msgTimeoutEvent.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics);
				}
			}
			else
			{
				if (this.Event is DigitalInputEvent)
				{
					return GUIUtil.MapDigitalInputNumber2String((this.Event as DigitalInputEvent).DigitalInput.Value);
				}
				if (this.Event is AnalogInputEvent)
				{
					return GUIUtil.MapAnalogInputNumber2String((this.Event as AnalogInputEvent).InputNumber.Value);
				}
				if (this.Event is KeyEvent)
				{
					return GUIUtil.MapKeyNumber2String((this.Event as KeyEvent).Number.Value, (this.Event as KeyEvent).IsOnPanel.Value);
				}
				if (this.Event is CANDataEvent)
				{
					return GUIUtil.MapCANChannelNumber2String((this.Event as CANDataEvent).ChannelNumber.Value);
				}
				if (this.Event is LINDataEvent)
				{
					return GUIUtil.MapLINChannelNumber2String((this.Event as LINDataEvent).ChannelNumber.Value, this.ModelValidator.LoggerSpecifics);
				}
				if (this.Event is CANBusStatisticsEvent)
				{
					return GUIUtil.MapCANChannelNumber2String((this.Event as CANBusStatisticsEvent).ChannelNumber.Value);
				}
				if (this.Event is CcpXcpSignalEvent)
				{
					return Vocabulary.CcpXcp;
				}
				if (this.Event is DiagnosticSignalEvent)
				{
					return string.Empty;
				}
			}
			return string.Empty;
		}

		public IEnumerable<IValidatedProperty> GetValidatedPropertyListChannel()
		{
			List<IValidatedProperty> list = new List<IValidatedProperty>();
			if (this.Event is CANIdEvent)
			{
				list.Add((this.Event as CANIdEvent).ChannelNumber);
			}
			else if (this.Event is LINIdEvent)
			{
				list.Add((this.Event as LINIdEvent).ChannelNumber);
			}
			else if (this.Event is FlexrayIdEvent)
			{
				list.Add((this.Event as FlexrayIdEvent).ChannelNumber);
			}
			else if (this.Event is SymbolicMessageEvent)
			{
				list.Add((this.Event as SymbolicMessageEvent).ChannelNumber);
			}
			else if (this.Event is SymbolicSignalEvent)
			{
				list.Add((this.Event as SymbolicSignalEvent).ChannelNumber);
			}
			else if (this.Event is DigitalInputEvent)
			{
				list.Add((this.Event as DigitalInputEvent).DigitalInput);
			}
			else if (this.Event is AnalogInputEvent)
			{
				list.Add((this.Event as AnalogInputEvent).InputNumber);
			}
			else if (this.Event is CANDataEvent)
			{
				list.Add((this.Event as CANDataEvent).ChannelNumber);
			}
			else if (this.Event is LINDataEvent)
			{
				list.Add((this.Event as LINDataEvent).ChannelNumber);
			}
			else if (this.Event is MsgTimeoutEvent)
			{
				list.Add((this.Event as MsgTimeoutEvent).ChannelNumber);
			}
			else if (this.Event is KeyEvent)
			{
				list.Add((this.Event as KeyEvent).Number);
				list.Add((this.Event as KeyEvent).IsOnPanel);
			}
			else if (this.Event is CANBusStatisticsEvent)
			{
				list.Add((this.Event as CANBusStatisticsEvent).ChannelNumber);
			}
			return list;
		}

		private T GetChannelValue<T>(IValidatedProperty<T> validatedProperty, string value)
		{
			if (this.Event is CANIdEvent)
			{
				return (T)((object)Convert.ChangeType(GUIUtil.MapCANChannelString2Number(value), typeof(T)));
			}
			if (this.Event is LINIdEvent)
			{
				return (T)((object)Convert.ChangeType(GUIUtil.MapLINChannelString2Number(value), typeof(T)));
			}
			if (this.Event is FlexrayIdEvent)
			{
				return (T)((object)Convert.ChangeType(GUIUtil.MapFlexrayChannelString2Number(value), typeof(T)));
			}
			if (this.Event is SymbolicMessageEvent)
			{
				SymbolicMessageEvent symbolicMessageEvent = this.Event as SymbolicMessageEvent;
				if (BusType.Bt_CAN == symbolicMessageEvent.BusType.Value)
				{
					return (T)((object)Convert.ChangeType(GUIUtil.MapCANChannelString2Number(value), typeof(T)));
				}
				if (BusType.Bt_LIN == symbolicMessageEvent.BusType.Value)
				{
					return (T)((object)Convert.ChangeType(GUIUtil.MapLINChannelString2Number(value), typeof(T)));
				}
				if (BusType.Bt_FlexRay == symbolicMessageEvent.BusType.Value)
				{
					return (T)((object)Convert.ChangeType(GUIUtil.MapFlexrayChannelString2Number(value), typeof(T)));
				}
			}
			else if (this.Event is SymbolicSignalEvent)
			{
				SymbolicSignalEvent symbolicSignalEvent = this.Event as SymbolicSignalEvent;
				if (BusType.Bt_CAN == symbolicSignalEvent.BusType.Value)
				{
					return (T)((object)Convert.ChangeType(GUIUtil.MapCANChannelString2Number(value), typeof(T)));
				}
				if (BusType.Bt_LIN == symbolicSignalEvent.BusType.Value)
				{
					return (T)((object)Convert.ChangeType(GUIUtil.MapLINChannelString2Number(value), typeof(T)));
				}
				if (BusType.Bt_FlexRay == symbolicSignalEvent.BusType.Value)
				{
					return (T)((object)Convert.ChangeType(GUIUtil.MapFlexrayChannelString2Number(value), typeof(T)));
				}
			}
			else if (this.Event is MsgTimeoutEvent)
			{
				MsgTimeoutEvent msgTimeoutEvent = this.Event as MsgTimeoutEvent;
				if (BusType.Bt_CAN == msgTimeoutEvent.BusType.Value)
				{
					return (T)((object)Convert.ChangeType(GUIUtil.MapCANChannelString2Number(value), typeof(T)));
				}
				if (BusType.Bt_LIN == msgTimeoutEvent.BusType.Value)
				{
					return (T)((object)Convert.ChangeType(GUIUtil.MapLINChannelString2Number(value), typeof(T)));
				}
			}
			else
			{
				if (this.Event is DigitalInputEvent)
				{
					return (T)((object)Convert.ChangeType(GUIUtil.MapDigitalInputString2Number(value), typeof(T)));
				}
				if (this.Event is AnalogInputEvent)
				{
					return (T)((object)Convert.ChangeType(GUIUtil.MapAnalogInputString2Number(value), typeof(T)));
				}
				if (this.Event is KeyEvent)
				{
					KeyEvent keyEvent = this.Event as KeyEvent;
					bool flag;
					uint num = GUIUtil.MapStringToKeyNumber(value, out flag);
					if (validatedProperty == keyEvent.Number)
					{
						return (T)((object)Convert.ChangeType(num, typeof(T)));
					}
					if (validatedProperty == keyEvent.IsOnPanel)
					{
						return (T)((object)Convert.ChangeType(flag, typeof(T)));
					}
				}
				else
				{
					if (this.Event is CANDataEvent)
					{
						return (T)((object)Convert.ChangeType(GUIUtil.MapCANChannelString2Number(value), typeof(T)));
					}
					if (this.Event is LINDataEvent)
					{
						return (T)((object)Convert.ChangeType(GUIUtil.MapLINChannelString2Number(value), typeof(T)));
					}
					if (this.Event is CANBusStatisticsEvent)
					{
						return (T)((object)Convert.ChangeType(GUIUtil.MapCANChannelString2Number(value), typeof(T)));
					}
				}
			}
			return default(T);
		}

		private string GetConditionString()
		{
			if (this.Event is IdEvent)
			{
				return GUIUtil.MapEventCondition2String(this.Event as IdEvent);
			}
			if (this.Event is SymbolicMessageEvent)
			{
				return GUIUtil.MapEventCondition2String(this.Event as SymbolicMessageEvent, this.ModelValidator.DatabaseServices);
			}
			if (this.Event is ISymbolicSignalEvent)
			{
				return GUIUtil.MapEventCondition2String(this.Event as ISymbolicSignalEvent, this.ModelValidator.DatabaseServices);
			}
			if (this.Event is DigitalInputEvent)
			{
				return GUIUtil.MapEventCondition2String(this.Event as DigitalInputEvent);
			}
			if (this.Event is AnalogInputEvent)
			{
				return GUIUtil.MapEventCondition2String(this.Event as AnalogInputEvent);
			}
			if (this.Event is CANDataEvent)
			{
				return GUIUtil.MapEventCondition2String(this.Event as CANDataEvent);
			}
			if (this.Event is LINDataEvent)
			{
				return GUIUtil.MapEventCondition2String(this.Event as LINDataEvent);
			}
			if (this.Event is MsgTimeoutEvent)
			{
				return GUIUtil.MapEventCondition2String(this.Event as MsgTimeoutEvent, this.ModelValidator.DatabaseServices);
			}
			if (this.Event is KeyEvent)
			{
				string format = Resources_Trigger.TriggerKeyOnRemoteControl;
				if ((this.Event as KeyEvent).IsOnPanel.Value)
				{
					format = Resources_Trigger.TriggerKeyOnPanel;
				}
				else if ((this.Event as KeyEvent).IsCasKey)
				{
					format = Resources_Trigger.TriggerKeyOnCas;
				}
				return string.Format(format, string.Empty);
			}
			if (this.Event is CANBusStatisticsEvent)
			{
				return GUIUtil.MapEventCondition2String(this.Event as CANBusStatisticsEvent);
			}
			if (this.Event is IgnitionEvent)
			{
				return GUIUtil.MapEventCondition2String(this.Event as IgnitionEvent);
			}
			if (this.Event is VoCanRecordingEvent)
			{
				return GUIUtil.MapEventCondition2String(this.Event as VoCanRecordingEvent);
			}
			if (this.Event is CombinedEvent)
			{
				return GUIUtil.MapEventCondition2String(this.Event as CombinedEvent);
			}
			if (this.Event is IncEvent)
			{
				return GUIUtil.MapEventCondition2String(this.Event as IncEvent);
			}
			return string.Empty;
		}

		public IEnumerable<IValidatedProperty> GetValidatedPropertyListCondition()
		{
			List<IValidatedProperty> list = new List<IValidatedProperty>();
			if (this.Event is IdEvent)
			{
				list.Add((this.Event as IdEvent).IdRelation);
				list.Add((this.Event as IdEvent).LowId);
				list.Add((this.Event as IdEvent).HighId);
				if (this.Event is CANIdEvent)
				{
					list.Add((this.Event as CANIdEvent).IsExtendedId);
				}
			}
			else if (this.Event is SymbolicMessageEvent)
			{
				list.Add((this.Event as SymbolicMessageEvent).DatabasePath);
				list.Add((this.Event as SymbolicMessageEvent).MessageName);
				list.Add((this.Event as SymbolicMessageEvent).BusType);
			}
			else if (this.Event is ISymbolicSignalEvent)
			{
				list.Add((this.Event as ISymbolicSignalEvent).LowValue);
				list.Add((this.Event as ISymbolicSignalEvent).HighValue);
				if (this.Event is SymbolicSignalEvent)
				{
					list.Add((this.Event as ISymbolicSignalEvent).MessageName);
				}
				list.Add((this.Event as ISymbolicSignalEvent).SignalName);
				list.Add((this.Event as ISymbolicSignalEvent).Relation);
			}
			else if (this.Event is DigitalInputEvent)
			{
				list.Add((this.Event as DigitalInputEvent).Edge);
			}
			else if (this.Event is AnalogInputEvent)
			{
				list.Add((this.Event as AnalogInputEvent).LowValue);
				list.Add((this.Event as AnalogInputEvent).HighValue);
				list.Add((this.Event as AnalogInputEvent).Relation);
				list.Add((this.Event as AnalogInputEvent).Tolerance);
			}
			else if (this.Event is CANDataEvent)
			{
				CANDataEvent cANDataEvent = this.Event as CANDataEvent;
				list.Add(cANDataEvent.ID);
				list.Add(cANDataEvent.IsExtendedId);
				if (cANDataEvent.RawDataSignal is RawDataSignalByte)
				{
					list.Add((cANDataEvent.RawDataSignal as RawDataSignalByte).DataBytePos);
				}
				else if (cANDataEvent.RawDataSignal is RawDataSignalStartbitLength)
				{
					list.Add((cANDataEvent.RawDataSignal as RawDataSignalStartbitLength).StartbitPos);
					list.Add((cANDataEvent.RawDataSignal as RawDataSignalStartbitLength).Length);
					list.Add((cANDataEvent.RawDataSignal as RawDataSignalStartbitLength).IsMotorola);
				}
				list.Add(cANDataEvent.Relation);
				list.Add(cANDataEvent.LowValue);
				list.Add(cANDataEvent.HighValue);
			}
			else if (this.Event is LINDataEvent)
			{
				LINDataEvent lINDataEvent = this.Event as LINDataEvent;
				list.Add(lINDataEvent.ID);
				if (lINDataEvent.RawDataSignal is RawDataSignalByte)
				{
					list.Add((lINDataEvent.RawDataSignal as RawDataSignalByte).DataBytePos);
				}
				else if (lINDataEvent.RawDataSignal is RawDataSignalStartbitLength)
				{
					list.Add((lINDataEvent.RawDataSignal as RawDataSignalStartbitLength).StartbitPos);
					list.Add((lINDataEvent.RawDataSignal as RawDataSignalStartbitLength).Length);
					list.Add((lINDataEvent.RawDataSignal as RawDataSignalStartbitLength).IsMotorola);
				}
				list.Add(lINDataEvent.Relation);
				list.Add(lINDataEvent.LowValue);
				list.Add(lINDataEvent.HighValue);
			}
			else if (this.Event is MsgTimeoutEvent)
			{
				list.Add((this.Event as MsgTimeoutEvent).DatabasePath);
				list.Add((this.Event as MsgTimeoutEvent).DatabaseName);
				list.Add((this.Event as MsgTimeoutEvent).MessageName);
				list.Add((this.Event as MsgTimeoutEvent).NetworkName);
				list.Add((this.Event as MsgTimeoutEvent).BusType);
				list.Add((this.Event as MsgTimeoutEvent).IsSymbolic);
				list.Add((this.Event as MsgTimeoutEvent).ID);
				list.Add((this.Event as MsgTimeoutEvent).IsExtendedId);
				list.Add((this.Event as MsgTimeoutEvent).IsCycletimeFromDatabase);
				list.Add((this.Event as MsgTimeoutEvent).UserDefinedCycleTime);
				list.Add((this.Event as MsgTimeoutEvent).MaxDelay);
			}
			else if (this.Event is KeyEvent)
			{
				list.Add((this.Event as KeyEvent).IsOnPanel);
			}
			else if (this.Event is CANBusStatisticsEvent)
			{
				list.Add((this.Event as CANBusStatisticsEvent).IsBusloadEnabled);
				list.Add((this.Event as CANBusStatisticsEvent).BusloadRelation);
				list.Add((this.Event as CANBusStatisticsEvent).BusloadLow);
				list.Add((this.Event as CANBusStatisticsEvent).BusloadHigh);
				list.Add((this.Event as CANBusStatisticsEvent).IsErrorFramesEnabled);
				list.Add((this.Event as CANBusStatisticsEvent).ErrorFramesRelation);
				list.Add((this.Event as CANBusStatisticsEvent).ErrorFramesLow);
			}
			else if (this.Event is IgnitionEvent)
			{
				list.Add((this.Event as IgnitionEvent).IsOn);
			}
			else if (this.Event is VoCanRecordingEvent)
			{
				list.Add((this.Event as VoCanRecordingEvent).Duration_s);
				list.Add((this.Event as VoCanRecordingEvent).IsBeepOnEndOn);
				list.Add((this.Event as VoCanRecordingEvent).IsRecordingLEDActive);
			}
			else if (this.Event is CombinedEvent)
			{
				list.Add((this.Event as CombinedEvent).IsConjunction);
			}
			else if (this.Event is IncEvent)
			{
				list.Add((this.Event as IncEvent).ParamIndex);
			}
			return list;
		}
	}
}
