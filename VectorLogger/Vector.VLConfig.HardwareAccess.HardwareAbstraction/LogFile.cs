using System;
using System.IO;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public class LogFile : ILogFile
	{
		private string defaultName;

		private string fullPath;

		private string typeName;

		private uint fileSize;

		private DateTime timestamp;

		private bool isConvertible;

		private bool isSecondary;

		public string DefaultName
		{
			get
			{
				return this.defaultName;
			}
		}

		public string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		public string FullFilePath
		{
			get
			{
				return Path.Combine(this.fullPath, this.defaultName);
			}
		}

		public string TypeName
		{
			get
			{
				return this.typeName;
			}
			set
			{
				this.typeName = value;
			}
		}

		public uint FileSize
		{
			get
			{
				return this.fileSize;
			}
		}

		public DateTime Timestamp
		{
			get
			{
				return this.timestamp;
			}
		}

		public bool IsConvertible
		{
			get
			{
				return this.isConvertible;
			}
			set
			{
				this.isConvertible = value;
			}
		}

		public bool IsSecondary
		{
			get
			{
				return this.isSecondary;
			}
			set
			{
				this.isSecondary = value;
			}
		}

		public bool IsSelected
		{
			get;
			set;
		}

		public LogFile(string defaultName, uint fileSize, DateTime timestamp) : this("", defaultName, fileSize, timestamp)
		{
		}

		public LogFile(string fullPath, string defaultName, uint fileSize, DateTime timestamp)
		{
			this.defaultName = defaultName;
			this.fullPath = fullPath;
			this.typeName = "";
			this.fileSize = fileSize;
			this.timestamp = timestamp;
			this.isConvertible = false;
			this.isSecondary = false;
		}
	}
}
