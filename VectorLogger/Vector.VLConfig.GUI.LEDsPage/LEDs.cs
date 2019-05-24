using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.LEDsPage
{
	public class LEDs : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LoggerType>, IUpdateObserver<LEDConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver
	{
		public delegate void DataChangedHandler(LEDConfiguration data);

		private LoggerType loggerType;

		private IContainer components;

		private LEDsGLXX00 ledsGL3000;

		private LEDsGLXX00 ledsGL4000;

		private LEDsGL1000 ledsGL1000;

		private LEDsGL2000 ledsGL2000;

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.ledsGL1000.ModelValidator;
			}
			set
			{
				this.ledsGL1000.ModelValidator = value;
				this.ledsGL2000.ModelValidator = value;
				this.ledsGL3000.ModelValidator = value;
				this.ledsGL4000.ModelValidator = value;
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
				return PageType.LEDs;
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

		public LEDs()
		{
			this.InitializeComponent();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is LEDConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.LEDs);
			}
		}

		void IPropertyWindow.Reset()
		{
			this.ledsGL1000.Reset();
			this.ledsGL2000.Reset();
			this.ledsGL3000.Reset();
			this.ledsGL4000.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
				return this.ledsGL1000.ValidateInput();
			case LoggerType.GL2000:
				return this.ledsGL2000.ValidateInput();
			case LoggerType.GL3000:
				return this.ledsGL3000.ValidateInput();
			case LoggerType.GL4000:
				return this.ledsGL4000.ValidateInput();
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
			case LoggerType.GL1020FTE:
				return this.ledsGL1000.HasErrors();
			case LoggerType.GL2000:
				return this.ledsGL2000.HasErrors();
			case LoggerType.GL3000:
				return this.ledsGL3000.HasErrors();
			case LoggerType.GL4000:
				return this.ledsGL4000.HasErrors();
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
			case LoggerType.GL1020FTE:
				return this.ledsGL1000.HasGlobalErrors();
			case LoggerType.GL2000:
				return this.ledsGL2000.HasGlobalErrors();
			case LoggerType.GL3000:
				return this.ledsGL3000.HasGlobalErrors();
			case LoggerType.GL4000:
				return this.ledsGL4000.HasErrors();
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
			case LoggerType.GL1020FTE:
				return this.ledsGL1000.HasLocalErrors();
			case LoggerType.GL2000:
				return this.ledsGL2000.HasLocalErrors();
			case LoggerType.GL3000:
				return this.ledsGL3000.HasLocalErrors();
			case LoggerType.GL4000:
				return this.ledsGL4000.HasLocalErrors();
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
			case LoggerType.GL1020FTE:
				return this.ledsGL1000.HasFormatErrors();
			case LoggerType.GL2000:
				return this.ledsGL2000.HasFormatErrors();
			case LoggerType.GL3000:
				return this.ledsGL3000.HasFormatErrors();
			case LoggerType.GL4000:
				return this.ledsGL4000.HasFormatErrors();
			case LoggerType.VN1630log:
				return false;
			default:
				return true;
			}
		}

		void IUpdateObserver<LEDConfiguration>.Update(LEDConfiguration data)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
				this.ledsGL1000.LEDConfiguration = data;
				return;
			case LoggerType.GL2000:
				this.ledsGL2000.LEDConfiguration = data;
				return;
			case LoggerType.GL3000:
				this.ledsGL3000.LEDConfiguration = data;
				return;
			case LoggerType.GL4000:
				this.ledsGL4000.LEDConfiguration = data;
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
			case LoggerType.GL1020FTE:
				this.ledsGL1000.DisplayMode = mode;
				return;
			case LoggerType.GL2000:
				this.ledsGL2000.DisplayMode = mode;
				return;
			case LoggerType.GL3000:
				this.ledsGL3000.DisplayMode = mode;
				return;
			case LoggerType.GL4000:
				this.ledsGL4000.DisplayMode = mode;
				break;
			case LoggerType.VN1630log:
				break;
			default:
				return;
			}
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType data)
		{
			if (this.loggerType == data)
			{
				return;
			}
			this.loggerType = data;
			this.ledsGL1000.Visible = false;
			this.ledsGL2000.Visible = false;
			this.ledsGL3000.Visible = false;
			this.ledsGL4000.Visible = false;
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
				this.ledsGL1000.Init();
				this.ledsGL1000.Visible = true;
				return;
			case LoggerType.GL2000:
				this.ledsGL2000.Init();
				this.ledsGL2000.Visible = true;
				return;
			case LoggerType.GL3000:
				this.ledsGL3000.Init();
				this.ledsGL3000.Visible = true;
				return;
			case LoggerType.GL4000:
				this.ledsGL4000.Init();
				this.ledsGL4000.Visible = true;
				break;
			case LoggerType.VN1630log:
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
			this.ledsGL3000 = new LEDsGLXX00();
			this.ledsGL4000 = new LEDsGLXX00();
			this.ledsGL1000 = new LEDsGL1000();
			this.ledsGL2000 = new LEDsGL2000();
			base.SuspendLayout();
			this.ledsGL3000.DisplayMode = null;
			this.ledsGL3000.Dock = DockStyle.Fill;
			this.ledsGL3000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ledsGL3000.LEDConfiguration = null;
			this.ledsGL3000.Location = new Point(0, 0);
			this.ledsGL3000.Name = "ledsGL3000";
			this.ledsGL3000.Size = new Size(352, 478);
			this.ledsGL3000.TabIndex = 1;
			this.ledsGL4000.DisplayMode = null;
			this.ledsGL4000.Dock = DockStyle.Fill;
			this.ledsGL4000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ledsGL4000.LEDConfiguration = null;
			this.ledsGL4000.Location = new Point(0, 0);
			this.ledsGL4000.Name = "ledsGL4000";
			this.ledsGL4000.Size = new Size(352, 478);
			this.ledsGL4000.TabIndex = 2;
			this.ledsGL1000.DisplayMode = null;
			this.ledsGL1000.Dock = DockStyle.Fill;
			this.ledsGL1000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ledsGL1000.LEDConfiguration = null;
			this.ledsGL1000.Location = new Point(0, 0);
			this.ledsGL1000.Name = "ledsGL1000";
			this.ledsGL1000.Size = new Size(352, 478);
			this.ledsGL1000.TabIndex = 3;
			this.ledsGL2000.DisplayMode = null;
			this.ledsGL2000.Dock = DockStyle.Fill;
			this.ledsGL2000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ledsGL2000.LEDConfiguration = null;
			this.ledsGL2000.Location = new Point(0, 0);
			this.ledsGL2000.Name = "ledsGL2000";
			this.ledsGL2000.Size = new Size(352, 478);
			this.ledsGL2000.TabIndex = 4;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.ledsGL2000);
			base.Controls.Add(this.ledsGL1000);
			base.Controls.Add(this.ledsGL4000);
			base.Controls.Add(this.ledsGL3000);
			base.Name = "LEDs";
			base.Size = new Size(352, 478);
			base.ResumeLayout(false);
		}
	}
}
