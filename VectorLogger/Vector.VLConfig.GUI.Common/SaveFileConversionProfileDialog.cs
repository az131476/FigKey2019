using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Vector.VLConfig.GUI.Common
{
	public class SaveFileConversionProfileDialog : Form
	{
		private string mDisplayName;

		private bool mHasMarkers;

		private bool mHasTriggers;

		private IContainer components;

		private Label label1;

		private TextBox mTextBoxDisplayName;

		private Button mButtonOK;

		private Button mButtonCancel;

		private CheckBox mCheckBoxStoreMarkers;

		private Label mLabelMarkersHint;

		private Label mLabelTriggersHint;

		private CheckBox mCheckBoxStoreTriggers;

		public string DisplayName
		{
			get
			{
				return this.mDisplayName;
			}
			set
			{
				this.mTextBoxDisplayName.Text = value;
			}
		}

		public bool MarkersAvailable
		{
			get;
			set;
		}

		public bool TriggersAvailable
		{
			get;
			set;
		}

		public bool HasMarkers
		{
			get
			{
				return this.mHasMarkers;
			}
			set
			{
				this.mCheckBoxStoreMarkers.Checked = value;
			}
		}

		public bool HasTriggers
		{
			get
			{
				return this.mHasTriggers;
			}
			set
			{
				this.mCheckBoxStoreTriggers.Checked = value;
			}
		}

		public SaveFileConversionProfileDialog()
		{
			this.InitializeComponent();
		}

		private void TextBoxDisplayName_TextChanged(object sender, EventArgs e)
		{
			this.mDisplayName = this.mTextBoxDisplayName.Text;
		}

		private void CheckBoxStoreMarkers_CheckedChanged(object sender, EventArgs e)
		{
			this.mHasMarkers = this.mCheckBoxStoreMarkers.Checked;
		}

		private void CheckBoxStoreTriggers_CheckedChanged(object sender, EventArgs e)
		{
			this.mHasTriggers = this.mCheckBoxStoreTriggers.Checked;
		}

		private void SaveFileConversionProfileDialog_Load(object sender, EventArgs e)
		{
			if (!this.MarkersAvailable && !this.TriggersAvailable)
			{
				this.mCheckBoxStoreMarkers.Visible = false;
				this.mLabelMarkersHint.Visible = false;
				this.mCheckBoxStoreTriggers.Visible = false;
				this.mLabelTriggersHint.Visible = false;
				int num = this.mButtonOK.Top - this.mTextBoxDisplayName.Bottom - 14;
				base.Height -= num;
				return;
			}
			if (!this.MarkersAvailable)
			{
				this.mCheckBoxStoreMarkers.Visible = false;
				this.mLabelMarkersHint.Visible = false;
				int num2 = this.mCheckBoxStoreTriggers.Top - this.mTextBoxDisplayName.Bottom - 14;
				base.Height -= num2;
				this.mCheckBoxStoreTriggers.Top = this.mCheckBoxStoreMarkers.Top;
				this.mLabelTriggersHint.Top = this.mLabelMarkersHint.Top;
				return;
			}
			if (!this.TriggersAvailable)
			{
				this.mCheckBoxStoreTriggers.Visible = false;
				this.mLabelTriggersHint.Visible = false;
				int num3 = this.mButtonOK.Top - this.mLabelMarkersHint.Bottom - 14;
				base.Height -= num3;
			}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SaveFileConversionProfileDialog));
			this.label1 = new Label();
			this.mTextBoxDisplayName = new TextBox();
			this.mButtonOK = new Button();
			this.mButtonCancel = new Button();
			this.mCheckBoxStoreMarkers = new CheckBox();
			this.mLabelMarkersHint = new Label();
			this.mLabelTriggersHint = new Label();
			this.mCheckBoxStoreTriggers = new CheckBox();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.mTextBoxDisplayName, "mTextBoxDisplayName");
			this.mTextBoxDisplayName.Name = "mTextBoxDisplayName";
			this.mTextBoxDisplayName.TextChanged += new EventHandler(this.TextBoxDisplayName_TextChanged);
			componentResourceManager.ApplyResources(this.mButtonOK, "mButtonOK");
			this.mButtonOK.DialogResult = DialogResult.OK;
			this.mButtonOK.Name = "mButtonOK";
			this.mButtonOK.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mButtonCancel, "mButtonCancel");
			this.mButtonCancel.DialogResult = DialogResult.Cancel;
			this.mButtonCancel.Name = "mButtonCancel";
			this.mButtonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mCheckBoxStoreMarkers, "mCheckBoxStoreMarkers");
			this.mCheckBoxStoreMarkers.Name = "mCheckBoxStoreMarkers";
			this.mCheckBoxStoreMarkers.UseVisualStyleBackColor = true;
			this.mCheckBoxStoreMarkers.CheckedChanged += new EventHandler(this.CheckBoxStoreMarkers_CheckedChanged);
			componentResourceManager.ApplyResources(this.mLabelMarkersHint, "mLabelMarkersHint");
			this.mLabelMarkersHint.Name = "mLabelMarkersHint";
			componentResourceManager.ApplyResources(this.mLabelTriggersHint, "mLabelTriggersHint");
			this.mLabelTriggersHint.Name = "mLabelTriggersHint";
			componentResourceManager.ApplyResources(this.mCheckBoxStoreTriggers, "mCheckBoxStoreTriggers");
			this.mCheckBoxStoreTriggers.Name = "mCheckBoxStoreTriggers";
			this.mCheckBoxStoreTriggers.UseVisualStyleBackColor = true;
			this.mCheckBoxStoreTriggers.CheckedChanged += new EventHandler(this.CheckBoxStoreTriggers_CheckedChanged);
			base.AcceptButton = this.mButtonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.mButtonCancel;
			base.Controls.Add(this.mLabelTriggersHint);
			base.Controls.Add(this.mCheckBoxStoreTriggers);
			base.Controls.Add(this.mLabelMarkersHint);
			base.Controls.Add(this.mCheckBoxStoreMarkers);
			base.Controls.Add(this.mButtonCancel);
			base.Controls.Add(this.mButtonOK);
			base.Controls.Add(this.mTextBoxDisplayName);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "SaveFileConversionProfileDialog";
			base.ShowInTaskbar = false;
			base.Load += new EventHandler(this.SaveFileConversionProfileDialog_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
