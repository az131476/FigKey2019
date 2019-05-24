using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.DatabasesPage
{
	public class NetworkSelection : Form
	{
		private IContainer components;

		private Label labelHeadline;

		private Button buttonOK;

		private Button buttonCancel;

		private Button buttonHelp;

		private ListView listViewNetworks;

		private ColumnHeader colNetwork;

		private ColumnHeader colBusType;

		public string Filename
		{
			get;
			set;
		}

		public Dictionary<string, BusType> NetworkNamesToBusType
		{
			get;
			set;
		}

		public string SelectedNetworkName
		{
			get;
			set;
		}

		public NetworkSelection()
		{
			this.InitializeComponent();
		}

		private void NetworkSelection_Shown(object sender, EventArgs e)
		{
			this.FillListBox();
			this.labelHeadline.Text = string.Format(Resources.HeadlineSelectNetwork, this.Filename);
			this.listViewNetworks.Columns[0].Width = (this.listViewNetworks.Width - 10) / 2;
			this.listViewNetworks.Columns[1].Width = (this.listViewNetworks.Width - 10) / 2;
			this.SelectedNetworkName = "";
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (this.listViewNetworks.SelectedItems.Count > 0)
			{
				this.SelectedNetworkName = this.listViewNetworks.SelectedItems[0].SubItems[0].Text;
			}
			else
			{
				InformMessageBox.Error("No network selected");
			}
			base.DialogResult = DialogResult.OK;
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void FillListBox()
		{
			this.listViewNetworks.Items.Clear();
			foreach (string current in this.NetworkNamesToBusType.Keys)
			{
				ListViewItem listViewItem = new ListViewItem(current);
				listViewItem.SubItems.Add(GUIUtil.MapBusType2String(this.NetworkNamesToBusType[current]));
				this.listViewNetworks.Items.Add(listViewItem);
			}
			if (this.listViewNetworks.Items.Count > 0)
			{
				this.listViewNetworks.Items[0].Selected = true;
			}
			this.buttonOK.Enabled = (this.listViewNetworks.Items.Count > 0);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(NetworkSelection));
			this.labelHeadline = new Label();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.buttonHelp = new Button();
			this.listViewNetworks = new ListView();
			this.colNetwork = new ColumnHeader();
			this.colBusType = new ColumnHeader();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelHeadline, "labelHeadline");
			this.labelHeadline.Name = "labelHeadline";
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.listViewNetworks, "listViewNetworks");
			this.listViewNetworks.Columns.AddRange(new ColumnHeader[]
			{
				this.colNetwork,
				this.colBusType
			});
			this.listViewNetworks.FullRowSelect = true;
			this.listViewNetworks.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			this.listViewNetworks.HideSelection = false;
			this.listViewNetworks.MultiSelect = false;
			this.listViewNetworks.Name = "listViewNetworks";
			this.listViewNetworks.UseCompatibleStateImageBehavior = false;
			this.listViewNetworks.View = View.Details;
			componentResourceManager.ApplyResources(this.colNetwork, "colNetwork");
			componentResourceManager.ApplyResources(this.colBusType, "colBusType");
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.listViewNetworks);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.labelHeadline);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "NetworkSelection";
			base.Shown += new EventHandler(this.NetworkSelection_Shown);
			base.ResumeLayout(false);
		}
	}
}
