using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "GPSConfiguration")]
	public class GPSConfiguration : Feature, IFeatureReferencedFiles, IFile, IFeatureVirtualLogMessages
	{
		private const uint _NumberOfSubsequentCANIds = 5u;

		private List<IFile> referencedFiles;

		private List<IVirtualCANLogMessage> activeVirtCANMsgs;

		private ValidatedProperty<uint> canIdDateTimeAltitude;

		private ValidatedProperty<uint> canIdLongitudeLatitude;

		private ValidatedProperty<uint> canIdVelocityDirection;

		private List<ValidatedProperty<uint>> subsequentCANIds;

		public override IFeatureReferencedFiles ReferencedFiles
		{
			get
			{
				return this;
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
				return this;
			}
		}

		public override IFeatureEvents Events
		{
			get
			{
				return null;
			}
		}

		IList<IFile> IFeatureReferencedFiles.ReferencedFiles
		{
			get
			{
				if (this.referencedFiles == null)
				{
					this.referencedFiles = new List<IFile>();
				}
				this.referencedFiles.Clear();
				this.referencedFiles.Add(this);
				return this.referencedFiles;
			}
		}

		ValidatedProperty<string> IFile.FilePath
		{
			get
			{
				return this.Database;
			}
		}

		IList<IVirtualCANLogMessage> IFeatureVirtualLogMessages.ActiveVirtualCANLogMessages
		{
			get
			{
				if (this.activeVirtCANMsgs == null)
				{
					this.activeVirtCANMsgs = new List<IVirtualCANLogMessage>();
				}
				this.activeVirtCANMsgs.Clear();
				if (this.MapToSystemChannel.Value && !string.IsNullOrEmpty(this.Database.Value))
				{
					VirtualCANLogMessage item = new VirtualCANLogMessage(this.canIdDateTimeAltitude, new ValidatedProperty<bool>(false), this.CANChannel);
					this.activeVirtCANMsgs.Add(item);
					item = new VirtualCANLogMessage(this.canIdLongitudeLatitude, new ValidatedProperty<bool>(false), this.CANChannel);
					this.activeVirtCANMsgs.Add(item);
					item = new VirtualCANLogMessage(this.canIdVelocityDirection, new ValidatedProperty<bool>(false), this.CANChannel);
					this.activeVirtCANMsgs.Add(item);
				}
				else if (this.MapToCANMessage.Value && string.IsNullOrEmpty(this.Database.Value))
				{
					VirtualCANLogMessage item2 = new VirtualCANLogMessage(this.StartCANId, this.IsExtendedStartCANId, this.CANChannel);
					this.activeVirtCANMsgs.Add(item2);
					foreach (ValidatedProperty<uint> current in this.SubsequentCANIds)
					{
						item2 = new VirtualCANLogMessage(current, this.IsExtendedStartCANId, this.CANChannel);
						this.activeVirtCANMsgs.Add(item2);
					}
				}
				return this.activeVirtCANMsgs;
			}
		}

		[DataMember(Name = "GPSConfigurationMapToSystemChannel")]
		public ValidatedProperty<bool> MapToSystemChannel
		{
			get;
			set;
		}

		[DataMember(Name = "GPSConfigurationMapToCANMessage")]
		public ValidatedProperty<bool> MapToCANMessage
		{
			get;
			set;
		}

		[DataMember(Name = "GPSConfigurationStartCANId")]
		public ValidatedProperty<uint> StartCANId
		{
			get;
			set;
		}

		[DataMember(Name = "GPSConfigurationIsExtendedStartCANId")]
		public ValidatedProperty<bool> IsExtendedStartCANId
		{
			get;
			set;
		}

		[DataMember(Name = "GPSConfigurationCANChannel")]
		public ValidatedProperty<uint> CANChannel
		{
			get;
			set;
		}

		[DataMember(Name = "GPSConfigurationDatabase")]
		public ValidatedProperty<string> Database
		{
			get;
			set;
		}

		public ValidatedProperty<uint> CANIdDateTimeAltitude
		{
			get
			{
				if (this.canIdDateTimeAltitude == null)
				{
					this.canIdDateTimeAltitude = new ValidatedProperty<uint>(0u);
				}
				return this.canIdDateTimeAltitude;
			}
			set
			{
				this.canIdDateTimeAltitude = value;
			}
		}

		public ValidatedProperty<uint> CANIdLongitudeLatitude
		{
			get
			{
				if (this.canIdLongitudeLatitude == null)
				{
					this.canIdLongitudeLatitude = new ValidatedProperty<uint>(0u);
				}
				return this.canIdLongitudeLatitude;
			}
			set
			{
				this.canIdLongitudeLatitude = value;
			}
		}

		public ValidatedProperty<uint> CANIdVelocityDirection
		{
			get
			{
				if (this.canIdVelocityDirection == null)
				{
					this.canIdVelocityDirection = new ValidatedProperty<uint>(0u);
				}
				return this.canIdVelocityDirection;
			}
			set
			{
				this.canIdVelocityDirection = value;
			}
		}

		public int LongitudeLatitudeMode
		{
			get;
			set;
		}

		public double AltitudeFactor
		{
			get;
			set;
		}

		public IList<ValidatedProperty<uint>> SubsequentCANIds
		{
			get
			{
				if (this.subsequentCANIds == null)
				{
					this.subsequentCANIds = new List<ValidatedProperty<uint>>();
					for (uint num = 1u; num <= 5u; num += 1u)
					{
						this.subsequentCANIds.Add(new ValidatedProperty<uint>(0u));
					}
				}
				return this.subsequentCANIds;
			}
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is GPSConfiguration || otherFeature is CANChannelConfiguration)
			{
				updateService.Notify<GPSConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<GPSConfiguration>(this);
		}

		public GPSConfiguration()
		{
			this.MapToSystemChannel = new ValidatedProperty<bool>(false);
			this.MapToCANMessage = new ValidatedProperty<bool>(false);
			this.StartCANId = new ValidatedProperty<uint>(0u);
			this.IsExtendedStartCANId = new ValidatedProperty<bool>(false);
			this.CANChannel = new ValidatedProperty<uint>(0u);
			this.Database = new ValidatedProperty<string>("");
		}
	}
}
