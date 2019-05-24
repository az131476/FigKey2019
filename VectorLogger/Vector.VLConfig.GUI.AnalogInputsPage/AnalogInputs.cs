using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.AnalogInputsPage
{
	public class AnalogInputs : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LoggerType>, IUpdateObserver<AnalogInputConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver
	{
		public delegate void DataChangedHandler(AnalogInputConfiguration data);

		private LoggerType loggerType;

		private DisplayMode displayMode;

		private DisplayMode lastNotifiedDisplayMode;

		private IContainer components;

		private AnalogInputsGL1000 analogInputsGL1000;

		private AnalogInputsGL3Plus analogInputsGL3000;

		private AnalogInputsGL3Plus analogInputsGL4000;

		private AnalogInputsGL2000 analogInputsGL2000;

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.analogInputsGL1000.ModelValidator;
			}
			set
			{
				this.analogInputsGL1000.ModelValidator = value;
				this.analogInputsGL2000.ModelValidator = value;
				this.analogInputsGL3000.ModelValidator = value;
				this.analogInputsGL4000.ModelValidator = value;
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
				return PageType.AnalogInputs;
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

		public AnalogInputs()
		{
			this.InitializeComponent();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is AnalogInputConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.AnalogInputs);
			}
		}

		void IPropertyWindow.Reset()
		{
			this.analogInputsGL1000.Reset();
			this.analogInputsGL2000.Reset();
			this.analogInputsGL3000.Reset();
			this.analogInputsGL4000.Reset();
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
				return this.analogInputsGL1000.ValidateInput();
			case LoggerType.GL2000:
				return this.analogInputsGL2000.ValidateInput();
			case LoggerType.GL3000:
				return this.analogInputsGL3000.ValidateInput();
			case LoggerType.GL4000:
				return this.analogInputsGL4000.ValidateInput();
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
				return this.analogInputsGL1000.HasErrors();
			case LoggerType.GL2000:
				return this.analogInputsGL2000.HasErrors();
			case LoggerType.GL3000:
				return this.analogInputsGL3000.HasErrors();
			case LoggerType.GL4000:
				return this.analogInputsGL4000.HasErrors();
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
				return this.analogInputsGL1000.HasGlobalErrors();
			case LoggerType.GL2000:
				return this.analogInputsGL2000.HasGlobalErrors();
			case LoggerType.GL3000:
				return this.analogInputsGL3000.HasGlobalErrors();
			case LoggerType.GL4000:
				return this.analogInputsGL4000.HasGlobalErrors();
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
				return this.analogInputsGL1000.HasLocalErrors();
			case LoggerType.GL2000:
				return this.analogInputsGL2000.HasLocalErrors();
			case LoggerType.GL3000:
				return this.analogInputsGL3000.HasLocalErrors();
			case LoggerType.GL4000:
				return this.analogInputsGL4000.HasLocalErrors();
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
				return this.analogInputsGL1000.HasFormatErrors();
			case LoggerType.GL2000:
				return this.analogInputsGL2000.HasFormatErrors();
			case LoggerType.GL3000:
				return this.analogInputsGL3000.HasFormatErrors();
			case LoggerType.GL4000:
				return this.analogInputsGL4000.HasFormatErrors();
			case LoggerType.VN1630log:
				return false;
			default:
				return true;
			}
		}

		void IUpdateObserver<AnalogInputConfiguration>.Update(AnalogInputConfiguration data)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
				this.analogInputsGL1000.AnalogInputConfiguration = data;
				return;
			case LoggerType.GL2000:
				this.analogInputsGL2000.AnalogInputConfiguration = data;
				return;
			case LoggerType.GL3000:
				this.analogInputsGL3000.AnalogInputConfiguration = data;
				return;
			case LoggerType.GL4000:
				this.analogInputsGL4000.AnalogInputConfiguration = data;
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
				this.analogInputsGL1000.DisplayMode = mode;
				break;
			case LoggerType.GL2000:
				this.analogInputsGL2000.DisplayMode = mode;
				break;
			case LoggerType.GL3000:
				this.analogInputsGL3000.DisplayMode = mode;
				break;
			case LoggerType.GL4000:
				this.analogInputsGL4000.DisplayMode = mode;
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
			this.analogInputsGL1000.Visible = false;
			this.analogInputsGL2000.Visible = false;
			this.analogInputsGL3000.Visible = false;
			this.analogInputsGL4000.Visible = false;
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
				this.analogInputsGL1000.Init();
				this.analogInputsGL1000.Visible = true;
				return;
			case LoggerType.GL2000:
				this.analogInputsGL2000.Init();
				this.analogInputsGL2000.Visible = true;
				return;
			case LoggerType.GL3000:
				this.analogInputsGL3000.Init();
				this.analogInputsGL3000.Visible = true;
				return;
			case LoggerType.GL4000:
				this.analogInputsGL4000.Init();
				this.analogInputsGL4000.Visible = true;
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
			this.analogInputsGL1000 = new AnalogInputsGL1000();
			this.analogInputsGL3000 = new AnalogInputsGL3Plus();
			this.analogInputsGL4000 = new AnalogInputsGL3Plus();
			this.analogInputsGL2000 = new AnalogInputsGL2000();
			base.SuspendLayout();
			this.analogInputsGL1000.AnalogInputConfiguration = null;
			this.analogInputsGL1000.AutoValidate = AutoValidate.EnableAllowFocusChange;
			this.analogInputsGL1000.DisplayMode = null;
			this.analogInputsGL1000.Dock = DockStyle.Fill;
			this.analogInputsGL1000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.analogInputsGL1000.Location = new Point(0, 0);
			this.analogInputsGL1000.ModelValidator = null;
			this.analogInputsGL1000.Name = "analogInputsGL1000";
			this.analogInputsGL1000.Size = new Size(754, 762);
			this.analogInputsGL1000.TabIndex = 0;
			this.analogInputsGL3000.AnalogInputConfiguration = null;
			this.analogInputsGL3000.AutoValidate = AutoValidate.EnableAllowFocusChange;
			this.analogInputsGL3000.DisplayMode = null;
			this.analogInputsGL3000.Dock = DockStyle.Fill;
			this.analogInputsGL3000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.analogInputsGL3000.Location = new Point(0, 0);
			this.analogInputsGL3000.ModelValidator = null;
			this.analogInputsGL3000.Name = "analogInputsGL3000";
			this.analogInputsGL3000.Size = new Size(754, 762);
			this.analogInputsGL3000.TabIndex = 1;
			this.analogInputsGL4000.AnalogInputConfiguration = null;
			this.analogInputsGL4000.AutoValidate = AutoValidate.EnableAllowFocusChange;
			this.analogInputsGL4000.DisplayMode = null;
			this.analogInputsGL4000.Dock = DockStyle.Fill;
			this.analogInputsGL4000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.analogInputsGL4000.Location = new Point(0, 0);
			this.analogInputsGL4000.ModelValidator = null;
			this.analogInputsGL4000.Name = "analogInputsGL4000";
			this.analogInputsGL4000.Size = new Size(754, 762);
			this.analogInputsGL4000.TabIndex = 2;
			this.analogInputsGL2000.AnalogInputConfiguration = null;
			this.analogInputsGL2000.AutoValidate = AutoValidate.EnableAllowFocusChange;
			this.analogInputsGL2000.DisplayMode = null;
			this.analogInputsGL2000.Dock = DockStyle.Fill;
			this.analogInputsGL2000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.analogInputsGL2000.Location = new Point(0, 0);
			this.analogInputsGL2000.ModelValidator = null;
			this.analogInputsGL2000.Name = "analogInputsGL2000";
			this.analogInputsGL2000.Size = new Size(754, 762);
			this.analogInputsGL2000.TabIndex = 3;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.analogInputsGL2000);
			base.Controls.Add(this.analogInputsGL4000);
			base.Controls.Add(this.analogInputsGL3000);
			base.Controls.Add(this.analogInputsGL1000);
			base.Name = "AnalogInputs";
			base.Size = new Size(754, 762);
			base.ResumeLayout(false);
		}
	}
}
