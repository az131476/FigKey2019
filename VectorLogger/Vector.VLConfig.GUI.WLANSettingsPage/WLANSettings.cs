using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.WLANSettingsPage
{
	public class WLANSettings : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<WLANConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver<LoggerType>, IUpdateObserver
	{
		public delegate void DataChangedHandler(WLANConfiguration data);

		private LoggerType loggerType;

		private IModelValidator modelValidator;

		private IContainer components;

		private WLANSettingsGL3Plus wlanSettingsGL3Plus;

		private WLANSettingsGL2000 wlanSettingsGL2000;

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.modelValidator;
			}
			set
			{
				this.modelValidator = value;
				this.wlanSettingsGL3Plus.ModelValidator = this.modelValidator;
				this.wlanSettingsGL2000.ModelValidator = this.modelValidator;
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
				return PageType.WLANSettings;
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
				if (value)
				{
					switch (this.loggerType)
					{
					case LoggerType.GL2000:
						this.wlanSettingsGL2000.DisplayErrors();
						break;
					case LoggerType.GL3000:
					case LoggerType.GL4000:
						this.wlanSettingsGL3Plus.DisplayErrors();
						return;
					default:
						return;
					}
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

		public WLANSettings()
		{
			this.InitializeComponent();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is WLANConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.WLANSettings);
			}
		}

		void IPropertyWindow.Reset()
		{
			this.wlanSettingsGL3Plus.Reset();
			this.wlanSettingsGL2000.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return true;
			case LoggerType.GL2000:
				return this.wlanSettingsGL2000.ValidateInput();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.wlanSettingsGL3Plus.ValidateInput();
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
				return this.wlanSettingsGL2000.HasErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.wlanSettingsGL3Plus.HasErrors();
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
				return this.wlanSettingsGL2000.HasGlobalErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.wlanSettingsGL3Plus.HasGlobalErrors();
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
				return this.wlanSettingsGL2000.HasLocalErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.wlanSettingsGL3Plus.HasLocalErrors();
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
				return this.wlanSettingsGL2000.HasFormatErrors();
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.wlanSettingsGL3Plus.HasFormatErrors();
			default:
				return false;
			}
		}

		void IUpdateObserver<WLANConfiguration>.Update(WLANConfiguration data)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL2000:
				this.wlanSettingsGL2000.WLANConfiguration = data;
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.wlanSettingsGL3Plus.WLANConfiguration = data;
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
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL2000:
				this.wlanSettingsGL2000.DisplayMode = mode;
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.wlanSettingsGL3Plus.DisplayMode = mode;
				return;
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
			this.wlanSettingsGL3Plus.Visible = false;
			this.wlanSettingsGL2000.Visible = false;
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL2000:
				this.wlanSettingsGL2000.Init();
				this.wlanSettingsGL2000.Visible = true;
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.wlanSettingsGL3Plus.Init();
				this.wlanSettingsGL3Plus.Visible = true;
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

		public bool Serialize<T>(T wlanSettingsPage)
		{
			if (typeof(WLANSettingsGL3PlusPage) == typeof(T))
			{
				return this.wlanSettingsGL3Plus.Serialize(wlanSettingsPage as WLANSettingsGL3PlusPage);
			}
			return !(typeof(WLANSettingsGL2000Page) == typeof(T)) || this.wlanSettingsGL2000.Serialize(wlanSettingsPage as WLANSettingsGL2000Page);
		}

		public bool DeSerialize<T>(T wlanSettingsPage)
		{
			if (typeof(WLANSettingsGL3PlusPage) == typeof(T))
			{
				return this.wlanSettingsGL3Plus.DeSerialize(wlanSettingsPage as WLANSettingsGL3PlusPage);
			}
			return !(typeof(WLANSettingsGL2000Page) == typeof(T)) || this.wlanSettingsGL2000.DeSerialize(wlanSettingsPage as WLANSettingsGL2000Page);
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
			this.wlanSettingsGL3Plus = new WLANSettingsGL3Plus();
			this.wlanSettingsGL2000 = new WLANSettingsGL2000();
			base.SuspendLayout();
			this.wlanSettingsGL3Plus.DisplayMode = null;
			this.wlanSettingsGL3Plus.Dock = DockStyle.Fill;
			this.wlanSettingsGL3Plus.Font = new Font("Microsoft Sans Serif", 8.25f);
			this.wlanSettingsGL3Plus.Location = new Point(0, 0);
			this.wlanSettingsGL3Plus.ModelValidator = null;
			this.wlanSettingsGL3Plus.Name = "wlanSettingsGL3Plus";
			this.wlanSettingsGL3Plus.Size = new Size(575, 407);
			this.wlanSettingsGL3Plus.TabIndex = 0;
			this.wlanSettingsGL3Plus.WLANConfiguration = null;
			this.wlanSettingsGL2000.Dock = DockStyle.Fill;
			this.wlanSettingsGL2000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.wlanSettingsGL2000.Location = new Point(0, 0);
			this.wlanSettingsGL2000.Name = "wlanSettingsGL2000";
			this.wlanSettingsGL2000.Size = new Size(575, 407);
			this.wlanSettingsGL2000.TabIndex = 1;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.wlanSettingsGL2000);
			base.Controls.Add(this.wlanSettingsGL3Plus);
			base.Name = "WLANSettings";
			base.Size = new Size(575, 407);
			base.ResumeLayout(false);
		}
	}
}
