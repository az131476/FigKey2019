using System;
using System.Collections.Generic;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.Data.DataAccess
{
	public interface IFeatureRegistration
	{
		IList<Feature> Features
		{
			get;
		}

		IList<IFeatureReferencedFiles> FeaturesUsingReferencedFiles
		{
			get;
		}

		IList<IFeatureSymbolicDefinitions> FeaturesUsingSymbolicDefinitions
		{
			get;
		}

		IList<IFeatureCcpXcpSignalDefinitions> FeaturesUsingCcpXcpSignalDefinitions
		{
			get;
		}

		IList<IFeatureTransmitMessages> FeaturesUsingTransmitMessages
		{
			get;
		}

		IList<IFeatureVirtualLogMessages> FeaturesUsingVirtualLogMessages
		{
			get;
		}

		IList<IFeatureEvents> FeaturesUsingEvents
		{
			get;
		}

		bool Register(Feature feature);
	}
}
