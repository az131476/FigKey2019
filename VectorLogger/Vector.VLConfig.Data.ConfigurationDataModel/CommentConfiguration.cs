using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CommentConfiguration")]
	public class CommentConfiguration : Feature
	{
		public override IFeatureReferencedFiles ReferencedFiles
		{
			get
			{
				return null;
			}
		}

		public override IFeatureSymbolicDefinitions SymbolicDefinitions
		{
			get
			{
				return null;
			}
		}

		public override IFeatureCcpXcpSignalDefinitions CcpXcpSignalDefinitions
		{
			get
			{
				return null;
			}
		}

		public override IFeatureTransmitMessages TransmitMessages
		{
			get
			{
				return null;
			}
		}

		public override IFeatureVirtualLogMessages VirtualLogMessages
		{
			get
			{
				return null;
			}
		}

		public override IFeatureEvents Events
		{
			get
			{
				return null;
			}
		}

		[DataMember(Name = "CommentConfigurationName")]
		public ValidatedProperty<string> Name
		{
			get;
			set;
		}

		[DataMember(Name = "CommentConfigurationVersion")]
		public ValidatedProperty<string> Version
		{
			get;
			set;
		}

		[DataMember(Name = "CommentConfigurationComment")]
		public ValidatedProperty<string> Comment
		{
			get;
			set;
		}

		[DataMember(Name = "CommentConfigurationCopyToLTL")]
		public ValidatedProperty<bool> CopyToLTL
		{
			get;
			set;
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is CommentConfiguration)
			{
				updateService.Notify<CommentConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<CommentConfiguration>(this);
		}

		public CommentConfiguration()
		{
			this.Name = new ValidatedProperty<string>("");
			this.Version = new ValidatedProperty<string>("");
			this.Comment = new ValidatedProperty<string>("");
			this.CopyToLTL = new ValidatedProperty<bool>(true);
		}
	}
}
