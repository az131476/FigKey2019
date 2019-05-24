using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.FiltersPage
{
	public class CANIdCondition : Form
	{
		private uint messageId;

		private uint lastMessageId;

		private bool isExtendedId;

		private bool isRange;

		private IContainer components;

		private Label labelConditionType;

		private ComboBox comboBoxConditionType;

		private Label labelID;

		private TextBox textBoxCANId;

		private TextBox textBoxCANIdLast;

		private Label labelLastCANId;

		private Label labelDescription;

		private Button buttonOk;

		private Button buttonCancel;

		private ErrorProvider errorProvider;

		private CheckBox checkBoxIsExtendedId;

		private Button buttonHelp;

		public uint MessageID
		{
			get
			{
				return this.messageId;
			}
			set
			{
				this.messageId = value;
			}
		}

		public uint LastMessageID
		{
			get
			{
				return this.lastMessageId;
			}
			set
			{
				this.lastMessageId = value;
			}
		}

		public bool IsExtendedId
		{
			get
			{
				return this.isExtendedId;
			}
			set
			{
				this.isExtendedId = value;
			}
		}

		public bool IsRange
		{
			get
			{
				return this.isRange;
			}
			set
			{
				this.isRange = value;
			}
		}

		public CANIdCondition()
		{
			this.InitializeComponent();
			this.messageId = 0u;
			this.lastMessageId = 0u;
			this.isExtendedId = false;
			this.isRange = false;
		}

		private void InitControls()
		{
			this.ResetErrorProvider();
			this.SubscribeControlEvents(false);
			this.comboBoxConditionType.Items.Add(Resources.FilterConditionRelationIDValue);
			this.comboBoxConditionType.Items.Add(Resources.FilterConditionRelationIDRange);
			this.comboBoxConditionType.SelectedIndex = (this.isRange ? 1 : 0);
			this.checkBoxIsExtendedId.Checked = this.isExtendedId;
			this.textBoxCANId.Text = GUIUtil.CANIdToDisplayString(this.messageId, this.isExtendedId);
			this.textBoxCANIdLast.Text = GUIUtil.CANIdToDisplayString(this.lastMessageId, this.isExtendedId);
			if (!this.isRange)
			{
				this.textBoxCANIdLast.Enabled = false;
			}
			this.SubscribeControlEvents(true);
		}

		private void ResetErrorProvider()
		{
			this.errorProvider.SetError(this.textBoxCANId, "");
			this.errorProvider.SetError(this.textBoxCANIdLast, "");
		}

		private void CANIdCondition_Shown(object sender, EventArgs e)
		{
			this.RenderLabelsForDisplayMode();
			this.InitControls();
		}

		private void comboBoxConditionType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.comboBoxConditionType.SelectedItem.ToString() == Resources.FilterConditionRelationIDValue)
			{
				this.textBoxCANIdLast.Enabled = false;
				this.lastMessageId = 0u;
				this.textBoxCANIdLast.Text = GUIUtil.CANIdToDisplayString(this.lastMessageId, this.isExtendedId);
				this.errorProvider.SetError(this.textBoxCANIdLast, "");
				this.isRange = false;
				return;
			}
			this.textBoxCANIdLast.Enabled = true;
			this.isRange = true;
		}

		private void checkBoxIsExtendedId_CheckedChanged(object sender, EventArgs e)
		{
			if (this.checkBoxIsExtendedId.Checked)
			{
				if (!this.HasError(this.textBoxCANId))
				{
					this.textBoxCANId.Text = string.Format(Resources.ExtendedCANIdFormat, this.textBoxCANId.Text);
				}
				if (!this.HasError(this.textBoxCANIdLast))
				{
					this.textBoxCANIdLast.Text = string.Format(Resources.ExtendedCANIdFormat, this.textBoxCANIdLast.Text);
				}
				this.isExtendedId = true;
			}
			else
			{
				if (!this.HasError(this.textBoxCANId))
				{
					string text = this.textBoxCANId.Text;
					if (text.Length > 0 && text.Substring(text.Length - 1).ToLower() == "x")
					{
						this.textBoxCANId.Text = text.Substring(0, text.Length - 1);
					}
				}
				if (!this.HasError(this.textBoxCANIdLast))
				{
					string text2 = this.textBoxCANIdLast.Text;
					if (text2.Length > 0 && text2.Substring(text2.Length - 1).ToLower() == "x")
					{
						this.textBoxCANIdLast.Text = text2.Substring(0, text2.Length - 1);
					}
				}
				this.isExtendedId = false;
			}
			this.ValidateChildren();
		}

		private void textBoxCANId_Validating(object sender, CancelEventArgs e)
		{
			uint num;
			bool flag;
			if (!GUIUtil.DisplayStringToCANId(this.textBoxCANId.Text, out num, out flag))
			{
				this.errorProvider.SetError(this.textBoxCANId, "");
				if (this.isExtendedId)
				{
					this.errorProvider.SetError(this.textBoxCANId, Resources.ErrorExtendedCANIdExpected);
				}
				else
				{
					this.errorProvider.SetError(this.textBoxCANId, Resources.ErrorStandardCANIdExpected);
				}
				e.Cancel = true;
				return;
			}
			if (this.isExtendedId == flag)
			{
				this.messageId = num;
				return;
			}
			this.errorProvider.SetError(this.textBoxCANId, "");
			if (this.isExtendedId)
			{
				this.errorProvider.SetError(this.textBoxCANId, Resources.ErrorExtendedCANIdExpected);
			}
			else
			{
				this.errorProvider.SetError(this.textBoxCANId, Resources.ErrorStandardCANIdExpected);
			}
			e.Cancel = true;
		}

		private void textBoxCANId_Validated(object sender, EventArgs e)
		{
			this.errorProvider.SetError(this.textBoxCANId, "");
		}

		private void textBoxCANIdLast_Validating(object sender, CancelEventArgs e)
		{
			uint lastMessageID;
			bool flag;
			if (!GUIUtil.DisplayStringToCANId(this.textBoxCANIdLast.Text, out lastMessageID, out flag))
			{
				this.errorProvider.SetError(this.textBoxCANIdLast, "");
				if (this.isExtendedId)
				{
					this.errorProvider.SetError(this.textBoxCANIdLast, Resources.ErrorExtendedCANIdExpected);
				}
				else
				{
					this.errorProvider.SetError(this.textBoxCANIdLast, Resources.ErrorStandardCANIdExpected);
				}
				e.Cancel = true;
				return;
			}
			if (this.isExtendedId == flag)
			{
				this.LastMessageID = lastMessageID;
				return;
			}
			this.errorProvider.SetError(this.textBoxCANIdLast, "");
			if (this.isExtendedId)
			{
				this.errorProvider.SetError(this.textBoxCANIdLast, Resources.ErrorExtendedCANIdExpected);
			}
			else
			{
				this.errorProvider.SetError(this.textBoxCANIdLast, Resources.ErrorStandardCANIdExpected);
			}
			e.Cancel = true;
		}

		private void textBoxCANIdLast_Validated(object sender, EventArgs e)
		{
			this.errorProvider.SetError(this.textBoxCANIdLast, "");
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			if (!this.ValidateChildren())
			{
				InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
				base.DialogResult = DialogResult.None;
				return;
			}
			if (this.IsRange && this.messageId > this.lastMessageId)
			{
				uint num = this.lastMessageId;
				this.lastMessageId = this.messageId;
				this.messageId = num;
			}
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void CANIdCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void SubscribeControlEvents(bool doSubscribe)
		{
			if (doSubscribe)
			{
				this.comboBoxConditionType.SelectedIndexChanged += new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
				this.checkBoxIsExtendedId.CheckedChanged += new EventHandler(this.checkBoxIsExtendedId_CheckedChanged);
				this.textBoxCANId.Validating += new CancelEventHandler(this.textBoxCANId_Validating);
				this.textBoxCANId.Validated += new EventHandler(this.textBoxCANId_Validated);
				this.textBoxCANIdLast.Validating += new CancelEventHandler(this.textBoxCANIdLast_Validating);
				this.textBoxCANIdLast.Validated += new EventHandler(this.textBoxCANIdLast_Validated);
				return;
			}
			this.comboBoxConditionType.SelectedIndexChanged -= new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
			this.checkBoxIsExtendedId.CheckedChanged -= new EventHandler(this.checkBoxIsExtendedId_CheckedChanged);
			this.textBoxCANId.Validating -= new CancelEventHandler(this.textBoxCANId_Validating);
			this.textBoxCANId.Validated -= new EventHandler(this.textBoxCANId_Validated);
			this.textBoxCANIdLast.Validating -= new CancelEventHandler(this.textBoxCANIdLast_Validating);
			this.textBoxCANIdLast.Validated -= new EventHandler(this.textBoxCANIdLast_Validated);
		}

		private bool HasError(Control control)
		{
			return !string.IsNullOrEmpty(this.errorProvider.GetError(control));
		}

		private void RenderLabelsForDisplayMode()
		{
			string arg = Resources.DisplayModeDec;
			if (GUIUtil.IsHexadecimal)
			{
				arg = Resources.DisplayModeHex;
			}
			this.labelID.Text = string.Format(Resources.IDLabelWithMode, arg);
			this.labelLastCANId.Text = string.Format(Resources.LastIDLabelWithMode, arg);
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
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CANIdCondition));
			this.labelConditionType = new Label();
			this.comboBoxConditionType = new ComboBox();
			this.labelID = new Label();
			this.textBoxCANId = new TextBox();
			this.textBoxCANIdLast = new TextBox();
			this.labelLastCANId = new Label();
			this.labelDescription = new Label();
			this.buttonOk = new Button();
			this.buttonCancel = new Button();
			this.errorProvider = new ErrorProvider(this.components);
			this.checkBoxIsExtendedId = new CheckBox();
			this.buttonHelp = new Button();
			((ISupportInitialize)this.errorProvider).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelConditionType, "labelConditionType");
			this.labelConditionType.Name = "labelConditionType";
			this.comboBoxConditionType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxConditionType.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxConditionType, "comboBoxConditionType");
			this.comboBoxConditionType.Name = "comboBoxConditionType";
			this.comboBoxConditionType.SelectedIndexChanged += new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelID, "labelID");
			this.labelID.Name = "labelID";
			this.errorProvider.SetIconAlignment(this.textBoxCANId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId.IconAlignment"));
			componentResourceManager.ApplyResources(this.textBoxCANId, "textBoxCANId");
			this.textBoxCANId.Name = "textBoxCANId";
			this.textBoxCANId.Validating += new CancelEventHandler(this.textBoxCANId_Validating);
			this.textBoxCANId.Validated += new EventHandler(this.textBoxCANId_Validated);
			this.errorProvider.SetIconAlignment(this.textBoxCANIdLast, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANIdLast.IconAlignment"));
			componentResourceManager.ApplyResources(this.textBoxCANIdLast, "textBoxCANIdLast");
			this.textBoxCANIdLast.Name = "textBoxCANIdLast";
			this.textBoxCANIdLast.Validating += new CancelEventHandler(this.textBoxCANIdLast_Validating);
			this.textBoxCANIdLast.Validated += new EventHandler(this.textBoxCANIdLast_Validated);
			componentResourceManager.ApplyResources(this.labelLastCANId, "labelLastCANId");
			this.labelLastCANId.Name = "labelLastCANId";
			componentResourceManager.ApplyResources(this.labelDescription, "labelDescription");
			this.labelDescription.Name = "labelDescription";
			this.buttonOk.DialogResult = DialogResult.OK;
			componentResourceManager.ApplyResources(this.buttonOk, "buttonOk");
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new EventHandler(this.buttonOk_Click);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProvider.ContainerControl = this;
			componentResourceManager.ApplyResources(this.checkBoxIsExtendedId, "checkBoxIsExtendedId");
			this.checkBoxIsExtendedId.Checked = true;
			this.checkBoxIsExtendedId.CheckState = CheckState.Checked;
			this.checkBoxIsExtendedId.Name = "checkBoxIsExtendedId";
			this.checkBoxIsExtendedId.UseVisualStyleBackColor = true;
			this.checkBoxIsExtendedId.CheckedChanged += new EventHandler(this.checkBoxIsExtendedId_CheckedChanged);
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			base.AcceptButton = this.buttonOk;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoValidate = AutoValidate.EnableAllowFocusChange;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.checkBoxIsExtendedId);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOk);
			base.Controls.Add(this.labelDescription);
			base.Controls.Add(this.labelLastCANId);
			base.Controls.Add(this.textBoxCANIdLast);
			base.Controls.Add(this.textBoxCANId);
			base.Controls.Add(this.labelID);
			base.Controls.Add(this.comboBoxConditionType);
			base.Controls.Add(this.labelConditionType);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CANIdCondition";
			base.SizeGripStyle = SizeGripStyle.Hide;
			base.Shown += new EventHandler(this.CANIdCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.CANIdCondition_HelpRequested);
			((ISupportInitialize)this.errorProvider).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
