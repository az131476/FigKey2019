using System;
using System.Collections.Generic;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLDigitalOutputs : LTLGenericAction
	{
		private DigitalOutputsConfiguration digOutConfig;

		private StringBuilder eventSectionCode;

		private StringBuilder outputSectionCode;

		protected override string SectionHeaderComment
		{
			get
			{
				return "DIGITAL OUTPUTS";
			}
		}

		public LTLDigitalOutputs(DigitalOutputsConfiguration digitalOutputConfiguration, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, string configurationFolderPath) : base(loggerSpecifics, databaseManager, configurationFolderPath)
		{
			this.digOutConfig = digitalOutputConfiguration;
			this.eventSectionCode = new StringBuilder();
			this.outputSectionCode = new StringBuilder();
		}

		protected override bool IsFeatureSupportedByLogger()
		{
			return this.loggerSpecifics.IO.IsDigitalOutputSupported && this.loggerSpecifics.IO.NumberOfDigitalInputs > 0u;
		}

		protected override bool IsNoActionAvtive(out string textForComment)
		{
			textForComment = "no digital output configured";
			return !this.digOutConfig.IsAnyDigitalOutputActive();
		}

		protected override List<Vector.VLConfig.Data.ConfigurationDataModel.Action> GetActionList()
		{
			List<Vector.VLConfig.Data.ConfigurationDataModel.Action> list = new List<Vector.VLConfig.Data.ConfigurationDataModel.Action>();
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.Action current in this.digOutConfig.Actions)
			{
				list.Add(current);
			}
			return list;
		}

		protected override string GetSetVariableName(int index)
		{
			return string.Format("SetDigOut{0:D}", index);
		}

		protected override string GetResetVariableName(int index)
		{
			return string.Format("ResetDigOut{0:D}", index);
		}

		protected override string GetSetResetFlagName(int index)
		{
			return string.Format("DigOutFlag{0:D}", index);
		}

		protected override string GetActionItemSubSectionComment(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index)
		{
			return string.Format("Digital Output {0:D} ({1})", index, base.GetResetTypeStringForComment(base.GetResetType(action.StopType)));
		}

		protected override LTLGenerator.LTLGenerationResult BuildCodeBeforeActions(out string code)
		{
			code = string.Empty;
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override LTLGenerator.LTLGenerationResult BuildActionCode(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index, out string code)
		{
			if (this.outputSectionCode.Length <= 0)
			{
				this.outputSectionCode.AppendLine("OUTPUT");
			}
			else
			{
				this.outputSectionCode.AppendLine();
			}
			this.outputSectionCode.AppendFormat("  DigOut{0:D} = ({1})", index, this.GetSetResetFlagName(index));
			code = string.Empty;
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override LTLGenerator.LTLGenerationResult BuildInactiveActionCode(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index, out string code)
		{
			if (this.outputSectionCode.Length <= 0)
			{
				this.outputSectionCode.AppendLine("OUTPUT");
			}
			else
			{
				this.outputSectionCode.AppendLine();
			}
			this.outputSectionCode.AppendFormat(" {{DigOut{0:D} is not configured}}", index);
			code = string.Empty;
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override LTLGenerator.LTLGenerationResult BuildCodeAfterActions(out string code)
		{
			code = this.outputSectionCode.ToString();
			return LTLGenerator.LTLGenerationResult.OK;
		}
	}
}
