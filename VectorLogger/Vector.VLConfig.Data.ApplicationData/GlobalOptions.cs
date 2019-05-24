using System;
using System.Collections.Generic;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.Data.ApplicationData
{
	public class GlobalOptions : IApplicationDataSettings
	{
		public enum Languages
		{
			German,
			English
		}

		public List<string> AdditionalDrivesList;

		public List<string> RecentSourceFolders;

		public List<string> RecentSourceZips;

		public List<string> RecentDestinationFolders;

		public List<string> RecentClfSourceFolders;

		public List<string> RecentClfExportFolders;

		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "GlobalOptions";
			}
		}

		public DisplayMode DisplayMode
		{
			get;
			set;
		}

		public int MainWindowLeft
		{
			get;
			set;
		}

		public int MainWindowTop
		{
			get;
			set;
		}

		public int MainWindowHeight
		{
			get;
			set;
		}

		public int MainWindowWidth
		{
			get;
			set;
		}

		public GlobalOptions.Languages Language
		{
			get;
			set;
		}

		public LoggerType LoggerTypeToStartWith
		{
			get;
			set;
		}

		public bool CaplComplianceTest
		{
			get;
			set;
		}

		public string SelectedCOMPort
		{
			get;
			set;
		}

		public bool IsAutoEjectEnabled
		{
			get;
			set;
		}

		public long MemoryCardSize
		{
			get;
			set;
		}

		public bool EnableSignalListFilterFeature
		{
			get;
			set;
		}

		public bool GenerateCANapeDBC
		{
			get;
			set;
		}

		public uint VN1630logMaxFileSizeMB
		{
			get;
			set;
		}

		public bool SortMDF3Files
		{
			get;
			set;
		}

		public int MDFCompressionLevel
		{
			get;
			set;
		}

		public bool HDF5WriteUnlimitedSignalNames
		{
			get;
			set;
		}

		public int HDF5CompressionLevel
		{
			get;
			set;
		}

		public string RecentFirmwareFolder
		{
			get;
			set;
		}

		public string GenerationFolder
		{
			get;
			set;
		}

		public bool UseUserDefinedTemplateCANoe
		{
			get;
			set;
		}

		public string UserDefinedTemplateCANoe
		{
			get;
			set;
		}

		public bool UseUserDefinedTemplateCANalyzer
		{
			get;
			set;
		}

		public string UserDefinedTemplateCANalyzer
		{
			get;
			set;
		}

		public bool UseUserDefinedTemplateCANape
		{
			get;
			set;
		}

		public string UserDefinedTemplateCANape
		{
			get;
			set;
		}

		public bool UseUserDefinedTemplatevSignalyzer
		{
			get;
			set;
		}

		public string UserDefinedTemplatevSignalyzer
		{
			get;
			set;
		}

		public uint MaxPlaybackDuration
		{
			get;
			set;
		}

		public QuickViewTool QuickViewTool
		{
			get;
			set;
		}

		public bool KeepTempFoldersFromCodeGeneration
		{
			get;
			set;
		}

		public GlobalOptions()
		{
			this.DisplayMode = new DisplayMode();
			this.MainWindowLeft = 0;
			this.MainWindowTop = 0;
			this.MainWindowHeight = 0;
			this.MainWindowWidth = 0;
			this.Language = GlobalOptions.Languages.English;
			this.LoggerTypeToStartWith = LoggerType.GL3000;
			this.CaplComplianceTest = true;
			this.AdditionalDrivesList = new List<string>();
			this.RecentSourceFolders = new List<string>();
			this.RecentSourceZips = new List<string>();
			this.RecentDestinationFolders = new List<string>();
			this.RecentClfSourceFolders = new List<string>();
			this.RecentClfExportFolders = new List<string>();
			this.SelectedCOMPort = string.Empty;
			this.IsAutoEjectEnabled = true;
			this.EnableSignalListFilterFeature = false;
			this.GenerateCANapeDBC = false;
			this.MemoryCardSize = (long)((ulong)Constants.DefaultMemoryCardSize);
			this.KeepTempFoldersFromCodeGeneration = false;
			this.VN1630logMaxFileSizeMB = 1024u;
			this.SortMDF3Files = false;
			this.MDFCompressionLevel = 0;
			this.HDF5WriteUnlimitedSignalNames = false;
			this.HDF5CompressionLevel = 0;
			this.RecentFirmwareFolder = FileSystemServices.GetApplicationPath();
			this.GenerationFolder = string.Empty;
			this.UseUserDefinedTemplateCANoe = false;
			this.UserDefinedTemplateCANoe = string.Empty;
			this.UseUserDefinedTemplateCANalyzer = false;
			this.UserDefinedTemplateCANalyzer = string.Empty;
			this.UseUserDefinedTemplateCANape = false;
			this.UserDefinedTemplateCANape = string.Empty;
			this.UseUserDefinedTemplatevSignalyzer = false;
			this.UserDefinedTemplatevSignalyzer = string.Empty;
			this.MaxPlaybackDuration = 15u;
			this.QuickViewTool = QuickViewTool.Unspecified;
		}
	}
}
