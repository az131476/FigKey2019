using System;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class FileConversionParametersChangedEventArgs : EventArgs
	{
		public enum Parameter
		{
			DestinationFolder,
			DestinationFormat,
			DestinationFormatVersion,
			DestinationFormatExtension,
			All
		}

		public FileConversionParametersChangedEventArgs.Parameter ChangedParameter
		{
			get;
			set;
		}

		public FileConversionParametersChangedEventArgs(FileConversionParametersChangedEventArgs.Parameter changedParam)
		{
			this.ChangedParameter = changedParam;
		}
	}
}
