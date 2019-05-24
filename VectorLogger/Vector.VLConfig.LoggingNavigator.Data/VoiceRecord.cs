using System;
using Vector.VLConfig.LoggingNavigator.Properties;

namespace Vector.VLConfig.LoggingNavigator.Data
{
	public class VoiceRecord : Entry
	{
		private string mPath;

		private bool mFileError;

		public string Path
		{
			get
			{
				return this.mPath;
			}
		}

		public bool FileError
		{
			get
			{
				return this.mFileError;
			}
			set
			{
				this.mFileError = value;
			}
		}

		public override string Tooltip
		{
			get
			{
				TimeSpec timeSpec = new TimeSpec(base.TimeSpec);
				return Resources.VoCanAudioRecording + ": " + timeSpec.DateTime;
			}
		}

		public VoiceRecord(uint tag, uint filepos, ulong timespec, string path) : base(tag, filepos, timespec)
		{
			this.mPath = path;
		}
	}
}
