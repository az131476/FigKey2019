using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Vector.McModule;
using Vector.McModule.Explorer;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.GUI.CcpXcpSignalRequestsPage
{
	internal class CcpXcpSymbolSelection : Form
	{
		private CcpXcpSignalRequests ccpXcpSignalRequests;

		private ReadOnlyCollection<Database> databaseList;

		private static string lastfocusedSignalName;

		private static string lastfocusedSignalEcuName;

		private bool signalSelectionApplied;

		private IContainer components;

		private Button mButtonClose;

		private ExplorerControl mExplorerControl;

		private Button mButtonOk;

		private Button mButtonApply;

		private Button mButtonHelp;

		public Dictionary<ISignal, IDatabase> SelectedSignals
		{
			get;
			private set;
		}

		public CcpXcpSymbolSelection(CcpXcpSignalRequests ccpXcpSignalRequests, ReadOnlyCollection<Database> databaseList, DisplaySettings displaySettings)
		{
			this.databaseList = databaseList;
			Dictionary<IDatabase, string> dictionary = new Dictionary<IDatabase, string>();
			foreach (Database current in this.databaseList)
			{
				A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(current);
				if (a2LDatabase != null && a2LDatabase.IsLoaded())
				{
					dictionary.Add(a2LDatabase.McDatabase, current.CcpXcpEcuDisplayName.Value);
				}
			}
			this.ccpXcpSignalRequests = ccpXcpSignalRequests;
			this.InitializeComponent();
			this.mExplorerControl.Init(dictionary, displaySettings);
			this.mExplorerControl.DoubleClick += new EventHandler<DoubleClickEventArgs>(this.OnSignalDoubleClick);
			this.mExplorerControl.SelectionChanged += new EventHandler(this.ExplorerControlIsValidChanged);
			this.mExplorerControl.FocuseNode(CcpXcpSymbolSelection.lastfocusedSignalName, CcpXcpSymbolSelection.lastfocusedSignalEcuName);
		}

		private void ExplorerControlIsValidChanged(object sender, EventArgs e)
		{
			this.mButtonOk.Enabled = (this.mExplorerControl.SelectedSignals.Count > 0);
			this.mButtonApply.Enabled = this.mButtonOk.Enabled;
			this.signalSelectionApplied = false;
		}

		private void mButtonOk_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			if (!this.signalSelectionApplied)
			{
				this.SelectedSignals = this.mExplorerControl.SelectedSignals;
				base.DialogResult = DialogResult.OK;
				this.AddSignals();
			}
			base.Close();
		}

		private void mButtonApply_Click(object sender, EventArgs e)
		{
			this.AddSignals();
			this.signalSelectionApplied = true;
		}

		private void OnSignalDoubleClick(object sender, DoubleClickEventArgs e)
		{
			this.AddSignals();
			this.signalSelectionApplied = true;
		}

		private void AddSignals()
		{
			this.SelectedSignals = this.mExplorerControl.SelectedSignals;
			this.ccpXcpSignalRequests.AddSignals(this.mExplorerControl.SelectedSignals);
		}

		private Database GetSelectedDatabase(IDatabase db)
		{
			if (db == null)
			{
				return null;
			}
			foreach (Database current in this.databaseList)
			{
				A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(current);
				if (a2LDatabase != null && a2LDatabase.Equals(db))
				{
					return current;
				}
			}
			return null;
		}

		private void mButtonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void CcpXcpSymbolSelection_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void CcpXcpSymbolSelection_FormClosed(object sender, FormClosedEventArgs e)
		{
			CcpXcpSymbolSelection.lastfocusedSignalName = ((this.mExplorerControl.FocusedSignal != null) ? this.mExplorerControl.FocusedSignal.Name : string.Empty);
			Database database = CcpXcpManager.Instance().GetDatabase(this.mExplorerControl.FocusedDatabase);
			CcpXcpSymbolSelection.lastfocusedSignalEcuName = ((database != null && database.CcpXcpEcuList.Any<CcpXcpEcu>()) ? database.CcpXcpEcuList[0].CcpXcpEcuDisplayName : string.Empty);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CcpXcpSymbolSelection));
			this.mButtonClose = new Button();
			this.mExplorerControl = new ExplorerControl();
			this.mButtonOk = new Button();
			this.mButtonApply = new Button();
			this.mButtonHelp = new Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.mButtonClose, "mButtonClose");
			this.mButtonClose.DialogResult = DialogResult.Cancel;
			this.mButtonClose.Name = "mButtonClose";
			this.mButtonClose.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mExplorerControl, "mExplorerControl");
			this.mExplorerControl.Name = "mExplorerControl";
			componentResourceManager.ApplyResources(this.mButtonOk, "mButtonOk");
			this.mButtonOk.DialogResult = DialogResult.OK;
			this.mButtonOk.Name = "mButtonOk";
			this.mButtonOk.UseVisualStyleBackColor = true;
			this.mButtonOk.Click += new EventHandler(this.mButtonOk_Click);
			componentResourceManager.ApplyResources(this.mButtonApply, "mButtonApply");
			this.mButtonApply.Name = "mButtonApply";
			this.mButtonApply.UseVisualStyleBackColor = true;
			this.mButtonApply.Click += new EventHandler(this.mButtonApply_Click);
			componentResourceManager.ApplyResources(this.mButtonHelp, "mButtonHelp");
			this.mButtonHelp.Name = "mButtonHelp";
			this.mButtonHelp.UseVisualStyleBackColor = true;
			this.mButtonHelp.Click += new EventHandler(this.mButtonHelp_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.mButtonClose;
			base.Controls.Add(this.mButtonHelp);
			base.Controls.Add(this.mButtonApply);
			base.Controls.Add(this.mButtonOk);
			base.Controls.Add(this.mExplorerControl);
			base.Controls.Add(this.mButtonClose);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CcpXcpSymbolSelection";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.FormClosed += new FormClosedEventHandler(this.CcpXcpSymbolSelection_FormClosed);
			base.HelpRequested += new HelpEventHandler(this.CcpXcpSymbolSelection_HelpRequested);
			base.ResumeLayout(false);
		}
	}
}
