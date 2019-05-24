using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.DiagnosticsDatabasesPage
{
	public class SelectECUs : Form
	{
		private Dictionary<string, bool> ecuQualifier2IsActive;

		private Dictionary<string, string> displayName2EcuQualifier;

		private bool isAllItemsAction;

		private IContainer components;

		private CheckedListBox checkedListBoxECUs;

		private Button buttonOK;

		private Button buttonCancel;

		private Label labelFileName;

		private TextBox textBoxDatabase;

		private ContextMenuStrip contextMenuStripEcuList;

		private ToolStripMenuItem menuStripEcuListItemSelectAll;

		private ToolStripMenuItem menuStripEcuListItemDeselectAll;

		private GroupBox groupBoxEcuList;

		private Button buttonDeselectAll;

		private Button buttonSelectAll;

		private Label labelStatus;

		private ErrorProvider errorProviderGlobalModel;

		private Label labelWarning;

		private Button buttonDeselectUndef;

		private Button buttonHelp;

		public string AbsDatabaseFilePath
		{
			get;
			set;
		}

		public Dictionary<string, bool> EcuQualifier2IsActive
		{
			get
			{
				return this.ecuQualifier2IsActive;
			}
			set
			{
				this.ecuQualifier2IsActive = value;
			}
		}

		public List<string> UndefinedEcuQualifiers
		{
			get;
			set;
		}

		public DiagnosticsDatabaseConfiguration DiagnosticsDatabaseConfiguration
		{
			get;
			set;
		}

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public SelectECUs()
		{
			this.InitializeComponent();
			this.ecuQualifier2IsActive = new Dictionary<string, bool>();
			this.displayName2EcuQualifier = new Dictionary<string, string>();
			this.UndefinedEcuQualifiers = new List<string>();
			this.isAllItemsAction = false;
		}

		private void InitEcuList()
		{
			this.isAllItemsAction = true;
			this.checkedListBoxECUs.Items.Clear();
			this.displayName2EcuQualifier.Clear();
			foreach (string current in this.ecuQualifier2IsActive.Keys)
			{
				string text;
				if (this.UndefinedEcuQualifiers.Contains(current))
				{
					text = string.Format(Resources.UndefinedEcuItem, current);
				}
				else
				{
					text = current;
				}
				this.displayName2EcuQualifier.Add(text, current);
				this.checkedListBoxECUs.Items.Add(text, this.ecuQualifier2IsActive[current]);
			}
			this.isAllItemsAction = false;
		}

		public IList<string> GetSelectedEcus()
		{
			List<string> list = new List<string>();
			foreach (string key in this.checkedListBoxECUs.CheckedItems)
			{
				list.Add(this.displayName2EcuQualifier[key]);
			}
			return list;
		}

		private void SelectECUs_Shown(object sender, EventArgs e)
		{
			this.RenderDatabasePathForCurrentWidth();
			this.InitEcuList();
			this.buttonDeselectUndef.Visible = (this.UndefinedEcuQualifiers.Count > 0);
			this.buttonOK.Enabled = (this.checkedListBoxECUs.CheckedIndices.Count - this.GetNumberOfSelectedUndefinedEcus() > 0);
			this.DisplayControlsForCurrentStatus();
		}

		private void checkedListBoxECUs_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (this.isAllItemsAction)
			{
				return;
			}
			string text = this.displayName2EcuQualifier[this.checkedListBoxECUs.Items[e.Index].ToString()];
			if (e.NewValue == CheckState.Checked)
			{
				if (this.IsEcuQualifierAlreadyConfigured(text))
				{
					InformMessageBox.Error(Resources.ErrorEcuAlreadyConfigured);
					e.NewValue = CheckState.Unchecked;
					return;
				}
				if (this.UndefinedEcuQualifiers.Contains(text))
				{
					InformMessageBox.Error(Resources.ErrorEcuNotDefinedAnymore);
					e.NewValue = CheckState.Unchecked;
					return;
				}
				this.EcuQualifier2IsActive[text] = true;
			}
			else if (e.NewValue == CheckState.Unchecked)
			{
				this.EcuQualifier2IsActive[text] = false;
			}
			this.DisplayControlsForCurrentStatus();
		}

		private void textBoxDatabase_Resize(object sender, EventArgs e)
		{
			this.RenderDatabasePathForCurrentWidth();
		}

		private void contextMenuStripEcuList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem == this.menuStripEcuListItemSelectAll)
			{
				this.SetSelectedStateForAllEcuItems(true);
				return;
			}
			if (e.ClickedItem == this.menuStripEcuListItemDeselectAll)
			{
				this.SetSelectedStateForAllEcuItems(false);
			}
		}

		private void buttonSelectAll_Click(object sender, EventArgs e)
		{
			this.SetSelectedStateForAllEcuItems(true);
		}

		private void buttonDeselectAll_Click(object sender, EventArgs e)
		{
			this.SetSelectedStateForAllEcuItems(false);
		}

		private void buttonDeselectUndef_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < this.checkedListBoxECUs.Items.Count; i++)
			{
				string text = this.displayName2EcuQualifier[this.checkedListBoxECUs.Items[i].ToString()];
				if (this.UndefinedEcuQualifiers.Contains(text))
				{
					this.checkedListBoxECUs.SetItemChecked(i, false);
					this.EcuQualifier2IsActive[text] = false;
				}
			}
			this.DisplayControlsForCurrentStatus();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void SelectECUs_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void SetSelectedStateForAllEcuItems(bool isSelected)
		{
			this.isAllItemsAction = true;
			if (isSelected)
			{
				bool flag = false;
				for (int i = 0; i < this.checkedListBoxECUs.Items.Count; i++)
				{
					string text = this.displayName2EcuQualifier[this.checkedListBoxECUs.Items[i].ToString()];
					if (this.IsEcuQualifierAlreadyConfigured(text) || this.UndefinedEcuQualifiers.Contains(text))
					{
						flag = true;
					}
					else
					{
						this.checkedListBoxECUs.SetItemChecked(i, true);
						this.EcuQualifier2IsActive[text] = true;
					}
				}
				if (flag)
				{
					InformMessageBox.Info(Resources.SomeEcuNotSelected);
				}
			}
			else
			{
				for (int j = 0; j < this.checkedListBoxECUs.Items.Count; j++)
				{
					this.checkedListBoxECUs.SetItemChecked(j, false);
					string key = this.displayName2EcuQualifier[this.checkedListBoxECUs.Items[j].ToString()];
					this.EcuQualifier2IsActive[key] = false;
				}
			}
			this.isAllItemsAction = false;
			this.DisplayControlsForCurrentStatus();
		}

		private void DisplayControlsForCurrentStatus()
		{
			int numberOfSelectedUndefinedEcus = this.GetNumberOfSelectedUndefinedEcus();
			if (numberOfSelectedUndefinedEcus > 0)
			{
				this.buttonDeselectUndef.Enabled = true;
				this.labelWarning.Visible = true;
				this.labelStatus.Visible = true;
				this.labelStatus.Text = string.Format(Resources.SomeOfSelEcusAreUndefined, numberOfSelectedUndefinedEcus, this.GetNumberOfSelectedEcus());
				this.buttonOK.Enabled = false;
				return;
			}
			this.buttonDeselectUndef.Enabled = false;
			if (this.GetNumberOfSelectedEcus() == 0)
			{
				this.labelWarning.Visible = true;
				this.labelStatus.Visible = true;
				this.labelStatus.Text = Resources.AtLeastOneEcuMustBeSel;
				this.buttonOK.Enabled = false;
				return;
			}
			this.labelWarning.Visible = false;
			this.labelStatus.Visible = false;
			this.buttonOK.Enabled = true;
		}

		public int GetNumberOfSelectedUndefinedEcus()
		{
			int num = 0;
			foreach (KeyValuePair<string, bool> current in this.EcuQualifier2IsActive)
			{
				if (this.UndefinedEcuQualifiers.Contains(current.Key) && current.Value)
				{
					num++;
				}
			}
			return num;
		}

		public int GetNumberOfSelectedEcus()
		{
			int num = 0;
			foreach (KeyValuePair<string, bool> current in this.EcuQualifier2IsActive)
			{
				if (current.Value)
				{
					num++;
				}
			}
			return num;
		}

		private bool IsEcuQualifierAlreadyConfigured(string qualifier)
		{
			if (this.DiagnosticsDatabaseConfiguration == null)
			{
				return false;
			}
			foreach (DiagnosticsECU current in this.DiagnosticsDatabaseConfiguration.ECUs)
			{
				if (current.Qualifier.Value == qualifier && this.AbsDatabaseFilePath != this.ModelValidator.GetAbsoluteFilePath(current.Database.FilePath.Value))
				{
					return true;
				}
			}
			return false;
		}

		private void RenderDatabasePathForCurrentWidth()
		{
			Size proposedSize = new Size(this.textBoxDatabase.Width - 40, (int)Math.Ceiling((double)this.textBoxDatabase.Font.GetHeight()));
			this.textBoxDatabase.Text = GUIUtil.FilePathToShortendDisplayPath(this.AbsDatabaseFilePath, this.textBoxDatabase.Font, proposedSize);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SelectECUs));
			this.checkedListBoxECUs = new CheckedListBox();
			this.contextMenuStripEcuList = new ContextMenuStrip(this.components);
			this.menuStripEcuListItemSelectAll = new ToolStripMenuItem();
			this.menuStripEcuListItemDeselectAll = new ToolStripMenuItem();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.labelFileName = new Label();
			this.textBoxDatabase = new TextBox();
			this.groupBoxEcuList = new GroupBox();
			this.buttonDeselectUndef = new Button();
			this.labelWarning = new Label();
			this.labelStatus = new Label();
			this.buttonDeselectAll = new Button();
			this.buttonSelectAll = new Button();
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.buttonHelp = new Button();
			this.contextMenuStripEcuList.SuspendLayout();
			this.groupBoxEcuList.SuspendLayout();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.checkedListBoxECUs, "checkedListBoxECUs");
			this.checkedListBoxECUs.CheckOnClick = true;
			this.checkedListBoxECUs.ContextMenuStrip = this.contextMenuStripEcuList;
			this.checkedListBoxECUs.FormattingEnabled = true;
			this.checkedListBoxECUs.Name = "checkedListBoxECUs";
			this.checkedListBoxECUs.Sorted = true;
			this.checkedListBoxECUs.ItemCheck += new ItemCheckEventHandler(this.checkedListBoxECUs_ItemCheck);
			this.contextMenuStripEcuList.Items.AddRange(new ToolStripItem[]
			{
				this.menuStripEcuListItemSelectAll,
				this.menuStripEcuListItemDeselectAll
			});
			this.contextMenuStripEcuList.Name = "contextMenuStripEcuList";
			this.contextMenuStripEcuList.ShowImageMargin = false;
			componentResourceManager.ApplyResources(this.contextMenuStripEcuList, "contextMenuStripEcuList");
			this.contextMenuStripEcuList.ItemClicked += new ToolStripItemClickedEventHandler(this.contextMenuStripEcuList_ItemClicked);
			this.menuStripEcuListItemSelectAll.Name = "menuStripEcuListItemSelectAll";
			componentResourceManager.ApplyResources(this.menuStripEcuListItemSelectAll, "menuStripEcuListItemSelectAll");
			this.menuStripEcuListItemDeselectAll.Name = "menuStripEcuListItemDeselectAll";
			componentResourceManager.ApplyResources(this.menuStripEcuListItemDeselectAll, "menuStripEcuListItemDeselectAll");
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			componentResourceManager.ApplyResources(this.labelFileName, "labelFileName");
			this.labelFileName.Name = "labelFileName";
			componentResourceManager.ApplyResources(this.textBoxDatabase, "textBoxDatabase");
			this.textBoxDatabase.Name = "textBoxDatabase";
			this.textBoxDatabase.ReadOnly = true;
			this.textBoxDatabase.Resize += new EventHandler(this.textBoxDatabase_Resize);
			componentResourceManager.ApplyResources(this.groupBoxEcuList, "groupBoxEcuList");
			this.groupBoxEcuList.Controls.Add(this.buttonDeselectUndef);
			this.groupBoxEcuList.Controls.Add(this.labelWarning);
			this.groupBoxEcuList.Controls.Add(this.labelStatus);
			this.groupBoxEcuList.Controls.Add(this.buttonDeselectAll);
			this.groupBoxEcuList.Controls.Add(this.buttonSelectAll);
			this.groupBoxEcuList.Controls.Add(this.checkedListBoxECUs);
			this.groupBoxEcuList.Name = "groupBoxEcuList";
			this.groupBoxEcuList.TabStop = false;
			componentResourceManager.ApplyResources(this.buttonDeselectUndef, "buttonDeselectUndef");
			this.buttonDeselectUndef.Name = "buttonDeselectUndef";
			this.buttonDeselectUndef.UseVisualStyleBackColor = true;
			this.buttonDeselectUndef.Click += new EventHandler(this.buttonDeselectUndef_Click);
			componentResourceManager.ApplyResources(this.labelWarning, "labelWarning");
			this.labelWarning.Image = Resources.ImageWarning;
			this.labelWarning.Name = "labelWarning";
			componentResourceManager.ApplyResources(this.labelStatus, "labelStatus");
			this.errorProviderGlobalModel.SetIconAlignment(this.labelStatus, (ErrorIconAlignment)componentResourceManager.GetObject("labelStatus.IconAlignment"));
			this.labelStatus.Name = "labelStatus";
			componentResourceManager.ApplyResources(this.buttonDeselectAll, "buttonDeselectAll");
			this.buttonDeselectAll.Name = "buttonDeselectAll";
			this.buttonDeselectAll.UseVisualStyleBackColor = true;
			this.buttonDeselectAll.Click += new EventHandler(this.buttonDeselectAll_Click);
			componentResourceManager.ApplyResources(this.buttonSelectAll, "buttonSelectAll");
			this.buttonSelectAll.Name = "buttonSelectAll";
			this.buttonSelectAll.UseVisualStyleBackColor = true;
			this.buttonSelectAll.Click += new EventHandler(this.buttonSelectAll_Click);
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			base.AcceptButton = this.buttonOK;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.CancelButton = this.buttonCancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.ControlBox = false;
			base.Controls.Add(this.groupBoxEcuList);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.textBoxDatabase);
			base.Controls.Add(this.labelFileName);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Name = "SelectECUs";
			base.Shown += new EventHandler(this.SelectECUs_Shown);
			base.HelpRequested += new HelpEventHandler(this.SelectECUs_HelpRequested);
			this.contextMenuStripEcuList.ResumeLayout(false);
			this.groupBoxEcuList.ResumeLayout(false);
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
