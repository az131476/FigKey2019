using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.FiltersPage
{
	public class SymbolicFlexrayCondition : Form
	{
		private SymbolicMessageFilter filter;

		private IModelValidator modelValidator;

		private IApplicationDatabaseManager databaseManager;

		private string databasePath;

		private string networkName;

		private string pduOrFrameName;

		private string databaseName;

		private BusType busType;

		private bool isPDU;

		private bool isFlexrayDBVersionGreater20;

		private IContainer components;

		private GroupBox groupBoxSymbolName;

		private Button buttonSelectPDU;

		private TextBox textBoxPDUName;

		private GroupBox groupBoxAffectedFrames;

		private ListView listViewAffectedFrames;

		private ColumnHeader columnSlotID;

		private ColumnHeader columnBaseCycle;

		private ColumnHeader columnCycleRepetition;

		private Button buttonOK;

		private Button buttonCancel;

		private Button buttonHelp;

		private GroupBox groupBoxAffectedPDUs;

		private ListView listViewAffectedPDUs;

		private ColumnHeader columnName;

		private ColumnHeader columnFrameName;

		private ToolTip toolTip;

		private Label labelAffectedFramesDesc;

		private Label labelAffectedPDUsDesc;

		public SymbolicMessageFilter Filter
		{
			get
			{
				return this.filter;
			}
			set
			{
				this.filter = value;
				this.pduOrFrameName = this.filter.MessageName.Value;
				this.databasePath = this.filter.DatabasePath.Value;
				this.networkName = this.filter.NetworkName.Value;
				this.databaseName = this.filter.DatabaseName.Value;
				this.busType = BusType.Bt_FlexRay;
				this.isPDU = this.filter.IsFlexrayPDU.Value;
				this.isFlexrayDBVersionGreater20 = this.filter.IsFibexVersionGreaterThan2;
			}
		}

		public SymbolicFlexrayCondition(IApplicationDatabaseManager manager, IModelValidator modelVal)
		{
			this.InitializeComponent();
			this.modelValidator = modelVal;
			this.databaseManager = manager;
		}

		private void SymbolicFlexrayPDUCondition_Shown(object sender, EventArgs e)
		{
			this.DisplayFramesAndPDUInformation();
		}

		private void buttonSelectPDU_Click(object sender, EventArgs e)
		{
			if (this.databaseManager.SelectMessageInDatabase(ref this.pduOrFrameName, ref this.databaseName, ref this.databasePath, ref this.networkName, ref this.busType, ref this.isPDU))
			{
				this.databasePath = this.modelValidator.GetFilePathRelativeToConfiguration(this.databasePath);
			}
			this.DisplayFramesAndPDUInformation();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.filter.DatabasePath.Value = this.databasePath;
			this.filter.NetworkName.Value = this.networkName;
			this.filter.DatabaseName.Value = this.databaseName;
			this.filter.MessageName.Value = this.pduOrFrameName;
			this.filter.IsFlexrayPDU.Value = this.isPDU;
			this.filter.IsFibexVersionGreaterThan2 = this.isFlexrayDBVersionGreater20;
			base.DialogResult = DialogResult.OK;
		}

		private void textBoxPDUName_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(this.textBoxPDUName, this.textBoxPDUName.Text);
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void SymbolicFlexrayCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void DisplayFramesAndPDUInformation()
		{
			this.textBoxPDUName.Text = string.Format("{0} (DB: {1})", this.pduOrFrameName, this.databaseName);
			if (this.isPDU)
			{
				this.groupBoxSymbolName.Text = Resources.PDUNameLabel;
				this.groupBoxAffectedPDUs.Text = Resources.OtherAffectedPDUsLabel;
			}
			else
			{
				this.groupBoxSymbolName.Text = Resources.FrameNameLabel;
				this.groupBoxAffectedPDUs.Text = Resources.AffectedPDUsLabel;
			}
			IList<MessageDefinition> list = null;
			IList<string> list2 = null;
			this.listViewAffectedFrames.Items.Clear();
			this.listViewAffectedPDUs.Items.Clear();
			if (this.databaseManager.GetFlexrayFrameOrPDUInfo(this.modelValidator.GetAbsoluteFilePath(this.databasePath), this.networkName, this.pduOrFrameName, this.isPDU, out list, out list2, out this.isFlexrayDBVersionGreater20))
			{
				foreach (MessageDefinition current in list)
				{
					ListViewItem listViewItem = new ListViewItem(current.Name);
					listViewItem.SubItems.Add(current.CanDbMessageId.ToString());
					listViewItem.SubItems.Add(current.FrBaseCycle.ToString());
					listViewItem.SubItems.Add(current.FrCycleRepetition.ToString());
					this.listViewAffectedFrames.Items.Add(listViewItem);
				}
				foreach (string current2 in list2)
				{
					this.listViewAffectedPDUs.Items.Add(current2);
				}
			}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SymbolicFlexrayCondition));
			this.groupBoxSymbolName = new GroupBox();
			this.buttonSelectPDU = new Button();
			this.textBoxPDUName = new TextBox();
			this.groupBoxAffectedFrames = new GroupBox();
			this.labelAffectedFramesDesc = new Label();
			this.listViewAffectedFrames = new ListView();
			this.columnFrameName = new ColumnHeader();
			this.columnSlotID = new ColumnHeader();
			this.columnBaseCycle = new ColumnHeader();
			this.columnCycleRepetition = new ColumnHeader();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.buttonHelp = new Button();
			this.groupBoxAffectedPDUs = new GroupBox();
			this.labelAffectedPDUsDesc = new Label();
			this.listViewAffectedPDUs = new ListView();
			this.columnName = new ColumnHeader();
			this.toolTip = new ToolTip(this.components);
			this.groupBoxSymbolName.SuspendLayout();
			this.groupBoxAffectedFrames.SuspendLayout();
			this.groupBoxAffectedPDUs.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBoxSymbolName, "groupBoxSymbolName");
			this.groupBoxSymbolName.Controls.Add(this.buttonSelectPDU);
			this.groupBoxSymbolName.Controls.Add(this.textBoxPDUName);
			this.groupBoxSymbolName.Name = "groupBoxSymbolName";
			this.groupBoxSymbolName.TabStop = false;
			componentResourceManager.ApplyResources(this.buttonSelectPDU, "buttonSelectPDU");
			this.buttonSelectPDU.Name = "buttonSelectPDU";
			this.buttonSelectPDU.UseVisualStyleBackColor = true;
			this.buttonSelectPDU.Click += new EventHandler(this.buttonSelectPDU_Click);
			componentResourceManager.ApplyResources(this.textBoxPDUName, "textBoxPDUName");
			this.textBoxPDUName.Name = "textBoxPDUName";
			this.textBoxPDUName.ReadOnly = true;
			this.textBoxPDUName.MouseEnter += new EventHandler(this.textBoxPDUName_MouseEnter);
			componentResourceManager.ApplyResources(this.groupBoxAffectedFrames, "groupBoxAffectedFrames");
			this.groupBoxAffectedFrames.Controls.Add(this.labelAffectedFramesDesc);
			this.groupBoxAffectedFrames.Controls.Add(this.listViewAffectedFrames);
			this.groupBoxAffectedFrames.Name = "groupBoxAffectedFrames";
			this.groupBoxAffectedFrames.TabStop = false;
			componentResourceManager.ApplyResources(this.labelAffectedFramesDesc, "labelAffectedFramesDesc");
			this.labelAffectedFramesDesc.Name = "labelAffectedFramesDesc";
			componentResourceManager.ApplyResources(this.listViewAffectedFrames, "listViewAffectedFrames");
			this.listViewAffectedFrames.AutoArrange = false;
			this.listViewAffectedFrames.Columns.AddRange(new ColumnHeader[]
			{
				this.columnFrameName,
				this.columnSlotID,
				this.columnBaseCycle,
				this.columnCycleRepetition
			});
			this.listViewAffectedFrames.GridLines = true;
			this.listViewAffectedFrames.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			this.listViewAffectedFrames.Name = "listViewAffectedFrames";
			this.listViewAffectedFrames.UseCompatibleStateImageBehavior = false;
			this.listViewAffectedFrames.View = View.Details;
			componentResourceManager.ApplyResources(this.columnFrameName, "columnFrameName");
			componentResourceManager.ApplyResources(this.columnSlotID, "columnSlotID");
			componentResourceManager.ApplyResources(this.columnBaseCycle, "columnBaseCycle");
			componentResourceManager.ApplyResources(this.columnCycleRepetition, "columnCycleRepetition");
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.groupBoxAffectedPDUs, "groupBoxAffectedPDUs");
			this.groupBoxAffectedPDUs.Controls.Add(this.labelAffectedPDUsDesc);
			this.groupBoxAffectedPDUs.Controls.Add(this.listViewAffectedPDUs);
			this.groupBoxAffectedPDUs.Name = "groupBoxAffectedPDUs";
			this.groupBoxAffectedPDUs.TabStop = false;
			componentResourceManager.ApplyResources(this.labelAffectedPDUsDesc, "labelAffectedPDUsDesc");
			this.labelAffectedPDUsDesc.Name = "labelAffectedPDUsDesc";
			componentResourceManager.ApplyResources(this.listViewAffectedPDUs, "listViewAffectedPDUs");
			this.listViewAffectedPDUs.AutoArrange = false;
			this.listViewAffectedPDUs.Columns.AddRange(new ColumnHeader[]
			{
				this.columnName
			});
			this.listViewAffectedPDUs.GridLines = true;
			this.listViewAffectedPDUs.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			this.listViewAffectedPDUs.Name = "listViewAffectedPDUs";
			this.listViewAffectedPDUs.UseCompatibleStateImageBehavior = false;
			this.listViewAffectedPDUs.View = View.Details;
			componentResourceManager.ApplyResources(this.columnName, "columnName");
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.groupBoxAffectedPDUs);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBoxAffectedFrames);
			base.Controls.Add(this.groupBoxSymbolName);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SymbolicFlexrayCondition";
			base.Shown += new EventHandler(this.SymbolicFlexrayPDUCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.SymbolicFlexrayCondition_HelpRequested);
			this.groupBoxSymbolName.ResumeLayout(false);
			this.groupBoxSymbolName.PerformLayout();
			this.groupBoxAffectedFrames.ResumeLayout(false);
			this.groupBoxAffectedPDUs.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
