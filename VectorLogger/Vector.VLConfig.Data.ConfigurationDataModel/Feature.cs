using System;
using System.Runtime.Serialization;
using Vector.VLConfig.Data.DataAccess;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "Feature")]
	public abstract class Feature
	{
		public abstract IFeatureReferencedFiles ReferencedFiles
		{
			get;
		}

		public abstract IFeatureSymbolicDefinitions SymbolicDefinitions
		{
			get;
		}

		public abstract IFeatureCcpXcpSignalDefinitions CcpXcpSignalDefinitions
		{
			get;
		}

		public abstract IFeatureTransmitMessages TransmitMessages
		{
			get;
		}

		public abstract IFeatureVirtualLogMessages VirtualLogMessages
		{
			get;
		}

		public abstract IFeatureEvents Events
		{
			get;
		}

		public Feature()
		{
			VLProject.FeatureRegistration.Register(this);
		}

		[OnDeserialized]
		public void OnDeserialized(StreamingContext ctx)
		{
			VLProject.FeatureRegistration.Register(this);
		}

		public abstract bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService);

		public abstract void Update(IUpdateServiceForFeature updateService);
	}
}
