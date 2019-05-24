using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Runtime.CompilerServices;

namespace Vector.VLConfig.CANwinAccess.COM
{
	internal abstract class CANwinApplication : ComBase
	{
		[CompilerGenerated]
		private static class <CheckVersion>o__SiteContainer0
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Site1;

			public static CallSite<Func<CallSite, Type, object, object, object, Version>> <>p__Site2;

			public static CallSite<Func<CallSite, object, object>> <>p__Site3;

			public static CallSite<Func<CallSite, object, object>> <>p__Site4;

			public static CallSite<Func<CallSite, object, object>> <>p__Site5;
		}

		[CompilerGenerated]
		private static class <Quit>o__SiteContainer6
		{
			public static CallSite<Action<CallSite, object>> <>p__Site7;
		}

		[CompilerGenerated]
		private static class <Open>o__SiteContainer8
		{
			public static CallSite<Action<CallSite, object, string>> <>p__Site9;
		}

		private CANwinConfiguration mConfiguration;

		private CANwinMeasurement mMeasurement;

		private CANwinSystem mSystem;

		public abstract CANwinServerType ServerType
		{
			get;
		}

		protected abstract string ProgId
		{
			get;
		}

		public CANwinConfiguration Configuration
		{
			get
			{
				CANwinConfiguration arg_1C_0;
				if ((arg_1C_0 = this.mConfiguration) == null)
				{
					arg_1C_0 = (this.mConfiguration = new CANwinConfiguration(this));
				}
				return arg_1C_0;
			}
		}

		public CANwinMeasurement Measurement
		{
			get
			{
				CANwinMeasurement arg_1C_0;
				if ((arg_1C_0 = this.mMeasurement) == null)
				{
					arg_1C_0 = (this.mMeasurement = new CANwinMeasurement(this));
				}
				return arg_1C_0;
			}
		}

		public CANwinSystem System
		{
			get
			{
				CANwinSystem arg_1C_0;
				if ((arg_1C_0 = this.mSystem) == null)
				{
					arg_1C_0 = (this.mSystem = new CANwinSystem(this));
				}
				return arg_1C_0;
			}
		}

		public static CANwinApplication Create(CANwinServerType canwinServerType)
		{
			switch (canwinServerType)
			{
			case CANwinServerType.CANoe:
				return new CANoeApplication();
			case CANwinServerType.CANalyzer:
				return new CANalyzerApplication();
			default:
				return null;
			}
		}

		protected CANwinApplication() : base(null)
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
				base.ComObject = Activator.CreateInstance(Type.GetTypeFromProgID(this.ProgId));
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
			this.DisposeChildren();
			return base.FinalRelease();
		}

		private void DisposeChildren()
		{
			if (this.mConfiguration != null)
			{
				this.mConfiguration.Dispose();
				this.mConfiguration = null;
			}
			if (this.mMeasurement != null)
			{
				this.mMeasurement.Dispose();
				this.mMeasurement = null;
			}
			if (this.mSystem != null)
			{
				this.mSystem.Dispose();
				this.mSystem = null;
			}
		}

		protected override void DisposeOnQuit()
		{
			this.DisposeChildren();
			base.DisposeOnQuit();
		}

		public bool CheckVersion()
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site1 == null)
				{
					CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Version", typeof(CANwinApplication), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object arg = CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site1.Target(CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site1, base.ComObject);
				if (CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site2 == null)
				{
					CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site2 = CallSite<Func<CallSite, Type, object, object, object, Version>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(CANwinApplication), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, Type, object, object, object, Version> arg_1E7_0 = CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site2.Target;
				CallSite arg_1E7_1 = CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site2;
				Type arg_1E7_2 = typeof(Version);
				if (CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site3 == null)
				{
					CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "major", typeof(CANwinApplication), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object arg_1E7_3 = CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site3.Target(CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site3, arg);
				if (CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site4 == null)
				{
					CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "minor", typeof(CANwinApplication), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				object arg_1E7_4 = CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site4.Target(CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site4, arg);
				if (CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site5 == null)
				{
					CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Build", typeof(CANwinApplication), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Version v = arg_1E7_0(arg_1E7_1, arg_1E7_2, arg_1E7_3, arg_1E7_4, CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site5.Target(CANwinApplication.<CheckVersion>o__SiteContainer0.<>p__Site5, arg));
				result = (v >= Constants.ProductMinVersion);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool Quit()
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				this.DisposeOnQuit();
				if (CANwinApplication.<Quit>o__SiteContainer6.<>p__Site7 == null)
				{
					CANwinApplication.<Quit>o__SiteContainer6.<>p__Site7 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Quit", null, typeof(CANwinApplication), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				CANwinApplication.<Quit>o__SiteContainer6.<>p__Site7.Target(CANwinApplication.<Quit>o__SiteContainer6.<>p__Site7, base.ComObject);
				base.ComObject = null;
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool Open(string configToLoad)
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinApplication.<Open>o__SiteContainer8.<>p__Site9 == null)
				{
					CANwinApplication.<Open>o__SiteContainer8.<>p__Site9 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Open", null, typeof(CANwinApplication), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				CANwinApplication.<Open>o__SiteContainer8.<>p__Site9.Target(CANwinApplication.<Open>o__SiteContainer8.<>p__Site9, base.ComObject, configToLoad);
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
