using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "AnalogInputConfiguration")]
	public class AnalogInputConfiguration : Feature, IFeatureVirtualLogMessages
	{
		private List<IVirtualCANLogMessage> activeVirtCANMsgs;

		[DataMember(Name = "AnalogInputsConfigurationCanChannel")]
		public ValidatedProperty<uint> CanChannel;

		[DataMember(Name = "AnalogInputsConfigurationInputsList")]
		private List<AnalogInput> analogInputList;

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
				if (this.MapToCANMessage.Value)
				{
					foreach (AnalogInput current in this.AnalogInputs)
					{
						if (current.IsActive.Value)
						{
							switch (this.AnalogInputsCANMappingMode.Value)
							{
							case Vector.VLConfig.LoggerSpecifics.AnalogInputsCANMappingMode.SameFixedIDs:
								this.activeVirtCANMsgs.Add(new VirtualCANLogMessage(this.AnalogInputs[0].MappedCANId, this.AnalogInputs[0].IsMappedCANIdExtended, this.CanChannel));
								return this.activeVirtCANMsgs;
							case Vector.VLConfig.LoggerSpecifics.AnalogInputsCANMappingMode.IndividualIDs:
							case Vector.VLConfig.LoggerSpecifics.AnalogInputsCANMappingMode.ContinuousIndividualIDs:
								this.activeVirtCANMsgs.Add(new VirtualCANLogMessage(current.MappedCANId, current.IsMappedCANIdExtended, this.CanChannel));
								break;
							}
						}
					}
				}
				return this.activeVirtCANMsgs;
			}
		}

		public ReadOnlyCollection<AnalogInput> AnalogInputs
		{
			get
			{
				return new ReadOnlyCollection<AnalogInput>(this.analogInputList);
			}
		}

		[DataMember(Name = "AnalogInputsConfigurationMapToSystemChanel")]
		public ValidatedProperty<bool> MapToSystemChannel
		{
			get;
			set;
		}

		[DataMember(Name = "AnalogInputsConfigurationMapToCANMessage")]
		public ValidatedProperty<bool> MapToCANMessage
		{
			get;
			set;
		}

		[DataMember(Name = "AnalogInputsConfigurationCANMappingMode")]
		public ValidatedProperty<AnalogInputsCANMappingMode> AnalogInputsCANMappingMode
		{
			get;
			set;
		}

		[DataMember(Name = "AnalogInputsConfigurationAveraging")]
		public ValidatedProperty<bool> Averaging
		{
			get;
			set;
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is AnalogInputConfiguration || otherFeature is CANChannelConfiguration)
			{
				updateService.Notify<AnalogInputConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<AnalogInputConfiguration>(this);
		}

		public AnalogInputConfiguration()
		{
			this.MapToSystemChannel = new ValidatedProperty<bool>(true);
			this.MapToCANMessage = new ValidatedProperty<bool>(false);
			this.CanChannel = new ValidatedProperty<uint>(1u);
			this.analogInputList = new List<AnalogInput>();
			this.AnalogInputsCANMappingMode = new ValidatedProperty<AnalogInputsCANMappingMode>(Vector.VLConfig.LoggerSpecifics.AnalogInputsCANMappingMode.SameFixedIDs);
			this.Averaging = new ValidatedProperty<bool>(true);
		}

		public bool IsAnyAnalogInputActive()
		{
			return this.AnalogInputs.Any((AnalogInput t) => t.IsActive.Value);
		}

		public void AddAnalogInput(AnalogInput analogInput)
		{
			this.analogInputList.Add(analogInput);
		}

		public bool RemoveAnalogInput(AnalogInput analogInput)
		{
			if (this.analogInputList.Contains(analogInput))
			{
				this.analogInputList.Remove(analogInput);
				return true;
			}
			return false;
		}

		public AnalogInput GetAnalogInput(uint inputNumber)
		{
			if ((ulong)inputNumber > (ulong)((long)this.analogInputList.Count) || inputNumber < 1u)
			{
				return null;
			}
			return this.analogInputList[(int)(inputNumber - 1u)];
		}

		public void ClearAnalogInputs()
		{
			this.analogInputList.Clear();
		}

		[OnDeserializing]
		private void OnDeserializing(StreamingContext context)
		{
			this.analogInputList = new List<AnalogInput>();
		}
	}
}
