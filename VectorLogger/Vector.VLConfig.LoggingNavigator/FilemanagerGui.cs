using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil.QuickView;
using Vector.VLConfig.LoggingNavigator.Data;
using Vector.VLConfig.LoggingNavigator.Export;
using Vector.VLConfig.LoggingNavigator.GUI;
using Vector.VLConfig.LoggingNavigator.Properties;

namespace Vector.VLConfig.LoggingNavigator
{
	public class FilemanagerGui : UserControl, INavigationGUI
	{
		private delegate void CloseCallback();

		private delegate void UpdateAllCallback();

		private INavigation mIndexManager;

		private List<Marker> mMarkerList;

		private List<Trigger> mTriggerList;

		private List<Measurement> mCurrentMeasurements;

		private List<LogFile> mLogFileList;

		private List<VoiceRecord> mVoiceRecordList;

		private bool mHasOpenIndexFile;

		private string mLastActiveIndexFile = "";

		private string mOutputDirectory = "";

		private string mOutputDirectoryLastUsed = "";

		private ulong mDateMin;

		private ulong mDateMax;

		private bool mDateSelectionChanged;

		private bool mIgnoreDateChange;

		private bool mIsValid;

		private ToolTip mToolTip;

		private string mDebugDefaultFolder = "..\\..\\..\\IndexFileTester\\bin\\Debug";

		private bool mStandaloneMode = true;

		private bool mDEBUGMode;

		private static bool ENABLE_ZIP_SUPPORT = FileAccessManager.IsZipSupportEnabled();

		private bool IS_LOADING_DATA;

		private IContainer components;

		private TabPage tabPage3;

		private GroupBox mGroupBoxDateTime;

		private DateTimePicker mDateTimePickerEndTime;

		private DateTimePicker mDateTimePickerEndDate;

		private Label label4;

		private Label label3;

		private DateTimePicker mDateTimePickerBeginTime;

		private DateTimePicker mDateTimePickerBeginDate;

		private TimeLineControl mTimeLineControl;

		private LogTable mLogTable;

		private CheckBox mCheckBoxShowOther;

		private CheckBox mCheckBoxShowMarker;

		private TabControl mTabControlSettings;

		private TabPage tabPage5;

		private Button mButtonOpenZip;

		private Button mButtonOpenSmallDots;

		private Button mButtonSelectDestination;

		private Label mLabelDestinationFolder;

		private TextBox mTextBoxOutputDir;

		private TextBox mTextBoxActiveFile;

		private Label mLabelActiveFileTitle;

		private Button mButtonOpen;

		private Button mButtonReload;

		private Button mButtonClose;

		private Button mButtonEject;

		private TableLayoutPanel tableLayoutPanel1;

		private Panel panel1;

		private Button mButtonExit;

		private Button mButtonExport;

		public event EventHandler IsValidChanged;

		public ContextMenuStrip ContextMenuStripQuickView
		{
			get
			{
				return this.mLogTable.ContextMenuStripQuickView;
			}
		}

		public bool IsValid
		{
			get
			{
				return this.mIsValid;
			}
		}

		public bool RecoverMeasurements
		{
			get;
			set;
		}

		[Category("Appearance"), DefaultValue(true)]
		public bool StandaloneMode
		{
			get
			{
				return this.mStandaloneMode;
			}
			set
			{
				if (this.mStandaloneMode == value)
				{
					return;
				}
				this.mStandaloneMode = value;
				this.UpdateAll();
			}
		}

		public FilemanagerGui()
		{
			this.InitializeComponent();
			this.Init();
		}

		private void Init()
		{
			this.mDateSelectionChanged = false;
			this.mIgnoreDateChange = false;
			this.mMarkerList = new List<Marker>();
			this.mTriggerList = new List<Trigger>();
			this.mLogFileList = new List<LogFile>();
			this.mVoiceRecordList = new List<VoiceRecord>();
			this.mCurrentMeasurements = new List<Measurement>();
			this.mIndexManager = new IndexManager();
			this.mLogTable.PropertyChanged += new PropertyChangedEventHandler(this.LogTable_PropertyChanged);
			this.mToolTip = new ToolTip();
			this.mIsValid = false;
			this.RecoverMeasurements = false;
			this.IS_LOADING_DATA = false;
			this.ShowHideControls();
			string path = this.mDebugDefaultFolder + "\\file.idx";
			if (!File.Exists(path))
			{
				this.mDebugDefaultFolder = ".";
			}
			this.mOutputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			if (this.mDEBUGMode)
			{
				this.mOutputDirectory += "\\FileManagerExport";
			}
			this.UpdateAll();
		}

		private void ClearData()
		{
			this.mMarkerList.Clear();
			this.mTriggerList.Clear();
			this.mLogFileList.Clear();
			this.mVoiceRecordList.Clear();
			this.mCurrentMeasurements.Clear();
			this.mIndexManager.Clear();
			this.mIgnoreDateChange = true;
			this.mDateTimePickerBeginDate.Value = (this.mDateTimePickerBeginTime.Value = (this.mDateTimePickerEndDate.Value = (this.mDateTimePickerEndTime.Value = DateTime.Now)));
			this.mIgnoreDateChange = false;
			this.mLastActiveIndexFile = "";
		}

		private bool CheckTime(ulong time)
		{
			return time <= this.mDateMax && time >= this.mDateMin;
		}

		public void Close()
		{
			if (base.InvokeRequired)
			{
				FilemanagerGui.CloseCallback method = new FilemanagerGui.CloseCallback(this.Close);
				base.Invoke(method, new object[0]);
				return;
			}
			if (this.mTimeLineControl != null)
			{
				this.mTimeLineControl.StopAllSounds();
			}
			this.mHasOpenIndexFile = false;
			this.mOutputDirectoryLastUsed = "";
			this.mToolTip.Hide(this.mTextBoxOutputDir);
			this.mLogTable.Close();
			this.ClearData();
			this.UpdateAll();
			FileAccessManager.CloseCurrentInstance();
		}

		public OfflineSourceConfig GetOfflineSourceConfig(bool isLocalContext, bool isCANoeCANalyzer)
		{
			return this.mLogTable.GetOfflineSourceConfig(isLocalContext, isCANoeCANalyzer);
		}

		public void UpdateAll()
		{
			if (base.InvokeRequired)
			{
				FilemanagerGui.UpdateAllCallback method = new FilemanagerGui.UpdateAllCallback(this.UpdateAll);
				base.Invoke(method, new object[0]);
				return;
			}
			this.UpdateData();
			this.UpdateGUI();
			this.ValidateComponents();
		}

		private void UpdateData()
		{
			this.mCurrentMeasurements.Clear();
			this.mLogFileList.Clear();
			this.mMarkerList.Clear();
			this.mTriggerList.Clear();
			this.mVoiceRecordList.Clear();
			if (!this.mDateSelectionChanged)
			{
				this.mDateMin = 18446744073709551615uL;
				this.mDateMax = 0uL;
			}
			foreach (Measurement current in this.mIndexManager.GetMeasurements())
			{
				if (current.Closed)
				{
					if (this.mDateSelectionChanged)
					{
						if (!this.CheckTime(current.Begin) && !this.CheckTime(current.End))
						{
							if (current.Begin > this.mDateMin)
							{
								continue;
							}
							if (current.End < this.mDateMax)
							{
								continue;
							}
						}
					}
					else
					{
						if (current.Begin < this.mDateMin)
						{
							this.mDateMin = current.Begin;
						}
						if (current.End > this.mDateMax)
						{
							this.mDateMax = current.End;
						}
					}
					this.mCurrentMeasurements.Add(current);
					foreach (LogFile current2 in current.GetLogFiles())
					{
						if (!this.mDateSelectionChanged || this.CheckTime(current2.Begin) || this.CheckTime(current2.End))
						{
							this.mLogFileList.Add(current2);
							foreach (Entry current3 in current2.GetEntries())
							{
								if (current3 is Marker)
								{
									Marker marker = (Marker)current3;
									if (this.CheckTime(current3.TimeSpec))
									{
										this.mMarkerList.Add(marker);
									}
									else
									{
										marker.SelectedForExport = false;
									}
								}
								else if (current3 is Trigger)
								{
									Trigger trigger = (Trigger)current3;
									if (this.CheckTime(current3.TimeSpec) && current3.Valid)
									{
										this.mTriggerList.Add(trigger);
									}
									else
									{
										trigger.SelectedForExport = false;
									}
								}
							}
						}
					}
				}
			}
			foreach (string current4 in this.mIndexManager.GetVoiceRecordFiles())
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(current4);
				string[] array = fileNameWithoutExtension.Split(new char[]
				{
					'-',
					'_'
				});
				if (array.Length == 7)
				{
					int[] array2 = new int[array.Length];
					bool flag = false;
					for (int i = 0; i < array.Length; i++)
					{
						try
						{
							array2[i] = Convert.ToInt32(array[i]);
						}
						catch (FormatException)
						{
							flag = true;
							break;
						}
						catch (OverflowException)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						DateTime time;
						try
						{
							time = new DateTime(array2[0], array2[1], array2[2], array2[3], array2[4], array2[5], array2[6]);
						}
						catch (ArgumentOutOfRangeException)
						{
							continue;
						}
						ulong timespec = TimeSpec.ConvertFromDateTimeWithMilliseconds(time, TimeSpec.ActiveLoggerType);
						this.mVoiceRecordList.Add(new VoiceRecord(0u, 0u, timespec, current4));
					}
				}
			}
		}

		private void UpdateGUI()
		{
			this.RefreshCalendar();
			this.RefreshTimeline();
			this.RefreshMarkerList();
			this.RefreshTables();
			this.ShowHideControls();
			if (!this.mHasOpenIndexFile)
			{
				this.mTextBoxActiveFile.Text = Resources.NoSourceSelected;
			}
			else if (!string.IsNullOrEmpty(this.mLastActiveIndexFile))
			{
				this.mTextBoxActiveFile.Text = Path.GetFullPath(this.mLastActiveIndexFile);
			}
			else
			{
				this.mTextBoxActiveFile.Text = "";
			}
			this.mTextBoxOutputDir.Text = this.mOutputDirectory;
			this.mTextBoxActiveFile.ForeColor = Color.Black;
			this.mTextBoxOutputDir.ForeColor = Color.Black;
		}

		private void RefreshCalendar()
		{
			if (this.mDateMax == 0uL || this.mDateMin == 18446744073709551615uL || this.mDateSelectionChanged)
			{
				return;
			}
			this.mIgnoreDateChange = true;
			TimeSpec timeSpec = new TimeSpec(this.mDateMin);
			TimeSpec timeSpec2 = new TimeSpec(this.mDateMax);
			this.mDateTimePickerBeginDate.Value = timeSpec.DateTime;
			this.mDateTimePickerBeginTime.Value = timeSpec.DateTime;
			this.mDateTimePickerEndDate.Value = timeSpec2.DateTime;
			this.mDateTimePickerEndTime.Value = timeSpec2.DateTime;
			this.mIgnoreDateChange = false;
		}

		private void RefreshTimeline()
		{
			this.mTimeLineControl.Clear();
			this.mTimeLineControl.Maximum = this.mDateMax;
			this.mTimeLineControl.Minimum = this.mDateMin;
			foreach (Measurement current in this.mCurrentMeasurements)
			{
				if (current.IsPermanent)
				{
					this.mTimeLineControl.AddReplaceMeasurementEventWithoutRedraw(current.Begin, current.End, MeasurementType.Permanent);
				}
				else
				{
					this.mTimeLineControl.AddReplaceMeasurementEventWithoutRedraw(current.Begin, current.End, MeasurementType.Triggered);
				}
			}
			this.mTimeLineControl.MeasurementEventManualSort();
			this.mTimeLineControl.AddMarkerList(this.mMarkerList);
			this.mTimeLineControl.AddTriggerList(this.mTriggerList);
			this.mTimeLineControl.SetVoiceRecordList(this.mVoiceRecordList);
			this.mTimeLineControl.ManualRedraw();
		}

		private void RefreshMarkerList()
		{
		}

		public void RefreshTables()
		{
			this.mLogTable.SetData(this.mCurrentMeasurements, this.mLogFileList, this.mMarkerList, this.mTriggerList, this.mIndexManager);
		}

		private void ShowHideControls()
		{
			this.mButtonExit.Visible = this.mStandaloneMode;
			this.mButtonExport.Visible = this.mStandaloneMode;
			this.mButtonOpen.Visible = this.mStandaloneMode;
			this.mButtonClose.Visible = this.mStandaloneMode;
			if (!this.mStandaloneMode)
			{
				FilemanagerGui.RemoveRow(this.tableLayoutPanel1, 3);
			}
			if (!this.mStandaloneMode)
			{
				this.mTabControlSettings.Visible = false;
			}
			if (FilemanagerGui.ENABLE_ZIP_SUPPORT)
			{
				this.mButtonOpenZip.Visible = true;
			}
		}

		public static void RemoveRow(TableLayoutPanel panel, int rowIndex)
		{
			if (panel == null || panel.RowCount <= rowIndex || panel.RowStyles.Count <= rowIndex)
			{
				return;
			}
			panel.RowStyles.RemoveAt(rowIndex);
			Control controlFromPosition = panel.GetControlFromPosition(0, rowIndex);
			panel.Controls.Remove(controlFromPosition);
			panel.RowCount--;
		}

		public bool ValidateComponents()
		{
			this.mIsValid = true;
			if (!this.mLogTable.IsValid())
			{
				this.mIsValid = false;
			}
			if (!this.mHasOpenIndexFile)
			{
				this.mIsValid = false;
			}
			if (this.IsValidChanged != null)
			{
				this.IsValidChanged(this, EventArgs.Empty);
			}
			return this.mIsValid;
		}

		private void mButtonOpen_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			if (this.mLastActiveIndexFile.Length > 0)
			{
				folderBrowserDialog.SelectedPath = Path.GetFullPath(this.mLastActiveIndexFile);
			}
			else
			{
				folderBrowserDialog.SelectedPath = Path.GetFullPath(".");
			}
			folderBrowserDialog.ShowNewFolderButton = false;
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				this.OpenIndexFilesFromSource(folderBrowserDialog.SelectedPath);
			}
		}

		private void mButtonOpenZip_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "zip files (*.zip)|*.zip|All files (*.*)|*.*";
			openFileDialog.FilterIndex = 2;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.Multiselect = false;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.OpenIndexFilesFromSource(openFileDialog.FileName);
			}
		}

		private void mButtonClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void mButtonTempReset_Click(object sender, EventArgs e)
		{
			this.mDateSelectionChanged = false;
			this.UpdateAll();
		}

		private void mCheckBoxShowMarker_CheckedChanged(object sender, EventArgs e)
		{
			if (!(sender is CheckBox))
			{
				return;
			}
			this.mTimeLineControl.ShowMarker = ((CheckBox)sender).Checked;
		}

		private void mCheckBoxShowOther_CheckedChanged(object sender, EventArgs e)
		{
			if (!(sender is CheckBox))
			{
				return;
			}
			this.mTimeLineControl.ShowTriggers = ((CheckBox)sender).Checked;
		}

		private void DateChanged(object sender, EventArgs e)
		{
			if (this.mIgnoreDateChange)
			{
				return;
			}
			this.mDateSelectionChanged = true;
			DateTime time = this.mDateTimePickerBeginDate.Value.Date.Add(this.mDateTimePickerBeginTime.Value.TimeOfDay);
			DateTime time2 = this.mDateTimePickerEndDate.Value.Date.Add(this.mDateTimePickerEndTime.Value.TimeOfDay);
			TimeSpec timeSpec = new TimeSpec(time);
			TimeSpec timeSpec2 = new TimeSpec(time2);
			this.mDateMin = timeSpec.Value;
			this.mDateMax = timeSpec2.Value;
			this.UpdateAll();
		}

		private void LogTable_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.ValidateComponents();
			switch (this.mLogTable.ExportMode)
			{
			case ExportType.Measurements:
				this.mButtonExport.Text = "Export " + Resources.Measurements + "...";
				return;
			case ExportType.Marker:
				this.mButtonExport.Text = "Export " + Resources.Marker + "...";
				return;
			case ExportType.Files:
				this.mButtonExport.Text = "Export " + Resources.Files + "...";
				return;
			case ExportType.Trigger:
				this.mButtonExport.Text = "Export " + Resources.Trigger + "...";
				return;
			default:
				return;
			}
		}

		private void mButtonSelectDestination_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.SelectedPath = this.mOutputDirectory;
			folderBrowserDialog.ShowNewFolderButton = true;
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				this.mOutputDirectory = folderBrowserDialog.SelectedPath;
			}
			this.UpdateAll();
		}

		private void mTextBoxOutputDir_DoubleClick(object sender, EventArgs e)
		{
			if (this.mOutputDirectoryLastUsed.Length > 0)
			{
				FilemanagerGui.LaunchDirectoryBrowser(this.mOutputDirectoryLastUsed);
			}
		}

		private static bool LaunchDirectoryBrowser(string directoryPath)
		{
			DirectoryInfo directoryInfo = null;
			try
			{
				directoryInfo = new DirectoryInfo(directoryPath);
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			if (directoryInfo == null)
			{
				return false;
			}
			if (directoryInfo.Exists)
			{
				try
				{
					Process.Start(directoryPath);
				}
				catch (Exception)
				{
					bool result = false;
					return result;
				}
			}
			return true;
		}

		private void mButtonExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void MarkerSelectionTable_Changed(object sender, EventArgs e)
		{
			this.RefreshTables();
		}

		private void mTextBoxActiveFile_Leave(object sender, EventArgs e)
		{
			try
			{
				string fullPath = Path.GetFullPath(this.mTextBoxActiveFile.Text);
				if (Directory.Exists(fullPath) || (File.Exists(fullPath) && fullPath.ToLower().EndsWith(".zip")))
				{
					this.OpenIndexFilesFromSource(fullPath);
				}
				else
				{
					this.mTextBoxActiveFile.ForeColor = Color.Red;
				}
			}
			catch (Exception)
			{
				this.mTextBoxActiveFile.ForeColor = Color.Red;
			}
		}

		private void mTextBoxActiveFile_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				try
				{
					string fullPath = Path.GetFullPath(this.mTextBoxActiveFile.Text);
					if (Directory.Exists(fullPath) || File.Exists(fullPath))
					{
						this.OpenIndexFilesFromSource(fullPath);
					}
					else
					{
						this.mTextBoxActiveFile.ForeColor = Color.Red;
					}
				}
				catch (Exception)
				{
					this.mTextBoxActiveFile.ForeColor = Color.Red;
				}
			}
		}

		private void mTextBoxOutputDir_Leave(object sender, EventArgs e)
		{
			try
			{
				string fullPath = Path.GetFullPath(this.mTextBoxOutputDir.Text);
				if (Directory.Exists(fullPath))
				{
					this.mOutputDirectory = fullPath;
					this.UpdateAll();
				}
				else
				{
					this.mTextBoxOutputDir.ForeColor = Color.Red;
				}
			}
			catch (Exception)
			{
				this.mTextBoxOutputDir.ForeColor = Color.Red;
			}
		}

		private void mTextBoxOutputDir_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				try
				{
					string fullPath = Path.GetFullPath(this.mTextBoxOutputDir.Text);
					if (Directory.Exists(fullPath))
					{
						this.mOutputDirectory = fullPath;
						this.UpdateAll();
					}
					else
					{
						this.mTextBoxOutputDir.ForeColor = Color.Red;
					}
				}
				catch (Exception)
				{
					this.mTextBoxOutputDir.ForeColor = Color.Red;
				}
			}
		}

		private void mTextBoxActiveFile_DragEnter(object sender, DragEventArgs e)
		{
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (array != null && array.Length != 0)
			{
				if (Directory.Exists(array[0]))
				{
					e.Effect = DragDropEffects.Copy;
					return;
				}
				if (File.Exists(array[0]) && array[0].ToLower().EndsWith(".zip") && FilemanagerGui.ENABLE_ZIP_SUPPORT)
				{
					e.Effect = DragDropEffects.Copy;
				}
			}
		}

		private void mTextBoxActiveFile_DragDrop(object sender, DragEventArgs e)
		{
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (array != null && array.Length != 0)
			{
				this.mTextBoxActiveFile.Text = array[0];
				if (Directory.Exists(this.mTextBoxActiveFile.Text))
				{
					e.Effect = DragDropEffects.Copy;
					this.OpenIndexFilesFromSource(this.mTextBoxActiveFile.Text);
					return;
				}
				if (File.Exists(this.mTextBoxActiveFile.Text) && this.mTextBoxActiveFile.Text.ToLower().EndsWith(".zip") && FilemanagerGui.ENABLE_ZIP_SUPPORT)
				{
					e.Effect = DragDropEffects.Copy;
					this.OpenIndexFilesFromSource(this.mTextBoxActiveFile.Text);
					return;
				}
				this.mTextBoxActiveFile.ForeColor = Color.Red;
			}
		}

		private void mTextBoxOutputDir_DragEnter(object sender, DragEventArgs e)
		{
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (array != null && array.Length != 0 && Directory.Exists(array[0]))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		private void mTextBoxOutputDir_DragDrop(object sender, DragEventArgs e)
		{
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (array != null && array.Length != 0 && Directory.Exists(array[0]))
			{
				this.mOutputDirectory = array[0];
				this.UpdateAll();
			}
		}

		private void Export(out IList<ExportJob> jobs)
		{
			ExportManager exportManager = new ExportManager();
			if (this.mLogTable.ExportMode == ExportType.Measurements)
			{
				exportManager.ExportMeasurements(this.mCurrentMeasurements, out jobs);
				return;
			}
			if (this.mLogTable.ExportMode == ExportType.Marker)
			{
				exportManager.ExportMarker(this.mMarkerList, out jobs, this.mLogTable.GetMarkerSelectionTable().GetMarkerBefore(), this.mLogTable.GetMarkerSelectionTable().GetMarkerAfter());
				return;
			}
			if (this.mLogTable.ExportMode == ExportType.Files)
			{
				exportManager.ExportLogFiles(this.mIndexManager, this.mLogFileList, out jobs);
				return;
			}
			if (this.mLogTable.ExportMode == ExportType.Trigger)
			{
				exportManager.ExportTrigger(this.mTriggerList, out jobs);
				return;
			}
			jobs = new List<ExportJob>();
		}

		private void mButtonExport_Click(object sender, EventArgs e)
		{
			IList<ExportJob> list;
			this.Export(out list);
			if (this.mDEBUGMode)
			{
				this.PrintExport(list);
			}
			if (list == null || list.Count < 1)
			{
				string text = "Selection is empty. Nothing to export!";
				MessageBox.Show(text);
				return;
			}
			string text2 = this.mOutputDirectory + "\\";
			text2 = text2 + Utils.ConvertDateTimeToFolderName(DateTime.Now) + "\\";
			FileCopyDialog fileCopyDialog = new FileCopyDialog();
			fileCopyDialog.SetFilesToCopy(Path.GetFullPath(this.mLastActiveIndexFile), list, text2);
			fileCopyDialog.ShowDialog();
			fileCopyDialog.Dispose();
			this.mOutputDirectoryLastUsed = text2;
			this.mToolTip.SetToolTip(this.mTextBoxOutputDir, "Double click to open \"" + this.mOutputDirectoryLastUsed + "\"");
		}

		private void PrintExport(IList<ExportJob> jobs)
		{
			string text = "";
			if (jobs == null || jobs.Count < 1)
			{
				return;
			}
			foreach (ExportJob current in jobs)
			{
				text = text + "[Export:] \"" + current.Name + "\"";
				string text2 = "";
				foreach (LogFile current2 in current.LogFileList)
				{
					text2 = text2 + current2.Name + ", ";
				}
				if (text2.Length > 1)
				{
					text2 = text2.Substring(0, text2.Length - 2);
				}
				text = text + " [" + text2 + "]\r\n";
			}
			MessageBox.Show(text);
		}

		public void OpenIndexFilesFromSource(string path)
		{
			if (this.IS_LOADING_DATA)
			{
				return;
			}
			this.IS_LOADING_DATA = true;
			this.Close();
			this.mLastActiveIndexFile = path;
			this.mHasOpenIndexFile = true;
			this.mDateSelectionChanged = false;
			this.mIndexManager.RecoverMeasurements = this.RecoverMeasurements;
			this.mIndexManager.ReadIndexFilesFromSource(path);
			this.mLogTable.Open();
			this.UpdateAll();
			this.IS_LOADING_DATA = false;
		}

		public bool GetExportJobs(out IList<ExportJob> jobs)
		{
			this.Export(out jobs);
			return jobs != null;
		}

		public INavigationGUI GetIndexNavigation()
		{
			return this;
		}

		public bool IsEmpty()
		{
			return this.mIndexManager.IsEmpty();
		}

		public string GetCurrentPath()
		{
			return this.mLastActiveIndexFile;
		}

		public ExportType GetExportType()
		{
			return this.mLogTable.ExportMode;
		}

		public void SetActiveLoggerType(LoggerType type)
		{
			TimeSpec.SetActiveLoggerType(type);
		}

		public LayoutSerializationContainer SerializeGrid()
		{
			return this.mLogTable.SerializeGrid();
		}

		public void DeSerializeGrid(LayoutSerializationContainer layout)
		{
			this.mLogTable.DeSerializeGrid(layout);
		}

		public bool HasMissingLogfiles()
		{
			return this.mIndexManager.HasMissingLogfiles();
		}

		public bool HasErrorInIndexFile()
		{
			return this.mIndexManager.HasIndexError();
		}

		public IEnumerable<string> RestoreMarkerTypeSelection(IEnumerable<string> markerTypeList)
		{
			return this.mLogTable.RestoreMarkerTypeSelection(markerTypeList);
		}

		public IEnumerable<string> GetMarkerTypeSelection()
		{
			return this.mLogTable.GetMarkerTypeSelection();
		}

		public IEnumerable<string> RestoreTriggerTypeSelection(IEnumerable<string> triggerTypeList)
		{
			return this.mLogTable.RestoreTriggerTypeSelection(triggerTypeList);
		}

		public IEnumerable<string> GetTriggerTypeSelection()
		{
			return this.mLogTable.GetTriggerTypeSelection();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FilemanagerGui));
			this.tabPage3 = new TabPage();
			this.mCheckBoxShowOther = new CheckBox();
			this.mCheckBoxShowMarker = new CheckBox();
			this.mGroupBoxDateTime = new GroupBox();
			this.mDateTimePickerEndTime = new DateTimePicker();
			this.mDateTimePickerEndDate = new DateTimePicker();
			this.label4 = new Label();
			this.label3 = new Label();
			this.mTimeLineControl = new TimeLineControl();
			this.mDateTimePickerBeginTime = new DateTimePicker();
			this.mDateTimePickerBeginDate = new DateTimePicker();
			this.mButtonReload = new Button();
			this.mTabControlSettings = new TabControl();
			this.tabPage5 = new TabPage();
			this.mButtonOpenZip = new Button();
			this.mButtonOpenSmallDots = new Button();
			this.mButtonSelectDestination = new Button();
			this.mLabelDestinationFolder = new Label();
			this.mTextBoxOutputDir = new TextBox();
			this.mTextBoxActiveFile = new TextBox();
			this.mLabelActiveFileTitle = new Label();
			this.mButtonOpen = new Button();
			this.mButtonClose = new Button();
			this.mButtonEject = new Button();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.mLogTable = new LogTable();
			this.panel1 = new Panel();
			this.mButtonExit = new Button();
			this.mButtonExport = new Button();
			this.mGroupBoxDateTime.SuspendLayout();
			this.mTabControlSettings.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tabPage3, "tabPage3");
			this.tabPage3.Name = "tabPage3";
			componentResourceManager.ApplyResources(this.mCheckBoxShowOther, "mCheckBoxShowOther");
			this.mCheckBoxShowOther.Checked = true;
			this.mCheckBoxShowOther.CheckState = CheckState.Checked;
			this.mCheckBoxShowOther.Name = "mCheckBoxShowOther";
			this.mCheckBoxShowOther.UseVisualStyleBackColor = true;
			this.mCheckBoxShowOther.CheckedChanged += new EventHandler(this.mCheckBoxShowOther_CheckedChanged);
			componentResourceManager.ApplyResources(this.mCheckBoxShowMarker, "mCheckBoxShowMarker");
			this.mCheckBoxShowMarker.Checked = true;
			this.mCheckBoxShowMarker.CheckState = CheckState.Checked;
			this.mCheckBoxShowMarker.Name = "mCheckBoxShowMarker";
			this.mCheckBoxShowMarker.UseVisualStyleBackColor = true;
			this.mCheckBoxShowMarker.CheckedChanged += new EventHandler(this.mCheckBoxShowMarker_CheckedChanged);
			componentResourceManager.ApplyResources(this.mGroupBoxDateTime, "mGroupBoxDateTime");
			this.mGroupBoxDateTime.Controls.Add(this.mCheckBoxShowOther);
			this.mGroupBoxDateTime.Controls.Add(this.mDateTimePickerEndTime);
			this.mGroupBoxDateTime.Controls.Add(this.mCheckBoxShowMarker);
			this.mGroupBoxDateTime.Controls.Add(this.mDateTimePickerEndDate);
			this.mGroupBoxDateTime.Controls.Add(this.label4);
			this.mGroupBoxDateTime.Controls.Add(this.label3);
			this.mGroupBoxDateTime.Controls.Add(this.mTimeLineControl);
			this.mGroupBoxDateTime.Controls.Add(this.mDateTimePickerBeginTime);
			this.mGroupBoxDateTime.Controls.Add(this.mDateTimePickerBeginDate);
			this.mGroupBoxDateTime.Controls.Add(this.mButtonReload);
			this.mGroupBoxDateTime.Name = "mGroupBoxDateTime";
			this.mGroupBoxDateTime.TabStop = false;
			this.mDateTimePickerEndTime.Format = DateTimePickerFormat.Time;
			componentResourceManager.ApplyResources(this.mDateTimePickerEndTime, "mDateTimePickerEndTime");
			this.mDateTimePickerEndTime.Name = "mDateTimePickerEndTime";
			this.mDateTimePickerEndTime.ShowUpDown = true;
			this.mDateTimePickerEndTime.ValueChanged += new EventHandler(this.DateChanged);
			componentResourceManager.ApplyResources(this.mDateTimePickerEndDate, "mDateTimePickerEndDate");
			this.mDateTimePickerEndDate.MinDate = new DateTime(1980, 1, 1, 0, 0, 0, 0);
			this.mDateTimePickerEndDate.Name = "mDateTimePickerEndDate";
			this.mDateTimePickerEndDate.ValueChanged += new EventHandler(this.DateChanged);
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.mTimeLineControl.AllowUserToChangePosition = false;
			componentResourceManager.ApplyResources(this.mTimeLineControl, "mTimeLineControl");
			this.mTimeLineControl.MinimumSeverityLevelToDisplay = 0u;
			this.mTimeLineControl.Name = "mTimeLineControl";
			this.mTimeLineControl.ShowCurrentPosition = false;
			this.mTimeLineControl.ShowSpecialEvents = false;
			this.mDateTimePickerBeginTime.Format = DateTimePickerFormat.Time;
			componentResourceManager.ApplyResources(this.mDateTimePickerBeginTime, "mDateTimePickerBeginTime");
			this.mDateTimePickerBeginTime.Name = "mDateTimePickerBeginTime";
			this.mDateTimePickerBeginTime.ShowUpDown = true;
			this.mDateTimePickerBeginTime.ValueChanged += new EventHandler(this.DateChanged);
			componentResourceManager.ApplyResources(this.mDateTimePickerBeginDate, "mDateTimePickerBeginDate");
			this.mDateTimePickerBeginDate.MinDate = new DateTime(1980, 1, 1, 0, 0, 0, 0);
			this.mDateTimePickerBeginDate.Name = "mDateTimePickerBeginDate";
			this.mDateTimePickerBeginDate.ValueChanged += new EventHandler(this.DateChanged);
			componentResourceManager.ApplyResources(this.mButtonReload, "mButtonReload");
			this.mButtonReload.Name = "mButtonReload";
			this.mButtonReload.UseVisualStyleBackColor = true;
			this.mButtonReload.Click += new EventHandler(this.mButtonTempReset_Click);
			componentResourceManager.ApplyResources(this.mTabControlSettings, "mTabControlSettings");
			this.mTabControlSettings.Controls.Add(this.tabPage5);
			this.mTabControlSettings.Name = "mTabControlSettings";
			this.mTabControlSettings.SelectedIndex = 0;
			this.tabPage5.Controls.Add(this.mButtonOpenZip);
			this.tabPage5.Controls.Add(this.mButtonOpenSmallDots);
			this.tabPage5.Controls.Add(this.mButtonSelectDestination);
			this.tabPage5.Controls.Add(this.mLabelDestinationFolder);
			this.tabPage5.Controls.Add(this.mTextBoxOutputDir);
			this.tabPage5.Controls.Add(this.mTextBoxActiveFile);
			this.tabPage5.Controls.Add(this.mLabelActiveFileTitle);
			this.tabPage5.Controls.Add(this.mButtonOpen);
			this.tabPage5.Controls.Add(this.mButtonClose);
			this.tabPage5.Controls.Add(this.mButtonEject);
			componentResourceManager.ApplyResources(this.tabPage5, "tabPage5");
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mButtonOpenZip, "mButtonOpenZip");
			this.mButtonOpenZip.Name = "mButtonOpenZip";
			this.mButtonOpenZip.UseVisualStyleBackColor = true;
			this.mButtonOpenZip.Click += new EventHandler(this.mButtonOpenZip_Click);
			componentResourceManager.ApplyResources(this.mButtonOpenSmallDots, "mButtonOpenSmallDots");
			this.mButtonOpenSmallDots.Name = "mButtonOpenSmallDots";
			this.mButtonOpenSmallDots.UseVisualStyleBackColor = true;
			this.mButtonOpenSmallDots.Click += new EventHandler(this.mButtonOpen_Click);
			componentResourceManager.ApplyResources(this.mButtonSelectDestination, "mButtonSelectDestination");
			this.mButtonSelectDestination.Name = "mButtonSelectDestination";
			this.mButtonSelectDestination.UseVisualStyleBackColor = true;
			this.mButtonSelectDestination.Click += new EventHandler(this.mButtonSelectDestination_Click);
			componentResourceManager.ApplyResources(this.mLabelDestinationFolder, "mLabelDestinationFolder");
			this.mLabelDestinationFolder.Name = "mLabelDestinationFolder";
			this.mTextBoxOutputDir.AllowDrop = true;
			componentResourceManager.ApplyResources(this.mTextBoxOutputDir, "mTextBoxOutputDir");
			this.mTextBoxOutputDir.Name = "mTextBoxOutputDir";
			this.mTextBoxOutputDir.DragDrop += new DragEventHandler(this.mTextBoxOutputDir_DragDrop);
			this.mTextBoxOutputDir.DragEnter += new DragEventHandler(this.mTextBoxOutputDir_DragEnter);
			this.mTextBoxOutputDir.DoubleClick += new EventHandler(this.mTextBoxOutputDir_DoubleClick);
			this.mTextBoxOutputDir.KeyUp += new KeyEventHandler(this.mTextBoxOutputDir_KeyUp);
			this.mTextBoxOutputDir.Leave += new EventHandler(this.mTextBoxOutputDir_Leave);
			this.mTextBoxActiveFile.AllowDrop = true;
			componentResourceManager.ApplyResources(this.mTextBoxActiveFile, "mTextBoxActiveFile");
			this.mTextBoxActiveFile.Name = "mTextBoxActiveFile";
			this.mTextBoxActiveFile.DragDrop += new DragEventHandler(this.mTextBoxActiveFile_DragDrop);
			this.mTextBoxActiveFile.DragEnter += new DragEventHandler(this.mTextBoxActiveFile_DragEnter);
			this.mTextBoxActiveFile.KeyUp += new KeyEventHandler(this.mTextBoxActiveFile_KeyUp);
			this.mTextBoxActiveFile.Leave += new EventHandler(this.mTextBoxActiveFile_Leave);
			componentResourceManager.ApplyResources(this.mLabelActiveFileTitle, "mLabelActiveFileTitle");
			this.mLabelActiveFileTitle.Name = "mLabelActiveFileTitle";
			componentResourceManager.ApplyResources(this.mButtonOpen, "mButtonOpen");
			this.mButtonOpen.Name = "mButtonOpen";
			this.mButtonOpen.UseVisualStyleBackColor = true;
			this.mButtonOpen.Click += new EventHandler(this.mButtonOpen_Click);
			componentResourceManager.ApplyResources(this.mButtonClose, "mButtonClose");
			this.mButtonClose.Name = "mButtonClose";
			this.mButtonClose.UseVisualStyleBackColor = true;
			this.mButtonClose.Click += new EventHandler(this.mButtonClose_Click);
			componentResourceManager.ApplyResources(this.mButtonEject, "mButtonEject");
			this.mButtonEject.Name = "mButtonEject";
			this.mButtonEject.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.mTabControlSettings, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.mGroupBoxDateTime, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.mLogTable, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.mLogTable, "mLogTable");
			this.mLogTable.ExportMode = ExportType.Measurements;
			this.mLogTable.Name = "mLogTable";
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Controls.Add(this.mButtonExit);
			this.panel1.Controls.Add(this.mButtonExport);
			this.panel1.Name = "panel1";
			componentResourceManager.ApplyResources(this.mButtonExit, "mButtonExit");
			this.mButtonExit.Name = "mButtonExit";
			this.mButtonExit.UseVisualStyleBackColor = true;
			this.mButtonExit.Click += new EventHandler(this.mButtonExit_Click);
			componentResourceManager.ApplyResources(this.mButtonExport, "mButtonExport");
			this.mButtonExport.Name = "mButtonExport";
			this.mButtonExport.UseVisualStyleBackColor = true;
			this.mButtonExport.Click += new EventHandler(this.mButtonExport_Click);
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.tableLayoutPanel1);
			base.Name = "FilemanagerGui";
			componentResourceManager.ApplyResources(this, "$this");
			this.mGroupBoxDateTime.ResumeLayout(false);
			this.mGroupBoxDateTime.PerformLayout();
			this.mTabControlSettings.ResumeLayout(false);
			this.tabPage5.ResumeLayout(false);
			this.tabPage5.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
