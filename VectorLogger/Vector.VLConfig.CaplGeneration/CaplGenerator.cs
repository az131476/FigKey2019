using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Vector.TemplateBasedGenerator;
using Vector.VLConfig.CaplGeneration.DataPresenter;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.CaplGeneration
{
	public class CaplGenerator
	{
		public enum EnumCaplGenerationError
		{
			InvalidTriggerCondition
		}

		private const uint cConditionIndexOffset = 100u;

		private readonly GlobalOptions mGlobalOptions;

		private readonly VLProject mVlProject;

		private readonly ILoggerSpecifics mLoggerSpecifics;

		private readonly IApplicationDatabaseManager mDatabaseManager;

		private readonly string mConfigurationFolderPath;

		private readonly DpHardware mDpHardware;

		private List<DpHardwareChannel> mDpHardwareChannelList;

		private readonly CaplFunctions mCaplFunctions = new CaplFunctions();

		private uint mTriggerIndex;

		private uint mConditionIndex = 100u;

		private readonly List<CaplGenerator.EnumCaplGenerationError> mErrors = new List<CaplGenerator.EnumCaplGenerationError>();

		private MultibusChannelConfiguration MultibusChannelConfiguration
		{
			get
			{
				return this.mVlProject.ProjectRoot.HardwareConfiguration.MultibusChannelConfiguration;
			}
		}

		public ReadOnlyCollection<CaplGenerator.EnumCaplGenerationError> GenerationErrors
		{
			get
			{
				return new ReadOnlyCollection<CaplGenerator.EnumCaplGenerationError>(this.mErrors);
			}
		}

		public CaplGenerator(VLProject vlProject, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, string configurationFolderPath, GlobalOptions globalOptions)
		{
			this.mVlProject = vlProject;
			this.mLoggerSpecifics = loggerSpecifics;
			this.mDatabaseManager = databaseManager;
			this.mConfigurationFolderPath = configurationFolderPath;
			this.mGlobalOptions = globalOptions;
			this.mDpHardware = new DpHardware(vlProject.ProjectRoot.LoggingConfiguration.SpecialFeaturesConfiguration.MaxLogFileSize.Value);
			this.mDpHardware.BeepOnOverflow = vlProject.ProjectRoot.LoggingConfiguration.SpecialFeaturesConfiguration.IsOverloadBuzzerActive.Value;
			TriggerConfiguration triggerConfiguration = this.mVlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurationsOfActiveMemories[0];
			if (triggerConfiguration.TriggerMode.Value == TriggerMode.Triggered)
			{
				this.mDpHardware.PostTriggerTimeMs = Math.Max(triggerConfiguration.PostTriggerTime.Value, 0u);
				this.mDpHardware.PreTriggerTimeMs = Math.Max(this.mVlProject.ProjectRoot.MetaInformation.BufferSizeCalculatorInformation.PreTriggerTimeSeconds.Value, 0u) * 1000u;
				this.mDpHardware.PreTriggerTimeNs = (long)((ulong)this.mDpHardware.PreTriggerTimeMs * 1000000uL);
			}
			else
			{
				this.mDpHardware.PostTriggerTimeMs = 0u;
				this.mDpHardware.PreTriggerTimeMs = 0u;
				this.mDpHardware.PreTriggerTimeNs = 0L;
			}
			TriggerMode value = this.mVlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurationsOfActiveMemories[0].TriggerMode.Value;
			this.mDpHardware.IsPermanentLogging = (value == TriggerMode.Permanent);
			this.mDpHardware.IsOnOffLogging = (value == TriggerMode.OnOff);
			this.mDpHardware.IsTriggeredLogging = (value == TriggerMode.Triggered);
			this.mDpHardware.StartupTimeMs = vlProject.ProjectRoot.HardwareConfiguration.LogDataStorage.EventActivationDelayAfterStart.Value;
		}

		public bool GenerateCapl(string caplFile)
		{
			this.mErrors.Clear();
			using (StreamWriter streamWriter = new StreamWriter(caplFile, false, Encoding.Default))
			{
				this.mTriggerIndex = 0u;
				this.mConditionIndex = 100u;
				this.PreprocessDeviceConfiguration();
				this.GenerateCaplForDeviceConfiguration();
				this.GenerateCaplForFilters();
				this.GenerateCaplForTriggers();
				this.GenerateCaplForwardEventFunctionCalls();
				this.GenerateCaplForOffTriggers();
				this.mCaplFunctions.Write(streamWriter);
			}
			return !this.mErrors.Any<CaplGenerator.EnumCaplGenerationError>();
		}

		private void AddError(CaplGenerator.EnumCaplGenerationError genRes)
		{
			if (!this.mErrors.Contains(genRes))
			{
				this.mErrors.Add(genRes);
			}
		}

		private void PreprocessDeviceConfiguration()
		{
			this.mDpHardwareChannelList = new List<DpHardwareChannel>();
			uint num;
			for (num = 1u; num <= this.mLoggerSpecifics.Multibus.NumberOfChannels; num += 1u)
			{
				if (this.MultibusChannelConfiguration.LINChannels.ContainsKey(num))
				{
					LINChannel lINChannel = this.MultibusChannelConfiguration.LINChannels[num];
					if (lINChannel.IsActive.Value)
					{
						this.mDpHardwareChannelList.Add(new DpHardwareChannelLin(lINChannel, num));
					}
				}
			}
			for (num = 1u; num <= this.mLoggerSpecifics.Multibus.NumberOfChannels; num += 1u)
			{
				if (this.MultibusChannelConfiguration.CANChannels.ContainsKey(num))
				{
					CANChannel cANChannel = this.MultibusChannelConfiguration.CANChannels[num];
					if (cANChannel.IsActive.Value)
					{
						this.mDpHardwareChannelList.Add(new DpHardwareChannelCan(cANChannel, num));
					}
				}
			}
			num = 0u;
			foreach (DigitalInput current in this.mVlProject.ProjectRoot.HardwareConfiguration.DigitalInputConfiguration.DigitalInputs)
			{
				if (!this.mDpHardware.IsPermanentLogging || current.IsActiveFrequency.Value)
				{
					this.mDpHardwareChannelList.Add(new DpHardwareChannelDigitalInput(current, num, current.IsActiveFrequency.Value));
				}
				num += 1u;
			}
			num = 0u;
			foreach (AnalogInput current2 in this.mVlProject.ProjectRoot.HardwareConfiguration.AnalogInputConfiguration.AnalogInputs)
			{
				if (!this.mDpHardware.IsPermanentLogging || current2.IsActive.Value)
				{
					this.mDpHardwareChannelList.Add(new DpHardwareChannelAnalogInput(current2, num, current2.IsActive.Value));
				}
				num += 1u;
			}
			uint num2 = 0u;
			foreach (DpHardwareChannel current3 in this.mDpHardwareChannelList)
			{
				if (current3 is DpHardwareChannelDigitalInput)
				{
					num2 = (current3 as DpHardwareChannelDigitalInput).DigitalInput.Frequency.Value;
					break;
				}
				if (current3 is DpHardwareChannelAnalogInput)
				{
					num2 = (current3 as DpHardwareChannelAnalogInput).AnalogInput.Frequency.Value;
					break;
				}
			}
			if (num2 == 0u)
			{
				num2 = 1u;
			}
			this.mDpHardware.IoCycleTimeUs = 1000000u / num2;
			this.mDpHardware.HasDAIO = (this.mDpHardwareChannelList.OfType<DpHardwareChannelDigitalInput>().Any<DpHardwareChannelDigitalInput>() || this.mDpHardwareChannelList.OfType<DpHardwareChannelAnalogInput>().Any<DpHardwareChannelAnalogInput>());
		}

		private void GenerateCaplForDeviceConfiguration()
		{
			TemplateEngine templateEngine = TemplateEngine.CreateFromResource("Vector.VLConfig.CaplGeneration.Resources.ConfigurationTemplates.txt");
			this.mCaplFunctions.AddCodeToFunction("variables", templateEngine.InstantiateTemplate("TVariables_Configuration", new object[]
			{
				this.mDpHardware
			}).ToString());
			this.mCaplFunctions.AddCompleteFunctionDefinition(templateEngine.InstantiateTemplate("TFunctions_Configuration", new object[]
			{
				this.mDpHardware,
				this.mDpHardwareChannelList.OfType<DpHardwareChannelLin>(),
				this.mDpHardwareChannelList.OfType<DpHardwareChannelCan>()
			}).ToString());
			this.mCaplFunctions.AddCompleteFunctionDefinition(templateEngine.InstantiateTemplate("THelpers_Configuration", new object[]
			{
				this.mDpHardware
			}).ToString());
			if (this.mDpHardware.IsOnOffLogging || this.mDpHardware.IsTriggeredLogging)
			{
				this.mCaplFunctions.AddCodeToFunction("on timer gStartupTimer", "  // add msg-timeout timers here");
			}
		}

		private void GenerateCaplForFilters()
		{
			TemplateEngine templateEngine = TemplateEngine.CreateFromResource("Vector.VLConfig.CaplGeneration.Resources.FilterTemplates.txt");
			List<DpFilterCondition> list = new List<DpFilterCondition>();
			bool flag = true;
			foreach (FilterConfiguration current in this.mVlProject.ProjectRoot.LoggingConfiguration.FilterConfigurationsOfActiveMemories)
			{
				foreach (Filter current2 in current.Filters)
				{
					if (current2.IsActive.Value)
					{
						if (current2 is DefaultFilter)
						{
							flag = ((current2 as DefaultFilter).Action.Value == FilterActionType.Pass);
						}
						else
						{
							DpFilterCondition dpFilterCondition = DpFilterCondition.Create(current2);
							if (dpFilterCondition != null)
							{
								list.Add(dpFilterCondition);
							}
						}
					}
				}
			}
			string text = flag ? "1" : "0";
			this.mCaplFunctions.AddCodeToFunction("variables", templateEngine.InstantiateTemplate("TVariables_Filters", new object[0]).ToString());
			if (this.mDpHardware.IsPermanentLogging)
			{
				foreach (DpHardwareChannel current3 in this.mDpHardwareChannelList)
				{
					this.mCaplFunctions.AddCodeToFunction(current3.NameOfCaplEventHandler, templateEngine.InstantiateTemplate("TEventHandler_PendingLogStart", new object[0]).ToString());
				}
			}
			foreach (DpHardwareChannel current4 in this.mDpHardwareChannelList)
			{
				this.mCaplFunctions.AddCodeToFunction(current4.NameOfCaplEventHandler, templateEngine.InstantiateTemplate("TEventHandler_DefaultFilter", new object[]
				{
					text
				}).ToString());
			}
			foreach (DpFilterCondition current5 in list)
			{
				string templateEventHandler = current5.TemplateEventHandler;
				if (!string.IsNullOrEmpty(templateEventHandler) && current5.NameOfCaplEventHandler != null)
				{
					this.mCaplFunctions.AddCodeToFunction(current5.NameOfCaplEventHandler, templateEngine.InstantiateTemplate(templateEventHandler, new object[]
					{
						current5
					}).ToString());
				}
			}
			if (this.mDpHardwareChannelList.Any((DpHardwareChannel x) => x is DpHardwareChannelLin))
			{
				this.mCaplFunctions.AddCompleteFunctionDefinition(templateEngine.InstantiateTemplate("TEventHandler_Filter_LinWakeupFrame", new object[]
				{
					this.mDpHardware
				}).ToString());
			}
			if (this.mDpHardwareChannelList.Any((DpHardwareChannel x) => x is DpHardwareChannelCan))
			{
				foreach (DpHardwareChannelCan current6 in this.mDpHardwareChannelList.OfType<DpHardwareChannelCan>())
				{
					this.mCaplFunctions.AddCompleteFunctionDefinition(templateEngine.InstantiateTemplate("TEventHandler_Filter_CanErrorFrame", new object[]
					{
						this.mDpHardware,
						current6
					}).ToString());
				}
			}
		}

		private void GenerateCaplForwardEventFunctionCalls()
		{
			TemplateEngine templateEngine = TemplateEngine.CreateFromResource("Vector.VLConfig.CaplGeneration.Resources.FilterTemplates.txt");
			foreach (DpHardwareChannel current in this.mDpHardwareChannelList)
			{
				if (!string.IsNullOrEmpty(current.ForwardEventFunctionCall))
				{
					this.mCaplFunctions.AddCodeToFunction(current.NameOfCaplEventHandler, templateEngine.InstantiateTemplate("TEventHandler_WriteToLog", new object[]
					{
						current.ForwardEventFunctionCall
					}).ToString());
				}
			}
		}

		private void GenerateCaplForTriggers()
		{
			TriggerMode value = this.mVlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurationsOfActiveMemories[0].TriggerMode.Value;
			if (!this.mVlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurationsOfActiveMemories.Any<TriggerConfiguration>())
			{
				return;
			}
			if (value == TriggerMode.Permanent)
			{
				return;
			}
			TemplateEngine te = TemplateEngine.CreateFromResource("Vector.VLConfig.CaplGeneration.Resources.TriggerTemplates.txt");
			List<DpTrigger> list;
			this.CreateDpTriggers(false, out list);
			bool flag = list.Any<DpTrigger>();
			if (!flag)
			{
				uint num = this.mTriggerIndex;
				uint num2 = this.mConditionIndex;
				List<DpTrigger> source;
				this.CreateDpTriggers(true, out source);
				flag = source.Any<DpTrigger>();
				this.mTriggerIndex = num;
				this.mConditionIndex = num2;
			}
			if (!flag)
			{
				return;
			}
			this.GenerateCaplForTriggersVariables(te, list);
			this.GenerateCaplForTriggersEventHandlers(te, list);
			this.GenerateCaplForTriggersAdditionalFunctionsAndStartupCode(te, list);
			this.GenerateCaplForTriggersEvaluationFunctions(te, list);
		}

		private void GenerateCaplForOffTriggers()
		{
			TriggerMode value = this.mVlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurationsOfActiveMemories[0].TriggerMode.Value;
			if (value != TriggerMode.OnOff)
			{
				return;
			}
			TemplateEngine te = TemplateEngine.CreateFromResource("Vector.VLConfig.CaplGeneration.Resources.TriggerTemplates.txt");
			List<DpTrigger> list;
			this.CreateDpTriggers(true, out list);
			if (!list.Any<DpTrigger>())
			{
				return;
			}
			this.GenerateCaplForTriggersVariables(te, list);
			this.GenerateCaplForTriggersEventHandlers(te, list);
			this.GenerateCaplForTriggersAdditionalFunctionsAndStartupCode(te, list);
			this.GenerateCaplForTriggersEvaluationFunctions(te, list);
		}

		private void CreateDpTriggers(bool offTriggers, out List<DpTrigger> dpTriggerList)
		{
			dpTriggerList = new List<DpTrigger>();
			TriggerMode value = this.mVlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurationsOfActiveMemories[0].TriggerMode.Value;
			if (value != TriggerMode.OnOff && value != TriggerMode.Triggered)
			{
				return;
			}
			if (offTriggers && value != TriggerMode.OnOff)
			{
				return;
			}
			foreach (TriggerConfiguration current in this.mVlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurationsOfActiveMemories)
			{
				if (value == TriggerMode.Triggered)
				{
					using (IEnumerator<RecordTrigger> enumerator2 = current.ActiveTriggers.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							RecordTrigger current2 = enumerator2.Current;
							DpTrigger dpTrigger = new DpTrigger(this.mTriggerIndex, current2, this.mConditionIndex);
							if (!this.ValidateTrigger(dpTrigger))
							{
								this.AddError(CaplGenerator.EnumCaplGenerationError.InvalidTriggerCondition);
							}
							else
							{
								dpTriggerList.Add(dpTrigger);
								this.mTriggerIndex += 1u;
								this.mConditionIndex += 1u;
							}
						}
						continue;
					}
				}
				foreach (RecordTrigger current3 in current.ActiveOnOffTriggers)
				{
					if ((!offTriggers || current3.TriggerEffect.Value == TriggerEffect.LoggingOff) && (offTriggers || current3.TriggerEffect.Value != TriggerEffect.LoggingOff))
					{
						DpTrigger dpTrigger2 = new DpTrigger(this.mTriggerIndex, current3, this.mConditionIndex);
						if (!this.ValidateTrigger(dpTrigger2))
						{
							this.AddError(CaplGenerator.EnumCaplGenerationError.InvalidTriggerCondition);
						}
						else
						{
							dpTriggerList.Add(dpTrigger2);
							this.mTriggerIndex += 1u;
							this.mConditionIndex += 1u;
						}
					}
				}
			}
		}

		private bool ValidateTrigger(DpTrigger dpTrigger)
		{
			return dpTrigger != null && this.ValidateCondition(dpTrigger.Condition) && dpTrigger.Condition.ChildConditionListFlat.All(new Func<DpTriggerCondition, bool>(this.ValidateCondition));
		}

		private bool ValidateCondition(DpTriggerCondition dpTriggerCondition)
		{
			return dpTriggerCondition != null && dpTriggerCondition.Validate(this.mDatabaseManager, this.mConfigurationFolderPath);
		}

		private void GenerateCaplForTriggersVariables(TemplateEngine te, IEnumerable<DpTrigger> dpTriggerList)
		{
			foreach (DpTrigger current in dpTriggerList)
			{
				this.mCaplFunctions.AddCodeToFunction("variables", te.InstantiateTemplate("TVariables_Trigger", new object[]
				{
					current
				}).ToString());
				if (current.Condition != null)
				{
					string templateVariables = current.Condition.TemplateVariables;
					if (!string.IsNullOrEmpty(templateVariables))
					{
						this.mCaplFunctions.AddCodeToFunction("variables", te.InstantiateTemplate(templateVariables, new object[]
						{
							current.Condition
						}).ToString());
					}
					foreach (DpTriggerCondition current2 in current.ChildConditionListFlat)
					{
						string templateVariables2 = current2.TemplateVariables;
						if (!string.IsNullOrEmpty(templateVariables2))
						{
							this.mCaplFunctions.AddCodeToFunction("variables", te.InstantiateTemplate(templateVariables2, new object[]
							{
								current2
							}).ToString());
						}
					}
				}
			}
		}

		private void GenerateCaplForTriggersEventHandlers(TemplateEngine te, IEnumerable<DpTrigger> dpTriggerList)
		{
			foreach (DpTrigger current in dpTriggerList)
			{
				List<string> list = new List<string>();
				if (current.Condition != null)
				{
					this.GenerateCaplForTriggersEventHandlers(te, current.Condition, list);
					foreach (DpTriggerCondition current2 in current.ChildConditionListFlat)
					{
						this.GenerateCaplForTriggersEventHandlers(te, current2, list);
					}
				}
				foreach (string current3 in list)
				{
					this.mCaplFunctions.AddCodeToFunction(current3, te.InstantiateTemplate("TEventHandler_Trigger", new object[]
					{
						current
					}).ToString());
				}
			}
			foreach (DpTrigger current4 in dpTriggerList)
			{
				foreach (DpTriggerCondition current5 in from trigger in current4.ChildConditionListFlat
				where trigger.IsPointInTime
				select trigger)
				{
					this.mCaplFunctions.AddCodeToFunction(current5.NameOfCaplEventHandler, te.InstantiateTemplate("TEventHandler_Trigger_ResetPointInTimeCondition", new object[]
					{
						current5
					}).ToString());
				}
			}
		}

		private void GenerateCaplForTriggersEventHandlers(TemplateEngine te, DpTriggerCondition condition, List<string> eventHandlerFunctionList)
		{
			string templateEventHandler = condition.TemplateEventHandler;
			if (!string.IsNullOrEmpty(templateEventHandler) && !string.IsNullOrEmpty(condition.NameOfCaplEventHandler))
			{
				this.mCaplFunctions.AddCodeToFunction(condition.NameOfCaplEventHandler, te.InstantiateTemplate(templateEventHandler, new object[]
				{
					condition
				}).ToString());
				if (!condition.TemplateCallsProcessTriggerInEventHandler && !eventHandlerFunctionList.Contains(condition.NameOfCaplEventHandler))
				{
					eventHandlerFunctionList.Add(condition.NameOfCaplEventHandler);
				}
			}
		}

		private void GenerateCaplForTriggersAdditionalFunctionsAndStartupCode(TemplateEngine te, IEnumerable<DpTrigger> dpTriggerList)
		{
			foreach (DpTrigger current in dpTriggerList)
			{
				if (current.Condition != null)
				{
					this.GenerateCaplForTriggersAdditionalFunctionsAndStartupCode(te, current.Condition);
					foreach (DpTriggerCondition current2 in current.ChildConditionListFlat)
					{
						this.GenerateCaplForTriggersAdditionalFunctionsAndStartupCode(te, current2);
					}
				}
			}
		}

		private void GenerateCaplForTriggersAdditionalFunctionsAndStartupCode(TemplateEngine te, DpTriggerCondition dpTriggerCondition)
		{
			string text = dpTriggerCondition.TemplateAdditionalFunction;
			if (!string.IsNullOrEmpty(text))
			{
				this.mCaplFunctions.AddCompleteFunctionDefinition(te.InstantiateTemplate(text, new object[]
				{
					dpTriggerCondition
				}).ToString());
			}
			text = dpTriggerCondition.TemplateStartupCode;
			if (!string.IsNullOrEmpty(text))
			{
				this.mCaplFunctions.AddCodeToFunction("on timer gStartupTimer", te.InstantiateTemplate(text, new object[]
				{
					dpTriggerCondition
				}).ToString());
			}
		}

		private void GenerateCaplForTriggersEvaluationFunctions(TemplateEngine te, IEnumerable<DpTrigger> dpTriggerList)
		{
			foreach (DpTrigger current in dpTriggerList)
			{
				this.mCaplFunctions.AddCompleteFunctionDefinition(te.InstantiateTemplate("TProcess_Trigger", new object[]
				{
					current,
					current.Condition
				}).ToString());
			}
		}
	}
}
