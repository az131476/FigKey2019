using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.TriggersPage
{
	public class CardSizeNumOfFilesCalculator : Form
	{
		private readonly ILoggerSpecifics mLoggerSpecifics;

		private readonly TriggerConfiguration mTriggerConfiguration;

		private readonly bool mIsInitialising;

		private IContainer components;

		private Label labelMemCapacity;

		private ComboBox comboBoxCardSize;

		private Label labelNumOfFilesOfSizeCanBeCreated;

		private Button buttonOK;

		private Button buttonCancel;

		private Button buttonHelp;

		private Label labelWarningFileNumberLimit;

		public uint NumberOfFiles
		{
			get;
			private set;
		}

		public uint CardSizeMB
		{
			get;
			set;
		}

		public static uint DisplayDialog(TriggerConfiguration triggerConfig, ILoggerSpecifics loggerSpecs, uint cardSizeMB)
		{
			CardSizeNumOfFilesCalculator cardSizeNumOfFilesCalculator = new CardSizeNumOfFilesCalculator(triggerConfig, loggerSpecs)
			{
				CardSizeMB = cardSizeMB
			};
			cardSizeNumOfFilesCalculator.ShowDialog();
			return cardSizeNumOfFilesCalculator.CardSizeMB;
		}

		public CardSizeNumOfFilesCalculator(TriggerConfiguration triggerConfig, ILoggerSpecifics loggerSpecs)
		{
			this.InitializeComponent();
			this.mLoggerSpecifics = loggerSpecs;
			this.mTriggerConfiguration = triggerConfig;
			this.mIsInitialising = true;
			this.InitPossibleMemoryCardSizes();
			this.mIsInitialising = false;
		}

		private void comboBoxCardSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.mIsInitialising)
			{
				return;
			}
			this.CalculateAndDisplayNumberOfFiles();
		}

		private void CardSizeNumOfFilesCalculator_Shown(object sender, EventArgs e)
		{
			this.InitPossibleMemoryCardSizes();
			this.comboBoxCardSize.SelectedItem = GUIUtil.MapMemoryCardSize2String(this.CardSizeMB);
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.CardSizeMB = GUIUtil.MapString2MemoryCardSize(this.comboBoxCardSize.SelectedItem.ToString());
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

		private void CardSizeNumOfFilesCalculator_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void InitPossibleMemoryCardSizes()
		{
			this.comboBoxCardSize.Items.Clear();
			IList<uint> memoryCardSizes = GUIUtil.GetMemoryCardSizes();
			foreach (uint current in from cardSize in memoryCardSizes
			where cardSize * 1024u <= this.mLoggerSpecifics.DataStorage.MaxMemoryCardSize
			select cardSize)
			{
				this.comboBoxCardSize.Items.Add(GUIUtil.MapMemoryCardSize2String(current));
			}
			if (this.comboBoxCardSize.Items.Count > 0)
			{
				this.comboBoxCardSize.SelectedIndex = 0;
			}
		}

		private void CalculateAndDisplayNumberOfFiles()
		{
			uint actualMaxRingBufferSizeSDCard = GUIUtil.GetActualMaxRingBufferSizeSDCard(GUIUtil.MapString2MemoryCardSize(this.comboBoxCardSize.SelectedItem.ToString()));
			this.NumberOfFiles = actualMaxRingBufferSizeSDCard * 1024u / this.mTriggerConfiguration.MemoryRingBuffer.Size.Value;
			if (this.NumberOfFiles == 0u)
			{
				this.NumberOfFiles = 1u;
			}
			if (this.mLoggerSpecifics.DataStorage.NumberOfMemories > 1u)
			{
				this.labelNumOfFilesOfSizeCanBeCreated.Text = string.Format(Resources_Trigger.UpToNumOfFilesOfSizeFitForMem, this.NumberOfFiles, GUIUtil.GetFractionedMByteDisplayString(this.mTriggerConfiguration.MemoryRingBuffer.Size.Value), this.mTriggerConfiguration.MemoryNr);
			}
			else
			{
				this.labelNumOfFilesOfSizeCanBeCreated.Text = string.Format(Resources_Trigger.UpToNumOfFilesOfSizeFit, this.NumberOfFiles, GUIUtil.GetFractionedMByteDisplayString(this.mTriggerConfiguration.MemoryRingBuffer.Size.Value));
			}
			if (this.NumberOfFiles > this.mLoggerSpecifics.DataStorage.MaxLoggerFiles)
			{
				this.labelWarningFileNumberLimit.Text = string.Format(Resources_Trigger.WarningNumberOfFilesLimit, this.mLoggerSpecifics.DataStorage.MaxLoggerFiles);
				this.labelWarningFileNumberLimit.Visible = true;
				return;
			}
			this.labelWarningFileNumberLimit.Visible = false;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CardSizeNumOfFilesCalculator));
			this.labelMemCapacity = new Label();
			this.comboBoxCardSize = new ComboBox();
			this.labelNumOfFilesOfSizeCanBeCreated = new Label();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.buttonHelp = new Button();
			this.labelWarningFileNumberLimit = new Label();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelMemCapacity, "labelMemCapacity");
			this.labelMemCapacity.Name = "labelMemCapacity";
			this.comboBoxCardSize.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxCardSize.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxCardSize, "comboBoxCardSize");
			this.comboBoxCardSize.Name = "comboBoxCardSize";
			this.comboBoxCardSize.SelectedIndexChanged += new EventHandler(this.comboBoxCardSize_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelNumOfFilesOfSizeCanBeCreated, "labelNumOfFilesOfSizeCanBeCreated");
			this.labelNumOfFilesOfSizeCanBeCreated.Name = "labelNumOfFilesOfSizeCanBeCreated";
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.labelWarningFileNumberLimit, "labelWarningFileNumberLimit");
			this.labelWarningFileNumberLimit.ForeColor = Color.Red;
			this.labelWarningFileNumberLimit.Name = "labelWarningFileNumberLimit";
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.labelWarningFileNumberLimit);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.labelNumOfFilesOfSizeCanBeCreated);
			base.Controls.Add(this.comboBoxCardSize);
			base.Controls.Add(this.labelMemCapacity);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "CardSizeNumOfFilesCalculator";
			base.Shown += new EventHandler(this.CardSizeNumOfFilesCalculator_Shown);
			base.HelpRequested += new HelpEventHandler(this.CardSizeNumOfFilesCalculator_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
