using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Vector.VLConfig.LTLGenerationCS.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Vector.VLConfig.LTLGenerationCS.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		internal static string EmptyString
		{
			get
			{
				return Resources.ResourceManager.GetString("EmptyString", Resources.resourceCulture);
			}
		}

		internal static string LTLError_ActionsDigitalOutputError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_ActionsDigitalOutputError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_ActionsError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_ActionsError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_ActionsSendMessage_MessageResolve
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_ActionsSendMessage_MessageResolve", Resources.resourceCulture);
			}
		}

		internal static string LTLError_ActionsSendMessageError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_ActionsSendMessageError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_AnalogInputsError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_AnalogInputsError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_CANchannelError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_CANchannelError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_CCPXCPError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_CCPXCPError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_CompileError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_CompileError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_DiagError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_DiagError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_DiagError_UnknownAddressingMode
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_DiagError_UnknownAddressingMode", Resources.resourceCulture);
			}
		}

		internal static string LTLError_DiagError_UnsupportedEvent
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_DiagError_UnsupportedEvent", Resources.resourceCulture);
			}
		}

		internal static string LTLError_DigitalInputError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_DigitalInputError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_EventInWrongContext
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_EventInWrongContext", Resources.resourceCulture);
			}
		}

		internal static string LTLError_FilterError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_FilterError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_FilterError_MessageResolve
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_FilterError_MessageResolve", Resources.resourceCulture);
			}
		}

		internal static string LTLError_FilterError_SigListReadFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_FilterError_SigListReadFailed", Resources.resourceCulture);
			}
		}

		internal static string LTLError_FilterError_SigListResolve
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_FilterError_SigListResolve", Resources.resourceCulture);
			}
		}

		internal static string LTLError_FlexRayChannelError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_FlexRayChannelError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_GeneralError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_GeneralError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_GPSerror
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_GPSerror", Resources.resourceCulture);
			}
		}

		internal static string LTLError_IncludeFileError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_IncludeFileError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_LEDError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_LEDError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_LINchannelError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_LINchannelError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_LogDateTimeError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_LogDateTimeError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_LTLFileWriteError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_LTLFileWriteError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_SignalExportListError_SignalResolve
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_SignalExportListError_SignalResolve", Resources.resourceCulture);
			}
		}

		internal static string LTLError_StopCyclicEventsError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_StopCyclicEventsError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_TriggerError
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_TriggerError", Resources.resourceCulture);
			}
		}

		internal static string LTLError_TriggerError_Bitlength
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_TriggerError_Bitlength", Resources.resourceCulture);
			}
		}

		internal static string LTLError_TriggerError_FlexRayMultiplexedSignal
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_TriggerError_FlexRayMultiplexedSignal", Resources.resourceCulture);
			}
		}

		internal static string LTLError_TriggerError_MessageResolve
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_TriggerError_MessageResolve", Resources.resourceCulture);
			}
		}

		internal static string LTLError_TriggerError_SignalResolve
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_TriggerError_SignalResolve", Resources.resourceCulture);
			}
		}

		internal static string LTLError_TriggerError_TriggerNameResolve
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_TriggerError_TriggerNameResolve", Resources.resourceCulture);
			}
		}

		internal static string LTLError_TriggerError_UnsupportedCondition
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_TriggerError_UnsupportedCondition", Resources.resourceCulture);
			}
		}

		internal static string LTLError_TriggerError_UnsupportedEvent
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_TriggerError_UnsupportedEvent", Resources.resourceCulture);
			}
		}

		internal static string LTLError_UnknownOperatingMode
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_UnknownOperatingMode", Resources.resourceCulture);
			}
		}

		internal static string LTLError_VoCANMultipleConfigured
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_VoCANMultipleConfigured", Resources.resourceCulture);
			}
		}

		internal static string LTLError_WLANSettingsInvalid
		{
			get
			{
				return Resources.ResourceManager.GetString("LTLError_WLANSettingsInvalid", Resources.resourceCulture);
			}
		}

		internal Resources()
		{
		}
	}
}
