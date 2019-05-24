using System;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	internal static class LTLEventFactory
	{
		public static bool GetLtlEventCodeObject(Event ev, string flagName, IApplicationDatabaseManager databaseManager, string configurationFolderPath, out LTLGenericEventCode ltlEventCode)
		{
			if (ev is CANIdEvent || ev is LINIdEvent || ev is FlexrayIdEvent)
			{
				ltlEventCode = new LTLIDEventCode(flagName, ev as IdEvent);
			}
			else if (ev is SymbolicMessageEvent)
			{
				ltlEventCode = new LTLSymbolicMessageEventCode(flagName, ev as SymbolicMessageEvent, databaseManager, configurationFolderPath);
			}
			else if (ev is SymbolicSignalEvent)
			{
				ltlEventCode = new LTLSymbolicSignalEventCode(flagName, ev as SymbolicSignalEvent, databaseManager, configurationFolderPath);
			}
			else if (ev is CANDataEvent || ev is LINDataEvent)
			{
				ltlEventCode = new LTLDataEventCode(flagName, ev);
			}
			else if (ev is DigitalInputEvent)
			{
				ltlEventCode = new LTLDigitalInputEventCode(flagName, ev as DigitalInputEvent);
			}
			else if (ev is AnalogInputEvent)
			{
				ltlEventCode = new LTLAnalogInputEventCode(flagName, ev as AnalogInputEvent);
			}
			else if (ev is IgnitionEvent)
			{
				ltlEventCode = new LTLIgnitionEventCode(flagName, ev as IgnitionEvent);
			}
			else if (ev is KeyEvent)
			{
				ltlEventCode = new LTLKeyEventCode(flagName, ev as KeyEvent);
			}
			else if (ev is VoCanRecordingEvent)
			{
				ltlEventCode = new LTLVoCANEventCode(flagName, ev as VoCanRecordingEvent);
			}
			else if (ev is CANBusStatisticsEvent)
			{
				ltlEventCode = new LTLCANBusStatisticsEventCode(flagName, ev as CANBusStatisticsEvent);
			}
			else if (ev is OnStartEvent)
			{
				ltlEventCode = new LTLOnStartEventCode(flagName, ev as OnStartEvent);
			}
			else if (ev is CyclicTimerEvent)
			{
				ltlEventCode = new LTLCyclicEventCode(flagName, ev as CyclicTimerEvent);
			}
			else if (ev is MsgTimeoutEvent)
			{
				ltlEventCode = new LTLMessageTimeoutEventCode(flagName, ev as MsgTimeoutEvent, databaseManager, configurationFolderPath);
			}
			else if (ev is NextLogSessionStartEvent)
			{
				ltlEventCode = new LTLNextLoggerStartEventCode(flagName, ev as NextLogSessionStartEvent);
			}
			else if (ev is OnShutdownEvent)
			{
				ltlEventCode = new LTLShutdownEventCode(flagName, ev as OnShutdownEvent);
			}
			else if (ev is ClockTimedEvent)
			{
				ltlEventCode = new LTLClockTimedEventCode(flagName, ev as ClockTimedEvent);
			}
			else if (ev is CombinedEvent)
			{
				ltlEventCode = new LTLCombinedEventCode(flagName, ev as CombinedEvent, databaseManager, configurationFolderPath);
			}
			else if (ev is CcpXcpSignalEvent)
			{
				ltlEventCode = new LTLCcpXcpSignalEventCode(flagName, ev as CcpXcpSignalEvent);
			}
			else if (ev is DiagnosticSignalEvent)
			{
				ltlEventCode = new LTLDiagnosticSignalEventCode(flagName, ev as DiagnosticSignalEvent);
			}
			else if (ev is ReferencedRecordTriggerNameEvent)
			{
				ltlEventCode = new LTLReferencedTriggerNameEventCode(flagName, ev as ReferencedRecordTriggerNameEvent);
			}
			else
			{
				if (!(ev is IncEvent))
				{
					ltlEventCode = null;
					return false;
				}
				ltlEventCode = new LTLIncSignalEventCode(flagName, ev as IncEvent);
			}
			return true;
		}
	}
}
