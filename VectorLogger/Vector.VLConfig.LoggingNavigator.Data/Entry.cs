using System;
using System.Drawing;

namespace Vector.VLConfig.LoggingNavigator.Data
{
	public class Entry
	{
		private uint mTag;

		private uint mFilePos;

		private ulong mTimeSpec;

		private Rectangle mTooltipRect;

		public uint Tag
		{
			get
			{
				return this.mTag;
			}
			set
			{
				this.mTag = value;
			}
		}

		public uint FilePos
		{
			get
			{
				return this.mFilePos;
			}
			set
			{
				this.mFilePos = value;
			}
		}

		public ulong TimeSpec
		{
			get
			{
				return this.mTimeSpec;
			}
			set
			{
				this.mTimeSpec = value;
			}
		}

		public virtual string Label
		{
			get
			{
				return "";
			}
		}

		public virtual string Name
		{
			get
			{
				return "";
			}
		}

		public virtual string Tooltip
		{
			get
			{
				return "";
			}
		}

		public virtual bool Valid
		{
			get
			{
				return false;
			}
		}

		public Rectangle TooltipRect
		{
			get
			{
				return this.mTooltipRect;
			}
			set
			{
				this.mTooltipRect = value;
			}
		}

		public Entry(uint tag, uint filepos, ulong timespec)
		{
			this.mTag = tag;
			this.mFilePos = filepos;
			this.mTimeSpec = timespec;
		}

		public static void Reset()
		{
		}
	}
}
