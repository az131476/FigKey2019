using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.FlexrayChannelsPage
{
	public class FlexrayChannels : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LoggerType>, IUpdateObserver<FlexrayChannelConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver
	{
		public delegate void DataChangedHandler(FlexrayChannelConfiguration data);

		private LoggerType loggerType;

		private IContainer components;

		private FlexrayChannelsGL1000 flexrayChannelsGL1000;

		private FlexrayChannelsGL3000 flexrayChannelsGL3000;

		private FlexrayChannelsGL4000 flexrayChannelsGL4000;

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.flexrayChannelsGL4000.ModelValidator;
			}
			set
			{
				this.flexrayChannelsGL4000.ModelValidator = value;
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
				return PageType.FlexrayChannels;
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

		public FlexrayChannels()
		{
			this.InitializeComponent();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is FlexrayChannelConfiguration;
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
			this.flexrayChannelsGL4000.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.GL2000:
			case LoggerType.GL3000:
			case LoggerType.VN1630log:
				return true;
			case LoggerType.GL4000:
				return this.flexrayChannelsGL4000.ValidateInput();
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
			case LoggerType.GL2000:
			case LoggerType.GL3000:
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL4000:
				return this.flexrayChannelsGL4000.HasErrors();
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
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL4000:
				return this.flexrayChannelsGL4000.HasGlobalErrors();
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
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL4000:
				return this.flexrayChannelsGL4000.HasLocalErrors();
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
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL4000:
				return this.flexrayChannelsGL4000.HasFormatErrors();
			default:
				return true;
			}
		}

		void IUpdateObserver<FlexrayChannelConfiguration>.Update(FlexrayChannelConfiguration data)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.GL2000:
			case LoggerType.GL3000:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL4000:
				this.flexrayChannelsGL4000.FlexrayChannelConfiguration = data;
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
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL4000:
				this.flexrayChannelsGL4000.DisplayMode = mode;
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
			this.flexrayChannelsGL1000.Visible = false;
			this.flexrayChannelsGL3000.Visible = false;
			this.flexrayChannelsGL4000.Visible = false;
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.GL2000:
				this.flexrayChannelsGL1000.Visible = true;
				return;
			case LoggerType.GL3000:
				this.flexrayChannelsGL3000.Visible = true;
				return;
			case LoggerType.GL4000:
				this.flexrayChannelsGL4000.Init();
				this.flexrayChannelsGL4000.Visible = true;
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
			this.flexrayChannelsGL1000 = new FlexrayChannelsGL1000();
			this.flexrayChannelsGL3000 = new FlexrayChannelsGL3000();
			this.flexrayChannelsGL4000 = new FlexrayChannelsGL4000();
			base.SuspendLayout();
			this.flexrayChannelsGL1000.Dock = DockStyle.Fill;
			this.flexrayChannelsGL1000.Location = new Point(0, 0);
			this.flexrayChannelsGL1000.Name = "flexrayChannelsGL1000";
			this.flexrayChannelsGL1000.Size = new Size(429, 509);
			this.flexrayChannelsGL1000.TabIndex = 0;
			this.flexrayChannelsGL3000.Dock = DockStyle.Fill;
			this.flexrayChannelsGL3000.Location = new Point(0, 0);
			this.flexrayChannelsGL3000.Name = "flexrayChannelsGL3000";
			this.flexrayChannelsGL3000.Size = new Size(429, 509);
			this.flexrayChannelsGL3000.TabIndex = 1;
			this.flexrayChannelsGL4000.Dock = DockStyle.Fill;
			this.flexrayChannelsGL4000.Location = new Point(0, 0);
			this.flexrayChannelsGL4000.Name = "flexrayChannelsGL4000";
			this.flexrayChannelsGL4000.Size = new Size(429, 509);
			this.flexrayChannelsGL4000.TabIndex = 2;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.flexrayChannelsGL4000);
			base.Controls.Add(this.flexrayChannelsGL3000);
			base.Controls.Add(this.flexrayChannelsGL1000);
			base.Name = "FlexrayChannels";
			base.Size = new Size(429, 509);
			base.ResumeLayout(false);
		}
	}
}
