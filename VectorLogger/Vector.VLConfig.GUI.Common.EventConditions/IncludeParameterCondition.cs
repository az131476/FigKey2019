using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.IncludeFilesPage;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.EventConditions
{
	public class IncludeParameterCondition : Form
	{
		private class ComboBoxItemOutParameter
		{
			public IncludeFileParameterPresenter IncludeFileParameter
			{
				get;
				private set;
			}

			public ComboBoxItemOutParameter(IncludeFileParameterPresenter includeFileParameter)
			{
				this.IncludeFileParameter = includeFileParameter;
			}

			public override string ToString()
			{
				if (this.IncludeFileParameter.Parent.InstanceParameter != null)
				{
					return string.Concat(new string[]
					{
						this.IncludeFileParameter.FileName,
						" (",
						this.IncludeFileParameter.Parent.InstanceValue,
						"): ",
						this.IncludeFileParameter.Name
					});
				}
				return this.IncludeFileParameter.FileName + ": " + this.IncludeFileParameter.Name;
			}
		}

		private class ComboBoxItemOutParameterWithError : IncludeParameterCondition.ComboBoxItemOutParameter
		{
			public string ErrorText
			{
				get;
				private set;
			}

			public ComboBoxItemOutParameterWithError(IncludeFileParameterPresenter includeFileParameter, string errorText) : base(includeFileParameter)
			{
				this.ErrorText = errorText;
			}
		}

		private class ComboBoxItemRelation
		{
			public CondRelation CondRelation
			{
				get;
				private set;
			}

			public ComboBoxItemRelation(CondRelation condRelation)
			{
				this.CondRelation = condRelation;
			}

			public override string ToString()
			{
				return GUIUtil.MapTriggerConditionRelation2String(this.CondRelation);
			}
		}

		private IContainer components;

		private Label mLabelOutParameters;

		private ComboBox mComboBoxOutParameters;

		private Button mButtonCancel;

		private Button mButtonOK;

		private Button mButtonHelp;

		private ComboBox mComboBoxRelation;

		private Label mLabelConditionType;

		private Label mLabelRaw;

		private TextBox mTextBoxLowValue;

		private Label mLabelValue;

		private TextBox mTextBoxHighValue;

		private Label mLabelHighLimitValue;

		private RichTextBox mRichTextBoxIncFileInfo;

		private ErrorProvider mErrorProviderFormat;

		private ErrorProvider mErrorProviderGlobalModel;

		public IncEvent IncEvent
		{
			get;
			set;
		}

		public string ErrorText
		{
			get;
			set;
		}

		private IncludeFileManager IncludeFileManager
		{
			get
			{
				return IncludeFileManager.Instance;
			}
		}

		public IncludeParameterCondition()
		{
			this.IncEvent = new IncEvent();
			this.InitializeComponent();
			this.RenderLabelsForDisplayMode();
			this.InitComboBoxOutParameters();
			this.InitRelationComboBox();
		}

		private void RenderLabelsForDisplayMode()
		{
			string arg = GUIUtil.IsHexadecimal ? Resources.DisplayModeHex : Resources.DisplayModeDec;
			this.mLabelRaw.Text = string.Format(Resources_IncFiles.LabelRawWithMode, arg);
		}

		private void InitComboBoxOutParameters()
		{
			foreach (IncludeFileParameterPresenter current in this.IncludeFileManager.OutParameters)
			{
				if (this.IncludeFileManager.IsValidOutParameterForTriggers(current))
				{
					IncludeParameterCondition.ComboBoxItemOutParameter item = new IncludeParameterCondition.ComboBoxItemOutParameter(current);
					this.mComboBoxOutParameters.Items.Add(item);
				}
			}
		}

		private void InitRelationComboBox()
		{
			foreach (CondRelation condRelation in Enum.GetValues(typeof(CondRelation)))
			{
				if (condRelation != CondRelation.OnChange)
				{
					this.mComboBoxRelation.Items.Add(new IncludeParameterCondition.ComboBoxItemRelation(condRelation));
				}
			}
		}

		private void RegisterControlEvents()
		{
			this.mComboBoxOutParameters.SelectedIndexChanged += new EventHandler(this.ComboBoxOutParameters_SelectedIndexChanged);
			this.mComboBoxRelation.SelectedIndexChanged += new EventHandler(this.ComboBoxRelation_SelectedIndexChanged);
			this.mComboBoxOutParameters.Validating += new CancelEventHandler(this.ComboBoxOutParameters_Validating);
			this.mComboBoxRelation.Validating += new CancelEventHandler(this.ComboBoxRelation_Validating);
			this.mTextBoxLowValue.Validating += new CancelEventHandler(this.TextBoxValue_Validating);
			this.mTextBoxHighValue.Validating += new CancelEventHandler(this.TextBoxValue_Validating);
		}

		private void UnregisterControlEvents()
		{
			this.mComboBoxOutParameters.SelectedIndexChanged -= new EventHandler(this.ComboBoxOutParameters_SelectedIndexChanged);
			this.mComboBoxRelation.SelectedIndexChanged -= new EventHandler(this.ComboBoxRelation_SelectedIndexChanged);
			this.mComboBoxOutParameters.Validating -= new CancelEventHandler(this.ComboBoxOutParameters_Validating);
			this.mComboBoxRelation.Validating -= new CancelEventHandler(this.ComboBoxRelation_Validating);
			this.mTextBoxLowValue.Validating -= new CancelEventHandler(this.TextBoxValue_Validating);
			this.mTextBoxHighValue.Validating -= new CancelEventHandler(this.TextBoxValue_Validating);
		}

		private void UpdateFromEvent()
		{
			this.UnregisterControlEvents();
			IncludeParameterCondition.ComboBoxItemOutParameter comboBoxItemOutParameter = this.mComboBoxOutParameters.Items.OfType<IncludeParameterCondition.ComboBoxItemOutParameter>().FirstOrDefault((IncludeParameterCondition.ComboBoxItemOutParameter item) => item.IncludeFileParameter.IncludeFile.FilePath.Value.Equals(this.IncEvent.FilePath.Value) && item.IncludeFileParameter.Parent.InstanceValue.Equals(this.IncEvent.InstanceValue.Value) && item.IncludeFileParameter.ParameterIndex.Equals(this.IncEvent.ParamIndex.Value));
			if (comboBoxItemOutParameter != null)
			{
				this.mComboBoxOutParameters.SelectedIndex = this.mComboBoxOutParameters.Items.IndexOf(comboBoxItemOutParameter);
			}
			else if (!string.IsNullOrEmpty(this.IncEvent.FilePath.Value))
			{
				IncludeFileParameterPresenter includeFileParameterPresenter;
				if (this.IncludeFileManager.TryGetOutParameter(this.IncEvent, out includeFileParameterPresenter, false))
				{
					IncludeParameterCondition.ComboBoxItemOutParameterWithError item3 = new IncludeParameterCondition.ComboBoxItemOutParameterWithError(includeFileParameterPresenter, this.IncludeFileManager.GetFirstErrorTextForTriggers(includeFileParameterPresenter));
					this.AddDummyEntryToComboBoxOutParameters(item3);
				}
				else
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.IncEvent.FilePath.Value);
					string str = (!string.IsNullOrEmpty(this.IncEvent.InstanceValue.Value)) ? ("(" + this.IncEvent.InstanceValue.Value + ")") : string.Empty;
					string str2 = string.Format(Resources_IncFiles.InParameterDefaultName, this.IncEvent.ParamIndex.Value + 1);
					string item2 = fileNameWithoutExtension + str + ": " + str2;
					this.AddDummyEntryToComboBoxOutParameters(item2);
				}
			}
			else if (this.mComboBoxOutParameters.Items.OfType<IncludeParameterCondition.ComboBoxItemOutParameter>().Any<IncludeParameterCondition.ComboBoxItemOutParameter>())
			{
				IncludeParameterCondition.ComboBoxItemOutParameter comboBoxItemOutParameter2 = this.mComboBoxOutParameters.Items.OfType<IncludeParameterCondition.ComboBoxItemOutParameter>().First<IncludeParameterCondition.ComboBoxItemOutParameter>();
				this.IncEvent.FilePath.Value = comboBoxItemOutParameter2.IncludeFileParameter.IncludeFile.FilePath.Value;
				this.IncEvent.InstanceValue.Value = comboBoxItemOutParameter2.IncludeFileParameter.Parent.InstanceValue;
				this.IncEvent.ParamIndex.Value = comboBoxItemOutParameter2.IncludeFileParameter.ParameterIndex;
				this.mComboBoxOutParameters.SelectedIndex = this.mComboBoxOutParameters.Items.IndexOf(comboBoxItemOutParameter2);
			}
			IncludeParameterCondition.ComboBoxItemRelation comboBoxItemRelation = this.mComboBoxRelation.Items.OfType<IncludeParameterCondition.ComboBoxItemRelation>().First((IncludeParameterCondition.ComboBoxItemRelation item) => item.CondRelation.Equals(this.IncEvent.Relation.Value));
			Trace.Assert(comboBoxItemRelation != null);
			this.mComboBoxRelation.SelectedIndex = this.mComboBoxRelation.Items.IndexOf(comboBoxItemRelation);
			this.mTextBoxLowValue.Text = GUIUtil.NumberToDisplayString(this.IncEvent.LowValue.Value);
			this.mTextBoxHighValue.Enabled = (this.IncEvent.Relation.Value == CondRelation.InRange || this.IncEvent.Relation.Value == CondRelation.NotInRange);
			if (!this.mTextBoxHighValue.Enabled)
			{
				this.IncEvent.HighValue.Value = 0;
			}
			this.mTextBoxHighValue.Text = GUIUtil.NumberToDisplayString(this.IncEvent.HighValue.Value);
			this.PrintIncFileInfo();
			this.RegisterControlEvents();
		}

		private void AddDummyEntryToComboBoxOutParameters(object item)
		{
			this.RemoveDummyEntryFromComboBoxOutParameters();
			this.mComboBoxOutParameters.Items.Insert(0, item);
			this.mComboBoxOutParameters.SelectedIndex = this.mComboBoxOutParameters.Items.IndexOf(item);
		}

		private void RemoveDummyEntryFromComboBoxOutParameters()
		{
			if (this.mComboBoxOutParameters.Items.Count == 0)
			{
				return;
			}
			object obj = this.mComboBoxOutParameters.Items[0];
			if (obj is string || obj is IncludeParameterCondition.ComboBoxItemOutParameterWithError)
			{
				this.mComboBoxOutParameters.Items.RemoveAt(0);
			}
		}

		private void PrintIncFileInfo()
		{
			this.mRichTextBoxIncFileInfo.Clear();
			IncludeParameterCondition.ComboBoxItemOutParameter comboBoxItemOutParameter = this.mComboBoxOutParameters.SelectedItem as IncludeParameterCondition.ComboBoxItemOutParameter;
			if (comboBoxItemOutParameter != null)
			{
				IncludeFileParameterPresenter includeFileParameter = comboBoxItemOutParameter.IncludeFileParameter;
				this.mRichTextBoxIncFileInfo.SelectionFont = new Font(this.mRichTextBoxIncFileInfo.Font, FontStyle.Bold);
				this.mRichTextBoxIncFileInfo.AppendText(Resources_IncFiles.InParameters);
				this.mRichTextBoxIncFileInfo.AppendText(Environment.NewLine);
				this.mRichTextBoxIncFileInfo.SelectionFont = new Font(this.mRichTextBoxIncFileInfo.Font, FontStyle.Regular);
				StringBuilder stringBuilder = new StringBuilder();
				List<IncludeFileParameterPresenter> list = (from param in includeFileParameter.Parent.Parameters
				where param.ParameterType == IncludeFileParameter.ParamType.In
				select param).ToList<IncludeFileParameterPresenter>();
				foreach (IncludeFileParameterPresenter current in list)
				{
					this.PrintParam(stringBuilder, current);
				}
				this.mRichTextBoxIncFileInfo.AppendText(stringBuilder.ToString());
			}
		}

		private void PrintParam(StringBuilder sb, IncludeFileParameterPresenter paramPres)
		{
			sb.Append(paramPres.Name);
			if (!string.IsNullOrEmpty(paramPres.Value))
			{
				sb.Append(" = '");
				sb.Append(paramPres.Value);
				sb.Append("'");
			}
			sb.AppendLine();
		}

		private void PrintOutParam(StringBuilder sb, IncludeFileParameterPresenter paramPres)
		{
			sb.Append(paramPres.Name);
			sb.Append(" => ");
			sb.Append(paramPres.LtlName);
			sb.AppendLine();
		}

		private void IncludeParameterCondition_Shown(object sender, EventArgs e)
		{
			this.UpdateFromEvent();
			this.ValidateChildren();
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			if (!this.ValidateChildren())
			{
				InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
				base.DialogResult = DialogResult.None;
			}
		}

		private void ButtonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void IncludeParameterCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void ComboBoxOutParameters_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.mErrorProviderGlobalModel.SetError(this.mComboBoxOutParameters, string.Empty);
			string value = this.mComboBoxOutParameters.SelectedItem as string;
			if (!string.IsNullOrEmpty(value))
			{
				this.mErrorProviderGlobalModel.SetError(this.mComboBoxOutParameters, (!string.IsNullOrEmpty(this.ErrorText)) ? this.ErrorText : Resources_IncFiles.ErrorInvalidOutParameter);
				return;
			}
			IncludeParameterCondition.ComboBoxItemOutParameterWithError comboBoxItemOutParameterWithError = this.mComboBoxOutParameters.SelectedItem as IncludeParameterCondition.ComboBoxItemOutParameterWithError;
			if (comboBoxItemOutParameterWithError != null)
			{
				this.mErrorProviderGlobalModel.SetError(this.mComboBoxOutParameters, comboBoxItemOutParameterWithError.ErrorText);
				return;
			}
			IncludeParameterCondition.ComboBoxItemOutParameter comboBoxItemOutParameter = this.mComboBoxOutParameters.SelectedItem as IncludeParameterCondition.ComboBoxItemOutParameter;
			if (comboBoxItemOutParameter == null)
			{
				this.mErrorProviderGlobalModel.SetError(this.mComboBoxOutParameters, Resources_IncFiles.ErrorNoValidOutParameterAvailable);
				if (!this.IncEvent.FilePath.Value.Equals(string.Empty) || !this.IncEvent.InstanceValue.Value.Equals(string.Empty) || this.IncEvent.ParamIndex.Value != 0)
				{
					this.IncEvent.FilePath.Value = string.Empty;
					this.IncEvent.InstanceValue.Value = string.Empty;
					this.IncEvent.ParamIndex.Value = 0;
					this.UpdateFromEvent();
				}
				return;
			}
			this.RemoveDummyEntryFromComboBoxOutParameters();
			IncludeFileParameterPresenter includeFileParameter = comboBoxItemOutParameter.IncludeFileParameter;
			if (!this.IncEvent.FilePath.Value.Equals(includeFileParameter.IncludeFile.FilePath.Value) || !this.IncEvent.InstanceValue.Value.Equals(includeFileParameter.Parent.InstanceValue) || this.IncEvent.ParamIndex.Value != includeFileParameter.ParameterIndex)
			{
				this.IncEvent.FilePath.Value = includeFileParameter.IncludeFile.FilePath.Value;
				this.IncEvent.InstanceValue.Value = includeFileParameter.Parent.InstanceValue;
				this.IncEvent.ParamIndex.Value = includeFileParameter.ParameterIndex;
				this.UpdateFromEvent();
			}
		}

		private void ComboBoxOutParameters_Validating(object sender, CancelEventArgs e)
		{
			this.mErrorProviderGlobalModel.SetError(this.mComboBoxOutParameters, string.Empty);
			string value = this.mComboBoxOutParameters.SelectedItem as string;
			if (!string.IsNullOrEmpty(value))
			{
				this.mErrorProviderGlobalModel.SetError(this.mComboBoxOutParameters, (!string.IsNullOrEmpty(this.ErrorText)) ? this.ErrorText : Resources_IncFiles.ErrorInvalidOutParameter);
				e.Cancel = true;
				return;
			}
			IncludeParameterCondition.ComboBoxItemOutParameterWithError comboBoxItemOutParameterWithError = this.mComboBoxOutParameters.SelectedItem as IncludeParameterCondition.ComboBoxItemOutParameterWithError;
			if (comboBoxItemOutParameterWithError != null)
			{
				this.mErrorProviderGlobalModel.SetError(this.mComboBoxOutParameters, comboBoxItemOutParameterWithError.ErrorText);
				e.Cancel = true;
				return;
			}
			if (!(this.mComboBoxOutParameters.SelectedItem is IncludeParameterCondition.ComboBoxItemOutParameter))
			{
				this.mErrorProviderGlobalModel.SetError(this.mComboBoxOutParameters, Resources_IncFiles.ErrorNoValidOutParameterAvailable);
				e.Cancel = true;
			}
		}

		private void ComboBoxRelation_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.mErrorProviderGlobalModel.SetError(this.mComboBoxRelation, string.Empty);
			IncludeParameterCondition.ComboBoxItemRelation comboBoxItemRelation = this.mComboBoxRelation.SelectedItem as IncludeParameterCondition.ComboBoxItemRelation;
			if (comboBoxItemRelation == null)
			{
				this.mErrorProviderGlobalModel.SetError(this.mComboBoxRelation, Resources_IncFiles.ErrorNoValidRelationAvailable);
				if (this.IncEvent.Relation.Value != CondRelation.Equal)
				{
					this.IncEvent.Relation.Value = CondRelation.Equal;
					this.UpdateFromEvent();
				}
				return;
			}
			if (this.IncEvent.Relation.Value != comboBoxItemRelation.CondRelation)
			{
				this.IncEvent.Relation.Value = comboBoxItemRelation.CondRelation;
				this.UpdateFromEvent();
			}
		}

		private void ComboBoxRelation_Validating(object sender, CancelEventArgs e)
		{
			this.mErrorProviderGlobalModel.SetError(this.mComboBoxRelation, string.Empty);
			if (!(this.mComboBoxRelation.SelectedItem is IncludeParameterCondition.ComboBoxItemRelation))
			{
				this.mErrorProviderGlobalModel.SetError(this.mComboBoxRelation, Resources_IncFiles.ErrorNoValidRelationAvailable);
				e.Cancel = true;
			}
		}

		private void TextBoxValue_Validating(object sender, CancelEventArgs e)
		{
			this.mErrorProviderFormat.SetError(this.mTextBoxLowValue, string.Empty);
			this.mErrorProviderFormat.SetError(this.mTextBoxHighValue, string.Empty);
			this.mErrorProviderGlobalModel.SetError(this.mTextBoxLowValue, string.Empty);
			this.mErrorProviderGlobalModel.SetError(this.mTextBoxHighValue, string.Empty);
			int num;
			if (!GUIUtil.DisplayStringToNumber(this.mTextBoxLowValue.Text, out num))
			{
				this.mErrorProviderFormat.SetError(this.mTextBoxLowValue, Resources_IncFiles.ErrorIntegerExpected);
				e.Cancel = true;
			}
			int num2;
			if (!GUIUtil.DisplayStringToNumber(this.mTextBoxHighValue.Text, out num2))
			{
				this.mErrorProviderFormat.SetError(this.mTextBoxHighValue, Resources_IncFiles.ErrorIntegerExpected);
				e.Cancel = true;
			}
			if (e.Cancel)
			{
				return;
			}
			if (num < -32768 || num > 65535)
			{
				this.mErrorProviderGlobalModel.SetError(this.mTextBoxLowValue, string.Format(Resources_IncFiles.ErrorValueOutOfRange, -32768, 65535));
				e.Cancel = true;
			}
			if (num2 < -32768 || num2 > 65535)
			{
				this.mErrorProviderGlobalModel.SetError(this.mTextBoxHighValue, string.Format(Resources_IncFiles.ErrorValueOutOfRange, -32768, 65535));
				e.Cancel = true;
			}
			if (e.Cancel)
			{
				return;
			}
			if (num > num2 && (this.IncEvent.Relation.Value == CondRelation.InRange || this.IncEvent.Relation.Value == CondRelation.NotInRange))
			{
				this.mErrorProviderFormat.SetError(this.mTextBoxLowValue, Resources_IncFiles.ErrorValueMustNotBeGreater);
				e.Cancel = true;
			}
			if (num2 < num && (this.IncEvent.Relation.Value == CondRelation.InRange || this.IncEvent.Relation.Value == CondRelation.NotInRange))
			{
				this.mErrorProviderFormat.SetError(this.mTextBoxHighValue, Resources_IncFiles.ErrorValueMustNotBeLess);
				e.Cancel = true;
			}
			if (e.Cancel)
			{
				return;
			}
			if (this.IncEvent.LowValue.Value != num || this.IncEvent.HighValue.Value != num2)
			{
				this.IncEvent.LowValue.Value = num;
				this.IncEvent.HighValue.Value = num2;
				this.UpdateFromEvent();
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
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(IncludeParameterCondition));
			this.mLabelOutParameters = new Label();
			this.mComboBoxOutParameters = new ComboBox();
			this.mButtonCancel = new Button();
			this.mButtonOK = new Button();
			this.mButtonHelp = new Button();
			this.mComboBoxRelation = new ComboBox();
			this.mLabelConditionType = new Label();
			this.mLabelRaw = new Label();
			this.mTextBoxLowValue = new TextBox();
			this.mLabelValue = new Label();
			this.mTextBoxHighValue = new TextBox();
			this.mLabelHighLimitValue = new Label();
			this.mRichTextBoxIncFileInfo = new RichTextBox();
			this.mErrorProviderFormat = new ErrorProvider(this.components);
			this.mErrorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.mErrorProviderFormat).BeginInit();
			((ISupportInitialize)this.mErrorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.mLabelOutParameters, "mLabelOutParameters");
			this.mLabelOutParameters.Name = "mLabelOutParameters";
			componentResourceManager.ApplyResources(this.mComboBoxOutParameters, "mComboBoxOutParameters");
			this.mComboBoxOutParameters.DropDownStyle = ComboBoxStyle.DropDownList;
			this.mComboBoxOutParameters.FormattingEnabled = true;
			this.mErrorProviderFormat.SetIconAlignment(this.mComboBoxOutParameters, (ErrorIconAlignment)componentResourceManager.GetObject("mComboBoxOutParameters.IconAlignment"));
			this.mErrorProviderGlobalModel.SetIconAlignment(this.mComboBoxOutParameters, (ErrorIconAlignment)componentResourceManager.GetObject("mComboBoxOutParameters.IconAlignment1"));
			this.mComboBoxOutParameters.Name = "mComboBoxOutParameters";
			componentResourceManager.ApplyResources(this.mButtonCancel, "mButtonCancel");
			this.mButtonCancel.DialogResult = DialogResult.Cancel;
			this.mButtonCancel.Name = "mButtonCancel";
			this.mButtonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mButtonOK, "mButtonOK");
			this.mButtonOK.DialogResult = DialogResult.OK;
			this.mButtonOK.Name = "mButtonOK";
			this.mButtonOK.UseVisualStyleBackColor = true;
			this.mButtonOK.Click += new EventHandler(this.ButtonOK_Click);
			componentResourceManager.ApplyResources(this.mButtonHelp, "mButtonHelp");
			this.mButtonHelp.Name = "mButtonHelp";
			this.mButtonHelp.UseVisualStyleBackColor = true;
			this.mButtonHelp.Click += new EventHandler(this.ButtonHelp_Click);
			this.mComboBoxRelation.DropDownStyle = ComboBoxStyle.DropDownList;
			this.mComboBoxRelation.FormattingEnabled = true;
			this.mErrorProviderFormat.SetIconAlignment(this.mComboBoxRelation, (ErrorIconAlignment)componentResourceManager.GetObject("mComboBoxRelation.IconAlignment"));
			this.mErrorProviderGlobalModel.SetIconAlignment(this.mComboBoxRelation, (ErrorIconAlignment)componentResourceManager.GetObject("mComboBoxRelation.IconAlignment1"));
			componentResourceManager.ApplyResources(this.mComboBoxRelation, "mComboBoxRelation");
			this.mComboBoxRelation.Name = "mComboBoxRelation";
			componentResourceManager.ApplyResources(this.mLabelConditionType, "mLabelConditionType");
			this.mLabelConditionType.Name = "mLabelConditionType";
			componentResourceManager.ApplyResources(this.mLabelRaw, "mLabelRaw");
			this.mLabelRaw.Name = "mLabelRaw";
			this.mErrorProviderGlobalModel.SetIconAlignment(this.mTextBoxLowValue, (ErrorIconAlignment)componentResourceManager.GetObject("mTextBoxLowValue.IconAlignment"));
			this.mErrorProviderFormat.SetIconAlignment(this.mTextBoxLowValue, (ErrorIconAlignment)componentResourceManager.GetObject("mTextBoxLowValue.IconAlignment1"));
			componentResourceManager.ApplyResources(this.mTextBoxLowValue, "mTextBoxLowValue");
			this.mTextBoxLowValue.Name = "mTextBoxLowValue";
			componentResourceManager.ApplyResources(this.mLabelValue, "mLabelValue");
			this.mLabelValue.Name = "mLabelValue";
			this.mErrorProviderGlobalModel.SetIconAlignment(this.mTextBoxHighValue, (ErrorIconAlignment)componentResourceManager.GetObject("mTextBoxHighValue.IconAlignment"));
			this.mErrorProviderFormat.SetIconAlignment(this.mTextBoxHighValue, (ErrorIconAlignment)componentResourceManager.GetObject("mTextBoxHighValue.IconAlignment1"));
			componentResourceManager.ApplyResources(this.mTextBoxHighValue, "mTextBoxHighValue");
			this.mTextBoxHighValue.Name = "mTextBoxHighValue";
			componentResourceManager.ApplyResources(this.mLabelHighLimitValue, "mLabelHighLimitValue");
			this.mLabelHighLimitValue.Name = "mLabelHighLimitValue";
			componentResourceManager.ApplyResources(this.mRichTextBoxIncFileInfo, "mRichTextBoxIncFileInfo");
			this.mRichTextBoxIncFileInfo.BackColor = SystemColors.Control;
			this.mRichTextBoxIncFileInfo.BorderStyle = BorderStyle.None;
			this.mRichTextBoxIncFileInfo.Name = "mRichTextBoxIncFileInfo";
			this.mRichTextBoxIncFileInfo.ReadOnly = true;
			this.mRichTextBoxIncFileInfo.TabStop = false;
			this.mErrorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.mErrorProviderFormat.ContainerControl = this;
			this.mErrorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.mErrorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.mErrorProviderGlobalModel, "mErrorProviderGlobalModel");
			base.AcceptButton = this.mButtonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoValidate = AutoValidate.EnableAllowFocusChange;
			base.CancelButton = this.mButtonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.mRichTextBoxIncFileInfo);
			base.Controls.Add(this.mTextBoxHighValue);
			base.Controls.Add(this.mLabelHighLimitValue);
			base.Controls.Add(this.mLabelRaw);
			base.Controls.Add(this.mTextBoxLowValue);
			base.Controls.Add(this.mLabelValue);
			base.Controls.Add(this.mComboBoxRelation);
			base.Controls.Add(this.mLabelConditionType);
			base.Controls.Add(this.mButtonHelp);
			base.Controls.Add(this.mButtonOK);
			base.Controls.Add(this.mButtonCancel);
			base.Controls.Add(this.mComboBoxOutParameters);
			base.Controls.Add(this.mLabelOutParameters);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "IncludeParameterCondition";
			base.Shown += new EventHandler(this.IncludeParameterCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.IncludeParameterCondition_HelpRequested);
			((ISupportInitialize)this.mErrorProviderFormat).EndInit();
			((ISupportInitialize)this.mErrorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
