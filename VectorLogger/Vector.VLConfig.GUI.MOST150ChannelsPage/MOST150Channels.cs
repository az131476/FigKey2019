using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.MOST150ChannelsPage
{
	public class MOST150Channels : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<MOST150ChannelConfiguration>, IUpdateObserver<LoggerType>, IUpdateObserver
	{
		public delegate void DataChangedHandler(MOST150ChannelConfiguration data);

		private LoggerType loggerType;

		private IContainer components;

		private MOST150ChannelsGL3Plus most150ChannelsGL3Plus;

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.most150ChannelsGL3Plus.ModelValidator;
			}
			set
			{
				this.most150ChannelsGL3Plus.ModelValidator = value;
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
				return PageType.MOST150Channels;
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

		public MOST150Channels()
		{
			this.InitializeComponent();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is MOST150ChannelConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.MOST150Channels);
			}
		}

		void IPropertyWindow.Reset()
		{
			this.most150ChannelsGL3Plus.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.GL2000:
			case LoggerType.VN1630log:
				return true;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.most150ChannelsGL3Plus.ValidateInput();
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
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.most150ChannelsGL3Plus.HasErrors();
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
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.most150ChannelsGL3Plus.HasGlobalErrors();
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
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.most150ChannelsGL3Plus.HasLocalErrors();
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
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				return this.most150ChannelsGL3Plus.HasFormatErrors();
			default:
				return true;
			}
		}

		void IUpdateObserver<MOST150ChannelConfiguration>.Update(MOST150ChannelConfiguration data)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.GL2000:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.most150ChannelsGL3Plus.MOST150ChannelConfiguration = data;
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
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL1020FTE:
			case LoggerType.GL2000:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.most150ChannelsGL3Plus.Init();
				this.most150ChannelsGL3Plus.Visible = true;
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
			this.most150ChannelsGL3Plus = new MOST150ChannelsGL3Plus();
			base.SuspendLayout();
			this.most150ChannelsGL3Plus.Dock = DockStyle.Fill;
			this.most150ChannelsGL3Plus.Location = new Point(0, 0);
			this.most150ChannelsGL3Plus.Name = "mostChannelsGL3Plus";
			this.most150ChannelsGL3Plus.Size = new Size(526, 322);
			this.most150ChannelsGL3Plus.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.most150ChannelsGL3Plus);
			base.Name = "MOSTChannels";
			base.Size = new Size(526, 322);
			base.ResumeLayout(false);
		}
	}
}
