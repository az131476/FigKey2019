using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "TriggeredDiagnosticActionSequence")]
	public class TriggeredDiagnosticActionSequence
	{
		private DiagnosticDummyAction dummyAction;

		[DataMember(Name = "TriggeredDiagnosticActionSequenceActions")]
		private List<DiagnosticAction> actions;

		public ReadOnlyCollection<DiagnosticAction> Actions
		{
			get
			{
				if (this.actions.Any<DiagnosticAction>())
				{
					return new ReadOnlyCollection<DiagnosticAction>(this.actions);
				}
				if (this.dummyAction == null)
				{
					this.dummyAction = new DiagnosticDummyAction();
					this.dummyAction.TriggeredDiagnosticActionSequence = this;
				}
				return new ReadOnlyCollection<DiagnosticAction>(new List<DiagnosticAction>
				{
					this.dummyAction
				});
			}
		}

		[DataMember(Name = "TriggeredDiagnosticActionSequenceEvent")]
		public Event Event
		{
			get;
			set;
		}

		public TriggeredDiagnosticActionSequence(Event ev) : this()
		{
			this.Event = ev;
		}

		public TriggeredDiagnosticActionSequence(TriggeredDiagnosticActionSequence other) : this()
		{
			this.Event = (Event)other.Event.Clone();
			foreach (DiagnosticAction current in from a in other.Actions
			where !(a is DiagnosticDummyAction)
			select a)
			{
				if (current is DiagnosticSignalRequest)
				{
					this.AddAction(new DiagnosticSignalRequest(current as DiagnosticSignalRequest));
				}
				else
				{
					this.AddAction(new DiagnosticAction(current));
				}
			}
		}

		private TriggeredDiagnosticActionSequence()
		{
			this.actions = new List<DiagnosticAction>();
		}

		public void AddAction(DiagnosticAction action)
		{
			this.actions.Add(action);
			action.TriggeredDiagnosticActionSequence = this;
		}

		public void InsertAction(DiagnosticAction action, int insertPos)
		{
			if (insertPos > this.actions.Count - 1)
			{
				this.AddAction(action);
				return;
			}
			this.actions.Insert(insertPos, action);
			action.TriggeredDiagnosticActionSequence = this;
		}

		public bool RemoveAction(DiagnosticAction action)
		{
			if (this.actions.Contains(action))
			{
				this.actions.Remove(action);
				return true;
			}
			return false;
		}
	}
}
