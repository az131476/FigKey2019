using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common
{
	public class GenerateDBCfromARXML : Form
	{
		private bool isSuccess;

		private IEnumerable<Database> databases;

		private string configFolderPath;

		private string destFolderPath;

		private List<string> generatedDBCFiles;

		private IContainer components;

		private TextBox textBoxStatus;

		private Button buttonCancel;

		private ProgressBar progressBar;

		public bool IsSuccess
		{
			get
			{
				return this.isSuccess;
			}
		}

		public ReadOnlyCollection<string> GeneratedDBCFiles
		{
			get
			{
				return new ReadOnlyCollection<string>(this.generatedDBCFiles);
			}
		}

		public static bool Execute(IEnumerable<Database> databases, string configFolder, string destFolder, out ReadOnlyCollection<string> generatedDBCFilePaths)
		{
			bool result;
			using (GenerateDBCfromARXML generateDBCfromARXML = new GenerateDBCfromARXML(databases, configFolder, destFolder))
			{
				generateDBCfromARXML.ShowDialog();
				generatedDBCFilePaths = generateDBCfromARXML.GeneratedDBCFiles;
				result = generateDBCfromARXML.IsSuccess;
			}
			return result;
		}

		public GenerateDBCfromARXML(IEnumerable<Database> databases, string configFolder, string destFolder)
		{
			this.InitializeComponent();
			this.isSuccess = false;
			this.databases = databases;
			this.configFolderPath = configFolder;
			this.destFolderPath = destFolder;
			this.generatedDBCFiles = new List<string>();
		}

		private void GenerateDBCfromARXML_Shown(object sender, EventArgs e)
		{
			this.isSuccess = true;
			int num = 0;
			foreach (Database current in this.databases)
			{
				if (current.IsAUTOSARFile && current.BusType.Value == BusType.Bt_CAN)
				{
					num++;
				}
			}
			this.progressBar.Maximum = num;
			this.progressBar.Step = 1;
			int num2 = 0;
			foreach (Database current2 in this.databases)
			{
				Application.DoEvents();
				if (!this.isSuccess)
				{
					break;
				}
				if (current2.IsAUTOSARFile && current2.BusType.Value == BusType.Bt_CAN)
				{
					Cursor.Current = Cursors.WaitCursor;
					this.textBoxStatus.Text = string.Format(Resources.WaitWhileConvertNetworkFromAutosar, current2.NetworkName.Value);
					this.progressBar.Increment(1);
					Application.DoEvents();
					string path = FileSystemServices.GenerateUniqueDbcFileName(Path.GetFileNameWithoutExtension(current2.FilePath.Value), current2.NetworkName.Value, num2);
					string text = Path.Combine(this.destFolderPath, path);
					string absolutePath = FileSystemServices.GetAbsolutePath(current2.FilePath.Value, this.configFolderPath);
					if (!File.Exists(text) && File.Exists(absolutePath))
					{
						if (!FileSystemServices.SaveAutosarNetworkAsDbc(absolutePath, current2.NetworkName.Value, text))
						{
							InformMessageBox.Error(string.Format(Resources.ErrorUnableToGenDBCforNetworkInARXML, current2.NetworkName.Value, Path.GetFileName(current2.FilePath.Value)));
							this.isSuccess = false;
						}
						else
						{
							this.generatedDBCFiles.Add(text);
						}
					}
					Application.DoEvents();
					Cursor.Current = Cursors.Default;
				}
				num2++;
			}
			base.DialogResult = DialogResult.OK;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.isSuccess = false;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(GenerateDBCfromARXML));
			this.textBoxStatus = new TextBox();
			this.buttonCancel = new Button();
			this.progressBar = new ProgressBar();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.textBoxStatus, "textBoxStatus");
			this.textBoxStatus.Name = "textBoxStatus";
			this.textBoxStatus.ReadOnly = true;
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			componentResourceManager.ApplyResources(this.progressBar, "progressBar");
			this.progressBar.Name = "progressBar";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.progressBar);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.textBoxStatus);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "GenerateDBCfromARXML";
			base.Shown += new EventHandler(this.GenerateDBCfromARXML_Shown);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
