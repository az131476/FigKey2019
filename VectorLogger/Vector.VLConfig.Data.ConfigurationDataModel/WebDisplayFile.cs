using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "WebDisplayFile")]
	public class WebDisplayFile : IFile
	{
		[DataMember(Name = "WebDisplayFilePath")]
		public ValidatedProperty<string> FilePath
		{
			get;
			set;
		}

		public WebDisplayFile() : this(string.Empty)
		{
		}

		public WebDisplayFile(string path)
		{
			this.FilePath = new ValidatedProperty<string>(path);
		}
	}
}
