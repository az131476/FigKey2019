using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "InterfaceModeConfiguration")]
	public class InterfaceModeConfiguration : Feature, IFeatureReferencedFiles, IFeatureSymbolicDefinitions, IFeatureVirtualLogMessages, IVirtualCANLogMessage
	{
		private List<IFile> referencedFiles;

		private IList<ISymbolicMessage> symbolicMsgs;

		private IList<ISymbolicSignal> symbolicSignals;

		private List<DiagnosticAction> diagActions;

		private List<IVirtualCANLogMessage> activeVirtCANMsgs;

		[DataMember(Name = "InterfaceModeConfigurationUseInterfaceMode")]
		public ValidatedProperty<bool> UseInterfaceMode;

		[DataMember(Name = "InterfaceModeConfigurationIpAddress")]
		public ValidatedProperty<string> IpAddress;

		[DataMember(Name = "InterfaceModeConfigurationSubnetMask")]
		public ValidatedProperty<string> SubnetMask;

		[DataMember(Name = "InterfaceModeConfigurationPort")]
		public ValidatedProperty<uint> Port;

		[DataMember(Name = "InterfaceModeConfigurationMarkerCANId")]
		public ValidatedProperty<uint> MarkerCANId;

		[DataMember(Name = "InterfaceModeConfigurationIsMarkerCANIdExtended")]
		public ValidatedProperty<bool> IsMarkerCANIdExtended;

		[DataMember(Name = "InterfaceModeConfigurationMarkerChannel")]
		public ValidatedProperty<uint> MarkerChannel;

		[DataMember(Name = "InterfaceModeConfigurationIsSendPhysTxActive")]
		public ValidatedProperty<bool> IsSendPhysTxActive;

		[DataMember(Name = "InterfaceModeConfigurationIsSendLoggedTxActive")]
		public ValidatedProperty<bool> IsSendLoggedTxActive;

		[DataMember(Name = "InterfaceModeConfigurationIsSendErrorFramesActive")]
		public ValidatedProperty<bool> IsSendErrorFramesActive;

		[DataMember(Name = "InterfaceModeConfigurationUseSignalExport")]
		public ValidatedProperty<bool> UseSignalExport;

		[DataMember(Name = "InterfaceModeConfigurationEnableAlwaysOnline")]
		public ValidatedProperty<bool> EnableAlwaysOnline;

		[DataMember(Name = "InterfaceModeConfigurationExportCycle")]
		public ValidatedProperty<uint> ExportCycle;

		[DataMember(Name = "InterfaceModeConfigurationSignalExportList")]
		private List<WebDisplayExportSignal> signalExportList;

		[DataMember(Name = "InterfaceModeConfigurationUseCustomWebDisplay")]
		public ValidatedProperty<bool> UseCustomWebDisplay;

		[DataMember(Name = "InterfaceModeConfigurationCustomWebDisplay")]
		public WebDisplayFile CustomWebDisplay;

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
				return this;
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
				if (this.CustomWebDisplay != null && !string.IsNullOrEmpty(this.CustomWebDisplay.FilePath.Value))
				{
					this.referencedFiles.Add(this.CustomWebDisplay);
				}
				return this.referencedFiles;
			}
		}

		IList<ISymbolicMessage> IFeatureSymbolicDefinitions.SymbolicMessages
		{
			get
			{
				if (this.symbolicMsgs == null)
				{
					this.symbolicMsgs = new List<ISymbolicMessage>();
				}
				return this.symbolicMsgs;
			}
		}

		IList<ISymbolicSignal> IFeatureSymbolicDefinitions.SymbolicSignals
		{
			get
			{
				if (this.symbolicSignals == null)
				{
					this.symbolicSignals = new List<ISymbolicSignal>();
				}
				this.symbolicSignals.Clear();
				foreach (WebDisplayExportSignal current in this.signalExportList)
				{
					this.symbolicSignals.Add(current);
				}
				return this.symbolicSignals;
			}
		}

		IList<DiagnosticAction> IFeatureSymbolicDefinitions.SymbolicDiagnosticActions
		{
			get
			{
				if (this.diagActions == null)
				{
					this.diagActions = new List<DiagnosticAction>();
				}
				return this.diagActions;
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
				if (this.UseInterfaceMode.Value)
				{
					this.activeVirtCANMsgs.Add(this);
				}
				return this.activeVirtCANMsgs;
			}
		}

		ValidatedProperty<uint> IVirtualCANLogMessage.Id
		{
			get
			{
				return this.MarkerCANId;
			}
		}

		ValidatedProperty<bool> IVirtualCANLogMessage.IsIdExtended
		{
			get
			{
				return this.IsMarkerCANIdExtended;
			}
		}

		ValidatedProperty<uint> IVirtualCANLogMessage.ChannelNr
		{
			get
			{
				return this.MarkerChannel;
			}
		}

		public ReadOnlyCollection<WebDisplayExportSignal> WebDisplayExportSignals
		{
			get
			{
				return new ReadOnlyCollection<WebDisplayExportSignal>(this.signalExportList);
			}
		}

		public ReadOnlyCollection<WebDisplayExportSignal> ActiveWebDisplayExportSignals
		{
			get
			{
				return new ReadOnlyCollection<WebDisplayExportSignal>((from WebDisplayExportSignal expSig in this.signalExportList
				where expSig.IsActive.Value
				select expSig).ToList<WebDisplayExportSignal>());
			}
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is InterfaceModeConfiguration || otherFeature is CANChannelConfiguration || otherFeature is LINChannelConfiguration || otherFeature is FlexrayChannelConfiguration || otherFeature is DatabaseConfiguration)
			{
				updateService.Notify<InterfaceModeConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<InterfaceModeConfiguration>(this);
		}

		public InterfaceModeConfiguration()
		{
			this.UseInterfaceMode = new ValidatedProperty<bool>(false);
			this.IpAddress = new ValidatedProperty<string>(string.Empty);
			this.SubnetMask = new ValidatedProperty<string>(string.Empty);
			this.Port = new ValidatedProperty<uint>(0u);
			this.MarkerCANId = new ValidatedProperty<uint>(0u);
			this.IsMarkerCANIdExtended = new ValidatedProperty<bool>(false);
			this.MarkerChannel = new ValidatedProperty<uint>(1u);
			this.IsSendPhysTxActive = new ValidatedProperty<bool>(false);
			this.IsSendLoggedTxActive = new ValidatedProperty<bool>(false);
			this.IsSendErrorFramesActive = new ValidatedProperty<bool>(true);
			this.UseSignalExport = new ValidatedProperty<bool>(false);
			this.EnableAlwaysOnline = new ValidatedProperty<bool>(false);
			this.ExportCycle = new ValidatedProperty<uint>(200u);
			this.signalExportList = new List<WebDisplayExportSignal>();
			this.UseCustomWebDisplay = new ValidatedProperty<bool>(false);
			this.CustomWebDisplay = new WebDisplayFile();
		}

		public void AddWebDisplayExportSignal(WebDisplayExportSignal sig)
		{
			if (this.signalExportList == null)
			{
				return;
			}
			this.signalExportList.Add(sig);
		}

		public bool ReplaceWebDisplayExportSignal(WebDisplayExportSignal sigToReplace, WebDisplayExportSignal newSig)
		{
			if (this.signalExportList == null || sigToReplace == null || newSig == null || !this.signalExportList.Contains(sigToReplace))
			{
				return false;
			}
			int index = this.signalExportList.IndexOf(sigToReplace);
			if (this.signalExportList.Remove(sigToReplace))
			{
				this.signalExportList.Insert(index, newSig);
				return true;
			}
			return false;
		}

		public bool RemoveWebDisplayExportSignal(WebDisplayExportSignal sig)
		{
			if (this.signalExportList == null)
			{
				return false;
			}
			if (this.signalExportList.Contains(sig))
			{
				this.signalExportList.Remove(sig);
				return true;
			}
			return false;
		}

		public void ClearWebDisplayExportSignals()
		{
			if (this.signalExportList == null)
			{
				return;
			}
			this.signalExportList.Clear();
		}

		public IList<WebDisplayExportSignal> GetSymbolicWebDisplayExportSignalsForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<WebDisplayExportSignal> list = new List<WebDisplayExportSignal>();
			foreach (WebDisplayExportSignal current in this.signalExportList)
			{
				if (string.Compare(current.DatabasePath.Value, databasePath, true) == 0 && ((current.ChannelNumber.Value == channel && current.BusType.Value == busType) || (current.BusType.Value == BusType.Bt_FlexRay && channel == Database.ChannelNumber_FlexrayAB)))
				{
					list.Add(current);
				}
			}
			return list;
		}
	}
}
