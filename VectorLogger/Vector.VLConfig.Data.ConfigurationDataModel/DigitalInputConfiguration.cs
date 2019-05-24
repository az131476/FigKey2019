using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DigitalInputConfiguration")]
	public class DigitalInputConfiguration : Feature, IFeatureVirtualLogMessages
	{
		private List<IVirtualCANLogMessage> activeVirtCANMsgs;

		[DataMember(Name = "DigitalInputsConfigurationCanChannel")]
		public ValidatedProperty<uint> CanChannel;

		[DataMember(Name = "DigitalInputsConfigurationInputsList")]
		private List<DigitalInput> DigitalInputList;

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

		IList<IVirtualCANLogMessage> IFeatureVirtualLogMessages.ActiveVirtualCANLogMessages
		{
			get
			{
				if (this.activeVirtCANMsgs == null)
				{
					this.activeVirtCANMsgs = new List<IVirtualCANLogMessage>();
				}
				this.activeVirtCANMsgs.Clear();
				foreach (DigitalInput current in this.DigitalInputList)
				{
					if (current.IsActiveFrequency.Value || current.IsActiveOnChange.Value)
					{
						switch (this.DigitalInputsMappingMode.Value)
						{
						case Vector.VLConfig.LoggerSpecifics.DigitalInputsMappingMode.GroupedCombinedIDs:
						case Vector.VLConfig.LoggerSpecifics.DigitalInputsMappingMode.GroupedSeparateIDs:
							this.activeVirtCANMsgs.Add(new VirtualCANLogMessage(this.DigitalInputList[0].MappedCANId, this.DigitalInputList[0].IsMappedCANIdExtended, this.CanChannel));
							return this.activeVirtCANMsgs;
						case Vector.VLConfig.LoggerSpecifics.DigitalInputsMappingMode.IndividualIDs:
						case Vector.VLConfig.LoggerSpecifics.DigitalInputsMappingMode.ContinuousIndividualIDs:
							this.activeVirtCANMsgs.Add(new VirtualCANLogMessage(current.MappedCANId, current.IsMappedCANIdExtended, this.CanChannel));
							break;
						}
					}
				}
				return this.activeVirtCANMsgs;
			}
		}

		public ReadOnlyCollection<DigitalInput> DigitalInputs
		{
			get
			{
				return new ReadOnlyCollection<DigitalInput>(this.DigitalInputList);
			}
		}

		[DataMember(Name = "DigitalInputsConfigurationMappingMode")]
		public ValidatedProperty<DigitalInputsMappingMode> DigitalInputsMappingMode
		{
			get;
			set;
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is DigitalInputConfiguration || otherFeature is CANChannelConfiguration || otherFeature is DigitalOutputsConfiguration)
			{
				updateService.Notify<DigitalInputConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<DigitalInputConfiguration>(this);
		}

		public DigitalInputConfiguration()
		{
			this.CanChannel = new ValidatedProperty<uint>(1u);
			this.DigitalInputList = new List<DigitalInput>();
			this.DigitalInputsMappingMode = new ValidatedProperty<DigitalInputsMappingMode>(Vector.VLConfig.LoggerSpecifics.DigitalInputsMappingMode.GroupedSeparateIDs);
		}

		public bool IsAnyDigitalInputActive()
		{
			return this.DigitalInputs.Any((DigitalInput t) => t.IsActiveFrequency.Value || t.IsActiveOnChange.Value);
		}

		public void AddDigitalInput(DigitalInput digitalInput)
		{
			this.DigitalInputList.Add(digitalInput);
		}

		public bool RemoveDigitalInput(DigitalInput digitalInput)
		{
			if (this.DigitalInputList.Contains(digitalInput))
			{
				this.DigitalInputList.Remove(digitalInput);
				return true;
			}
			return false;
		}

		public DigitalInput GetDigitalInput(uint inputNumber)
		{
			if ((ulong)inputNumber > (ulong)((long)this.DigitalInputList.Count) || inputNumber < 1u)
			{
				return null;
			}
			return this.DigitalInputList[(int)(inputNumber - 1u)];
		}

		public void ClearDigitalInputs()
		{
			this.DigitalInputList.Clear();
		}

		[OnDeserializing]
		private void OnDeserializing(StreamingContext context)
		{
			this.DigitalInputList = new List<DigitalInput>();
		}
	}
}
