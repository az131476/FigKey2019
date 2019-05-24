using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CombinedEvent")]
	public class CombinedEvent : Event, IList<Event>, ICollection<Event>, IEnumerable<Event>, IEnumerable
	{
		private ValidatedProperty<bool> mIsPointInTime;

		[DataMember(Name = "Children")]
		private readonly List<Event> mChildren = new List<Event>();

		[DataMember(Name = "ChildActivation")]
		private readonly List<bool> mChildActivation = new List<bool>();

		public override ValidatedProperty<bool> IsPointInTime
		{
			get
			{
				if (this.mIsPointInTime == null)
				{
					this.mIsPointInTime = new ValidatedProperty<bool>(false);
				}
				List<Event> source = this.Where(new Func<Event, bool>(this.ChildIsActive)).ToList<Event>();
				this.mIsPointInTime.Value = source.Any((Event t) => t.IsPointInTime.Value);
				return this.mIsPointInTime;
			}
		}

		[DataMember(Name = "IsConjunction")]
		public ValidatedProperty<bool> IsConjunction
		{
			get;
			set;
		}

		public Event this[int index]
		{
			get
			{
				return this.mChildren[index];
			}
			set
			{
				this.mChildren[index] = value;
			}
		}

		public int Count
		{
			get
			{
				return this.mChildren.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public CombinedEvent()
		{
			this.IsConjunction = new ValidatedProperty<bool>(true);
		}

		public CombinedEvent(CombinedEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new CombinedEvent(this);
		}

		public override void SetUniqueId()
		{
			base.SetUniqueId();
			foreach (Event current in this.mChildren)
			{
				current.SetUniqueId();
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			CombinedEvent other = (CombinedEvent)obj;
			return this.IsConjunction.Value == other.IsConjunction.Value && this.mChildren.Count == other.mChildren.Count && !this.mChildren.Where((Event t, int i) => !t.Equals(other.mChildren[i])).Any<Event>();
		}

		public override int GetHashCode()
		{
			return this.IsConjunction.Value.GetHashCode() ^ this.mChildren.GetHashCode();
		}

		public void Assign(CombinedEvent other)
		{
			this.IsConjunction.Value = other.IsConjunction.Value;
			for (int i = 0; i < other.Count; i++)
			{
				Event @event = other[i].Clone() as Event;
				if (@event != null)
				{
					this.mChildren.Add(@event);
					this.mChildActivation.Add(other.ChildIsActive(other[i]));
				}
			}
		}

		public IList<Event> GetAllChildren()
		{
			List<Event> result = new List<Event>();
			this.GetAllChildrenInternal(ref result);
			return result;
		}

		private void GetAllChildrenInternal(ref List<Event> allChildren)
		{
			foreach (Event current in this.mChildren)
			{
				allChildren.Add(current);
				CombinedEvent combinedEvent = current as CombinedEvent;
				if (combinedEvent != null)
				{
					combinedEvent.GetAllChildrenInternal(ref allChildren);
				}
			}
		}

		public IList<Event> GetAllActiveChildren()
		{
			List<Event> result = new List<Event>();
			this.GetAllActiveChildrenInternal(ref result);
			return result;
		}

		private void GetAllActiveChildrenInternal(ref List<Event> allChildren)
		{
			foreach (Event current in this.mChildren)
			{
				if (this.ChildIsActive(current))
				{
					allChildren.Add(current);
					CombinedEvent combinedEvent = current as CombinedEvent;
					if (combinedEvent != null)
					{
						combinedEvent.GetAllActiveChildrenInternal(ref allChildren);
					}
				}
			}
		}

		public bool ChildIsActive(Event ev)
		{
			int num = this.IndexOf(ev);
			return num >= 0 && this.mChildActivation[num];
		}

		public void SetChildActive(Event ev, bool active)
		{
			int num = this.IndexOf(ev);
			if (num >= 0)
			{
				this.mChildActivation[num] = active;
			}
		}

		public IList<VoCanRecordingEvent> GetActiveChildVoCANEvents()
		{
			List<VoCanRecordingEvent> list = new List<VoCanRecordingEvent>();
			foreach (Event current in this.mChildren)
			{
				if (this.ChildIsActive(current))
				{
					if (current is VoCanRecordingEvent)
					{
						list.Add(current as VoCanRecordingEvent);
					}
					else if (current is CombinedEvent)
					{
						list.AddRange((current as CombinedEvent).GetActiveChildVoCANEvents());
					}
				}
			}
			return list;
		}

		public int GetNumberOfActiveChildCasKeyEvents()
		{
			int num = 0;
			foreach (Event current in this.mChildren)
			{
				if (this.ChildIsActive(current))
				{
					if (current is KeyEvent && (current as KeyEvent).IsCasKey)
					{
						num++;
					}
					else if (current is CombinedEvent)
					{
						num += (current as CombinedEvent).GetNumberOfActiveChildCasKeyEvents();
					}
				}
			}
			return num;
		}

		public int IndexOf(Event item)
		{
			for (int i = 0; i < this.mChildren.Count; i++)
			{
				if (this.mChildren[i] == item)
				{
					return i;
				}
			}
			return -1;
		}

		public void Insert(int index, Event item)
		{
			CombinedEvent combinedEvent = item as CombinedEvent;
			if (combinedEvent != null)
			{
				combinedEvent.IsConjunction.Value = !this.IsConjunction.Value;
			}
			this.mChildren.Insert(index, item);
			this.mChildActivation.Insert(index, true);
		}

		public void RemoveAt(int index)
		{
			this.mChildren.RemoveAt(index);
			this.mChildActivation.RemoveAt(index);
		}

		public void Add(Event item)
		{
			this.Insert(this.mChildren.Count, item);
		}

		public void Clear()
		{
			this.mChildren.Clear();
			this.mChildActivation.Clear();
		}

		public bool Contains(Event item)
		{
			return this.mChildren.Any((Event t) => t == item);
		}

		public void CopyTo(Event[] array, int arrayIndex)
		{
			this.mChildren.CopyTo(array, arrayIndex);
		}

		public void CopyTo(bool[] array, int arrayIndex)
		{
			this.mChildActivation.CopyTo(array, arrayIndex);
		}

		public bool Remove(Event item)
		{
			int num = this.IndexOf(item);
			if (num < 0)
			{
				return false;
			}
			this.mChildActivation.RemoveAt(num);
			int count = this.mChildren.Count;
			this.mChildren.RemoveAt(num);
			return this.mChildren.Count == count - 1;
		}

		public IEnumerator<Event> GetEnumerator()
		{
			return this.mChildren.GetEnumerator();
		}

		[DispId(-4)]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.mChildren.GetEnumerator();
		}
	}
}
