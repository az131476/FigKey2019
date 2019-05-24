using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Runtime.CompilerServices;
using Vector.VLConfig.GeneralUtil.QuickView;

namespace Vector.VLConfig.CANwinAccess.COM
{
	internal abstract class CANwinMeasurementSetup : ComBase
	{
		[CompilerGenerated]
		private static class <ClearChannelMapping>o__SiteContainer0
		{
			public static CallSite<Func<CallSite, object, CANwinBusType, object>> <>p__Site1;

			public static CallSite<Func<CallSite, object, object>> <>p__Site2;

			public static CallSite<Action<CallSite, object, int, int>> <>p__Site3;

			public static CallSite<Func<CallSite, object, CANwinBusType, object>> <>p__Site4;

			public static CallSite<Func<CallSite, object, object>> <>p__Site5;

			public static CallSite<Action<CallSite, object, int, int>> <>p__Site6;

			public static CallSite<Func<CallSite, object, CANwinBusType, object>> <>p__Site7;

			public static CallSite<Func<CallSite, object, object>> <>p__Site8;

			public static CallSite<Action<CallSite, object, int, int>> <>p__Site9;

			public static CallSite<Func<CallSite, object, CANwinBusType, object>> <>p__Sitea;

			public static CallSite<Func<CallSite, object, object>> <>p__Siteb;

			public static CallSite<Action<CallSite, object, int, int>> <>p__Sitec;
		}

		[CompilerGenerated]
		private static class <SetSources>o__SiteContainerd
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Sitee;

			public static CallSite<Func<CallSite, object, object>> <>p__Sitef;

			public static CallSite<Action<CallSite, object>> <>p__Site10;

			public static CallSite<Action<CallSite, object, string>> <>p__Site11;
		}

		[CompilerGenerated]
		private static class <SetTimeSection>o__SiteContainer12
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Site13;

			public static CallSite<Func<CallSite, object, object>> <>p__Site14;

			public static CallSite<Func<CallSite, object, int, object>> <>p__Site15;

			public static CallSite<Func<CallSite, object, string, object>> <>p__Site16;

			public static CallSite<Func<CallSite, object, string, object>> <>p__Site17;

			public static CallSite<Func<CallSite, object, int, object>> <>p__Site18;

			public static CallSite<Func<CallSite, object, string, object>> <>p__Site19;

			public static CallSite<Func<CallSite, object, string, object>> <>p__Site1a;
		}

		private const string cDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

		private CANwinViewSynchronization mViewSynchronization;

		protected abstract bool IsOnline
		{
			get;
		}

		public CANwinViewSynchronization ViewSynchronization
		{
			get
			{
				CANwinViewSynchronization arg_1C_0;
				if ((arg_1C_0 = this.mViewSynchronization) == null)
				{
					arg_1C_0 = (this.mViewSynchronization = new CANwinViewSynchronization(this));
				}
				return arg_1C_0;
			}
		}

		public static CANwinMeasurementSetup Create(CANwinConfiguration parent, bool isOnline)
		{
			if (isOnline)
			{
				return new CANwinOnlineSetup(parent);
			}
			return new CANwinOfflineSetup(parent);
		}

		protected CANwinMeasurementSetup(CANwinConfiguration parent) : base(parent)
		{
		}

		protected override bool FinalConstruct()
		{
			return base.FinalConstruct();
		}

		protected override bool FinalRelease()
		{
			if (this.mViewSynchronization != null)
			{
				this.mViewSynchronization.Dispose();
				this.mViewSynchronization = null;
			}
			return base.FinalRelease();
		}

		public bool ConfigureSource(OfflineSourceConfig offlineSourceConfig)
		{
			return !this.IsOnline && base.IsInitialized && this.SetSources(offlineSourceConfig) && this.SetTimeSection(offlineSourceConfig);
		}

		public bool ClearChannelMapping()
		{
			if (this.IsOnline || !base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site1 == null)
				{
					CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site1 = CallSite<Func<CallSite, object, CANwinBusType, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetMappingTable", null, typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, CANwinBusType, object> arg_CF_0 = CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site1.Target;
				CallSite arg_CF_1 = CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site1;
				if (CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site2 == null)
				{
					CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Source", typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object arg = arg_CF_0(arg_CF_1, CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site2.Target(CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site2, base.ComObject), CANwinBusType.CAN);
				for (int i = 1; i <= Constants.NumCANChannels; i++)
				{
					if (CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site3 == null)
					{
						CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site3 = CallSite<Action<CallSite, object, int, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Put", null, typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site3.Target(CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site3, arg, i, i);
				}
				if (CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site4 == null)
				{
					CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site4 = CallSite<Func<CallSite, object, CANwinBusType, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetMappingTable", null, typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, CANwinBusType, object> arg_225_0 = CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site4.Target;
				CallSite arg_225_1 = CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site4;
				if (CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site5 == null)
				{
					CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Source", typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				arg = arg_225_0(arg_225_1, CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site5.Target(CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site5, base.ComObject), CANwinBusType.LIN);
				for (int j = 1; j <= Constants.NumLinChannels; j++)
				{
					if (CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site6 == null)
					{
						CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site6 = CallSite<Action<CallSite, object, int, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Put", null, typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site6.Target(CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site6, arg, j, j);
				}
				if (CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site7 == null)
				{
					CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site7 = CallSite<Func<CallSite, object, CANwinBusType, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetMappingTable", null, typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, CANwinBusType, object> arg_381_0 = CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site7.Target;
				CallSite arg_381_1 = CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site7;
				if (CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site8 == null)
				{
					CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Source", typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				arg = arg_381_0(arg_381_1, CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site8.Target(CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site8, base.ComObject), CANwinBusType.MOST);
				for (int k = 1; k <= Constants.NumMOSTChannels; k++)
				{
					if (CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site9 == null)
					{
						CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site9 = CallSite<Action<CallSite, object, int, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Put", null, typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site9.Target(CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Site9, arg, k, k);
				}
				if (CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Sitea == null)
				{
					CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Sitea = CallSite<Func<CallSite, object, CANwinBusType, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetMappingTable", null, typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, CANwinBusType, object> arg_4DD_0 = CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Sitea.Target;
				CallSite arg_4DD_1 = CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Sitea;
				if (CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Siteb == null)
				{
					CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Siteb = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Source", typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				arg = arg_4DD_0(arg_4DD_1, CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Siteb.Target(CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Siteb, base.ComObject), CANwinBusType.FLEXRAY);
				for (int l = 1; l <= Constants.NumFlexrayChannels; l++)
				{
					if (CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Sitec == null)
					{
						CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Sitec = CallSite<Action<CallSite, object, int, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Put", null, typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Sitec.Target(CANwinMeasurementSetup.<ClearChannelMapping>o__SiteContainer0.<>p__Sitec, arg, l, l);
				}
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		private bool SetSources(OfflineSourceConfig offlineSourceConfig)
		{
			if (this.IsOnline || !base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Sitee == null)
				{
					CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Sitee = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Sources", typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> arg_BF_0 = CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Sitee.Target;
				CallSite arg_BF_1 = CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Sitee;
				if (CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Sitef == null)
				{
					CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Sitef = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Source", typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object arg = arg_BF_0(arg_BF_1, CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Sitef.Target(CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Sitef, base.ComObject));
				if (CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Site10 == null)
				{
					CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Site10 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Clear", null, typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Site10.Target(CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Site10, arg);
				foreach (Tuple<string, string> current in offlineSourceConfig.OfflineSourceFiles)
				{
					if (CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Site11 == null)
					{
						CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Site11 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Site11.Target(CANwinMeasurementSetup.<SetSources>o__SiteContainerd.<>p__Site11, arg, current.Item2);
				}
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		private bool SetTimeSection(OfflineSourceConfig offlineSourceConfig)
		{
			if (this.IsOnline || !base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site13 == null)
				{
					CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TimeSection", typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> arg_BF_0 = CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site13.Target;
				CallSite arg_BF_1 = CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site13;
				if (CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site14 == null)
				{
					CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Source", typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object arg = arg_BF_0(arg_BF_1, CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site14.Target(CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site14, base.ComObject));
				if (offlineSourceConfig.TimeSectionType == TimeSectionType.IntervalBetweenTwoDateTimes)
				{
					if (CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site15 == null)
					{
						CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site15 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Type", typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site15.Target(CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site15, arg, 6);
					string arg2 = offlineSourceConfig.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss");
					if (CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site16 == null)
					{
						CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Start", typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site16.Target(CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site16, arg, arg2);
					string arg3 = offlineSourceConfig.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss");
					if (CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site17 == null)
					{
						CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site17 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "End", typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site17.Target(CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site17, arg, arg3);
					result = true;
				}
				else if (offlineSourceConfig.TimeSectionType == TimeSectionType.BreakAtTimeStamp)
				{
					if (CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site18 == null)
					{
						CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site18 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Type", typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site18.Target(CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site18, arg, 3);
					if (CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site19 == null)
					{
						CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site19 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Start", typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site19.Target(CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site19, arg, offlineSourceConfig.EndTimeStamp);
					if (CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site1a == null)
					{
						CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site1a = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "End", typeof(CANwinMeasurementSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site1a.Target(CANwinMeasurementSetup.<SetTimeSection>o__SiteContainer12.<>p__Site1a, arg, offlineSourceConfig.StartTimeStamp);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
	}
}
