using System;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLDigitalInputs : LTLGenericCodePart
	{
		private DigitalInputConfiguration digitalInputConfig;

		private ILoggerSpecifics loggerSpecifics;

		public LTLDigitalInputs(DigitalInputConfiguration digitalInputConfig, ILoggerSpecifics loggerSpecifics)
		{
			this.digitalInputConfig = digitalInputConfig;
			this.loggerSpecifics = loggerSpecifics;
		}

		public LTLGenerator.LTLGenerationResult GenerateLTLDigitalInputsCode()
		{
			base.LtlCode = new StringBuilder();
			if (this.loggerSpecifics.IO.NumberOfDigitalInputs <= 0u)
			{
				return LTLGenerator.LTLGenerationResult.OK;
			}
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine(LTLUtil.GetFormattedHeaderComment("DIGITAL MEASUREMENT"));
			base.LtlCode.AppendLine();
			int num = this.digitalInputConfig.DigitalInputs.Count<DigitalInput>();
			if ((long)num != (long)((ulong)this.loggerSpecifics.IO.NumberOfDigitalInputs))
			{
				return LTLGenerator.LTLGenerationResult.DigitalInputError;
			}
			uint arg_8D_0 = this.digitalInputConfig.CanChannel.Value;
			bool flag = false;
			DigitalInputsMappingMode value = this.digitalInputConfig.DigitalInputsMappingMode.Value;
			if (value == DigitalInputsMappingMode.GroupedCombinedIDs || value == DigitalInputsMappingMode.GroupedSeparateIDs)
			{
				if (num > 8)
				{
					return LTLGenerator.LTLGenerationResult.DigitalInputError;
				}
				uint value2 = this.digitalInputConfig.GetDigitalInput(1u).Frequency.Value;
				uint value3 = this.digitalInputConfig.CanChannel.Value;
				uint value4 = this.digitalInputConfig.GetDigitalInput(1u).MappedCANId.Value;
				bool value5 = this.digitalInputConfig.GetDigitalInput(1u).IsMappedCANIdExtended.Value;
				bool flag2 = false;
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				uint num2 = 1u;
				while ((ulong)num2 <= (ulong)((long)num))
				{
					DigitalInput digitalInput = this.digitalInputConfig.GetDigitalInput(num2);
					if (digitalInput != null && (digitalInput.IsActiveFrequency.Value || digitalInput.IsActiveOnChange.Value))
					{
						flag = true;
						if (stringBuilder2.Length > 0)
						{
							stringBuilder2.Append(" ");
						}
						if (value == DigitalInputsMappingMode.GroupedCombinedIDs)
						{
							stringBuilder2.AppendFormat("DigIn{0:D}", num2);
						}
						else
						{
							stringBuilder2.AppendFormat("DigIn{0:D} 0(7:1)", num2);
						}
						if (digitalInput.IsActiveFrequency.Value)
						{
							flag2 = true;
						}
						if (digitalInput.IsActiveOnChange.Value)
						{
							if (stringBuilder.Length > 0)
							{
								stringBuilder.AppendLine();
							}
							stringBuilder.AppendFormat(" ON SET (DigIn{0:D})  ON RESET (DigIn{0:D}) ", num2);
						}
					}
					num2 += 1u;
				}
				if (flag)
				{
					if (flag2)
					{
						StringBuilder stringBuilder3 = new StringBuilder();
						stringBuilder3.AppendFormat("ON CYCLE ({0:D})", 1000u / value2);
						stringBuilder3.AppendLine();
						stringBuilder.Insert(0, stringBuilder3);
					}
					StringBuilder stringBuilder4 = new StringBuilder();
					stringBuilder4.AppendFormat("EVENT {0} BEGIN", stringBuilder);
					stringBuilder4.AppendLine();
					stringBuilder4 = new StringBuilder(LTLUtil.GetSignAlignedText(stringBuilder4.ToString(), "ON"));
					stringBuilder4.AppendFormat("  TRANSMIT {0} \"DigitalInputs\" INTEL [{1}] LOG ONLY", LTLUtil.GetIdString(BusType.Bt_CAN, value3, value5, value4), stringBuilder2);
					stringBuilder4.AppendLine();
					stringBuilder4.AppendLine("END");
					base.LtlCode.Append(stringBuilder4.ToString());
				}
			}
			else if (value == DigitalInputsMappingMode.ContinuousIndividualIDs || value == DigitalInputsMappingMode.IndividualIDs)
			{
				uint num3 = 1u;
				while ((ulong)num3 <= (ulong)((long)num))
				{
					DigitalInput digitalInput2 = this.digitalInputConfig.GetDigitalInput(num3);
					StringBuilder stringBuilder5 = new StringBuilder();
					StringBuilder stringBuilder6 = new StringBuilder();
					if (digitalInput2 != null && (digitalInput2.IsActiveFrequency.Value || digitalInput2.IsActiveOnChange.Value))
					{
						flag = true;
						stringBuilder6.AppendFormat("DigIn{0:D}", num3);
						if (digitalInput2.IsActiveFrequency.Value)
						{
							stringBuilder5.AppendFormat("ON CYCLE ({0:D})", 1000u / digitalInput2.Frequency.Value);
						}
						if (digitalInput2.IsActiveOnChange.Value)
						{
							if (stringBuilder5.Length > 0)
							{
								stringBuilder5.AppendLine();
								stringBuilder5.Append(" ");
							}
							stringBuilder5.AppendFormat("ON SET (DigIn{0:D})  ON RESET (DigIn{0:D}) ", num3);
						}
						StringBuilder stringBuilder7 = new StringBuilder();
						stringBuilder7.AppendFormat("EVENT {0} BEGIN", stringBuilder5.ToString());
						stringBuilder7 = new StringBuilder(LTLUtil.GetSignAlignedText(stringBuilder7.ToString(), "ON"));
						stringBuilder7.AppendFormat("  TRANSMIT {0} \"DigitalInput{1:D}\" INTEL [{2}] LOG ONLY", LTLUtil.GetIdString(BusType.Bt_CAN, this.digitalInputConfig.CanChannel.Value, digitalInput2.IsMappedCANIdExtended.Value, digitalInput2.MappedCANId.Value), num3, stringBuilder6);
						stringBuilder7.AppendLine();
						stringBuilder7.AppendLine("END");
						base.LtlCode.Append(stringBuilder7.ToString());
					}
					num3 += 1u;
				}
			}
			if (!flag)
			{
				base.LtlCode.AppendLine("  {no digital measurement configured}");
			}
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine();
			return LTLGenerator.LTLGenerationResult.OK;
		}
	}
}
