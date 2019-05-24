using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.TriggersPage
{
	internal class RecordTriggerPresenter : EventPresenter
	{
		public readonly RecordTrigger RecordTrigger;

		public override bool Active
		{
			get
			{
				return this.RecordTrigger.IsActive.Value;
			}
			set
			{
				bool modified;
				if (base.PageValidator.Tree.UpdateModel<bool>(value, this.RecordTrigger.IsActive, out modified))
				{
					base.Raise_DataChanged(modified);
				}
			}
		}

		public override string Channel
		{
			get
			{
				return base.Channel;
			}
			set
			{
				bool hasDefaultName = this.HasDefaultName;
				string channel = this.Channel;
				base.Channel = value;
				if (hasDefaultName && !this.Channel.Equals(channel))
				{
					this.Name = this.DefaultName;
				}
			}
		}

		public override string Type
		{
			get
			{
				return GUIUtil.MapTriggerEffect2String(this.RecordTrigger.TriggerEffect.Value);
			}
			set
			{
				bool modified;
				if (base.PageValidator.Tree.UpdateModel<TriggerEffect>(GUIUtil.MapTriggerString2Effect(value), this.RecordTrigger.TriggerEffect, out modified))
				{
					this.ResetTypeDependentMembers(modified);
					base.Raise_DataChanged(modified);
				}
			}
		}

		public override string Action
		{
			get
			{
				return GUIUtil.MapTriggerAction2String(this.RecordTrigger.Action.Value);
			}
			set
			{
				bool modified;
				if (base.PageValidator.Tree.UpdateModel<TriggerAction>(GUIUtil.MapString2TriggerAction(value), this.RecordTrigger.Action, out modified))
				{
					base.Raise_DataChanged(modified);
				}
			}
		}

		public override string Name
		{
			get
			{
				return this.RecordTrigger.Name.Value ?? string.Empty;
			}
			set
			{
				if ((this.RecordTrigger.TriggerEffect.Value == TriggerEffect.Marker || base.TriggerMode == TriggerMode.Triggered) && string.IsNullOrEmpty(value.Trim()) && DialogResult.Yes == InformMessageBox.Show(EnumInfoType.Warning, EnumQuestionType.Question, Resources_Trigger.InvalidTriggerNameEmptyUseDefault))
				{
					value = this.DefaultName;
				}
				bool flag;
				if (base.PageValidator.Tree.UpdateModel<string>(value, this.RecordTrigger.Name, out flag))
				{
					if (flag)
					{
						base.ModelEditor.UpdateReferencedTriggerNameInDataTransferTriggers(this.RecordTrigger.Event.Id, value);
					}
					base.Raise_DataChanged(flag);
				}
			}
		}

		public override string Comment
		{
			get
			{
				return this.RecordTrigger.Comment.Value ?? string.Empty;
			}
			set
			{
				bool modified;
				if (base.PageValidator.Tree.UpdateModel<string>(value, this.RecordTrigger.Comment, out modified))
				{
					base.Raise_DataChanged(modified);
				}
			}
		}

		public bool HasDefaultName
		{
			get
			{
				return this.Name.Equals(this.DefaultName);
			}
		}

		public string DefaultName
		{
			get
			{
				string str;
				if (this.RecordTrigger.TriggerEffect.Value != TriggerEffect.Marker && base.TriggerMode != TriggerMode.Triggered)
				{
					str = string.Empty;
				}
				else if (this.RecordTrigger.Event is SymbolicMessageEvent)
				{
					str = GUIUtil.MapEventCondition2MarkerName(this.RecordTrigger.Event as SymbolicMessageEvent, base.ModelValidator.DatabaseServices);
				}
				else if (this.RecordTrigger.Event is SymbolicSignalEvent)
				{
					str = GUIUtil.MapEventCondition2MarkerName(this.RecordTrigger.Event as SymbolicSignalEvent, base.ModelValidator.DatabaseServices);
				}
				else if (this.RecordTrigger.Event is CcpXcpSignalEvent)
				{
					str = GUIUtil.MapEventCondition2MarkerName(this.RecordTrigger.Event as CcpXcpSignalEvent, base.ModelValidator.DatabaseServices);
				}
				else if (this.RecordTrigger.Event is DiagnosticSignalEvent)
				{
					str = GUIUtil.MapEventCondition2MarkerName(this.RecordTrigger.Event as DiagnosticSignalEvent, base.ModelValidator.DatabaseServices);
				}
				else if (this.RecordTrigger.Event is IdEvent)
				{
					str = GUIUtil.MapEventCondition2MarkerName(this.RecordTrigger.Event as IdEvent);
				}
				else if (this.RecordTrigger.Event is CANDataEvent)
				{
					str = GUIUtil.MapEventCondition2MarkerName(this.RecordTrigger.Event as CANDataEvent);
				}
				else if (this.RecordTrigger.Event is LINDataEvent)
				{
					str = GUIUtil.MapEventCondition2MarkerName(this.RecordTrigger.Event as LINDataEvent);
				}
				else if (this.RecordTrigger.Event is CANBusStatisticsEvent)
				{
					str = GUIUtil.MapEventCondition2MarkerName(this.RecordTrigger.Event as CANBusStatisticsEvent);
				}
				else if (this.RecordTrigger.Event is DigitalInputEvent)
				{
					str = GUIUtil.MapEventCondition2MarkerName(this.RecordTrigger.Event as DigitalInputEvent);
				}
				else if (this.RecordTrigger.Event is AnalogInputEvent)
				{
					str = GUIUtil.MapEventCondition2MarkerName(this.RecordTrigger.Event as AnalogInputEvent);
				}
				else if (this.RecordTrigger.Event is KeyEvent)
				{
					KeyEvent keyEvent = this.RecordTrigger.Event as KeyEvent;
					if ((this.RecordTrigger.Event as KeyEvent).IsOnPanel.Value)
					{
						str = Vocabulary.PanelKey + keyEvent.Number;
					}
					else if ((this.RecordTrigger.Event as KeyEvent).IsCasKey)
					{
						str = Vocabulary.CasKey + (keyEvent.Number.Value - Constants.CasKeyOffset);
					}
					else
					{
						str = Vocabulary.Key + keyEvent.Number;
					}
				}
				else if (this.RecordTrigger.Event is IgnitionEvent)
				{
					str = GUIUtil.MapEventCondition2MarkerName(this.RecordTrigger.Event as IgnitionEvent);
				}
				else if (this.RecordTrigger.Event is VoCanRecordingEvent)
				{
					str = Vocabulary.VoCan;
				}
				else if (this.RecordTrigger.Event is MsgTimeoutEvent)
				{
					str = GUIUtil.MapEventCondition2MarkerName(this.RecordTrigger.Event as MsgTimeoutEvent, base.ModelValidator.DatabaseServices);
				}
				else if (this.RecordTrigger.Event is IncEvent)
				{
					str = GUIUtil.MapEventCondition2MarkerName(this.RecordTrigger.Event as IncEvent);
				}
				else
				{
					str = this.Condition;
				}
				return base.ModelValidator.ReplaceInvalidMarkerNameCharactersIfPossible(str);
			}
		}

		protected override IList<IValidatedProperty> GetErrors()
		{
			List<IValidatedProperty> source = new List<IValidatedProperty>
			{
				this.RecordTrigger.IsActive,
				this.RecordTrigger.TriggerEffect,
				this.RecordTrigger.Action,
				this.RecordTrigger.Name,
				this.RecordTrigger.Comment
			};
			List<IValidatedProperty> list = source.Where(new Func<IValidatedProperty, bool>(base.PageValidator.General.HasError)).ToList<IValidatedProperty>();
			if (!list.Any<IValidatedProperty>())
			{
				return base.GetErrors();
			}
			return list;
		}

		public RecordTriggerPresenter(RecordTrigger recordTrigger, TriggerTree triggerTree) : base(recordTrigger.Event, triggerTree, null)
		{
			this.RecordTrigger = recordTrigger;
		}

		public override bool IsReadOnlyAtAll(string fieldName)
		{
			if (fieldName != null)
			{
				if (fieldName == "Channel")
				{
					return base.Event is IncEvent;
				}
				if (fieldName == "Type")
				{
					return base.TriggerMode == TriggerMode.Permanent;
				}
				if (fieldName == "Action")
				{
					return this.RecordTrigger.TriggerEffect.Value == TriggerEffect.Marker && base.TriggerMode != TriggerMode.OnOff;
				}
				if (fieldName == "Name")
				{
					return this.RecordTrigger.TriggerEffect.Value != TriggerEffect.Marker && base.TriggerMode == TriggerMode.Permanent;
				}
				if (fieldName == "Comment")
				{
					return this.RecordTrigger.TriggerEffect.Value == TriggerEffect.Marker && base.TriggerMode != TriggerMode.OnOff;
				}
			}
			return base.IsReadOnlyAtAll(fieldName);
		}

		public override bool IsReadOnlyByCellContent(string fieldName)
		{
			if (fieldName != null)
			{
				if (fieldName == "Type")
				{
					return false;
				}
				if (fieldName == "Action")
				{
					return this.RecordTrigger.TriggerEffect.Value == TriggerEffect.Marker && base.TriggerMode == TriggerMode.OnOff;
				}
				if (fieldName == "Name")
				{
					return this.RecordTrigger.TriggerEffect.Value != TriggerEffect.Marker && base.TriggerMode == TriggerMode.OnOff;
				}
				if (fieldName == "Comment")
				{
					return this.RecordTrigger.TriggerEffect.Value == TriggerEffect.Marker && base.TriggerMode == TriggerMode.OnOff;
				}
			}
			return base.IsReadOnlyByCellContent(fieldName);
		}

		private void ResetTypeDependentMembers(bool modified)
		{
			if (!modified)
			{
				return;
			}
			if (this.RecordTrigger.TriggerEffect.Value == TriggerEffect.Marker)
			{
				this.Action = GUIUtil.MapTriggerAction2String(TriggerAction.None);
				this.Comment = string.Empty;
				if (string.IsNullOrEmpty(this.Name))
				{
					this.Name = this.DefaultName;
					return;
				}
			}
			else if (this.RecordTrigger.TriggerEffect.Value != TriggerEffect.Unknown)
			{
				this.Name = this.DefaultName;
			}
		}
	}
}
