using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vector.VLConfig.CANwinAccess.Data;

namespace Vector.VLConfig.CANwinAccess.COM
{
	internal class CANwinSimulationSetup : ComBase
	{
		[CompilerGenerated]
		private static class <FinalConstruct>o__SiteContainer0
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Site1;
		}

		[CompilerGenerated]
		private static class <CleanupDatabases>o__SiteContainer2
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Site3;

			public static CallSite<Func<CallSite, object, IEnumerable>> <>p__Site4;

			public static CallSite<Func<CallSite, object, object>> <>p__Site5;

			public static CallSite<Action<CallSite, object, int>> <>p__Site6;

			public static CallSite<Func<CallSite, object, bool>> <>p__Site7;

			public static CallSite<Func<CallSite, object, int, object>> <>p__Site8;

			public static CallSite<Func<CallSite, object, object>> <>p__Site9;
		}

		[CompilerGenerated]
		private static class <AssignDatabases>o__SiteContainera
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Siteb;

			public static CallSite<Func<CallSite, object, IEnumerable>> <>p__Sitec;

			public static CallSite<Func<CallSite, object, string>> <>p__Sited;

			public static CallSite<Func<CallSite, object, object>> <>p__Sitee;

			public static CallSite<Func<CallSite, object, object>> <>p__Sitef;

			public static CallSite<Action<CallSite, object, string>> <>p__Site10;
		}

		[CompilerGenerated]
		private static class <AddMissingNetworks>o__SiteContainer14
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Site15;

			public static CallSite<Func<CallSite, CANwinSimulationSetup, object, string, object>> <>p__Site16;

			public static CallSite<Func<CallSite, object, bool>> <>p__Site17;

			public static CallSite<Func<CallSite, object, object, object>> <>p__Site18;

			public static CallSite<Func<CallSite, object, string, CANwinBusType, object>> <>p__Site19;

			public static CallSite<Action<CallSite, object, CANwinBusType, int>> <>p__Site1a;

			public static CallSite<Func<CallSite, object, object>> <>p__Site1b;
		}

		public CANwinSimulationSetup(CANwinConfiguration parent) : base(parent)
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
				if (CANwinSimulationSetup.<FinalConstruct>o__SiteContainer0.<>p__Site1 == null)
				{
					CANwinSimulationSetup.<FinalConstruct>o__SiteContainer0.<>p__Site1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SimulationSetup", typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				base.ComObject = CANwinSimulationSetup.<FinalConstruct>o__SiteContainer0.<>p__Site1.Target(CANwinSimulationSetup.<FinalConstruct>o__SiteContainer0.<>p__Site1, base.Parent.ComObject);
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

		public bool ConfigureNetworks(IList<CANwinBusItem> busConfiguration)
		{
			return base.IsInitialized && busConfiguration.Any<CANwinBusItem>() && this.AddMissingNetworks(busConfiguration);
		}

		public bool CleanupDatabases()
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site3 == null)
				{
					CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Buses", typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object arg = CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site3.Target(CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site3, base.ComObject);
				if (CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site4 == null)
				{
					CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site4 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(CANwinSimulationSetup)));
				}
				foreach (object current in CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site4.Target(CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site4, arg))
				{
					if (CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site5 == null)
					{
						CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Databases", typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					object arg2 = CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site5.Target(CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site5, current);
					while (true)
					{
						if (CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site7 == null)
						{
							CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						Func<CallSite, object, bool> arg_290_0 = CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site7.Target;
						CallSite arg_290_1 = CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site7;
						if (CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site8 == null)
						{
							CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site8 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						Func<CallSite, object, int, object> arg_28B_0 = CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site8.Target;
						CallSite arg_28B_1 = CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site8;
						if (CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site9 == null)
						{
							CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						if (!arg_290_0(arg_290_1, arg_28B_0(arg_28B_1, CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site9.Target(CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site9, arg2), 0)))
						{
							break;
						}
						if (CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site6 == null)
						{
							CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site6 = CallSite<Action<CallSite, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", null, typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
							}));
						}
						CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site6.Target(CANwinSimulationSetup.<CleanupDatabases>o__SiteContainer2.<>p__Site6, arg2, 1);
					}
				}
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool AssignDatabases(IList<CANwinBusItem> busConfiguration)
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			if (!this.CleanupDatabases())
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Siteb == null)
				{
					CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Siteb = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Buses", typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object arg = CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Siteb.Target(CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Siteb, base.ComObject);
				if (CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sitec == null)
				{
					CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sitec = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(CANwinSimulationSetup)));
				}
				foreach (object current in CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sitec.Target(CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sitec, arg))
				{
					CANwinSimulationSetup.<>c__DisplayClass12 <>c__DisplayClass = new CANwinSimulationSetup.<>c__DisplayClass12();
					CANwinSimulationSetup.<>c__DisplayClass12 arg_16B_0 = <>c__DisplayClass;
					if (CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sited == null)
					{
						CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sited = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(CANwinSimulationSetup)));
					}
					Func<CallSite, object, string> arg_166_0 = CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sited.Target;
					CallSite arg_166_1 = CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sited;
					if (CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sitee == null)
					{
						CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sitee = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					arg_16B_0.busName = arg_166_0(arg_166_1, CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sitee.Target(CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sitee, current));
					if (!string.IsNullOrEmpty(<>c__DisplayClass.busName))
					{
						<>c__DisplayClass.busName = <>c__DisplayClass.busName.ToUpperInvariant();
						CANwinBusItem cANwinBusItem = busConfiguration.FirstOrDefault((CANwinBusItem bi) => bi.GenericNetworkName == <>c__DisplayClass.busName);
						if (cANwinBusItem != null)
						{
							IList<string> absoluteDatabaseFilePaths = cANwinBusItem.AbsoluteDatabaseFilePaths;
							if (absoluteDatabaseFilePaths.Any<string>())
							{
								if (CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sitef == null)
								{
									CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sitef = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Databases", typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
									}));
								}
								object arg2 = CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sitef.Target(CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Sitef, current);
								foreach (string current2 in absoluteDatabaseFilePaths)
								{
									if (CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Site10 == null)
									{
										CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Site10 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
										}));
									}
									CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Site10.Target(CANwinSimulationSetup.<AssignDatabases>o__SiteContainera.<>p__Site10, arg2, current2);
								}
							}
						}
					}
				}
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		private bool AddMissingNetworks(IEnumerable<CANwinBusItem> busConfiguration)
		{
			bool result;
			try
			{
				if (CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site15 == null)
				{
					CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Buses", typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object obj = CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site15.Target(CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site15, base.ComObject);
				foreach (CANwinBusItem current in busConfiguration)
				{
					if (CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site16 == null)
					{
						CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site16 = CallSite<Func<CallSite, CANwinSimulationSetup, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName, "GetComObjectFromCollectionWithNamePropery", null, typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					object arg = CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site16.Target(CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site16, this, obj, current.GenericNetworkName);
					if (CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site17 == null)
					{
						CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site17 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, bool> arg_19F_0 = CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site17.Target;
					CallSite arg_19F_1 = CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site17;
					if (CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site18 == null)
					{
						CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site18 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					if (arg_19F_0(arg_19F_1, CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site18.Target(CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site18, arg, null)))
					{
						if (CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site19 == null)
						{
							CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site19 = CallSite<Func<CallSite, object, string, CANwinBusType, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "AddWithType", null, typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						arg = CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site19.Target(CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site19, obj, current.GenericNetworkName, current.BusType);
						if (CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site1a == null)
						{
							CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site1a = CallSite<Action<CallSite, object, CANwinBusType, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						Action<CallSite, object, CANwinBusType, int> arg_305_0 = CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site1a.Target;
						CallSite arg_305_1 = CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site1a;
						if (CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site1b == null)
						{
							CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site1b = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Channels", typeof(CANwinSimulationSetup), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
							}));
						}
						arg_305_0(arg_305_1, CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site1b.Target(CANwinSimulationSetup.<AddMissingNetworks>o__SiteContainer14.<>p__Site1b, arg), current.BusType, current.ChannelNumber);
					}
				}
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
