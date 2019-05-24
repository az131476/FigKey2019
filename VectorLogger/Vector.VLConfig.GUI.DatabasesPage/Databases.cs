using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.DatabasesPage
{
	public class Databases : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LoggerType>, IUpdateObserver<DatabaseConfiguration>, IUpdateObserver
	{
		public delegate void DataChangedHandler(DatabaseConfiguration data);

		private LoggerType loggerType;

		private IContainer components;

		private GroupBox groupBoxDatabases;

		private Button buttonAdd;

		private DatabaseGrid databaseGrid;

		private Button buttonRemove;

		private TableLayoutPanel tableLayoutPanel1;

		private Button buttonReplace;

		private Label labelHintDbMissing;

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.databaseGrid.ApplicationDatabaseManager;
			}
			set
			{
				this.databaseGrid.ApplicationDatabaseManager = value;
			}
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.databaseGrid.ModelValidator;
			}
			set
			{
				this.databaseGrid.ModelValidator = value;
			}
		}

		ISemanticChecker IPropertyWindow.SemanticChecker
		{
			get
			{
				return this.databaseGrid.SemanticChecker;
			}
			set
			{
				this.databaseGrid.SemanticChecker = value;
			}
		}

		IModelEditor IPropertyWindow.ModelEditor
		{
			get
			{
				return this.databaseGrid.ModelEditor;
			}
			set
			{
				this.databaseGrid.ModelEditor = value;
			}
		}

		public DisplayMode DisplayMode
		{
			get;
			set;
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
				return PageType.Databases;
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
					this.databaseGrid.DisplayErrors();
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

		public Databases()
		{
			this.InitializeComponent();
			this.databaseGrid.SelectionChanged += new EventHandler(this.OnDatabaseGridSelectionChanged);
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			FileType fileType = FileType.DBCDatabase;
			if (this.databaseGrid.ModelValidator.LoggerSpecifics.Configuration.IsARXMLDatabaseConfigurationSupported)
			{
				fileType = FileType.AnyCANDatabase;
			}
			if (this.databaseGrid.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_LIN) > 0u)
			{
				fileType = FileType.AnyCANorLINDatabase;
			}
			if (this.databaseGrid.ModelValidator.LoggerSpecifics.Flexray.NumberOfChannels > 0u)
			{
				fileType = FileType.AnyDatabase;
			}
			GenericOpenFileDialog.FileName = "";
			if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(fileType))
			{
				this.databaseGrid.AddDatabase(GenericOpenFileDialog.FileName);
			}
		}

		private void buttonRemove_Click(object sender, EventArgs e)
		{
			this.databaseGrid.RemoveDatabase();
		}

		private void buttonReplace_Click(object sender, EventArgs e)
		{
			Database database;
			if (this.databaseGrid.TryGetSelectedDatabase(out database))
			{
				FileType fileType;
				if (BusType.Bt_LIN == database.BusType.Value)
				{
					fileType = FileType.LDFDatabase;
				}
				else if (BusType.Bt_FlexRay == database.BusType.Value)
				{
					fileType = FileType.XMLDatabase;
				}
				else
				{
					fileType = FileType.DBCDatabase;
				}
				GenericOpenFileDialog.FileName = "";
				if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(Resources.SelectNewDatabase, fileType))
				{
					string fileName = GenericOpenFileDialog.FileName;
					bool isDataChanged = ((IPropertyWindow)this).ModelEditor.ReplaceDatabase(fileName, database);
					this.databaseGrid.ValidateInput(isDataChanged);
					this.databaseGrid.SelectRowOfDatabase(database);
				}
			}
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is DatabaseConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.Databases);
			}
			this.databaseGrid.Init();
		}

		bool IPropertyWindow.ValidateInput()
		{
			bool result = this.databaseGrid.ValidateInput(false);
			this.DisplayDbMissingHint();
			return result;
		}

		void IPropertyWindow.Reset()
		{
			this.databaseGrid.Reset();
		}

		bool IPropertyWindow.HasErrors()
		{
			return this.databaseGrid.HasErrors();
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			return this.databaseGrid.HasGlobalErrors();
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			return this.databaseGrid.HasLocalErrors();
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			return this.databaseGrid.HasFormatErrors();
		}

		void IUpdateObserver<DatabaseConfiguration>.Update(DatabaseConfiguration data)
		{
			this.databaseGrid.DatabaseConfiguration = data;
			this.DisplayDbMissingHint();
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType data)
		{
			if (this.loggerType == data)
			{
				return;
			}
			this.loggerType = data;
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
			Database database;
			bool flag = this.databaseGrid.TryGetSelectedDatabase(out database);
			this.buttonRemove.Enabled = flag;
			if (!flag)
			{
				this.buttonReplace.Enabled = false;
				return;
			}
			if (BusType.Bt_FlexRay == database.BusType.Value || database.IsAUTOSARFile)
			{
				this.buttonReplace.Enabled = false;
				return;
			}
			this.buttonReplace.Enabled = true;
		}

		private void DisplayDbMissingHint()
		{
			this.labelHintDbMissing.Visible = false;
			if (this.databaseGrid.IsDatabaseFileMissing)
			{
				this.labelHintDbMissing.Visible = true;
			}
		}

		public bool Serialize(DatabasesPage databasesPage)
		{
			if (databasesPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.databaseGrid.Serialize(databasesPage);
		}

		public bool DeSerialize(DatabasesPage databasesPage)
		{
			if (databasesPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.databaseGrid.DeSerialize(databasesPage);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Databases));
			this.groupBoxDatabases = new GroupBox();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.buttonAdd = new Button();
			this.buttonRemove = new Button();
			this.buttonReplace = new Button();
			this.labelHintDbMissing = new Label();
			this.databaseGrid = new DatabaseGrid();
			this.groupBoxDatabases.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBoxDatabases, "groupBoxDatabases");
			this.groupBoxDatabases.Controls.Add(this.tableLayoutPanel1);
			this.groupBoxDatabases.Controls.Add(this.databaseGrid);
			this.groupBoxDatabases.Name = "groupBoxDatabases";
			this.groupBoxDatabases.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.buttonAdd, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonRemove, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonReplace, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelHintDbMissing, 3, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.buttonAdd, "buttonAdd");
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.UseVisualStyleBackColor = true;
			this.buttonAdd.Click += new EventHandler(this.buttonAdd_Click);
			componentResourceManager.ApplyResources(this.buttonRemove, "buttonRemove");
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.UseVisualStyleBackColor = true;
			this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
			componentResourceManager.ApplyResources(this.buttonReplace, "buttonReplace");
			this.buttonReplace.Name = "buttonReplace";
			this.buttonReplace.UseVisualStyleBackColor = true;
			this.buttonReplace.Click += new EventHandler(this.buttonReplace_Click);
			componentResourceManager.ApplyResources(this.labelHintDbMissing, "labelHintDbMissing");
			this.labelHintDbMissing.Name = "labelHintDbMissing";
			componentResourceManager.ApplyResources(this.databaseGrid, "databaseGrid");
			this.databaseGrid.AllowDrop = true;
			this.databaseGrid.ApplicationDatabaseManager = null;
			this.databaseGrid.CurrentLogger = null;
			this.databaseGrid.DatabaseConfiguration = null;
			this.databaseGrid.Name = "databaseGrid";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxDatabases);
			base.Name = "Databases";
			this.groupBoxDatabases.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
