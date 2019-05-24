using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Vector.VLConfig.LoggingNavigator.Properties
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
					ResourceManager resourceManager = new ResourceManager("Vector.VLConfig.LoggingNavigator.Properties.Resources", typeof(Resources).Assembly);
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

		internal static string Duration
		{
			get
			{
				return Resources.ResourceManager.GetString("Duration", Resources.resourceCulture);
			}
		}

		internal static string Error
		{
			get
			{
				return Resources.ResourceManager.GetString("Error", Resources.resourceCulture);
			}
		}

		internal static string ErrorFilesCorrupt
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorFilesCorrupt", Resources.resourceCulture);
			}
		}

		internal static string Files
		{
			get
			{
				return Resources.ResourceManager.GetString("Files", Resources.resourceCulture);
			}
		}

		internal static Icon IconInfo
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("IconInfo", Resources.resourceCulture);
				return (Icon)@object;
			}
		}

		internal static string LogTableNoSourceSelected
		{
			get
			{
				return Resources.ResourceManager.GetString("LogTableNoSourceSelected", Resources.resourceCulture);
			}
		}

		internal static string LogTableSelectionEmptyLogFiles
		{
			get
			{
				return Resources.ResourceManager.GetString("LogTableSelectionEmptyLogFiles", Resources.resourceCulture);
			}
		}

		internal static string LogTableSelectionEmptyMarker
		{
			get
			{
				return Resources.ResourceManager.GetString("LogTableSelectionEmptyMarker", Resources.resourceCulture);
			}
		}

		internal static string LogTableSelectionEmptyMarkerChangeFilter
		{
			get
			{
				return Resources.ResourceManager.GetString("LogTableSelectionEmptyMarkerChangeFilter", Resources.resourceCulture);
			}
		}

		internal static string LogTableSelectionEmptyMeasurements
		{
			get
			{
				return Resources.ResourceManager.GetString("LogTableSelectionEmptyMeasurements", Resources.resourceCulture);
			}
		}

		internal static string LogTableSelectionEmptyTrigger
		{
			get
			{
				return Resources.ResourceManager.GetString("LogTableSelectionEmptyTrigger", Resources.resourceCulture);
			}
		}

		internal static string LogTableSelectionEmptyTriggerChangeFilter
		{
			get
			{
				return Resources.ResourceManager.GetString("LogTableSelectionEmptyTriggerChangeFilter", Resources.resourceCulture);
			}
		}

		internal static string Marker
		{
			get
			{
				return Resources.ResourceManager.GetString("Marker", Resources.resourceCulture);
			}
		}

		internal static string MarkerOffsetNaN
		{
			get
			{
				return Resources.ResourceManager.GetString("MarkerOffsetNaN", Resources.resourceCulture);
			}
		}

		internal static string MarkerOffsetTooLarge
		{
			get
			{
				return Resources.ResourceManager.GetString("MarkerOffsetTooLarge", Resources.resourceCulture);
			}
		}

		internal static string MeasurementRecovered
		{
			get
			{
				return Resources.ResourceManager.GetString("MeasurementRecovered", Resources.resourceCulture);
			}
		}

		internal static string Measurements
		{
			get
			{
				return Resources.ResourceManager.GetString("Measurements", Resources.resourceCulture);
			}
		}

		internal static string NoSourceSelected
		{
			get
			{
				return Resources.ResourceManager.GetString("NoSourceSelected", Resources.resourceCulture);
			}
		}

		internal static Icon speaker_error
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("speaker_error", Resources.resourceCulture);
				return (Icon)@object;
			}
		}

		internal static Icon speaker_play
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("speaker_play", Resources.resourceCulture);
				return (Icon)@object;
			}
		}

		internal static Icon speaker_stop
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("speaker_stop", Resources.resourceCulture);
				return (Icon)@object;
			}
		}

		internal static string Trigger
		{
			get
			{
				return Resources.ResourceManager.GetString("Trigger", Resources.resourceCulture);
			}
		}

		internal static string VoCanAudioRecording
		{
			get
			{
				return Resources.ResourceManager.GetString("VoCanAudioRecording", Resources.resourceCulture);
			}
		}

		internal Resources()
		{
		}
	}
}
