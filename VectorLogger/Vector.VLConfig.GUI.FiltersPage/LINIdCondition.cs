using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.FiltersPage
{
	public class LINIdCondition : Form
	{
		private uint messageId;

		private uint lastMessageId;

		private bool isRange;

		private IContainer components;

		private Label labelConditionType;

		private ComboBox comboBoxConditionType;

		private Label labelID;

		private Label labelLastId;

		private TextBox textBoxLINId;

		private TextBox textBoxLINIdLast;

		private Button buttonOk;

		private Button buttonCancel;

		private ErrorProvider errorProvider;

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

		public LINIdCondition()
		{
			this.InitializeComponent();
			this.messageId = 0u;
			this.lastMessageId = 0u;
			this.isRange = false;
		}

		private void InitControls()
		{
			this.ResetErrorProvider();
			this.SubscribeControlEvents(false);
			this.comboBoxConditionType.Items.Add(Resources.FilterConditionRelationIDValue);
			this.comboBoxConditionType.Items.Add(Resources.FilterConditionRelationIDRange);
			this.comboBoxConditionType.SelectedIndex = (this.isRange ? 1 : 0);
			this.textBoxLINId.Text = GUIUtil.LINIdToDisplayString(this.messageId);
			this.textBoxLINIdLast.Text = GUIUtil.LINIdToDisplayString(this.lastMessageId);
			if (!this.isRange)
			{
				this.textBoxLINIdLast.Enabled = false;
			}
			this.SubscribeControlEvents(true);
		}

		private void ResetErrorProvider()
		{
			this.errorProvider.SetError(this.textBoxLINId, "");
			this.errorProvider.SetError(this.textBoxLINIdLast, "");
		}

		private void LINIdCondition_Shown(object sender, EventArgs e)
		{
			this.RenderLabelsForDisplayMode();
			this.InitControls();
		}

		private void comboBoxConditionType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.comboBoxConditionType.SelectedItem.ToString() == Resources.FilterConditionRelationIDValue)
			{
				this.textBoxLINIdLast.Enabled = false;
				this.lastMessageId = 0u;
				this.textBoxLINIdLast.Text = GUIUtil.LINIdToDisplayString(this.lastMessageId);
				this.errorProvider.SetError(this.textBoxLINIdLast, "");
				this.isRange = false;
				return;
			}
			this.textBoxLINIdLast.Enabled = true;
			this.isRange = true;
		}

		private void textBoxLINId_Validating(object sender, CancelEventArgs e)
		{
			uint num;
			if (GUIUtil.DisplayStringToLINId(this.textBoxLINId.Text, out num))
			{
				this.messageId = num;
				return;
			}
			this.errorProvider.SetError(this.textBoxLINId, "");
			this.errorProvider.SetError(this.textBoxLINId, Resources.ErrorLINIdExpected);
			e.Cancel = true;
		}

		private void textBoxLINId_Validated(object sender, EventArgs e)
		{
			this.errorProvider.SetError(this.textBoxLINId, "");
		}

		private void textBoxLINIdLast_Validating(object sender, CancelEventArgs e)
		{
			uint num;
			if (GUIUtil.DisplayStringToLINId(this.textBoxLINIdLast.Text, out num))
			{
				this.lastMessageId = num;
				return;
			}
			this.errorProvider.SetError(this.textBoxLINIdLast, "");
			this.errorProvider.SetError(this.textBoxLINIdLast, Resources.ErrorLINIdExpected);
			e.Cancel = true;
		}

		private void textBoxLINIdLast_Validated(object sender, EventArgs e)
		{
			this.errorProvider.SetError(this.textBoxLINIdLast, "");
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

		private void LINIdCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void RenderLabelsForDisplayMode()
		{
			string arg = Resources.DisplayModeDec;
			if (GUIUtil.IsHexadecimal)
			{
				arg = Resources.DisplayModeHex;
			}
			this.labelID.Text = string.Format(Resources.IDLabelWithMode, arg);
			this.labelLastId.Text = string.Format(Resources.LastIDLabelWithMode, arg);
		}

		private void SubscribeControlEvents(bool doSubscribe)
		{
			if (doSubscribe)
			{
				this.comboBoxConditionType.SelectedIndexChanged += new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
				this.textBoxLINId.Validating += new CancelEventHandler(this.textBoxLINId_Validating);
				this.textBoxLINId.Validated += new EventHandler(this.textBoxLINId_Validated);
				this.textBoxLINIdLast.Validating += new CancelEventHandler(this.textBoxLINIdLast_Validating);
				this.textBoxLINIdLast.Validated += new EventHandler(this.textBoxLINIdLast_Validated);
				return;
			}
			this.comboBoxConditionType.SelectedIndexChanged -= new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
			this.textBoxLINId.Validating -= new CancelEventHandler(this.textBoxLINId_Validating);
			this.textBoxLINId.Validated -= new EventHandler(this.textBoxLINId_Validated);
			this.textBoxLINIdLast.Validating -= new CancelEventHandler(this.textBoxLINIdLast_Validating);
			this.textBoxLINIdLast.Validated -= new EventHandler(this.textBoxLINIdLast_Validated);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LINIdCondition));
			this.labelConditionType = new Label();
			this.comboBoxConditionType = new ComboBox();
			this.labelID = new Label();
			this.labelLastId = new Label();
			this.textBoxLINId = new TextBox();
			this.textBoxLINIdLast = new TextBox();
			this.buttonOk = new Button();
			this.buttonCancel = new Button();
			this.errorProvider = new ErrorProvider(this.components);
			this.buttonHelp = new Button();
			((ISupportInitialize)this.errorProvider).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelConditionType, "labelConditionType");
			this.errorProvider.SetError(this.labelConditionType, componentResourceManager.GetString("labelConditionType.Error"));
			this.errorProvider.SetIconAlignment(this.labelConditionType, (ErrorIconAlignment)componentResourceManager.GetObject("labelConditionType.IconAlignment"));
			this.errorProvider.SetIconPadding(this.labelConditionType, (int)componentResourceManager.GetObject("labelConditionType.IconPadding"));
			this.labelConditionType.Name = "labelConditionType";
			componentResourceManager.ApplyResources(this.comboBoxConditionType, "comboBoxConditionType");
			this.comboBoxConditionType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProvider.SetError(this.comboBoxConditionType, componentResourceManager.GetString("comboBoxConditionType.Error"));
			this.comboBoxConditionType.FormattingEnabled = true;
			this.errorProvider.SetIconAlignment(this.comboBoxConditionType, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxConditionType.IconAlignment"));
			this.errorProvider.SetIconPadding(this.comboBoxConditionType, (int)componentResourceManager.GetObject("comboBoxConditionType.IconPadding"));
			this.comboBoxConditionType.Name = "comboBoxConditionType";
			this.comboBoxConditionType.SelectedIndexChanged += new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelID, "labelID");
			this.errorProvider.SetError(this.labelID, componentResourceManager.GetString("labelID.Error"));
			this.errorProvider.SetIconAlignment(this.labelID, (ErrorIconAlignment)componentResourceManager.GetObject("labelID.IconAlignment"));
			this.errorProvider.SetIconPadding(this.labelID, (int)componentResourceManager.GetObject("labelID.IconPadding"));
			this.labelID.Name = "labelID";
			componentResourceManager.ApplyResources(this.labelLastId, "labelLastId");
			this.errorProvider.SetError(this.labelLastId, componentResourceManager.GetString("labelLastId.Error"));
			this.errorProvider.SetIconAlignment(this.labelLastId, (ErrorIconAlignment)componentResourceManager.GetObject("labelLastId.IconAlignment"));
			this.errorProvider.SetIconPadding(this.labelLastId, (int)componentResourceManager.GetObject("labelLastId.IconPadding"));
			this.labelLastId.Name = "labelLastId";
			componentResourceManager.ApplyResources(this.textBoxLINId, "textBoxLINId");
			this.errorProvider.SetError(this.textBoxLINId, componentResourceManager.GetString("textBoxLINId.Error"));
			this.errorProvider.SetIconAlignment(this.textBoxLINId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLINId.IconAlignment"));
			this.errorProvider.SetIconPadding(this.textBoxLINId, (int)componentResourceManager.GetObject("textBoxLINId.IconPadding"));
			this.textBoxLINId.Name = "textBoxLINId";
			this.textBoxLINId.Validating += new CancelEventHandler(this.textBoxLINId_Validating);
			this.textBoxLINId.Validated += new EventHandler(this.textBoxLINId_Validated);
			componentResourceManager.ApplyResources(this.textBoxLINIdLast, "textBoxLINIdLast");
			this.errorProvider.SetError(this.textBoxLINIdLast, componentResourceManager.GetString("textBoxLINIdLast.Error"));
			this.errorProvider.SetIconAlignment(this.textBoxLINIdLast, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLINIdLast.IconAlignment"));
			this.errorProvider.SetIconPadding(this.textBoxLINIdLast, (int)componentResourceManager.GetObject("textBoxLINIdLast.IconPadding"));
			this.textBoxLINIdLast.Name = "textBoxLINIdLast";
			this.textBoxLINIdLast.Validating += new CancelEventHandler(this.textBoxLINIdLast_Validating);
			this.textBoxLINIdLast.Validated += new EventHandler(this.textBoxLINIdLast_Validated);
			componentResourceManager.ApplyResources(this.buttonOk, "buttonOk");
			this.buttonOk.DialogResult = DialogResult.OK;
			this.errorProvider.SetError(this.buttonOk, componentResourceManager.GetString("buttonOk.Error"));
			this.errorProvider.SetIconAlignment(this.buttonOk, (ErrorIconAlignment)componentResourceManager.GetObject("buttonOk.IconAlignment"));
			this.errorProvider.SetIconPadding(this.buttonOk, (int)componentResourceManager.GetObject("buttonOk.IconPadding"));
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new EventHandler(this.buttonOk_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.errorProvider.SetError(this.buttonCancel, componentResourceManager.GetString("buttonCancel.Error"));
			this.errorProvider.SetIconAlignment(this.buttonCancel, (ErrorIconAlignment)componentResourceManager.GetObject("buttonCancel.IconAlignment"));
			this.errorProvider.SetIconPadding(this.buttonCancel, (int)componentResourceManager.GetObject("buttonCancel.IconPadding"));
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProvider.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProvider, "errorProvider");
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.errorProvider.SetError(this.buttonHelp, componentResourceManager.GetString("buttonHelp.Error"));
			this.errorProvider.SetIconAlignment(this.buttonHelp, (ErrorIconAlignment)componentResourceManager.GetObject("buttonHelp.IconAlignment"));
			this.errorProvider.SetIconPadding(this.buttonHelp, (int)componentResourceManager.GetObject("buttonHelp.IconPadding"));
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
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOk);
			base.Controls.Add(this.textBoxLINIdLast);
			base.Controls.Add(this.textBoxLINId);
			base.Controls.Add(this.labelLastId);
			base.Controls.Add(this.labelID);
			base.Controls.Add(this.comboBoxConditionType);
			base.Controls.Add(this.labelConditionType);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "LINIdCondition";
			base.Shown += new EventHandler(this.LINIdCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.LINIdCondition_HelpRequested);
			((ISupportInitialize)this.errorProvider).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
