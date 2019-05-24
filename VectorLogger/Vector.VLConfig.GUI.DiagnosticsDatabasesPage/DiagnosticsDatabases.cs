using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.DiagnosticsDatabasesPage
{
	internal class DiagnosticsDatabases : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LoggerType>, IUpdateObserver<DiagnosticsDatabaseConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver<DiagnosticActionsConfiguration>, IUpdateObserver
	{
		public delegate void DataChangedHandler(DiagnosticsDatabaseConfiguration data);

		private LoggerType loggerType;

		private IContainer components;

		private GroupBox groupBoxDiagDatabases;

		private TableLayoutPanel tableLayoutPanel1;

		private Button buttonAdd;

		private Button buttonRemove;

		private DiagnosticsDatabasesGrid diagnosticsDatabasesGrid;

		private Button buttonEditCommParams;

		private Button buttonSelectECUs;

		private Button buttonReplace;

		public IDiagSymbolsManager DiagSymbolsManager
		{
			get
			{
				return this.diagnosticsDatabasesGrid.DiagSymbolsManager;
			}
			set
			{
				this.diagnosticsDatabasesGrid.DiagSymbolsManager = value;
			}
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.diagnosticsDatabasesGrid.ModelValidator;
			}
			set
			{
				this.diagnosticsDatabasesGrid.ModelValidator = value;
			}
		}

		ISemanticChecker IPropertyWindow.SemanticChecker
		{
			get
			{
				return this.diagnosticsDatabasesGrid.SemanticChecker;
			}
			set
			{
				this.diagnosticsDatabasesGrid.SemanticChecker = value;
			}
		}

		IModelEditor IPropertyWindow.ModelEditor
		{
			get
			{
				return this.diagnosticsDatabasesGrid.ModelEditor;
			}
			set
			{
				this.diagnosticsDatabasesGrid.ModelEditor = value;
			}
		}

		IUpdateService IPropertyWindow.UpdateService
		{
			get;
			set;
		}

		IUpdateObserver IPropertyWindow.UpdateObserver
		{
			get
			{
				return this;
			}
		}

		PageType IPropertyWindow.Type
		{
			get
			{
				return PageType.DiagnosticsDatabases;
			}
		}

		bool IPropertyWindow.IsVisible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				bool visible = base.Visible;
				base.Visible = value;
				if (!visible && base.Visible)
				{
					this.diagnosticsDatabasesGrid.DisplayErrors();
				}
			}
		}

		public List<Vector.VLConfig.Data.ConfigurationDataModel.Action> FocusedActions
		{
			get
			{
				return null;
			}
		}

		public DiagnosticsDatabases()
		{
			this.InitializeComponent();
			this.diagnosticsDatabasesGrid.SelectionChanged += new EventHandler(this.OnDatabaseGridSelectionChanged);
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is DiagnosticsDatabaseConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.DiagnosticsDatabases);
			}
			this.diagnosticsDatabasesGrid.Init();
		}

		bool IPropertyWindow.ValidateInput()
		{
			return this.loggerType == LoggerType.GL1020FTE || this.diagnosticsDatabasesGrid.ValidateInput(false);
		}

		void IPropertyWindow.Reset()
		{
			this.diagnosticsDatabasesGrid.Reset();
		}

		bool IPropertyWindow.HasErrors()
		{
			return this.loggerType != LoggerType.GL1020FTE && this.diagnosticsDatabasesGrid.HasErrors();
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			return this.loggerType != LoggerType.GL1020FTE && this.diagnosticsDatabasesGrid.HasGlobalErrors();
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			return this.loggerType != LoggerType.GL1020FTE && this.diagnosticsDatabasesGrid.HasLocalErrors();
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			return this.loggerType != LoggerType.GL1020FTE && this.diagnosticsDatabasesGrid.HasFormatErrors();
		}

		void IUpdateObserver<DiagnosticsDatabaseConfiguration>.Update(DiagnosticsDatabaseConfiguration data)
		{
			if (this.loggerType != LoggerType.GL1020FTE)
			{
				this.diagnosticsDatabasesGrid.DatabaseConfiguration = data;
				this.buttonEditCommParams.Enabled = (this.diagnosticsDatabasesGrid.DatabaseConfiguration.ECUs.Count != 0);
			}
		}

		void IUpdateObserver<DiagnosticActionsConfiguration>.Update(DiagnosticActionsConfiguration data)
		{
			if (this.loggerType != LoggerType.GL1020FTE)
			{
				this.diagnosticsDatabasesGrid.DiagnosticActionsConfiguration = data;
			}
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType data)
		{
			if (this.loggerType == data)
			{
				return;
			}
			this.loggerType = data;
		}

		void IUpdateObserver<DisplayMode>.Update(DisplayMode mode)
		{
			if (this.loggerType != LoggerType.GL1020FTE)
			{
				this.diagnosticsDatabasesGrid.DisplayMode = mode;
			}
		}

		public ConfigClipboardManager.AcceptType Accept(Vector.VLConfig.Data.ConfigurationDataModel.Action action)
		{
			return ConfigClipboardManager.AcceptType.None;
		}

		public bool Insert(Vector.VLConfig.Data.ConfigurationDataModel.Action action)
		{
			return false;
		}

		public bool Insert(Event evt)
		{
			return false;
		}

		private void OnDatabaseGridSelectionChanged(object sender, EventArgs e)
		{
			this.buttonRemove.Enabled = this.diagnosticsDatabasesGrid.IsGroupRowSelected;
			this.buttonSelectECUs.Enabled = this.diagnosticsDatabasesGrid.IsGroupRowSelected;
			this.buttonReplace.Enabled = this.diagnosticsDatabasesGrid.IsGroupRowSelected;
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			GenericOpenFileDialog.FileName = "";
			if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(FileType.AnyDiagDesc))
			{
				this.diagnosticsDatabasesGrid.AddDatabase(GenericOpenFileDialog.FileName);
			}
		}

		private void buttonRemove_Click(object sender, EventArgs e)
		{
			DiagnosticsDatabase db;
			if (this.diagnosticsDatabasesGrid.TryGetSelectedDatabase(out db) && DialogResult.Yes == InformMessageBox.Question(Resources.QuestionRemoveSelectedDesc))
			{
				this.diagnosticsDatabasesGrid.RemoveDatabase(db);
			}
		}

		private void buttonReplace_Click(object sender, EventArgs e)
		{
			DiagnosticsDatabase diagnosticsDatabase;
			if (!this.diagnosticsDatabasesGrid.TryGetSelectedDatabase(out diagnosticsDatabase))
			{
				return;
			}
			FileType fileType;
			switch (diagnosticsDatabase.Type.Value)
			{
			case DiagDbType.ODX:
				fileType = FileType.ODXDiagDesc;
				goto IL_63;
			case DiagDbType.PDX:
				fileType = FileType.PDXDiagDesc;
				goto IL_63;
			case DiagDbType.CDD:
				fileType = FileType.CDDDiagDesc;
				goto IL_63;
			case DiagDbType.MDX:
				fileType = FileType.MDXDiagDesc;
				goto IL_63;
			}
			fileType = FileType.AnyDiagDesc;
			IL_63:
			GenericOpenFileDialog.FileName = "";
			string directoryName = Path.GetDirectoryName(diagnosticsDatabase.FilePath.Value);
			if (Directory.Exists(directoryName))
			{
				GenericOpenFileDialog.InitialDirectory = directoryName;
			}
			if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(fileType))
			{
				string fileName = Path.GetFileName(diagnosticsDatabase.FilePath.Value);
				Cursor.Current = Cursors.WaitCursor;
				DSMResult dSMResult = ((IPropertyWindow)this).ModelEditor.ReplaceDiagnosticsDatabase(ref diagnosticsDatabase, GenericOpenFileDialog.FileName);
				if (dSMResult != DSMResult.OK)
				{
					this.diagnosticsDatabasesGrid.DisplayDiagSymbolsManagerError(dSMResult);
					return;
				}
				List<DiagnosticsDatabase> list = new List<DiagnosticsDatabase>();
				list.Add(diagnosticsDatabase);
				string text;
				bool flag = ((IPropertyWindow)this).ModelEditor.AutoCorrectDiagCommParams(new ReadOnlyCollection<DiagnosticsDatabase>(list), true, out text);
				this.diagnosticsDatabasesGrid.Refresh();
				this.diagnosticsDatabasesGrid.ValidateInput(true);
				if (flag)
				{
					DisplayReport.ShowDisplayReportDialog(Resources.ChangedConfigReport, string.Format(Resources.ReplacingFile, fileName), new RtfText(text).ToString(), true);
				}
				List<string> undefinedEcuQualifiersOfDiagDatabase = this.diagnosticsDatabasesGrid.ModelValidator.GetUndefinedEcuQualifiersOfDiagDatabase(diagnosticsDatabase);
				if (undefinedEcuQualifiersOfDiagDatabase.Count > 0)
				{
					this.diagnosticsDatabasesGrid.SelectEcus(ref diagnosticsDatabase, undefinedEcuQualifiersOfDiagDatabase);
				}
			}
		}

		private void buttonEditCommParams_Click(object sender, EventArgs e)
		{
			this.diagnosticsDatabasesGrid.EditCommParameters();
		}

		private void buttonSelectECUs_Click(object sender, EventArgs e)
		{
			this.diagnosticsDatabasesGrid.SelectEcus();
		}

		public bool Serialize(DiagnosticsDatabasesPage diagDatabasesPage)
		{
			if (diagDatabasesPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.diagnosticsDatabasesGrid.Serialize(diagDatabasesPage);
		}

		public bool DeSerialize(DiagnosticsDatabasesPage diagDatabasesPage)
		{
			if (diagDatabasesPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.diagnosticsDatabasesGrid.DeSerialize(diagDatabasesPage);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DiagnosticsDatabases));
			this.groupBoxDiagDatabases = new GroupBox();
			this.diagnosticsDatabasesGrid = new DiagnosticsDatabasesGrid();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.buttonAdd = new Button();
			this.buttonRemove = new Button();
			this.buttonEditCommParams = new Button();
			this.buttonSelectECUs = new Button();
			this.buttonReplace = new Button();
			this.groupBoxDiagDatabases.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBoxDiagDatabases, "groupBoxDiagDatabases");
			this.groupBoxDiagDatabases.Controls.Add(this.diagnosticsDatabasesGrid);
			this.groupBoxDiagDatabases.Controls.Add(this.tableLayoutPanel1);
			this.groupBoxDiagDatabases.Name = "groupBoxDiagDatabases";
			this.groupBoxDiagDatabases.TabStop = false;
			this.diagnosticsDatabasesGrid.AllowDrop = true;
			componentResourceManager.ApplyResources(this.diagnosticsDatabasesGrid, "diagnosticsDatabasesGrid");
			this.diagnosticsDatabasesGrid.ApplicationDatabaseManager = null;
			this.diagnosticsDatabasesGrid.CurrentLogger = null;
			this.diagnosticsDatabasesGrid.DatabaseConfiguration = null;
			this.diagnosticsDatabasesGrid.DiagSymbolsManager = null;
			this.diagnosticsDatabasesGrid.DisplayMode = null;
			this.diagnosticsDatabasesGrid.Name = "diagnosticsDatabasesGrid";
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.buttonAdd, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonRemove, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonEditCommParams, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonSelectECUs, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonReplace, 2, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.buttonAdd, "buttonAdd");
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.UseVisualStyleBackColor = true;
			this.buttonAdd.Click += new EventHandler(this.buttonAdd_Click);
			componentResourceManager.ApplyResources(this.buttonRemove, "buttonRemove");
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.UseVisualStyleBackColor = true;
			this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
			componentResourceManager.ApplyResources(this.buttonEditCommParams, "buttonEditCommParams");
			this.buttonEditCommParams.Name = "buttonEditCommParams";
			this.buttonEditCommParams.UseVisualStyleBackColor = true;
			this.buttonEditCommParams.Click += new EventHandler(this.buttonEditCommParams_Click);
			componentResourceManager.ApplyResources(this.buttonSelectECUs, "buttonSelectECUs");
			this.buttonSelectECUs.Name = "buttonSelectECUs";
			this.buttonSelectECUs.UseVisualStyleBackColor = true;
			this.buttonSelectECUs.Click += new EventHandler(this.buttonSelectECUs_Click);
			componentResourceManager.ApplyResources(this.buttonReplace, "buttonReplace");
			this.buttonReplace.Name = "buttonReplace";
			this.buttonReplace.UseVisualStyleBackColor = true;
			this.buttonReplace.Click += new EventHandler(this.buttonReplace_Click);
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxDiagDatabases);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "DiagnosticsDatabases";
			this.groupBoxDiagDatabases.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
