using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.DeviceInteraction
{
	public class SetEthernetAddress : Form
	{
		private uint MaxIPValue = 255u;

		private string errorTextValueOutOfRange;

		private string errorTextIsDefaultAddress;

		private uint ipValue1;

		private uint ipValue2;

		private uint ipValue3;

		private uint ipValue4;

		private uint snMaskValue1;

		private uint snMaskValue2;

		private uint snMaskValue3;

		private uint snMaskValue4;

		private uint defaultIpValue1;

		private uint defaultIpValue2;

		private uint defaultIpValue3;

		private uint defaultIpValue4;

		private uint defaultSnMaskValue1;

		private uint defaultSnMaskValue2;

		private uint defaultSnMaskValue3;

		private uint defaultSnMaskValue4;

		private IContainer components;

		private GroupBox groupBoxAddress;

		private Button buttonSet;

		private Button buttonGet;

		private Button buttonOK;

		private ErrorProvider errorProvider;

		private ComboBox comboBoxPorts;

		private Label labelPort;

		private MaskedTextBox maskedTextBoxIPAddress;

		private GroupBox groupBoxConnection;

		private Button buttonHelp;

		private GroupBox groupBoxHint;

		private Label labelHint;

		private MaskedTextBox maskedTextBoxSubnetMask;

		private Label labelIpAddress;

		private Label labelSubnetMask;

		private Label labelDefaultSubnetMask;

		private Label labelDefaultIpAddress;

		private MaskedTextBox maskedTextBoxDefaultSubnetMask;

		private MaskedTextBox maskedTextBoxDefaultIPAddress;

		public string SelectedCOMPort
		{
			get;
			set;
		}

		public SetEthernetAddress()
		{
			this.InitializeComponent();
			this.errorTextValueOutOfRange = string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, 0, this.MaxIPValue);
			this.errorTextIsDefaultAddress = Resources.EthernetAddressMustDifferFromDefault;
			this.ResetErrorProvider();
		}

		private void buttonGet_Click(object sender, EventArgs e)
		{
			string port = this.comboBoxPorts.SelectedItem.ToString();
			new SerialCommandProgress(Resources.TitleReadIPAddress, port, "")
			{
				SerialCommandProcessingMethod = new ThreadStart(SerialPortServices.ReadIPAddress),
				ProcessingTimeout = SerialPortServices.CommunicationTimeout
			}.ShowDialog();
			if (SerialPortServices.IsFinished)
			{
				this.ResetErrorProvider();
				SerialPortServices.GetIpAddressBytes(SerialPortServices.IPAddress, out this.ipValue1, out this.ipValue2, out this.ipValue3, out this.ipValue4);
				this.SetIpValues(this.ipValue1, this.ipValue2, this.ipValue3, this.ipValue4);
				SerialPortServices.GetIpAddressBytes(SerialPortServices.SubnetMask, out this.snMaskValue1, out this.snMaskValue2, out this.snMaskValue3, out this.snMaskValue4);
				this.SetSubnetMaskValues(this.snMaskValue1, this.snMaskValue2, this.snMaskValue3, this.snMaskValue4);
				SerialPortServices.GetIpAddressBytes(SerialPortServices.DefaultIPAddress, out this.defaultIpValue1, out this.defaultIpValue2, out this.defaultIpValue3, out this.defaultIpValue4);
				this.SetDefaultIpValues(this.defaultIpValue1, this.defaultIpValue2, this.defaultIpValue3, this.defaultIpValue4);
				SerialPortServices.GetIpAddressBytes(SerialPortServices.DefaultSubnetMask, out this.defaultSnMaskValue1, out this.defaultSnMaskValue2, out this.defaultSnMaskValue3, out this.defaultSnMaskValue4);
				this.SetDefaultSubnetMaskValues(this.defaultSnMaskValue1, this.defaultSnMaskValue2, this.defaultSnMaskValue3, this.defaultSnMaskValue4);
				Color backColor = this.maskedTextBoxIPAddress.BackColor;
				this.maskedTextBoxIPAddress.BackColor = SystemColors.Highlight;
				this.maskedTextBoxSubnetMask.BackColor = SystemColors.Highlight;
				Thread.Sleep(1000);
				this.maskedTextBoxIPAddress.BackColor = backColor;
				this.maskedTextBoxSubnetMask.BackColor = backColor;
			}
		}

		private void buttonSet_Click(object sender, EventArgs e)
		{
			if (this.ValidateInput())
			{
				string port = this.comboBoxPorts.SelectedItem.ToString();
				SerialCommandProgress serialCommandProgress = new SerialCommandProgress(Resources.TitleSetIPAddress, port, Resources.IPConfigSetSuccess);
				SerialPortServices.IPAddress = SerialPortServices.BuildCompleteIpAddress(this.ipValue1, this.ipValue2, this.ipValue3, this.ipValue4);
				SerialPortServices.SubnetMask = SerialPortServices.BuildCompleteIpAddress(this.snMaskValue1, this.snMaskValue2, this.snMaskValue3, this.snMaskValue4);
				uint arg_76_0 = SerialPortServices.SubnetMask;
				serialCommandProgress.SerialCommandProcessingMethod = new ThreadStart(SerialPortServices.SetIPAddress);
				serialCommandProgress.ProcessingTimeout = SerialPortServices.CommunicationTimeoutSetEthernet;
				serialCommandProgress.ShowDialog();
				SerialPortServices.GetIpAddressBytes(SerialPortServices.DefaultIPAddress, out this.defaultIpValue1, out this.defaultIpValue2, out this.defaultIpValue3, out this.defaultIpValue4);
				this.SetDefaultIpValues(this.defaultIpValue1, this.defaultIpValue2, this.defaultIpValue3, this.defaultIpValue4);
				SerialPortServices.GetIpAddressBytes(SerialPortServices.DefaultSubnetMask, out this.defaultSnMaskValue1, out this.defaultSnMaskValue2, out this.defaultSnMaskValue3, out this.defaultSnMaskValue4);
				this.SetDefaultSubnetMaskValues(this.defaultSnMaskValue1, this.defaultSnMaskValue2, this.defaultSnMaskValue3, this.defaultSnMaskValue4);
				this.ValidateInput();
				return;
			}
			InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			this.ResetErrorProvider();
		}

		private void SetEthernetAddress_Shown(object sender, EventArgs e)
		{
			Cursor current = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			this.comboBoxPorts.Items.Clear();
			string[] cOMPortNames = SerialPortServices.GetCOMPortNames();
			for (int i = 0; i < cOMPortNames.Length; i++)
			{
				string item = cOMPortNames[i];
				this.comboBoxPorts.Items.Add(item);
			}
			if (this.comboBoxPorts.Items.Count > 0)
			{
				if (!string.IsNullOrEmpty(this.SelectedCOMPort) && this.comboBoxPorts.Items.Contains(this.SelectedCOMPort))
				{
					this.comboBoxPorts.SelectedItem = this.SelectedCOMPort;
				}
				else
				{
					this.comboBoxPorts.SelectedIndex = 0;
					this.SelectedCOMPort = this.comboBoxPorts.SelectedItem.ToString();
				}
			}
			this.buttonGet.Enabled = (this.comboBoxPorts.Items.Count > 0);
			this.buttonSet.Enabled = (this.comboBoxPorts.Items.Count > 0);
			Cursor.Current = current;
			Application.DoEvents();
		}

		private void maskedTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (e.KeyCode == Keys.Decimal && maskedTextBox != null && maskedTextBox.MaskedTextProvider != null)
			{
				int selectionStart = maskedTextBox.SelectionStart;
				int num = maskedTextBox.MaskedTextProvider.Length - maskedTextBox.MaskedTextProvider.EditPositionCount;
				int num2 = 0;
				for (int i = 0; i < maskedTextBox.MaskedTextProvider.Length; i++)
				{
					if (!maskedTextBox.MaskedTextProvider.IsEditPosition(i) && selectionStart + num >= i)
					{
						num2 = i;
					}
				}
				num2++;
				maskedTextBox.SelectionStart = num2;
			}
		}

		private void maskedTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (e.KeyChar == '.' && maskedTextBox != null && maskedTextBox.MaskedTextProvider != null)
			{
				int selectionStart = maskedTextBox.SelectionStart;
				int num = maskedTextBox.MaskedTextProvider.Length - maskedTextBox.MaskedTextProvider.EditPositionCount;
				int num2 = 0;
				for (int i = 0; i < maskedTextBox.MaskedTextProvider.Length; i++)
				{
					if (!maskedTextBox.MaskedTextProvider.IsEditPosition(i) && selectionStart + num >= i)
					{
						num2 = i;
					}
				}
				num2++;
				maskedTextBox.SelectionStart = num2;
			}
		}

		private void maskedTextBox_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateInput();
		}

		private void SetEthernetAddress_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void comboBoxPorts_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.SelectedCOMPort = this.comboBoxPorts.SelectedItem.ToString();
		}

		private bool ValidateInput()
		{
			bool result = true;
			bool flag = true;
			bool flag2 = false;
			this.ResetErrorProvider();
			string[] array = this.maskedTextBoxIPAddress.Text.Split(new char[]
			{
				'.'
			});
			if (array.Length < 4)
			{
				flag = false;
			}
			else
			{
				if (!SetEthernetAddress.ValidateByteValue(array[0], ref this.ipValue1))
				{
					flag = false;
				}
				if (!SetEthernetAddress.ValidateByteValue(array[1], ref this.ipValue2))
				{
					flag = false;
				}
				if (!SetEthernetAddress.ValidateByteValue(array[2], ref this.ipValue3))
				{
					flag = false;
				}
				if (!SetEthernetAddress.ValidateByteValue(array[3], ref this.ipValue4))
				{
					flag = false;
				}
				if (this.ipValue1 == this.defaultIpValue1 && this.ipValue2 == this.defaultIpValue2 && this.ipValue3 == this.defaultIpValue3 && this.ipValue4 == this.defaultIpValue4 && this.defaultIpValue1 != 0u && this.defaultIpValue2 != 0u && this.defaultIpValue3 != 0u && this.defaultIpValue4 != 0u)
				{
					flag = false;
					flag2 = true;
				}
			}
			if (!flag)
			{
				if (flag2)
				{
					this.errorProvider.SetError(this.maskedTextBoxIPAddress, this.errorTextIsDefaultAddress);
				}
				else
				{
					this.errorProvider.SetError(this.maskedTextBoxIPAddress, this.errorTextValueOutOfRange);
				}
				result = false;
			}
			flag = true;
			array = this.maskedTextBoxSubnetMask.Text.Split(new char[]
			{
				'.'
			});
			if (array.Length < 4)
			{
				flag = false;
			}
			else
			{
				if (!SetEthernetAddress.ValidateByteValue(array[0], ref this.snMaskValue1))
				{
					flag = false;
				}
				if (!SetEthernetAddress.ValidateByteValue(array[1], ref this.snMaskValue2))
				{
					flag = false;
				}
				if (!SetEthernetAddress.ValidateByteValue(array[2], ref this.snMaskValue3))
				{
					flag = false;
				}
				if (!SetEthernetAddress.ValidateByteValue(array[3], ref this.snMaskValue4))
				{
					flag = false;
				}
			}
			if (!flag)
			{
				this.errorProvider.SetError(this.maskedTextBoxSubnetMask, this.errorTextValueOutOfRange);
				result = false;
			}
			else if (!SerialPortServices.IsValidSubnetMask(this.snMaskValue1, this.snMaskValue2, this.snMaskValue3, this.snMaskValue4))
			{
				this.errorProvider.SetError(this.maskedTextBoxSubnetMask, Resources.ErrorInvalidSubnetMask);
				result = false;
			}
			return result;
		}

		private static bool ValidateByteValue(string textValue, ref uint value)
		{
			return uint.TryParse(textValue, out value) && value <= 255u;
		}

		private void ResetErrorProvider()
		{
			this.errorProvider.SetError(this.maskedTextBoxIPAddress, "");
			this.errorProvider.SetError(this.maskedTextBoxSubnetMask, "");
		}

		private void SetIpValues(uint value1, uint value2, uint value3, uint value4)
		{
			this.ipValue1 = value1;
			this.ipValue2 = value2;
			this.ipValue3 = value3;
			this.ipValue4 = value4;
			string text = string.Format("{0,3}{1,3}{2,3}{3,3}", new object[]
			{
				value1,
				value2,
				value3,
				value4
			});
			this.maskedTextBoxIPAddress.Text = text;
		}

		private void SetSubnetMaskValues(uint value1, uint value2, uint value3, uint value4)
		{
			this.snMaskValue1 = value1;
			this.snMaskValue2 = value2;
			this.snMaskValue3 = value3;
			this.snMaskValue4 = value4;
			string text = string.Format("{0,3}{1,3}{2,3}{3,3}", new object[]
			{
				value1,
				value2,
				value3,
				value4
			});
			this.maskedTextBoxSubnetMask.Text = text;
		}

		private void SetDefaultIpValues(uint value1, uint value2, uint value3, uint value4)
		{
			this.defaultIpValue1 = value1;
			this.defaultIpValue2 = value2;
			this.defaultIpValue3 = value3;
			this.defaultIpValue4 = value4;
			string text = string.Format("{0,3}{1,3}{2,3}{3,3}", new object[]
			{
				value1,
				value2,
				value3,
				value4
			});
			this.maskedTextBoxDefaultIPAddress.Text = text;
		}

		private void SetDefaultSubnetMaskValues(uint value1, uint value2, uint value3, uint value4)
		{
			this.defaultSnMaskValue1 = value1;
			this.defaultSnMaskValue2 = value2;
			this.defaultSnMaskValue3 = value3;
			this.defaultSnMaskValue4 = value4;
			string text = string.Format("{0,3}{1,3}{2,3}{3,3}", new object[]
			{
				value1,
				value2,
				value3,
				value4
			});
			this.maskedTextBoxDefaultSubnetMask.Text = text;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SetEthernetAddress));
			this.groupBoxAddress = new GroupBox();
			this.labelDefaultSubnetMask = new Label();
			this.labelDefaultIpAddress = new Label();
			this.maskedTextBoxDefaultSubnetMask = new MaskedTextBox();
			this.maskedTextBoxDefaultIPAddress = new MaskedTextBox();
			this.labelSubnetMask = new Label();
			this.labelIpAddress = new Label();
			this.maskedTextBoxSubnetMask = new MaskedTextBox();
			this.maskedTextBoxIPAddress = new MaskedTextBox();
			this.buttonSet = new Button();
			this.buttonGet = new Button();
			this.buttonOK = new Button();
			this.errorProvider = new ErrorProvider(this.components);
			this.labelPort = new Label();
			this.comboBoxPorts = new ComboBox();
			this.groupBoxConnection = new GroupBox();
			this.buttonHelp = new Button();
			this.groupBoxHint = new GroupBox();
			this.labelHint = new Label();
			this.groupBoxAddress.SuspendLayout();
			((ISupportInitialize)this.errorProvider).BeginInit();
			this.groupBoxConnection.SuspendLayout();
			this.groupBoxHint.SuspendLayout();
			base.SuspendLayout();
			this.groupBoxAddress.Controls.Add(this.labelDefaultSubnetMask);
			this.groupBoxAddress.Controls.Add(this.labelDefaultIpAddress);
			this.groupBoxAddress.Controls.Add(this.maskedTextBoxDefaultSubnetMask);
			this.groupBoxAddress.Controls.Add(this.maskedTextBoxDefaultIPAddress);
			this.groupBoxAddress.Controls.Add(this.labelSubnetMask);
			this.groupBoxAddress.Controls.Add(this.labelIpAddress);
			this.groupBoxAddress.Controls.Add(this.maskedTextBoxSubnetMask);
			this.groupBoxAddress.Controls.Add(this.maskedTextBoxIPAddress);
			this.groupBoxAddress.Controls.Add(this.buttonSet);
			this.groupBoxAddress.Controls.Add(this.buttonGet);
			componentResourceManager.ApplyResources(this.groupBoxAddress, "groupBoxAddress");
			this.groupBoxAddress.Name = "groupBoxAddress";
			this.groupBoxAddress.TabStop = false;
			componentResourceManager.ApplyResources(this.labelDefaultSubnetMask, "labelDefaultSubnetMask");
			this.labelDefaultSubnetMask.Name = "labelDefaultSubnetMask";
			componentResourceManager.ApplyResources(this.labelDefaultIpAddress, "labelDefaultIpAddress");
			this.labelDefaultIpAddress.Name = "labelDefaultIpAddress";
			this.maskedTextBoxDefaultSubnetMask.Culture = new CultureInfo("");
			this.errorProvider.SetIconAlignment(this.maskedTextBoxDefaultSubnetMask, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxDefaultSubnetMask.IconAlignment"));
			componentResourceManager.ApplyResources(this.maskedTextBoxDefaultSubnetMask, "maskedTextBoxDefaultSubnetMask");
			this.maskedTextBoxDefaultSubnetMask.Name = "maskedTextBoxDefaultSubnetMask";
			this.maskedTextBoxDefaultSubnetMask.ReadOnly = true;
			this.maskedTextBoxDefaultIPAddress.Culture = new CultureInfo("");
			this.errorProvider.SetIconAlignment(this.maskedTextBoxDefaultIPAddress, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxDefaultIPAddress.IconAlignment"));
			componentResourceManager.ApplyResources(this.maskedTextBoxDefaultIPAddress, "maskedTextBoxDefaultIPAddress");
			this.maskedTextBoxDefaultIPAddress.Name = "maskedTextBoxDefaultIPAddress";
			this.maskedTextBoxDefaultIPAddress.ReadOnly = true;
			this.maskedTextBoxDefaultIPAddress.ResetOnSpace = false;
			componentResourceManager.ApplyResources(this.labelSubnetMask, "labelSubnetMask");
			this.labelSubnetMask.Name = "labelSubnetMask";
			componentResourceManager.ApplyResources(this.labelIpAddress, "labelIpAddress");
			this.labelIpAddress.Name = "labelIpAddress";
			this.maskedTextBoxSubnetMask.Culture = new CultureInfo("");
			this.errorProvider.SetIconAlignment(this.maskedTextBoxSubnetMask, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxSubnetMask.IconAlignment"));
			componentResourceManager.ApplyResources(this.maskedTextBoxSubnetMask, "maskedTextBoxSubnetMask");
			this.maskedTextBoxSubnetMask.Name = "maskedTextBoxSubnetMask";
			this.maskedTextBoxSubnetMask.KeyDown += new KeyEventHandler(this.maskedTextBox_KeyDown);
			this.maskedTextBoxSubnetMask.KeyPress += new KeyPressEventHandler(this.maskedTextBox_KeyPress);
			this.maskedTextBoxSubnetMask.Validating += new CancelEventHandler(this.maskedTextBox_Validating);
			this.maskedTextBoxIPAddress.Culture = new CultureInfo("");
			this.errorProvider.SetIconAlignment(this.maskedTextBoxIPAddress, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxIPAddress.IconAlignment"));
			componentResourceManager.ApplyResources(this.maskedTextBoxIPAddress, "maskedTextBoxIPAddress");
			this.maskedTextBoxIPAddress.Name = "maskedTextBoxIPAddress";
			this.maskedTextBoxIPAddress.ResetOnSpace = false;
			this.maskedTextBoxIPAddress.KeyDown += new KeyEventHandler(this.maskedTextBox_KeyDown);
			this.maskedTextBoxIPAddress.KeyPress += new KeyPressEventHandler(this.maskedTextBox_KeyPress);
			this.maskedTextBoxIPAddress.Validating += new CancelEventHandler(this.maskedTextBox_Validating);
			componentResourceManager.ApplyResources(this.buttonSet, "buttonSet");
			this.buttonSet.Name = "buttonSet";
			this.buttonSet.UseVisualStyleBackColor = true;
			this.buttonSet.Click += new EventHandler(this.buttonSet_Click);
			componentResourceManager.ApplyResources(this.buttonGet, "buttonGet");
			this.buttonGet.Name = "buttonGet";
			this.buttonGet.UseVisualStyleBackColor = true;
			this.buttonGet.Click += new EventHandler(this.buttonGet_Click);
			this.buttonOK.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProvider.ContainerControl = this;
			componentResourceManager.ApplyResources(this.labelPort, "labelPort");
			this.labelPort.Name = "labelPort";
			this.comboBoxPorts.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxPorts.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxPorts, "comboBoxPorts");
			this.comboBoxPorts.Name = "comboBoxPorts";
			this.comboBoxPorts.Sorted = true;
			this.comboBoxPorts.SelectedIndexChanged += new EventHandler(this.comboBoxPorts_SelectedIndexChanged);
			this.groupBoxConnection.Controls.Add(this.labelPort);
			this.groupBoxConnection.Controls.Add(this.comboBoxPorts);
			componentResourceManager.ApplyResources(this.groupBoxConnection, "groupBoxConnection");
			this.groupBoxConnection.Name = "groupBoxConnection";
			this.groupBoxConnection.TabStop = false;
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.groupBoxHint, "groupBoxHint");
			this.groupBoxHint.Controls.Add(this.labelHint);
			this.groupBoxHint.Name = "groupBoxHint";
			this.groupBoxHint.TabStop = false;
			componentResourceManager.ApplyResources(this.labelHint, "labelHint");
			this.labelHint.Name = "labelHint";
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonOK;
			base.ControlBox = false;
			base.Controls.Add(this.groupBoxHint);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.groupBoxConnection);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBoxAddress);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SetEthernetAddress";
			base.Shown += new EventHandler(this.SetEthernetAddress_Shown);
			base.HelpRequested += new HelpEventHandler(this.SetEthernetAddress_HelpRequested);
			this.groupBoxAddress.ResumeLayout(false);
			this.groupBoxAddress.PerformLayout();
			((ISupportInitialize)this.errorProvider).EndInit();
			this.groupBoxConnection.ResumeLayout(false);
			this.groupBoxConnection.PerformLayout();
			this.groupBoxHint.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
