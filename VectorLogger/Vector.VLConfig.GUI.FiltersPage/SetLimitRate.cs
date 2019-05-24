using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.FiltersPage
{
	public class SetLimitRate : Form
	{
		private uint interval;

		private IModelValidator modelValidator;

		private uint cycleTimeFromDB;

		private string symMessageName;

		private IContainer components;

		private Label labelIntervalPerMsg;

		private TextBox textBoxInterval;

		private ErrorProvider errorProviderFormat;

		private Button buttonHelp;

		private Button buttonCancel;

		private Button buttonOK;

		private Label labelCycleTimeFromDB;

		private Label labelMs;

		public Filter Filter
		{
			get;
			set;
		}

		public SetLimitRate(IModelValidator modelVal)
		{
			this.InitializeComponent();
			this.interval = 1u;
			this.modelValidator = modelVal;
			this.cycleTimeFromDB = 0u;
			this.symMessageName = "";
		}

		private void SetLimitRate_Shown(object sender, EventArgs e)
		{
			this.textBoxInterval.Text = this.Filter.LimitIntervalPerFrame.Value.ToString();
			this.labelCycleTimeFromDB.Visible = false;
			if (this.Filter is SymbolicMessageFilter)
			{
				SymbolicMessageFilter symbolicMessageFilter = this.Filter as SymbolicMessageFilter;
				MessageDefinition messageDefinition;
				if (symbolicMessageFilter.BusType.Value != BusType.Bt_FlexRay && this.modelValidator.DatabaseServices.IsSymbolicMessageDefined(symbolicMessageFilter.DatabasePath.Value, symbolicMessageFilter.NetworkName.Value, symbolicMessageFilter.MessageName.Value, symbolicMessageFilter.ChannelNumber.Value, symbolicMessageFilter.BusType.Value, out messageDefinition))
				{
					this.cycleTimeFromDB = (uint)messageDefinition.CycleTime;
					this.symMessageName = symbolicMessageFilter.MessageName.Value;
					this.labelCycleTimeFromDB.Visible = true;
				}
			}
			this.ValidateInput();
			this.textBoxInterval.Focus();
		}

		private void textBoxInterval_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateInput();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (!this.ValidateInput())
			{
				InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
				base.DialogResult = DialogResult.None;
				return;
			}
			this.Filter.LimitIntervalPerFrame.Value = this.interval;
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

		private bool ValidateInput()
		{
			this.errorProviderFormat.SetError(this.textBoxInterval, "");
			bool result = true;
			uint num = 0u;
			if (this.labelCycleTimeFromDB.Visible)
			{
				this.labelCycleTimeFromDB.Text = string.Format(Resources.MsgHasCycleTimeOfms, this.symMessageName, this.cycleTimeFromDB);
			}
			if (uint.TryParse(this.textBoxInterval.Text, out num))
			{
				if (num < 1u || num > Constants.MaxLimitInterval_ms)
				{
					this.errorProviderFormat.SetError(this.textBoxInterval, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, 1, Constants.MaxLimitInterval_ms));
					result = false;
				}
				else
				{
					this.interval = num;
					if (this.labelCycleTimeFromDB.Visible && this.cycleTimeFromDB > 0u)
					{
						string str = string.Format(Resources.MsgHasCycleTimeOfms, this.symMessageName, this.cycleTimeFromDB);
						if (this.cycleTimeFromDB >= this.interval)
						{
							this.labelCycleTimeFromDB.Text = str + " " + Resources.EveryFrameIsRec;
						}
						else
						{
							uint num2 = this.interval / this.cycleTimeFromDB;
							this.labelCycleTimeFromDB.Text = str + " " + string.Format(Resources.Approx1OutOfNthFramesIsRec, num2);
						}
					}
				}
			}
			else
			{
				this.errorProviderFormat.SetError(this.textBoxInterval, Resources.ErrorIntExpected);
				result = false;
			}
			return result;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SetLimitRate));
			this.labelIntervalPerMsg = new Label();
			this.textBoxInterval = new TextBox();
			this.labelMs = new Label();
			this.labelCycleTimeFromDB = new Label();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.buttonHelp = new Button();
			this.errorProviderFormat = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelIntervalPerMsg, "labelIntervalPerMsg");
			this.errorProviderFormat.SetError(this.labelIntervalPerMsg, componentResourceManager.GetString("labelIntervalPerMsg.Error"));
			this.errorProviderFormat.SetIconAlignment(this.labelIntervalPerMsg, (ErrorIconAlignment)componentResourceManager.GetObject("labelIntervalPerMsg.IconAlignment"));
			this.errorProviderFormat.SetIconPadding(this.labelIntervalPerMsg, (int)componentResourceManager.GetObject("labelIntervalPerMsg.IconPadding"));
			this.labelIntervalPerMsg.Name = "labelIntervalPerMsg";
			componentResourceManager.ApplyResources(this.textBoxInterval, "textBoxInterval");
			this.errorProviderFormat.SetError(this.textBoxInterval, componentResourceManager.GetString("textBoxInterval.Error"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxInterval, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxInterval.IconAlignment"));
			this.errorProviderFormat.SetIconPadding(this.textBoxInterval, (int)componentResourceManager.GetObject("textBoxInterval.IconPadding"));
			this.textBoxInterval.Name = "textBoxInterval";
			this.textBoxInterval.Validating += new CancelEventHandler(this.textBoxInterval_Validating);
			componentResourceManager.ApplyResources(this.labelMs, "labelMs");
			this.errorProviderFormat.SetError(this.labelMs, componentResourceManager.GetString("labelMs.Error"));
			this.errorProviderFormat.SetIconAlignment(this.labelMs, (ErrorIconAlignment)componentResourceManager.GetObject("labelMs.IconAlignment"));
			this.errorProviderFormat.SetIconPadding(this.labelMs, (int)componentResourceManager.GetObject("labelMs.IconPadding"));
			this.labelMs.Name = "labelMs";
			componentResourceManager.ApplyResources(this.labelCycleTimeFromDB, "labelCycleTimeFromDB");
			this.errorProviderFormat.SetError(this.labelCycleTimeFromDB, componentResourceManager.GetString("labelCycleTimeFromDB.Error"));
			this.errorProviderFormat.SetIconAlignment(this.labelCycleTimeFromDB, (ErrorIconAlignment)componentResourceManager.GetObject("labelCycleTimeFromDB.IconAlignment"));
			this.errorProviderFormat.SetIconPadding(this.labelCycleTimeFromDB, (int)componentResourceManager.GetObject("labelCycleTimeFromDB.IconPadding"));
			this.labelCycleTimeFromDB.Name = "labelCycleTimeFromDB";
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.errorProviderFormat.SetError(this.buttonOK, componentResourceManager.GetString("buttonOK.Error"));
			this.errorProviderFormat.SetIconAlignment(this.buttonOK, (ErrorIconAlignment)componentResourceManager.GetObject("buttonOK.IconAlignment"));
			this.errorProviderFormat.SetIconPadding(this.buttonOK, (int)componentResourceManager.GetObject("buttonOK.IconPadding"));
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.errorProviderFormat.SetError(this.buttonCancel, componentResourceManager.GetString("buttonCancel.Error"));
			this.errorProviderFormat.SetIconAlignment(this.buttonCancel, (ErrorIconAlignment)componentResourceManager.GetObject("buttonCancel.IconAlignment"));
			this.errorProviderFormat.SetIconPadding(this.buttonCancel, (int)componentResourceManager.GetObject("buttonCancel.IconPadding"));
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.errorProviderFormat.SetError(this.buttonHelp, componentResourceManager.GetString("buttonHelp.Error"));
			this.errorProviderFormat.SetIconAlignment(this.buttonHelp, (ErrorIconAlignment)componentResourceManager.GetObject("buttonHelp.IconAlignment"));
			this.errorProviderFormat.SetIconPadding(this.buttonHelp, (int)componentResourceManager.GetObject("buttonHelp.IconPadding"));
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderFormat, "errorProviderFormat");
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.labelCycleTimeFromDB);
			base.Controls.Add(this.labelMs);
			base.Controls.Add(this.textBoxInterval);
			base.Controls.Add(this.labelIntervalPerMsg);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "SetLimitRate";
			base.Shown += new EventHandler(this.SetLimitRate_Shown);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
