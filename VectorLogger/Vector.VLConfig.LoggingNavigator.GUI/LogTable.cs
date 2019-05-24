using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil.QuickView;
using Vector.VLConfig.LoggingNavigator.Data;
using Vector.VLConfig.LoggingNavigator.Export;
using Vector.VLConfig.LoggingNavigator.Properties;

namespace Vector.VLConfig.LoggingNavigator.GUI
{
	public class LogTable : UserControl, INotifyPropertyChanged
	{
		private IList<Measurement> mMeasurementList;

		private IList<LogFile> mLogFileList;

		private IList<Marker> mMarkerList;

		private IList<Trigger> mTriggerList;

		private bool mOpenIndexFile;

		private ExportType mExportMode;

		private INavigation mIndexManager;

		private IContainer components;

		private TabControl mTabControlLogTable;

		private TabPage tabPageMeasurements;

		private GridControl mGridControlMeasurements;

		private GridView mGridViewMeasurements;

		private GridColumn gridColumnName;

		private GridColumn gridColumnBegin;

		private GridColumn gridColumnEnd;

		private GridColumn gridColumnListOfLogFiles;

		private GridColumn gridColumnPermanent;

		private TabPage tabPageMarker;

		private GridControl mGridControlMarker;

		private GridView mGridViewMarker;

		private GridColumn gridColumnMarkerName;

		private GridColumn gridColumnMarkerTime;

		private TabPage tabPageFiles;

		private GridColumn gridColumnMarkerLogFiles;

		private GridControl mGridControlLogFiles;

		private GridView mGridViewLogFiles;

		private GridColumn gridColumnLogName;

		private GridColumn gridColumnLogBegin;

		private GridColumn gridColumnLogEnd;

		private GridColumn gridColumnLogPermanent;

		private GridColumn gridColumn8;

		private GridColumn gridColumnSelected;

		private GridColumn gridColumnMarkerSelected;

		private GridColumn gridColumnLogSelected;

		private GridColumn gridColumnMarkerLabel;

		private RepositoryItemCheckEdit repositoryItemCheckEditLogFilesIsPermanent;

		private RepositoryItemCheckEdit repositoryItemCheckEditIsPermanent;

		private MarkerSelectionTable mMarkerSelectionTable;

		private RepositoryItemCheckEdit repositoryItemCheckEditMeasurementSelected;

		private RepositoryItemCheckEdit repositoryItemCheckEditMarkerSelected;

		private RepositoryItemCheckEdit repositoryItemCheckEditFilesSelected;

		private ContextMenuStrip contextMenuStripLogTable;

		private ToolStripMenuItem toolStripMenuItemSelectAll;

		private ToolStripMenuItem toolStripMenuItemSelectNone;

		private TabPage tabPageTrigger;

		private GridControl mGridControlTrigger;

		private GridView mGridViewTrigger;

		private GridColumn gridColumnTriggerSelected;

		private RepositoryItemCheckEdit repositoryItemCheckEdit1;

		private GridColumn gridColumnTriggerName;

		private GridColumn gridColumnTriggerTime;

		private GridColumn gridColumnTriggerLogFiles;

		private GridColumn gridColumnTriggerLabel;

		private TriggerSelectionTable mTriggerSelectionTable;

		private ToolTipController toolTipControllerLogTable;

		public event PropertyChangedEventHandler PropertyChanged;

		public ExportType ExportMode
		{
			get
			{
				return this.mExportMode;
			}
			set
			{
				this.mExportMode = value;
				this.NotifyPropertyChanged("ExportMode");
			}
		}

		public ContextMenuStrip ContextMenuStripQuickView
		{
			get
			{
				return this.contextMenuStripLogTable;
			}
		}

		public LogTable()
		{
			this.InitializeComponent();
			this.mMarkerList = new List<Marker>();
			this.mTriggerList = new List<Trigger>();
			this.mOpenIndexFile = false;
			this.ExportMode = ExportType.Measurements;
			this.mGridViewMarker.ActiveFilterCriteria = new BinaryOperator("Selected", true);
			this.mGridViewTrigger.ActiveFilterCriteria = new BinaryOperator("Selected", true);
			this.repositoryItemCheckEditIsPermanent.Enabled = false;
			this.repositoryItemCheckEditLogFilesIsPermanent.Enabled = false;
			this.mMarkerSelectionTable.Changed += new EventHandler(this.MarkerSelectionTable_Changed);
			this.mTriggerSelectionTable.Changed += new EventHandler(this.TriggerSelectionTable_Changed);
		}

		public void SetData(IList<Measurement> measurements, IList<LogFile> logfiles, IList<Marker> markers, IList<Trigger> triggers, INavigation indexManager)
		{
			this.mIndexManager = indexManager;
			this.mMeasurementList = measurements;
			this.mGridControlMeasurements.DataSource = this.mMeasurementList;
			this.mGridControlMeasurements.RefreshDataSource();
			this.mLogFileList = logfiles;
			this.mGridControlLogFiles.DataSource = this.mLogFileList;
			this.mGridControlLogFiles.RefreshDataSource();
			this.mMarkerList = markers;
			this.mGridControlMarker.DataSource = this.mMarkerList;
			this.mGridControlMarker.RefreshDataSource();
			this.mMarkerSelectionTable.SetData(this.mMarkerList);
			this.UpdateMarkerList(this.mMarkerSelectionTable.GetMarkerBefore(), this.mMarkerSelectionTable.GetMarkerAfter());
			this.mTriggerList = triggers;
			this.mGridControlTrigger.DataSource = this.mTriggerList;
			this.mGridControlTrigger.RefreshDataSource();
			this.mTriggerSelectionTable.SetData(this.mTriggerList);
			this.UpdateTriggerList();
		}

		private void mGridViewMeasurements_CustomDrawEmptyForeground(object sender, CustomDrawEventArgs e)
		{
			if (!(sender is GridView))
			{
				return;
			}
			GridView gridView = sender as GridView;
			if (gridView.RowCount > 0)
			{
				return;
			}
			string s;
			if (!this.mOpenIndexFile)
			{
				s = Resources.LogTableNoSourceSelected;
			}
			else
			{
				s = Resources.LogTableSelectionEmptyMeasurements;
			}
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = (stringFormat.LineAlignment = StringAlignment.Center);
			e.Graphics.DrawString(s, e.Appearance.Font, SystemBrushes.ControlDark, new RectangleF((float)e.Bounds.X, (float)e.Bounds.Y, (float)e.Bounds.Width, (float)e.Bounds.Height), stringFormat);
		}

		private void mGridViewMeasurements_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			Measurement measurement;
			if (!this.GetMeasurement(e.RowHandle, out measurement))
			{
				return;
			}
			if (e.Column == this.gridColumnEnd && measurement.IsForcedClose)
			{
				int num = 16;
				Rectangle bounds = e.Bounds;
				Point location = bounds.Location;
				int num2 = (bounds.Height - num) / 2;
				int num3 = bounds.Width - num;
				Rectangle rect = new Rectangle(location.X + num3, location.Y + num2, num, num);
				e.Graphics.DrawImageUnscaled(Resources.IconInfo.ToBitmap(), rect);
			}
		}

		private void mGridViewMeasurements_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			Measurement measurement;
			if (!this.GetMeasurement(e.ListSourceRowIndex, out measurement))
			{
				return;
			}
			if (e.Column == this.gridColumnName)
			{
				e.Value = measurement.Name;
				return;
			}
			if (e.Column == this.gridColumnBegin)
			{
				TimeSpec timeSpec = new TimeSpec(measurement.Begin);
				e.Value = timeSpec.DateTime;
				return;
			}
			if (e.Column == this.gridColumnEnd)
			{
				TimeSpec timeSpec2 = new TimeSpec(measurement.End);
				e.Value = timeSpec2.DateTime;
			}
		}

		private bool GetMeasurement(int listSourceRowIndex, out Measurement measurement)
		{
			measurement = null;
			if (listSourceRowIndex < 0 || listSourceRowIndex >= this.mMeasurementList.Count)
			{
				return false;
			}
			measurement = this.mMeasurementList[listSourceRowIndex];
			return null != measurement;
		}

		private void mGridViewMarker_CustomDrawEmptyForeground(object sender, CustomDrawEventArgs e)
		{
			if (!(sender is GridView))
			{
				return;
			}
			GridView gridView = sender as GridView;
			if (gridView.RowCount > 0)
			{
				return;
			}
			string s;
			if (!this.mOpenIndexFile)
			{
				s = Resources.LogTableNoSourceSelected;
			}
			else if (this.mMarkerList.Count < 1)
			{
				s = Resources.LogTableSelectionEmptyMarker;
			}
			else
			{
				s = Resources.LogTableSelectionEmptyMarkerChangeFilter;
			}
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = (stringFormat.LineAlignment = StringAlignment.Center);
			e.Graphics.DrawString(s, e.Appearance.Font, SystemBrushes.ControlDark, new RectangleF((float)e.Bounds.X, (float)e.Bounds.Y, (float)e.Bounds.Width, (float)e.Bounds.Height), stringFormat);
		}

		private void mGridViewMarker_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			Marker marker;
			if (!this.GetMarker(e.ListSourceRowIndex, out marker))
			{
				return;
			}
			if (e.Column == this.gridColumnMarkerTime)
			{
				TimeSpec timeSpec = new TimeSpec(marker.TimeSpec);
				e.Value = timeSpec.DateTime;
			}
			if (e.Column == this.gridColumnMarkerLabel)
			{
				e.Value = marker.LabelAndOccurences;
				return;
			}
			if (e.Column == this.gridColumnMarkerLogFiles)
			{
				string text = "";
				foreach (LogFile current in marker.GetLogFilesForExport())
				{
					text = text + current.Name + ", ";
				}
				if (text.Length > 1)
				{
					text = text.Substring(0, text.Length - 2);
				}
				e.Value = text;
			}
		}

		private bool GetMarker(int listSourceRowIndex, out Marker marker)
		{
			marker = null;
			if (listSourceRowIndex < 0 || listSourceRowIndex >= this.mMarkerList.Count)
			{
				return false;
			}
			marker = this.mMarkerList[listSourceRowIndex];
			return null != marker;
		}

		private IList<Marker> GetAllMarkers()
		{
			List<Marker> list = new List<Marker>();
			foreach (LogFile current in this.mLogFileList)
			{
				foreach (Entry current2 in current.GetEntries())
				{
					if (current2 is Marker)
					{
						Marker marker = (Marker)current2;
						if (marker.Selected)
						{
							list.Add(marker);
						}
					}
				}
			}
			return list;
		}

		public void UpdateMarkerList(ulong before, ulong after)
		{
			foreach (Marker current in this.mMarkerList)
			{
				current.ClearLogFilesForExport();
				if (current.Selected)
				{
					foreach (LogFile current2 in this.mLogFileList)
					{
						if (Utils.LogFileMatchesMarker(current, current2, before, after))
						{
							current.AddLogFileForExport(current2);
						}
					}
				}
			}
			this.mGridControlMarker.RefreshDataSource();
		}

		private void MarkerSelectionTable_Changed(object sender, EventArgs e)
		{
			this.UpdateMarkerList(this.mMarkerSelectionTable.GetMarkerBefore(), this.mMarkerSelectionTable.GetMarkerAfter());
			this.NotifyPropertyChanged("validate");
		}

		public MarkerSelectionTable GetMarkerSelectionTable()
		{
			return this.mMarkerSelectionTable;
		}

		public IEnumerable<string> RestoreMarkerTypeSelection(IEnumerable<string> markerTypeList)
		{
			this.mGridControlMarker.FocusedView.CloseEditor();
			return this.mMarkerSelectionTable.RestoreMarkerTypeSelection(markerTypeList);
		}

		public IEnumerable<string> GetMarkerTypeSelection()
		{
			return this.mMarkerSelectionTable.GetMarkerTypeSelection();
		}

		public IEnumerable<string> RestoreTriggerTypeSelection(IEnumerable<string> triggerTypeList)
		{
			this.mGridControlTrigger.FocusedView.CloseEditor();
			return this.mTriggerSelectionTable.RestoreTriggerTypeSelection(triggerTypeList);
		}

		public IEnumerable<string> GetTriggerTypeSelection()
		{
			return this.mTriggerSelectionTable.GetTriggerTypeSelection();
		}

		private void mGridViewTrigger_CustomDrawEmptyForeground(object sender, CustomDrawEventArgs e)
		{
			if (!(sender is GridView))
			{
				return;
			}
			GridView gridView = sender as GridView;
			if (gridView.RowCount > 0)
			{
				return;
			}
			string s;
			if (!this.mOpenIndexFile)
			{
				s = Resources.LogTableNoSourceSelected;
			}
			else if (this.mTriggerList.Count < 1)
			{
				s = Resources.LogTableSelectionEmptyTrigger;
			}
			else
			{
				s = Resources.LogTableSelectionEmptyTriggerChangeFilter;
			}
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = (stringFormat.LineAlignment = StringAlignment.Center);
			e.Graphics.DrawString(s, e.Appearance.Font, SystemBrushes.ControlDark, new RectangleF((float)e.Bounds.X, (float)e.Bounds.Y, (float)e.Bounds.Width, (float)e.Bounds.Height), stringFormat);
		}

		private void mGridViewTrigger_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			Trigger trigger;
			if (!this.GetTrigger(e.ListSourceRowIndex, out trigger))
			{
				return;
			}
			if (e.Column == this.gridColumnTriggerTime)
			{
				TimeSpec timeSpec = new TimeSpec(trigger.TimeSpec);
				e.Value = timeSpec.DateTime;
			}
			if (e.Column == this.gridColumnTriggerLabel)
			{
				e.Value = trigger.LabelAndOccurences;
				return;
			}
			if (e.Column == this.gridColumnTriggerLogFiles)
			{
				if (trigger.Measurement != null)
				{
					e.Value = trigger.Measurement.LogFileNames;
					return;
				}
				e.Value = "";
			}
		}

		private bool GetTrigger(int listSourceRowIndex, out Trigger trigger)
		{
			trigger = null;
			if (listSourceRowIndex < 0 || listSourceRowIndex >= this.mTriggerList.Count)
			{
				return false;
			}
			trigger = this.mTriggerList[listSourceRowIndex];
			return null != trigger;
		}

		private IList<Trigger> GetAllTriggers()
		{
			List<Trigger> list = new List<Trigger>();
			foreach (LogFile current in this.mLogFileList)
			{
				foreach (Entry current2 in current.GetEntries())
				{
					if (current2 is Trigger)
					{
						Trigger trigger = (Trigger)current2;
						if (trigger.Selected)
						{
							list.Add(trigger);
						}
					}
				}
			}
			return list;
		}

		public void UpdateTriggerList()
		{
			this.mGridControlTrigger.RefreshDataSource();
		}

		private void TriggerSelectionTable_Changed(object sender, EventArgs e)
		{
			this.UpdateTriggerList();
			this.NotifyPropertyChanged("validate");
		}

		public TriggerSelectionTable GetTriggerSelectionTable()
		{
			return this.mTriggerSelectionTable;
		}

		private void mGridViewLogFiles_CustomDrawEmptyForeground(object sender, CustomDrawEventArgs e)
		{
			if (!(sender is GridView))
			{
				return;
			}
			GridView gridView = sender as GridView;
			if (gridView.RowCount > 0)
			{
				return;
			}
			string s;
			if (!this.mOpenIndexFile)
			{
				s = Resources.LogTableNoSourceSelected;
			}
			else
			{
				s = Resources.LogTableSelectionEmptyLogFiles;
			}
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = (stringFormat.LineAlignment = StringAlignment.Center);
			e.Graphics.DrawString(s, e.Appearance.Font, SystemBrushes.ControlDark, new RectangleF((float)e.Bounds.X, (float)e.Bounds.Y, (float)e.Bounds.Width, (float)e.Bounds.Height), stringFormat);
		}

		private void mGridViewLogFiles_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			LogFile logFile;
			if (!this.GetLogFile(e.ListSourceRowIndex, out logFile))
			{
				return;
			}
			if (e.Column == this.gridColumnLogName)
			{
				e.Value = logFile.Name;
				return;
			}
			if (e.Column == this.gridColumnLogBegin)
			{
				TimeSpec timeSpec = new TimeSpec(logFile.Begin);
				e.Value = timeSpec.DateTime;
				return;
			}
			if (e.Column == this.gridColumnLogEnd)
			{
				TimeSpec timeSpec2 = new TimeSpec(logFile.End);
				e.Value = timeSpec2.DateTime;
			}
		}

		private void mGridViewLogFiles_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
		{
			e.Handled = false;
			LogFile logFile;
			if (!this.GetLogFile(e.ListSourceRowIndex1, out logFile))
			{
				return;
			}
			LogFile logFile2;
			if (!this.GetLogFile(e.ListSourceRowIndex1, out logFile2))
			{
				return;
			}
			if (e.Column == this.gridColumnLogName)
			{
				e.Handled = true;
				int num = string.CompareOrdinal(logFile.LoggerMemNumber, logFile2.LoggerMemNumber);
				if (num == 0)
				{
					num = (int)(logFile.ID - logFile2.ID);
				}
				e.Result = num;
			}
		}

		private bool GetLogFile(int listSourceRowIndex, out LogFile logfile)
		{
			logfile = null;
			if (listSourceRowIndex < 0 || listSourceRowIndex >= this.mLogFileList.Count)
			{
				return false;
			}
			logfile = this.mLogFileList[listSourceRowIndex];
			return null != logfile;
		}

		private IList<LogFile> GetAllLogfiles()
		{
			List<LogFile> list = new List<LogFile>();
			foreach (Measurement current in this.mMeasurementList)
			{
				list.AddRange(current.GetLogFiles());
			}
			return list;
		}

		private void mTabControlLogTable_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.mTabControlLogTable.SelectedTab.Text == Resources.Measurements)
			{
				this.ExportMode = ExportType.Measurements;
			}
			else if (this.mTabControlLogTable.SelectedTab.Text == Resources.Marker)
			{
				this.ExportMode = ExportType.Marker;
			}
			else if (this.mTabControlLogTable.SelectedTab.Text == Resources.Files)
			{
				this.ExportMode = ExportType.Files;
			}
			else if (this.mTabControlLogTable.SelectedTab.Text == Resources.Trigger)
			{
				this.ExportMode = ExportType.Trigger;
			}
			this.mMarkerSelectionTable.Enable(this.ExportMode == ExportType.Marker);
			this.mTriggerSelectionTable.Enable(this.ExportMode == ExportType.Trigger);
		}

		private void NotifyPropertyChanged(string s)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(s));
			}
		}

		public void Open()
		{
			this.mOpenIndexFile = true;
		}

		public void Close()
		{
			this.mOpenIndexFile = false;
		}

		public bool IsValid()
		{
			return this.mMarkerSelectionTable.IsValid();
		}

		public LayoutSerializationContainer SerializeGrid()
		{
			LayoutSerializationContainer layoutSerializationContainer = new LayoutSerializationContainer();
			if (this.mGridViewMeasurements != null)
			{
				layoutSerializationContainer.GridViewMeasurementsLayout = LayoutSerializationContainer.SerializeGridComponent(this.mGridViewMeasurements);
			}
			if (this.mGridViewMarker != null)
			{
				layoutSerializationContainer.GridControlMarkerLayout = LayoutSerializationContainer.SerializeGridComponent(this.mGridViewMarker);
			}
			if (this.mGridViewLogFiles != null)
			{
				layoutSerializationContainer.GridControlLogFilesLayout = LayoutSerializationContainer.SerializeGridComponent(this.mGridViewLogFiles);
			}
			if (this.mGridViewTrigger != null)
			{
				layoutSerializationContainer.GridControlTriggerLayout = LayoutSerializationContainer.SerializeGridComponent(this.mGridViewTrigger);
			}
			if (this.mMarkerSelectionTable != null)
			{
				layoutSerializationContainer.GridControlMarkerSelectionTableLayout = this.mMarkerSelectionTable.SerializeGrid();
			}
			if (this.mTriggerSelectionTable != null)
			{
				layoutSerializationContainer.GridControlTriggerSelectionTableLayout = this.mTriggerSelectionTable.SerializeGrid();
			}
			return layoutSerializationContainer;
		}

		public void DeSerializeGrid(LayoutSerializationContainer layout)
		{
			if (this.mGridViewMeasurements != null)
			{
				LayoutSerializationContainer.DeSerializeGridComponent(this.mGridViewMeasurements, layout.GridViewMeasurementsLayout);
			}
			if (this.mGridViewMarker != null)
			{
				LayoutSerializationContainer.DeSerializeGridComponent(this.mGridViewMarker, layout.GridControlMarkerLayout);
			}
			if (this.mGridViewLogFiles != null)
			{
				LayoutSerializationContainer.DeSerializeGridComponent(this.mGridViewLogFiles, layout.GridControlLogFilesLayout);
			}
			if (this.mGridViewTrigger != null)
			{
				LayoutSerializationContainer.DeSerializeGridComponent(this.mGridViewTrigger, layout.GridControlTriggerLayout);
			}
			if (this.mMarkerSelectionTable != null)
			{
				this.mMarkerSelectionTable.DeSerializeGrid(layout.GridControlMarkerSelectionTableLayout);
			}
			if (this.mTriggerSelectionTable != null)
			{
				this.mTriggerSelectionTable.DeSerializeGrid(layout.GridControlTriggerSelectionTableLayout);
			}
		}

		private void repositoryItemCheckEditMeasurementSelected_CheckedChanged(object sender, EventArgs e)
		{
			if (sender is CheckEdit)
			{
				this.mGridViewMeasurements.PostEditor();
				this.NotifyPropertyChanged("validate");
			}
		}

		private void repositoryItemCheckEditMarkerSelected_CheckedChanged(object sender, EventArgs e)
		{
			if (sender is CheckEdit)
			{
				this.mGridViewMarker.PostEditor();
				this.NotifyPropertyChanged("validate");
			}
		}

		private void repositoryItemCheckEditTriggerSelected_CheckedChanged(object sender, EventArgs e)
		{
			if (sender is CheckEdit)
			{
				this.mGridViewTrigger.PostEditor();
				this.NotifyPropertyChanged("validate");
			}
		}

		private void repositoryItemCheckEditFilesSelected_CheckedChanged(object sender, EventArgs e)
		{
			if (sender is CheckEdit)
			{
				this.mGridViewLogFiles.PostEditor();
				this.NotifyPropertyChanged("validate");
			}
		}

		private void toolStripMenuItemSelectAll_Click(object sender, EventArgs e)
		{
			this.SelectItems(true);
		}

		private void toolStripMenuItemSelectNone_Click(object sender, EventArgs e)
		{
			this.SelectItems(false);
		}

		private void SelectItems(bool selected)
		{
			if (this.mExportMode == ExportType.Measurements)
			{
				foreach (Measurement current in this.mMeasurementList)
				{
					current.Selected = selected;
				}
				this.mGridControlMeasurements.RefreshDataSource();
			}
			else if (this.mExportMode == ExportType.Marker)
			{
				foreach (Marker current2 in this.mMarkerList)
				{
					current2.SelectedForExport = selected;
				}
				this.mGridControlMarker.RefreshDataSource();
			}
			else if (this.mExportMode == ExportType.Files)
			{
				foreach (LogFile current3 in this.mLogFileList)
				{
					current3.Selected = selected;
				}
				this.mGridControlLogFiles.RefreshDataSource();
			}
			else if (this.mExportMode == ExportType.Trigger)
			{
				foreach (Trigger current4 in this.mTriggerList)
				{
					current4.SelectedForExport = selected;
				}
				this.mGridControlTrigger.RefreshDataSource();
			}
			this.NotifyPropertyChanged("validate");
		}

		private void mGridViewMeasurements_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
		{
			if (e.Column == this.gridColumnName)
			{
				Measurement measurement;
				if (!this.GetMeasurement(e.ListSourceRowIndex1, out measurement))
				{
					return;
				}
				Measurement measurement2;
				if (!this.GetMeasurement(e.ListSourceRowIndex2, out measurement2))
				{
					return;
				}
				e.Handled = true;
				e.Result = measurement.Number.CompareTo(measurement.Number);
			}
		}

		private void toolTipControllerLogTable_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
		{
			if (e.SelectedControl != this.mGridControlMeasurements)
			{
				return;
			}
			GridView gridView = this.mGridControlMeasurements.GetViewAt(e.ControlMousePosition) as GridView;
			if (gridView == null)
			{
				return;
			}
			GridHitInfo gridHitInfo = gridView.CalcHitInfo(e.ControlMousePosition);
			if (gridHitInfo.InRowCell)
			{
				Measurement measurement = null;
				if (this.GetMeasurement(gridHitInfo.RowHandle, out measurement) && measurement.IsForcedClose)
				{
					object @object = gridHitInfo.Column.FieldName + gridHitInfo.RowHandle.ToString();
					string text = Resources.MeasurementRecovered;
					if (e.Info != null)
					{
						text = e.Info.Text + " / " + text;
					}
					e.Info = new ToolTipControlInfo(@object, text);
				}
			}
		}

		public OfflineSourceConfig GetOfflineSourceConfig(bool isLocalContext, bool isCANoeCANalyzer)
		{
			if (isLocalContext)
			{
				if (this.mExportMode == ExportType.Measurements)
				{
					GridHitInfo gridHitInfo = this.mGridViewMeasurements.CalcHitInfo(this.mGridControlMeasurements.PointToClient(Control.MousePosition));
					Measurement measurement = this.mGridViewMeasurements.GetRow(gridHitInfo.RowHandle) as Measurement;
					IList<LogFile> logFiles = (measurement != null) ? measurement.GetLogFiles() : new List<LogFile>();
					return this.CreateOfflineSourceConfig(logFiles, measurement);
				}
				if (this.mExportMode == ExportType.Marker)
				{
					GridHitInfo gridHitInfo2 = this.mGridViewMarker.CalcHitInfo(this.mGridControlMarker.PointToClient(Control.MousePosition));
					Marker marker = this.mGridViewMarker.GetRow(gridHitInfo2.RowHandle) as Marker;
					IList<LogFile> list = (marker != null) ? marker.GetLogFilesForExport() : new List<LogFile>();
					OfflineSourceConfig offlineSourceConfig = this.CreateOfflineSourceConfig(list, marker);
					if (list.Any<LogFile>() && marker != null)
					{
						ulong num = TimeSpec.ConvertToNanoseconds(this.mIndexManager.GetGlobalBegin(list.First<LogFile>().IndexFilePath));
						ulong num2 = TimeSpec.ConvertToNanoseconds(marker.TimeSpec);
						offlineSourceConfig.TimePosToSetAfterMeasurementEndNs = num2 - num;
					}
					return offlineSourceConfig;
				}
				if (this.mExportMode == ExportType.Files)
				{
					GridHitInfo gridHitInfo3 = this.mGridViewLogFiles.CalcHitInfo(this.mGridControlLogFiles.PointToClient(Control.MousePosition));
					LogFile logFile = this.mGridViewLogFiles.GetRow(gridHitInfo3.RowHandle) as LogFile;
					return (logFile != null) ? this.CreateOfflineSourceConfigFile(new List<LogFile>
					{
						logFile
					}) : this.CreateOfflineSourceConfigFile(new List<LogFile>());
				}
				if (this.mExportMode == ExportType.Trigger)
				{
					GridHitInfo gridHitInfo4 = this.mGridViewTrigger.CalcHitInfo(this.mGridControlTrigger.PointToClient(Control.MousePosition));
					Trigger trigger = this.mGridViewTrigger.GetRow(gridHitInfo4.RowHandle) as Trigger;
					Measurement measurement2 = (trigger != null) ? trigger.Measurement : null;
					IList<LogFile> list2 = (measurement2 != null) ? measurement2.GetLogFiles() : new List<LogFile>();
					OfflineSourceConfig offlineSourceConfig2 = this.CreateOfflineSourceConfig(list2, trigger);
					if (list2.Any<LogFile>() && trigger != null)
					{
						ulong num3 = TimeSpec.ConvertToNanoseconds(this.mIndexManager.GetGlobalBegin(list2.First<LogFile>().IndexFilePath));
						ulong num4 = TimeSpec.ConvertToNanoseconds(trigger.TimeSpec);
						offlineSourceConfig2.TimePosToSetAfterMeasurementEndNs = num4 - num3;
					}
					return offlineSourceConfig2;
				}
				return new OfflineSourceConfig();
			}
			else
			{
				OfflineSourceConfig offlineSourceConfig3 = new OfflineSourceConfig();
				IList<Measurement> measurements = this.mIndexManager.GetMeasurements();
				if (!measurements.Any<Measurement>())
				{
					return offlineSourceConfig3;
				}
				Measurement measurement3 = measurements.First<Measurement>();
				IList<LogFile> logFiles2 = measurement3.GetLogFiles();
				if (!logFiles2.Any<LogFile>())
				{
					return offlineSourceConfig3;
				}
				LogFile logFile2 = logFiles2.First<LogFile>();
				offlineSourceConfig3.OfflineSourceFiles.Add(new Tuple<string, string>(logFile2.Name, logFile2.IndexFilePath));
				if (!isCANoeCANalyzer)
				{
					offlineSourceConfig3.SetTimeSection(logFile2.GetBeginDateTime(), logFile2.GetEndDateTime());
				}
				offlineSourceConfig3.SelectionType = SelectionType.Measurement;
				offlineSourceConfig3.MeasurementName = measurement3.Name;
				offlineSourceConfig3.MeasurementNumber = measurement3.Number;
				offlineSourceConfig3.LoggerMemNumber = logFile2.LoggerMemNumber;
				return offlineSourceConfig3;
			}
		}

		private OfflineSourceConfig CreateOfflineSourceConfig(IList<LogFile> logFiles)
		{
			OfflineSourceConfig offlineSourceConfig = new OfflineSourceConfig();
			if (logFiles.Any<LogFile>())
			{
				LogFile logFile = logFiles.First<LogFile>();
				LogFile logFile2 = logFiles.Last<LogFile>();
				offlineSourceConfig.OfflineSourceFiles.Add(new Tuple<string, string>(logFile.Name, logFile.IndexFilePath));
				TimeSpec timeSpec = new TimeSpec(logFile.Begin);
				TimeSpec timeSpec2 = new TimeSpec(logFile2.End);
				offlineSourceConfig.SetTimeSection(timeSpec.DateTimeTicks, timeSpec2.DateTimeTicks);
				offlineSourceConfig.LoggerMemNumber = logFile.LoggerMemNumber;
			}
			return offlineSourceConfig;
		}

		private OfflineSourceConfig CreateOfflineSourceConfig(IList<LogFile> logFiles, Measurement measurement)
		{
			OfflineSourceConfig offlineSourceConfig = this.CreateOfflineSourceConfig(logFiles);
			offlineSourceConfig.SelectionType = SelectionType.Measurement;
			if (measurement != null)
			{
				offlineSourceConfig.MeasurementName = measurement.Name;
				offlineSourceConfig.MeasurementNumber = measurement.Number;
			}
			return offlineSourceConfig;
		}

		private OfflineSourceConfig CreateOfflineSourceConfig(IList<LogFile> logFiles, Marker marker)
		{
			OfflineSourceConfig offlineSourceConfig = this.CreateOfflineSourceConfig(logFiles);
			offlineSourceConfig.SelectionType = SelectionType.Marker;
			if (marker != null)
			{
				offlineSourceConfig.MeasurementName = marker.Name;
				offlineSourceConfig.MeasurementNumber = (int)marker.Instance;
			}
			return offlineSourceConfig;
		}

		private OfflineSourceConfig CreateOfflineSourceConfig(IList<LogFile> logFiles, Trigger trigger)
		{
			OfflineSourceConfig offlineSourceConfig = this.CreateOfflineSourceConfig(logFiles);
			offlineSourceConfig.SelectionType = SelectionType.Trigger;
			if (trigger != null)
			{
				offlineSourceConfig.MeasurementName = trigger.Name;
				offlineSourceConfig.MeasurementNumber = (int)trigger.Instance;
			}
			return offlineSourceConfig;
		}

		private OfflineSourceConfig CreateOfflineSourceConfigFile(IList<LogFile> logFiles)
		{
			OfflineSourceConfig offlineSourceConfig = this.CreateOfflineSourceConfig(logFiles);
			offlineSourceConfig.SelectionType = SelectionType.File;
			LogFile logFile = logFiles.FirstOrDefault<LogFile>();
			if (logFile != null)
			{
				offlineSourceConfig.MeasurementName = logFile.Name;
				offlineSourceConfig.MeasurementNumber = 0;
			}
			return offlineSourceConfig;
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
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LogTable));
			this.mTabControlLogTable = new TabControl();
			this.tabPageMeasurements = new TabPage();
			this.mGridControlMeasurements = new GridControl();
			this.contextMenuStripLogTable = new ContextMenuStrip(this.components);
			this.toolStripMenuItemSelectAll = new ToolStripMenuItem();
			this.toolStripMenuItemSelectNone = new ToolStripMenuItem();
			this.mGridViewMeasurements = new GridView();
			this.gridColumnSelected = new GridColumn();
			this.repositoryItemCheckEditMeasurementSelected = new RepositoryItemCheckEdit();
			this.gridColumnName = new GridColumn();
			this.gridColumnBegin = new GridColumn();
			this.gridColumnEnd = new GridColumn();
			this.gridColumnListOfLogFiles = new GridColumn();
			this.gridColumnPermanent = new GridColumn();
			this.repositoryItemCheckEditIsPermanent = new RepositoryItemCheckEdit();
			this.toolTipControllerLogTable = new ToolTipController(this.components);
			this.tabPageMarker = new TabPage();
			this.mMarkerSelectionTable = new MarkerSelectionTable();
			this.mGridControlMarker = new GridControl();
			this.mGridViewMarker = new GridView();
			this.gridColumnMarkerSelected = new GridColumn();
			this.repositoryItemCheckEditMarkerSelected = new RepositoryItemCheckEdit();
			this.gridColumnMarkerName = new GridColumn();
			this.gridColumnMarkerTime = new GridColumn();
			this.gridColumnMarkerLogFiles = new GridColumn();
			this.gridColumnMarkerLabel = new GridColumn();
			this.tabPageTrigger = new TabPage();
			this.mTriggerSelectionTable = new TriggerSelectionTable();
			this.mGridControlTrigger = new GridControl();
			this.mGridViewTrigger = new GridView();
			this.gridColumnTriggerSelected = new GridColumn();
			this.repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
			this.gridColumnTriggerName = new GridColumn();
			this.gridColumnTriggerTime = new GridColumn();
			this.gridColumnTriggerLogFiles = new GridColumn();
			this.gridColumnTriggerLabel = new GridColumn();
			this.tabPageFiles = new TabPage();
			this.mGridControlLogFiles = new GridControl();
			this.mGridViewLogFiles = new GridView();
			this.gridColumnLogSelected = new GridColumn();
			this.repositoryItemCheckEditFilesSelected = new RepositoryItemCheckEdit();
			this.gridColumnLogName = new GridColumn();
			this.gridColumnLogBegin = new GridColumn();
			this.gridColumnLogEnd = new GridColumn();
			this.gridColumnLogPermanent = new GridColumn();
			this.repositoryItemCheckEditLogFilesIsPermanent = new RepositoryItemCheckEdit();
			this.gridColumn8 = new GridColumn();
			this.mTabControlLogTable.SuspendLayout();
			this.tabPageMeasurements.SuspendLayout();
			((ISupportInitialize)this.mGridControlMeasurements).BeginInit();
			this.contextMenuStripLogTable.SuspendLayout();
			((ISupportInitialize)this.mGridViewMeasurements).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditMeasurementSelected).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsPermanent).BeginInit();
			this.tabPageMarker.SuspendLayout();
			((ISupportInitialize)this.mGridControlMarker).BeginInit();
			((ISupportInitialize)this.mGridViewMarker).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditMarkerSelected).BeginInit();
			this.tabPageTrigger.SuspendLayout();
			((ISupportInitialize)this.mGridControlTrigger).BeginInit();
			((ISupportInitialize)this.mGridViewTrigger).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEdit1).BeginInit();
			this.tabPageFiles.SuspendLayout();
			((ISupportInitialize)this.mGridControlLogFiles).BeginInit();
			((ISupportInitialize)this.mGridViewLogFiles).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditFilesSelected).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditLogFilesIsPermanent).BeginInit();
			base.SuspendLayout();
			this.mTabControlLogTable.Controls.Add(this.tabPageMeasurements);
			this.mTabControlLogTable.Controls.Add(this.tabPageMarker);
			this.mTabControlLogTable.Controls.Add(this.tabPageTrigger);
			this.mTabControlLogTable.Controls.Add(this.tabPageFiles);
			componentResourceManager.ApplyResources(this.mTabControlLogTable, "mTabControlLogTable");
			this.mTabControlLogTable.Name = "mTabControlLogTable";
			this.mTabControlLogTable.SelectedIndex = 0;
			this.mTabControlLogTable.SelectedIndexChanged += new EventHandler(this.mTabControlLogTable_SelectedIndexChanged);
			this.tabPageMeasurements.BackColor = SystemColors.Control;
			this.tabPageMeasurements.Controls.Add(this.mGridControlMeasurements);
			componentResourceManager.ApplyResources(this.tabPageMeasurements, "tabPageMeasurements");
			this.tabPageMeasurements.Name = "tabPageMeasurements";
			this.mGridControlMeasurements.ContextMenuStrip = this.contextMenuStripLogTable;
			componentResourceManager.ApplyResources(this.mGridControlMeasurements, "mGridControlMeasurements");
			this.mGridControlMeasurements.MainView = this.mGridViewMeasurements;
			this.mGridControlMeasurements.Name = "mGridControlMeasurements";
			this.mGridControlMeasurements.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemCheckEditIsPermanent,
				this.repositoryItemCheckEditMeasurementSelected
			});
			this.mGridControlMeasurements.ToolTipController = this.toolTipControllerLogTable;
			this.mGridControlMeasurements.ViewCollection.AddRange(new BaseView[]
			{
				this.mGridViewMeasurements
			});
			this.contextMenuStripLogTable.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripMenuItemSelectAll,
				this.toolStripMenuItemSelectNone
			});
			this.contextMenuStripLogTable.Name = "contextMenuStrip1";
			componentResourceManager.ApplyResources(this.contextMenuStripLogTable, "contextMenuStripLogTable");
			this.toolStripMenuItemSelectAll.Name = "toolStripMenuItemSelectAll";
			componentResourceManager.ApplyResources(this.toolStripMenuItemSelectAll, "toolStripMenuItemSelectAll");
			this.toolStripMenuItemSelectAll.Click += new EventHandler(this.toolStripMenuItemSelectAll_Click);
			this.toolStripMenuItemSelectNone.Name = "toolStripMenuItemSelectNone";
			componentResourceManager.ApplyResources(this.toolStripMenuItemSelectNone, "toolStripMenuItemSelectNone");
			this.toolStripMenuItemSelectNone.Click += new EventHandler(this.toolStripMenuItemSelectNone_Click);
			this.mGridViewMeasurements.Columns.AddRange(new GridColumn[]
			{
				this.gridColumnSelected,
				this.gridColumnName,
				this.gridColumnBegin,
				this.gridColumnEnd,
				this.gridColumnListOfLogFiles,
				this.gridColumnPermanent
			});
			this.mGridViewMeasurements.GridControl = this.mGridControlMeasurements;
			this.mGridViewMeasurements.Name = "mGridViewMeasurements";
			this.mGridViewMeasurements.OptionsView.ShowGroupPanel = false;
			this.mGridViewMeasurements.OptionsView.ShowIndicator = false;
			this.mGridViewMeasurements.PaintStyleName = "WindowsXP";
			this.mGridViewMeasurements.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.gridColumnBegin, ColumnSortOrder.Ascending)
			});
			this.mGridViewMeasurements.CustomDrawCell += new RowCellCustomDrawEventHandler(this.mGridViewMeasurements_CustomDrawCell);
			this.mGridViewMeasurements.CustomDrawEmptyForeground += new CustomDrawEventHandler(this.mGridViewMeasurements_CustomDrawEmptyForeground);
			this.mGridViewMeasurements.CustomColumnSort += new CustomColumnSortEventHandler(this.mGridViewMeasurements_CustomColumnSort);
			this.mGridViewMeasurements.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.mGridViewMeasurements_CustomUnboundColumnData);
			componentResourceManager.ApplyResources(this.gridColumnSelected, "gridColumnSelected");
			this.gridColumnSelected.ColumnEdit = this.repositoryItemCheckEditMeasurementSelected;
			this.gridColumnSelected.FieldName = "Selected";
			this.gridColumnSelected.Name = "gridColumnSelected";
			this.gridColumnSelected.OptionsColumn.ShowCaption = false;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditMeasurementSelected, "repositoryItemCheckEditMeasurementSelected");
			this.repositoryItemCheckEditMeasurementSelected.Name = "repositoryItemCheckEditMeasurementSelected";
			this.repositoryItemCheckEditMeasurementSelected.CheckedChanged += new EventHandler(this.repositoryItemCheckEditMeasurementSelected_CheckedChanged);
			componentResourceManager.ApplyResources(this.gridColumnName, "gridColumnName");
			this.gridColumnName.FieldName = "gridColumnName";
			this.gridColumnName.Name = "gridColumnName";
			this.gridColumnName.OptionsColumn.AllowEdit = false;
			this.gridColumnName.SortMode = ColumnSortMode.Custom;
			this.gridColumnName.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.gridColumnBegin, "gridColumnBegin");
			this.gridColumnBegin.DisplayFormat.FormatString = "G";
			this.gridColumnBegin.DisplayFormat.FormatType = FormatType.DateTime;
			this.gridColumnBegin.FieldName = "gridColumnBegin";
			this.gridColumnBegin.Name = "gridColumnBegin";
			this.gridColumnBegin.OptionsColumn.AllowEdit = false;
			this.gridColumnBegin.UnboundType = UnboundColumnType.DateTime;
			componentResourceManager.ApplyResources(this.gridColumnEnd, "gridColumnEnd");
			this.gridColumnEnd.DisplayFormat.FormatString = "G";
			this.gridColumnEnd.DisplayFormat.FormatType = FormatType.DateTime;
			this.gridColumnEnd.FieldName = "gridColumnEnd";
			this.gridColumnEnd.Name = "gridColumnEnd";
			this.gridColumnEnd.OptionsColumn.AllowEdit = false;
			this.gridColumnEnd.UnboundType = UnboundColumnType.DateTime;
			componentResourceManager.ApplyResources(this.gridColumnListOfLogFiles, "gridColumnListOfLogFiles");
			this.gridColumnListOfLogFiles.FieldName = "LogFileNames";
			this.gridColumnListOfLogFiles.Name = "gridColumnListOfLogFiles";
			this.gridColumnListOfLogFiles.OptionsColumn.AllowEdit = false;
			componentResourceManager.ApplyResources(this.gridColumnPermanent, "gridColumnPermanent");
			this.gridColumnPermanent.ColumnEdit = this.repositoryItemCheckEditIsPermanent;
			this.gridColumnPermanent.FieldName = "IsPermanent";
			this.gridColumnPermanent.Name = "gridColumnPermanent";
			this.gridColumnPermanent.OptionsColumn.AllowEdit = false;
			this.gridColumnPermanent.OptionsColumn.ReadOnly = true;
			this.repositoryItemCheckEditIsPermanent.AllowFocused = false;
			this.repositoryItemCheckEditIsPermanent.AllowGrayed = true;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditIsPermanent, "repositoryItemCheckEditIsPermanent");
			this.repositoryItemCheckEditIsPermanent.Name = "repositoryItemCheckEditIsPermanent";
			this.repositoryItemCheckEditIsPermanent.ReadOnly = true;
			this.toolTipControllerLogTable.GetActiveObjectInfo += new ToolTipControllerGetActiveObjectInfoEventHandler(this.toolTipControllerLogTable_GetActiveObjectInfo);
			this.tabPageMarker.BackColor = Color.Transparent;
			this.tabPageMarker.Controls.Add(this.mMarkerSelectionTable);
			this.tabPageMarker.Controls.Add(this.mGridControlMarker);
			componentResourceManager.ApplyResources(this.tabPageMarker, "tabPageMarker");
			this.tabPageMarker.Name = "tabPageMarker";
			componentResourceManager.ApplyResources(this.mMarkerSelectionTable, "mMarkerSelectionTable");
			this.mMarkerSelectionTable.BackColor = SystemColors.Control;
			this.mMarkerSelectionTable.Name = "mMarkerSelectionTable";
			componentResourceManager.ApplyResources(this.mGridControlMarker, "mGridControlMarker");
			this.mGridControlMarker.ContextMenuStrip = this.contextMenuStripLogTable;
			this.mGridControlMarker.MainView = this.mGridViewMarker;
			this.mGridControlMarker.Name = "mGridControlMarker";
			this.mGridControlMarker.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemCheckEditMarkerSelected
			});
			this.mGridControlMarker.ViewCollection.AddRange(new BaseView[]
			{
				this.mGridViewMarker
			});
			this.mGridViewMarker.Columns.AddRange(new GridColumn[]
			{
				this.gridColumnMarkerSelected,
				this.gridColumnMarkerName,
				this.gridColumnMarkerTime,
				this.gridColumnMarkerLogFiles,
				this.gridColumnMarkerLabel
			});
			this.mGridViewMarker.GridControl = this.mGridControlMarker;
			this.mGridViewMarker.Name = "mGridViewMarker";
			this.mGridViewMarker.OptionsFilter.AllowFilterEditor = false;
			this.mGridViewMarker.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.mGridViewMarker.OptionsSelection.EnableAppearanceFocusedRow = false;
			this.mGridViewMarker.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
			this.mGridViewMarker.OptionsView.ShowGroupPanel = false;
			this.mGridViewMarker.OptionsView.ShowIndicator = false;
			this.mGridViewMarker.PaintStyleName = "WindowsXP";
			this.mGridViewMarker.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.gridColumnMarkerTime, ColumnSortOrder.Ascending)
			});
			this.mGridViewMarker.CustomDrawEmptyForeground += new CustomDrawEventHandler(this.mGridViewMarker_CustomDrawEmptyForeground);
			this.mGridViewMarker.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.mGridViewMarker_CustomUnboundColumnData);
			componentResourceManager.ApplyResources(this.gridColumnMarkerSelected, "gridColumnMarkerSelected");
			this.gridColumnMarkerSelected.ColumnEdit = this.repositoryItemCheckEditMarkerSelected;
			this.gridColumnMarkerSelected.FieldName = "SelectedForExport";
			this.gridColumnMarkerSelected.Name = "gridColumnMarkerSelected";
			this.gridColumnMarkerSelected.OptionsColumn.ShowCaption = false;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditMarkerSelected, "repositoryItemCheckEditMarkerSelected");
			this.repositoryItemCheckEditMarkerSelected.Name = "repositoryItemCheckEditMarkerSelected";
			this.repositoryItemCheckEditMarkerSelected.CheckedChanged += new EventHandler(this.repositoryItemCheckEditMarkerSelected_CheckedChanged);
			componentResourceManager.ApplyResources(this.gridColumnMarkerName, "gridColumnMarkerName");
			this.gridColumnMarkerName.FieldName = "Name";
			this.gridColumnMarkerName.Name = "gridColumnMarkerName";
			this.gridColumnMarkerName.OptionsColumn.AllowEdit = false;
			this.gridColumnMarkerName.OptionsColumn.ReadOnly = true;
			componentResourceManager.ApplyResources(this.gridColumnMarkerTime, "gridColumnMarkerTime");
			this.gridColumnMarkerTime.DisplayFormat.FormatString = "G";
			this.gridColumnMarkerTime.DisplayFormat.FormatType = FormatType.DateTime;
			this.gridColumnMarkerTime.FieldName = "gridColumnMarkerTime";
			this.gridColumnMarkerTime.Name = "gridColumnMarkerTime";
			this.gridColumnMarkerTime.OptionsColumn.AllowEdit = false;
			this.gridColumnMarkerTime.OptionsColumn.ReadOnly = true;
			this.gridColumnMarkerTime.UnboundType = UnboundColumnType.DateTime;
			componentResourceManager.ApplyResources(this.gridColumnMarkerLogFiles, "gridColumnMarkerLogFiles");
			this.gridColumnMarkerLogFiles.FieldName = "gridColumnMarkerLogFiles";
			this.gridColumnMarkerLogFiles.Name = "gridColumnMarkerLogFiles";
			this.gridColumnMarkerLogFiles.OptionsColumn.AllowEdit = false;
			this.gridColumnMarkerLogFiles.OptionsColumn.ReadOnly = true;
			this.gridColumnMarkerLogFiles.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.gridColumnMarkerLabel, "gridColumnMarkerLabel");
			this.gridColumnMarkerLabel.FieldName = "gridColumnMarkerLabel";
			this.gridColumnMarkerLabel.Name = "gridColumnMarkerLabel";
			this.gridColumnMarkerLabel.OptionsColumn.AllowEdit = false;
			this.gridColumnMarkerLabel.OptionsColumn.ReadOnly = true;
			this.gridColumnMarkerLabel.UnboundType = UnboundColumnType.String;
			this.tabPageTrigger.BackColor = Color.Transparent;
			this.tabPageTrigger.Controls.Add(this.mTriggerSelectionTable);
			this.tabPageTrigger.Controls.Add(this.mGridControlTrigger);
			componentResourceManager.ApplyResources(this.tabPageTrigger, "tabPageTrigger");
			this.tabPageTrigger.Name = "tabPageTrigger";
			componentResourceManager.ApplyResources(this.mTriggerSelectionTable, "mTriggerSelectionTable");
			this.mTriggerSelectionTable.BackColor = SystemColors.Control;
			this.mTriggerSelectionTable.Name = "mTriggerSelectionTable";
			componentResourceManager.ApplyResources(this.mGridControlTrigger, "mGridControlTrigger");
			this.mGridControlTrigger.ContextMenuStrip = this.contextMenuStripLogTable;
			this.mGridControlTrigger.MainView = this.mGridViewTrigger;
			this.mGridControlTrigger.Name = "mGridControlTrigger";
			this.mGridControlTrigger.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemCheckEdit1
			});
			this.mGridControlTrigger.ViewCollection.AddRange(new BaseView[]
			{
				this.mGridViewTrigger
			});
			this.mGridViewTrigger.Columns.AddRange(new GridColumn[]
			{
				this.gridColumnTriggerSelected,
				this.gridColumnTriggerName,
				this.gridColumnTriggerTime,
				this.gridColumnTriggerLogFiles,
				this.gridColumnTriggerLabel
			});
			this.mGridViewTrigger.GridControl = this.mGridControlTrigger;
			this.mGridViewTrigger.Name = "mGridViewTrigger";
			this.mGridViewTrigger.OptionsFilter.AllowFilterEditor = false;
			this.mGridViewTrigger.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.mGridViewTrigger.OptionsSelection.EnableAppearanceFocusedRow = false;
			this.mGridViewTrigger.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
			this.mGridViewTrigger.OptionsView.ShowGroupPanel = false;
			this.mGridViewTrigger.OptionsView.ShowIndicator = false;
			this.mGridViewTrigger.PaintStyleName = "WindowsXP";
			this.mGridViewTrigger.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.gridColumnTriggerTime, ColumnSortOrder.Ascending)
			});
			this.mGridViewTrigger.CustomDrawEmptyForeground += new CustomDrawEventHandler(this.mGridViewTrigger_CustomDrawEmptyForeground);
			this.mGridViewTrigger.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.mGridViewTrigger_CustomUnboundColumnData);
			componentResourceManager.ApplyResources(this.gridColumnTriggerSelected, "gridColumnTriggerSelected");
			this.gridColumnTriggerSelected.ColumnEdit = this.repositoryItemCheckEdit1;
			this.gridColumnTriggerSelected.FieldName = "SelectedForExport";
			this.gridColumnTriggerSelected.Name = "gridColumnTriggerSelected";
			this.gridColumnTriggerSelected.OptionsColumn.ShowCaption = false;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEdit1, "repositoryItemCheckEdit1");
			this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
			this.repositoryItemCheckEdit1.CheckedChanged += new EventHandler(this.repositoryItemCheckEditTriggerSelected_CheckedChanged);
			componentResourceManager.ApplyResources(this.gridColumnTriggerName, "gridColumnTriggerName");
			this.gridColumnTriggerName.FieldName = "Name";
			this.gridColumnTriggerName.Name = "gridColumnTriggerName";
			this.gridColumnTriggerName.OptionsColumn.AllowEdit = false;
			this.gridColumnTriggerName.OptionsColumn.ReadOnly = true;
			componentResourceManager.ApplyResources(this.gridColumnTriggerTime, "gridColumnTriggerTime");
			this.gridColumnTriggerTime.DisplayFormat.FormatString = "G";
			this.gridColumnTriggerTime.DisplayFormat.FormatType = FormatType.DateTime;
			this.gridColumnTriggerTime.FieldName = "gridColumnMarkerTime";
			this.gridColumnTriggerTime.Name = "gridColumnTriggerTime";
			this.gridColumnTriggerTime.OptionsColumn.AllowEdit = false;
			this.gridColumnTriggerTime.OptionsColumn.ReadOnly = true;
			this.gridColumnTriggerTime.UnboundType = UnboundColumnType.DateTime;
			componentResourceManager.ApplyResources(this.gridColumnTriggerLogFiles, "gridColumnTriggerLogFiles");
			this.gridColumnTriggerLogFiles.FieldName = "gridColumnMarkerLogFiles";
			this.gridColumnTriggerLogFiles.Name = "gridColumnTriggerLogFiles";
			this.gridColumnTriggerLogFiles.OptionsColumn.AllowEdit = false;
			this.gridColumnTriggerLogFiles.OptionsColumn.ReadOnly = true;
			this.gridColumnTriggerLogFiles.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.gridColumnTriggerLabel, "gridColumnTriggerLabel");
			this.gridColumnTriggerLabel.FieldName = "gridColumnMarkerLabel";
			this.gridColumnTriggerLabel.Name = "gridColumnTriggerLabel";
			this.gridColumnTriggerLabel.OptionsColumn.AllowEdit = false;
			this.gridColumnTriggerLabel.OptionsColumn.ReadOnly = true;
			this.gridColumnTriggerLabel.UnboundType = UnboundColumnType.String;
			this.tabPageFiles.BackColor = Color.Transparent;
			this.tabPageFiles.Controls.Add(this.mGridControlLogFiles);
			componentResourceManager.ApplyResources(this.tabPageFiles, "tabPageFiles");
			this.tabPageFiles.Name = "tabPageFiles";
			this.mGridControlLogFiles.ContextMenuStrip = this.contextMenuStripLogTable;
			componentResourceManager.ApplyResources(this.mGridControlLogFiles, "mGridControlLogFiles");
			this.mGridControlLogFiles.MainView = this.mGridViewLogFiles;
			this.mGridControlLogFiles.Name = "mGridControlLogFiles";
			this.mGridControlLogFiles.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemCheckEditLogFilesIsPermanent,
				this.repositoryItemCheckEditFilesSelected
			});
			this.mGridControlLogFiles.ViewCollection.AddRange(new BaseView[]
			{
				this.mGridViewLogFiles
			});
			this.mGridViewLogFiles.Columns.AddRange(new GridColumn[]
			{
				this.gridColumnLogSelected,
				this.gridColumnLogName,
				this.gridColumnLogBegin,
				this.gridColumnLogEnd,
				this.gridColumnLogPermanent
			});
			this.mGridViewLogFiles.GridControl = this.mGridControlLogFiles;
			this.mGridViewLogFiles.Name = "mGridViewLogFiles";
			this.mGridViewLogFiles.OptionsView.ShowGroupPanel = false;
			this.mGridViewLogFiles.OptionsView.ShowIndicator = false;
			this.mGridViewLogFiles.PaintStyleName = "WindowsXP";
			this.mGridViewLogFiles.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.gridColumnLogName, ColumnSortOrder.Ascending)
			});
			this.mGridViewLogFiles.CustomDrawEmptyForeground += new CustomDrawEventHandler(this.mGridViewLogFiles_CustomDrawEmptyForeground);
			this.mGridViewLogFiles.CustomColumnSort += new CustomColumnSortEventHandler(this.mGridViewLogFiles_CustomColumnSort);
			this.mGridViewLogFiles.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.mGridViewLogFiles_CustomUnboundColumnData);
			componentResourceManager.ApplyResources(this.gridColumnLogSelected, "gridColumnLogSelected");
			this.gridColumnLogSelected.ColumnEdit = this.repositoryItemCheckEditFilesSelected;
			this.gridColumnLogSelected.FieldName = "Selected";
			this.gridColumnLogSelected.Name = "gridColumnLogSelected";
			this.gridColumnLogSelected.OptionsColumn.ShowCaption = false;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditFilesSelected, "repositoryItemCheckEditFilesSelected");
			this.repositoryItemCheckEditFilesSelected.Name = "repositoryItemCheckEditFilesSelected";
			this.repositoryItemCheckEditFilesSelected.CheckedChanged += new EventHandler(this.repositoryItemCheckEditFilesSelected_CheckedChanged);
			componentResourceManager.ApplyResources(this.gridColumnLogName, "gridColumnLogName");
			this.gridColumnLogName.FieldName = "gridColumnName";
			this.gridColumnLogName.Name = "gridColumnLogName";
			this.gridColumnLogName.OptionsColumn.AllowEdit = false;
			this.gridColumnLogName.SortMode = ColumnSortMode.Custom;
			this.gridColumnLogName.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.gridColumnLogBegin, "gridColumnLogBegin");
			this.gridColumnLogBegin.DisplayFormat.FormatString = "G";
			this.gridColumnLogBegin.DisplayFormat.FormatType = FormatType.DateTime;
			this.gridColumnLogBegin.FieldName = "gridColumnBegin";
			this.gridColumnLogBegin.Name = "gridColumnLogBegin";
			this.gridColumnLogBegin.OptionsColumn.AllowEdit = false;
			this.gridColumnLogBegin.UnboundType = UnboundColumnType.DateTime;
			componentResourceManager.ApplyResources(this.gridColumnLogEnd, "gridColumnLogEnd");
			this.gridColumnLogEnd.DisplayFormat.FormatString = "G";
			this.gridColumnLogEnd.DisplayFormat.FormatType = FormatType.DateTime;
			this.gridColumnLogEnd.FieldName = "gridColumnEnd";
			this.gridColumnLogEnd.Name = "gridColumnLogEnd";
			this.gridColumnLogEnd.OptionsColumn.AllowEdit = false;
			this.gridColumnLogEnd.UnboundType = UnboundColumnType.DateTime;
			componentResourceManager.ApplyResources(this.gridColumnLogPermanent, "gridColumnLogPermanent");
			this.gridColumnLogPermanent.ColumnEdit = this.repositoryItemCheckEditLogFilesIsPermanent;
			this.gridColumnLogPermanent.FieldName = "IsPermanent";
			this.gridColumnLogPermanent.Name = "gridColumnLogPermanent";
			this.gridColumnLogPermanent.OptionsColumn.AllowEdit = false;
			this.gridColumnLogPermanent.OptionsColumn.ReadOnly = true;
			this.repositoryItemCheckEditLogFilesIsPermanent.AllowGrayed = true;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditLogFilesIsPermanent, "repositoryItemCheckEditLogFilesIsPermanent");
			this.repositoryItemCheckEditLogFilesIsPermanent.Name = "repositoryItemCheckEditLogFilesIsPermanent";
			this.repositoryItemCheckEditLogFilesIsPermanent.ReadOnly = true;
			componentResourceManager.ApplyResources(this.gridColumn8, "gridColumn8");
			this.gridColumn8.Name = "gridColumn8";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.mTabControlLogTable);
			base.Name = "LogTable";
			this.mTabControlLogTable.ResumeLayout(false);
			this.tabPageMeasurements.ResumeLayout(false);
			((ISupportInitialize)this.mGridControlMeasurements).EndInit();
			this.contextMenuStripLogTable.ResumeLayout(false);
			((ISupportInitialize)this.mGridViewMeasurements).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditMeasurementSelected).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsPermanent).EndInit();
			this.tabPageMarker.ResumeLayout(false);
			((ISupportInitialize)this.mGridControlMarker).EndInit();
			((ISupportInitialize)this.mGridViewMarker).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditMarkerSelected).EndInit();
			this.tabPageTrigger.ResumeLayout(false);
			((ISupportInitialize)this.mGridControlTrigger).EndInit();
			((ISupportInitialize)this.mGridViewTrigger).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEdit1).EndInit();
			this.tabPageFiles.ResumeLayout(false);
			((ISupportInitialize)this.mGridControlLogFiles).EndInit();
			((ISupportInitialize)this.mGridViewLogFiles).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditFilesSelected).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditLogFilesIsPermanent).EndInit();
			base.ResumeLayout(false);
		}
	}
}
