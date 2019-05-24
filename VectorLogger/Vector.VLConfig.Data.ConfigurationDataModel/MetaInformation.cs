using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "MetaInformation")]
	public class MetaInformation : Feature
	{
		[DataMember(Name = "MetaInformationBufferSizeCalculatorInformation")]
		private BufferSizeCalculatorInformation mBufferSizeCalculatorInformation;

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

		public BufferSizeCalculatorInformation BufferSizeCalculatorInformation
		{
			get
			{
				BufferSizeCalculatorInformation arg_1B_0;
				if ((arg_1B_0 = this.mBufferSizeCalculatorInformation) == null)
				{
					arg_1B_0 = (this.mBufferSizeCalculatorInformation = new BufferSizeCalculatorInformation());
				}
				return arg_1B_0;
			}
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is MetaInformation)
			{
				updateService.Notify<MetaInformation>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<MetaInformation>(this);
		}
	}
}
