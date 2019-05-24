using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Vector.VLConfig.CANwinAccess
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
					ResourceManager resourceManager = new ResourceManager("Vector.VLConfig.CANwinAccess.Resources", typeof(Resources).Assembly);
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

		internal static string ErrorIllegalConfigFolder
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorIllegalConfigFolder", Resources.resourceCulture);
			}
		}

		internal static string ErrorIllegalUserDefinedTemplate
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorIllegalUserDefinedTemplate", Resources.resourceCulture);
			}
		}

		internal static string ErrorMeasurementRunningOrModified
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorMeasurementRunningOrModified", Resources.resourceCulture);
			}
		}

		internal static string ErrorMinimumProductVersion
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorMinimumProductVersion", Resources.resourceCulture);
			}
		}

		internal static string ErrorNoApplicationRegistered
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorNoApplicationRegistered", Resources.resourceCulture);
			}
		}

		internal static string ErrorNoOfflineSourceFilesConfigured
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorNoOfflineSourceFilesConfigured", Resources.resourceCulture);
			}
		}

		internal static string ErrorUnableToActivateOfflineMode
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorUnableToActivateOfflineMode", Resources.resourceCulture);
			}
		}

		internal static string ErrorUnableToCloseProcesses
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorUnableToCloseProcesses", Resources.resourceCulture);
			}
		}

		internal static string ErrorUnableToConfigureChannelMapping
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorUnableToConfigureChannelMapping", Resources.resourceCulture);
			}
		}

		internal static string ErrorUnableToConfigureChannels
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorUnableToConfigureChannels", Resources.resourceCulture);
			}
		}

		internal static string ErrorUnableToConfigureDatabases
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorUnableToConfigureDatabases", Resources.resourceCulture);
			}
		}

		internal static string ErrorUnableToConfigureOfflineSource
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorUnableToConfigureOfflineSource", Resources.resourceCulture);
			}
		}

		internal static string ErrorUnableToConfigureSystemVariables
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorUnableToConfigureSystemVariables", Resources.resourceCulture);
			}
		}

		internal static string ErrorUnableToCreateOfflineConfig
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorUnableToCreateOfflineConfig", Resources.resourceCulture);
			}
		}

		internal static string ErrorUnableToLoadTemplate
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorUnableToLoadTemplate", Resources.resourceCulture);
			}
		}

		internal static string ErrorUnableToPrepareConfigFolder
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorUnableToPrepareConfigFolder", Resources.resourceCulture);
			}
		}

		internal static string ErrorUnableToPrepareTemplate
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorUnableToPrepareTemplate", Resources.resourceCulture);
			}
		}

		internal static string ErrorUnableToSaveConfig
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorUnableToSaveConfig", Resources.resourceCulture);
			}
		}

		internal static string ErrorUnableToStartMeasurement
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorUnableToStartMeasurement", Resources.resourceCulture);
			}
		}

		internal static string StatusInfoOfflineConfigCreationRunning
		{
			get
			{
				return Resources.ResourceManager.GetString("StatusInfoOfflineConfigCreationRunning", Resources.resourceCulture);
			}
		}

		internal static string StatusInfoOfflineConfigCreationSucceded
		{
			get
			{
				return Resources.ResourceManager.GetString("StatusInfoOfflineConfigCreationSucceded", Resources.resourceCulture);
			}
		}

		internal static string WarningUnableToActivateViewSync
		{
			get
			{
				return Resources.ResourceManager.GetString("WarningUnableToActivateViewSync", Resources.resourceCulture);
			}
		}

		internal Resources()
		{
		}
	}
}
