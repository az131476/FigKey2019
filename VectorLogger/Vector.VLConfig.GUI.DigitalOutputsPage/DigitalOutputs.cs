using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;

namespace Vector.VLConfig.GUI.DigitalOutputsPage
{
	public class DigitalOutputs : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<DigitalOutputsConfiguration>, IUpdateObserver<DiagnosticActionsConfiguration>, IUpdateObserver<DiagnosticsDatabaseConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver
	{
		public delegate void DataChangedHandler(DigitalOutputsConfiguration data);

		private IContainer components;

		private GroupBox groupBoxDigitalOutputs;

		private DigitalOutputsGrid digitalOutputsGrid;

		private TableLayoutPanel tableLayoutPanel1;

		private Label labelInfo;

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.digitalOutputsGrid.ApplicationDatabaseManager;
			}
			set
			{
				this.digitalOutputsGrid.ApplicationDatabaseManager = value;
			}
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.digitalOutputsGrid.ModelValidator;
			}
			set
			{
				this.digitalOutputsGrid.ModelValidator = value;
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
				return PageType.DigitalOutputs;
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
				bool visible = base.Visible;
				base.Visible = value;
				if (!visible && base.Visible)
				{
					this.digitalOutputsGrid.DisplayErrors();
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

		public DigitalOutputs()
		{
			this.InitializeComponent();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is DigitalOutputsConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.DigitalOutputs);
			}
			this.digitalOutputsGrid.Init();
		}

		void IPropertyWindow.Reset()
		{
			this.digitalOutputsGrid.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			return !this.digitalOutputsGrid.ModelValidator.LoggerSpecifics.IO.IsDigitalOutputSupported || this.digitalOutputsGrid.ValidateInput(false);
		}

		bool IPropertyWindow.HasErrors()
		{
			return this.digitalOutputsGrid.ModelValidator.LoggerSpecifics.IO.IsDigitalOutputSupported && this.digitalOutputsGrid.HasErrors();
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			return this.digitalOutputsGrid.ModelValidator.LoggerSpecifics.IO.IsDigitalOutputSupported && this.digitalOutputsGrid.HasGlobalErrors();
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			return this.digitalOutputsGrid.ModelValidator.LoggerSpecifics.IO.IsDigitalOutputSupported && this.digitalOutputsGrid.HasLocalErrors();
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			return this.digitalOutputsGrid.ModelValidator.LoggerSpecifics.IO.IsDigitalOutputSupported && this.digitalOutputsGrid.HasFormatErrors();
		}

		void IUpdateObserver<DigitalOutputsConfiguration>.Update(DigitalOutputsConfiguration data)
		{
			if (this.digitalOutputsGrid.ModelValidator.LoggerSpecifics.IO.IsDigitalOutputSupported)
			{
				this.digitalOutputsGrid.DigitalOutputsConfiguration = data;
			}
		}

		void IUpdateObserver<DisplayMode>.Update(DisplayMode data)
		{
			if (this.digitalOutputsGrid.ModelValidator.LoggerSpecifics.IO.IsDigitalOutputSupported)
			{
				this.digitalOutputsGrid.DisplayMode = data;
			}
		}

		void IUpdateObserver<DiagnosticActionsConfiguration>.Update(DiagnosticActionsConfiguration data)
		{
			if (this.digitalOutputsGrid.ModelValidator.LoggerSpecifics.IO.IsDigitalOutputSupported)
			{
				this.digitalOutputsGrid.DiagnosticActionsConfiguration = data;
				((IPropertyWindow)this).ValidateInput();
			}
		}

		void IUpdateObserver<DiagnosticsDatabaseConfiguration>.Update(DiagnosticsDatabaseConfiguration data)
		{
			if (this.digitalOutputsGrid.ModelValidator.LoggerSpecifics.IO.IsDigitalOutputSupported)
			{
				this.digitalOutputsGrid.DiagnosticDatabaseConfiguration = data;
				((IPropertyWindow)this).ValidateInput();
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

		public bool Serialize(DigitalOutputsPage digitalOutputsPage)
		{
			if (digitalOutputsPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.digitalOutputsGrid.Serialize(digitalOutputsPage);
		}

		public bool DeSerialize(DigitalOutputsPage digitalOutputsPage)
		{
			if (digitalOutputsPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.digitalOutputsGrid.DeSerialize(digitalOutputsPage);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DigitalOutputs));
			this.groupBoxDigitalOutputs = new GroupBox();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.labelInfo = new Label();
			this.digitalOutputsGrid = new DigitalOutputsGrid();
			this.groupBoxDigitalOutputs.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBoxDigitalOutputs, "groupBoxDigitalOutputs");
			this.groupBoxDigitalOutputs.Controls.Add(this.tableLayoutPanel1);
			this.groupBoxDigitalOutputs.Controls.Add(this.digitalOutputsGrid);
			this.groupBoxDigitalOutputs.Name = "groupBoxDigitalOutputs";
			this.groupBoxDigitalOutputs.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.labelInfo, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.labelInfo, "labelInfo");
			this.labelInfo.Name = "labelInfo";
			componentResourceManager.ApplyResources(this.digitalOutputsGrid, "digitalOutputsGrid");
			this.digitalOutputsGrid.ApplicationDatabaseManager = null;
			this.digitalOutputsGrid.DigitalOutputsConfiguration = null;
			this.digitalOutputsGrid.DisplayMode = null;
			this.digitalOutputsGrid.Name = "digitalOutputsGrid";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxDigitalOutputs);
			base.Name = "DigitalOutputs";
			this.groupBoxDigitalOutputs.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
