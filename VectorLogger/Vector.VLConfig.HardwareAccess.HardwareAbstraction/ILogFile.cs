using System;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public interface ILogFile
	{
		string DefaultName
		{
			get;
		}

		string FullPath
		{
			get;
		}

		string FullFilePath
		{
			get;
		}

		string TypeName
		{
			get;
		}

		uint FileSize
		{
			get;
		}

		DateTime Timestamp
		{
			get;
		}

		bool IsConvertible
		{
			get;
		}

		bool IsSecondary
		{
			get;
		}

		bool IsSelected
		{
			get;
			set;
		}
	}
}
