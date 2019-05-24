using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Vector.VLConfig.GeneralUtil
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	public class Resources_General
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources_General.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Vector.VLConfig.GeneralUtil.Resources_General", typeof(Resources_General).Assembly);
					Resources_General.resourceMan = resourceManager;
				}
				return Resources_General.resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static CultureInfo Culture
		{
			get
			{
				return Resources_General.resourceCulture;
			}
			set
			{
				Resources_General.resourceCulture = value;
			}
		}

		public static string CopyFilesFinished
		{
			get
			{
				return Resources_General.ResourceManager.GetString("CopyFilesFinished", Resources_General.resourceCulture);
			}
		}

		public static string CopyFilesStatusText
		{
			get
			{
				return Resources_General.ResourceManager.GetString("CopyFilesStatusText", Resources_General.resourceCulture);
			}
		}

		public static string Error
		{
			get
			{
				return Resources_General.ResourceManager.GetString("Error", Resources_General.resourceCulture);
			}
		}

		public static string GenericBackgroundWorkerCanceledByUser
		{
			get
			{
				return Resources_General.ResourceManager.GetString("GenericBackgroundWorkerCanceledByUser", Resources_General.resourceCulture);
			}
		}

		public static string GenericBackgroundWorkerResultDestinationFileAlreadyExists
		{
			get
			{
				return Resources_General.ResourceManager.GetString("GenericBackgroundWorkerResultDestinationFileAlreadyExists", Resources_General.resourceCulture);
			}
		}

		public static string GenericBackgroundWorkerStatusText
		{
			get
			{
				return Resources_General.ResourceManager.GetString("GenericBackgroundWorkerStatusText", Resources_General.resourceCulture);
			}
		}

		public static string GenericBackgroundWorkerWaitingForBackgroundTasksToFinish
		{
			get
			{
				return Resources_General.ResourceManager.GetString("GenericBackgroundWorkerWaitingForBackgroundTasksToFinish", Resources_General.resourceCulture);
			}
		}

		public static string GenericFileTypeName
		{
			get
			{
				return Resources_General.ResourceManager.GetString("GenericFileTypeName", Resources_General.resourceCulture);
			}
		}

		public static string InternalError
		{
			get
			{
				return Resources_General.ResourceManager.GetString("InternalError", Resources_General.resourceCulture);
			}
		}

		public static string QuestionContinueAnyway
		{
			get
			{
				return Resources_General.ResourceManager.GetString("QuestionContinueAnyway", Resources_General.resourceCulture);
			}
		}

		public static string Unknown
		{
			get
			{
				return Resources_General.ResourceManager.GetString("Unknown", Resources_General.resourceCulture);
			}
		}

		public static string Warning
		{
			get
			{
				return Resources_General.ResourceManager.GetString("Warning", Resources_General.resourceCulture);
			}
		}

		internal Resources_General()
		{
		}
	}
}
