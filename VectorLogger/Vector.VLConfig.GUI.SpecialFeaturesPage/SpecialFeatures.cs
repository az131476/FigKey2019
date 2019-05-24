using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.SpecialFeaturesPage
{
	public class SpecialFeatures : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<SpecialFeaturesConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver<LoggerType>, IUpdateObserver
	{
		public delegate void DataChangedHandler(SpecialFeaturesConfiguration data);

		private LoggerType loggerType;

		private DisplayMode displayMode;

		private DisplayMode lastNotifiedDisplayMode;

		private IContainer components;

		private SpecialFeaturesGL3Plus specialFeaturesGL3Plus;

		private SpecialFeaturesGL1000 specialFeaturesGL1000;

		private SpecialFeaturesGL2000 specialFeaturesGL2000;

		private SpecialFeaturesGL1020FTE specialFeaturesGL1020FTE;

		private SpecialFeaturesVN1630log specialFeaturesVN1630log;

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.specialFeaturesGL3Plus.ModelValidator;
			}
			set
			{
				this.specialFeaturesGL3Plus.ModelValidator = value;
				this.specialFeaturesGL1000.ModelValidator = value;
				this.specialFeaturesGL1020FTE.ModelValidator = value;
				this.specialFeaturesGL2000.ModelValidator = value;
				this.specialFeaturesVN1630log.ModelValidator = value;
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
				return PageType.SpecialFeatures;
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

		public SpecialFeatures()
		{
			this.InitializeComponent();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is SpecialFeaturesConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.SpecialFeatures);
			}
		}

		void IPropertyWindow.Reset()
		{
			this.specialFeaturesGL3Plus.Reset();
			this.specialFeaturesGL1000.Reset();
			this.specialFeaturesGL1020FTE.Reset();
			this.specialFeaturesGL2000.Reset();
			this.specialFeaturesVN1630log.Reset();
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
				return this.specialFeaturesGL1000.ValidateInput();
			case LoggerType.GL1020FTE:
				return this.specialFeaturesGL1020FTE.ValidateInput();
			case LoggerType.GL2000:
				return this.specialFeaturesGL2000.ValidateInput();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.specialFeaturesGL3Plus.ValidateInput();
			case LoggerType.VN1630log:
				return this.specialFeaturesVN1630log.ValidateInput();
			default:
				return false;
			}
		}

		bool IPropertyWindow.HasErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.specialFeaturesGL1000.HasErrors();
			case LoggerType.GL1020FTE:
				return this.specialFeaturesGL1020FTE.HasErrors();
			case LoggerType.GL2000:
				return this.specialFeaturesGL2000.HasErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.specialFeaturesGL3Plus.HasErrors();
			case LoggerType.VN1630log:
				return this.specialFeaturesVN1630log.HasErrors();
			default:
				return false;
			}
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.specialFeaturesGL1000.HasGlobalErrors();
			case LoggerType.GL1020FTE:
				return this.specialFeaturesGL1020FTE.HasGlobalErrors();
			case LoggerType.GL2000:
				return this.specialFeaturesGL2000.HasGlobalErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.specialFeaturesGL3Plus.HasGlobalErrors();
			case LoggerType.VN1630log:
				return this.specialFeaturesVN1630log.HasGlobalErrors();
			default:
				return false;
			}
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.specialFeaturesGL1000.HasLocalErrors();
			case LoggerType.GL1020FTE:
				return this.specialFeaturesGL1020FTE.HasLocalErrors();
			case LoggerType.GL2000:
				return this.specialFeaturesGL2000.HasLocalErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.specialFeaturesGL3Plus.HasLocalErrors();
			case LoggerType.VN1630log:
				return this.specialFeaturesVN1630log.HasLocalErrors();
			default:
				return false;
			}
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.specialFeaturesGL1000.HasFormatErrors();
			case LoggerType.GL1020FTE:
				return this.specialFeaturesGL1020FTE.HasFormatErrors();
			case LoggerType.GL2000:
				return this.specialFeaturesGL2000.HasFormatErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.specialFeaturesGL3Plus.HasFormatErrors();
			case LoggerType.VN1630log:
				return this.specialFeaturesVN1630log.HasFormatErrors();
			default:
				return false;
			}
		}

		void IUpdateObserver<SpecialFeaturesConfiguration>.Update(SpecialFeaturesConfiguration data)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				this.specialFeaturesGL1000.SpecialFeaturesConfiguration = data;
				return;
			case LoggerType.GL1020FTE:
				this.specialFeaturesGL1020FTE.SpecialFeaturesConfiguration = data;
				return;
			case LoggerType.GL2000:
				this.specialFeaturesGL2000.SpecialFeaturesConfiguration = data;
				return;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.specialFeaturesGL3Plus.SpecialFeaturesConfiguration = data;
				return;
			case LoggerType.VN1630log:
				this.specialFeaturesVN1630log.SpecialFeaturesConfiguration = data;
				return;
			default:
				return;
			}
		}

		void IUpdateObserver<DisplayMode>.Update(DisplayMode mode)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				this.specialFeaturesGL1000.DisplayMode = mode;
				break;
			case LoggerType.GL1020FTE:
				this.specialFeaturesGL1020FTE.DisplayMode = mode;
				break;
			case LoggerType.GL2000:
				this.specialFeaturesGL2000.DisplayMode = mode;
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.specialFeaturesGL3Plus.DisplayMode = mode;
				break;
			case LoggerType.VN1630log:
				this.specialFeaturesVN1630log.DisplayMode = mode;
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
			this.specialFeaturesGL3Plus.Visible = false;
			this.specialFeaturesGL1000.Visible = false;
			this.specialFeaturesGL1020FTE.Visible = false;
			this.specialFeaturesGL2000.Visible = false;
			this.specialFeaturesVN1630log.Visible = false;
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				this.specialFeaturesGL1000.Init();
				this.specialFeaturesGL1000.Visible = true;
				return;
			case LoggerType.GL1020FTE:
				this.specialFeaturesGL1020FTE.Init();
				this.specialFeaturesGL1020FTE.Visible = true;
				return;
			case LoggerType.GL2000:
				this.specialFeaturesGL2000.Init();
				this.specialFeaturesGL2000.Visible = true;
				return;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.specialFeaturesGL3Plus.Init();
				this.specialFeaturesGL3Plus.Visible = true;
				return;
			case LoggerType.VN1630log:
				this.specialFeaturesVN1630log.Init();
				this.specialFeaturesVN1630log.Visible = true;
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
			this.specialFeaturesGL1020FTE = new SpecialFeaturesGL1020FTE();
			this.specialFeaturesGL2000 = new SpecialFeaturesGL2000();
			this.specialFeaturesGL1000 = new SpecialFeaturesGL1000();
			this.specialFeaturesGL3Plus = new SpecialFeaturesGL3Plus();
			this.specialFeaturesVN1630log = new SpecialFeaturesVN1630log();
			base.SuspendLayout();
			this.specialFeaturesGL1020FTE.DisplayMode = null;
			this.specialFeaturesGL1020FTE.Dock = DockStyle.Fill;
			this.specialFeaturesGL1020FTE.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.specialFeaturesGL1020FTE.Location = new Point(0, 0);
			this.specialFeaturesGL1020FTE.ModelValidator = null;
			this.specialFeaturesGL1020FTE.Name = "specialFeaturesGL1020FTE";
			this.specialFeaturesGL1020FTE.Size = new Size(585, 192);
			this.specialFeaturesGL1020FTE.SpecialFeaturesConfiguration = null;
			this.specialFeaturesGL1020FTE.TabIndex = 3;
			this.specialFeaturesGL2000.DisplayMode = null;
			this.specialFeaturesGL2000.Dock = DockStyle.Fill;
			this.specialFeaturesGL2000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.specialFeaturesGL2000.Location = new Point(0, 0);
			this.specialFeaturesGL2000.ModelValidator = null;
			this.specialFeaturesGL2000.Name = "specialFeaturesGL2000";
			this.specialFeaturesGL2000.Size = new Size(585, 192);
			this.specialFeaturesGL2000.SpecialFeaturesConfiguration = null;
			this.specialFeaturesGL2000.TabIndex = 2;
			this.specialFeaturesGL1000.DisplayMode = null;
			this.specialFeaturesGL1000.Dock = DockStyle.Fill;
			this.specialFeaturesGL1000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.specialFeaturesGL1000.Location = new Point(0, 0);
			this.specialFeaturesGL1000.ModelValidator = null;
			this.specialFeaturesGL1000.Name = "specialFeaturesGL1000";
			this.specialFeaturesGL1000.Size = new Size(585, 192);
			this.specialFeaturesGL1000.SpecialFeaturesConfiguration = null;
			this.specialFeaturesGL1000.TabIndex = 1;
			this.specialFeaturesGL3Plus.DisplayMode = null;
			this.specialFeaturesGL3Plus.Dock = DockStyle.Fill;
			this.specialFeaturesGL3Plus.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.specialFeaturesGL3Plus.Location = new Point(0, 0);
			this.specialFeaturesGL3Plus.ModelValidator = null;
			this.specialFeaturesGL3Plus.Name = "specialFeaturesGL3Plus";
			this.specialFeaturesGL3Plus.Size = new Size(585, 192);
			this.specialFeaturesGL3Plus.SpecialFeaturesConfiguration = null;
			this.specialFeaturesGL3Plus.TabIndex = 0;
			this.specialFeaturesVN1630log.Dock = DockStyle.Fill;
			this.specialFeaturesVN1630log.Location = new Point(0, 0);
			this.specialFeaturesVN1630log.Name = "specialFeaturesVN1630log";
			this.specialFeaturesVN1630log.Size = new Size(585, 192);
			this.specialFeaturesVN1630log.TabIndex = 4;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.specialFeaturesVN1630log);
			base.Controls.Add(this.specialFeaturesGL1020FTE);
			base.Controls.Add(this.specialFeaturesGL2000);
			base.Controls.Add(this.specialFeaturesGL1000);
			base.Controls.Add(this.specialFeaturesGL3Plus);
			base.Name = "SpecialFeatures";
			base.Size = new Size(585, 192);
			base.ResumeLayout(false);
		}
	}
}
