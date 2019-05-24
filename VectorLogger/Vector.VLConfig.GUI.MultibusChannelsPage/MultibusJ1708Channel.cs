using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.MultibusChannelsPage
{
	internal class MultibusJ1708Channel : UserControl, IValidatable
	{
		private class BitTimeComboboxItem
		{
			private readonly uint bitTime;

			public BitTimeComboboxItem(uint bitTime)
			{
				this.bitTime = bitTime;
			}

			public override string ToString()
			{
				return string.Format(Resources.NumBits, this.bitTime);
			}

			public static void Select(ComboBox comboBox, uint bitTime)
			{
				Trace.Assert(comboBox != null);
				MultibusJ1708Channel.BitTimeComboboxItem bitTimeComboboxItem = comboBox.Items.OfType<MultibusJ1708Channel.BitTimeComboboxItem>().FirstOrDefault((MultibusJ1708Channel.BitTimeComboboxItem t) => t.bitTime == bitTime);
				if (bitTimeComboboxItem != null)
				{
					comboBox.SelectedItem = bitTimeComboboxItem;
				}
			}

			public static uint GetSelected(ComboBox comboBox)
			{
				Trace.Assert(comboBox != null);
				MultibusJ1708Channel.BitTimeComboboxItem bitTimeComboboxItem = comboBox.SelectedItem as MultibusJ1708Channel.BitTimeComboboxItem;
				Trace.Assert(bitTimeComboboxItem != null);
				return bitTimeComboboxItem.bitTime;
			}
		}

		private J1708Channel channel;

		private IValidationHost validationHost;

		private bool isInitControls;

		private IContainer components;

		private ComboBox comboBoxSpeedRate;

		private Label labelMaxInterCharBitTime;

		private ComboBox comboBoxMaxInterCharBitTime;

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public uint ChannelNr
		{
			get;
			set;
		}

		public J1708Channel Channel
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

		public MultibusJ1708Channel()
		{
			this.InitializeComponent();
			this.isInitControls = true;
			this.InitSpeedRateComboBox();
			this.InitMaxInterCharBitTimeComboBoxes();
			this.isInitControls = false;
		}

		private void InitSpeedRateComboBox()
		{
			this.comboBoxSpeedRate.Items.Clear();
			IList<uint> standardJ1708Baudrates = GUIUtil.GetStandardJ1708Baudrates();
			foreach (uint current in standardJ1708Baudrates)
			{
				this.comboBoxSpeedRate.Items.Add(new BaudrateComboboxItem(current));
			}
			this.comboBoxSpeedRate.SelectedIndex = 0;
		}

		private void InitMaxInterCharBitTimeComboBoxes()
		{
			this.comboBoxMaxInterCharBitTime.Items.Clear();
			foreach (uint current in GUIUtil.GetJ1708MaxInterCharBitTimes())
			{
				this.comboBoxMaxInterCharBitTime.Items.Add(new MultibusJ1708Channel.BitTimeComboboxItem(current));
			}
			this.comboBoxMaxInterCharBitTime.SelectedIndex = 0;
		}

		private void UpdateGUI()
		{
			if (this.channel == null || this.ChannelNr < 1u)
			{
				return;
			}
			this.isInitControls = true;
			BaudrateComboboxItem.Select(this.comboBoxSpeedRate, this.channel.SpeedRate.Value);
			MultibusJ1708Channel.BitTimeComboboxItem.Select(this.comboBoxMaxInterCharBitTime, this.channel.MaxInterCharBitTime.Value);
			this.isInitControls = false;
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
			bool flag2;
			flag &= this.PageValidatorControl.UpdateModel<uint>(BaudrateComboboxItem.GetSelected(this.comboBoxSpeedRate), this.channel.SpeedRate, this.GUIElementManager.GetGUIElement(this.comboBoxSpeedRate), out flag2);
			valueChanged |= flag2;
			flag &= this.PageValidatorControl.UpdateModel<uint>(MultibusJ1708Channel.BitTimeComboboxItem.GetSelected(this.comboBoxMaxInterCharBitTime), this.channel.MaxInterCharBitTime, this.GUIElementManager.GetGUIElement(this.comboBoxMaxInterCharBitTime), out flag2);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MultibusJ1708Channel));
			this.comboBoxSpeedRate = new ComboBox();
			this.labelMaxInterCharBitTime = new Label();
			this.comboBoxMaxInterCharBitTime = new ComboBox();
			base.SuspendLayout();
			this.comboBoxSpeedRate.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxSpeedRate.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxSpeedRate, "comboBoxSpeedRate");
			this.comboBoxSpeedRate.Name = "comboBoxSpeedRate";
			this.comboBoxSpeedRate.SelectedIndexChanged += new EventHandler(this.control_DataChanged);
			componentResourceManager.ApplyResources(this.labelMaxInterCharBitTime, "labelMaxInterCharBitTime");
			this.labelMaxInterCharBitTime.Name = "labelMaxInterCharBitTime";
			this.comboBoxMaxInterCharBitTime.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxMaxInterCharBitTime.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxMaxInterCharBitTime, "comboBoxMaxInterCharBitTime");
			this.comboBoxMaxInterCharBitTime.Name = "comboBoxMaxInterCharBitTime";
			this.comboBoxMaxInterCharBitTime.SelectedIndexChanged += new EventHandler(this.control_DataChanged);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.comboBoxMaxInterCharBitTime);
			base.Controls.Add(this.labelMaxInterCharBitTime);
			base.Controls.Add(this.comboBoxSpeedRate);
			base.Name = "MultibusJ1708Channel";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
