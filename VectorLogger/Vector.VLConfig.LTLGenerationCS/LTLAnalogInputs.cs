using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLAnalogInputs : LTLGenericCodePart
	{
		private AnalogInputConfiguration analogInputConfig;

		private ILoggerSpecifics loggerSpecifics;

		public LTLAnalogInputs(AnalogInputConfiguration analogInputConfig, ILoggerSpecifics loggerSpecifics)
		{
			this.analogInputConfig = analogInputConfig;
			this.loggerSpecifics = loggerSpecifics;
		}

		public LTLGenerator.LTLGenerationResult GenerateLTLAnalogInputsCode()
		{
			base.LtlCode = new StringBuilder();
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine(LTLUtil.GetFormattedHeaderComment("ANALOG MEASUREMENT"));
			base.LtlCode.AppendLine();
			int num = this.analogInputConfig.AnalogInputs.Count((AnalogInput ai) => ai.IsActive.Value);
			if ((!this.analogInputConfig.MapToSystemChannel.Value && !this.analogInputConfig.MapToCANMessage.Value) || num <= 0)
			{
				base.LtlCode.AppendLine("  {no analog measurement configured}");
				base.LtlCode.AppendLine();
				base.LtlCode.AppendLine();
				return LTLGenerator.LTLGenerationResult.OK;
			}
			int num2 = this.analogInputConfig.AnalogInputs.Count<AnalogInput>();
			if ((long)num2 != (long)((ulong)this.loggerSpecifics.IO.NumberOfAnalogInputs))
			{
				return LTLGenerator.LTLGenerationResult.AnalogInputError;
			}
			if (this.analogInputConfig.Averaging.Value)
			{
				this.AppendAnalogAveragingToSystemSettings();
			}
			if (this.analogInputConfig.MapToSystemChannel.Value)
			{
				this.GenerateRecordingToSystemChannel(num2);
			}
			if (base.LtlSystemCode.Length > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("{Analog Inputs:} ");
				base.LtlSystemCode.Insert(0, stringBuilder);
			}
			if (this.analogInputConfig.MapToCANMessage.Value)
			{
				this.GenerateRecordingToVirtualCANMessages(num2);
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private void GenerateRecordingToSystemChannel(int numAnalogIns)
		{
			if (this.analogInputConfig.Averaging.Value)
			{
				base.LtlSystemCode.AppendLine();
			}
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)numAnalogIns))
			{
				AnalogInput analogInput = this.analogInputConfig.GetAnalogInput(num);
				if (analogInput != null && analogInput.IsActive.Value)
				{
					uint num2 = 1000u / analogInput.Frequency.Value;
					base.LtlSystemCode.AppendFormat("AnaIn{0:D}SamplingCycle = {1:D} {{ms}}", num, num2);
					base.LtlSystemCode.AppendLine();
				}
				num += 1u;
			}
			base.LtlCode.AppendLine("  {analog data is recorded to system channel, see SYSTEM section above}");
			base.LtlCode.AppendLine();
		}

		private void GenerateRecordingToVirtualCANMessages(int numAnalogIns)
		{
			uint value = this.analogInputConfig.CanChannel.Value;
			bool flag = false;
			if (this.analogInputConfig.AnalogInputsCANMappingMode.Value == AnalogInputsCANMappingMode.SameFixedIDs)
			{
				AnalogInput analogInput = null;
				List<uint> list = new List<uint>();
				uint num = 1u;
				while ((ulong)num <= (ulong)((long)(numAnalogIns + 1)))
				{
					AnalogInput analogInput2 = this.analogInputConfig.GetAnalogInput(num);
					if (list.Any<uint>() && analogInput != null && ((analogInput2 == null && (ulong)num > (ulong)((long)numAnalogIns)) || (analogInput2 != null && analogInput2.MappedCANId.Value != analogInput.MappedCANId.Value)))
					{
						base.LtlCode.Append(this.BuildAnalogInTransmitEventBlockForFixedIDs(list, analogInput.Frequency.Value, value, analogInput.MappedCANId.Value, analogInput.IsMappedCANIdExtended.Value));
						list.Clear();
						analogInput = null;
					}
					if (analogInput2 != null && analogInput2.IsActive.Value)
					{
						flag = true;
						list.Add(num);
						if (analogInput == null)
						{
							analogInput = analogInput2;
						}
					}
					num += 1u;
				}
			}
			else
			{
				uint num2 = 1u;
				while ((ulong)num2 <= (ulong)((long)numAnalogIns))
				{
					AnalogInput analogInput3 = this.analogInputConfig.GetAnalogInput(num2);
					if (analogInput3 != null && analogInput3.IsActive.Value)
					{
						flag = true;
						base.LtlCode.Append(this.BuildAnalogInTransmitEventBlock(num2, analogInput3.Frequency.Value, value, analogInput3.MappedCANId.Value, analogInput3.IsMappedCANIdExtended.Value));
					}
					num2 += 1u;
				}
			}
			if (!flag)
			{
				base.LtlCode.AppendLine("  {no analog measurement configured}");
			}
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine();
		}

		private LTLGenerator.LTLGenerationResult AppendAnalogAveragingToSystemSettings()
		{
			StringBuilder stringBuilder = new StringBuilder();
			uint numberOfAnalogInputs = this.loggerSpecifics.IO.NumberOfAnalogInputs;
			for (uint num = 1u; num <= numberOfAnalogInputs; num += 1u)
			{
				AnalogInput analogInput = this.analogInputConfig.GetAnalogInput(num);
				string arg;
				if (analogInput != null && analogInput.IsActive.Value)
				{
					arg = (1000u / analogInput.Frequency.Value).ToString();
				}
				else
				{
					arg = "Off";
				}
				stringBuilder.AppendFormat("AnaIn{0:D}Averaging  {2} = {1}", num, arg, (num < 10u) ? " " : "");
				stringBuilder.AppendLine();
			}
			base.LtlSystemCode = stringBuilder;
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private string BuildAnalogInTransmitEventBlock(uint analogInputNumber, uint frequency, uint canChannelNumber, uint canID, bool isExtendedID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("EVENT ON CYCLE ({0:D}) BEGIN  {{log analog input {1} at {2} Hz}}", 1000u / frequency, analogInputNumber, frequency);
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("  TRANSMIT CAN{0} {1} 0x{2:X} \"AnalogInput{3:D}\" [AnaIn{3:D}] LOG ONLY", new object[]
			{
				canChannelNumber,
				isExtendedID ? "XDATA" : "DATA",
				canID,
				analogInputNumber
			});
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("END");
			return stringBuilder.ToString();
		}

		private string BuildAnalogInTransmitEventBlockForFixedIDs(IList<uint> analogInputList, uint frequency, uint channelNumber, uint canID, bool isExtendedID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			if (analogInputList.Count<uint>() <= 0)
			{
				return stringBuilder.AppendLine().ToString();
			}
			for (int i = 0; i < analogInputList.Count<uint>(); i++)
			{
				stringBuilder2.AppendFormat("AnaIn{0:D} ", analogInputList[i]);
			}
			stringBuilder.AppendFormat("EVENT ON CYCLE ({0:D}) BEGIN  {{log analog inputs at {1} Hz}}", 1000u / frequency, frequency);
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("  TRANSMIT CAN{0} {1} 0x{2:X} \"AnalogInputs_{2:X}\" [{3}] LOG ONLY", new object[]
			{
				channelNumber,
				isExtendedID ? "XDATA" : "DATA",
				canID,
				stringBuilder2
			});
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("END");
			return stringBuilder.ToString();
		}
	}
}
