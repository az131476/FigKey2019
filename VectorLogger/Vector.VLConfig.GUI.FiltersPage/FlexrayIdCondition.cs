using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.FiltersPage
{
	public class FlexrayIdCondition : Form
	{
		private uint frameId;

		private uint lastFrameId;

		private bool isIdRange;

		private uint cycleTime;

		private uint cycleRepetition;

		private IContainer components;

		private Button buttonHelp;

		private Button buttonCancel;

		private Button buttonOK;

		private Label labelRelation;

		private ComboBox comboBoxConditionType;

		private TextBox textBoxId;

		private TextBox textBoxIdLast;

		private Label labelSlotId;

		private Label labelIdLast;

		private TextBox textBoxCycleTime;

		private Label labelCycleTime;

		private Label labelCycleRepetition;

		private ErrorProvider errorProvider;

		private ComboBox comboBoxCycleRepetition;

		public uint FrameId
		{
			get
			{
				return this.frameId;
			}
			set
			{
				this.frameId = value;
			}
		}

		public uint LastFrameId
		{
			get
			{
				return this.lastFrameId;
			}
			set
			{
				this.lastFrameId = value;
			}
		}

		public bool IsIdRange
		{
			get
			{
				return this.isIdRange;
			}
			set
			{
				this.isIdRange = value;
			}
		}

		public uint CycleTime
		{
			get
			{
				return this.cycleTime;
			}
			set
			{
				this.cycleTime = value;
			}
		}

		public uint CycleRepetiton
		{
			get
			{
				return this.cycleRepetition;
			}
			set
			{
				this.cycleRepetition = value;
			}
		}

		public FlexrayIdCondition()
		{
			this.InitializeComponent();
			this.frameId = Constants.MinimumFlexraySlotId;
			this.lastFrameId = Constants.MinimumFlexraySlotId;
			this.cycleTime = 0u;
			this.cycleRepetition = 1u;
		}

		private void InitControls()
		{
			this.ResetErrorProvider();
			this.SubscribeControlEvents(false);
			this.comboBoxConditionType.Items.Clear();
			this.comboBoxConditionType.Items.Add(Resources.FilterConditionRelationSlotValue);
			this.comboBoxConditionType.Items.Add(Resources.FilterConditionRelationSlotRange);
			this.comboBoxConditionType.SelectedIndex = (this.isIdRange ? 1 : 0);
			this.textBoxId.Text = GUIUtil.FlexraySlotIdToDisplayString(this.frameId);
			this.textBoxIdLast.Text = GUIUtil.FlexraySlotIdToDisplayString(this.lastFrameId);
			this.textBoxCycleTime.Text = this.cycleTime.ToString();
			this.InitCycleRepetitionComboBox();
			if (this.comboBoxCycleRepetition.Items.Contains(this.cycleRepetition.ToString()))
			{
				this.comboBoxCycleRepetition.SelectedItem = this.cycleRepetition.ToString();
			}
			this.EnableControlsAndSetValuesForRelation();
			this.SubscribeControlEvents(true);
		}

		private void InitCycleRepetitionComboBox()
		{
			IList<uint> flexrayCycleRepetitionValues = GUIUtil.GetFlexrayCycleRepetitionValues(false);
			string text = "";
			if (this.comboBoxCycleRepetition.SelectedItem != null)
			{
				text = this.comboBoxCycleRepetition.SelectedItem.ToString();
			}
			this.comboBoxCycleRepetition.Items.Clear();
			foreach (uint current in flexrayCycleRepetitionValues)
			{
				this.comboBoxCycleRepetition.Items.Add(current.ToString());
			}
			if (!string.IsNullOrEmpty(text) && this.comboBoxCycleRepetition.Items.Contains(text))
			{
				this.comboBoxCycleRepetition.SelectedItem = text;
				return;
			}
			this.comboBoxCycleRepetition.SelectedIndex = 0;
		}

		private void FlexrayIdCondition_Shown(object sender, EventArgs e)
		{
			this.InitControls();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (!this.ValidateChildren())
			{
				InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
				base.DialogResult = DialogResult.None;
				return;
			}
			if (this.isIdRange && this.frameId > this.lastFrameId)
			{
				uint num = this.lastFrameId;
				this.lastFrameId = this.frameId;
				this.frameId = num;
			}
			base.DialogResult = DialogResult.OK;
		}

		private void comboBoxConditionType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.isIdRange = (this.comboBoxConditionType.SelectedItem.ToString() != Resources.FilterConditionRelationSlotValue);
			this.EnableControlsAndSetValuesForRelation();
		}

		private void comboBoxCycleRepetition_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.cycleRepetition = uint.Parse(this.comboBoxCycleRepetition.SelectedItem.ToString());
		}

		private void textBoxId_Validating(object sender, CancelEventArgs e)
		{
			uint num;
			if (GUIUtil.DisplayStringToFlexraySlotId(this.textBoxId.Text, out num))
			{
				this.frameId = num;
				return;
			}
			this.errorProvider.SetError(this.textBoxId, "");
			this.errorProvider.SetError(this.textBoxId, string.Format(Resources.ErrorFlexraySlotExpected, Constants.MinimumFlexraySlotId, Constants.MaximumFlexraySlotId));
			e.Cancel = true;
		}

		private void textBoxId_Validated(object sender, EventArgs e)
		{
			this.errorProvider.SetError(this.textBoxId, "");
		}

		private void textBoxIdLast_Validating(object sender, CancelEventArgs e)
		{
			uint num;
			if (GUIUtil.DisplayStringToFlexraySlotId(this.textBoxIdLast.Text, out num))
			{
				this.lastFrameId = num;
				return;
			}
			this.errorProvider.SetError(this.textBoxIdLast, "");
			this.errorProvider.SetError(this.textBoxIdLast, string.Format(Resources.ErrorFlexraySlotExpected, Constants.MinimumFlexraySlotId, Constants.MaximumFlexraySlotId));
			e.Cancel = true;
		}

		private void textBoxIdLast_Validated(object sender, EventArgs e)
		{
			this.errorProvider.SetError(this.textBoxIdLast, "");
		}

		private void textBoxCycleTime_Validating(object sender, CancelEventArgs e)
		{
			uint num;
			if (!uint.TryParse(this.textBoxCycleTime.Text, out num))
			{
				this.errorProvider.SetError(this.textBoxCycleTime, "");
				this.errorProvider.SetError(this.textBoxCycleTime, Resources.ErrorNumberExpected);
				e.Cancel = true;
				return;
			}
			if (num > Constants.MaximumFlexrayBaseCycle)
			{
				this.errorProvider.SetError(this.textBoxCycleTime, "");
				this.errorProvider.SetError(this.textBoxCycleTime, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, 0, Constants.MaximumFlexrayBaseCycle));
				e.Cancel = true;
				return;
			}
			this.cycleTime = num;
		}

		private void textBoxCycleTime_Validated(object sender, EventArgs e)
		{
			this.errorProvider.SetError(this.textBoxCycleTime, "");
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void FlexrayIdCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void EnableControlsAndSetValuesForRelation()
		{
			if (!this.isIdRange)
			{
				this.textBoxIdLast.Enabled = false;
				this.lastFrameId = Constants.MinimumFlexraySlotId;
				this.textBoxIdLast.Text = GUIUtil.FlexraySlotIdToDisplayString(this.lastFrameId);
				this.errorProvider.SetError(this.textBoxIdLast, "");
				this.textBoxCycleTime.Enabled = true;
				this.comboBoxCycleRepetition.Enabled = true;
				return;
			}
			this.textBoxIdLast.Enabled = true;
			this.textBoxCycleTime.Text = "0";
			this.textBoxCycleTime.Enabled = false;
			this.errorProvider.SetError(this.textBoxCycleTime, "");
			this.comboBoxCycleRepetition.SelectedItem = "1";
			this.comboBoxCycleRepetition.Enabled = false;
		}

		private void ResetErrorProvider()
		{
			this.errorProvider.SetError(this.textBoxId, "");
			this.errorProvider.SetError(this.textBoxIdLast, "");
			this.errorProvider.SetError(this.textBoxCycleTime, "");
		}

		private void SubscribeControlEvents(bool doSubscribe)
		{
			if (doSubscribe)
			{
				this.comboBoxConditionType.SelectedIndexChanged += new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
				this.comboBoxCycleRepetition.SelectedIndexChanged += new EventHandler(this.comboBoxCycleRepetition_SelectedIndexChanged);
				this.textBoxId.Validating += new CancelEventHandler(this.textBoxId_Validating);
				this.textBoxId.Validated += new EventHandler(this.textBoxId_Validated);
				this.textBoxIdLast.Validating += new CancelEventHandler(this.textBoxIdLast_Validating);
				this.textBoxIdLast.Validated += new EventHandler(this.textBoxIdLast_Validated);
				this.textBoxCycleTime.Validating += new CancelEventHandler(this.textBoxCycleTime_Validating);
				this.textBoxCycleTime.Validated += new EventHandler(this.textBoxCycleTime_Validated);
				return;
			}
			this.comboBoxConditionType.SelectedIndexChanged -= new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
			this.comboBoxCycleRepetition.SelectedIndexChanged -= new EventHandler(this.comboBoxCycleRepetition_SelectedIndexChanged);
			this.textBoxId.Validating -= new CancelEventHandler(this.textBoxId_Validating);
			this.textBoxId.Validated -= new EventHandler(this.textBoxId_Validated);
			this.textBoxIdLast.Validating -= new CancelEventHandler(this.textBoxIdLast_Validating);
			this.textBoxIdLast.Validated -= new EventHandler(this.textBoxIdLast_Validated);
			this.textBoxCycleTime.Validating -= new CancelEventHandler(this.textBoxCycleTime_Validating);
			this.textBoxCycleTime.Validated -= new EventHandler(this.textBoxCycleTime_Validated);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FlexrayIdCondition));
			this.buttonHelp = new Button();
			this.buttonCancel = new Button();
			this.buttonOK = new Button();
			this.labelRelation = new Label();
			this.comboBoxConditionType = new ComboBox();
			this.textBoxId = new TextBox();
			this.textBoxIdLast = new TextBox();
			this.labelSlotId = new Label();
			this.labelIdLast = new Label();
			this.textBoxCycleTime = new TextBox();
			this.labelCycleTime = new Label();
			this.labelCycleRepetition = new Label();
			this.errorProvider = new ErrorProvider(this.components);
			this.comboBoxCycleRepetition = new ComboBox();
			((ISupportInitialize)this.errorProvider).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.errorProvider.SetError(this.buttonHelp, componentResourceManager.GetString("buttonHelp.Error"));
			this.errorProvider.SetIconAlignment(this.buttonHelp, (ErrorIconAlignment)componentResourceManager.GetObject("buttonHelp.IconAlignment"));
			this.errorProvider.SetIconPadding(this.buttonHelp, (int)componentResourceManager.GetObject("buttonHelp.IconPadding"));
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.errorProvider.SetError(this.buttonCancel, componentResourceManager.GetString("buttonCancel.Error"));
			this.errorProvider.SetIconAlignment(this.buttonCancel, (ErrorIconAlignment)componentResourceManager.GetObject("buttonCancel.IconAlignment"));
			this.errorProvider.SetIconPadding(this.buttonCancel, (int)componentResourceManager.GetObject("buttonCancel.IconPadding"));
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.errorProvider.SetError(this.buttonOK, componentResourceManager.GetString("buttonOK.Error"));
			this.errorProvider.SetIconAlignment(this.buttonOK, (ErrorIconAlignment)componentResourceManager.GetObject("buttonOK.IconAlignment"));
			this.errorProvider.SetIconPadding(this.buttonOK, (int)componentResourceManager.GetObject("buttonOK.IconPadding"));
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			componentResourceManager.ApplyResources(this.labelRelation, "labelRelation");
			this.errorProvider.SetError(this.labelRelation, componentResourceManager.GetString("labelRelation.Error"));
			this.errorProvider.SetIconAlignment(this.labelRelation, (ErrorIconAlignment)componentResourceManager.GetObject("labelRelation.IconAlignment"));
			this.errorProvider.SetIconPadding(this.labelRelation, (int)componentResourceManager.GetObject("labelRelation.IconPadding"));
			this.labelRelation.Name = "labelRelation";
			componentResourceManager.ApplyResources(this.comboBoxConditionType, "comboBoxConditionType");
			this.comboBoxConditionType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProvider.SetError(this.comboBoxConditionType, componentResourceManager.GetString("comboBoxConditionType.Error"));
			this.comboBoxConditionType.FormattingEnabled = true;
			this.errorProvider.SetIconAlignment(this.comboBoxConditionType, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxConditionType.IconAlignment"));
			this.errorProvider.SetIconPadding(this.comboBoxConditionType, (int)componentResourceManager.GetObject("comboBoxConditionType.IconPadding"));
			this.comboBoxConditionType.Name = "comboBoxConditionType";
			this.comboBoxConditionType.SelectedIndexChanged += new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.textBoxId, "textBoxId");
			this.errorProvider.SetError(this.textBoxId, componentResourceManager.GetString("textBoxId.Error"));
			this.errorProvider.SetIconAlignment(this.textBoxId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxId.IconAlignment"));
			this.errorProvider.SetIconPadding(this.textBoxId, (int)componentResourceManager.GetObject("textBoxId.IconPadding"));
			this.textBoxId.Name = "textBoxId";
			this.textBoxId.Validating += new CancelEventHandler(this.textBoxId_Validating);
			this.textBoxId.Validated += new EventHandler(this.textBoxId_Validated);
			componentResourceManager.ApplyResources(this.textBoxIdLast, "textBoxIdLast");
			this.errorProvider.SetError(this.textBoxIdLast, componentResourceManager.GetString("textBoxIdLast.Error"));
			this.errorProvider.SetIconAlignment(this.textBoxIdLast, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxIdLast.IconAlignment"));
			this.errorProvider.SetIconPadding(this.textBoxIdLast, (int)componentResourceManager.GetObject("textBoxIdLast.IconPadding"));
			this.textBoxIdLast.Name = "textBoxIdLast";
			this.textBoxIdLast.Validating += new CancelEventHandler(this.textBoxIdLast_Validating);
			this.textBoxIdLast.Validated += new EventHandler(this.textBoxIdLast_Validated);
			componentResourceManager.ApplyResources(this.labelSlotId, "labelSlotId");
			this.errorProvider.SetError(this.labelSlotId, componentResourceManager.GetString("labelSlotId.Error"));
			this.errorProvider.SetIconAlignment(this.labelSlotId, (ErrorIconAlignment)componentResourceManager.GetObject("labelSlotId.IconAlignment"));
			this.errorProvider.SetIconPadding(this.labelSlotId, (int)componentResourceManager.GetObject("labelSlotId.IconPadding"));
			this.labelSlotId.Name = "labelSlotId";
			componentResourceManager.ApplyResources(this.labelIdLast, "labelIdLast");
			this.errorProvider.SetError(this.labelIdLast, componentResourceManager.GetString("labelIdLast.Error"));
			this.errorProvider.SetIconAlignment(this.labelIdLast, (ErrorIconAlignment)componentResourceManager.GetObject("labelIdLast.IconAlignment"));
			this.errorProvider.SetIconPadding(this.labelIdLast, (int)componentResourceManager.GetObject("labelIdLast.IconPadding"));
			this.labelIdLast.Name = "labelIdLast";
			componentResourceManager.ApplyResources(this.textBoxCycleTime, "textBoxCycleTime");
			this.errorProvider.SetError(this.textBoxCycleTime, componentResourceManager.GetString("textBoxCycleTime.Error"));
			this.errorProvider.SetIconAlignment(this.textBoxCycleTime, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCycleTime.IconAlignment"));
			this.errorProvider.SetIconPadding(this.textBoxCycleTime, (int)componentResourceManager.GetObject("textBoxCycleTime.IconPadding"));
			this.textBoxCycleTime.Name = "textBoxCycleTime";
			this.textBoxCycleTime.Validating += new CancelEventHandler(this.textBoxCycleTime_Validating);
			this.textBoxCycleTime.Validated += new EventHandler(this.textBoxCycleTime_Validated);
			componentResourceManager.ApplyResources(this.labelCycleTime, "labelCycleTime");
			this.errorProvider.SetError(this.labelCycleTime, componentResourceManager.GetString("labelCycleTime.Error"));
			this.errorProvider.SetIconAlignment(this.labelCycleTime, (ErrorIconAlignment)componentResourceManager.GetObject("labelCycleTime.IconAlignment"));
			this.errorProvider.SetIconPadding(this.labelCycleTime, (int)componentResourceManager.GetObject("labelCycleTime.IconPadding"));
			this.labelCycleTime.Name = "labelCycleTime";
			componentResourceManager.ApplyResources(this.labelCycleRepetition, "labelCycleRepetition");
			this.errorProvider.SetError(this.labelCycleRepetition, componentResourceManager.GetString("labelCycleRepetition.Error"));
			this.errorProvider.SetIconAlignment(this.labelCycleRepetition, (ErrorIconAlignment)componentResourceManager.GetObject("labelCycleRepetition.IconAlignment"));
			this.errorProvider.SetIconPadding(this.labelCycleRepetition, (int)componentResourceManager.GetObject("labelCycleRepetition.IconPadding"));
			this.labelCycleRepetition.Name = "labelCycleRepetition";
			this.errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProvider.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProvider, "errorProvider");
			componentResourceManager.ApplyResources(this.comboBoxCycleRepetition, "comboBoxCycleRepetition");
			this.comboBoxCycleRepetition.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProvider.SetError(this.comboBoxCycleRepetition, componentResourceManager.GetString("comboBoxCycleRepetition.Error"));
			this.comboBoxCycleRepetition.FormattingEnabled = true;
			this.errorProvider.SetIconAlignment(this.comboBoxCycleRepetition, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxCycleRepetition.IconAlignment"));
			this.errorProvider.SetIconPadding(this.comboBoxCycleRepetition, (int)componentResourceManager.GetObject("comboBoxCycleRepetition.IconPadding"));
			this.comboBoxCycleRepetition.Name = "comboBoxCycleRepetition";
			this.comboBoxCycleRepetition.SelectedIndexChanged += new EventHandler(this.comboBoxCycleRepetition_SelectedIndexChanged);
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoValidate = AutoValidate.EnableAllowFocusChange;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.comboBoxCycleRepetition);
			base.Controls.Add(this.labelCycleRepetition);
			base.Controls.Add(this.labelCycleTime);
			base.Controls.Add(this.textBoxCycleTime);
			base.Controls.Add(this.labelIdLast);
			base.Controls.Add(this.labelSlotId);
			base.Controls.Add(this.textBoxIdLast);
			base.Controls.Add(this.textBoxId);
			base.Controls.Add(this.comboBoxConditionType);
			base.Controls.Add(this.labelRelation);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonHelp);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FlexrayIdCondition";
			base.Shown += new EventHandler(this.FlexrayIdCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.FlexrayIdCondition_HelpRequested);
			((ISupportInitialize)this.errorProvider).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
