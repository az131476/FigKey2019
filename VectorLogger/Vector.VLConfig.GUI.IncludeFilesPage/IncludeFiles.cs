using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.IncludeFilesPage
{
	public class IncludeFiles : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<IncludeFileConfiguration>, IUpdateObserver
	{
		public delegate void DataChangedHandler(IncludeFileConfiguration data);

		private IContainer components;

		private GroupBox groupBoxIncludeFiles;

		private IncludeFileGrid includeFileGrid;

		private TableLayoutPanel tableLayoutPanel1;

		private Button buttonAdd;

		private Button buttonRemove;

		private Button buttonNew;

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.includeFileGrid.ModelValidator;
			}
			set
			{
				this.includeFileGrid.ModelValidator = value;
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

		public DisplayMode DisplayMode
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
				return PageType.IncludeFiles;
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
					this.includeFileGrid.DisplayErrors();
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

		public IncludeFiles()
		{
			this.InitializeComponent();
			this.includeFileGrid.SelectionChanged += new EventHandler(this.OnIncludeFileGridSelectionChanged);
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is IncludeFileConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.IncludeFiles);
			}
			this.includeFileGrid.Init();
		}

		void IPropertyWindow.Reset()
		{
			this.includeFileGrid.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			return this.includeFileGrid.ValidateInput(false);
		}

		bool IPropertyWindow.HasErrors()
		{
			return this.includeFileGrid.HasErrors();
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			return this.includeFileGrid.HasGlobalErrors();
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			return this.includeFileGrid.HasLocalErrors();
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			return this.includeFileGrid.HasFormatErrors();
		}

		void IUpdateObserver<IncludeFileConfiguration>.Update(IncludeFileConfiguration data)
		{
			this.includeFileGrid.IncludeFileConfiguration = data;
		}

		public void RefreshView()
		{
			this.includeFileGrid.RefreshView();
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			GenericOpenFileDialog.FileName = "";
			if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(FileType.INCFile))
			{
				this.includeFileGrid.AddIncludeFile(GenericOpenFileDialog.FileName);
			}
		}

		private void buttonNew_Click(object sender, EventArgs e)
		{
			string text = this.includeFileGrid.ModelValidator.GetAbsoluteFilePath("\\");
			string fullPath = Path.GetFullPath(text);
			if (string.Compare(text.Trim(), fullPath, true) != 0)
			{
				text = Environment.CurrentDirectory;
			}
			if (DialogResult.OK == GenericSaveFileDialog.ShowDialog(FileType.INCFile, "", ((IPropertyWindow)this).ModelValidator.LoggerSpecifics.Name, text))
			{
				try
				{
					using (File.CreateText(GenericSaveFileDialog.FileName))
					{
					}
				}
				catch
				{
					InformMessageBox.Error(string.Format(Resources.ErrorUnableToCreateFile, GenericSaveFileDialog.FileName));
					return;
				}
				this.includeFileGrid.AddIncludeFile(GenericSaveFileDialog.FileName);
				FileSystemServices.LaunchFile(GenericSaveFileDialog.FileName);
			}
		}

		private void buttonRemove_Click(object sender, EventArgs e)
		{
			IncludeFile includeFile;
			if (this.includeFileGrid.TryGetSelectedIncludeFile(out includeFile))
			{
				this.includeFileGrid.RemoveIncludeFile(includeFile);
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

		private void OnIncludeFileGridSelectionChanged(object sender, EventArgs e)
		{
			IncludeFile includeFile;
			if (this.includeFileGrid.TryGetSelectedIncludeFile(out includeFile))
			{
				this.buttonRemove.Enabled = true;
				return;
			}
			this.buttonRemove.Enabled = false;
		}

		public bool Serialize(IncludeFilesPage includeFilesPage)
		{
			if (includeFilesPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.includeFileGrid.Serialize(includeFilesPage);
		}

		public bool DeSerialize(IncludeFilesPage includeFilesPage)
		{
			if (includeFilesPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.includeFileGrid.DeSerialize(includeFilesPage);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(IncludeFiles));
			this.groupBoxIncludeFiles = new GroupBox();
			this.includeFileGrid = new IncludeFileGrid();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.buttonAdd = new Button();
			this.buttonRemove = new Button();
			this.buttonNew = new Button();
			this.groupBoxIncludeFiles.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBoxIncludeFiles, "groupBoxIncludeFiles");
			this.groupBoxIncludeFiles.Controls.Add(this.includeFileGrid);
			this.groupBoxIncludeFiles.Controls.Add(this.tableLayoutPanel1);
			this.groupBoxIncludeFiles.Name = "groupBoxIncludeFiles";
			this.groupBoxIncludeFiles.TabStop = false;
			this.includeFileGrid.AllowDrop = true;
			componentResourceManager.ApplyResources(this.includeFileGrid, "includeFileGrid");
			this.includeFileGrid.IncludeFileConfiguration = null;
			this.includeFileGrid.Name = "includeFileGrid";
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.buttonAdd, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonRemove, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonNew, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.buttonAdd, "buttonAdd");
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.UseVisualStyleBackColor = true;
			this.buttonAdd.Click += new EventHandler(this.buttonAdd_Click);
			componentResourceManager.ApplyResources(this.buttonRemove, "buttonRemove");
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.UseVisualStyleBackColor = true;
			this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
			componentResourceManager.ApplyResources(this.buttonNew, "buttonNew");
			this.buttonNew.Name = "buttonNew";
			this.buttonNew.UseVisualStyleBackColor = true;
			this.buttonNew.Click += new EventHandler(this.buttonNew_Click);
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxIncludeFiles);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "IncludeFiles";
			this.groupBoxIncludeFiles.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
