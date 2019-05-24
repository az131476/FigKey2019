using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.MultibusChannelsPage
{
	public class MultibusChannels : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LoggerType>, IUpdateObserver<DisplayMode>, IUpdateObserver<MultibusChannelConfiguration>, IUpdateObserver<DatabaseConfiguration>, IUpdateObserver
	{
		public delegate void DataChangedHandler(MultibusChannelConfiguration data);

		private LoggerType loggerType;

		private IContainer components;

		private MultibusChannelsVN16XXlog multibusChannelsVN16XXlog;

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.multibusChannelsVN16XXlog.ApplicationDatabaseManager;
			}
			set
			{
				this.multibusChannelsVN16XXlog.ApplicationDatabaseManager = value;
			}
		}

		public IHardwareFrontend HardwareFrontend
		{
			get
			{
				return this.multibusChannelsVN16XXlog.HardwareFrontend;
			}
			set
			{
				this.multibusChannelsVN16XXlog.HardwareFrontend = value;
			}
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.multibusChannelsVN16XXlog.ModelValidator;
			}
			set
			{
				this.multibusChannelsVN16XXlog.ModelValidator = value;
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
				return PageType.MultibusChannels;
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

		public MultibusChannels()
		{
			this.InitializeComponent();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is MultibusChannelConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.MultibusChannels);
			}
		}

		void IPropertyWindow.Reset()
		{
			this.multibusChannelsVN16XXlog.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.GL2000:
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return true;
			case LoggerType.VN1630log:
				return this.multibusChannelsVN16XXlog.ValidateInput();
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
			case LoggerType.GL2000:
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return false;
			case LoggerType.VN1630log:
				return this.multibusChannelsVN16XXlog.HasErrors();
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
			case LoggerType.GL2000:
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return false;
			case LoggerType.VN1630log:
				return this.multibusChannelsVN16XXlog.HasGlobalErrors();
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
			case LoggerType.GL2000:
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return false;
			case LoggerType.VN1630log:
				return this.multibusChannelsVN16XXlog.HasLocalErrors();
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
			case LoggerType.GL2000:
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return false;
			case LoggerType.VN1630log:
				return this.multibusChannelsVN16XXlog.HasFormatErrors();
			default:
				return true;
			}
		}

		void IUpdateObserver<MultibusChannelConfiguration>.Update(MultibusChannelConfiguration data)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.GL2000:
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				break;
			case LoggerType.VN1630log:
				this.multibusChannelsVN16XXlog.MultibusChannelConfiguration = data;
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
			case LoggerType.GL2000:
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				break;
			case LoggerType.VN1630log:
				this.multibusChannelsVN16XXlog.DisplayMode = mode;
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
			CANChipConfigurationManager.Reset(((IPropertyWindow)this).ModelValidator.LoggerSpecifics);
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.GL2000:
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				break;
			case LoggerType.VN1630log:
				this.multibusChannelsVN16XXlog.Init();
				this.multibusChannelsVN16XXlog.Visible = true;
				break;
			default:
				return;
			}
		}

		void IUpdateObserver<DatabaseConfiguration>.Update(DatabaseConfiguration data)
		{
			((IPropertyWindow)this).ValidateInput();
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
			this.multibusChannelsVN16XXlog = new MultibusChannelsVN16XXlog();
			base.SuspendLayout();
			this.multibusChannelsVN16XXlog.ApplicationDatabaseManager = null;
			this.multibusChannelsVN16XXlog.DisplayMode = null;
			this.multibusChannelsVN16XXlog.Dock = DockStyle.Fill;
			this.multibusChannelsVN16XXlog.Font = new Font("Microsoft Sans Serif", 8.25f);
			this.multibusChannelsVN16XXlog.Location = new Point(0, 0);
			this.multibusChannelsVN16XXlog.ModelValidator = null;
			this.multibusChannelsVN16XXlog.MultibusChannelConfiguration = null;
			this.multibusChannelsVN16XXlog.Name = "multibusChannelsVN16XXlog";
			this.multibusChannelsVN16XXlog.Size = new Size(537, 548);
			this.multibusChannelsVN16XXlog.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.multibusChannelsVN16XXlog);
			base.Name = "MultibusChannels";
			base.Size = new Size(537, 548);
			base.ResumeLayout(false);
		}
	}
}
