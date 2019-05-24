using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Runtime.CompilerServices;

namespace Vector.VLConfig.CANwinAccess.COM
{
	internal class CANwinConfiguration : ComBase
	{
		[CompilerGenerated]
		private static class <FinalConstruct>o__SiteContainer0
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Site1;
		}

		[CompilerGenerated]
		private static class <GetModified>o__SiteContainer2
		{
			public static CallSite<Func<CallSite, object, bool>> <>p__Site3;

			public static CallSite<Func<CallSite, object, object>> <>p__Site4;
		}

		[CompilerGenerated]
		private static class <SetModified>o__SiteContainer5
		{
			public static CallSite<Func<CallSite, object, bool, object>> <>p__Site6;
		}

		[CompilerGenerated]
		private static class <GetMode>o__SiteContainer7
		{
			public static CallSite<Func<CallSite, object, ConfigurationMode>> <>p__Site8;

			public static CallSite<Func<CallSite, object, object>> <>p__Site9;
		}

		[CompilerGenerated]
		private static class <SetMode>o__SiteContainera
		{
			public static CallSite<Func<CallSite, object, ConfigurationMode, object>> <>p__Siteb;
		}

		[CompilerGenerated]
		private static class <Save>o__SiteContainerc
		{
			public static CallSite<Action<CallSite, object, string, bool>> <>p__Sited;

			public static CallSite<Action<CallSite, object>> <>p__Sitee;
		}

		[CompilerGenerated]
		private static class <GetFullName>o__SiteContainerf
		{
			public static CallSite<Func<CallSite, object, string>> <>p__Site10;

			public static CallSite<Func<CallSite, object, object>> <>p__Site11;
		}

		private CANwinGeneralSetup mGeneralSetup;

		private CANwinSimulationSetup mSimulationSetup;

		private CANwinMeasurementSetup mOnlineSetup;

		private CANwinMeasurementSetup mOfflineSetup;

		public CANwinGeneralSetup GeneralSetup
		{
			get
			{
				CANwinGeneralSetup arg_1C_0;
				if ((arg_1C_0 = this.mGeneralSetup) == null)
				{
					arg_1C_0 = (this.mGeneralSetup = new CANwinGeneralSetup(this));
				}
				return arg_1C_0;
			}
		}

		public CANwinSimulationSetup SimulationSetup
		{
			get
			{
				CANwinSimulationSetup arg_1C_0;
				if ((arg_1C_0 = this.mSimulationSetup) == null)
				{
					arg_1C_0 = (this.mSimulationSetup = new CANwinSimulationSetup(this));
				}
				return arg_1C_0;
			}
		}

		public CANwinMeasurementSetup OnlineSetup
		{
			get
			{
				CANwinMeasurementSetup arg_1D_0;
				if ((arg_1D_0 = this.mOnlineSetup) == null)
				{
					arg_1D_0 = (this.mOnlineSetup = CANwinMeasurementSetup.Create(this, true));
				}
				return arg_1D_0;
			}
		}

		public CANwinMeasurementSetup OfflineSetup
		{
			get
			{
				CANwinMeasurementSetup arg_1D_0;
				if ((arg_1D_0 = this.mOfflineSetup) == null)
				{
					arg_1D_0 = (this.mOfflineSetup = CANwinMeasurementSetup.Create(this, false));
				}
				return arg_1D_0;
			}
		}

		public CANwinConfiguration(CANwinApplication parent) : base(parent)
		{
		}

		protected override bool FinalConstruct()
		{
			if (base.IsInitialized)
			{
				return true;
			}
			bool result;
			try
			{
				if (CANwinConfiguration.<FinalConstruct>o__SiteContainer0.<>p__Site1 == null)
				{
					CANwinConfiguration.<FinalConstruct>o__SiteContainer0.<>p__Site1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Configuration", typeof(CANwinConfiguration), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				base.ComObject = CANwinConfiguration.<FinalConstruct>o__SiteContainer0.<>p__Site1.Target(CANwinConfiguration.<FinalConstruct>o__SiteContainer0.<>p__Site1, base.Parent.ComObject);
				result = base.FinalConstruct();
			}
			catch (Exception)
			{
				base.FinalConstruct();
				result = false;
			}
			return result;
		}

		protected override bool FinalRelease()
		{
			if (this.mGeneralSetup != null)
			{
				this.mGeneralSetup.Dispose();
				this.mGeneralSetup = null;
			}
			if (this.mSimulationSetup != null)
			{
				this.mSimulationSetup.Dispose();
				this.mSimulationSetup = null;
			}
			if (this.mOnlineSetup != null)
			{
				this.mOnlineSetup.Dispose();
				this.mOnlineSetup = null;
			}
			if (this.mOfflineSetup != null)
			{
				this.mOfflineSetup.Dispose();
				this.mOfflineSetup = null;
			}
			return base.FinalRelease();
		}

		public bool GetModified(out bool modified)
		{
			modified = false;
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinConfiguration.<GetModified>o__SiteContainer2.<>p__Site3 == null)
				{
					CANwinConfiguration.<GetModified>o__SiteContainer2.<>p__Site3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(bool), typeof(CANwinConfiguration)));
				}
				Func<CallSite, object, bool> arg_A7_0 = CANwinConfiguration.<GetModified>o__SiteContainer2.<>p__Site3.Target;
				CallSite arg_A7_1 = CANwinConfiguration.<GetModified>o__SiteContainer2.<>p__Site3;
				if (CANwinConfiguration.<GetModified>o__SiteContainer2.<>p__Site4 == null)
				{
					CANwinConfiguration.<GetModified>o__SiteContainer2.<>p__Site4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Modified", typeof(CANwinConfiguration), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				modified = arg_A7_0(arg_A7_1, CANwinConfiguration.<GetModified>o__SiteContainer2.<>p__Site4.Target(CANwinConfiguration.<GetModified>o__SiteContainer2.<>p__Site4, base.ComObject));
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool SetModified(bool modified)
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinConfiguration.<SetModified>o__SiteContainer5.<>p__Site6 == null)
				{
					CANwinConfiguration.<SetModified>o__SiteContainer5.<>p__Site6 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Modified", typeof(CANwinConfiguration), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				CANwinConfiguration.<SetModified>o__SiteContainer5.<>p__Site6.Target(CANwinConfiguration.<SetModified>o__SiteContainer5.<>p__Site6, base.ComObject, modified);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool GetMode(out ConfigurationMode mode)
		{
			mode = ConfigurationMode.Online;
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinConfiguration.<GetMode>o__SiteContainer7.<>p__Site8 == null)
				{
					CANwinConfiguration.<GetMode>o__SiteContainer7.<>p__Site8 = CallSite<Func<CallSite, object, ConfigurationMode>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(ConfigurationMode), typeof(CANwinConfiguration)));
				}
				Func<CallSite, object, ConfigurationMode> arg_A7_0 = CANwinConfiguration.<GetMode>o__SiteContainer7.<>p__Site8.Target;
				CallSite arg_A7_1 = CANwinConfiguration.<GetMode>o__SiteContainer7.<>p__Site8;
				if (CANwinConfiguration.<GetMode>o__SiteContainer7.<>p__Site9 == null)
				{
					CANwinConfiguration.<GetMode>o__SiteContainer7.<>p__Site9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Mode", typeof(CANwinConfiguration), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				mode = arg_A7_0(arg_A7_1, CANwinConfiguration.<GetMode>o__SiteContainer7.<>p__Site9.Target(CANwinConfiguration.<GetMode>o__SiteContainer7.<>p__Site9, base.ComObject));
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool SetMode(ConfigurationMode mode)
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinConfiguration.<SetMode>o__SiteContainera.<>p__Siteb == null)
				{
					CANwinConfiguration.<SetMode>o__SiteContainera.<>p__Siteb = CallSite<Func<CallSite, object, ConfigurationMode, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Mode", typeof(CANwinConfiguration), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				CANwinConfiguration.<SetMode>o__SiteContainera.<>p__Siteb.Target(CANwinConfiguration.<SetMode>o__SiteContainera.<>p__Siteb, base.ComObject, mode);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool Save(string filePath = null, bool propmtUser = false)
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (!string.IsNullOrEmpty(filePath))
				{
					if (CANwinConfiguration.<Save>o__SiteContainerc.<>p__Sited == null)
					{
						CANwinConfiguration.<Save>o__SiteContainerc.<>p__Sited = CallSite<Action<CallSite, object, string, bool>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Save", null, typeof(CANwinConfiguration), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					CANwinConfiguration.<Save>o__SiteContainerc.<>p__Sited.Target(CANwinConfiguration.<Save>o__SiteContainerc.<>p__Sited, base.ComObject, filePath, propmtUser);
				}
				else
				{
					if (CANwinConfiguration.<Save>o__SiteContainerc.<>p__Sitee == null)
					{
						CANwinConfiguration.<Save>o__SiteContainerc.<>p__Sitee = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Save", null, typeof(CANwinConfiguration), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					CANwinConfiguration.<Save>o__SiteContainerc.<>p__Sitee.Target(CANwinConfiguration.<Save>o__SiteContainerc.<>p__Sitee, base.ComObject);
				}
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool GetFullName(out string fullName)
		{
			fullName = string.Empty;
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinConfiguration.<GetFullName>o__SiteContainerf.<>p__Site10 == null)
				{
					CANwinConfiguration.<GetFullName>o__SiteContainerf.<>p__Site10 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(CANwinConfiguration)));
				}
				Func<CallSite, object, string> arg_AB_0 = CANwinConfiguration.<GetFullName>o__SiteContainerf.<>p__Site10.Target;
				CallSite arg_AB_1 = CANwinConfiguration.<GetFullName>o__SiteContainerf.<>p__Site10;
				if (CANwinConfiguration.<GetFullName>o__SiteContainerf.<>p__Site11 == null)
				{
					CANwinConfiguration.<GetFullName>o__SiteContainerf.<>p__Site11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "FullName", typeof(CANwinConfiguration), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				fullName = arg_AB_0(arg_AB_1, CANwinConfiguration.<GetFullName>o__SiteContainerf.<>p__Site11.Target(CANwinConfiguration.<GetFullName>o__SiteContainerf.<>p__Site11, base.ComObject));
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
	}
}
