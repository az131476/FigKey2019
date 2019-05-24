using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Runtime.CompilerServices;

namespace Vector.VLConfig.CANwinAccess.COM
{
	internal class CANwinOfflineSetup : CANwinMeasurementSetup
	{
		[CompilerGenerated]
		private static class <FinalConstruct>o__SiteContainer0
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Site1;
		}

		protected override bool IsOnline
		{
			get
			{
				return false;
			}
		}

		public CANwinOfflineSetup(CANwinConfiguration parent) : base(parent)
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
				if (CANwinOfflineSetup.<FinalConstruct>o__SiteContainer0.<>p__Site1 == null)
				{
					CANwinOfflineSetup.<FinalConstruct>o__SiteContainer0.<>p__Site1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "OfflineSetup", typeof(CANwinOfflineSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				base.ComObject = CANwinOfflineSetup.<FinalConstruct>o__SiteContainer0.<>p__Site1.Target(CANwinOfflineSetup.<FinalConstruct>o__SiteContainer0.<>p__Site1, base.Parent.ComObject);
				result = base.FinalConstruct();
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		protected override bool FinalRelease()
		{
			return base.FinalRelease();
		}
	}
}
