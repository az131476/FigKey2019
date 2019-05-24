using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vector.VLConfig.CANwinAccess.COM
{
	internal abstract class ComBase : IDisposable
	{
		[CompilerGenerated]
		private static class <FinalConstruct>o__SiteContainer0
		{
			public static CallSite<Func<CallSite, object, bool>> <>p__Site1;

			public static CallSite<Func<CallSite, object, object, object>> <>p__Site2;
		}

		[CompilerGenerated]
		private static class <FinalRelease>o__SiteContainer3
		{
			public static CallSite<Func<CallSite, object, bool>> <>p__Site4;

			public static CallSite<Func<CallSite, object, object, object>> <>p__Site5;

			public static CallSite<Action<CallSite, ComBase, object>> <>p__Site6;
		}

		[CompilerGenerated]
		private static class <ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd
		{
			public static CallSite<Func<CallSite, object, IEnumerable>> <>p__Sitee;

			public static CallSite<Func<CallSite, object, bool>> <>p__Sitef;

			public static CallSite<Func<CallSite, object, string, StringComparison, object>> <>p__Site10;

			public static CallSite<Func<CallSite, object, object>> <>p__Site11;
		}

		[CompilerGenerated]
		private static class <GetComObjectFromCollectionWithNamePropery>o__SiteContainer12
		{
			public static CallSite<Func<CallSite, object, IEnumerable>> <>p__Site13;

			public static CallSite<Func<CallSite, object, bool>> <>p__Site14;

			public static CallSite<Func<CallSite, object, string, StringComparison, object>> <>p__Site15;

			public static CallSite<Func<CallSite, object, object>> <>p__Site16;
		}

		[Dynamic]
		public dynamic ComObject
		{
			[return: Dynamic]
			get;
			[param: Dynamic]
			protected set;
		}

		public ComBase Parent
		{
			get;
			private set;
		}

		public bool IsInitialized
		{
			get;
			private set;
		}

		private bool IsDisposed
		{
			get;
			set;
		}

		protected virtual bool FinalConstruct()
		{
			if (ComBase.<FinalConstruct>o__SiteContainer0.<>p__Site1 == null)
			{
				ComBase.<FinalConstruct>o__SiteContainer0.<>p__Site1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(bool), typeof(ComBase)));
			}
			Func<CallSite, object, bool> arg_A2_0 = ComBase.<FinalConstruct>o__SiteContainer0.<>p__Site1.Target;
			CallSite arg_A2_1 = ComBase.<FinalConstruct>o__SiteContainer0.<>p__Site1;
			if (ComBase.<FinalConstruct>o__SiteContainer0.<>p__Site2 == null)
			{
				ComBase.<FinalConstruct>o__SiteContainer0.<>p__Site2 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(ComBase), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
				}));
			}
			return arg_A2_0(arg_A2_1, ComBase.<FinalConstruct>o__SiteContainer0.<>p__Site2.Target(ComBase.<FinalConstruct>o__SiteContainer0.<>p__Site2, this.ComObject, null));
		}

		protected virtual bool FinalRelease()
		{
			if (ComBase.<FinalRelease>o__SiteContainer3.<>p__Site4 == null)
			{
				ComBase.<FinalRelease>o__SiteContainer3.<>p__Site4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ComBase), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Func<CallSite, object, bool> arg_B0_0 = ComBase.<FinalRelease>o__SiteContainer3.<>p__Site4.Target;
			CallSite arg_B0_1 = ComBase.<FinalRelease>o__SiteContainer3.<>p__Site4;
			if (ComBase.<FinalRelease>o__SiteContainer3.<>p__Site5 == null)
			{
				ComBase.<FinalRelease>o__SiteContainer3.<>p__Site5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof(ComBase), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, null)
				}));
			}
			if (arg_B0_0(arg_B0_1, ComBase.<FinalRelease>o__SiteContainer3.<>p__Site5.Target(ComBase.<FinalRelease>o__SiteContainer3.<>p__Site5, this.ComObject, null)))
			{
				if (ComBase.<FinalRelease>o__SiteContainer3.<>p__Site6 == null)
				{
					ComBase.<FinalRelease>o__SiteContainer3.<>p__Site6 = CallSite<Action<CallSite, ComBase, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "ReleaseComObject", null, typeof(ComBase), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				ComBase.<FinalRelease>o__SiteContainer3.<>p__Site6.Target(ComBase.<FinalRelease>o__SiteContainer3.<>p__Site6, this, this.ComObject);
				this.ComObject = null;
			}
			return true;
		}

		protected ComBase(ComBase parent)
		{
			this.IsDisposed = false;
			this.IsInitialized = false;
			this.Parent = parent;
			this.IsInitialized = this.FinalConstruct();
		}

		protected override void Finalize()
		{
			try
			{
				bool arg_06_0 = this.IsDisposed;
			}
			finally
			{
				base.Finalize();
			}
		}

		public void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			this.Parent = null;
			this.IsDisposed = this.FinalRelease();
			this.IsInitialized = false;
		}

		protected virtual void DisposeOnQuit()
		{
			if (this.IsDisposed)
			{
				return;
			}
			this.Parent = null;
			this.IsDisposed = true;
			this.IsInitialized = false;
		}

		protected int ReleaseComObject(object comObject)
		{
			if (comObject != null)
			{
				return Marshal.ReleaseComObject(comObject);
			}
			return 0;
		}

		protected static bool Contains(IEnumerable<string> stringList, string stringToFind)
		{
			return stringList.Any((string listEntry) => listEntry.Equals(stringToFind, StringComparison.OrdinalIgnoreCase));
		}

		protected static string GetListValue(IEnumerable<string> stringList, string stringToFind)
		{
			return stringList.FirstOrDefault((string entry) => entry.Equals(stringToFind, StringComparison.OrdinalIgnoreCase));
		}

		protected static bool ComObjectCollectionWithFullNameProperyContains([Dynamic] dynamic comObjectCollection, string fullFilePath)
		{
			if (ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Sitee == null)
			{
				ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Sitee = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(ComBase)));
			}
			foreach (object current in ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Sitee.Target(ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Sitee, comObjectCollection))
			{
				if (ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Sitef == null)
				{
					ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Sitef = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ComBase), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, bool> arg_169_0 = ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Sitef.Target;
				CallSite arg_169_1 = ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Sitef;
				if (ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Site10 == null)
				{
					ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Site10 = CallSite<Func<CallSite, object, string, StringComparison, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Equals", null, typeof(ComBase), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, string, StringComparison, object> arg_164_0 = ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Site10.Target;
				CallSite arg_164_1 = ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Site10;
				if (ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Site11 == null)
				{
					ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Site11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "FullName", typeof(ComBase), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				if (arg_169_0(arg_169_1, arg_164_0(arg_164_1, ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Site11.Target(ComBase.<ComObjectCollectionWithFullNameProperyContains>o__SiteContainerd.<>p__Site11, current), fullFilePath, StringComparison.OrdinalIgnoreCase)))
				{
					return true;
				}
			}
			return false;
		}

		[return: Dynamic]
		protected static dynamic GetComObjectFromCollectionWithNamePropery([Dynamic] dynamic comObjectCollection, string name)
		{
			if (ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site13 == null)
			{
				ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site13 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(ComBase)));
			}
			foreach (object current in ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site13.Target(ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site13, comObjectCollection))
			{
				if (ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site14 == null)
				{
					ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site14 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(ComBase), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, bool> arg_169_0 = ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site14.Target;
				CallSite arg_169_1 = ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site14;
				if (ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site15 == null)
				{
					ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site15 = CallSite<Func<CallSite, object, string, StringComparison, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Equals", null, typeof(ComBase), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<CallSite, object, string, StringComparison, object> arg_164_0 = ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site15.Target;
				CallSite arg_164_1 = ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site15;
				if (ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site16 == null)
				{
					ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Name", typeof(ComBase), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				if (arg_169_0(arg_169_1, arg_164_0(arg_164_1, ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site16.Target(ComBase.<GetComObjectFromCollectionWithNamePropery>o__SiteContainer12.<>p__Site16, current), name, StringComparison.OrdinalIgnoreCase)))
				{
					return current;
				}
			}
			return null;
		}

		[Conditional("DEBUG")]
		public static void DebugInfo(Exception e)
		{
		}

		[Conditional("DEBUG")]
		public static void DebugInfo(string message)
		{
		}
	}
}
