using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.FiltersPage
{
	public class SelectColumnInCSVFile : Form
	{
		private uint selectedColumn;

		private IContainer components;

		private Label labelDescription;

		private Label label1;

		private ComboBox comboBoxColNumber;

		private Button buttonOK;

		private TextBox textBoxFilePreview;

		private GroupBox groupBoxFilePreview;

		private Button buttonCancel;

		private Button buttonHelp;

		public string Filename
		{
			get;
			set;
		}

		public string PreviewText
		{
			get;
			set;
		}

		public uint TotalNumberOfColumns
		{
			get;
			set;
		}

		public uint SelectedColumn
		{
			get
			{
				return this.selectedColumn;
			}
		}

		public SelectColumnInCSVFile()
		{
			this.InitializeComponent();
			this.TotalNumberOfColumns = 1u;
			this.selectedColumn = 1u;
		}

		private void SelectColumnInCSVFile_Shown(object sender, EventArgs e)
		{
			this.labelDescription.Text = string.Format(Resources.ColumnSelectionDescription, this.Filename, this.TotalNumberOfColumns);
			this.textBoxFilePreview.Text = this.PreviewText;
			this.comboBoxColNumber.Items.Clear();
			for (uint num = 1u; num <= this.TotalNumberOfColumns; num += 1u)
			{
				this.comboBoxColNumber.Items.Add(num.ToString());
			}
			if (this.comboBoxColNumber.Items.Count > 0)
			{
				this.comboBoxColNumber.SelectedIndex = 0;
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			string s = this.comboBoxColNumber.SelectedItem.ToString();
			this.selectedColumn = uint.Parse(s);
			base.DialogResult = DialogResult.OK;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void SelectColumnInCSVFile_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SelectColumnInCSVFile));
			this.labelDescription = new Label();
			this.label1 = new Label();
			this.comboBoxColNumber = new ComboBox();
			this.buttonOK = new Button();
			this.textBoxFilePreview = new TextBox();
			this.groupBoxFilePreview = new GroupBox();
			this.buttonCancel = new Button();
			this.buttonHelp = new Button();
			this.groupBoxFilePreview.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelDescription, "labelDescription");
			this.labelDescription.Name = "labelDescription";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.comboBoxColNumber, "comboBoxColNumber");
			this.comboBoxColNumber.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxColNumber.FormattingEnabled = true;
			this.comboBoxColNumber.Name = "comboBoxColNumber";
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.textBoxFilePreview.AcceptsReturn = true;
			componentResourceManager.ApplyResources(this.textBoxFilePreview, "textBoxFilePreview");
			this.textBoxFilePreview.Name = "textBoxFilePreview";
			this.textBoxFilePreview.ReadOnly = true;
			componentResourceManager.ApplyResources(this.groupBoxFilePreview, "groupBoxFilePreview");
			this.groupBoxFilePreview.Controls.Add(this.textBoxFilePreview);
			this.groupBoxFilePreview.Name = "groupBoxFilePreview";
			this.groupBoxFilePreview.TabStop = false;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.groupBoxFilePreview);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.comboBoxColNumber);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.labelDescription);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "SelectColumnInCSVFile";
			base.Shown += new EventHandler(this.SelectColumnInCSVFile_Shown);
			base.HelpRequested += new HelpEventHandler(this.SelectColumnInCSVFile_HelpRequested);
			this.groupBoxFilePreview.ResumeLayout(false);
			this.groupBoxFilePreview.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
