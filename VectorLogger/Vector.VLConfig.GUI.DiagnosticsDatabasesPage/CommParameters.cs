using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.DiagnosticsDatabasesPage
{
	internal class CommParameters : Form
	{
		private List<DiagnosticCommParamsECU> commParamsECUList;

		private DiagnosticsECU preselectedEcu;

		private bool isInitControls;

		private IModelValidator modelValidator;

		private DiagnosticsDatabaseConfigurationInternal diagDatabaseConfigWorkingCopy;

		private IContainer components;

		private SplitContainer splitContainer;

		private Button buttonHelp;

		private Button buttonCancel;

		private Button buttonOK;

		private EcuTreeControl ecuTreeControl;

		private EcuParameters ecuParameters;

		public DiagnosticsDatabaseConfiguration DiagnosticsDatabaseConfiguration
		{
			get;
			set;
		}

		public IDiagSymbolsManager DiagSymbolsManager
		{
			get
			{
				return this.ecuParameters.DiagSymbolsManager;
			}
			set
			{
				this.ecuParameters.DiagSymbolsManager = value;
			}
		}

		public DiagnosticsECU PreselectedEcu
		{
			get
			{
				return this.preselectedEcu;
			}
			set
			{
				this.preselectedEcu = value;
			}
		}

		public CommParameters(IModelValidator modelVal)
		{
			this.isInitControls = true;
			this.InitializeComponent();
			this.commParamsECUList = new List<DiagnosticCommParamsECU>();
			this.modelValidator = modelVal;
			this.ecuParameters.ModelValidator = modelVal;
			this.preselectedEcu = null;
			this.ecuParameters.DataChanged += new EventHandler(this.OnCommParamsEcuDataChanged);
			this.splitContainer.SplitterDistance = base.ClientSize.Width - this.ecuParameters.Width - this.splitContainer.SplitterWidth;
		}

		private void SubscribeTreeControlEvents()
		{
			EcuTreeControl expr_06 = this.ecuTreeControl;
			expr_06.OnSelectEcu = (EventHandler)Delegate.Combine(expr_06.OnSelectEcu, new EventHandler(this.OnSelectEcu));
			EcuTreeControl expr_2D = this.ecuTreeControl;
			expr_2D.OnBeforeSelect = (EventHandler<TreeViewCancelEventArgs>)Delegate.Combine(expr_2D.OnBeforeSelect, new EventHandler<TreeViewCancelEventArgs>(this.OnBeforeSelect));
		}

		private void UnsubscribeTreeControlEvents()
		{
			EcuTreeControl expr_06 = this.ecuTreeControl;
			expr_06.OnSelectEcu = (EventHandler)Delegate.Remove(expr_06.OnSelectEcu, new EventHandler(this.OnSelectEcu));
			EcuTreeControl expr_2D = this.ecuTreeControl;
			expr_2D.OnBeforeSelect = (EventHandler<TreeViewCancelEventArgs>)Delegate.Remove(expr_2D.OnBeforeSelect, new EventHandler<TreeViewCancelEventArgs>(this.OnBeforeSelect));
		}

		private void CommParameters_Shown(object sender, EventArgs e)
		{
			this.commParamsECUList.Clear();
			if (this.DiagnosticsDatabaseConfiguration == null)
			{
				return;
			}
			if (this.DiagnosticsDatabaseConfiguration.ECUs.Count == 0)
			{
				return;
			}
			this.diagDatabaseConfigWorkingCopy = new DiagnosticsDatabaseConfigurationInternal(this.DiagnosticsDatabaseConfiguration);
			this.ecuTreeControl.DiagnosticsDatabaseConfiguration = this.diagDatabaseConfigWorkingCopy;
			this.ecuParameters.DiagnosticsDatabaseConfiguration = this.diagDatabaseConfigWorkingCopy;
			foreach (DiagnosticsECU current in this.diagDatabaseConfigWorkingCopy.ECUs)
			{
				this.commParamsECUList.Add(current.DiagnosticCommParamsECU);
			}
			this.SubscribeTreeControlEvents();
			this.isInitControls = true;
			this.ecuTreeControl.Init();
			if (this.preselectedEcu != null)
			{
				this.ecuTreeControl.SelectEcu(this.preselectedEcu.Qualifier.Value);
				this.preselectedEcu = null;
			}
			else
			{
				this.ecuTreeControl.SelectEcu(this.diagDatabaseConfigWorkingCopy.ECUs[0].Qualifier.Value);
			}
			this.isInitControls = false;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (this.ValidateInput())
			{
				this.DiagnosticsDatabaseConfiguration.RemoveAllDatabases();
				foreach (DiagnosticsDatabase current in this.diagDatabaseConfigWorkingCopy.Databases)
				{
					this.DiagnosticsDatabaseConfiguration.AddDatabase(current);
				}
				base.DialogResult = DialogResult.OK;
				this.UnsubscribeTreeControlEvents();
				return;
			}
			InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
			base.DialogResult = DialogResult.None;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.UnsubscribeTreeControlEvents();
			base.DialogResult = DialogResult.Cancel;
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void CommParameters_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void OnSelectEcu(object sender, EventArgs e)
		{
			this.ecuParameters.DiagnosticCommParamsECU = this.commParamsECUList[this.ecuTreeControl.SelectedEcuIndex];
			this.ecuParameters.EcuQualifier = this.diagDatabaseConfigWorkingCopy.ECUs[this.ecuTreeControl.SelectedEcuIndex].Qualifier.Value;
			this.ecuParameters.VariantQualifier = this.diagDatabaseConfigWorkingCopy.ECUs[this.ecuTreeControl.SelectedEcuIndex].Variant.Value;
			this.ecuParameters.UpdateControls();
			this.ecuParameters.Visible = true;
		}

		private void OnBeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			if (!this.ValidateInput())
			{
				InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
				e.Cancel = true;
			}
		}

		private bool ValidateInput()
		{
			return this.ecuParameters.ValidateInput();
		}

		private void OnCommParamsEcuDataChanged(object sender, EventArgs e)
		{
			this.ecuTreeControl.SetNodesErrorStateAndTooltip(this.ecuParameters.PageValidator);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CommParameters));
			this.splitContainer = new SplitContainer();
			this.ecuTreeControl = new EcuTreeControl();
			this.ecuParameters = new EcuParameters();
			this.buttonHelp = new Button();
			this.buttonCancel = new Button();
			this.buttonOK = new Button();
			((ISupportInitialize)this.splitContainer).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.splitContainer, "splitContainer");
			this.splitContainer.FixedPanel = FixedPanel.Panel2;
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Panel1.Controls.Add(this.ecuTreeControl);
			componentResourceManager.ApplyResources(this.splitContainer.Panel1, "splitContainer.Panel1");
			this.splitContainer.Panel2.Controls.Add(this.ecuParameters);
			componentResourceManager.ApplyResources(this.splitContainer.Panel2, "splitContainer.Panel2");
			this.ecuTreeControl.DiagnosticsDatabaseConfiguration = null;
			componentResourceManager.ApplyResources(this.ecuTreeControl, "ecuTreeControl");
			this.ecuTreeControl.Name = "ecuTreeControl";
			this.ecuParameters.DiagnosticCommParamsECU = null;
			this.ecuParameters.DiagnosticsDatabaseConfiguration = null;
			this.ecuParameters.DiagSymbolsManager = null;
			componentResourceManager.ApplyResources(this.ecuParameters, "ecuParameters");
			this.ecuParameters.EcuQualifier = null;
			this.ecuParameters.Name = "ecuParameters";
			this.ecuParameters.VariantQualifier = "";
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.splitContainer);
			base.Name = "CommParameters";
			base.Shown += new EventHandler(this.CommParameters_Shown);
			base.HelpRequested += new HelpEventHandler(this.CommParameters_HelpRequested);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer).EndInit();
			this.splitContainer.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
