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

namespace Vector.VLConfig.LoggingNavigator.GUI
{
	public class TriggerSelectionTable : UserControl
	{
		private IList<Trigger> mTriggerFullList;

		private IList<Trigger> mTriggerUniqueList;

		private Dictionary<uint, IList<Trigger>> mTriggerMap;

		private IContainer components;

		private GridControl mGridControlTriggerSelection;

		private GridView mGridViewTriggerSelection;

		private GridColumn gridColumnName;

		private GridColumn gridColumnID;

		private GridColumn gridColumnLabel;

		private GridColumn gridColumnSelected;

		private RepositoryItemCheckEdit repositoryItemCheckEdit1;

		private GroupBox groupBox1;

		private Button mButtonSelectNone;

		private Button mButtonSelectAll;

		private RepositoryItemCheckEdit repositoryItemCheckEdit2;

		private GridColumn gridColumnCount;

		private ErrorProvider mErrorProviderMarkerTypeFilter;

		public event EventHandler Changed;

		public TriggerSelectionTable()
		{
			this.InitializeComponent();
			this.mTriggerMap = new Dictionary<uint, IList<Trigger>>();
		}

		public void SetData(IList<Trigger> list)
		{
			this.mTriggerFullList = list;
			this.mTriggerUniqueList = new List<Trigger>();
			this.mTriggerMap.Clear();
			foreach (Trigger current in list)
			{
				if (!this.mTriggerMap.ContainsKey(current.Type))
				{
					this.mTriggerMap[current.Type] = new List<Trigger>();
					this.mTriggerUniqueList.Add(current);
				}
				this.mTriggerMap[current.Type].Add(current);
			}
			this.mGridControlTriggerSelection.DataSource = this.mTriggerUniqueList;
			this.mGridControlTriggerSelection.RefreshDataSource();
		}

		private void gridViewTriggerSelection_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			Trigger trigger;
			if (!this.GetTrigger(e.ListSourceRowIndex, out trigger))
			{
				return;
			}
			if (e.Column == this.gridColumnCount)
			{
				int num = 0;
				if (this.mTriggerMap.ContainsKey(trigger.Type))
				{
					num = this.mTriggerMap[trigger.Type].Count;
				}
				e.Value = num;
			}
		}

		private bool GetTrigger(int listSourceRowIndex, out Trigger trigger)
		{
			trigger = null;
			if (listSourceRowIndex < 0 || listSourceRowIndex >= this.mTriggerUniqueList.Count)
			{
				return false;
			}
			trigger = this.mTriggerUniqueList[listSourceRowIndex];
			return null != trigger;
		}

		public void SelectAll(bool selected)
		{
			foreach (Trigger current in this.mTriggerFullList)
			{
				current.Selected = selected;
			}
			this.mGridControlTriggerSelection.RefreshDataSource();
		}

		public void SelectSameType(bool selected)
		{
			object focusedRow = this.mGridViewTriggerSelection.GetFocusedRow();
			if (!(focusedRow is Trigger))
			{
				return;
			}
			Trigger trigger = (Trigger)focusedRow;
			foreach (Trigger current in this.mTriggerFullList)
			{
				if (trigger.Type == current.Type)
				{
					current.Selected = selected;
				}
			}
			this.mGridControlTriggerSelection.RefreshDataSource();
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
				this.mGridViewTriggerSelection.PostEditor();
				this.UpdateTriggerSelection();
				this.OnChanged();
			}
		}

		private void UpdateTriggerSelection()
		{
			object focusedRow = this.mGridViewTriggerSelection.GetFocusedRow();
			if (!(focusedRow is Trigger))
			{
				return;
			}
			Trigger trigger = (Trigger)focusedRow;
			foreach (Trigger current in this.mTriggerMap[trigger.Type])
			{
				current.Selected = trigger.Selected;
			}
		}

		public string SerializeGrid()
		{
			return LayoutSerializationContainer.SerializeGridComponent(this.mGridViewTriggerSelection);
		}

		public void DeSerializeGrid(string layout)
		{
			LayoutSerializationContainer.DeSerializeGridComponent(this.mGridViewTriggerSelection, layout);
		}

		public IEnumerable<string> RestoreTriggerTypeSelection(IEnumerable<string> triggerTypeList)
		{
			this.mGridControlTriggerSelection.FocusedView.CloseEditor();
			this.SelectAll(false);
			IList<string> list = new List<string>();
			foreach (string current in triggerTypeList)
			{
				bool flag = false;
				foreach (Trigger current2 in this.mTriggerFullList)
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
			this.mGridControlTriggerSelection.RefreshDataSource();
			this.OnChanged();
			return list;
		}

		public IEnumerable<string> GetTriggerTypeSelection()
		{
			IList<string> list = new List<string>();
			foreach (Trigger current in this.mTriggerUniqueList)
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

		public bool IsValid()
		{
			return true;
		}

		public void Enable(bool enableTriggerSelection)
		{
			this.mGridControlTriggerSelection.Enabled = enableTriggerSelection;
			this.mButtonSelectAll.Enabled = enableTriggerSelection;
			this.mButtonSelectNone.Enabled = enableTriggerSelection;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(TriggerSelectionTable));
			this.repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
			this.mGridControlTriggerSelection = new GridControl();
			this.mGridViewTriggerSelection = new GridView();
			this.gridColumnSelected = new GridColumn();
			this.repositoryItemCheckEdit2 = new RepositoryItemCheckEdit();
			this.gridColumnName = new GridColumn();
			this.gridColumnLabel = new GridColumn();
			this.gridColumnCount = new GridColumn();
			this.gridColumnID = new GridColumn();
			this.groupBox1 = new GroupBox();
			this.mButtonSelectNone = new Button();
			this.mButtonSelectAll = new Button();
			this.mErrorProviderMarkerTypeFilter = new ErrorProvider(this.components);
			((ISupportInitialize)this.repositoryItemCheckEdit1).BeginInit();
			((ISupportInitialize)this.mGridControlTriggerSelection).BeginInit();
			((ISupportInitialize)this.mGridViewTriggerSelection).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEdit2).BeginInit();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.mErrorProviderMarkerTypeFilter).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.repositoryItemCheckEdit1, "repositoryItemCheckEdit1");
			this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
			componentResourceManager.ApplyResources(this.mGridControlTriggerSelection, "mGridControlTriggerSelection");
			this.mGridControlTriggerSelection.MainView = this.mGridViewTriggerSelection;
			this.mGridControlTriggerSelection.Name = "mGridControlTriggerSelection";
			this.mGridControlTriggerSelection.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemCheckEdit2
			});
			this.mGridControlTriggerSelection.ViewCollection.AddRange(new BaseView[]
			{
				this.mGridViewTriggerSelection
			});
			this.mGridViewTriggerSelection.Columns.AddRange(new GridColumn[]
			{
				this.gridColumnSelected,
				this.gridColumnName,
				this.gridColumnLabel,
				this.gridColumnCount,
				this.gridColumnID
			});
			this.mGridViewTriggerSelection.GridControl = this.mGridControlTriggerSelection;
			this.mGridViewTriggerSelection.Name = "mGridViewTriggerSelection";
			this.mGridViewTriggerSelection.OptionsView.ShowGroupPanel = false;
			this.mGridViewTriggerSelection.OptionsView.ShowIndicator = false;
			this.mGridViewTriggerSelection.PaintStyleName = "WindowsXP";
			this.mGridViewTriggerSelection.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.gridColumnLabel, ColumnSortOrder.Ascending)
			});
			this.mGridViewTriggerSelection.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gridViewTriggerSelection_CustomUnboundColumnData);
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
			this.groupBox1.Controls.Add(this.mGridControlTriggerSelection);
			this.groupBox1.Controls.Add(this.mButtonSelectNone);
			this.groupBox1.Controls.Add(this.mButtonSelectAll);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.mButtonSelectNone, "mButtonSelectNone");
			this.mButtonSelectNone.Name = "mButtonSelectNone";
			this.mButtonSelectNone.UseVisualStyleBackColor = true;
			this.mButtonSelectNone.Click += new EventHandler(this.mButtonSelectNone_Click);
			componentResourceManager.ApplyResources(this.mButtonSelectAll, "mButtonSelectAll");
			this.mButtonSelectAll.Name = "mButtonSelectAll";
			this.mButtonSelectAll.UseVisualStyleBackColor = true;
			this.mButtonSelectAll.Click += new EventHandler(this.mButtonSelectAll_Click);
			this.mErrorProviderMarkerTypeFilter.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.mErrorProviderMarkerTypeFilter.ContainerControl = this;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBox1);
			base.Name = "TriggerSelectionTable";
			((ISupportInitialize)this.repositoryItemCheckEdit1).EndInit();
			((ISupportInitialize)this.mGridControlTriggerSelection).EndInit();
			((ISupportInitialize)this.mGridViewTriggerSelection).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEdit2).EndInit();
			this.groupBox1.ResumeLayout(false);
			((ISupportInitialize)this.mErrorProviderMarkerTypeFilter).EndInit();
			base.ResumeLayout(false);
		}
	}
}
