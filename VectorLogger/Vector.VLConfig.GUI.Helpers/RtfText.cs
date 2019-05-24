using System;

namespace Vector.VLConfig.GUI.Helpers
{
	internal class RtfText
	{
		private const string cRtfStart = "{\\rtf1\\ansi";

		private const string cRtfEnd = "}";

		private string mText = string.Empty;

		public bool IsEmpty
		{
			get
			{
				return string.IsNullOrEmpty(this.mText);
			}
		}

		public RtfText()
		{
			this.mText = string.Empty;
		}

		public RtfText(string text)
		{
			this.mText = text;
		}

		public void Append(string part)
		{
			this.mText += part;
		}

		public override string ToString()
		{
			return "{\\rtf1\\ansi" + this.mText + "}";
		}

		public static bool IsRtf(string text)
		{
			return !string.IsNullOrEmpty(text) && text.StartsWith("{\\rtf1\\ansi");
		}
	}
}
