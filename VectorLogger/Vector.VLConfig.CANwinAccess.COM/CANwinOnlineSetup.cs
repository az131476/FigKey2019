using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Runtime.CompilerServices;

namespace Vector.VLConfig.CANwinAccess.COM
{
	internal class CANwinOnlineSetup : CANwinMeasurementSetup
	{
		[CompilerGenerated]
		private static class <FinalConstruct>o__SiteContainer1b
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Site1c;
		}

		protected override bool IsOnline
		{
			get
			{
				return true;
			}
		}

		public CANwinOnlineSetup(CANwinConfiguration parent) : base(parent)
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
				if (CANwinOnlineSetup.<FinalConstruct>o__SiteContainer1b.<>p__Site1c == null)
				{
					CANwinOnlineSetup.<FinalConstruct>o__SiteContainer1b.<>p__Site1c = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "OnlineSetup", typeof(CANwinOnlineSetup), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				base.ComObject = CANwinOnlineSetup.<FinalConstruct>o__SiteContainer1b.<>p__Site1c.Target(CANwinOnlineSetup.<FinalConstruct>o__SiteContainer1b.<>p__Site1c, base.Parent.ComObject);
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
