using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.LINChannelsPage
{
	public class LINprobeChannelRow : UserControl, IValidatable
	{
		private uint channelNr;

		private LINprobeChannel linprobeChannel;

		private IValidationHost validationHost;

		private bool isInitControls;

		private IContainer components;

		private Label labelName;

		private CheckBox checkBoxUseFixBaudrateOfLINprobe;

		private ComboBox comboBoxBaudrate;

		private CheckBox checkBoxUseDatabaseValues;

		public uint ChannelNr
		{
			get
			{
				return this.channelNr;
			}
			set
			{
				this.channelNr = value;
				this.labelName.Text = string.Format(Resources.LINprobeChannel, this.channelNr);
			}
		}

		public LINprobeChannel LINprobeChannel
		{
			get
			{
				return this.linprobeChannel;
			}
			set
			{
				this.linprobeChannel = value;
				this.UpdateGUI();
			}
		}

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		IValidationHost IValidatable.ValidationHost
		{
			get
			{
				return this.validationHost;
			}
			set
			{
				this.validationHost = value;
				if (this.validationHost != null)
				{
					this.validationHost.RegisterForErrorProvider(this.checkBoxUseFixBaudrateOfLINprobe);
					this.validationHost.RegisterForErrorProvider(this.comboBoxBaudrate);
					this.validationHost.RegisterForErrorProvider(this.checkBoxUseDatabaseValues);
				}
			}
		}

		private IPageValidatorControl PageValidatorControl
		{
			get
			{
				return this.validationHost.PageValidator.Control;
			}
		}

		private GUIElementManager_Control GUIElementManager
		{
			get
			{
				return this.validationHost.GUIElementManager;
			}
		}

		public LINprobeChannelRow()
		{
			this.InitializeComponent();
			this.isInitControls = true;
			this.checkBoxUseFixBaudrateOfLINprobe.Checked = false;
			this.checkBoxUseDatabaseValues.Checked = false;
			this.InitBaudratesCombobox();
			this.isInitControls = false;
			this.linprobeChannel = null;
			this.channelNr = 0u;
			this.labelName.Text = string.Format(Resources.LINprobeChannel, this.channelNr);
		}

		private void InitBaudratesCombobox()
		{
			this.comboBoxBaudrate.Items.Clear();
			foreach (uint current in GUIUtil.GetStandardLINBaudrates())
			{
				this.comboBoxBaudrate.Items.Add(GUIUtil.MapBaudrate2String(current));
			}
			this.comboBoxBaudrate.SelectedIndex = 0;
		}

		private void checkBoxUseFixBaudrateOfLINprobe_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.EnableBaudrateControls(this.checkBoxUseFixBaudrateOfLINprobe.Checked);
			this.validationHost.ValidateInput();
		}

		private void comboBoxBaudrate_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.validationHost.ValidateInput();
		}

		private void checkBoxUseDatabaseValues_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.comboBoxBaudrate.Enabled = !this.checkBoxUseDatabaseValues.Checked;
			this.validationHost.ValidateInput();
		}

		bool IValidatable.ValidateInput(ref bool valueChanged)
		{
			bool flag = true;
			this.DisplayBaudrateFromDb();
			bool flag2;
			flag &= this.PageValidatorControl.UpdateModel<bool>(this.checkBoxUseFixBaudrateOfLINprobe.Checked, this.linprobeChannel.UseFixLINprobeBaudrate, this.GUIElementManager.GetGUIElement(this.checkBoxUseFixBaudrateOfLINprobe), out flag2);
			valueChanged |= flag2;
			flag &= this.PageValidatorControl.UpdateModel<uint>(GUIUtil.MapString2Baudrate(this.comboBoxBaudrate.SelectedItem.ToString()), this.linprobeChannel.SpeedRate, this.GUIElementManager.GetGUIElement(this.comboBoxBaudrate), out flag2);
			valueChanged |= flag2;
			flag &= this.PageValidatorControl.UpdateModel<bool>(this.checkBoxUseDatabaseValues.Checked, this.linprobeChannel.UseDbConfigValues, this.GUIElementManager.GetGUIElement(this.checkBoxUseDatabaseValues), out flag2);
			valueChanged |= flag2;
			return flag;
		}

		private void UpdateGUI()
		{
			if (this.linprobeChannel == null)
			{
				return;
			}
			this.isInitControls = true;
			this.checkBoxUseFixBaudrateOfLINprobe.Checked = this.linprobeChannel.UseFixLINprobeBaudrate.Value;
			this.checkBoxUseDatabaseValues.Checked = this.linprobeChannel.UseDbConfigValues.Value;
			this.comboBoxBaudrate.SelectedItem = GUIUtil.MapBaudrate2String(this.linprobeChannel.SpeedRate.Value);
			this.EnableBaudrateControls(!this.linprobeChannel.UseFixLINprobeBaudrate.Value);
			this.isInitControls = false;
			this.DisplayBaudrateFromDb();
		}

		private void EnableBaudrateControls(bool isEnabled)
		{
			this.comboBoxBaudrate.Enabled = (!this.checkBoxUseDatabaseValues.Checked && isEnabled);
			this.checkBoxUseDatabaseValues.Enabled = isEnabled;
		}

		private void DisplayBaudrateFromDb()
		{
			int num;
			string text;
			if (this.GetLinChipConfigFromLdfDatabase(out num, out text))
			{
				this.checkBoxUseDatabaseValues.Text = string.Format(Resources.LINChannelDbBaudrate, num);
				return;
			}
			this.checkBoxUseDatabaseValues.Text = string.Format(Resources.LINChannelDbBaudrate, "-");
		}

		private bool GetLinChipConfigFromLdfDatabase(out int baudrate, out string protocol)
		{
			baudrate = 0;
			protocol = string.Empty;
			int num;
			return this.ModelValidator != null && this.ModelValidator.DatabaseServices.GetLinChipConfigFromLdfDatabase(this.ChannelNr, out baudrate, out protocol, out num);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LINprobeChannelRow));
			this.labelName = new Label();
			this.checkBoxUseFixBaudrateOfLINprobe = new CheckBox();
			this.comboBoxBaudrate = new ComboBox();
			this.checkBoxUseDatabaseValues = new CheckBox();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelName, "labelName");
			this.labelName.Name = "labelName";
			componentResourceManager.ApplyResources(this.checkBoxUseFixBaudrateOfLINprobe, "checkBoxUseFixBaudrateOfLINprobe");
			this.checkBoxUseFixBaudrateOfLINprobe.Name = "checkBoxUseFixBaudrateOfLINprobe";
			this.checkBoxUseFixBaudrateOfLINprobe.UseVisualStyleBackColor = true;
			this.checkBoxUseFixBaudrateOfLINprobe.CheckedChanged += new EventHandler(this.checkBoxUseFixBaudrateOfLINprobe_CheckedChanged);
			this.comboBoxBaudrate.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBaudrate.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxBaudrate, "comboBoxBaudrate");
			this.comboBoxBaudrate.Name = "comboBoxBaudrate";
			this.comboBoxBaudrate.SelectedIndexChanged += new EventHandler(this.comboBoxBaudrate_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxUseDatabaseValues, "checkBoxUseDatabaseValues");
			this.checkBoxUseDatabaseValues.Name = "checkBoxUseDatabaseValues";
			this.checkBoxUseDatabaseValues.UseVisualStyleBackColor = true;
			this.checkBoxUseDatabaseValues.CheckedChanged += new EventHandler(this.checkBoxUseDatabaseValues_CheckedChanged);
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.checkBoxUseDatabaseValues);
			base.Controls.Add(this.comboBoxBaudrate);
			base.Controls.Add(this.checkBoxUseFixBaudrateOfLINprobe);
			base.Controls.Add(this.labelName);
			base.Name = "LINprobeChannelRow";
			componentResourceManager.ApplyResources(this, "$this");
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
