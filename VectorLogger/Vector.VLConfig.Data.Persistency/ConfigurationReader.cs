using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter;

namespace Vector.VLConfig.Data.Persistency
{
	public class ConfigurationReader
	{
		public static bool BuildRawDataFromProjectFile(string aFileAndPath, ref ProjectRoot newProjectRoot, out bool isIncompatibleFileVersion)
		{
			isIncompatibleFileVersion = false;
			if (!File.Exists(aFileAndPath))
			{
				return false;
			}
			XmlDictionaryReader xmlDictionaryReader = null;
			Stream stream = null;
			bool result;
			try
			{
				stream = ConfigurationReader.GetStreamFromProjectFileWithVersionConversion(aFileAndPath, out isIncompatibleFileVersion);
				if (isIncompatibleFileVersion)
				{
					return false;
				}
				xmlDictionaryReader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
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
				newProjectRoot = (ProjectRoot)dataContractSerializer.ReadObject(xmlDictionaryReader, true);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			finally
			{
				if (xmlDictionaryReader != null)
				{
					xmlDictionaryReader.Close();
				}
				if (stream != null)
				{
					stream.Close();
				}
			}
			return result;
		}

		private static Stream GetStreamFromProjectFileWithVersionConversion(string fileAndPath, out bool isIncompatibleFileVersion)
		{
			Stream stream = null;
			isIncompatibleFileVersion = false;
			Stream result;
			try
			{
				FileStream inputXML = new FileStream(fileAndPath, FileMode.Open, FileAccess.Read);
				VersionConverter versionConverter = new VersionConverter(ProjectRoot.CurrentFileFormatVersion);
				if (!versionConverter.Convert(inputXML, ref stream, out isIncompatibleFileVersion))
				{
					result = null;
				}
				else
				{
					stream.Seek(0L, SeekOrigin.Begin);
					result = stream;
				}
			}
			catch (Exception)
			{
				throw;
			}
			return result;
		}
	}
}
