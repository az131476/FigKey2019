using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.MultibusChannelsPage
{
	internal class MultibusLINChannel : UserControl, IValidatable
	{
		private enum LinProtocolVersion
		{
			LIN_1_0,
			LIN_1_1,
			LIN_1_2,
			LIN_1_3,
			LIN_2_0,
			LIN_2_1,
			LIN_2_2,
			LIN_ISO_2015
		}

		private class ProtocolVersionComboboxItem
		{
			private readonly MultibusLINChannel.LinProtocolVersion mProtocolVersion;

			public ProtocolVersionComboboxItem(MultibusLINChannel.LinProtocolVersion protocolVersion)
			{
				this.mProtocolVersion = protocolVersion;
			}

			public override string ToString()
			{
				switch (this.mProtocolVersion)
				{
				case MultibusLINChannel.LinProtocolVersion.LIN_1_0:
					return "LIN 1.0";
				case MultibusLINChannel.LinProtocolVersion.LIN_1_1:
					return "LIN 1.1";
				case MultibusLINChannel.LinProtocolVersion.LIN_1_2:
					return "LIN 1.2";
				case MultibusLINChannel.LinProtocolVersion.LIN_1_3:
					return "LIN 1.3";
				case MultibusLINChannel.LinProtocolVersion.LIN_2_0:
					return "LIN 2.0";
				case MultibusLINChannel.LinProtocolVersion.LIN_2_1:
					return "LIN 2.1";
				case MultibusLINChannel.LinProtocolVersion.LIN_2_2:
					return "LIN 2.2";
				case MultibusLINChannel.LinProtocolVersion.LIN_ISO_2015:
					return "ISO17987:2015";
				default:
					Trace.Assert(false);
					return string.Empty;
				}
			}

			public static void Select(ComboBox comboBox, int protocolVersion)
			{
				Trace.Assert(comboBox != null);
				MultibusLINChannel.ProtocolVersionComboboxItem protocolVersionComboboxItem = comboBox.Items.OfType<MultibusLINChannel.ProtocolVersionComboboxItem>().FirstOrDefault((MultibusLINChannel.ProtocolVersionComboboxItem t) => t.mProtocolVersion == (MultibusLINChannel.LinProtocolVersion)protocolVersion);
				if (protocolVersionComboboxItem != null)
				{
					comboBox.SelectedItem = protocolVersionComboboxItem;
				}
			}

			public static int GetSelected(ComboBox comboBox)
			{
				Trace.Assert(comboBox != null);
				MultibusLINChannel.ProtocolVersionComboboxItem protocolVersionComboboxItem = comboBox.SelectedItem as MultibusLINChannel.ProtocolVersionComboboxItem;
				Trace.Assert(protocolVersionComboboxItem != null);
				return (int)protocolVersionComboboxItem.mProtocolVersion;
			}
		}

		private LINChannel channel;

		private IValidationHost validationHost;

		private bool isInitControls;

		private IContainer components;

		private ComboBox comboBoxSpeedRate;

		private ComboBox comboBoxProtocolVersion;

		private CheckBox checkBoxUseDbValues;

		private Label labelProt;

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
		}

		public uint ChannelNr
		{
			get;
			set;
		}

		public LINChannel Channel
		{
			get
			{
				return this.channel;
			}
			set
			{
				this.channel = value;
				this.UpdateGUI();
			}
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
					this.validationHost.RegisterForErrorProvider(this.checkBoxUseDbValues);
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

		public MultibusLINChannel()
		{
			this.InitializeComponent();
			this.isInitControls = true;
			this.InitSpeedRateComboBox();
			this.InitProtocolVersionComboBox();
			this.isInitControls = false;
		}

		private void InitSpeedRateComboBox()
		{
			this.comboBoxSpeedRate.Items.Clear();
			IList<uint> standardLINBaudrates = GUIUtil.GetStandardLINBaudrates();
			foreach (uint current in standardLINBaudrates)
			{
				this.comboBoxSpeedRate.Items.Add(new BaudrateComboboxItem(current));
			}
			this.comboBoxSpeedRate.SelectedIndex = 0;
		}

		private void InitProtocolVersionComboBox()
		{
			this.comboBoxProtocolVersion.Items.Clear();
			this.comboBoxProtocolVersion.Items.Add(new MultibusLINChannel.ProtocolVersionComboboxItem(MultibusLINChannel.LinProtocolVersion.LIN_1_3));
			this.comboBoxProtocolVersion.Items.Add(new MultibusLINChannel.ProtocolVersionComboboxItem(MultibusLINChannel.LinProtocolVersion.LIN_2_0));
			this.comboBoxProtocolVersion.Items.Add(new MultibusLINChannel.ProtocolVersionComboboxItem(MultibusLINChannel.LinProtocolVersion.LIN_2_1));
			this.comboBoxProtocolVersion.Items.Add(new MultibusLINChannel.ProtocolVersionComboboxItem(MultibusLINChannel.LinProtocolVersion.LIN_2_2));
			this.comboBoxProtocolVersion.SelectedIndex = 0;
		}

		private void UpdateGUI()
		{
			if (this.channel == null || this.ChannelNr < 1u)
			{
				return;
			}
			this.isInitControls = true;
			BaudrateComboboxItem.Select(this.comboBoxSpeedRate, this.channel.SpeedRate.Value);
			MultibusLINChannel.ProtocolVersionComboboxItem.Select(this.comboBoxProtocolVersion, this.channel.ProtocolVersion.Value);
			this.checkBoxUseDbValues.Checked = this.channel.UseDbConfigValues.Value;
			this.DisplayBaudrateFromDb();
			this.comboBoxSpeedRate.Enabled = !this.checkBoxUseDbValues.Checked;
			this.comboBoxProtocolVersion.Enabled = !this.checkBoxUseDbValues.Checked;
			this.isInitControls = false;
		}

		private void DisplayBaudrateFromDb()
		{
			int num;
			string arg;
			int num2;
			bool linChipConfigFromLdfDatabase = this.ModelValidator.DatabaseServices.GetLinChipConfigFromLdfDatabase(this.ChannelNr, out num, out arg, out num2);
			this.checkBoxUseDbValues.Text = (linChipConfigFromLdfDatabase ? string.Format(Resources.LINChannelDbConfigValues, num, arg) : string.Format(Resources.LINChannelDbConfigValues, "-", "-"));
		}

		private void control_DataChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.validationHost.ValidateInput();
		}

		bool IValidatable.ValidateInput(ref bool valueChanged)
		{
			if (this.channel == null)
			{
				return true;
			}
			bool flag = true;
			this.DisplayBaudrateFromDb();
			bool flag2;
			flag &= this.PageValidatorControl.UpdateModel<uint>(BaudrateComboboxItem.GetSelected(this.comboBoxSpeedRate), this.channel.SpeedRate, this.GUIElementManager.GetGUIElement(this.comboBoxSpeedRate), out flag2);
			valueChanged |= flag2;
			flag &= this.PageValidatorControl.UpdateModel<int>(MultibusLINChannel.ProtocolVersionComboboxItem.GetSelected(this.comboBoxProtocolVersion), this.channel.ProtocolVersion, this.GUIElementManager.GetGUIElement(this.comboBoxProtocolVersion), out flag2);
			valueChanged |= flag2;
			flag &= this.PageValidatorControl.UpdateModel<bool>(this.checkBoxUseDbValues.Checked, this.channel.UseDbConfigValues, this.GUIElementManager.GetGUIElement(this.checkBoxUseDbValues), out flag2);
			valueChanged |= flag2;
			return flag;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MultibusLINChannel));
			this.comboBoxSpeedRate = new ComboBox();
			this.comboBoxProtocolVersion = new ComboBox();
			this.checkBoxUseDbValues = new CheckBox();
			this.labelProt = new Label();
			base.SuspendLayout();
			this.comboBoxSpeedRate.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxSpeedRate.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxSpeedRate, "comboBoxSpeedRate");
			this.comboBoxSpeedRate.Name = "comboBoxSpeedRate";
			this.comboBoxSpeedRate.SelectedIndexChanged += new EventHandler(this.control_DataChanged);
			this.comboBoxProtocolVersion.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxProtocolVersion.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxProtocolVersion, "comboBoxProtocolVersion");
			this.comboBoxProtocolVersion.Name = "comboBoxProtocolVersion";
			this.comboBoxProtocolVersion.SelectedIndexChanged += new EventHandler(this.control_DataChanged);
			componentResourceManager.ApplyResources(this.checkBoxUseDbValues, "checkBoxUseDbValues");
			this.checkBoxUseDbValues.Name = "checkBoxUseDbValues";
			this.checkBoxUseDbValues.UseVisualStyleBackColor = true;
			this.checkBoxUseDbValues.CheckedChanged += new EventHandler(this.control_DataChanged);
			componentResourceManager.ApplyResources(this.labelProt, "labelProt");
			this.labelProt.Name = "labelProt";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.labelProt);
			base.Controls.Add(this.checkBoxUseDbValues);
			base.Controls.Add(this.comboBoxProtocolVersion);
			base.Controls.Add(this.comboBoxSpeedRate);
			base.Name = "MultibusLINChannel";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
