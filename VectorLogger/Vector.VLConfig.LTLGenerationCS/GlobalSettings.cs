using System;
using System.Collections.Generic;
using System.Linq;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal static class GlobalSettings
	{
		private static ProjectRoot projectRoot;

		private static ILoggerSpecifics loggerSpecifics;

		private static bool isAuxSwitchBoxUsed;

		private static bool isVoCANalreadyConfigured;

		public static Dictionary<DiagnosticSignalRequest, string> ServiceQualifiers;

		private static string vehicleSleepIndicationFlag;

		private static bool isMinutesOfDayUsed;

		public static readonly string MinutesOfDayVariableName;

		private static List<Tuple<string, string, int>> tne_triggerNameEvents;

		public static bool IsAuxSwitchBoxUsed
		{
			get
			{
				return GlobalSettings.isAuxSwitchBoxUsed;
			}
			private set
			{
				GlobalSettings.isAuxSwitchBoxUsed = value;
			}
		}

		public static bool IsVoCANalreadyConfigured
		{
			get
			{
				return GlobalSettings.isVoCANalreadyConfigured;
			}
		}

		public static string VehicleSleepIndicationFlag
		{
			get
			{
				if (GlobalSettings.vehicleSleepIndicationFlag == null)
				{
					GlobalSettings.vehicleSleepIndicationFlag = string.Empty;
				}
				return GlobalSettings.vehicleSleepIndicationFlag;
			}
			set
			{
				GlobalSettings.vehicleSleepIndicationFlag = value;
			}
		}

		public static bool IsMinutesOfDayUsed
		{
			get
			{
				return GlobalSettings.isMinutesOfDayUsed;
			}
			private set
			{
				GlobalSettings.isMinutesOfDayUsed = value;
			}
		}

		public static void SetProjectRootAndLoggerSpecifics(ProjectRoot prjRt, ILoggerSpecifics logSpec)
		{
			GlobalSettings.projectRoot = prjRt;
			GlobalSettings.loggerSpecifics = logSpec;
		}

		public static IList<int> GetFromProjectMemoriesThatUseTriggeredLogging()
		{
			IList<int> result = new List<int>();
			if (GlobalSettings.projectRoot == null)
			{
				return result;
			}
			return (from memory in GlobalSettings.projectRoot.LoggingConfiguration.TriggerConfigurationsOfActiveMemories
			where memory.TriggerMode.Value == TriggerMode.Triggered
			select memory.MemoryNr).ToList<int>();
		}

		public static void UseAuxSwitchBox()
		{
			GlobalSettings.isAuxSwitchBoxUsed = true;
		}

		public static void UseVoCAN()
		{
			GlobalSettings.isVoCANalreadyConfigured = true;
		}

		public static void UseMinutesOfDay()
		{
			GlobalSettings.isMinutesOfDayUsed = true;
		}

		public static void TNE_AddTriggernameEvent(string triggerName, string ltlSetFlag, int memory)
		{
			Tuple<string, string, int> item = new Tuple<string, string, int>(triggerName, ltlSetFlag, memory);
			GlobalSettings.tne_triggerNameEvents.Add(item);
		}

		public static void TNE_GetInfosFromTriggerName(string triggerName, out string ltlSetFlag, out int memory)
		{
			List<Tuple<string, string, int>> list = (from t in GlobalSettings.tne_triggerNameEvents
			where t.Item1.Equals(triggerName)
			select t).ToList<Tuple<string, string, int>>();
			if (list.Count > 0)
			{
				ltlSetFlag = list[0].Item2;
				memory = list[0].Item3;
				return;
			}
			throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.TriggerError_TriggerNameResolve);
		}

		static GlobalSettings()
		{
			GlobalSettings.MinutesOfDayVariableName = "CurrentDayMinutes";
			GlobalSettings.Reset();
		}

		public static void Reset()
		{
			GlobalSettings.isAuxSwitchBoxUsed = false;
			GlobalSettings.isVoCANalreadyConfigured = false;
			GlobalSettings.isMinutesOfDayUsed = false;
			GlobalSettings.ServiceQualifiers = new Dictionary<DiagnosticSignalRequest, string>();
			GlobalSettings.tne_triggerNameEvents = new List<Tuple<string, string, int>>();
		}
	}
}
