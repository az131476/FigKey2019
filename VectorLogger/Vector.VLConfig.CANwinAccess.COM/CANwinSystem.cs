using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Vector.VLConfig.CANwinAccess.COM
{
	internal class CANwinSystem : ComBase
	{
		[CompilerGenerated]
		private static class <FinalConstruct>o__SiteContainer0
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Site1;
		}

		[CompilerGenerated]
		private static class <CleanupVariablesFiles>o__SiteContainer2
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Site3;

			public static CallSite<Action<CallSite, object, int>> <>p__Site4;

			public static CallSite<Func<CallSite, object, bool>> <>p__Site5;

			public static CallSite<Func<CallSite, object, int, object>> <>p__Site6;

			public static CallSite<Func<CallSite, object, object>> <>p__Site7;
		}

		[CompilerGenerated]
		private static class <AssignVariablesFiles>o__SiteContainer8
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Site9;

			public static CallSite<Action<CallSite, object, string>> <>p__Sitea;
		}

		public CANwinSystem(CANwinApplication parent) : base(parent)
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
				if (CANwinSystem.<FinalConstruct>o__SiteContainer0.<>p__Site1 == null)
				{
					CANwinSystem.<FinalConstruct>o__SiteContainer0.<>p__Site1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "System", typeof(CANwinSystem), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				base.ComObject = CANwinSystem.<FinalConstruct>o__SiteContainer0.<>p__Site1.Target(CANwinSystem.<FinalConstruct>o__SiteContainer0.<>p__Site1, base.Parent.ComObject);
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

		public bool CleanupVariablesFiles()
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site3 == null)
				{
					CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "VariablesFiles", typeof(CANwinSystem), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object arg = CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site3.Target(CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site3, base.ComObject);
				while (true)
				{
					if (CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site5 == null)
					{
						CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(CANwinSystem), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Func<CallSite, object, bool> arg_1D9_0 = CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site5.Target;
					CallSite arg_1D9_1 = CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site5;
					if (CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site6 == null)
					{
						CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site6 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof(CANwinSystem), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					Func<CallSite, object, int, object> arg_1D4_0 = CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site6.Target;
					CallSite arg_1D4_1 = CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site6;
					if (CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site7 == null)
					{
						CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(CANwinSystem), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					if (!arg_1D9_0(arg_1D9_1, arg_1D4_0(arg_1D4_1, CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site7.Target(CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site7, arg), 0)))
					{
						break;
					}
					if (CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site4 == null)
					{
						CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site4 = CallSite<Action<CallSite, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", null, typeof(CANwinSystem), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
						}));
					}
					CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site4.Target(CANwinSystem.<CleanupVariablesFiles>o__SiteContainer2.<>p__Site4, arg, 1);
				}
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool AssignVariablesFiles(IList<string> filePathList)
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			if (!this.CleanupVariablesFiles())
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinSystem.<AssignVariablesFiles>o__SiteContainer8.<>p__Site9 == null)
				{
					CANwinSystem.<AssignVariablesFiles>o__SiteContainer8.<>p__Site9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "VariablesFiles", typeof(CANwinSystem), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object arg = CANwinSystem.<AssignVariablesFiles>o__SiteContainer8.<>p__Site9.Target(CANwinSystem.<AssignVariablesFiles>o__SiteContainer8.<>p__Site9, base.ComObject);
				foreach (string current in filePathList)
				{
					if (CANwinSystem.<AssignVariablesFiles>o__SiteContainer8.<>p__Sitea == null)
					{
						CANwinSystem.<AssignVariablesFiles>o__SiteContainer8.<>p__Sitea = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(CANwinSystem), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					CANwinSystem.<AssignVariablesFiles>o__SiteContainer8.<>p__Sitea.Target(CANwinSystem.<AssignVariablesFiles>o__SiteContainer8.<>p__Sitea, arg, current);
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
