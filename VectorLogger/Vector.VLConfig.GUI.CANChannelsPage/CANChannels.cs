using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.CANChannelsPage
{
	public class CANChannels : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LoggerType>, IUpdateObserver<CANChannelConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver
	{
		public delegate void DataChangedHandler(CANChannelConfiguration data);

		private LoggerType loggerType;

		private IContainer components;

		private CANChannelsGL1000 canChannelsGL1000;

		private CANChannelsGL3000 canChannelsGL3000;

		private CANChannelsGL4000 canChannelsGL4000;

		private CANChannelsGL2000 canChannelsGL2000;

		private CANChannelsGL1020FTE canChannelsGL1020FTE;

		public IConfigurationManagerService ConfigurationManagerService
		{
			get
			{
				return this.canChannelsGL1000.ConfigurationManagerService;
			}
			set
			{
				this.canChannelsGL1000.ConfigurationManagerService = value;
				this.canChannelsGL1020FTE.ConfigurationManagerService = value;
				this.canChannelsGL2000.ConfigurationManagerService = value;
				this.canChannelsGL3000.ConfigurationManagerService = value;
				this.canChannelsGL4000.ConfigurationManagerService = value;
			}
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.canChannelsGL1000.ModelValidator;
			}
			set
			{
				this.canChannelsGL1000.ModelValidator = value;
				this.canChannelsGL1020FTE.ModelValidator = value;
				this.canChannelsGL2000.ModelValidator = value;
				this.canChannelsGL3000.ModelValidator = value;
				this.canChannelsGL4000.ModelValidator = value;
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
				return PageType.CANChannels;
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

		public CANChannels()
		{
			this.InitializeComponent();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is CANChannelConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.CANChannels);
			}
		}

		void IPropertyWindow.Reset()
		{
			this.canChannelsGL1000.Reset();
			this.canChannelsGL1020FTE.Reset();
			this.canChannelsGL2000.Reset();
			this.canChannelsGL3000.Reset();
			this.canChannelsGL4000.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.canChannelsGL1000.ValidateInput();
			case LoggerType.GL1020FTE:
				return this.canChannelsGL1020FTE.ValidateInput();
			case LoggerType.GL2000:
				return this.canChannelsGL2000.ValidateInput();
			case LoggerType.GL3000:
				return this.canChannelsGL3000.ValidateInput();
			case LoggerType.GL4000:
				return this.canChannelsGL4000.ValidateInput();
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
				return this.canChannelsGL1000.HasErrors();
			case LoggerType.GL1020FTE:
				return this.canChannelsGL1020FTE.HasErrors();
			case LoggerType.GL2000:
				return this.canChannelsGL2000.HasErrors();
			case LoggerType.GL3000:
				return this.canChannelsGL3000.HasErrors();
			case LoggerType.GL4000:
				return this.canChannelsGL4000.HasErrors();
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
				return this.canChannelsGL1000.HasGlobalErrors();
			case LoggerType.GL1020FTE:
				return this.canChannelsGL1020FTE.HasGlobalErrors();
			case LoggerType.GL2000:
				return this.canChannelsGL2000.HasGlobalErrors();
			case LoggerType.GL3000:
				return this.canChannelsGL3000.HasGlobalErrors();
			case LoggerType.GL4000:
				return this.canChannelsGL4000.HasGlobalErrors();
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
				return this.canChannelsGL1000.HasLocalErrors();
			case LoggerType.GL1020FTE:
				return this.canChannelsGL1020FTE.HasLocalErrors();
			case LoggerType.GL2000:
				return this.canChannelsGL2000.HasLocalErrors();
			case LoggerType.GL3000:
				return this.canChannelsGL3000.HasLocalErrors();
			case LoggerType.GL4000:
				return this.canChannelsGL4000.HasLocalErrors();
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
				return this.canChannelsGL1000.HasFormatErrors();
			case LoggerType.GL1020FTE:
				return this.canChannelsGL1020FTE.HasFormatErrors();
			case LoggerType.GL2000:
				return this.canChannelsGL2000.HasFormatErrors();
			case LoggerType.GL3000:
				return this.canChannelsGL3000.HasFormatErrors();
			case LoggerType.GL4000:
				return this.canChannelsGL4000.HasFormatErrors();
			case LoggerType.VN1630log:
				return false;
			default:
				return true;
			}
		}

		void IUpdateObserver<CANChannelConfiguration>.Update(CANChannelConfiguration data)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				this.canChannelsGL1000.CANChannelConfiguration = data;
				return;
			case LoggerType.GL1020FTE:
				this.canChannelsGL1020FTE.CANChannelConfiguration = data;
				return;
			case LoggerType.GL2000:
				this.canChannelsGL2000.CANChannelConfiguration = data;
				return;
			case LoggerType.GL3000:
				this.canChannelsGL3000.CANChannelConfiguration = data;
				return;
			case LoggerType.GL4000:
				this.canChannelsGL4000.CANChannelConfiguration = data;
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
				this.canChannelsGL1000.DisplayMode = mode;
				return;
			case LoggerType.GL1020FTE:
				this.canChannelsGL1020FTE.DisplayMode = mode;
				return;
			case LoggerType.GL2000:
				this.canChannelsGL2000.DisplayMode = mode;
				return;
			case LoggerType.GL3000:
				this.canChannelsGL3000.DisplayMode = mode;
				return;
			case LoggerType.GL4000:
				this.canChannelsGL4000.DisplayMode = mode;
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
			CANChipConfigurationManager.Reset(((IPropertyWindow)this).ModelValidator.LoggerSpecifics);
			this.canChannelsGL1000.Visible = false;
			this.canChannelsGL1020FTE.Visible = false;
			this.canChannelsGL2000.Visible = false;
			this.canChannelsGL3000.Visible = false;
			this.canChannelsGL4000.Visible = false;
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				this.canChannelsGL1000.Init();
				this.canChannelsGL1000.Visible = true;
				return;
			case LoggerType.GL1020FTE:
				this.canChannelsGL1020FTE.Init();
				this.canChannelsGL1020FTE.Visible = true;
				return;
			case LoggerType.GL2000:
				this.canChannelsGL2000.Init();
				this.canChannelsGL2000.Visible = true;
				return;
			case LoggerType.GL3000:
				this.canChannelsGL3000.Init();
				this.canChannelsGL3000.Visible = true;
				return;
			case LoggerType.GL4000:
				this.canChannelsGL4000.Init();
				this.canChannelsGL4000.Visible = true;
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
			this.canChannelsGL4000 = new CANChannelsGL4000();
			this.canChannelsGL3000 = new CANChannelsGL3000();
			this.canChannelsGL1000 = new CANChannelsGL1000();
			this.canChannelsGL2000 = new CANChannelsGL2000();
			this.canChannelsGL1020FTE = new CANChannelsGL1020FTE();
			base.SuspendLayout();
			this.canChannelsGL4000.CANChannelConfiguration = null;
			this.canChannelsGL4000.DisplayMode = null;
			this.canChannelsGL4000.Dock = DockStyle.Fill;
			this.canChannelsGL4000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.canChannelsGL4000.Location = new Point(0, 0);
			this.canChannelsGL4000.Name = "canChannelsGL4000";
			this.canChannelsGL4000.Size = new Size(535, 478);
			this.canChannelsGL4000.TabIndex = 2;
			this.canChannelsGL3000.CANChannelConfiguration = null;
			this.canChannelsGL3000.DisplayMode = null;
			this.canChannelsGL3000.Dock = DockStyle.Fill;
			this.canChannelsGL3000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.canChannelsGL3000.Location = new Point(0, 0);
			this.canChannelsGL3000.Name = "canChannelsGL3000";
			this.canChannelsGL3000.Size = new Size(535, 478);
			this.canChannelsGL3000.TabIndex = 1;
			this.canChannelsGL1000.CANChannelConfiguration = null;
			this.canChannelsGL1000.DisplayMode = null;
			this.canChannelsGL1000.Dock = DockStyle.Fill;
			this.canChannelsGL1000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.canChannelsGL1000.Location = new Point(0, 0);
			this.canChannelsGL1000.Name = "canChannelsGL1000";
			this.canChannelsGL1000.Size = new Size(535, 478);
			this.canChannelsGL1000.TabIndex = 0;
			this.canChannelsGL2000.CANChannelConfiguration = null;
			this.canChannelsGL2000.DisplayMode = null;
			this.canChannelsGL2000.Dock = DockStyle.Fill;
			this.canChannelsGL2000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.canChannelsGL2000.Location = new Point(0, 0);
			this.canChannelsGL2000.Name = "canChannelsGL2000";
			this.canChannelsGL2000.Size = new Size(535, 478);
			this.canChannelsGL2000.TabIndex = 3;
			this.canChannelsGL1020FTE.CANChannelConfiguration = null;
			this.canChannelsGL1020FTE.DisplayMode = null;
			this.canChannelsGL1020FTE.Dock = DockStyle.Fill;
			this.canChannelsGL1020FTE.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.canChannelsGL1020FTE.Location = new Point(0, 0);
			this.canChannelsGL1020FTE.Name = "canChannelsGL1020FTE";
			this.canChannelsGL1020FTE.Size = new Size(535, 478);
			this.canChannelsGL1020FTE.TabIndex = 4;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.canChannelsGL1020FTE);
			base.Controls.Add(this.canChannelsGL2000);
			base.Controls.Add(this.canChannelsGL4000);
			base.Controls.Add(this.canChannelsGL3000);
			base.Controls.Add(this.canChannelsGL1000);
			base.Name = "CANChannels";
			base.Size = new Size(535, 478);
			base.ResumeLayout(false);
		}
	}
}
