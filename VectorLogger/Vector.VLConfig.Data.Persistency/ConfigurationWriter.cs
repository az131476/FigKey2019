using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.Data.Persistency
{
	public class ConfigurationWriter
	{
		public static bool SaveRawDataToProjectFile(string aFileAndPath, ProjectRoot projectRoot)
		{
			XmlWriter xmlWriter = null;
			bool result;
			try
			{
				DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(ProjectRoot), new List<Type>
				{
					typeof(CANIdEvent),
					typeof(LINIdEvent),
					typeof(FlexrayIdEvent),
					typeof(CANDataEvent),
					typeof(LINDataEvent),
					typeof(RawDataSignalByte),
					typeof(RawDataSignalStartbitLength),
					typeof(SymbolicMessageEvent),
					typeof(SymbolicSignalEvent),
					typeof(KeyEvent),
					typeof(DigitalInputEvent),
					typeof(AnalogInputEvent),
					typeof(CANBusStatisticsEvent),
					typeof(IgnitionEvent),
					typeof(VoCanRecordingEvent),
					typeof(OnStartEvent),
					typeof(CyclicTimerEvent),
					typeof(MsgTimeoutEvent),
					typeof(DefaultFilter),
					typeof(ChannelFilter),
					typeof(CANIdFilter),
					typeof(LINIdFilter),
					typeof(FlexrayIdFilter),
					typeof(SymbolicMessageFilter),
					typeof(SignalListFileFilter),
					typeof(ActionSendMessage),
					typeof(ActionDigitalOutput),
					typeof(StopImmediate),
					typeof(StopOnTimer),
					typeof(StopOnEvent),
					typeof(StopOnStartEvent),
					typeof(ClockTimedEvent),
					typeof(NextLogSessionStartEvent),
					typeof(CANChannel),
					typeof(LINChannel),
					typeof(J1708Channel),
					typeof(CombinedEvent),
					typeof(CcpXcpSignalEvent),
					typeof(DiagnosticSignalEvent),
					typeof(CANStdChipConfiguration),
					typeof(CANFDChipConfiguration),
					typeof(OnShutdownEvent),
					typeof(DiagnosticSignalRequest),
					typeof(ReferencedRecordTriggerNameEvent),
					typeof(ActionCcpXcp),
					typeof(IncEvent)
				}, 2147483647, false, true, null);
				XmlWriterSettings settings = new XmlWriterSettings
				{
					Indent = true,
					NewLineHandling = NewLineHandling.Entitize
				};
				XmlWriter xmlWriter2;
				xmlWriter = (xmlWriter2 = XmlWriter.Create(aFileAndPath, settings));
				try
				{
					dataContractSerializer.WriteObject(xmlWriter, projectRoot);
					xmlWriter.Close();
				}
				finally
				{
					if (xmlWriter2 != null)
					{
						((IDisposable)xmlWriter2).Dispose();
					}
				}
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			finally
			{
				if (xmlWriter != null)
				{
					xmlWriter.Close();
				}
			}
			return result;
		}
	}
}
