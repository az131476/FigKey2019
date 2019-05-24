using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.GUI.CcpXcpSignalRequestsPage
{
	public class CcpXcpSignalImportAssistent : Form
	{
		private IContainer components;

		private Button buttonYes;

		private Button buttonNo;

		private CheckBox checkBoxMatchCase;

		private ComboBox comboBoxECUs;

		private Label label1;

		private Label labelAssignTo;

		public string SelectedECU
		{
			get
			{
				return this.comboBoxECUs.SelectedItem.ToString();
			}
		}

		public bool MatchCase
		{
			get
			{
				return this.checkBoxMatchCase.Checked;
			}
		}

		public CcpXcpSignalImportAssistent(ReadOnlyCollection<Database> dbList)
		{
			this.InitializeComponent();
			foreach (Database current in dbList)
			{
				this.comboBoxECUs.Items.Add(current.CcpXcpEcuDisplayName);
			}
			if (this.comboBoxECUs.Items.Count > 0)
			{
				this.comboBoxECUs.SelectedIndex = 0;
			}
			this.checkBoxMatchCase.Checked = (dbList.Count != 1);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CcpXcpSignalImportAssistent));
			this.buttonYes = new Button();
			this.buttonNo = new Button();
			this.checkBoxMatchCase = new CheckBox();
			this.comboBoxECUs = new ComboBox();
			this.label1 = new Label();
			this.labelAssignTo = new Label();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.buttonYes, "buttonYes");
			this.buttonYes.DialogResult = DialogResult.Yes;
			this.buttonYes.Name = "buttonYes";
			this.buttonYes.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonNo, "buttonNo");
			this.buttonNo.DialogResult = DialogResult.No;
			this.buttonNo.Name = "buttonNo";
			this.buttonNo.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBoxMatchCase, "checkBoxMatchCase");
			this.checkBoxMatchCase.Name = "checkBoxMatchCase";
			this.checkBoxMatchCase.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.comboBoxECUs, "comboBoxECUs");
			this.comboBoxECUs.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxECUs.FormattingEnabled = true;
			this.comboBoxECUs.Name = "comboBoxECUs";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.labelAssignTo, "labelAssignTo");
			this.labelAssignTo.Name = "labelAssignTo";
			base.AcceptButton = this.buttonYes;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ControlBox = false;
			base.Controls.Add(this.labelAssignTo);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.comboBoxECUs);
			base.Controls.Add(this.checkBoxMatchCase);
			base.Controls.Add(this.buttonNo);
			base.Controls.Add(this.buttonYes);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CcpXcpSignalImportAssistent";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
