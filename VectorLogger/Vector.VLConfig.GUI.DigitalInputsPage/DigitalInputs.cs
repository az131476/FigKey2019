using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.DigitalInputsPage
{
	public class DigitalInputs : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LoggerType>, IUpdateObserver<DigitalInputConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver
	{
		public delegate void DataChangedHandler(DigitalInputConfiguration data);

		private LoggerType loggerType;

		private DisplayMode displayMode;

		private DisplayMode lastNotifiedDisplayMode;

		private IContainer components;

		private DigitalInputsGL1000 digitalInputsGL1000;

		private DigitalInputsGL3Plus digitalInputsGL3000;

		private DigitalInputsGL3Plus digitalInputsGL4000;

		private DigitalInputsGL2000 digitalInputsGL2000;

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.digitalInputsGL1000.ModelValidator;
			}
			set
			{
				this.digitalInputsGL1000.ModelValidator = value;
				this.digitalInputsGL2000.ModelValidator = value;
				this.digitalInputsGL3000.ModelValidator = value;
				this.digitalInputsGL4000.ModelValidator = value;
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
				return PageType.DigitalInputs;
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

		public DigitalInputs()
		{
			this.InitializeComponent();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is DigitalInputConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.DigitalInputs);
			}
		}

		void IPropertyWindow.Reset()
		{
			this.digitalInputsGL1000.Reset();
			this.digitalInputsGL2000.Reset();
			this.digitalInputsGL3000.Reset();
			this.digitalInputsGL4000.Reset();
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
				return this.digitalInputsGL1000.ValidateInput();
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return true;
			case LoggerType.GL2000:
				return this.digitalInputsGL2000.ValidateInput();
			case LoggerType.GL3000:
				return this.digitalInputsGL3000.ValidateInput();
			case LoggerType.GL4000:
				return this.digitalInputsGL4000.ValidateInput();
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.digitalInputsGL1000.HasErrors();
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL2000:
				return this.digitalInputsGL2000.HasErrors();
			case LoggerType.GL3000:
				return this.digitalInputsGL3000.HasErrors();
			case LoggerType.GL4000:
				return this.digitalInputsGL4000.HasErrors();
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.digitalInputsGL1000.HasGlobalErrors();
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL2000:
				return this.digitalInputsGL2000.HasGlobalErrors();
			case LoggerType.GL3000:
				return this.digitalInputsGL3000.HasGlobalErrors();
			case LoggerType.GL4000:
				return this.digitalInputsGL4000.HasGlobalErrors();
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.digitalInputsGL1000.HasLocalErrors();
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL2000:
				return this.digitalInputsGL2000.HasLocalErrors();
			case LoggerType.GL3000:
				return this.digitalInputsGL3000.HasLocalErrors();
			case LoggerType.GL4000:
				return this.digitalInputsGL4000.HasLocalErrors();
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.digitalInputsGL1000.HasFormatErrors();
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL2000:
				return this.digitalInputsGL2000.HasFormatErrors();
			case LoggerType.GL3000:
				return this.digitalInputsGL3000.HasFormatErrors();
			case LoggerType.GL4000:
				return this.digitalInputsGL4000.HasFormatErrors();
			default:
				return true;
			}
		}

		void IUpdateObserver<DigitalInputConfiguration>.Update(DigitalInputConfiguration data)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				this.digitalInputsGL1000.DigitalInputConfiguration = data;
				return;
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL2000:
				this.digitalInputsGL2000.DigitalInputConfiguration = data;
				return;
			case LoggerType.GL3000:
				this.digitalInputsGL3000.DigitalInputConfiguration = data;
				return;
			case LoggerType.GL4000:
				this.digitalInputsGL4000.DigitalInputConfiguration = data;
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
				this.digitalInputsGL1000.DisplayMode = mode;
				break;
			case LoggerType.GL2000:
				this.digitalInputsGL2000.DisplayMode = mode;
				break;
			case LoggerType.GL3000:
				this.digitalInputsGL3000.DisplayMode = mode;
				break;
			case LoggerType.GL4000:
				this.digitalInputsGL4000.DisplayMode = mode;
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
			this.digitalInputsGL1000.Visible = false;
			this.digitalInputsGL2000.Visible = false;
			this.digitalInputsGL3000.Visible = false;
			this.digitalInputsGL4000.Visible = false;
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				this.digitalInputsGL1000.Init();
				this.digitalInputsGL1000.Visible = true;
				return;
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL2000:
				this.digitalInputsGL2000.Init();
				this.digitalInputsGL2000.Visible = true;
				return;
			case LoggerType.GL3000:
				this.digitalInputsGL3000.Init();
				this.digitalInputsGL3000.Visible = true;
				return;
			case LoggerType.GL4000:
				this.digitalInputsGL4000.Init();
				this.digitalInputsGL4000.Visible = true;
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
			this.digitalInputsGL1000 = new DigitalInputsGL1000();
			this.digitalInputsGL3000 = new DigitalInputsGL3Plus();
			this.digitalInputsGL4000 = new DigitalInputsGL3Plus();
			this.digitalInputsGL2000 = new DigitalInputsGL2000();
			base.SuspendLayout();
			this.digitalInputsGL1000.DigitalInputConfiguration = null;
			this.digitalInputsGL1000.DisplayMode = null;
			this.digitalInputsGL1000.Dock = DockStyle.Fill;
			this.digitalInputsGL1000.Location = new Point(0, 0);
			this.digitalInputsGL1000.Name = "digitalInputsGL1000";
			this.digitalInputsGL1000.Size = new Size(546, 464);
			this.digitalInputsGL1000.TabIndex = 0;
			this.digitalInputsGL3000.DigitalInputConfiguration = null;
			this.digitalInputsGL3000.DisplayMode = null;
			this.digitalInputsGL3000.Dock = DockStyle.Fill;
			this.digitalInputsGL3000.Location = new Point(0, 0);
			this.digitalInputsGL3000.Name = "digitalInputsGL3000";
			this.digitalInputsGL3000.Size = new Size(546, 464);
			this.digitalInputsGL3000.TabIndex = 1;
			this.digitalInputsGL4000.DigitalInputConfiguration = null;
			this.digitalInputsGL4000.DisplayMode = null;
			this.digitalInputsGL4000.Dock = DockStyle.Fill;
			this.digitalInputsGL4000.Location = new Point(0, 0);
			this.digitalInputsGL4000.Name = "digitalInputsGL4000";
			this.digitalInputsGL4000.Size = new Size(546, 464);
			this.digitalInputsGL4000.TabIndex = 2;
			this.digitalInputsGL2000.DigitalInputConfiguration = null;
			this.digitalInputsGL2000.DisplayMode = null;
			this.digitalInputsGL2000.Dock = DockStyle.Fill;
			this.digitalInputsGL2000.Location = new Point(0, 0);
			this.digitalInputsGL2000.Name = "digitalInputsGL2000";
			this.digitalInputsGL2000.Size = new Size(546, 464);
			this.digitalInputsGL2000.TabIndex = 3;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.digitalInputsGL2000);
			base.Controls.Add(this.digitalInputsGL4000);
			base.Controls.Add(this.digitalInputsGL3000);
			base.Controls.Add(this.digitalInputsGL1000);
			base.Name = "DigitalInputs";
			base.Size = new Size(546, 464);
			base.ResumeLayout(false);
		}
	}
}
