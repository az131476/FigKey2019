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

namespace Vector.VLConfig.GUI.LINChannelsPage
{
	public class LINChannels : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LoggerType>, IUpdateObserver<LINChannelConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver<DatabaseConfiguration>, IUpdateObserver
	{
		public delegate void DataChangedHandler(LINChannelConfiguration data);

		private LoggerType loggerType;

		private IContainer components;

		private LINChannelsGL1000 linChannelsGL1000;

		private LINChannelsGL3000 linChannelsGL3000;

		private LINChannelsGL4000 linChannelsGL4000;

		private LINChannelsGL2000 linChannelsGL2000;

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.linChannelsGL1000.ApplicationDatabaseManager;
			}
			set
			{
				this.linChannelsGL1000.ApplicationDatabaseManager = value;
				this.linChannelsGL2000.ApplicationDatabaseManager = value;
				this.linChannelsGL3000.ApplicationDatabaseManager = value;
				this.linChannelsGL4000.ApplicationDatabaseManager = value;
			}
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.linChannelsGL1000.ModelValidator;
			}
			set
			{
				this.linChannelsGL1000.ModelValidator = value;
				this.linChannelsGL2000.ModelValidator = value;
				this.linChannelsGL3000.ModelValidator = value;
				this.linChannelsGL4000.ModelValidator = value;
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
				return PageType.LINChannels;
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

		public LINChannels()
		{
			this.InitializeComponent();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is LINChannelConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.LINChannels);
			}
		}

		void IPropertyWindow.Reset()
		{
			this.linChannelsGL1000.Reset();
			this.linChannelsGL2000.Reset();
			this.linChannelsGL3000.Reset();
			this.linChannelsGL4000.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.linChannelsGL1000.ValidateInput();
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return true;
			case LoggerType.GL2000:
				return this.linChannelsGL2000.ValidateInput();
			case LoggerType.GL3000:
				return this.linChannelsGL3000.ValidateInput();
			case LoggerType.GL4000:
				return this.linChannelsGL4000.ValidateInput();
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.linChannelsGL1000.HasErrors();
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL2000:
				return this.linChannelsGL2000.HasErrors();
			case LoggerType.GL3000:
				return this.linChannelsGL3000.HasErrors();
			case LoggerType.GL4000:
				return this.linChannelsGL4000.HasErrors();
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.linChannelsGL1000.HasGlobalErrors();
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL2000:
				return this.linChannelsGL2000.HasGlobalErrors();
			case LoggerType.GL3000:
				return this.linChannelsGL3000.HasGlobalErrors();
			case LoggerType.GL4000:
				return this.linChannelsGL4000.HasGlobalErrors();
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.linChannelsGL1000.HasLocalErrors();
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL2000:
				return this.linChannelsGL2000.HasLocalErrors();
			case LoggerType.GL3000:
				return this.linChannelsGL3000.HasLocalErrors();
			case LoggerType.GL4000:
				return this.linChannelsGL4000.HasLocalErrors();
			default:
				return true;
			}
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				return this.linChannelsGL1000.HasFormatErrors();
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				return false;
			case LoggerType.GL2000:
				return this.linChannelsGL2000.HasFormatErrors();
			case LoggerType.GL3000:
				return this.linChannelsGL3000.HasFormatErrors();
			case LoggerType.GL4000:
				return this.linChannelsGL4000.HasFormatErrors();
			default:
				return true;
			}
		}

		void IUpdateObserver<LINChannelConfiguration>.Update(LINChannelConfiguration data)
		{
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				this.linChannelsGL1000.LINChannelConfiguration = data;
				return;
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL2000:
				this.linChannelsGL2000.LINChannelConfiguration = data;
				return;
			case LoggerType.GL3000:
				this.linChannelsGL3000.LINChannelConfiguration = data;
				return;
			case LoggerType.GL4000:
				this.linChannelsGL4000.LINChannelConfiguration = data;
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
				this.linChannelsGL1000.DisplayMode = mode;
				return;
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL2000:
				this.linChannelsGL2000.DisplayMode = mode;
				return;
			case LoggerType.GL3000:
				this.linChannelsGL3000.DisplayMode = mode;
				return;
			case LoggerType.GL4000:
				this.linChannelsGL4000.DisplayMode = mode;
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
			this.linChannelsGL1000.Visible = false;
			this.linChannelsGL2000.Visible = false;
			this.linChannelsGL3000.Visible = false;
			this.linChannelsGL4000.Visible = false;
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				this.linChannelsGL1000.Init();
				this.linChannelsGL1000.Visible = true;
				return;
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL2000:
				this.linChannelsGL2000.Init();
				this.linChannelsGL2000.Visible = true;
				return;
			case LoggerType.GL3000:
				this.linChannelsGL3000.Init();
				this.linChannelsGL3000.Visible = true;
				return;
			case LoggerType.GL4000:
				this.linChannelsGL4000.Init();
				this.linChannelsGL4000.Visible = true;
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
			this.linChannelsGL1000 = new LINChannelsGL1000();
			this.linChannelsGL3000 = new LINChannelsGL3000();
			this.linChannelsGL4000 = new LINChannelsGL4000();
			this.linChannelsGL2000 = new LINChannelsGL2000();
			base.SuspendLayout();
			this.linChannelsGL1000.DisplayMode = null;
			this.linChannelsGL1000.Dock = DockStyle.Fill;
			this.linChannelsGL1000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.linChannelsGL1000.LINChannelConfiguration = null;
			this.linChannelsGL1000.Location = new Point(0, 0);
			this.linChannelsGL1000.Name = "linChannelsGL1000";
			this.linChannelsGL1000.Size = new Size(352, 478);
			this.linChannelsGL1000.TabIndex = 0;
			this.linChannelsGL3000.DisplayMode = null;
			this.linChannelsGL3000.Dock = DockStyle.Fill;
			this.linChannelsGL3000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.linChannelsGL3000.LINChannelConfiguration = null;
			this.linChannelsGL3000.Location = new Point(0, 0);
			this.linChannelsGL3000.Name = "linChannelsGL3000";
			this.linChannelsGL3000.Size = new Size(352, 478);
			this.linChannelsGL3000.TabIndex = 1;
			this.linChannelsGL4000.DisplayMode = null;
			this.linChannelsGL4000.Dock = DockStyle.Fill;
			this.linChannelsGL4000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.linChannelsGL4000.LINChannelConfiguration = null;
			this.linChannelsGL4000.Location = new Point(0, 0);
			this.linChannelsGL4000.Name = "linChannelsGL4000";
			this.linChannelsGL4000.Size = new Size(352, 478);
			this.linChannelsGL4000.TabIndex = 2;
			this.linChannelsGL2000.DisplayMode = null;
			this.linChannelsGL2000.Dock = DockStyle.Fill;
			this.linChannelsGL2000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.linChannelsGL2000.LINChannelConfiguration = null;
			this.linChannelsGL2000.Location = new Point(0, 0);
			this.linChannelsGL2000.Name = "linChannelsGL2000";
			this.linChannelsGL2000.Size = new Size(352, 478);
			this.linChannelsGL2000.TabIndex = 3;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.linChannelsGL2000);
			base.Controls.Add(this.linChannelsGL4000);
			base.Controls.Add(this.linChannelsGL3000);
			base.Controls.Add(this.linChannelsGL1000);
			base.Name = "LINChannels";
			base.Size = new Size(352, 478);
			base.ResumeLayout(false);
		}
	}
}
