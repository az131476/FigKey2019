using System;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLGPS : LTLGenericCodePart
	{
		private GPSConfiguration gpsConfig;

		private ILoggerSpecifics loggerSpecifics;

		public LTLGPS(GPSConfiguration gpsConfig, ILoggerSpecifics loggerSpecifics)
		{
			this.gpsConfig = gpsConfig;
			this.loggerSpecifics = loggerSpecifics;
		}

		public LTLGenerator.LTLGenerationResult GenerateLTLGPSCode()
		{
			base.LtlCode = new StringBuilder();
			base.LtlSystemCode = new StringBuilder();
			if (this.loggerSpecifics.GPS.HasSerialGPS)
			{
				this.GenerateSerialGPS();
			}
			LTLGenerator.LTLGenerationResult result;
			if (this.loggerSpecifics.GPS.HasCANgpsSupport && this.gpsConfig.MapToSystemChannel.Value && (result = this.GenerateCANgps()) != LTLGenerator.LTLGenerationResult.OK)
			{
				return result;
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private void GenerateSerialGPS()
		{
			if (this.gpsConfig.MapToSystemChannel.Value)
			{
				base.LtlSystemCode.AppendLine("GPStoSystemChannel = On");
			}
			else
			{
				base.LtlSystemCode.AppendLine("GPStoSystemChannel = Off");
			}
			if (this.gpsConfig.MapToCANMessage.Value)
			{
				base.LtlSystemCode.AppendFormat("GPSlog             = {0}", LTLUtil.GetIdString(BusType.Bt_CAN, this.gpsConfig.CANChannel.Value, this.gpsConfig.IsExtendedStartCANId.Value, this.gpsConfig.StartCANId.Value));
				return;
			}
			base.LtlSystemCode.AppendLine("GPSlog             = 0");
		}

		private LTLGenerator.LTLGenerationResult GenerateCANgps()
		{
			base.LtlCode.AppendLine(LTLUtil.GetFormattedHeaderComment("GPS recording from CANgps"));
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("VAR cangps.Day             = {0} [0]", LTLUtil.GetIdString(BusType.Bt_CAN, this.gpsConfig.CANChannel.Value, false, this.gpsConfig.CANIdDateTimeAltitude.Value));
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("    cangps.Month           = {0} [1]", LTLUtil.GetIdString(BusType.Bt_CAN, this.gpsConfig.CANChannel.Value, false, this.gpsConfig.CANIdDateTimeAltitude.Value));
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("    cangps.Year            = {0} [2]", LTLUtil.GetIdString(BusType.Bt_CAN, this.gpsConfig.CANChannel.Value, false, this.gpsConfig.CANIdDateTimeAltitude.Value));
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("    cangps.Hour            = {0} [3]", LTLUtil.GetIdString(BusType.Bt_CAN, this.gpsConfig.CANChannel.Value, false, this.gpsConfig.CANIdDateTimeAltitude.Value));
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("    cangps.Minute          = {0} [4]", LTLUtil.GetIdString(BusType.Bt_CAN, this.gpsConfig.CANChannel.Value, false, this.gpsConfig.CANIdDateTimeAltitude.Value));
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("    cangps.Second          = {0} [5]", LTLUtil.GetIdString(BusType.Bt_CAN, this.gpsConfig.CANChannel.Value, false, this.gpsConfig.CANIdDateTimeAltitude.Value));
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("    cangps.Altitude        = {0} [7 6] SIGNED", LTLUtil.GetIdString(BusType.Bt_CAN, this.gpsConfig.CANChannel.Value, false, this.gpsConfig.CANIdDateTimeAltitude.Value));
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("    cangps.Longitude.H     = {0} [7 6]", LTLUtil.GetIdString(BusType.Bt_CAN, this.gpsConfig.CANChannel.Value, false, this.gpsConfig.CANIdLongitudeLatitude.Value));
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("    cangps.Longitude.L     = {0} [5 4]", LTLUtil.GetIdString(BusType.Bt_CAN, this.gpsConfig.CANChannel.Value, false, this.gpsConfig.CANIdLongitudeLatitude.Value));
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("    cangps.Latitude.H      = {0} [3 2]", LTLUtil.GetIdString(BusType.Bt_CAN, this.gpsConfig.CANChannel.Value, false, this.gpsConfig.CANIdLongitudeLatitude.Value));
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("    cangps.Latitude.L      = {0} [1 0]", LTLUtil.GetIdString(BusType.Bt_CAN, this.gpsConfig.CANChannel.Value, false, this.gpsConfig.CANIdLongitudeLatitude.Value));
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("    cangps.Course          = {0} [3 2]         FACTOR =   0.1   OFFSET = 0  UNITSTRING = \"Degrees\"", LTLUtil.GetIdString(BusType.Bt_CAN, this.gpsConfig.CANChannel.Value, false, this.gpsConfig.CANIdVelocityDirection.Value));
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("    cangps.Speed           = {0} [1 0]         FACTOR =   0.1   OFFSET = 0  UNITSTRING = \"km/h\"", LTLUtil.GetIdString(BusType.Bt_CAN, this.gpsConfig.CANChannel.Value, false, this.gpsConfig.CANIdVelocityDirection.Value));
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("EVENT ON RECEIVE ({0}) BEGIN", LTLUtil.GetIdString(BusType.Bt_CAN, this.gpsConfig.CANChannel.Value, false, this.gpsConfig.CANIdVelocityDirection.Value));
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("  VAR  altitude = FREE[16]", new object[0]);
			base.LtlCode.AppendLine();
			if (this.gpsConfig.AltitudeFactor == 0.1)
			{
				base.LtlCode.AppendFormat("  CALC altitude = (cangps.Altitude / 2)", new object[0]);
			}
			else
			{
				if (this.gpsConfig.AltitudeFactor != 1.0)
				{
					return LTLGenerator.LTLGenerationResult.GPSerror;
				}
				base.LtlCode.AppendFormat("  CALC altitude = (cangps.Altitude * 5)", new object[0]);
			}
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("  VAR  speed = FREE[16]", new object[0]);
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("  CALC speed = SCALE32(cangps.Speed, 10, 36)", new object[0]);
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine("  VAR  fixQuality = FREE[8]");
			base.LtlCode.AppendLine("  CALC fixQuality = (1)");
			base.LtlCode.AppendLine("  CALC fixQuality = (0) WHEN (cangps.Longitude.H = 0x8000  AND  cangps.Longitude.L = 0x0000) OR (cangps.Latitude.H = 0x8000  AND  cangps.Latitude.L = 0x0000)");
			base.LtlCode.AppendFormat("  CALC System.NULL = Sys1_SetGPSPosition ({{flags:}} {0:D}, cangps.Longitude.H, cangps.Longitude.L, cangps.Latitude.H, cangps.Latitude.L, altitude, speed, cangps.Course, fixQuality)", 3u);
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("  CALC System.NULL = Sys1_SetGPSDateTime ({{flags:}} {0:D}, cangps.Year, cangps.Month, cangps.Day, cangps.Hour, cangps.Minute, cangps.Second, {{Millisecond:}} 0)", 3u);
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine("END");
			base.LtlCode.AppendLine();
			return LTLGenerator.LTLGenerationResult.OK;
		}
	}
}
