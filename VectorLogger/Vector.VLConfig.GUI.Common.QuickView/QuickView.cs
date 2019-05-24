using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.CANwinAccess;
using Vector.VLConfig.CANwinAccess.Data;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GeneralUtil.QuickView;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.QuickView
{
	public class QuickView : ISplitButtonExClient
	{
		private const string MenuSeparatorTag = "QuickView";

		private static readonly List<QuickView> sInstances = new List<QuickView>();

		private readonly IQuickViewClient mClient;

		private readonly IFileConversionParametersClient mFileConversionParametersClient;

		private readonly SplitButtonEx mSplitButtonEx;

		private List<ToolInfo> mToolInfoList;

		private ToolInfo mSelectedTool;

		private OfflineSourceConfig mCurrentOfflineSourceConfig;

		private readonly List<ToolStripItem> mToolStripItems = new List<ToolStripItem>();

		internal static string DefaultTempFolder
		{
			get
			{
				return Path.Combine(Path.GetTempPath(), "VLConfig_QuickView");
			}
		}

		private bool IsVisible
		{
			get
			{
				return this.mClient.LoggerSpecifics.FileConversion.IsQuickViewSupported;
			}
		}

		private GlobalOptions GlobalOptions
		{
			get
			{
				return GlobalOptionsManager.GlobalOptions;
			}
		}

		SplitButton ISplitButtonExClient.SplitButton
		{
			get
			{
				return this.mClient.SplitButtonQuickView;
			}
		}

		string ISplitButtonExClient.SplitButtonEmptyDefault
		{
			get
			{
				return Vector.VLConfig.Properties.Resources.SplitButtonEmptyDefault;
			}
		}

		internal static QuickView Create(IQuickViewClient client, IFileConversionParametersClient fileConversionParametersClient, bool signalOrientedQuickViewSupported)
		{
			if (client == null)
			{
				return null;
			}
			if (client.SplitButtonQuickView == null && client.ContextMenuStripQuickView == null)
			{
				return null;
			}
			QuickView quickView = new QuickView(client, fileConversionParametersClient, signalOrientedQuickViewSupported);
			QuickView.sInstances.Add(quickView);
			return quickView;
		}

		internal static void Release(QuickView quickView)
		{
			if (QuickView.sInstances.Contains(quickView))
			{
				QuickView.sInstances.Remove(quickView);
			}
		}

		private QuickView(IQuickViewClient client, IFileConversionParametersClient fileConversionParametersExClient, bool signalOrientedQuickViewSupported)
		{
			this.mClient = client;
			this.mFileConversionParametersClient = fileConversionParametersExClient;
			this.mToolInfoList = new List<ToolInfo>();
			this.mToolInfoList.Add(new ToolInfoCANoe());
			this.mToolInfoList.Add(new ToolInfoCANalyzer());
			if (signalOrientedQuickViewSupported)
			{
				this.mToolInfoList.Add(new ToolInfoCANape());
				this.mToolInfoList.Add(new ToolInfovSignalyzer());
			}
			if (this.mClient.SplitButtonQuickView != null)
			{
				this.mSplitButtonEx = new SplitButtonEx(this);
				this.InitSplitButtonEx();
			}
			if (this.mClient.ContextMenuStripQuickView != null)
			{
				this.InitContextMenuStrip();
			}
		}

		internal void ApplyLoggerSpecifics()
		{
			if (this.mClient.SplitButtonQuickView != null)
			{
				this.ShowHideSplitButtonEx();
			}
		}

		public static void OnGlobalOptionsChanged()
		{
			foreach (QuickView current in QuickView.sInstances)
			{
				current.ApplyGlobalOptionsToSplitButton();
			}
		}

		private bool CheckSelectedTool()
		{
			if (this.mSelectedTool == null)
			{
				InformMessageBox.Error(Vector.VLConfig.Properties.Resources.ErrorQuickViewNoTool);
				return false;
			}
			if (this.mSelectedTool.CheckPathOfExecutableBeforeStarting && string.IsNullOrEmpty(this.mSelectedTool.PathOfExecutable))
			{
				InformMessageBox.Error(string.Format(Vector.VLConfig.Properties.Resources.ErrorQuickViewCannotStartPath, this.mSelectedTool.Name));
				return false;
			}
			return true;
		}

		private bool TryMoveFileToFolderWithIndexing(string sourceFilePath, string destinationFolder, out string destinationFilePathAbs)
		{
			destinationFilePathAbs = string.Empty;
			string text = Path.GetFileName(sourceFilePath) ?? string.Empty;
			string text2 = Path.Combine(destinationFolder, text);
			int num = 2;
			while (File.Exists(text2))
			{
				text2 = Path.Combine(destinationFolder, string.Format("{0}_({1}){2}", Path.GetFileNameWithoutExtension(text), num, Path.GetExtension(text)));
				num++;
			}
			try
			{
				destinationFilePathAbs = Path.Combine(destinationFolder, text2);
				File.Move(sourceFilePath, destinationFilePathAbs);
			}
			catch (Exception)
			{
				InformMessageBox.Error(string.Format(Vector.VLConfig.Properties.Resources.ErrorCannotMoveFileTo, sourceFilePath, destinationFolder));
				return false;
			}
			return true;
		}

		private void InitSplitButtonEx()
		{
			this.mClient.SplitButtonQuickView.AutoSize = false;
			foreach (ToolInfo current in this.mToolInfoList)
			{
				ToolStripItem toolStripItem = this.mSplitButtonEx.AddItem(current.Name, current.IconImage, "");
				toolStripItem.Tag = current.QuickViewTool;
			}
		}

		private void ApplyGlobalOptionsToSplitButton()
		{
			this.mSelectedTool = null;
			if (this.mClient.SplitButtonQuickView == null)
			{
				return;
			}
			foreach (ToolInfo current in this.mToolInfoList)
			{
				if (this.GlobalOptions.QuickViewTool == current.QuickViewTool)
				{
					this.mSelectedTool = current;
					this.mSplitButtonEx.DefaultAction = current.Name;
					return;
				}
			}
			this.mSplitButtonEx.DefaultAction = ((ISplitButtonExClient)this).SplitButtonEmptyDefault;
		}

		private void ShowHideSplitButtonEx()
		{
			this.mClient.SplitButtonQuickView.Visible = this.IsVisible;
			foreach (Control current in this.mClient.OtherFeatureControls)
			{
				current.Visible = this.IsVisible;
			}
		}

		bool ISplitButtonExClient.IsItemVisible(ToolStripItem item)
		{
			return true;
		}

		void ISplitButtonExClient.ItemClicked(ToolStripItem item)
		{
			this.GlobalOptions.QuickViewTool = QuickViewTool.Unspecified;
			this.mSelectedTool = null;
			QuickViewTool quickViewTool = (QuickViewTool)item.Tag;
			foreach (ToolInfo current in this.mToolInfoList)
			{
				if (quickViewTool == current.QuickViewTool)
				{
					this.mSelectedTool = current;
					this.GlobalOptions.QuickViewTool = quickViewTool;
					break;
				}
			}
			this.mCurrentOfflineSourceConfig = this.mClient.GetOfflineSourceConfig(false, this.GlobalOptions.QuickViewTool == QuickViewTool.CANoe || this.GlobalOptions.QuickViewTool == QuickViewTool.CANalyzer);
			QuickView.OnGlobalOptionsChanged();
			this.CreateOfflineConfig();
		}

		void ISplitButtonExClient.DefaultActionClicked()
		{
			foreach (ToolInfo current in this.mToolInfoList)
			{
				if (this.GlobalOptions.QuickViewTool == current.QuickViewTool)
				{
					this.mSelectedTool = current;
					break;
				}
			}
			this.mCurrentOfflineSourceConfig = this.mClient.GetOfflineSourceConfig(false, this.GlobalOptions.QuickViewTool == QuickViewTool.CANoe || this.GlobalOptions.QuickViewTool == QuickViewTool.CANalyzer);
			this.CreateOfflineConfig();
		}

		private void InitContextMenuStrip()
		{
			ContextMenuStrip contextMenuStripQuickView = this.mClient.ContextMenuStripQuickView;
			contextMenuStripQuickView.Opening += new CancelEventHandler(this.ContextMenu_Opening);
			ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
			toolStripSeparator.Tag = "QuickView";
			this.mToolStripItems.Add(toolStripSeparator);
			contextMenuStripQuickView.Items.Add(toolStripSeparator);
			foreach (ToolInfo current in this.mToolInfoList)
			{
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(string.Format(Vector.VLConfig.Properties.Resources.QuickViewWith, current.Name), current.IconImage);
				toolStripMenuItem.Tag = current.QuickViewTool;
				toolStripMenuItem.Click += new EventHandler(this.ContextMenuItem_Click);
				this.mToolStripItems.Add(toolStripMenuItem);
				contextMenuStripQuickView.Items.Add(toolStripMenuItem);
			}
		}

		private void ShowHideContextMenuItems(bool enableItems)
		{
			if (this.mClient.ContextMenuStripQuickView == null)
			{
				return;
			}
			using (List<ToolInfo>.Enumerator enumerator = this.mToolInfoList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ToolInfo toolInfo = enumerator.Current;
					if (this.GlobalOptions.QuickViewTool == toolInfo.QuickViewTool)
					{
						this.ShowMenuItemsExclusively((from item in this.mToolStripItems
						where toolInfo.QuickViewTool.Equals(item.Tag)
						select item).ToList<ToolStripItem>(), enableItems);
						return;
					}
				}
			}
			this.ShowMenuItemsExclusively(this.mToolStripItems, enableItems);
		}

		private void ShowMenuItemsExclusively(IList<ToolStripItem> itemsToShow, bool enableItems)
		{
			foreach (ToolStripItem current in this.mToolStripItems)
			{
				ToolStripMenuItem toolStripMenuItem = current as ToolStripMenuItem;
				ToolStripSeparator toolStripSeparator = current as ToolStripSeparator;
				if (toolStripMenuItem != null)
				{
					toolStripMenuItem.Visible = (itemsToShow.Contains(toolStripMenuItem) && this.IsVisible);
					toolStripMenuItem.Enabled = enableItems;
				}
				else if (toolStripSeparator != null)
				{
					toolStripSeparator.Visible = this.IsVisible;
				}
			}
		}

		private void ContextMenu_Opening(object sender, CancelEventArgs e)
		{
			this.mCurrentOfflineSourceConfig = this.mClient.GetOfflineSourceConfig(true, this.GlobalOptions.QuickViewTool == QuickViewTool.CANoe || this.GlobalOptions.QuickViewTool == QuickViewTool.CANalyzer);
			this.ShowHideContextMenuItems(this.mCurrentOfflineSourceConfig.OfflineSourceFiles.Any<Tuple<string, string>>());
		}

		private void ContextMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripItem toolStripItem = sender as ToolStripItem;
			if (toolStripItem != null && toolStripItem.Tag is QuickViewTool)
			{
				this.GlobalOptions.QuickViewTool = QuickViewTool.Unspecified;
				this.mSelectedTool = null;
				QuickViewTool quickViewTool = (QuickViewTool)toolStripItem.Tag;
				foreach (ToolInfo current in this.mToolInfoList)
				{
					if (quickViewTool == current.QuickViewTool)
					{
						this.mSelectedTool = current;
						this.GlobalOptions.QuickViewTool = quickViewTool;
						break;
					}
				}
				QuickView.OnGlobalOptionsChanged();
				this.CreateOfflineConfig();
			}
		}

		private void CreateOfflineConfig()
		{
			if (!this.CheckSelectedTool())
			{
				return;
			}
			if (this.mSelectedTool.QuickViewTool == QuickViewTool.CANoe || this.mSelectedTool.QuickViewTool == QuickViewTool.CANalyzer)
			{
				string tempDirectoryName;
				if (!TempDirectoryManager.Instance.CreateNewTempDirectory(out tempDirectoryName))
				{
					return;
				}
				DirectoryInfo tempDir = new DirectoryInfo(TempDirectoryManager.Instance.GetFullTempDirectoryPath(tempDirectoryName));
				CANwinQuickViewData configData = this.CreateCANwinQuickViewData(tempDir);
				using (new WaitCursor())
				{
					CANwinAutomation.Instance.CreateOfflineConfig(configData);
					return;
				}
			}
			if (this.mSelectedTool.QuickViewTool == QuickViewTool.CANape || this.mSelectedTool.QuickViewTool == QuickViewTool.vSignalyzer)
			{
				this.CANapevSignalyzerQuickView();
			}
		}

		private CANwinQuickViewData CreateCANwinQuickViewData(DirectoryInfo tempDir)
		{
			if (this.mSelectedTool == null)
			{
				return null;
			}
			CANwinQuickViewData cANwinQuickViewData = new CANwinQuickViewData();
			if (this.mSelectedTool.QuickViewTool == QuickViewTool.CANalyzer)
			{
				cANwinQuickViewData.ServerType = CANwinServerType.CANalyzer;
			}
			else
			{
				cANwinQuickViewData.ServerType = CANwinServerType.CANoe;
			}
			cANwinQuickViewData.BusConfiguration.AddRange(this.GetBusConfiguration());
			cANwinQuickViewData.SysvarFiles.AddRange(this.GetSysvarsFiles(tempDir));
			cANwinQuickViewData.OfflineSourceConfig.CopyFrom(this.mCurrentOfflineSourceConfig);
			this.GetGlobalOptions(cANwinQuickViewData);
			return cANwinQuickViewData;
		}

		private IEnumerable<CANwinBusItem> GetBusConfiguration()
		{
			List<CANwinBusItem> list = new List<CANwinBusItem>();
			uint num = this.mClient.LoggerSpecifics.CAN.NumberOfChannels;
			if (this.mClient.LoggerSpecifics.CAN.NumberOfVirtualChannels > 0u)
			{
				num += 1u;
			}
			for (uint num2 = 1u; num2 <= num; num2 += 1u)
			{
				list.Add(new CANwinBusItem(BusType.Bt_CAN, (int)num2));
			}
			uint numberOfChannels = this.mClient.LoggerSpecifics.LIN.NumberOfChannels;
			for (uint num3 = 1u; num3 <= numberOfChannels; num3 += 1u)
			{
				list.Add(new CANwinBusItem(BusType.Bt_LIN, (int)num3));
			}
			if (this.mClient.LoggerSpecifics.Recording.IsMOST150Supported)
			{
				list.Add(new CANwinBusItem(BusType.Bt_MOST, 1));
			}
			uint numberOfChannels2 = this.mClient.LoggerSpecifics.Flexray.NumberOfChannels;
			for (uint num4 = 1u; num4 <= numberOfChannels2; num4 += 1u)
			{
				list.Add(new CANwinBusItem(BusType.Bt_FlexRay, (int)num4));
			}
			IEnumerable<Database> conversionDatabases = this.mClient.GetConversionDatabases();
			using (IEnumerator<Database> enumerator = conversionDatabases.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Database database = enumerator.Current;
					CANwinBusItem cANwinBusItem = list.FirstOrDefault((CANwinBusItem bi) => bi.BusType == CANwinBusItem.Convert(database.BusType.Value) && (long)bi.ChannelNumber == (long)((ulong)database.ChannelNumber.Value));
					if (cANwinBusItem != null)
					{
						cANwinBusItem.AbsoluteDatabaseFilePaths.Add(this.mClient.ConfigurationManagerService.GetAbsoluteFilePath(database.FilePath.Value));
					}
				}
			}
			return list;
		}

		private IEnumerable<string> GetSysvarsFiles(DirectoryInfo tempDir)
		{
			if (!string.IsNullOrEmpty(this.mClient.LogDataIniFile2))
			{
				GenerationUtil.GenerateVSysVarFileFromIniFile(tempDir.FullName, this.mClient.LogDataIniFile2);
				FileInfo[] files = tempDir.GetFiles("*.vsysvar");
				return (from sf in files
				select sf.FullName).ToList<string>();
			}
			if (GenerationUtil.AnalysisFileCollector == null)
			{
				AnalysisFileCollector.Create();
			}
			string text;
			if (GenerationUtil.GenerateVSysVarFileFromConfig(tempDir.FullName, out text) == Result.OK)
			{
				FileInfo[] files2 = tempDir.GetFiles("*.vsysvar");
				return (from sf in files2
				select sf.FullName).ToList<string>();
			}
			return new List<string>();
		}

		private void GetGlobalOptions(CANwinQuickViewData data)
		{
			data.BaseConfigFolder = this.GetBaseConfigFolder();
			if (this.mSelectedTool.QuickViewTool == QuickViewTool.CANoe)
			{
				data.UseUserDefinedTemplate = this.GlobalOptions.UseUserDefinedTemplateCANoe;
				data.UserDefinedTemplate = this.GlobalOptions.UserDefinedTemplateCANoe;
			}
			else
			{
				data.UseUserDefinedTemplate = this.GlobalOptions.UseUserDefinedTemplateCANalyzer;
				data.UserDefinedTemplate = this.GlobalOptions.UserDefinedTemplateCANalyzer;
			}
			data.OfflineSourceConfig.PlaybackDuration = this.GlobalOptions.MaxPlaybackDuration;
		}

		private string GetBaseConfigFolder()
		{
			if (!string.IsNullOrEmpty(this.GlobalOptions.GenerationFolder))
			{
				return this.GlobalOptions.GenerationFolder;
			}
			return QuickView.DefaultTempFolder;
		}

		private void CANapevSignalyzerQuickView()
		{
			bool flag = false;
			FileConversionParameters fileConversionParameters = null;
			string text = string.Empty;
			using (new WaitCursor())
			{
				if (this.mFileConversionParametersClient == null || this.mClient.CurrentDevice == null || !this.mCurrentOfflineSourceConfig.OfflineSourceFiles.Any<Tuple<string, string>>())
				{
					InformMessageBox.Error(Vector.VLConfig.Properties.Resources.ErrorQuickViewExcerptFailed);
					return;
				}
				if (!this.mClient.ConfigurationManagerService.DatabaseServices.HasDatabasesAccessibleForSignalConversion(this.mFileConversionParametersClient.FileConversionParameters, true))
				{
					InformMessageBox.Error(string.Format(Vector.VLConfig.Properties.Resources.ErrorQuickViewNoDatabases, this.mSelectedTool.Name));
					return;
				}
				if (this.mSelectedTool.QuickViewTool == QuickViewTool.CANape)
				{
					flag = this.GlobalOptions.UseUserDefinedTemplateCANape;
					text = this.GlobalOptions.UserDefinedTemplateCANape;
				}
				else if (this.mSelectedTool.QuickViewTool == QuickViewTool.vSignalyzer)
				{
					flag = this.GlobalOptions.UseUserDefinedTemplatevSignalyzer;
					text = this.GlobalOptions.UserDefinedTemplatevSignalyzer;
				}
				if (flag && !File.Exists(text))
				{
					InformMessageBox.Error(Vector.VLConfig.Properties.Resources.ErrorQuickViewProjectNotFound);
					return;
				}
				fileConversionParameters = new FileConversionParameters(this.mFileConversionParametersClient.FileConversionParameters);
				bool flag2 = fileConversionParameters.DestinationFormat != FileConversionDestFormat.MDF || FileConversionHelper.UseMDFLegacyConversion(fileConversionParameters) || FileConversionHelper.UseMDFCompatibilityMode(fileConversionParameters);
				if (flag2)
				{
					FileConversionHelper.SetMDFDefaultVersion(fileConversionParameters);
				}
				fileConversionParameters.SaveRawFile = false;
				fileConversionParameters.GlobalTimestamps = false;
			}
			if (FileConversionHelper.PerformChecksForSignalOrientedDestFormat(FileConversionDestFormat.MDF, fileConversionParameters, this.mClient.PropertyWindow))
			{
				using (new WaitCursor())
				{
					List<ConversionJob> list = new List<ConversionJob>();
					ConversionJobType type;
					switch (this.mCurrentOfflineSourceConfig.SelectionType)
					{
					case SelectionType.Measurement:
						type = ConversionJobType.Measurement;
						break;
					case SelectionType.Marker:
						type = ConversionJobType.Marker;
						break;
					case SelectionType.File:
						type = ConversionJobType.File;
						break;
					case SelectionType.Trigger:
						type = ConversionJobType.Trigger;
						break;
					default:
						type = ConversionJobType.Measurement;
						break;
					}
					this.mCurrentOfflineSourceConfig.PlaybackDuration = this.GlobalOptions.MaxPlaybackDuration;
					ConversionJob conversionJob = new ConversionJob(this.mCurrentOfflineSourceConfig.MeasurementName, type, (uint)this.mCurrentOfflineSourceConfig.MeasurementNumber, this.mCurrentOfflineSourceConfig.LoggerMemNumber);
					list.Add(conversionJob);
					string item = this.mCurrentOfflineSourceConfig.OfflineSourceFiles[0].Item1;
					if (this.mClient.CurrentDevice.LogFileStorage.IsPrimaryFileGroupCompressed)
					{
						conversionJob.SelectedFileNames.Add(item + Vector.VLConfig.GeneralUtil.Vocabulary.FileExtensionDotGZ);
					}
					else
					{
						conversionJob.SelectedFileNames.Add(item);
					}
					DateTime dateTime = this.mCurrentOfflineSourceConfig.StartDateTime.Add(new TimeSpan(0, (int)this.mCurrentOfflineSourceConfig.PlaybackDuration, 1));
					if (this.mCurrentOfflineSourceConfig.EndDateTime > dateTime)
					{
						this.mCurrentOfflineSourceConfig.SetTimeSection(this.mCurrentOfflineSourceConfig.StartDateTime, dateTime);
					}
					conversionJob.AddBeginEndForLogFile(this.mCurrentOfflineSourceConfig.OfflineSourceFiles[0].Item1, this.mCurrentOfflineSourceConfig.StartDateTime, this.mCurrentOfflineSourceConfig.EndDateTime);
					conversionJob.StartTime = this.mCurrentOfflineSourceConfig.StartDateTime;
					conversionJob.ExtractStart = this.mCurrentOfflineSourceConfig.StartDateTime;
					conversionJob.ExtractT1 = 0;
					conversionJob.ExtractT2 = (int)(this.mCurrentOfflineSourceConfig.PlaybackDuration * 60u + 1u);
					conversionJob.FileConversionParameters = fileConversionParameters;
					string tempDirectoryName;
					if (!TempDirectoryManager.Instance.CreateNewTempDirectory(out tempDirectoryName))
					{
						return;
					}
					DirectoryInfo directoryInfo = new DirectoryInfo(TempDirectoryManager.Instance.GetFullTempDirectoryPath(tempDirectoryName));
					fileConversionParameters.DestinationFolder = directoryInfo.FullName;
					this.mClient.CurrentDevice.LogFileStorage.ConvertSelectedLogFiles(fileConversionParameters, list, this.mClient.DatabaseConfiguration, this.mClient.ConfigurationFolderPath);
					FileInfoProxy fileInfoProxy = null;
					DateTime t = default(DateTime);
					List<string> list2;
					if (FileSystemServices.HasSubFolders(fileConversionParameters.DestinationFolder, "*", out list2))
					{
						foreach (string current in list2)
						{
							IEnumerable<FileInfoProxy> files = FileSystemServices.GetFiles(current);
							foreach (FileInfoProxy current2 in files)
							{
								if (!string.IsNullOrEmpty(current2.Extension) && t < current2.LastWriteTime && (string.Compare(current2.Extension, Vector.VLConfig.GeneralUtil.Vocabulary.FileExtensionDotMDF, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(current2.Extension, Vector.VLConfig.GeneralUtil.Vocabulary.FileExtensionDotMF4, StringComparison.OrdinalIgnoreCase) == 0))
								{
									if (!current2.Attributes.HasFlag(FileAttributes.ReadOnly))
									{
										t = current2.LastWriteTime;
									}
									fileInfoProxy = current2;
								}
							}
						}
					}
					if (fileInfoProxy == null)
					{
						InformMessageBox.Error(Vector.VLConfig.Properties.Resources.ErrorQuickViewExcerptFailed);
						return;
					}
					string text2 = Path.Combine(this.GetBaseConfigFolder(), this.mSelectedTool.QuickViewTool.ToString());
					if (Directory.Exists(text2))
					{
						IEnumerable<string> files2 = Directory.GetFiles(text2, "*");
						using (IEnumerator<string> enumerator3 = files2.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								string current3 = enumerator3.Current;
								FileSystemServices.TryDeleteFile(current3);
							}
							goto IL_56B;
						}
					}
					FileSystemServices.TryCreateDirectory(text2);
					IL_56B:
					string text3;
					if (!this.TryMoveFileToFolderWithIndexing(fileInfoProxy.FullName, text2, out text3))
					{
						return;
					}
					StringBuilder stringBuilder = new StringBuilder();
					if (this.mSelectedTool.QuickViewTool == QuickViewTool.CANape)
					{
						stringBuilder.Append("-g ");
					}
					if (flag)
					{
						stringBuilder.Append("\"");
						stringBuilder.Append(text);
						stringBuilder.Append("\" ");
						stringBuilder.Append("-m ");
					}
					string text4 = text3;
					stringBuilder.Append("\"");
					stringBuilder.Append(text4);
					stringBuilder.Append("\"");
					ProcessStartInfo processStartInfo = new ProcessStartInfo(this.mSelectedTool.PathOfExecutable, stringBuilder.ToString());
					processStartInfo.WorkingDirectory = (Path.GetDirectoryName(text4) ?? string.Empty);
					try
					{
						Process.Start(processStartInfo);
					}
					catch (Exception)
					{
						InformMessageBox.Error(string.Format(Vector.VLConfig.Properties.Resources.ErrorQuickViewCannotStart, this.mSelectedTool.Name));
					}
				}
				return;
			}
		}
	}
}
