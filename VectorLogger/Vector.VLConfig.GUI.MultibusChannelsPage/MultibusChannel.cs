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
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.MultibusChannelsPage
{
	internal class MultibusChannel : UserControl, IValidatable
	{
		private class BusTypeComboboxItem
		{
			private readonly BusType busType;

			public BusTypeComboboxItem(BusType busType)
			{
				this.busType = busType;
			}

			public override string ToString()
			{
				return GUIUtil.MapMultibusType2String(this.busType);
			}

			public static void Select(ComboBox comboBox, BusType busType)
			{
				Trace.Assert(comboBox != null);
				MultibusChannel.BusTypeComboboxItem busTypeComboboxItem = comboBox.Items.OfType<MultibusChannel.BusTypeComboboxItem>().FirstOrDefault((MultibusChannel.BusTypeComboboxItem t) => t.busType == busType);
				if (busTypeComboboxItem != null)
				{
					comboBox.SelectedItem = busTypeComboboxItem;
				}
			}

			public static BusType GetSelected(ComboBox comboBox)
			{
				Trace.Assert(comboBox != null);
				MultibusChannel.BusTypeComboboxItem busTypeComboboxItem = comboBox.SelectedItem as MultibusChannel.BusTypeComboboxItem;
				Trace.Assert(busTypeComboboxItem != null);
				return busTypeComboboxItem.busType;
			}
		}

		private uint channelNr;

		private readonly List<BusType> possibleBusTypes;

		private MultibusChannelConfiguration multibusChannelConfiguration;

		private IModelValidator modelValidator;

		private IValidationHost validationHost;

		private bool isInitControls;

		private IContainer components;

		private CheckBox checkBoxActive;

		private ComboBox comboBoxBusType;

		private MultibusCANChannel multibusCANChannel;

		private MultibusJ1708Channel multibusJ1708Channel;

		private MultibusLINChannel multibusLINChannel;

		public uint ChannelNr
		{
			get
			{
				return this.channelNr;
			}
			set
			{
				this.channelNr = value;
				this.checkBoxActive.Text = string.Format(Resources.ChannelX, this.channelNr);
				this.multibusCANChannel.ChannelNr = value;
				this.multibusJ1708Channel.ChannelNr = value;
				this.multibusLINChannel.ChannelNr = value;
			}
		}

		public bool SupportsCANFD
		{
			get
			{
				return this.multibusCANChannel.SupportsCANFD;
			}
			set
			{
				this.multibusCANChannel.SupportsCANFD = value;
			}
		}

		public MultibusChannelConfiguration MultibusChannelConfiguration
		{
			get
			{
				return this.multibusChannelConfiguration;
			}
			set
			{
				this.multibusChannelConfiguration = value;
				HardwareChannel hardwareChannel = (this.multibusChannelConfiguration != null) ? this.multibusChannelConfiguration.GetChannel(this.channelNr) : null;
				this.multibusCANChannel.Channel = (hardwareChannel as CANChannel);
				this.multibusJ1708Channel.Channel = (hardwareChannel as J1708Channel);
				this.multibusLINChannel.Channel = (hardwareChannel as LINChannel);
				this.UpdateGUI();
			}
		}

		public IModelValidator ModelValidator
		{
			get
			{
				return this.modelValidator;
			}
			set
			{
				this.modelValidator = value;
				this.multibusCANChannel.ModelValidator = value;
				this.multibusJ1708Channel.ModelValidator = value;
				this.multibusLINChannel.ModelValidator = value;
			}
		}

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.multibusLINChannel.ApplicationDatabaseManager;
			}
			set
			{
				this.multibusLINChannel.ApplicationDatabaseManager = value;
			}
		}

		public IHardwareFrontend HardwareFrontend
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
					this.validationHost.RegisterForErrorProvider(this.comboBoxBusType);
					this.validationHost.RegisterForErrorProvider(this.checkBoxActive);
				}
				((IValidatable)this.multibusCANChannel).ValidationHost = value;
				((IValidatable)this.multibusJ1708Channel).ValidationHost = value;
				((IValidatable)this.multibusLINChannel).ValidationHost = value;
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

		public MultibusChannel()
		{
			this.possibleBusTypes = new List<BusType>();
			this.InitializeComponent();
		}

		private void InitComboboxBusTypes()
		{
			this.isInitControls = true;
			this.comboBoxBusType.Items.Clear();
			foreach (BusType current in this.possibleBusTypes)
			{
				this.comboBoxBusType.Items.Add(new MultibusChannel.BusTypeComboboxItem(current));
			}
			if (this.comboBoxBusType.Items.Count > 0)
			{
				this.comboBoxBusType.SelectedIndex = 0;
			}
			this.isInitControls = false;
		}

		public void SetPossibleBusTypes(IEnumerable<BusType> busTypes)
		{
			this.possibleBusTypes.Clear();
			if (busTypes != null)
			{
				this.possibleBusTypes.AddRange(busTypes);
			}
			this.InitComboboxBusTypes();
			this.UpdateGUI();
		}

		bool IValidatable.ValidateInput(ref bool valueChanged)
		{
			bool flag = true;
			bool flag2 = false;
			flag &= this.PageValidatorControl.UpdateModel<bool>(this.checkBoxActive.Checked, this.multibusChannelConfiguration.GetChannel(this.channelNr).IsActive, this.GUIElementManager.GetGUIElement(this.checkBoxActive), out flag2);
			valueChanged |= flag2;
			if (!this.multibusChannelConfiguration.GetChannel(this.channelNr).IsActive.Value)
			{
				return flag;
			}
			bool flag3 = false;
			HardwareChannel channel = this.multibusChannelConfiguration.GetChannel(this.channelNr);
			BusType selected = MultibusChannel.BusTypeComboboxItem.GetSelected(this.comboBoxBusType);
			if (selected == BusType.Bt_CAN && !(channel is CANChannel))
			{
				this.multibusChannelConfiguration.SetChannelBusType(this.channelNr, BusType.Bt_CAN);
				flag3 = true;
			}
			else if (selected == BusType.Bt_LIN && !(channel is LINChannel))
			{
				this.multibusChannelConfiguration.SetChannelBusType(this.channelNr, BusType.Bt_LIN);
				flag3 = true;
			}
			else if (selected == BusType.Bt_J1708 && !(channel is J1708Channel))
			{
				this.multibusChannelConfiguration.SetChannelBusType(this.channelNr, BusType.Bt_J1708);
				flag3 = true;
			}
			if (flag3)
			{
				HardwareChannel channel2 = this.multibusChannelConfiguration.GetChannel(this.channelNr);
				ModelEditor.InitializeMultibusChannel(this.channelNr, ref channel2);
				valueChanged = true;
				if (this.HardwareFrontend.PrimaryOnlineDevice != null)
				{
					this.HardwareFrontend.PrimaryOnlineDevice.Update();
				}
			}
			channel = this.multibusChannelConfiguration.GetChannel(this.channelNr);
			bool flag4;
			this.PageValidatorControl.UpdateModel<BusType>(channel.BusType.Value, channel.BusType, this.GUIElementManager.GetGUIElement(this.comboBoxBusType), out flag4);
			IValidatable activeChild = this.GetActiveChild();
			if (activeChild != null)
			{
				flag &= activeChild.ValidateInput(ref valueChanged);
			}
			return flag;
		}

		private IValidatable GetActiveChild()
		{
			BusType selected = MultibusChannel.BusTypeComboboxItem.GetSelected(this.comboBoxBusType);
			if (selected == BusType.Bt_CAN)
			{
				return this.multibusCANChannel;
			}
			if (selected == BusType.Bt_J1708)
			{
				return this.multibusJ1708Channel;
			}
			if (selected == BusType.Bt_LIN)
			{
				return this.multibusLINChannel;
			}
			return null;
		}

		private void UpdateGUI()
		{
			this.isInitControls = true;
			HardwareChannel hardwareChannel = (this.multibusChannelConfiguration != null) ? this.multibusChannelConfiguration.GetChannel(this.channelNr) : null;
			this.multibusCANChannel.Visible = (this.multibusCANChannel.Channel != null);
			this.multibusJ1708Channel.Visible = (this.multibusJ1708Channel.Channel != null);
			this.multibusLINChannel.Visible = (this.multibusLINChannel.Channel != null);
			bool flag = hardwareChannel != null && hardwareChannel.IsActive.Value;
			this.checkBoxActive.Checked = flag;
			this.comboBoxBusType.Enabled = flag;
			this.multibusCANChannel.Enabled = flag;
			this.multibusJ1708Channel.Enabled = flag;
			this.multibusLINChannel.Enabled = flag;
			MultibusChannel.BusTypeComboboxItem.Select(this.comboBoxBusType, this.GetBusTypeFromHwChannel(hardwareChannel));
			this.isInitControls = false;
		}

		private void checkBoxActive_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.validationHost.ValidateInput();
		}

		private void comboBoxBusType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.validationHost.ValidateInput();
		}

		private BusType GetBusTypeFromHwChannel(HardwareChannel hwChannel)
		{
			if (hwChannel is CANChannel)
			{
				return BusType.Bt_CAN;
			}
			if (hwChannel is J1708Channel)
			{
				return BusType.Bt_J1708;
			}
			if (hwChannel is LINChannel)
			{
				return BusType.Bt_LIN;
			}
			return BusType.Bt_None;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MultibusChannel));
			this.checkBoxActive = new CheckBox();
			this.comboBoxBusType = new ComboBox();
			this.multibusCANChannel = new MultibusCANChannel();
			this.multibusLINChannel = new MultibusLINChannel();
			this.multibusJ1708Channel = new MultibusJ1708Channel();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.checkBoxActive, "checkBoxActive");
			this.checkBoxActive.Name = "checkBoxActive";
			this.checkBoxActive.UseVisualStyleBackColor = true;
			this.checkBoxActive.CheckedChanged += new EventHandler(this.checkBoxActive_CheckedChanged);
			this.comboBoxBusType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBusType.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxBusType, "comboBoxBusType");
			this.comboBoxBusType.Name = "comboBoxBusType";
			this.comboBoxBusType.SelectedIndexChanged += new EventHandler(this.comboBoxBusType_SelectedIndexChanged);
			this.multibusCANChannel.Channel = null;
			this.multibusCANChannel.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.multibusCANChannel, "multibusCANChannel");
			this.multibusCANChannel.ModelValidator = null;
			this.multibusCANChannel.Name = "multibusCANChannel";
			this.multibusCANChannel.SupportsCANFD = false;
			this.multibusLINChannel.ApplicationDatabaseManager = null;
			this.multibusLINChannel.Channel = null;
			this.multibusLINChannel.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.multibusLINChannel, "multibusLINChannel");
			this.multibusLINChannel.ModelValidator = null;
			this.multibusLINChannel.Name = "multibusLINChannel";
			this.multibusJ1708Channel.Channel = null;
			this.multibusJ1708Channel.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.multibusJ1708Channel, "multibusJ1708Channel");
			this.multibusJ1708Channel.ModelValidator = null;
			this.multibusJ1708Channel.Name = "multibusJ1708Channel";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.multibusCANChannel);
			base.Controls.Add(this.multibusLINChannel);
			base.Controls.Add(this.checkBoxActive);
			base.Controls.Add(this.comboBoxBusType);
			base.Controls.Add(this.multibusJ1708Channel);
			base.Name = "MultibusChannel";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
