using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.CcpXcpDescriptionsPage
{
	internal class CcpXcpDescriptions : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<DatabaseConfiguration>, IUpdateObserver<CcpXcpSignalConfiguration>, IUpdateObserver
	{
		public delegate void DataChangedHandler(DatabaseConfiguration data);

		public delegate void SignalConfigHandler(CcpXcpSignalConfiguration data);

		public delegate void LogDataChangedHandler(LogDataStorage data);

		private string mConfigurationFolderPath;

		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private IContainer components;

		private GroupBox mGroupBoxCcpXcpDescriptions;

		private CcpXcpDescriptionsGrid ccpXcpDescriptionsGrid;

		private Button buttonAdd;

		private Button buttonRemove;

		private TableLayoutPanel tableLayoutPanel1;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private Button mButtonSettings;

		private Button mButtonReplace;

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.ccpXcpDescriptionsGrid.ApplicationDatabaseManager;
			}
			set
			{
				this.ccpXcpDescriptionsGrid.ApplicationDatabaseManager = value;
			}
		}

		public string ConfigurationFolderPath
		{
			get
			{
				return this.mConfigurationFolderPath;
			}
			set
			{
				this.mConfigurationFolderPath = value;
				this.ccpXcpDescriptionsGrid.ConfigurationFolderPath = value;
			}
		}

		public IConfigurationManagerService ConfigManager
		{
			set
			{
				this.ccpXcpDescriptionsGrid.ConfigManager = value;
			}
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.ccpXcpDescriptionsGrid.ModelValidator;
			}
			set
			{
				this.ccpXcpDescriptionsGrid.ModelValidator = value;
			}
		}

		ISemanticChecker IPropertyWindow.SemanticChecker
		{
			get
			{
				return this.ccpXcpDescriptionsGrid.SemanticChecker;
			}
			set
			{
				this.ccpXcpDescriptionsGrid.SemanticChecker = value;
			}
		}

		IModelEditor IPropertyWindow.ModelEditor
		{
			get
			{
				return this.ccpXcpDescriptionsGrid.ModelEditor;
			}
			set
			{
				this.ccpXcpDescriptionsGrid.ModelEditor = value;
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
				return PageType.CcpXcpDescriptions;
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
					this.ccpXcpDescriptionsGrid.DisplayErrors();
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

		public CcpXcpDescriptions()
		{
			this.InitializeComponent();
			this.ccpXcpDescriptionsGrid.SelectionChanged += new EventHandler(this.OnDatabaseGridSelectionChanged);
			this.ccpXcpDescriptionsGrid.CcpXcpDescriptionsPage = this;
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			FileType fileType = FileType.DBCorA2LDatabase;
			if (this.ccpXcpDescriptionsGrid.ModelValidator.LoggerSpecifics.Flexray.NumberOfChannels > 0u)
			{
				fileType = FileType.DBCorA2LorXMLDatabase;
			}
			GenericOpenFileDialog.FileName = "";
			if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(fileType))
			{
				this.ccpXcpDescriptionsGrid.AddDatabase(GenericOpenFileDialog.FileName);
			}
		}

		private void buttonRemove_Click(object sender, EventArgs e)
		{
			Database db;
			if (this.ccpXcpDescriptionsGrid.TryGetSelectedDatabase(out db) && DialogResult.Yes == InformMessageBox.Question(Resources.QuestionRemoveSelectedDb))
			{
				this.ccpXcpDescriptionsGrid.RemoveDatabase(db);
			}
		}

		private void mButtonSettings_Click(object sender, EventArgs e)
		{
			this.ccpXcpDescriptionsGrid.ShowProtocolSettingsDialog();
		}

		private void mButtonReplace_Click(object sender, EventArgs e)
		{
			Database database;
			if (this.ccpXcpDescriptionsGrid.TryGetSelectedDatabase(out database))
			{
				FileType fileType;
				switch (database.FileType)
				{
				case DatabaseFileType.A2L:
					fileType = FileType.A2LDatabase;
					break;
				case DatabaseFileType.DBC:
					fileType = FileType.DBCDatabase;
					break;
				case DatabaseFileType.XML:
					fileType = FileType.XMLDatabase;
					break;
				default:
					fileType = FileType.DBCorA2LorXMLDatabase;
					break;
				}
				GenericOpenFileDialog.FileName = "";
				if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(fileType))
				{
					this.ccpXcpDescriptionsGrid.ReplaceDatabase(GenericOpenFileDialog.FileName, database);
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
			this.ccpXcpDescriptionsGrid.Init();
		}

		void IPropertyWindow.Reset()
		{
			this.ResetValidationFramework();
			this.ccpXcpDescriptionsGrid.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			if (!this.ccpXcpDescriptionsGrid.ModelValidator.LoggerSpecifics.Recording.IsCcpXcpSupported)
			{
				return true;
			}
			bool flag = true;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			flag &= this.ccpXcpDescriptionsGrid.ValidateInput(false);
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			return flag;
		}

		bool IPropertyWindow.HasErrors()
		{
			if (!this.ccpXcpDescriptionsGrid.ModelValidator.LoggerSpecifics.Recording.IsCcpXcpSupported)
			{
				return false;
			}
			bool flag = this.pageValidator.General.HasErrors(Enum.GetValues(typeof(ValidationErrorClass)) as ValidationErrorClass[]);
			return flag | this.ccpXcpDescriptionsGrid.HasErrors();
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			if (!this.ccpXcpDescriptionsGrid.ModelValidator.LoggerSpecifics.Recording.IsCcpXcpSupported)
			{
				return false;
			}
			bool flag = this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.GlobalModelError
			});
			return flag | this.ccpXcpDescriptionsGrid.HasGlobalErrors();
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			if (!this.ccpXcpDescriptionsGrid.ModelValidator.LoggerSpecifics.Recording.IsCcpXcpSupported)
			{
				return false;
			}
			bool flag = this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.FormatError,
				ValidationErrorClass.LocalModelError
			});
			return flag | this.ccpXcpDescriptionsGrid.HasLocalErrors();
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			if (!this.ccpXcpDescriptionsGrid.ModelValidator.LoggerSpecifics.Recording.IsCcpXcpSupported)
			{
				return false;
			}
			IPageValidatorGeneral arg_34_0 = this.pageValidator.General;
			ValidationErrorClass[] errorClasses = new ValidationErrorClass[1];
			bool flag = arg_34_0.HasErrors(errorClasses);
			return flag | this.ccpXcpDescriptionsGrid.HasFormatErrors();
		}

		private void ResetValidationFramework()
		{
			this.pageValidator.General.Reset();
			this.guiElementManager.Reset();
		}

		void IUpdateObserver<DatabaseConfiguration>.Update(DatabaseConfiguration data)
		{
			if (!this.ccpXcpDescriptionsGrid.ModelValidator.LoggerSpecifics.Recording.IsCcpXcpSupported)
			{
				return;
			}
			this.ccpXcpDescriptionsGrid.DatabaseConfiguration = data;
			Database database;
			this.mButtonSettings.Enabled = (this.ccpXcpDescriptionsGrid.TryGetSelectedDatabase(out database) && database != null && database.IsCPActive.Value && !database.IsFileNotFound && !database.IsInconsistent);
			this.mButtonReplace.Enabled = (this.ccpXcpDescriptionsGrid.TryGetSelectedDatabase(out database) && database != null && (database.FileType == DatabaseFileType.A2L || database.FileType == DatabaseFileType.DBC) && database.IsCPActive.Value);
			((IPropertyWindow)this).ValidateInput();
		}

		void IUpdateObserver<CcpXcpSignalConfiguration>.Update(CcpXcpSignalConfiguration data)
		{
			this.ccpXcpDescriptionsGrid.ValidateInput(false);
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
			this.buttonRemove.Enabled = this.ccpXcpDescriptionsGrid.TryGetSelectedDatabase(out database);
		}

		public void UpdateSettingsButtonState(bool active)
		{
			this.mButtonSettings.Enabled = active;
		}

		public void UpdateReplaceButtonState(bool active)
		{
			this.mButtonReplace.Enabled = active;
		}

		public bool Serialize(CcpXcpDescriptionsPage page)
		{
			if (page == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.ccpXcpDescriptionsGrid.Serialize(page);
		}

		public bool DeSerialize(CcpXcpDescriptionsPage page)
		{
			if (page == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.ccpXcpDescriptionsGrid.DeSerialize(page);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CcpXcpDescriptions));
			this.mGroupBoxCcpXcpDescriptions = new GroupBox();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.buttonAdd = new Button();
			this.buttonRemove = new Button();
			this.mButtonReplace = new Button();
			this.mButtonSettings = new Button();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.ccpXcpDescriptionsGrid = new CcpXcpDescriptionsGrid();
			this.mGroupBoxCcpXcpDescriptions.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.mGroupBoxCcpXcpDescriptions, "mGroupBoxCcpXcpDescriptions");
			this.mGroupBoxCcpXcpDescriptions.Controls.Add(this.tableLayoutPanel1);
			this.mGroupBoxCcpXcpDescriptions.Controls.Add(this.ccpXcpDescriptionsGrid);
			this.mGroupBoxCcpXcpDescriptions.Name = "mGroupBoxCcpXcpDescriptions";
			this.mGroupBoxCcpXcpDescriptions.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.buttonAdd, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonRemove, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.mButtonReplace, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.mButtonSettings, 6, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.buttonAdd, "buttonAdd");
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.UseVisualStyleBackColor = true;
			this.buttonAdd.Click += new EventHandler(this.buttonAdd_Click);
			componentResourceManager.ApplyResources(this.buttonRemove, "buttonRemove");
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.UseVisualStyleBackColor = true;
			this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
			componentResourceManager.ApplyResources(this.mButtonReplace, "mButtonReplace");
			this.mButtonReplace.Name = "mButtonReplace";
			this.mButtonReplace.UseVisualStyleBackColor = true;
			this.mButtonReplace.Click += new EventHandler(this.mButtonReplace_Click);
			componentResourceManager.ApplyResources(this.mButtonSettings, "mButtonSettings");
			this.mButtonSettings.Name = "mButtonSettings";
			this.mButtonSettings.UseVisualStyleBackColor = true;
			this.mButtonSettings.Click += new EventHandler(this.mButtonSettings_Click);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderLocalModel, "errorProviderLocalModel");
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			this.ccpXcpDescriptionsGrid.AllowDrop = true;
			componentResourceManager.ApplyResources(this.ccpXcpDescriptionsGrid, "ccpXcpDescriptionsGrid");
			this.ccpXcpDescriptionsGrid.ApplicationDatabaseManager = null;
			this.ccpXcpDescriptionsGrid.ConfigManager = null;
			this.ccpXcpDescriptionsGrid.ConfigurationFolderPath = null;
			this.ccpXcpDescriptionsGrid.CurrentLogger = null;
			this.ccpXcpDescriptionsGrid.DatabaseConfiguration = null;
			this.ccpXcpDescriptionsGrid.ModelEditor = null;
			this.ccpXcpDescriptionsGrid.ModelValidator = null;
			this.ccpXcpDescriptionsGrid.Name = "ccpXcpDescriptionsGrid";
			this.ccpXcpDescriptionsGrid.SemanticChecker = null;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.mGroupBoxCcpXcpDescriptions);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "CcpXcpDescriptions";
			this.mGroupBoxCcpXcpDescriptions.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
