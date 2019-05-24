using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Vector.ConfigurationDialog;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil;

namespace Vector.VLConfig.GUI.Options
{
	public class AdditionalDrives : ConfigurationPageView, IConfigurationPageController, IDisposable
	{
		private IContainer components;

		private TitledGroup titledGroupAddtionalDrives;

		private Button buttonRemove;

		private Button buttonAdd;

		private Label labelWarning;

		private Label labelAvailableDrives;

		private Label labelDescription;

		private ComboBox comboBoxAvailableDrives;

		private Label labelAdditionalDrivesList;

		private ListBox listBoxConfiguredDrives;

		private GlobalOptions GlobalOptions
		{
			get;
			set;
		}

		public uint PageID
		{
			get
			{
				return 1u;
			}
		}

		public uint HelpID
		{
			get
			{
				return (uint)GUIUtil.HelpPageID_AddCardReaderDrives;
			}
		}

		public Control ConfigurationPageView
		{
			get
			{
				return this;
			}
		}

		public AdditionalDrives(GlobalOptions globalOptions)
		{
			this.InitializeComponent();
			this.GlobalOptions = globalOptions;
			this.Init();
		}

		public void Init()
		{
			this.InitAvailableDrives();
			this.InitConfiguredDrives();
			this.UpdateUI();
		}

		private void InitAvailableDrives()
		{
			string b = "C" + Path.VolumeSeparatorChar + Path.DirectorySeparatorChar;
			IEnumerable<DriveInfo> availableFixedDrives = FileSystemServices.GetAvailableFixedDrives();
			foreach (DriveInfo current in availableFixedDrives)
			{
				if (!(current.Name == b) && !this.GlobalOptions.AdditionalDrivesList.Contains(current.Name))
				{
					this.comboBoxAvailableDrives.Items.Add(current.Name);
				}
			}
			if (this.comboBoxAvailableDrives.Items.Count > 0)
			{
				this.comboBoxAvailableDrives.SelectedIndex = 0;
			}
		}

		private void InitConfiguredDrives()
		{
			if (this.GlobalOptions.AdditionalDrivesList != null)
			{
				foreach (string current in this.GlobalOptions.AdditionalDrivesList)
				{
					this.listBoxConfiguredDrives.Items.Add(current);
				}
				if (this.listBoxConfiguredDrives.Items.Count > 0)
				{
					this.listBoxConfiguredDrives.SelectedIndex = 0;
				}
			}
		}

		public void UpdateUI()
		{
			this.buttonAdd.Enabled = (this.comboBoxAvailableDrives.Items.Count > 0 && null != this.comboBoxAvailableDrives.SelectedItem);
			this.buttonRemove.Enabled = (this.listBoxConfiguredDrives.Items.Count > 0 && null != this.listBoxConfiguredDrives.SelectedItem);
			this.Refresh();
		}

		public void UpdateOptions()
		{
			this.GlobalOptions.AdditionalDrivesList.Clear();
			foreach (object current in this.listBoxConfiguredDrives.Items)
			{
				this.GlobalOptions.AdditionalDrivesList.Add(current.ToString());
			}
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			this.listBoxConfiguredDrives.Items.Add(this.comboBoxAvailableDrives.SelectedItem.ToString());
			this.listBoxConfiguredDrives.SelectedItem = this.comboBoxAvailableDrives.SelectedItem.ToString();
			this.comboBoxAvailableDrives.Items.Remove(this.comboBoxAvailableDrives.SelectedItem);
			if (this.comboBoxAvailableDrives.Items.Count > 0)
			{
				this.comboBoxAvailableDrives.SelectedIndex = 0;
			}
			this.UpdateUI();
		}

		private void buttonRemove_Click(object sender, EventArgs e)
		{
			this.comboBoxAvailableDrives.Items.Add(this.listBoxConfiguredDrives.SelectedItem.ToString());
			this.comboBoxAvailableDrives.SelectedItem = this.listBoxConfiguredDrives.SelectedItem.ToString();
			this.listBoxConfiguredDrives.Items.Remove(this.listBoxConfiguredDrives.SelectedItem);
			if (this.listBoxConfiguredDrives.Items.Count > 0)
			{
				this.listBoxConfiguredDrives.SelectedIndex = 0;
			}
			this.UpdateUI();
		}

		public bool SavePageData()
		{
			this.UpdateOptions();
			return false;
		}

		public virtual void CancelPageData()
		{
		}

		public void OnViewOpening()
		{
		}

		public void OnViewOpened()
		{
		}

		public bool OnViewClosing(DialogResult closingReason)
		{
			return true;
		}

		public void OnViewClosed()
		{
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AdditionalDrives));
			this.titledGroupAddtionalDrives = new TitledGroup();
			this.buttonRemove = new Button();
			this.buttonAdd = new Button();
			this.labelWarning = new Label();
			this.labelAvailableDrives = new Label();
			this.labelDescription = new Label();
			this.comboBoxAvailableDrives = new ComboBox();
			this.labelAdditionalDrivesList = new Label();
			this.listBoxConfiguredDrives = new ListBox();
			this.titledGroupAddtionalDrives.SuspendLayout();
			base.SuspendLayout();
			this.titledGroupAddtionalDrives.AllowDrop = true;
			componentResourceManager.ApplyResources(this.titledGroupAddtionalDrives, "titledGroupAddtionalDrives");
			this.titledGroupAddtionalDrives.AutoSizeGroup = true;
			this.titledGroupAddtionalDrives.BackColor = SystemColors.Window;
			this.titledGroupAddtionalDrives.Controls.Add(this.buttonRemove);
			this.titledGroupAddtionalDrives.Controls.Add(this.buttonAdd);
			this.titledGroupAddtionalDrives.Controls.Add(this.labelWarning);
			this.titledGroupAddtionalDrives.Controls.Add(this.labelAvailableDrives);
			this.titledGroupAddtionalDrives.Controls.Add(this.labelDescription);
			this.titledGroupAddtionalDrives.Controls.Add(this.comboBoxAvailableDrives);
			this.titledGroupAddtionalDrives.Controls.Add(this.labelAdditionalDrivesList);
			this.titledGroupAddtionalDrives.Controls.Add(this.listBoxConfiguredDrives);
			this.titledGroupAddtionalDrives.Image = null;
			this.titledGroupAddtionalDrives.Name = "titledGroupAddtionalDrives";
			componentResourceManager.ApplyResources(this.buttonRemove, "buttonRemove");
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.UseVisualStyleBackColor = true;
			this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
			componentResourceManager.ApplyResources(this.buttonAdd, "buttonAdd");
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.UseVisualStyleBackColor = true;
			this.buttonAdd.Click += new EventHandler(this.buttonAdd_Click);
			componentResourceManager.ApplyResources(this.labelWarning, "labelWarning");
			this.labelWarning.Name = "labelWarning";
			componentResourceManager.ApplyResources(this.labelAvailableDrives, "labelAvailableDrives");
			this.labelAvailableDrives.Name = "labelAvailableDrives";
			componentResourceManager.ApplyResources(this.labelDescription, "labelDescription");
			this.labelDescription.Name = "labelDescription";
			componentResourceManager.ApplyResources(this.comboBoxAvailableDrives, "comboBoxAvailableDrives");
			this.comboBoxAvailableDrives.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAvailableDrives.FormattingEnabled = true;
			this.comboBoxAvailableDrives.Name = "comboBoxAvailableDrives";
			this.comboBoxAvailableDrives.Sorted = true;
			componentResourceManager.ApplyResources(this.labelAdditionalDrivesList, "labelAdditionalDrivesList");
			this.labelAdditionalDrivesList.Name = "labelAdditionalDrivesList";
			componentResourceManager.ApplyResources(this.listBoxConfiguredDrives, "listBoxConfiguredDrives");
			this.listBoxConfiguredDrives.FormattingEnabled = true;
			this.listBoxConfiguredDrives.Name = "listBoxConfiguredDrives";
			this.listBoxConfiguredDrives.Sorted = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.titledGroupAddtionalDrives);
			base.Name = "AdditionalDrives";
			this.titledGroupAddtionalDrives.ResumeLayout(false);
			this.titledGroupAddtionalDrives.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
