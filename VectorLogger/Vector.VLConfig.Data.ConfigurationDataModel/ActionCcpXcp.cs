using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "ActionCcpXcp")]
	public class ActionCcpXcp : Action
	{
		public enum ActivationMode
		{
			Always,
			Triggered,
			Conditional
		}

		[DataMember(Name = "ActionCcpXcpSignalList")]
		private List<CcpXcpSignal> signalList;

		private CcpXcpSignalDummy dummySignal;

		[DataMember(Name = "ActionCcpXcpStartDelay")]
		public ValidatedProperty<uint> StartDelay
		{
			get;
			set;
		}

		[DataMember(Name = "ActionCcpXcpStopDelay")]
		public ValidatedProperty<uint> StopDelay
		{
			get;
			set;
		}

		public ReadOnlyCollection<CcpXcpSignal> Signals
		{
			get
			{
				if (this.signalList.Any<CcpXcpSignal>())
				{
					return new ReadOnlyCollection<CcpXcpSignal>(this.signalList);
				}
				if (this.dummySignal == null)
				{
					this.dummySignal = new CcpXcpSignalDummy();
					this.dummySignal.IsActive.Value = false;
					this.dummySignal.ActionCcpXcp = this;
				}
				return new ReadOnlyCollection<CcpXcpSignal>(new List<CcpXcpSignal>
				{
					this.dummySignal
				});
			}
		}

		public ReadOnlyCollection<CcpXcpSignal> ActiveSignals
		{
			get
			{
				return new ReadOnlyCollection<CcpXcpSignal>((from CcpXcpSignal sig in this.signalList
				where sig.IsActive.Value
				select sig).ToList<CcpXcpSignal>());
			}
		}

		public ActionCcpXcp.ActivationMode Mode
		{
			get
			{
				if (this.Event == null)
				{
					return ActionCcpXcp.ActivationMode.Always;
				}
				if (this.StopType is StopImmediate)
				{
					return ActionCcpXcp.ActivationMode.Triggered;
				}
				return ActionCcpXcp.ActivationMode.Conditional;
			}
		}

		public bool IsDirty
		{
			get;
			private set;
		}

		public ActionCcpXcp(Event ev)
		{
			this.Event = ev;
			this.signalList = new List<CcpXcpSignal>();
			this.StartDelay = new ValidatedProperty<uint>(0u);
			this.StopDelay = new ValidatedProperty<uint>(0u);
			this.IsDirty = true;
		}

		public ActionCcpXcp(ActionCcpXcp other) : this()
		{
			this.Assign(other);
		}

		private ActionCcpXcp()
		{
			this.signalList = new List<CcpXcpSignal>();
			this.StartDelay = new ValidatedProperty<uint>(0u);
			this.StopDelay = new ValidatedProperty<uint>(0u);
			this.IsDirty = true;
		}

		public override object Clone()
		{
			return new ActionCcpXcp(this);
		}

		public void SetClean()
		{
			this.IsDirty = false;
		}

		public override bool Equals(Action action)
		{
			if (action == null || base.GetType() != action.GetType())
			{
				return false;
			}
			ActionCcpXcp actionCcpXcp = (ActionCcpXcp)action;
			return this.StartDelay == actionCcpXcp.StartDelay && this.StopDelay == actionCcpXcp.StopDelay && this.Signals.SequenceEqual(actionCcpXcp.Signals) && base.Equals(action);
		}

		public override int GetHashCode()
		{
			int num = 0;
			foreach (CcpXcpSignal current in this.Signals)
			{
				num ^= current.GetHashCode();
			}
			return this.StartDelay.Value.GetHashCode() ^ this.StopDelay.Value.GetHashCode() ^ num ^ base.GetHashCode();
		}

		public void Assign(ActionCcpXcp other)
		{
			base.Assign(other);
			if (other.Event != null)
			{
				this.Event = (Event)other.Event.Clone();
			}
			this.StartDelay = other.StartDelay;
			this.StopDelay = other.StopDelay;
			this.signalList = new List<CcpXcpSignal>();
			if (other.signalList != null)
			{
				this.signalList.AddRange(other.signalList);
			}
			this.IsDirty = true;
		}

		public bool IsEmpty()
		{
			return !this.signalList.Any<CcpXcpSignal>();
		}

		public bool HasOnlyOneSignal()
		{
			return this.signalList.Count == 1;
		}

		public void AddSignal(CcpXcpSignal sig)
		{
			this.signalList.Add(sig);
			sig.ActionCcpXcp = this;
			this.IsDirty = true;
		}

		public void AddSignals(List<CcpXcpSignal> aSignalList)
		{
			this.signalList.AddRange(aSignalList);
			this.IsDirty = true;
		}

		public void InsertSignal(CcpXcpSignal signal, int insertPos)
		{
			if (insertPos > this.Signals.Count - 1)
			{
				this.AddSignal(signal);
			}
			else
			{
				this.signalList.Insert(insertPos, signal);
				signal.ActionCcpXcp = this;
			}
			this.IsDirty = true;
		}

		public bool RemoveSignal(CcpXcpSignal sig)
		{
			this.IsDirty = true;
			return this.signalList.Remove(sig);
		}

		public void RemoveSignals(List<CcpXcpSignal> delList)
		{
			if (delList != null && delList.Any<CcpXcpSignal>())
			{
				this.signalList = this.signalList.Except(delList).ToList<CcpXcpSignal>();
			}
			this.IsDirty = true;
		}

		public bool IsEmptyAfterRemove(List<CcpXcpSignal> delList)
		{
			bool flag = !this.signalList.Any<CcpXcpSignal>();
			if (!flag && delList != null && delList.Any<CcpXcpSignal>())
			{
				flag = !this.signalList.Except(delList).Any<CcpXcpSignal>();
			}
			return flag;
		}
	}
}
