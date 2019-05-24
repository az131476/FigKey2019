using System;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLClockTimedEventCode : LTLGenericEventCode
	{
		private ClockTimedEvent clockTimedEvent;

		private int StartHour
		{
			get
			{
				return this.clockTimedEvent.StartTime.Value.Hour;
			}
		}

		private int StartMinute
		{
			get
			{
				return this.clockTimedEvent.StartTime.Value.Minute;
			}
		}

		private int IntervalHours
		{
			get
			{
				return this.clockTimedEvent.RepetitionInterval.Value.Hours;
			}
		}

		private int IntervalMinutes
		{
			get
			{
				return this.clockTimedEvent.RepetitionInterval.Value.Minutes;
			}
		}

		public LTLClockTimedEventCode(string eventFlagName, ClockTimedEvent ev) : base(eventFlagName)
		{
			this.clockTimedEvent = ev;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			base.PreEventCode = null;
			base.TriggerEvent = "SET (second = 0)";
			base.WhenCondition = null;
			base.AdditionalCodeAfterFlagSet = null;
			base.PostEventCode = null;
			string eventFlagNameWithoutDot = base.GetEventFlagNameWithoutDot();
			if (this.clockTimedEvent.RepetitionInterval.Value == ClockTimedEvent.TimeSpan_Daily)
			{
				base.WhenCondition = string.Format("hour = {0:D} AND minute = {1:D}", this.StartHour, this.StartMinute);
			}
			else if (this.clockTimedEvent.RepetitionInterval.Value == ClockTimedEvent.TimeSpan_2h || this.clockTimedEvent.RepetitionInterval.Value == ClockTimedEvent.TimeSpan_4h || this.clockTimedEvent.RepetitionInterval.Value == ClockTimedEvent.TimeSpan_8h)
			{
				GlobalSettings.UseMinutesOfDay();
				string text = "";
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("VAR {0}_HourModulo = FREE[8]", eventFlagNameWithoutDot);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("CALC {0}_HourModulo = (hour % {1:D})", eventFlagNameWithoutDot, this.IntervalHours);
				if (!LTLClockTimedEventCode.IsTimeZero(this.clockTimedEvent.StartTime.Value))
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendFormat("CONST {0}_StartDayMinutes = {1:D} {{ == {2:D}*60 + {3:D} }}", new object[]
					{
						eventFlagNameWithoutDot,
						this.StartHour * 60 + this.StartMinute,
						this.StartHour,
						this.StartMinute
					});
					text = string.Format(" AND {0} >= {1}_StartDayMinutes", GlobalSettings.MinutesOfDayVariableName, eventFlagNameWithoutDot);
				}
				base.PreEventCode = stringBuilder.ToString();
				base.TriggerEvent = "SET (second = 1)";
				base.WhenCondition = string.Format("{0}_HourModulo = {1:D} AND minute = {2:D}{3}", new object[]
				{
					eventFlagNameWithoutDot,
					this.StartHour % this.IntervalHours,
					this.StartMinute,
					text
				});
			}
			else
			{
				if (!(this.clockTimedEvent.RepetitionInterval.Value == ClockTimedEvent.TimeSpan_15min) && !(this.clockTimedEvent.RepetitionInterval.Value == ClockTimedEvent.TimeSpan_30min) && !(this.clockTimedEvent.RepetitionInterval.Value == ClockTimedEvent.TimeSpan_1h))
				{
					ltlCode = "";
					return LTLGenerator.LTLGenerationResult.TriggerError_UnsupportedCondition;
				}
				StringBuilder stringBuilder2 = new StringBuilder();
				int num = (int)this.clockTimedEvent.RepetitionInterval.Value.TotalMinutes;
				int num2 = 60 / num;
				for (int i = 0; i < num2; i++)
				{
					if (i > 0)
					{
						stringBuilder2.Append(") OR (");
					}
					int num3 = i * num + this.StartMinute % num;
					stringBuilder2.AppendFormat("minute = {0:D}", num3);
				}
				if (LTLClockTimedEventCode.IsTimeZero(this.clockTimedEvent.StartTime.Value))
				{
					base.WhenCondition = stringBuilder2.ToString();
				}
				else
				{
					GlobalSettings.UseMinutesOfDay();
					StringBuilder stringBuilder3 = new StringBuilder();
					stringBuilder3.AppendFormat("FLAG {0}_ConnectTime = ({1})", eventFlagNameWithoutDot, stringBuilder2);
					stringBuilder3.AppendLine();
					stringBuilder3.AppendFormat("CONST {0}_StartDayMinutes = {1:D} {{ == {2:D}*60 + {3:D} }}", new object[]
					{
						eventFlagNameWithoutDot,
						this.StartHour * 60 + this.StartMinute,
						this.StartHour,
						this.StartMinute
					});
					base.PreEventCode = stringBuilder3.ToString();
					base.TriggerEvent = string.Format("SET ({0}_ConnectTime)", eventFlagNameWithoutDot);
					base.WhenCondition = string.Format("{0} >= {1}_StartDayMinutes", GlobalSettings.MinutesOfDayVariableName, eventFlagNameWithoutDot);
				}
			}
			ltlCode = base.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override string GetComment()
		{
			string repetitionIntervalString = LTLClockTimedEventCode.GetRepetitionIntervalString(this.clockTimedEvent.RepetitionInterval.Value);
			string arg = this.clockTimedEvent.StartTime.Value.ToString("t");
			return string.Format("Clock-timed event, repeating {0}, starting {1}.", repetitionIntervalString, arg);
		}

		private static bool IsTimeZero(DateTime dateTime)
		{
			return dateTime.Hour == 0 && dateTime.Minute == 0 && dateTime.Second == 0;
		}

		private static string GetRepetitionIntervalString(TimeSpan timeSpan)
		{
			if (timeSpan.TotalMinutes <= 60.0)
			{
				return string.Format("every {0:D} minutes", (int)timeSpan.TotalMinutes);
			}
			if (timeSpan.TotalHours < 24.0)
			{
				return string.Format("every {0:D} hours", (int)timeSpan.TotalHours);
			}
			if (timeSpan == ClockTimedEvent.TimeSpan_Daily)
			{
				return "daily";
			}
			return string.Format("every {0}", timeSpan.ToString());
		}
	}
}
