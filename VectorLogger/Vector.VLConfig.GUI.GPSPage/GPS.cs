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

namespace Vector.VLConfig.GUI.GPSPage
{
	public class GPS : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<GPSConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver<LoggerType>, IUpdateObserver
	{
		public delegate void DataChangedHandler(GPSConfiguration data);

		private LoggerType loggerType;

		private DisplayMode displayMode;

		private DisplayMode lastNotifiedDisplayMode;

		private IContainer components;

		private GPSGL2000 gpsGL2000;

		private GPSGL3Plus gpsGL3Plus;

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.gpsGL3Plus.ApplicationDatabaseManager;
			}
			set
			{
				this.gpsGL3Plus.ApplicationDatabaseManager = value;
			}
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.gpsGL2000.ModelValidator;
			}
			set
			{
				this.gpsGL2000.ModelValidator = value;
				this.gpsGL3Plus.ModelValidator = value;
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
				if (this.loggerType == LoggerType.GL2000)
				{
					return PageType.GPS;
				}
				return PageType.CANgps;
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

		public GPS()
		{
			this.InitializeComponent();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is GPSConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.GPS);
			}
		}

		void IPropertyWindow.Reset()
		{
			this.gpsGL2000.Reset();
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
				return this.gpsGL2000.ValidateInput();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.gpsGL3Plus.ValidateInput();
			default:
				return false;
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
				return this.gpsGL2000.HasErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.gpsGL3Plus.HasErrors();
			default:
				return false;
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
				return this.gpsGL2000.HasGlobalErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.gpsGL3Plus.HasGlobalErrors();
			default:
				return false;
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
				return this.gpsGL2000.HasLocalErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.gpsGL3Plus.HasLocalErrors();
			default:
				return false;
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
				return this.gpsGL2000.HasFormatErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.gpsGL3Plus.HasFormatErrors();
			default:
				return false;
			}
		}

		void IUpdateObserver<GPSConfiguration>.Update(GPSConfiguration data)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL2000:
				this.gpsGL2000.GPSConfiguration = data;
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.gpsGL3Plus.GPSConfiguration = data;
				return;
			default:
				return;
			}
		}

		void IUpdateObserver<DisplayMode>.Update(DisplayMode mode)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL2000:
				this.gpsGL2000.DisplayMode = mode;
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.gpsGL3Plus.DisplayMode = mode;
				break;
			}
			this.displayMode = mode;
			this.lastNotifiedDisplayMode = new DisplayMode(mode);
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType type)
		{
			if (this.loggerType == type)
			{
				return;
			}
			this.loggerType = type;
			this.gpsGL3Plus.Visible = false;
			this.gpsGL2000.Visible = false;
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL2000:
				this.gpsGL2000.Init();
				this.gpsGL2000.Visible = true;
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.gpsGL3Plus.Init();
				this.gpsGL3Plus.Visible = true;
				return;
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
			this.gpsGL2000 = new GPSGL2000();
			this.gpsGL3Plus = new GPSGL3Plus();
			base.SuspendLayout();
			this.gpsGL2000.DisplayMode = null;
			this.gpsGL2000.Dock = DockStyle.Fill;
			this.gpsGL2000.GPSConfiguration = null;
			this.gpsGL2000.Location = new Point(0, 0);
			this.gpsGL2000.ModelValidator = null;
			this.gpsGL2000.Name = "gpsGL2000";
			this.gpsGL2000.Size = new Size(634, 588);
			this.gpsGL2000.TabIndex = 0;
			this.gpsGL3Plus.AutoValidate = AutoValidate.EnableAllowFocusChange;
			this.gpsGL3Plus.Dock = DockStyle.Fill;
			this.gpsGL3Plus.Location = new Point(0, 0);
			this.gpsGL3Plus.Name = "gpsGL3Plus";
			this.gpsGL3Plus.Size = new Size(634, 588);
			this.gpsGL3Plus.TabIndex = 1;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.gpsGL3Plus);
			base.Controls.Add(this.gpsGL2000);
			base.Name = "GPS";
			base.Size = new Size(634, 588);
			base.ResumeLayout(false);
		}
	}
}
