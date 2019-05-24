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

namespace Vector.VLConfig.GUI.HardwareSettingsPage
{
	public class HardwareSettings : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LogDataStorage>, IUpdateObserver<DisplayMode>, IUpdateObserver<LoggerType>, IUpdateObserver
	{
		public delegate void DataChangedHandler(LogDataStorage data);

		private LoggerType loggerType;

		private IContainer components;

		private HardwareSettingsGL1000 hardwareSettingsGL1000;

		private HardwareSettingsGL3000 hardwareSettingsGL3000;

		private HardwareSettingsGL4000 hardwareSettingsGL4000;

		private HardwareSettingsGL2000 hardwareSettingsGL2000;

		private HardwareSettingsGL1020FTE hardwareSettingsGL1020FTE;

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.hardwareSettingsGL1000.ApplicationDatabaseManager;
			}
			set
			{
				this.hardwareSettingsGL1000.ApplicationDatabaseManager = value;
				this.hardwareSettingsGL1020FTE.ApplicationDatabaseManager = value;
				this.hardwareSettingsGL2000.ApplicationDatabaseManager = value;
				this.hardwareSettingsGL3000.ApplicationDatabaseManager = value;
				this.hardwareSettingsGL4000.ApplicationDatabaseManager = value;
			}
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.hardwareSettingsGL1000.ModelValidator;
			}
			set
			{
				this.hardwareSettingsGL1000.ModelValidator = value;
				this.hardwareSettingsGL1020FTE.ModelValidator = value;
				this.hardwareSettingsGL2000.ModelValidator = value;
				this.hardwareSettingsGL3000.ModelValidator = value;
				this.hardwareSettingsGL4000.ModelValidator = value;
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
				return PageType.HardwareSettings;
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

		public HardwareSettings()
		{
			this.InitializeComponent();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is LogDataStorage;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.HardwareSettings);
			}
		}

		void IPropertyWindow.Reset()
		{
			this.hardwareSettingsGL1000.Reset();
			this.hardwareSettingsGL1020FTE.Reset();
			this.hardwareSettingsGL2000.Reset();
			this.hardwareSettingsGL3000.Reset();
			this.hardwareSettingsGL4000.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.hardwareSettingsGL1000.ValidateInput(false);
			case LoggerType.GL1020FTE:
				return this.hardwareSettingsGL1020FTE.ValidateInput(false);
			case LoggerType.GL2000:
				return this.hardwareSettingsGL2000.ValidateInput(false);
			case LoggerType.GL3000:
				return this.hardwareSettingsGL3000.ValidateInput(false);
			case LoggerType.GL4000:
				return this.hardwareSettingsGL4000.ValidateInput(false);
			case LoggerType.VN1630log:
				return true;
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.hardwareSettingsGL1000.HasErrors();
			case LoggerType.GL1020FTE:
				return this.hardwareSettingsGL1020FTE.HasErrors();
			case LoggerType.GL2000:
				return this.hardwareSettingsGL2000.HasErrors();
			case LoggerType.GL3000:
				return this.hardwareSettingsGL3000.HasErrors();
			case LoggerType.GL4000:
				return this.hardwareSettingsGL4000.HasErrors();
			case LoggerType.VN1630log:
				return false;
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.hardwareSettingsGL1000.HasGlobalErrors();
			case LoggerType.GL1020FTE:
				return this.hardwareSettingsGL1020FTE.HasGlobalErrors();
			case LoggerType.GL2000:
				return this.hardwareSettingsGL2000.HasGlobalErrors();
			case LoggerType.GL3000:
				return this.hardwareSettingsGL3000.HasGlobalErrors();
			case LoggerType.GL4000:
				return this.hardwareSettingsGL4000.HasGlobalErrors();
			case LoggerType.VN1630log:
				return false;
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.hardwareSettingsGL1000.HasLocalErrors();
			case LoggerType.GL1020FTE:
				return this.hardwareSettingsGL1020FTE.HasLocalErrors();
			case LoggerType.GL2000:
				return this.hardwareSettingsGL2000.HasLocalErrors();
			case LoggerType.GL3000:
				return this.hardwareSettingsGL3000.HasLocalErrors();
			case LoggerType.GL4000:
				return this.hardwareSettingsGL4000.HasLocalErrors();
			case LoggerType.VN1630log:
				return false;
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.hardwareSettingsGL1000.HasFormatErrors();
			case LoggerType.GL1020FTE:
				return this.hardwareSettingsGL1020FTE.HasFormatErrors();
			case LoggerType.GL2000:
				return this.hardwareSettingsGL2000.HasFormatErrors();
			case LoggerType.GL3000:
				return this.hardwareSettingsGL3000.HasFormatErrors();
			case LoggerType.GL4000:
				return this.hardwareSettingsGL4000.HasFormatErrors();
			case LoggerType.VN1630log:
				return false;
			default:
				return true;
			}
		}

		void IUpdateObserver<LogDataStorage>.Update(LogDataStorage data)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				this.hardwareSettingsGL1000.LogDataStorage = data;
				return;
			case LoggerType.GL1020FTE:
				this.hardwareSettingsGL1020FTE.LogDataStorage = data;
				return;
			case LoggerType.GL2000:
				this.hardwareSettingsGL2000.LogDataStorage = data;
				return;
			case LoggerType.GL3000:
				this.hardwareSettingsGL3000.LogDataStorage = data;
				return;
			case LoggerType.GL4000:
				this.hardwareSettingsGL4000.LogDataStorage = data;
				break;
			case LoggerType.VN1630log:
				break;
			default:
				return;
			}
		}

		void IUpdateObserver<DisplayMode>.Update(DisplayMode mode)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				this.hardwareSettingsGL1000.DisplayMode = mode;
				return;
			case LoggerType.GL1020FTE:
				this.hardwareSettingsGL1020FTE.DisplayMode = mode;
				return;
			case LoggerType.GL2000:
				this.hardwareSettingsGL2000.DisplayMode = mode;
				return;
			case LoggerType.GL3000:
				this.hardwareSettingsGL3000.DisplayMode = mode;
				return;
			case LoggerType.GL4000:
				this.hardwareSettingsGL4000.DisplayMode = mode;
				break;
			case LoggerType.VN1630log:
				break;
			default:
				return;
			}
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType data)
		{
			this.loggerType = data;
			this.hardwareSettingsGL1000.Visible = false;
			this.hardwareSettingsGL1020FTE.Visible = false;
			this.hardwareSettingsGL2000.Visible = false;
			this.hardwareSettingsGL3000.Visible = false;
			this.hardwareSettingsGL4000.Visible = false;
			if (this.loggerType == LoggerType.GL1000)
			{
				this.hardwareSettingsGL1000.Init();
				this.hardwareSettingsGL1000.Visible = true;
			}
			if (this.loggerType == LoggerType.GL1020FTE)
			{
				this.hardwareSettingsGL1020FTE.Init();
				this.hardwareSettingsGL1020FTE.Visible = true;
			}
			if (this.loggerType == LoggerType.GL2000)
			{
				this.hardwareSettingsGL2000.Init();
				this.hardwareSettingsGL2000.Visible = true;
			}
			if (this.loggerType == LoggerType.GL3000)
			{
				this.hardwareSettingsGL3000.Init();
				this.hardwareSettingsGL3000.Visible = true;
			}
			if (this.loggerType == LoggerType.GL4000)
			{
				this.hardwareSettingsGL4000.Init();
				this.hardwareSettingsGL4000.Visible = true;
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
			this.hardwareSettingsGL4000 = new HardwareSettingsGL4000();
			this.hardwareSettingsGL3000 = new HardwareSettingsGL3000();
			this.hardwareSettingsGL1000 = new HardwareSettingsGL1000();
			this.hardwareSettingsGL2000 = new HardwareSettingsGL2000();
			this.hardwareSettingsGL1020FTE = new HardwareSettingsGL1020FTE();
			base.SuspendLayout();
			this.hardwareSettingsGL4000.AutoValidate = AutoValidate.EnableAllowFocusChange;
			this.hardwareSettingsGL4000.DisplayMode = null;
			this.hardwareSettingsGL4000.Dock = DockStyle.Fill;
			this.hardwareSettingsGL4000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.hardwareSettingsGL4000.Location = new Point(0, 0);
			this.hardwareSettingsGL4000.LogDataStorage = null;
			this.hardwareSettingsGL4000.Name = "hardwareSettingsGL4000";
			this.hardwareSettingsGL4000.Size = new Size(526, 402);
			this.hardwareSettingsGL4000.TabIndex = 2;
			this.hardwareSettingsGL3000.AutoValidate = AutoValidate.EnableAllowFocusChange;
			this.hardwareSettingsGL3000.DisplayMode = null;
			this.hardwareSettingsGL3000.Dock = DockStyle.Fill;
			this.hardwareSettingsGL3000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.hardwareSettingsGL3000.Location = new Point(0, 0);
			this.hardwareSettingsGL3000.LogDataStorage = null;
			this.hardwareSettingsGL3000.Name = "hardwareSettingsGL3000";
			this.hardwareSettingsGL3000.Size = new Size(526, 402);
			this.hardwareSettingsGL3000.TabIndex = 1;
			this.hardwareSettingsGL1000.AutoValidate = AutoValidate.EnableAllowFocusChange;
			this.hardwareSettingsGL1000.DisplayMode = null;
			this.hardwareSettingsGL1000.Dock = DockStyle.Fill;
			this.hardwareSettingsGL1000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.hardwareSettingsGL1000.Location = new Point(0, 0);
			this.hardwareSettingsGL1000.LogDataStorage = null;
			this.hardwareSettingsGL1000.Name = "hardwareSettingsGL1000";
			this.hardwareSettingsGL1000.Size = new Size(526, 402);
			this.hardwareSettingsGL1000.TabIndex = 0;
			this.hardwareSettingsGL2000.AutoValidate = AutoValidate.EnableAllowFocusChange;
			this.hardwareSettingsGL2000.DisplayMode = null;
			this.hardwareSettingsGL2000.Dock = DockStyle.Fill;
			this.hardwareSettingsGL2000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.hardwareSettingsGL2000.Location = new Point(0, 0);
			this.hardwareSettingsGL2000.LogDataStorage = null;
			this.hardwareSettingsGL2000.Name = "hardwareSettingsGL2000";
			this.hardwareSettingsGL2000.Size = new Size(526, 402);
			this.hardwareSettingsGL2000.TabIndex = 3;
			this.hardwareSettingsGL1020FTE.AutoValidate = AutoValidate.EnableAllowFocusChange;
			this.hardwareSettingsGL1020FTE.DisplayMode = null;
			this.hardwareSettingsGL1020FTE.Dock = DockStyle.Fill;
			this.hardwareSettingsGL1020FTE.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.hardwareSettingsGL1020FTE.Location = new Point(0, 0);
			this.hardwareSettingsGL1020FTE.LogDataStorage = null;
			this.hardwareSettingsGL1020FTE.Name = "hardwareSettingsGL1020FTE";
			this.hardwareSettingsGL1020FTE.Size = new Size(526, 402);
			this.hardwareSettingsGL1020FTE.TabIndex = 4;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			this.AutoValidate = AutoValidate.EnableAllowFocusChange;
			base.Controls.Add(this.hardwareSettingsGL1020FTE);
			base.Controls.Add(this.hardwareSettingsGL2000);
			base.Controls.Add(this.hardwareSettingsGL4000);
			base.Controls.Add(this.hardwareSettingsGL3000);
			base.Controls.Add(this.hardwareSettingsGL1000);
			base.Name = "HardwareSettings";
			base.Size = new Size(526, 402);
			base.ResumeLayout(false);
		}
	}
}
