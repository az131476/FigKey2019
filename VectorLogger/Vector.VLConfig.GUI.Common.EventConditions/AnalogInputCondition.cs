using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.EventConditions
{
	public class AnalogInputCondition : Form
	{
		private IModelValidator modelValidator;

		private AnalogInputEvent analogInputEvent;

		private IContainer components;

		private Label labelInput;

		private ComboBox comboBoxInputNumber;

		private Label labelRelOp;

		private ComboBox comboBoxRelOperator;

		private TextBox textBoxLowValue;

		private Label labelLowValue;

		private TextBox textBoxHighValue;

		private Label labelHighValue;

		private Label labelMilliVolt1;

		private Label labelMilliVolt2;

		private Label labelTolerance;

		private TextBox textBoxTolerance;

		private Label labelMilliVolt3;

		private Button buttonOK;

		private Button buttonCancel;

		private Button buttonHelp;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderGlobalModel;

		private Label labelToleranceInfo;

		public AnalogInputEvent AnalogInputEvent
		{
			get
			{
				return this.analogInputEvent;
			}
			set
			{
				this.analogInputEvent = value;
			}
		}

		public AnalogInputCondition(IModelValidator modelVal)
		{
			this.InitializeComponent();
			this.modelValidator = modelVal;
			this.analogInputEvent = new AnalogInputEvent();
			this.InitRelOperatorComboBox();
			this.InitAnalogInputComboBox();
			this.ResetToDefaults();
		}

		private void InitAnalogInputComboBox()
		{
			this.comboBoxInputNumber.SelectedIndexChanged -= new EventHandler(this.comboBoxInputNumber_SelectedIndexChanged);
			this.comboBoxInputNumber.Items.Clear();
			for (uint num = 1u; num <= this.modelValidator.LoggerSpecifics.IO.NumberOfAnalogInputs; num += 1u)
			{
				this.comboBoxInputNumber.Items.Add(string.Format(Vocabulary.AnalogInputName, num));
			}
			this.comboBoxInputNumber.SelectedIndex = 0;
			this.comboBoxInputNumber.SelectedIndexChanged += new EventHandler(this.comboBoxInputNumber_SelectedIndexChanged);
		}

		private void InitRelOperatorComboBox()
		{
			this.comboBoxRelOperator.SelectedIndexChanged -= new EventHandler(this.comboBoxRelOperator_SelectedIndexChanged);
			foreach (CondRelation condRelation in Enum.GetValues(typeof(CondRelation)))
			{
				if (condRelation != CondRelation.Equal && condRelation != CondRelation.NotEqual && condRelation != CondRelation.LessThan && condRelation != CondRelation.GreaterThan && condRelation != CondRelation.OnChange)
				{
					this.comboBoxRelOperator.Items.Add(GUIUtil.MapTriggerConditionRelation2String(condRelation));
				}
			}
			this.comboBoxRelOperator.SelectedIndex = 0;
			this.comboBoxRelOperator.SelectedIndexChanged += new EventHandler(this.comboBoxRelOperator_SelectedIndexChanged);
		}

		private void ApplyValueToControls()
		{
			this.SubscribeControlEvents(false);
			if ((ulong)this.analogInputEvent.InputNumber.Value <= (ulong)((long)this.comboBoxInputNumber.Items.Count))
			{
				this.comboBoxInputNumber.SelectedIndex = (int)(this.analogInputEvent.InputNumber.Value - 1u);
			}
			this.textBoxLowValue.Text = this.analogInputEvent.LowValue.Value.ToString();
			this.comboBoxRelOperator.SelectedItem = GUIUtil.MapTriggerConditionRelation2String(this.analogInputEvent.Relation.Value);
			this.textBoxHighValue.Enabled = (this.analogInputEvent.Relation.Value == CondRelation.InRange || this.analogInputEvent.Relation.Value == CondRelation.NotInRange);
			this.textBoxHighValue.Text = this.analogInputEvent.HighValue.Value.ToString();
			this.textBoxTolerance.Text = this.analogInputEvent.Tolerance.Value.ToString();
			this.SubscribeControlEvents(true);
			this.ValidateInput();
		}

		public void ResetToDefaults()
		{
			this.analogInputEvent.InputNumber.Value = 1u;
			this.analogInputEvent.Relation.Value = CondRelation.GreaterThanOrEqual;
			this.analogInputEvent.LowValue.Value = Constants.DefaultAnalogInputThreshold_mV;
			this.analogInputEvent.HighValue.Value = this.modelValidator.LoggerSpecifics.IO.MaximumAnalogInputVoltage_mV;
			this.analogInputEvent.Tolerance.Value = Constants.DefaultAnalogInputHysTolerance_mV;
		}

		private void AnalogInputCondition_Shown(object sender, EventArgs e)
		{
			this.InitAnalogInputComboBox();
			this.ApplyValueToControls();
		}

		private void comboBoxInputNumber_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.ValidateInput();
		}

		private void comboBoxRelOperator_SelectedIndexChanged(object sender, EventArgs e)
		{
			CondRelation condRelation = GUIUtil.MapString2TriggerConditionRelation(this.comboBoxRelOperator.SelectedItem.ToString());
			if (CondRelation.InRange == condRelation || CondRelation.NotInRange == condRelation)
			{
				this.textBoxHighValue.Enabled = true;
			}
			else
			{
				this.textBoxHighValue.Enabled = false;
				this.analogInputEvent.HighValue.Value = this.modelValidator.LoggerSpecifics.IO.MaximumAnalogInputVoltage_mV;
				this.textBoxHighValue.Text = this.analogInputEvent.HighValue.Value.ToString();
			}
			this.ValidateInput();
		}

		private void textBox_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateInput();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (this.ValidateInput())
			{
				if (this.analogInputEvent.Relation.Value == CondRelation.InRange && this.analogInputEvent.Relation.Value == CondRelation.NotInRange && this.analogInputEvent.LowValue.Value > this.analogInputEvent.HighValue.Value)
				{
					uint value = this.analogInputEvent.LowValue.Value;
					this.analogInputEvent.LowValue.Value = this.analogInputEvent.HighValue.Value;
					this.analogInputEvent.HighValue.Value = value;
				}
				base.DialogResult = DialogResult.OK;
				return;
			}
			InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
			base.DialogResult = DialogResult.None;
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void AnalogInputCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private bool ValidateInput()
		{
			bool flag = true;
			this.ResetErrorProvider();
			uint value = (uint)(this.comboBoxInputNumber.SelectedIndex + 1);
			CondRelation condRelation = GUIUtil.MapString2TriggerConditionRelation(this.comboBoxRelOperator.SelectedItem.ToString());
			uint num;
			if (!uint.TryParse(this.textBoxLowValue.Text, out num))
			{
				this.errorProviderFormat.SetError(this.textBoxLowValue, Resources.ErrorNumberExpected);
				flag = false;
			}
			else if (num > this.modelValidator.LoggerSpecifics.IO.MaximumAnalogInputVoltage_mV)
			{
				this.errorProviderFormat.SetError(this.textBoxLowValue, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, 0, this.modelValidator.LoggerSpecifics.IO.MaximumAnalogInputVoltage_mV));
				flag = false;
			}
			else if ((condRelation == CondRelation.GreaterThan || condRelation == CondRelation.GreaterThanOrEqual) && (ulong)num < (ulong)((long)Constants.MinimumAnalogInputIntervalToMinMax_mV))
			{
				this.errorProviderFormat.SetError(this.textBoxLowValue, string.Format(Resources.ErrorInvalidValueWithRelOpGreater, Constants.MinimumAnalogInputIntervalToMinMax_mV));
				flag = false;
			}
			else if ((condRelation == CondRelation.LessThan || condRelation == CondRelation.LessThanOrEqual) && (ulong)num > (ulong)this.modelValidator.LoggerSpecifics.IO.MaximumAnalogInputVoltage_mV - (ulong)((long)Constants.MinimumAnalogInputIntervalToMinMax_mV))
			{
				this.errorProviderFormat.SetError(this.textBoxLowValue, string.Format(Resources.ErrorInvalidValueWithRelOpLesser, (long)((ulong)this.modelValidator.LoggerSpecifics.IO.MaximumAnalogInputVoltage_mV - (ulong)((long)Constants.MinimumAnalogInputIntervalToMinMax_mV))));
				flag = false;
			}
			uint num2 = 0u;
			if (condRelation == CondRelation.InRange || condRelation == CondRelation.NotInRange)
			{
				if (!uint.TryParse(this.textBoxHighValue.Text, out num2))
				{
					this.errorProviderFormat.SetError(this.textBoxHighValue, Resources.ErrorNumberExpected);
					flag = false;
				}
				else if (num2 > this.modelValidator.LoggerSpecifics.IO.MaximumAnalogInputVoltage_mV)
				{
					this.errorProviderFormat.SetError(this.textBoxHighValue, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, 0, this.modelValidator.LoggerSpecifics.IO.MaximumAnalogInputVoltage_mV));
					flag = false;
				}
				if (flag)
				{
					if (condRelation == CondRelation.InRange && (ulong)num2 > (ulong)this.modelValidator.LoggerSpecifics.IO.MaximumAnalogInputVoltage_mV - (ulong)((long)Constants.MinimumAnalogInputIntervalToMinMax_mV) && (ulong)num < (ulong)((long)Constants.MinimumAnalogInputIntervalToMinMax_mV))
					{
						this.errorProviderFormat.SetError(this.textBoxLowValue, Resources.ErrorRangeMustbeNarrowed);
						this.errorProviderFormat.SetError(this.textBoxHighValue, Resources.ErrorRangeMustbeNarrowed);
						flag = false;
					}
					else if (num2 <= num)
					{
						this.errorProviderFormat.SetError(this.textBoxLowValue, Resources.ErrorValueNotGreaterThanUpper);
						this.errorProviderFormat.SetError(this.textBoxHighValue, Resources.ErrorValueNotGreaterThanUpper);
						flag = false;
					}
				}
			}
			uint num3;
			if (!uint.TryParse(this.textBoxTolerance.Text, out num3))
			{
				this.errorProviderFormat.SetError(this.textBoxTolerance, Resources.ErrorNumberExpected);
				flag = false;
			}
			else
			{
				int num4 = (int)Constants.MaximumAnalogInputHysTolerance_mV;
				if (flag)
				{
					switch (condRelation)
					{
					case CondRelation.LessThan:
					case CondRelation.LessThanOrEqual:
						num4 = (int)(this.modelValidator.LoggerSpecifics.IO.MaximumAnalogInputVoltage_mV - num - (uint)Constants.MinimumAnalogInputIntervalToMinMax_mV);
						break;
					case CondRelation.GreaterThan:
					case CondRelation.GreaterThanOrEqual:
						num4 = (int)(num - (uint)Constants.MinimumAnalogInputIntervalToMinMax_mV);
						break;
					case CondRelation.InRange:
						num4 = Math.Max((int)num, (int)(this.modelValidator.LoggerSpecifics.IO.MaximumAnalogInputVoltage_mV - num2)) - Constants.MinimumAnalogInputIntervalToMinMax_mV;
						break;
					case CondRelation.NotInRange:
					{
						int num5 = (int)((num2 - num) / 2u);
						num4 = num5;
						break;
					}
					}
				}
				if (num4 > (int)Constants.MaximumAnalogInputHysTolerance_mV)
				{
					num4 = (int)Constants.MaximumAnalogInputHysTolerance_mV;
				}
				if (num3 > (uint)num4)
				{
					if (num4 <= 0)
					{
						this.errorProviderFormat.SetError(this.textBoxTolerance, Resources.ErrorToleranceMustBeZero);
					}
					else
					{
						this.errorProviderFormat.SetError(this.textBoxTolerance, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, 0, num4));
					}
					flag = false;
				}
			}
			this.labelToleranceInfo.Text = "";
			if (flag)
			{
				this.analogInputEvent.InputNumber.Value = value;
				this.analogInputEvent.LowValue.Value = num;
				this.analogInputEvent.HighValue.Value = num2;
				this.analogInputEvent.Relation.Value = condRelation;
				this.analogInputEvent.Tolerance.Value = num3;
				switch (this.analogInputEvent.Relation.Value)
				{
				case CondRelation.LessThan:
				case CondRelation.LessThanOrEqual:
					this.labelToleranceInfo.Text = string.Format(Resources.CondResetVoltAbove, this.analogInputEvent.LowValue.Value + this.analogInputEvent.Tolerance.Value);
					break;
				case CondRelation.GreaterThan:
				case CondRelation.GreaterThanOrEqual:
					this.labelToleranceInfo.Text = string.Format(Resources.ConditResetVoltBelow, this.analogInputEvent.LowValue.Value - this.analogInputEvent.Tolerance.Value);
					break;
				case CondRelation.InRange:
					if (this.analogInputEvent.HighValue.Value + this.analogInputEvent.Tolerance.Value >= this.modelValidator.LoggerSpecifics.IO.MaximumAnalogInputVoltage_mV)
					{
						this.labelToleranceInfo.Text = string.Format(Resources.ConditResetVoltBelow, this.analogInputEvent.LowValue.Value - this.analogInputEvent.Tolerance.Value);
					}
					else if (this.analogInputEvent.LowValue.Value - this.analogInputEvent.Tolerance.Value <= 0u)
					{
						this.labelToleranceInfo.Text = string.Format(Resources.CondResetVoltAbove, this.analogInputEvent.HighValue.Value + this.analogInputEvent.Tolerance.Value);
					}
					else
					{
						this.labelToleranceInfo.Text = string.Format(Resources.CondResetVoltBelowAbove, this.analogInputEvent.LowValue.Value - this.analogInputEvent.Tolerance.Value, this.analogInputEvent.HighValue.Value + this.analogInputEvent.Tolerance.Value);
					}
					break;
				case CondRelation.NotInRange:
					this.labelToleranceInfo.Text = string.Format(Resources.CondResetVoltAboveBelow, this.analogInputEvent.LowValue.Value + this.analogInputEvent.Tolerance.Value, this.analogInputEvent.HighValue.Value - this.analogInputEvent.Tolerance.Value);
					break;
				}
			}
			return flag;
		}

		private void ResetErrorProvider()
		{
			this.errorProviderFormat.SetError(this.textBoxLowValue, "");
			this.errorProviderFormat.SetError(this.textBoxHighValue, "");
			this.errorProviderFormat.SetError(this.textBoxTolerance, "");
			this.errorProviderGlobalModel.SetError(this.comboBoxInputNumber, "");
		}

		private void SubscribeControlEvents(bool doSubscribe)
		{
			if (doSubscribe)
			{
				this.comboBoxInputNumber.SelectedIndexChanged += new EventHandler(this.comboBoxInputNumber_SelectedIndexChanged);
				this.comboBoxRelOperator.SelectedIndexChanged += new EventHandler(this.comboBoxRelOperator_SelectedIndexChanged);
				this.textBoxLowValue.Validating += new CancelEventHandler(this.textBox_Validating);
				this.textBoxHighValue.Validating += new CancelEventHandler(this.textBox_Validating);
				this.textBoxTolerance.Validating += new CancelEventHandler(this.textBox_Validating);
				return;
			}
			this.comboBoxInputNumber.SelectedIndexChanged -= new EventHandler(this.comboBoxInputNumber_SelectedIndexChanged);
			this.comboBoxRelOperator.SelectedIndexChanged -= new EventHandler(this.comboBoxRelOperator_SelectedIndexChanged);
			this.textBoxLowValue.Validating -= new CancelEventHandler(this.textBox_Validating);
			this.textBoxHighValue.Validating -= new CancelEventHandler(this.textBox_Validating);
			this.textBoxTolerance.Validating -= new CancelEventHandler(this.textBox_Validating);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AnalogInputCondition));
			this.labelInput = new Label();
			this.comboBoxInputNumber = new ComboBox();
			this.labelRelOp = new Label();
			this.comboBoxRelOperator = new ComboBox();
			this.textBoxLowValue = new TextBox();
			this.labelLowValue = new Label();
			this.textBoxHighValue = new TextBox();
			this.labelHighValue = new Label();
			this.labelMilliVolt1 = new Label();
			this.labelMilliVolt2 = new Label();
			this.labelTolerance = new Label();
			this.textBoxTolerance = new TextBox();
			this.labelMilliVolt3 = new Label();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.buttonHelp = new Button();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.labelToleranceInfo = new Label();
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelInput, "labelInput");
			this.errorProviderFormat.SetError(this.labelInput, componentResourceManager.GetString("labelInput.Error"));
			this.errorProviderGlobalModel.SetError(this.labelInput, componentResourceManager.GetString("labelInput.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelInput, (ErrorIconAlignment)componentResourceManager.GetObject("labelInput.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelInput, (ErrorIconAlignment)componentResourceManager.GetObject("labelInput.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelInput, (int)componentResourceManager.GetObject("labelInput.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelInput, (int)componentResourceManager.GetObject("labelInput.IconPadding1"));
			this.labelInput.Name = "labelInput";
			componentResourceManager.ApplyResources(this.comboBoxInputNumber, "comboBoxInputNumber");
			this.comboBoxInputNumber.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderGlobalModel.SetError(this.comboBoxInputNumber, componentResourceManager.GetString("comboBoxInputNumber.Error"));
			this.errorProviderFormat.SetError(this.comboBoxInputNumber, componentResourceManager.GetString("comboBoxInputNumber.Error1"));
			this.comboBoxInputNumber.FormattingEnabled = true;
			this.errorProviderFormat.SetIconAlignment(this.comboBoxInputNumber, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxInputNumber.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxInputNumber, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxInputNumber.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxInputNumber, (int)componentResourceManager.GetObject("comboBoxInputNumber.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.comboBoxInputNumber, (int)componentResourceManager.GetObject("comboBoxInputNumber.IconPadding1"));
			this.comboBoxInputNumber.Name = "comboBoxInputNumber";
			this.comboBoxInputNumber.SelectedIndexChanged += new EventHandler(this.comboBoxInputNumber_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelRelOp, "labelRelOp");
			this.errorProviderFormat.SetError(this.labelRelOp, componentResourceManager.GetString("labelRelOp.Error"));
			this.errorProviderGlobalModel.SetError(this.labelRelOp, componentResourceManager.GetString("labelRelOp.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelRelOp, (ErrorIconAlignment)componentResourceManager.GetObject("labelRelOp.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelRelOp, (ErrorIconAlignment)componentResourceManager.GetObject("labelRelOp.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelRelOp, (int)componentResourceManager.GetObject("labelRelOp.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelRelOp, (int)componentResourceManager.GetObject("labelRelOp.IconPadding1"));
			this.labelRelOp.Name = "labelRelOp";
			componentResourceManager.ApplyResources(this.comboBoxRelOperator, "comboBoxRelOperator");
			this.comboBoxRelOperator.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderGlobalModel.SetError(this.comboBoxRelOperator, componentResourceManager.GetString("comboBoxRelOperator.Error"));
			this.errorProviderFormat.SetError(this.comboBoxRelOperator, componentResourceManager.GetString("comboBoxRelOperator.Error1"));
			this.comboBoxRelOperator.FormattingEnabled = true;
			this.errorProviderFormat.SetIconAlignment(this.comboBoxRelOperator, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxRelOperator.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxRelOperator, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxRelOperator.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxRelOperator, (int)componentResourceManager.GetObject("comboBoxRelOperator.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.comboBoxRelOperator, (int)componentResourceManager.GetObject("comboBoxRelOperator.IconPadding1"));
			this.comboBoxRelOperator.Name = "comboBoxRelOperator";
			this.comboBoxRelOperator.SelectedIndexChanged += new EventHandler(this.comboBoxRelOperator_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.textBoxLowValue, "textBoxLowValue");
			this.errorProviderGlobalModel.SetError(this.textBoxLowValue, componentResourceManager.GetString("textBoxLowValue.Error"));
			this.errorProviderFormat.SetError(this.textBoxLowValue, componentResourceManager.GetString("textBoxLowValue.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxLowValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLowValue.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxLowValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLowValue.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxLowValue, (int)componentResourceManager.GetObject("textBoxLowValue.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxLowValue, (int)componentResourceManager.GetObject("textBoxLowValue.IconPadding1"));
			this.textBoxLowValue.Name = "textBoxLowValue";
			this.textBoxLowValue.Validating += new CancelEventHandler(this.textBox_Validating);
			componentResourceManager.ApplyResources(this.labelLowValue, "labelLowValue");
			this.errorProviderFormat.SetError(this.labelLowValue, componentResourceManager.GetString("labelLowValue.Error"));
			this.errorProviderGlobalModel.SetError(this.labelLowValue, componentResourceManager.GetString("labelLowValue.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelLowValue, (ErrorIconAlignment)componentResourceManager.GetObject("labelLowValue.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelLowValue, (ErrorIconAlignment)componentResourceManager.GetObject("labelLowValue.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelLowValue, (int)componentResourceManager.GetObject("labelLowValue.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelLowValue, (int)componentResourceManager.GetObject("labelLowValue.IconPadding1"));
			this.labelLowValue.Name = "labelLowValue";
			componentResourceManager.ApplyResources(this.textBoxHighValue, "textBoxHighValue");
			this.errorProviderGlobalModel.SetError(this.textBoxHighValue, componentResourceManager.GetString("textBoxHighValue.Error"));
			this.errorProviderFormat.SetError(this.textBoxHighValue, componentResourceManager.GetString("textBoxHighValue.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxHighValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxHighValue.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxHighValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxHighValue.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxHighValue, (int)componentResourceManager.GetObject("textBoxHighValue.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxHighValue, (int)componentResourceManager.GetObject("textBoxHighValue.IconPadding1"));
			this.textBoxHighValue.Name = "textBoxHighValue";
			componentResourceManager.ApplyResources(this.labelHighValue, "labelHighValue");
			this.errorProviderFormat.SetError(this.labelHighValue, componentResourceManager.GetString("labelHighValue.Error"));
			this.errorProviderGlobalModel.SetError(this.labelHighValue, componentResourceManager.GetString("labelHighValue.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelHighValue, (ErrorIconAlignment)componentResourceManager.GetObject("labelHighValue.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelHighValue, (ErrorIconAlignment)componentResourceManager.GetObject("labelHighValue.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelHighValue, (int)componentResourceManager.GetObject("labelHighValue.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelHighValue, (int)componentResourceManager.GetObject("labelHighValue.IconPadding1"));
			this.labelHighValue.Name = "labelHighValue";
			componentResourceManager.ApplyResources(this.labelMilliVolt1, "labelMilliVolt1");
			this.errorProviderFormat.SetError(this.labelMilliVolt1, componentResourceManager.GetString("labelMilliVolt1.Error"));
			this.errorProviderGlobalModel.SetError(this.labelMilliVolt1, componentResourceManager.GetString("labelMilliVolt1.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelMilliVolt1, (ErrorIconAlignment)componentResourceManager.GetObject("labelMilliVolt1.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelMilliVolt1, (ErrorIconAlignment)componentResourceManager.GetObject("labelMilliVolt1.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelMilliVolt1, (int)componentResourceManager.GetObject("labelMilliVolt1.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelMilliVolt1, (int)componentResourceManager.GetObject("labelMilliVolt1.IconPadding1"));
			this.labelMilliVolt1.Name = "labelMilliVolt1";
			componentResourceManager.ApplyResources(this.labelMilliVolt2, "labelMilliVolt2");
			this.errorProviderFormat.SetError(this.labelMilliVolt2, componentResourceManager.GetString("labelMilliVolt2.Error"));
			this.errorProviderGlobalModel.SetError(this.labelMilliVolt2, componentResourceManager.GetString("labelMilliVolt2.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelMilliVolt2, (ErrorIconAlignment)componentResourceManager.GetObject("labelMilliVolt2.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelMilliVolt2, (ErrorIconAlignment)componentResourceManager.GetObject("labelMilliVolt2.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelMilliVolt2, (int)componentResourceManager.GetObject("labelMilliVolt2.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelMilliVolt2, (int)componentResourceManager.GetObject("labelMilliVolt2.IconPadding1"));
			this.labelMilliVolt2.Name = "labelMilliVolt2";
			componentResourceManager.ApplyResources(this.labelTolerance, "labelTolerance");
			this.errorProviderFormat.SetError(this.labelTolerance, componentResourceManager.GetString("labelTolerance.Error"));
			this.errorProviderGlobalModel.SetError(this.labelTolerance, componentResourceManager.GetString("labelTolerance.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelTolerance, (ErrorIconAlignment)componentResourceManager.GetObject("labelTolerance.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelTolerance, (ErrorIconAlignment)componentResourceManager.GetObject("labelTolerance.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelTolerance, (int)componentResourceManager.GetObject("labelTolerance.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelTolerance, (int)componentResourceManager.GetObject("labelTolerance.IconPadding1"));
			this.labelTolerance.Name = "labelTolerance";
			componentResourceManager.ApplyResources(this.textBoxTolerance, "textBoxTolerance");
			this.errorProviderGlobalModel.SetError(this.textBoxTolerance, componentResourceManager.GetString("textBoxTolerance.Error"));
			this.errorProviderFormat.SetError(this.textBoxTolerance, componentResourceManager.GetString("textBoxTolerance.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxTolerance, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxTolerance.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxTolerance, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxTolerance.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxTolerance, (int)componentResourceManager.GetObject("textBoxTolerance.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxTolerance, (int)componentResourceManager.GetObject("textBoxTolerance.IconPadding1"));
			this.textBoxTolerance.Name = "textBoxTolerance";
			this.textBoxTolerance.Validating += new CancelEventHandler(this.textBox_Validating);
			componentResourceManager.ApplyResources(this.labelMilliVolt3, "labelMilliVolt3");
			this.errorProviderFormat.SetError(this.labelMilliVolt3, componentResourceManager.GetString("labelMilliVolt3.Error"));
			this.errorProviderGlobalModel.SetError(this.labelMilliVolt3, componentResourceManager.GetString("labelMilliVolt3.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelMilliVolt3, (ErrorIconAlignment)componentResourceManager.GetObject("labelMilliVolt3.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelMilliVolt3, (ErrorIconAlignment)componentResourceManager.GetObject("labelMilliVolt3.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelMilliVolt3, (int)componentResourceManager.GetObject("labelMilliVolt3.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelMilliVolt3, (int)componentResourceManager.GetObject("labelMilliVolt3.IconPadding1"));
			this.labelMilliVolt3.Name = "labelMilliVolt3";
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.errorProviderFormat.SetError(this.buttonOK, componentResourceManager.GetString("buttonOK.Error"));
			this.errorProviderGlobalModel.SetError(this.buttonOK, componentResourceManager.GetString("buttonOK.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.buttonOK, (ErrorIconAlignment)componentResourceManager.GetObject("buttonOK.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.buttonOK, (ErrorIconAlignment)componentResourceManager.GetObject("buttonOK.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.buttonOK, (int)componentResourceManager.GetObject("buttonOK.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.buttonOK, (int)componentResourceManager.GetObject("buttonOK.IconPadding1"));
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.errorProviderFormat.SetError(this.buttonCancel, componentResourceManager.GetString("buttonCancel.Error"));
			this.errorProviderGlobalModel.SetError(this.buttonCancel, componentResourceManager.GetString("buttonCancel.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.buttonCancel, (ErrorIconAlignment)componentResourceManager.GetObject("buttonCancel.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.buttonCancel, (ErrorIconAlignment)componentResourceManager.GetObject("buttonCancel.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.buttonCancel, (int)componentResourceManager.GetObject("buttonCancel.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.buttonCancel, (int)componentResourceManager.GetObject("buttonCancel.IconPadding1"));
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.errorProviderFormat.SetError(this.buttonHelp, componentResourceManager.GetString("buttonHelp.Error"));
			this.errorProviderGlobalModel.SetError(this.buttonHelp, componentResourceManager.GetString("buttonHelp.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.buttonHelp, (ErrorIconAlignment)componentResourceManager.GetObject("buttonHelp.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.buttonHelp, (ErrorIconAlignment)componentResourceManager.GetObject("buttonHelp.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.buttonHelp, (int)componentResourceManager.GetObject("buttonHelp.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.buttonHelp, (int)componentResourceManager.GetObject("buttonHelp.IconPadding1"));
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderFormat, "errorProviderFormat");
			componentResourceManager.ApplyResources(this.labelToleranceInfo, "labelToleranceInfo");
			this.errorProviderFormat.SetError(this.labelToleranceInfo, componentResourceManager.GetString("labelToleranceInfo.Error"));
			this.errorProviderGlobalModel.SetError(this.labelToleranceInfo, componentResourceManager.GetString("labelToleranceInfo.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelToleranceInfo, (ErrorIconAlignment)componentResourceManager.GetObject("labelToleranceInfo.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelToleranceInfo, (ErrorIconAlignment)componentResourceManager.GetObject("labelToleranceInfo.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelToleranceInfo, (int)componentResourceManager.GetObject("labelToleranceInfo.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelToleranceInfo, (int)componentResourceManager.GetObject("labelToleranceInfo.IconPadding1"));
			this.labelToleranceInfo.Name = "labelToleranceInfo";
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.labelToleranceInfo);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.labelMilliVolt3);
			base.Controls.Add(this.textBoxTolerance);
			base.Controls.Add(this.labelTolerance);
			base.Controls.Add(this.labelMilliVolt2);
			base.Controls.Add(this.labelMilliVolt1);
			base.Controls.Add(this.labelHighValue);
			base.Controls.Add(this.textBoxHighValue);
			base.Controls.Add(this.labelLowValue);
			base.Controls.Add(this.textBoxLowValue);
			base.Controls.Add(this.comboBoxRelOperator);
			base.Controls.Add(this.labelRelOp);
			base.Controls.Add(this.comboBoxInputNumber);
			base.Controls.Add(this.labelInput);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AnalogInputCondition";
			base.Shown += new EventHandler(this.AnalogInputCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.AnalogInputCondition_HelpRequested);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
