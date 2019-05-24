using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public class DisplayMode
	{
		private bool isHexadecimal;

		public bool IsHexadecimal
		{
			get
			{
				return this.isHexadecimal;
			}
			set
			{
				this.isHexadecimal = value;
			}
		}

		public DisplayMode()
		{
			this.isHexadecimal = false;
		}

		public DisplayMode(bool isHex)
		{
			this.isHexadecimal = isHex;
		}

		public DisplayMode(DisplayMode other)
		{
			this.isHexadecimal = false;
			if (other != null)
			{
				this.isHexadecimal = other.isHexadecimal;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			DisplayMode displayMode = obj as DisplayMode;
			return displayMode != null && this.isHexadecimal == displayMode.isHexadecimal;
		}

		public bool Equals(DisplayMode other)
		{
			return other != null && this.isHexadecimal == other.isHexadecimal;
		}

		public override int GetHashCode()
		{
			return this.isHexadecimal.GetHashCode();
		}
	}
}
