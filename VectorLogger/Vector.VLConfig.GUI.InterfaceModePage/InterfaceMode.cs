using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.InterfaceModePage
{
	public class InterfaceMode : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LoggerType>, IUpdateObserver<InterfaceModeConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver
	{
		public delegate void DataChangedHandler(InterfaceModeConfiguration data);

		private string mConfigurationFolderPath;

		private LoggerType loggerType;

		private DisplayMode displayMode;

		private DisplayMode lastNotifiedDisplayMode;

		private IModelValidator modelValidator;

		private IContainer components;

		private InterfaceModeGL3X00 interfaceModeGL3X00;

		private TabControl tabControl;

		private TabPage tabPageMonitoringMode;

		private TabPage tabPageSignalExport;

		private RepositoryItemCheckEdit repositoryItemCheckEditIsEnabled;

		private RepositoryItemComboBox repositoryItemEcuComboBox;

		private RepositoryItemComboBox repositoryItemMeasurementModeComboBox;

		private SignalExportList signalExportList;

		private ImageList imageListErrorIcons;

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.signalExportList.ApplicationDatabaseManager;
			}
			set
			{
				this.signalExportList.ApplicationDatabaseManager = value;
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
				this.signalExportList.ConfigurationFolderPath = value;
			}
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.modelValidator;
			}
			set
			{
				this.modelValidator = value;
				this.interfaceModeGL3X00.ModelValidator = this.modelValidator;
				this.signalExportList.ModelValidator = this.modelValidator;
			}
		}

		ISemanticChecker IPropertyWindow.SemanticChecker
		{
			get;
			set;
		}

		IModelEditor IPropertyWindow.ModelEditor
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
				return PageType.InterfaceMode;
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
				base.Visible = value;
			}
		}

		public List<Vector.VLConfig.Data.ConfigurationDataModel.Action> FocusedActions
		{
			get
			{
				return null;
			}
		}

		public InterfaceMode()
		{
			this.InitializeComponent();
			this.interfaceModeGL3X00.Validating += new EventHandler(this.OnInterfaceModeValidating);
			this.signalExportList.Validating += new EventHandler(this.OnInterfaceModeValidating);
			this.imageListErrorIcons.Images.Add(Resources.IconError.ToBitmap());
			this.imageListErrorIcons.Images.Add(Resources.ImageWarning);
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is InterfaceModeConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.InterfaceMode);
			}
		}

		void IPropertyWindow.Reset()
		{
			this.interfaceModeGL3X00.Reset();
			this.signalExportList.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			if (this.lastNotifiedDisplayMode != null && !this.lastNotifiedDisplayMode.Equals(this.displayMode))
			{
				((IUpdateObserver<DisplayMode>)this).Update(this.displayMode);
			}
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return true;
			case LoggerType.GL2000:
				return this.interfaceModeGL3X00.ValidateInput();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.interfaceModeGL3X00.ValidateInput() & this.signalExportList.ValidateInput(false);
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL2000:
				return this.interfaceModeGL3X00.HasErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.interfaceModeGL3X00.HasErrors() | this.signalExportList.HasErrors();
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL2000:
				return this.interfaceModeGL3X00.HasGlobalErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.interfaceModeGL3X00.HasGlobalErrors() | this.signalExportList.HasGlobalErrors();
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL2000:
				return this.interfaceModeGL3X00.HasLocalErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.interfaceModeGL3X00.HasLocalErrors() | this.signalExportList.HasLocalErrors();
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL2000:
				return this.interfaceModeGL3X00.HasFormatErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.interfaceModeGL3X00.HasFormatErrors() | this.signalExportList.HasFormatErrors();
			default:
				return true;
			}
		}

		void IUpdateObserver<InterfaceModeConfiguration>.Update(InterfaceModeConfiguration data)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL2000:
				this.interfaceModeGL3X00.InterfaceModeConfiguration = data;
				return;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.interfaceModeGL3X00.InterfaceModeConfiguration = data;
				this.signalExportList.InterfaceModeConfiguration = data;
				break;
			default:
				return;
			}
		}

		void IUpdateObserver<DisplayMode>.Update(DisplayMode mode)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL2000:
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.interfaceModeGL3X00.DisplayMode = mode;
				break;
			}
			this.displayMode = mode;
			this.lastNotifiedDisplayMode = new DisplayMode(mode);
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType data)
		{
			if (this.loggerType == data)
			{
				return;
			}
			this.loggerType = data;
			this.InitTabs();
		}

		private void InitTabs()
		{
			this.tabControl.Visible = false;
			foreach (TabPage tabPage in this.tabControl.TabPages)
			{
				tabPage.Controls.Clear();
			}
			this.interfaceModeGL3X00.Visible = false;
			this.signalExportList.Visible = false;
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL2000:
				this.interfaceModeGL3X00.Init();
				this.interfaceModeGL3X00.Visible = true;
				base.Controls.Add(this.interfaceModeGL3X00);
				return;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.tabControl.Visible = true;
				this.interfaceModeGL3X00.Init();
				this.interfaceModeGL3X00.Visible = true;
				this.tabPageMonitoringMode.Controls.Add(this.interfaceModeGL3X00);
				this.signalExportList.Visible = true;
				this.tabPageSignalExport.Controls.Add(this.signalExportList);
				break;
			default:
				return;
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

		private void OnInterfaceModeValidating(object sender, EventArgs e)
		{
			if (!sender.Equals(this.interfaceModeGL3X00))
			{
				if (sender.Equals(this.signalExportList))
				{
					if (this.signalExportList.HasLocalErrors())
					{
						this.tabPageSignalExport.ImageIndex = 0;
						return;
					}
					if (this.signalExportList.HasFormatErrors())
					{
						this.tabPageSignalExport.ImageIndex = 0;
						return;
					}
					if (this.signalExportList.HasErrors())
					{
						this.tabPageSignalExport.ImageIndex = 1;
						return;
					}
					this.tabPageSignalExport.ImageIndex = -1;
				}
				return;
			}
			if (this.interfaceModeGL3X00.HasLocalErrors())
			{
				this.tabPageMonitoringMode.ImageIndex = 0;
				return;
			}
			if (this.interfaceModeGL3X00.HasFormatErrors())
			{
				this.tabPageMonitoringMode.ImageIndex = 0;
				return;
			}
			if (this.interfaceModeGL3X00.HasErrors())
			{
				this.tabPageMonitoringMode.ImageIndex = 1;
				return;
			}
			this.tabPageMonitoringMode.ImageIndex = -1;
		}

		public bool Serialize(SignalExportListPage signalExportListPage)
		{
			if (signalExportListPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.signalExportList.Serialize(signalExportListPage);
		}

		public bool DeSerialize(SignalExportListPage signalExportListPage)
		{
			if (signalExportListPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.signalExportList.DeSerialize(signalExportListPage);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(InterfaceMode));
			this.repositoryItemCheckEditIsEnabled = new RepositoryItemCheckEdit();
			this.repositoryItemEcuComboBox = new RepositoryItemComboBox();
			this.repositoryItemMeasurementModeComboBox = new RepositoryItemComboBox();
			this.tabControl = new TabControl();
			this.tabPageMonitoringMode = new TabPage();
			this.interfaceModeGL3X00 = new InterfaceModeGL3X00();
			this.tabPageSignalExport = new TabPage();
			this.signalExportList = new SignalExportList();
			this.imageListErrorIcons = new ImageList(this.components);
			((ISupportInitialize)this.repositoryItemCheckEditIsEnabled).BeginInit();
			((ISupportInitialize)this.repositoryItemEcuComboBox).BeginInit();
			((ISupportInitialize)this.repositoryItemMeasurementModeComboBox).BeginInit();
			this.tabControl.SuspendLayout();
			this.tabPageMonitoringMode.SuspendLayout();
			this.tabPageSignalExport.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditIsEnabled, "repositoryItemCheckEditIsEnabled");
			this.repositoryItemCheckEditIsEnabled.Name = "repositoryItemCheckEditIsEnabled";
			componentResourceManager.ApplyResources(this.repositoryItemEcuComboBox, "repositoryItemEcuComboBox");
			this.repositoryItemEcuComboBox.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemEcuComboBox.Buttons"))
			});
			this.repositoryItemEcuComboBox.Name = "repositoryItemEcuComboBox";
			this.repositoryItemEcuComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;
			componentResourceManager.ApplyResources(this.repositoryItemMeasurementModeComboBox, "repositoryItemMeasurementModeComboBox");
			this.repositoryItemMeasurementModeComboBox.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemMeasurementModeComboBox.Buttons"))
			});
			this.repositoryItemMeasurementModeComboBox.Name = "repositoryItemMeasurementModeComboBox";
			this.repositoryItemMeasurementModeComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.tabControl.Controls.Add(this.tabPageMonitoringMode);
			this.tabControl.Controls.Add(this.tabPageSignalExport);
			componentResourceManager.ApplyResources(this.tabControl, "tabControl");
			this.tabControl.ImageList = this.imageListErrorIcons;
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabPageMonitoringMode.BackColor = SystemColors.Control;
			this.tabPageMonitoringMode.Controls.Add(this.interfaceModeGL3X00);
			componentResourceManager.ApplyResources(this.tabPageMonitoringMode, "tabPageMonitoringMode");
			this.tabPageMonitoringMode.Name = "tabPageMonitoringMode";
			this.interfaceModeGL3X00.DisplayMode = null;
			componentResourceManager.ApplyResources(this.interfaceModeGL3X00, "interfaceModeGL3X00");
			this.interfaceModeGL3X00.InterfaceModeConfiguration = null;
			this.interfaceModeGL3X00.ModelValidator = null;
			this.interfaceModeGL3X00.Name = "interfaceModeGL3X00";
			this.tabPageSignalExport.BackColor = SystemColors.Control;
			this.tabPageSignalExport.Controls.Add(this.signalExportList);
			componentResourceManager.ApplyResources(this.tabPageSignalExport, "tabPageSignalExport");
			this.tabPageSignalExport.Name = "tabPageSignalExport";
			this.signalExportList.ApplicationDatabaseManager = null;
			this.signalExportList.ConfigurationFolderPath = null;
			componentResourceManager.ApplyResources(this.signalExportList, "signalExportList");
			this.signalExportList.InterfaceModeConfiguration = null;
			this.signalExportList.ModelValidator = null;
			this.signalExportList.Name = "signalExportList";
			this.imageListErrorIcons.ColorDepth = ColorDepth.Depth8Bit;
			componentResourceManager.ApplyResources(this.imageListErrorIcons, "imageListErrorIcons");
			this.imageListErrorIcons.TransparentColor = Color.Transparent;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.tabControl);
			base.Name = "InterfaceMode";
			((ISupportInitialize)this.repositoryItemCheckEditIsEnabled).EndInit();
			((ISupportInitialize)this.repositoryItemEcuComboBox).EndInit();
			((ISupportInitialize)this.repositoryItemMeasurementModeComboBox).EndInit();
			this.tabControl.ResumeLayout(false);
			this.tabPageMonitoringMode.ResumeLayout(false);
			this.tabPageSignalExport.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
