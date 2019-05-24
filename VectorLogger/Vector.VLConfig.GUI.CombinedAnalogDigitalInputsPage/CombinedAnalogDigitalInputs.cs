using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.CombinedAnalogDigitalInputsPage
{
	public class CombinedAnalogDigitalInputs : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LoggerType>, IUpdateObserver<DisplayMode>, IUpdateObserver<AnalogInputConfiguration>, IUpdateObserver<DigitalInputConfiguration>, IUpdateObserver
	{
		public delegate void AnalogInputDataChangedHandler(AnalogInputConfiguration data);

		public delegate void DigitalInputDataChangedHandler(DigitalInputConfiguration data);

		private LoggerType loggerType;

		private IContainer components;

		private CombinedAnalogDigitalInputsVN16XXlog combinedAnalogDigitalInputsVN16XXlog;

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.combinedAnalogDigitalInputsVN16XXlog.ModelValidator;
			}
			set
			{
				this.combinedAnalogDigitalInputsVN16XXlog.ModelValidator = value;
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
				return PageType.CombinedAnalogDigitalInputs;
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

		public CombinedAnalogDigitalInputs()
		{
			this.InitializeComponent();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return this.loggerType == LoggerType.VN1630log && (feature is AnalogInputConfiguration || feature is DigitalInputConfiguration);
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.CombinedAnalogDigitalInputs);
			}
		}

		void IPropertyWindow.Reset()
		{
			this.combinedAnalogDigitalInputsVN16XXlog.Reset();
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
				return this.combinedAnalogDigitalInputsVN16XXlog.ValidateInput();
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
				return this.combinedAnalogDigitalInputsVN16XXlog.HasErrors();
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
				return this.combinedAnalogDigitalInputsVN16XXlog.HasGlobalErrors();
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
				return this.combinedAnalogDigitalInputsVN16XXlog.HasLocalErrors();
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
				return this.combinedAnalogDigitalInputsVN16XXlog.HasFormatErrors();
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
			case LoggerType.GL2000:
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				break;
			case LoggerType.VN1630log:
				this.combinedAnalogDigitalInputsVN16XXlog.AnalogInputConfiguration = data;
				break;
			default:
				return;
			}
		}

		void IUpdateObserver<DigitalInputConfiguration>.Update(DigitalInputConfiguration data)
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
				this.combinedAnalogDigitalInputsVN16XXlog.DigitalInputConfiguration = data;
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
				this.combinedAnalogDigitalInputsVN16XXlog.DisplayMode = mode;
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
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				break;
			case LoggerType.VN1630log:
				this.combinedAnalogDigitalInputsVN16XXlog.Init();
				this.combinedAnalogDigitalInputsVN16XXlog.Visible = true;
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
			this.combinedAnalogDigitalInputsVN16XXlog = new CombinedAnalogDigitalInputsVN16XXlog();
			base.SuspendLayout();
			this.combinedAnalogDigitalInputsVN16XXlog.Dock = DockStyle.Fill;
			this.combinedAnalogDigitalInputsVN16XXlog.Location = new Point(0, 0);
			this.combinedAnalogDigitalInputsVN16XXlog.Name = "combinedAnalogDigitalInputsVN16XXlog";
			this.combinedAnalogDigitalInputsVN16XXlog.Size = new Size(402, 370);
			this.combinedAnalogDigitalInputsVN16XXlog.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.combinedAnalogDigitalInputsVN16XXlog);
			base.Name = "CombinedAnalogDigitalInputs";
			base.Size = new Size(402, 370);
			base.ResumeLayout(false);
		}
	}
}
