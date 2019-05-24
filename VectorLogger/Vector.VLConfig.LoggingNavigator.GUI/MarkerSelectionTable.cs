using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.LoggingNavigator.Data;
using Vector.VLConfig.LoggingNavigator.Properties;

namespace Vector.VLConfig.LoggingNavigator.GUI
{
	public class MarkerSelectionTable : UserControl
	{
		private IList<Marker> mMarkerFullList;

		private IList<Marker> mMarkerUniqueList;

		private Dictionary<uint, IList<Marker>> mMarkerMap;

		private ulong mMarkerBefore;

		private ulong mMarkerAfter;

		private static ulong sMarkerOffsetMaximum = 100000uL;

		private IContainer components;

		private GridControl mGridControlMarkerSelection;

		private GridView mGridViewMarkerSelection;

		private GridColumn gridColumnName;

		private GridColumn gridColumnID;

		private GridColumn gridColumnLabel;

		private GridColumn gridColumnSelected;

		private RepositoryItemCheckEdit repositoryItemCheckEdit1;

		private GroupBox groupBox1;

		private Button mButtonSelectSame;

		private System.Windows.Forms.ComboBox mComboBoxMarkerAfterUnit;

		private System.Windows.Forms.ComboBox mComboBoxMarkerBeforeUnit;

		private Button mButtonSelectNone;

		private Button mButtonSelectAll;

		private Label label2;

		private Label label1;

		private TextBox mTextBoxMarkerBefore;

		private TextBox mTextBoxMarkerAfter;

		private RepositoryItemCheckEdit repositoryItemCheckEdit2;

		private GridColumn gridColumnCount;

		private ErrorProvider mErrorProviderMarkerTypeFilter;

		public event EventHandler Changed;

		public MarkerSelectionTable()
		{
			this.InitializeComponent();
			this.mComboBoxMarkerBeforeUnit.SelectedIndex = 0;
			this.mComboBoxMarkerAfterUnit.SelectedIndex = 0;
			this.mMarkerMap = new Dictionary<uint, IList<Marker>>();
		}

		public void SetData(IList<Marker> list)
		{
			this.mMarkerFullList = list;
			this.mMarkerUniqueList = new List<Marker>();
			this.mMarkerMap.Clear();
			foreach (Marker current in list)
			{
				if (!this.mMarkerMap.ContainsKey(current.Type))
				{
					this.mMarkerMap[current.Type] = new List<Marker>();
					this.mMarkerUniqueList.Add(current);
				}
				this.mMarkerMap[current.Type].Add(current);
			}
			this.mGridControlMarkerSelection.DataSource = this.mMarkerUniqueList;
			this.mGridControlMarkerSelection.RefreshDataSource();
		}

		private void gridViewMarkerSelection_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			Marker marker;
			if (!this.GetMarker(e.ListSourceRowIndex, out marker))
			{
				return;
			}
			if (e.Column == this.gridColumnCount)
			{
				int num = 0;
				if (this.mMarkerMap.ContainsKey(marker.Type))
				{
					num = this.mMarkerMap[marker.Type].Count;
				}
				e.Value = num;
			}
		}

		private bool GetMarker(int listSourceRowIndex, out Marker marker)
		{
			marker = null;
			if (listSourceRowIndex < 0 || listSourceRowIndex >= this.mMarkerUniqueList.Count)
			{
				return false;
			}
			marker = this.mMarkerUniqueList[listSourceRowIndex];
			return null != marker;
		}

		public void SelectAll(bool selected)
		{
			foreach (Marker current in this.mMarkerFullList)
			{
				current.Selected = selected;
			}
			this.mGridControlMarkerSelection.RefreshDataSource();
		}

		public void SelectSameType(bool selected)
		{
			object focusedRow = this.mGridViewMarkerSelection.GetFocusedRow();
			if (!(focusedRow is Marker))
			{
				return;
			}
			Marker marker = (Marker)focusedRow;
			foreach (Marker current in this.mMarkerFullList)
			{
				if (marker.Type == current.Type)
				{
					current.Selected = selected;
				}
			}
			this.mGridControlMarkerSelection.RefreshDataSource();
		}

		protected virtual void OnChanged()
		{
			if (this.Changed != null)
			{
				this.Changed(this, EventArgs.Empty);
			}
		}

		private void repositoryItemCheckEdit2_CheckedChanged(object sender, EventArgs e)
		{
			if (sender is CheckEdit)
			{
				this.mGridViewMarkerSelection.PostEditor();
				this.UpdateMarkerSelection();
				this.OnChanged();
			}
		}

		private void UpdateMarkerSelection()
		{
			object focusedRow = this.mGridViewMarkerSelection.GetFocusedRow();
			if (!(focusedRow is Marker))
			{
				return;
			}
			Marker marker = (Marker)focusedRow;
			foreach (Marker current in this.mMarkerMap[marker.Type])
			{
				current.Selected = marker.Selected;
			}
		}

		public string SerializeGrid()
		{
			return LayoutSerializationContainer.SerializeGridComponent(this.mGridViewMarkerSelection);
		}

		public void DeSerializeGrid(string layout)
		{
			LayoutSerializationContainer.DeSerializeGridComponent(this.mGridViewMarkerSelection, layout);
		}

		public IEnumerable<string> RestoreMarkerTypeSelection(IEnumerable<string> markerList)
		{
			this.mGridControlMarkerSelection.FocusedView.CloseEditor();
			this.SelectAll(false);
			IList<string> list = new List<string>();
			foreach (string current in markerList)
			{
				bool flag = false;
				foreach (Marker current2 in this.mMarkerFullList)
				{
					if (current2.Name == current)
					{
						current2.Selected = (current2.SelectedForExport = true);
						flag = true;
					}
				}
				if (!flag)
				{
					list.Add(current);
				}
			}
			this.mGridControlMarkerSelection.RefreshDataSource();
			this.OnChanged();
			return list;
		}

		public IEnumerable<string> GetMarkerTypeSelection()
		{
			IList<string> list = new List<string>();
			foreach (Marker current in this.mMarkerUniqueList)
			{
				if (current.Selected && !list.Contains(current.Name))
				{
					list.Add(current.Name);
				}
			}
			return list;
		}

		private void mButtonSelectAll_Click(object sender, EventArgs e)
		{
			this.SelectAll(true);
			this.OnChanged();
		}

		private void mButtonSelectNone_Click(object sender, EventArgs e)
		{
			this.SelectAll(false);
			this.OnChanged();
		}

		private void mButtonSelectSame_Click(object sender, EventArgs e)
		{
			this.SelectSameType(true);
			this.OnChanged();
		}

		private void MarkerOffsetSelectionChanged(object sender, EventArgs e)
		{
			bool flag = true;
			ulong num = 0uL;
			string text = this.mComboBoxMarkerBeforeUnit.Text;
			ulong num2 = 0uL;
			string text2 = this.mComboBoxMarkerAfterUnit.Text;
			try
			{
				num = Convert.ToUInt64(this.mTextBoxMarkerBefore.Text);
				this.mErrorProviderMarkerTypeFilter.SetError(this.mTextBoxMarkerBefore, "");
				if (num > MarkerSelectionTable.sMarkerOffsetMaximum)
				{
					this.mErrorProviderMarkerTypeFilter.SetError(this.mTextBoxMarkerBefore, string.Format(Resources.MarkerOffsetTooLarge, MarkerSelectionTable.sMarkerOffsetMaximum));
				}
			}
			catch (Exception)
			{
				this.mErrorProviderMarkerTypeFilter.SetError(this.mTextBoxMarkerBefore, Resources.MarkerOffsetNaN);
				flag = false;
			}
			try
			{
				num2 = Convert.ToUInt64(this.mTextBoxMarkerAfter.Text);
				this.mErrorProviderMarkerTypeFilter.SetError(this.mTextBoxMarkerAfter, "");
				if (num2 > MarkerSelectionTable.sMarkerOffsetMaximum)
				{
					this.mErrorProviderMarkerTypeFilter.SetError(this.mTextBoxMarkerAfter, string.Format(Resources.MarkerOffsetTooLarge, MarkerSelectionTable.sMarkerOffsetMaximum));
				}
			}
			catch (Exception)
			{
				this.mErrorProviderMarkerTypeFilter.SetError(this.mTextBoxMarkerAfter, Resources.MarkerOffsetNaN);
				flag = false;
			}
			if (flag)
			{
				Utils.CalculateAndUpdateMarkerOffsets(num, text, num2, text2, out this.mMarkerBefore, out this.mMarkerAfter);
			}
			this.OnChanged();
		}

		public bool IsValid()
		{
			return this.mErrorProviderMarkerTypeFilter.GetError(this.mTextBoxMarkerBefore).Length <= 0 && this.mErrorProviderMarkerTypeFilter.GetError(this.mTextBoxMarkerAfter).Length <= 0;
		}

		public ulong GetMarkerBefore()
		{
			return this.mMarkerBefore;
		}

		public ulong GetMarkerAfter()
		{
			return this.mMarkerAfter;
		}

		public void Enable(bool enableMarkerSelection)
		{
			this.mGridControlMarkerSelection.Enabled = enableMarkerSelection;
			this.mTextBoxMarkerAfter.Enabled = enableMarkerSelection;
			this.mTextBoxMarkerBefore.Enabled = enableMarkerSelection;
			this.mComboBoxMarkerBeforeUnit.Enabled = enableMarkerSelection;
			this.mComboBoxMarkerAfterUnit.Enabled = enableMarkerSelection;
			this.mButtonSelectAll.Enabled = enableMarkerSelection;
			this.mButtonSelectNone.Enabled = enableMarkerSelection;
			this.mButtonSelectSame.Enabled = enableMarkerSelection;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MarkerSelectionTable));
			this.repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
			this.mGridControlMarkerSelection = new GridControl();
			this.mGridViewMarkerSelection = new GridView();
			this.gridColumnSelected = new GridColumn();
			this.repositoryItemCheckEdit2 = new RepositoryItemCheckEdit();
			this.gridColumnName = new GridColumn();
			this.gridColumnLabel = new GridColumn();
			this.gridColumnCount = new GridColumn();
			this.gridColumnID = new GridColumn();
			this.groupBox1 = new GroupBox();
			this.mButtonSelectSame = new Button();
			this.mComboBoxMarkerAfterUnit = new System.Windows.Forms.ComboBox();
			this.mComboBoxMarkerBeforeUnit = new System.Windows.Forms.ComboBox();
			this.mButtonSelectNone = new Button();
			this.mButtonSelectAll = new Button();
			this.label2 = new Label();
			this.label1 = new Label();
			this.mTextBoxMarkerBefore = new TextBox();
			this.mTextBoxMarkerAfter = new TextBox();
			this.mErrorProviderMarkerTypeFilter = new ErrorProvider();
			((ISupportInitialize)this.repositoryItemCheckEdit1).BeginInit();
			((ISupportInitialize)this.mGridControlMarkerSelection).BeginInit();
			((ISupportInitialize)this.mGridViewMarkerSelection).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEdit2).BeginInit();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.mErrorProviderMarkerTypeFilter).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.repositoryItemCheckEdit1, "repositoryItemCheckEdit1");
			this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
			componentResourceManager.ApplyResources(this.mGridControlMarkerSelection, "mGridControlMarkerSelection");
			this.mGridControlMarkerSelection.MainView = this.mGridViewMarkerSelection;
			this.mGridControlMarkerSelection.Name = "mGridControlMarkerSelection";
			this.mGridControlMarkerSelection.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemCheckEdit2
			});
			this.mGridControlMarkerSelection.ViewCollection.AddRange(new BaseView[]
			{
				this.mGridViewMarkerSelection
			});
			this.mGridViewMarkerSelection.Columns.AddRange(new GridColumn[]
			{
				this.gridColumnSelected,
				this.gridColumnName,
				this.gridColumnLabel,
				this.gridColumnCount,
				this.gridColumnID
			});
			this.mGridViewMarkerSelection.GridControl = this.mGridControlMarkerSelection;
			this.mGridViewMarkerSelection.Name = "mGridViewMarkerSelection";
			this.mGridViewMarkerSelection.OptionsView.ShowGroupPanel = false;
			this.mGridViewMarkerSelection.OptionsView.ShowIndicator = false;
			this.mGridViewMarkerSelection.PaintStyleName = "WindowsXP";
			this.mGridViewMarkerSelection.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.gridColumnLabel, ColumnSortOrder.Ascending)
			});
			this.mGridViewMarkerSelection.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gridViewMarkerSelection_CustomUnboundColumnData);
			componentResourceManager.ApplyResources(this.gridColumnSelected, "gridColumnSelected");
			this.gridColumnSelected.ColumnEdit = this.repositoryItemCheckEdit2;
			this.gridColumnSelected.FieldName = "Selected";
			this.gridColumnSelected.MinWidth = 25;
			this.gridColumnSelected.Name = "gridColumnSelected";
			this.gridColumnSelected.OptionsColumn.ShowCaption = false;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEdit2, "repositoryItemCheckEdit2");
			this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
			this.repositoryItemCheckEdit2.CheckedChanged += new EventHandler(this.repositoryItemCheckEdit2_CheckedChanged);
			componentResourceManager.ApplyResources(this.gridColumnName, "gridColumnName");
			this.gridColumnName.FieldName = "Name";
			this.gridColumnName.Name = "gridColumnName";
			this.gridColumnName.OptionsColumn.AllowEdit = false;
			this.gridColumnName.OptionsColumn.ReadOnly = true;
			componentResourceManager.ApplyResources(this.gridColumnLabel, "gridColumnLabel");
			this.gridColumnLabel.FieldName = "Label";
			this.gridColumnLabel.Name = "gridColumnLabel";
			this.gridColumnLabel.OptionsColumn.AllowEdit = false;
			this.gridColumnLabel.OptionsColumn.ReadOnly = true;
			componentResourceManager.ApplyResources(this.gridColumnCount, "gridColumnCount");
			this.gridColumnCount.FieldName = "gridColumn1";
			this.gridColumnCount.Name = "gridColumnCount";
			this.gridColumnCount.OptionsColumn.AllowEdit = false;
			this.gridColumnCount.OptionsColumn.ReadOnly = true;
			this.gridColumnCount.UnboundType = UnboundColumnType.Integer;
			componentResourceManager.ApplyResources(this.gridColumnID, "gridColumnID");
			this.gridColumnID.DisplayFormat.FormatString = "X";
			this.gridColumnID.DisplayFormat.FormatType = FormatType.Numeric;
			this.gridColumnID.FieldName = "ID";
			this.gridColumnID.Name = "gridColumnID";
			this.gridColumnID.OptionsColumn.AllowEdit = false;
			this.gridColumnID.OptionsColumn.ReadOnly = true;
			this.groupBox1.Controls.Add(this.mButtonSelectSame);
			this.groupBox1.Controls.Add(this.mGridControlMarkerSelection);
			this.groupBox1.Controls.Add(this.mComboBoxMarkerAfterUnit);
			this.groupBox1.Controls.Add(this.mComboBoxMarkerBeforeUnit);
			this.groupBox1.Controls.Add(this.mButtonSelectNone);
			this.groupBox1.Controls.Add(this.mButtonSelectAll);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.mTextBoxMarkerBefore);
			this.groupBox1.Controls.Add(this.mTextBoxMarkerAfter);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.mButtonSelectSame, "mButtonSelectSame");
			this.mButtonSelectSame.Name = "mButtonSelectSame";
			this.mButtonSelectSame.UseVisualStyleBackColor = true;
			this.mButtonSelectSame.Click += new EventHandler(this.mButtonSelectSame_Click);
			componentResourceManager.ApplyResources(this.mComboBoxMarkerAfterUnit, "mComboBoxMarkerAfterUnit");
			this.mComboBoxMarkerAfterUnit.DropDownStyle = ComboBoxStyle.DropDownList;
			this.mComboBoxMarkerAfterUnit.FormattingEnabled = true;
			this.mComboBoxMarkerAfterUnit.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("mComboBoxMarkerAfterUnit.Items"),
				componentResourceManager.GetString("mComboBoxMarkerAfterUnit.Items1"),
				componentResourceManager.GetString("mComboBoxMarkerAfterUnit.Items2")
			});
			this.mComboBoxMarkerAfterUnit.Name = "mComboBoxMarkerAfterUnit";
			this.mComboBoxMarkerAfterUnit.SelectedValueChanged += new EventHandler(this.MarkerOffsetSelectionChanged);
			componentResourceManager.ApplyResources(this.mComboBoxMarkerBeforeUnit, "mComboBoxMarkerBeforeUnit");
			this.mComboBoxMarkerBeforeUnit.DropDownStyle = ComboBoxStyle.DropDownList;
			this.mComboBoxMarkerBeforeUnit.FormattingEnabled = true;
			this.mComboBoxMarkerBeforeUnit.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("mComboBoxMarkerBeforeUnit.Items"),
				componentResourceManager.GetString("mComboBoxMarkerBeforeUnit.Items1"),
				componentResourceManager.GetString("mComboBoxMarkerBeforeUnit.Items2")
			});
			this.mComboBoxMarkerBeforeUnit.Name = "mComboBoxMarkerBeforeUnit";
			this.mComboBoxMarkerBeforeUnit.SelectedValueChanged += new EventHandler(this.MarkerOffsetSelectionChanged);
			componentResourceManager.ApplyResources(this.mButtonSelectNone, "mButtonSelectNone");
			this.mButtonSelectNone.Name = "mButtonSelectNone";
			this.mButtonSelectNone.UseVisualStyleBackColor = true;
			this.mButtonSelectNone.Click += new EventHandler(this.mButtonSelectNone_Click);
			componentResourceManager.ApplyResources(this.mButtonSelectAll, "mButtonSelectAll");
			this.mButtonSelectAll.Name = "mButtonSelectAll";
			this.mButtonSelectAll.UseVisualStyleBackColor = true;
			this.mButtonSelectAll.Click += new EventHandler(this.mButtonSelectAll_Click);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.mTextBoxMarkerBefore, "mTextBoxMarkerBefore");
			this.mErrorProviderMarkerTypeFilter.SetIconAlignment(this.mTextBoxMarkerBefore, (ErrorIconAlignment)componentResourceManager.GetObject("mTextBoxMarkerBefore.IconAlignment"));
			this.mTextBoxMarkerBefore.Name = "mTextBoxMarkerBefore";
			this.mTextBoxMarkerBefore.Leave += new EventHandler(this.MarkerOffsetSelectionChanged);
			componentResourceManager.ApplyResources(this.mTextBoxMarkerAfter, "mTextBoxMarkerAfter");
			this.mErrorProviderMarkerTypeFilter.SetIconAlignment(this.mTextBoxMarkerAfter, (ErrorIconAlignment)componentResourceManager.GetObject("mTextBoxMarkerAfter.IconAlignment"));
			this.mTextBoxMarkerAfter.Name = "mTextBoxMarkerAfter";
			this.mTextBoxMarkerAfter.Leave += new EventHandler(this.MarkerOffsetSelectionChanged);
			this.mErrorProviderMarkerTypeFilter.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.mErrorProviderMarkerTypeFilter.ContainerControl = this;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBox1);
			base.Name = "MarkerSelectionTable";
			((ISupportInitialize)this.repositoryItemCheckEdit1).EndInit();
			((ISupportInitialize)this.mGridControlMarkerSelection).EndInit();
			((ISupportInitialize)this.mGridViewMarkerSelection).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEdit2).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((ISupportInitialize)this.mErrorProviderMarkerTypeFilter).EndInit();
			base.ResumeLayout(false);
		}
	}
}
