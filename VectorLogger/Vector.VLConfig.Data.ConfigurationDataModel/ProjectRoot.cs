using System;
using System.Runtime.Serialization;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "ConfigurationDataModel")]
	public class ProjectRoot
	{
		private const uint cCurrentFileFormatVersion = 23u;

		[DataMember(Name = "ConfigurationDataModelHardwareConfiguration")]
		private HardwareConfiguration mHardwareConfiguration;

		[DataMember(Name = "ConfigurationDataModelLoggingConfiguration")]
		private LoggingConfiguration mLoggingConfiguration;

		[DataMember(Name = "ConfigurationDataModelOutputConfiguration")]
		private OutputConfiguration mOutputConfiguration;

		[DataMember(Name = "MetaInformation")]
		private MetaInformation mMetaInformation;

		public static uint CurrentFileFormatVersion
		{
			get
			{
				return 23u;
			}
		}

		public HardwareConfiguration HardwareConfiguration
		{
			get
			{
				HardwareConfiguration arg_1B_0;
				if ((arg_1B_0 = this.mHardwareConfiguration) == null)
				{
					arg_1B_0 = (this.mHardwareConfiguration = new HardwareConfiguration());
				}
				return arg_1B_0;
			}
		}

		public LoggingConfiguration LoggingConfiguration
		{
			get
			{
				LoggingConfiguration arg_1B_0;
				if ((arg_1B_0 = this.mLoggingConfiguration) == null)
				{
					arg_1B_0 = (this.mLoggingConfiguration = new LoggingConfiguration());
				}
				return arg_1B_0;
			}
		}

		public OutputConfiguration OutputConfiguration
		{
			get
			{
				OutputConfiguration arg_1B_0;
				if ((arg_1B_0 = this.mOutputConfiguration) == null)
				{
					arg_1B_0 = (this.mOutputConfiguration = new OutputConfiguration());
				}
				return arg_1B_0;
			}
		}

		public MetaInformation MetaInformation
		{
			get
			{
				MetaInformation arg_1B_0;
				if ((arg_1B_0 = this.mMetaInformation) == null)
				{
					arg_1B_0 = (this.mMetaInformation = new MetaInformation());
				}
				return arg_1B_0;
			}
		}

		[DataMember(Name = "FileFormatVersion")]
		private uint FileFormatVersion
		{
			get;
			set;
		}

		[DataMember(Name = "ApplicationVersion")]
		public string ApplicationVersion
		{
			get;
			set;
		}

		[DataMember(Name = "ConfigurationDataModelLoggerType")]
		public LoggerType LoggerType
		{
			get;
			set;
		}

		public ProjectRoot()
		{
			this.FileFormatVersion = ProjectRoot.CurrentFileFormatVersion;
		}
	}
}
