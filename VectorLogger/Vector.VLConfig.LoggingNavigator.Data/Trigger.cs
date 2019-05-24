using System;

namespace Vector.VLConfig.LoggingNavigator.Data
{
	public class Trigger : Entry
	{
		private uint mId;

		private string mLabel;

		private string mName;

		private uint mType;

		private uint mInstance;

		private bool mSelected;

		private bool mSelectedForExport;

		private bool mIsValid;

		private int mLabelOccurenceCount;

		private Measurement mMeasurement;

		private IdManager mIdManager;

		public virtual uint Type
		{
			get
			{
				return this.mType;
			}
		}

		public virtual uint Instance
		{
			get
			{
				return this.mInstance;
			}
		}

		public virtual uint ID
		{
			get
			{
				return this.mId;
			}
		}

		public bool Selected
		{
			get
			{
				return this.mSelected;
			}
			set
			{
				this.mSelected = value;
			}
		}

		public override string Label
		{
			get
			{
				return this.mLabel;
			}
		}

		public override string Name
		{
			get
			{
				return this.mName;
			}
		}

		public override bool Valid
		{
			get
			{
				return this.mIsValid;
			}
		}

		public bool SelectedForExport
		{
			get
			{
				return this.mSelectedForExport;
			}
			set
			{
				this.mSelectedForExport = value;
			}
		}

		public virtual string LabelAndOccurences
		{
			get
			{
				if (this.mLabelOccurenceCount <= 0)
				{
					return this.Label;
				}
				return string.Concat(new object[]
				{
					this.Label,
					" (",
					this.LabelOccurenceCount,
					")"
				});
			}
		}

		public virtual int LabelOccurenceCount
		{
			get
			{
				return this.mLabelOccurenceCount;
			}
		}

		public Measurement Measurement
		{
			get
			{
				return this.mMeasurement;
			}
		}

		public override string Tooltip
		{
			get
			{
				TimeSpec timeSpec = new TimeSpec(base.TimeSpec);
				if (this.Name.Trim().Length > 0)
				{
					return string.Concat(new object[]
					{
						"Trigger: ",
						this.Name,
						" - ",
						timeSpec.DateTime
					});
				}
				return "Trigger: " + timeSpec.DateTime;
			}
		}

		public double Timestamp
		{
			get
			{
				return base.TimeSpec;
			}
		}

		public Trigger(uint id, bool valid, uint tag, uint filepos, ulong timespec, Measurement measurement, IdManager idManager) : base(tag, filepos, timespec)
		{
			this.mId = id;
			this.mIsValid = valid;
			this.mMeasurement = measurement;
			this.mSelectedForExport = true;
			this.mIdManager = idManager;
			this.mType = this.mId;
			this.mInstance = this.mId;
			int markerIdCounterBits = this.mIdManager.GetMarkerIdCounterBits();
			if (markerIdCounterBits > 0 && markerIdCounterBits < 24)
			{
				this.mType &= 16777215u >> markerIdCounterBits;
				this.mInstance >>= 24 - markerIdCounterBits;
				this.SetLabel(this.mType, this.mInstance);
			}
			else
			{
				this.mLabel = Trigger.GetInternalLabel(id);
				this.mLabelOccurenceCount = this.mIdManager.GetTriggerLabelOccurenceCount(id);
			}
			string text;
			if (this.mIdManager.GetTriggerNameByID(this.Type, out text))
			{
				this.mName = text;
				return;
			}
			this.mName = "Trigger";
		}

		private static string GetInternalLabel(uint id)
		{
			string str = (char)(65u + id % 26u) + ((id >= 26u) ? string.Concat(id / 26u) : "");
			return "T" + str;
		}

		private void SetLabel(uint type, uint instance)
		{
			if (type > 0u)
			{
				type -= 1u;
			}
			string str = (char)(65u + type % 26u) + ((type >= 26u) ? string.Concat(type / 26u) : "");
			this.mLabel = "T" + str;
			this.mLabelOccurenceCount = (int)instance;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Trigger))
			{
				return false;
			}
			Trigger trigger = obj as Trigger;
			return this.Timestamp == trigger.Timestamp && this.Label == trigger.Label;
		}

		public override int GetHashCode()
		{
			return this.Timestamp.GetHashCode() * 17 + this.Label.GetHashCode();
		}
	}
}
