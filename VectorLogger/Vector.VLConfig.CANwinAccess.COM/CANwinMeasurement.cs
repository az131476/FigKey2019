using CANalyzer;
using CANoe;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vector.VLConfig.CANwinAccess.COM
{
	internal class CANwinMeasurement : ComBase
	{
		[CompilerGenerated]
		private static class <FinalConstruct>o__SiteContainer0
		{
			public static CallSite<Func<CallSite, object, object>> <>p__Site1;
		}

		[CompilerGenerated]
		private static class <GetRunning>o__SiteContainer2
		{
			public static CallSite<Func<CallSite, object, bool>> <>p__Site3;

			public static CallSite<Func<CallSite, object, object>> <>p__Site4;
		}

		[CompilerGenerated]
		private static class <SetRunning>o__SiteContainer5
		{
			public static CallSite<Func<CallSite, object, bool, object>> <>p__Site6;
		}

		[CompilerGenerated]
		private static class <Start>o__SiteContainer7
		{
			public static CallSite<Action<CallSite, object>> <>p__Site8;
		}

		[CompilerGenerated]
		private static class <Stop>o__SiteContainer9
		{
			public static CallSite<Action<CallSite, object>> <>p__Sitea;
		}

		private readonly CANwinServerType mServerType;

		private event VoidEventHandler OnInitInternal;

		public event VoidEventHandler OnInit
		{
			add
			{
				if (this.CANoeMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANoe._IMeasurementEvents_Event), "OnInit").AddEventHandler(this.CANoeMeasurement, new CANoe._IMeasurementEvents_OnInitEventHandler(this, (UIntPtr)ldftn(CANwin_OnInit)));
				}
				else if (this.CANalyzerMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANalyzer._IMeasurementEvents_Event), "OnInit").AddEventHandler(this.CANalyzerMeasurement, new CANalyzer._IMeasurementEvents_OnInitEventHandler(this, (UIntPtr)ldftn(CANwin_OnInit)));
				}
				this.OnInitInternal += value;
			}
			remove
			{
				if (this.CANoeMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANoe._IMeasurementEvents_Event), "OnInit").RemoveEventHandler(this.CANoeMeasurement, new CANoe._IMeasurementEvents_OnInitEventHandler(this, (UIntPtr)ldftn(CANwin_OnInit)));
				}
				else if (this.CANalyzerMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANalyzer._IMeasurementEvents_Event), "OnInit").RemoveEventHandler(this.CANalyzerMeasurement, new CANalyzer._IMeasurementEvents_OnInitEventHandler(this, (UIntPtr)ldftn(CANwin_OnInit)));
				}
				this.OnInitInternal -= value;
			}
		}

		private event VoidEventHandler OnStartInternal;

		public event VoidEventHandler OnStart
		{
			add
			{
				if (this.CANoeMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANoe._IMeasurementEvents_Event), "OnStart").AddEventHandler(this.CANoeMeasurement, new CANoe._IMeasurementEvents_OnStartEventHandler(this, (UIntPtr)ldftn(CANwin_OnStart)));
				}
				else if (this.CANalyzerMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANalyzer._IMeasurementEvents_Event), "OnStart").AddEventHandler(this.CANalyzerMeasurement, new CANalyzer._IMeasurementEvents_OnStartEventHandler(this, (UIntPtr)ldftn(CANwin_OnStart)));
				}
				this.OnStartInternal += value;
			}
			remove
			{
				if (this.CANoeMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANoe._IMeasurementEvents_Event), "OnStart").RemoveEventHandler(this.CANoeMeasurement, new CANoe._IMeasurementEvents_OnStartEventHandler(this, (UIntPtr)ldftn(CANwin_OnStart)));
				}
				else if (this.CANalyzerMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANalyzer._IMeasurementEvents_Event), "OnStart").RemoveEventHandler(this.CANalyzerMeasurement, new CANalyzer._IMeasurementEvents_OnStartEventHandler(this, (UIntPtr)ldftn(CANwin_OnStart)));
				}
				this.OnStartInternal -= value;
			}
		}

		private event VoidEventHandler OnStopInternal;

		public event VoidEventHandler OnStop
		{
			add
			{
				if (this.CANoeMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANoe._IMeasurementEvents_Event), "OnStop").AddEventHandler(this.CANoeMeasurement, new CANoe._IMeasurementEvents_OnStopEventHandler(this, (UIntPtr)ldftn(CANwin_OnStop)));
				}
				else if (this.CANalyzerMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANalyzer._IMeasurementEvents_Event), "OnStop").AddEventHandler(this.CANalyzerMeasurement, new CANalyzer._IMeasurementEvents_OnStopEventHandler(this, (UIntPtr)ldftn(CANwin_OnStop)));
				}
				this.OnStopInternal += value;
			}
			remove
			{
				if (this.CANoeMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANoe._IMeasurementEvents_Event), "OnStop").RemoveEventHandler(this.CANoeMeasurement, new CANoe._IMeasurementEvents_OnStopEventHandler(this, (UIntPtr)ldftn(CANwin_OnStop)));
				}
				else if (this.CANalyzerMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANalyzer._IMeasurementEvents_Event), "OnStop").RemoveEventHandler(this.CANalyzerMeasurement, new CANalyzer._IMeasurementEvents_OnStopEventHandler(this, (UIntPtr)ldftn(CANwin_OnStop)));
				}
				this.OnStopInternal -= value;
			}
		}

		private event VoidEventHandler OnExitInternal;

		public event VoidEventHandler OnExit
		{
			add
			{
				if (this.CANoeMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANoe._IMeasurementEvents_Event), "OnExit").AddEventHandler(this.CANoeMeasurement, new CANoe._IMeasurementEvents_OnExitEventHandler(this, (UIntPtr)ldftn(CANwin_OnExit)));
				}
				else if (this.CANalyzerMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANalyzer._IMeasurementEvents_Event), "OnExit").AddEventHandler(this.CANalyzerMeasurement, new CANalyzer._IMeasurementEvents_OnExitEventHandler(this, (UIntPtr)ldftn(CANwin_OnExit)));
				}
				this.OnExitInternal += value;
			}
			remove
			{
				if (this.CANoeMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANoe._IMeasurementEvents_Event), "OnExit").RemoveEventHandler(this.CANoeMeasurement, new CANoe._IMeasurementEvents_OnExitEventHandler(this, (UIntPtr)ldftn(CANwin_OnExit)));
				}
				else if (this.CANalyzerMeasurement != null)
				{
					new ComAwareEventInfo(typeof(CANalyzer._IMeasurementEvents_Event), "OnExit").RemoveEventHandler(this.CANalyzerMeasurement, new CANalyzer._IMeasurementEvents_OnExitEventHandler(this, (UIntPtr)ldftn(CANwin_OnExit)));
				}
				this.OnExitInternal -= value;
			}
		}

		private CANoe.Measurement CANoeMeasurement
		{
			get
			{
				if (this.mServerType != CANwinServerType.CANoe)
				{
					return null;
				}
				return base.ComObject as CANoe.Measurement;
			}
		}

		private CANalyzer.Measurement CANalyzerMeasurement
		{
			get
			{
				if (this.mServerType != CANwinServerType.CANalyzer)
				{
					return null;
				}
				return base.ComObject as CANalyzer.Measurement;
			}
		}

		public CANwinMeasurement(CANwinApplication parent) : base(parent)
		{
			this.mServerType = parent.ServerType;
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
				if (CANwinMeasurement.<FinalConstruct>o__SiteContainer0.<>p__Site1 == null)
				{
					CANwinMeasurement.<FinalConstruct>o__SiteContainer0.<>p__Site1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Measurement", typeof(CANwinMeasurement), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				base.ComObject = CANwinMeasurement.<FinalConstruct>o__SiteContainer0.<>p__Site1.Target(CANwinMeasurement.<FinalConstruct>o__SiteContainer0.<>p__Site1, base.Parent.ComObject);
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
			return this.OnInitInternal == null && this.OnStartInternal == null && this.OnStopInternal == null && this.OnExitInternal == null && base.FinalRelease();
		}

		public bool GetRunning(out bool running)
		{
			running = false;
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinMeasurement.<GetRunning>o__SiteContainer2.<>p__Site3 == null)
				{
					CANwinMeasurement.<GetRunning>o__SiteContainer2.<>p__Site3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(bool), typeof(CANwinMeasurement)));
				}
				Func<CallSite, object, bool> arg_A7_0 = CANwinMeasurement.<GetRunning>o__SiteContainer2.<>p__Site3.Target;
				CallSite arg_A7_1 = CANwinMeasurement.<GetRunning>o__SiteContainer2.<>p__Site3;
				if (CANwinMeasurement.<GetRunning>o__SiteContainer2.<>p__Site4 == null)
				{
					CANwinMeasurement.<GetRunning>o__SiteContainer2.<>p__Site4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Running", typeof(CANwinMeasurement), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				running = arg_A7_0(arg_A7_1, CANwinMeasurement.<GetRunning>o__SiteContainer2.<>p__Site4.Target(CANwinMeasurement.<GetRunning>o__SiteContainer2.<>p__Site4, base.ComObject));
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool SetRunning(bool running)
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinMeasurement.<SetRunning>o__SiteContainer5.<>p__Site6 == null)
				{
					CANwinMeasurement.<SetRunning>o__SiteContainer5.<>p__Site6 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Running", typeof(CANwinMeasurement), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				CANwinMeasurement.<SetRunning>o__SiteContainer5.<>p__Site6.Target(CANwinMeasurement.<SetRunning>o__SiteContainer5.<>p__Site6, base.ComObject, running);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool Start()
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinMeasurement.<Start>o__SiteContainer7.<>p__Site8 == null)
				{
					CANwinMeasurement.<Start>o__SiteContainer7.<>p__Site8 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Start", null, typeof(CANwinMeasurement), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				CANwinMeasurement.<Start>o__SiteContainer7.<>p__Site8.Target(CANwinMeasurement.<Start>o__SiteContainer7.<>p__Site8, base.ComObject);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool Stop()
		{
			if (!base.IsInitialized)
			{
				return false;
			}
			bool result;
			try
			{
				if (CANwinMeasurement.<Stop>o__SiteContainer9.<>p__Sitea == null)
				{
					CANwinMeasurement.<Stop>o__SiteContainer9.<>p__Sitea = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Stop", null, typeof(CANwinMeasurement), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				CANwinMeasurement.<Stop>o__SiteContainer9.<>p__Sitea.Target(CANwinMeasurement.<Stop>o__SiteContainer9.<>p__Sitea, base.ComObject);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		private void CANwin_OnInit()
		{
			if (this.OnInitInternal != null)
			{
				this.OnInitInternal();
			}
		}

		private void CANwin_OnStart()
		{
			if (this.OnStartInternal != null)
			{
				this.OnStartInternal();
			}
		}

		private void CANwin_OnStop()
		{
			if (this.OnStopInternal != null)
			{
				this.OnStopInternal();
			}
		}

		private void CANwin_OnExit()
		{
			if (this.OnExitInternal != null)
			{
				this.OnExitInternal();
			}
		}
	}
}
