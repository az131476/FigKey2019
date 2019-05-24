using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vector.VLConfig.CANwinAccess.Data;

namespace Vector.VLConfig.CANwinAccess.COM
{
	internal class CANwinGeneralSetup : ComBase
	{
		[CompilerGenerated]
		private static class <FinalConstruct>o__SiteContainer0
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Site1;
		}

		[CompilerGenerated]
		private static class <AssignDatabases>o__SiteContainer2
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Site3;

			public static CallSite<Func<CallSite, object, object>> <>p__Site4;

			public static CallSite<Action<CallSite, object, int>> <>p__Site5;

			public static CallSite<Func<CallSite, object, bool>> <>p__Site6;

			public static CallSite<Func<CallSite, object, int, object>> <>p__Site7;

			public static CallSite<Func<CallSite, object, object>> <>p__Site8;

			public static CallSite<Func<CallSite, object, string, object>> <>p__Site9;

			public static CallSite<Func<CallSite, object, int, object>> <>p__Sitea;
		}

		[CompilerGenerated]
		private static class <SetChannelUsage>o__SiteContainerd
		{
			public static CallSite<Func<CallSite, object, int, int, object>> <>p__Sitee;

			public static CallSite<Func<CallSite, object, object>> <>p__Sitef;
		}

		public CANwinGeneralSetup(CANwinConfiguration parent) : base(parent)
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
				if (CANwinGeneralSetup.<FinalConstruct>o__SiteContainer0.<>p__Site1 == null)
				{
					CANwinGeneralSetup.<FinalConstruct>o__SiteContainer0.<>p__Site1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "GeneralSetup", typeof(CANwinGeneralSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				base.ComObject = CANwinGeneralSetup.<FinalConstruct>o__SiteContainer0.<>p__Site1.Target(CANwinGeneralSetup.<FinalConstruct>o__SiteContainer0.<>p__Site1, base.Parent.ComObject);
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
			return base.FinalRelease();
		}

		public bool SetChannelUsage(IList<CANwinBusItem> busConfiguration)
		{
			return base.IsInitialized && (this.SetChannelUsage(CANwinBusType.CAN, busConfiguration) && this.SetChannelUsage(CANwinBusType.LIN, busConfiguration) && this.SetChannelUsage(CANwinBusType.MOST, busConfiguration)) && this.SetChannelUsage(CANwinBusType.FLEXRAY, busConfiguration);
		}

		public bool AssignDatabases(IList<CANwinBusItem> busConfiguration)
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			if (base.Parent.Parent is CANoeApplication)
			{
				return false;
			}
			object arg;
			try
			{
				if (CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site3 == null)
				{
					CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Databases", typeof(CANwinGeneralSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> arg_CB_0 = CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site3.Target;
				CallSite arg_CB_1 = CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site3;
				if (CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site4 == null)
				{
					CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "DatabaseSetup", typeof(CANwinGeneralSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				arg = arg_CB_0(arg_CB_1, CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site4.Target(CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site4, base.ComObject));
				while (true)
				{
					if (CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site6 == null)
					{
						CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CANwinGeneralSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, bool> arg_246_0 = CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site6.Target;
					CallSite arg_246_1 = CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site6;
					if (CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site7 == null)
					{
						CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site7 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(CANwinGeneralSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					Func<CallSite, object, int, object> arg_241_0 = CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site7.Target;
					CallSite arg_241_1 = CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site7;
					if (CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site8 == null)
					{
						CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(CANwinGeneralSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					if (!arg_246_0(arg_246_1, arg_241_0(arg_241_1, CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site8.Target(CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site8, arg), 0)))
					{
						break;
					}
					if (CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site5 == null)
					{
						CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site5 = CallSite<Action<CallSite, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", null, typeof(CANwinGeneralSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site5.Target(CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site5, arg, 1);
				}
			}
			catch (Exception)
			{
				return false;
			}
			List<CANwinBusItem> list = new List<CANwinBusItem>(busConfiguration);
			list.Sort((CANwinBusItem bi1, CANwinBusItem bi2) => bi2.ChannelNumber.CompareTo(bi1.ChannelNumber));
			try
			{
				foreach (CANwinBusItem current in list)
				{
					foreach (string current2 in current.AbsoluteDatabaseFilePaths)
					{
						if (CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site9 == null)
						{
							CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Add", null, typeof(CANwinGeneralSetup), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						object arg2 = CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site9.Target(CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Site9, arg, current2);
						if (CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Sitea == null)
						{
							CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Sitea = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Channel", typeof(CANwinGeneralSetup), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Sitea.Target(CANwinGeneralSetup.<AssignDatabases>o__SiteContainer2.<>p__Sitea, arg2, current.ChannelNumber);
					}
				}
			}
			catch (Exception)
			{
			}
			return true;
		}

		private bool SetChannelUsage(CANwinBusType busType, IEnumerable<CANwinBusItem> busConfiguration)
		{
			bool result;
			try
			{
				if (CANwinGeneralSetup.<SetChannelUsage>o__SiteContainerd.<>p__Sitee == null)
				{
					CANwinGeneralSetup.<SetChannelUsage>o__SiteContainerd.<>p__Sitee = CallSite<Func<CallSite, object, int, int, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof(CANwinGeneralSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				Func<CallSite, object, int, int, object> arg_C7_0 = CANwinGeneralSetup.<SetChannelUsage>o__SiteContainerd.<>p__Sitee.Target;
				CallSite arg_C7_1 = CANwinGeneralSetup.<SetChannelUsage>o__SiteContainerd.<>p__Sitee;
				if (CANwinGeneralSetup.<SetChannelUsage>o__SiteContainerd.<>p__Sitef == null)
				{
					CANwinGeneralSetup.<SetChannelUsage>o__SiteContainerd.<>p__Sitef = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Channels", typeof(CANwinGeneralSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				arg_C7_0(arg_C7_1, CANwinGeneralSetup.<SetChannelUsage>o__SiteContainerd.<>p__Sitef.Target(CANwinGeneralSetup.<SetChannelUsage>o__SiteContainerd.<>p__Sitef, base.ComObject), (int)busType, CANwinBusItem.GetMaxChannelNumber(busType, busConfiguration));
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
