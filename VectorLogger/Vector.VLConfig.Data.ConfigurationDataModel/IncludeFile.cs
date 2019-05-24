using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "IncludeFile")]
	public class IncludeFile : IFile
	{
		[DataMember(Name = "IncludeFileParameterList")]
		private List<ValidatedProperty<string>> mParameterList;

		[DataMember(Name = "IncludeFilePath")]
		public ValidatedProperty<string> FilePath
		{
			get;
			set;
		}

		public int NumberOfParameters
		{
			get
			{
				if (this.mParameterList == null)
				{
					return 0;
				}
				return this.mParameterList.Count;
			}
		}

		public IList<ValidatedProperty<string>> Parameters
		{
			get
			{
				return this.mParameterList;
			}
		}

		public int HighestInParameterIndex
		{
			get;
			set;
		}

		public IncludeFile()
		{
			this.FilePath = new ValidatedProperty<string>(string.Empty);
			this.mParameterList = new List<ValidatedProperty<string>>();
			this.HighestInParameterIndex = -1;
		}
	}
}
