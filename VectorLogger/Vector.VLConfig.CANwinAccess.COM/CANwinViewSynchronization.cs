using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Runtime.CompilerServices;

namespace Vector.VLConfig.CANwinAccess.COM
{
	internal class CANwinViewSynchronization : ComBase
	{
		[CompilerGenerated]
		private static class <FinalConstruct>o__SiteContainer17
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Site18;
		}

		[CompilerGenerated]
		private static class <SetEnabled>o__SiteContainer19
		{
			public static CallSite<Func<CallSite, object, bool, object>> <>p__Site1a;
		}

		[CompilerGenerated]
		private static class <SetTimeNs>o__SiteContainer1b
		{
			public static CallSite<Action<CallSite, object, uint, uint>> <>p__Site1c;
		}

		public CANwinViewSynchronization(CANwinMeasurementSetup parent) : base(parent)
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
				if (CANwinViewSynchronization.<FinalConstruct>o__SiteContainer17.<>p__Site18 == null)
				{
					CANwinViewSynchronization.<FinalConstruct>o__SiteContainer17.<>p__Site18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ViewSynchronization", typeof(CANwinViewSynchronization), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				base.ComObject = CANwinViewSynchronization.<FinalConstruct>o__SiteContainer17.<>p__Site18.Target(CANwinViewSynchronization.<FinalConstruct>o__SiteContainer17.<>p__Site18, base.Parent.ComObject);
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

		public bool SetEnabled(bool enable)
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinViewSynchronization.<SetEnabled>o__SiteContainer19.<>p__Site1a == null)
				{
					CANwinViewSynchronization.<SetEnabled>o__SiteContainer19.<>p__Site1a = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Enabled", typeof(CANwinViewSynchronization), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				CANwinViewSynchronization.<SetEnabled>o__SiteContainer19.<>p__Site1a.Target(CANwinViewSynchronization.<SetEnabled>o__SiteContainer19.<>p__Site1a, base.ComObject, enable);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool SetTimeNs(ulong timePos)
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				uint arg = Convert.ToUInt32(timePos & (ulong)-1);
				uint arg2 = Convert.ToUInt32((timePos & 18446744069414584320uL) >> 32);
				if (CANwinViewSynchronization.<SetTimeNs>o__SiteContainer1b.<>p__Site1c == null)
				{
					CANwinViewSynchronization.<SetTimeNs>o__SiteContainer1b.<>p__Site1c = CallSite<Action<CallSite, object, uint, uint>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SetTimeNs", null, typeof(CANwinViewSynchronization), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				CANwinViewSynchronization.<SetTimeNs>o__SiteContainer1b.<>p__Site1c.Target(CANwinViewSynchronization.<SetTimeNs>o__SiteContainer1b.<>p__Site1c, base.ComObject, arg2, arg);
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
