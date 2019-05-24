using System;

namespace Vector.VLConfig.GUI.CLFExportPage
{
	public class DisplayedFileItem
	{
		public string Filename
		{
			get;
			set;
		}

		public bool IsEnabled
		{
			get;
			set;
		}

		public DateTime DateTimeModified
		{
			get;
			set;
		}

		public long FileSize
		{
			get;
			set;
		}

		public string DestinationFilename
		{
			get;
			set;
		}

		public bool IsDestinationFileExisting
		{
			get;
			set;
		}

		public string MetaInformations
		{
			get;
			set;
		}
	}
}
