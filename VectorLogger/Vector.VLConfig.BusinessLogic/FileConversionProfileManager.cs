using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic
{
	public class FileConversionProfileManager
	{
		private readonly List<FileConversionProfile> mProfiles = new List<FileConversionProfile>();

		private bool mHasChanged;

		private static FileConversionProfileManager sInstance;

		public AppDataAccess AppDataAccess
		{
			get;
			private set;
		}

		private bool IsInitialized
		{
			get;
			set;
		}

		public bool HasChanged
		{
			get
			{
				return this.mProfiles.Any((FileConversionProfile t) => t.HasChanged) || this.mHasChanged;
			}
		}

		public static FileConversionProfileManager Instance
		{
			get
			{
				FileConversionProfileManager arg_17_0;
				if ((arg_17_0 = FileConversionProfileManager.sInstance) == null)
				{
					arg_17_0 = (FileConversionProfileManager.sInstance = new FileConversionProfileManager());
				}
				return arg_17_0;
			}
		}

		public FileConversionProfileManager()
		{
			this.IsInitialized = false;
		}

		public void Init(AppDataAccess appDataAccess)
		{
			this.AppDataAccess = appDataAccess;
			this.IsInitialized = (this.AppDataAccess != null);
			this.InitFromPersistence();
		}

		public static void InitDropDownItems(ToolStripItemCollection itemCollection, IFileConversionParametersClient client)
		{
			itemCollection.Clear();
			if (client == null)
			{
				return;
			}
			IEnumerable<FileConversionProfile> profiles = FileConversionProfileManager.Instance.GetProfiles();
			foreach (FileConversionProfile current in profiles)
			{
				string text = (!string.IsNullOrEmpty(current.DisplayName)) ? current.DisplayName : Path.GetFileName(current.FilePath);
				ToolStripItem toolStripItem = itemCollection.Add(text);
				toolStripItem.Tag = current;
				toolStripItem.Image = current.GetStateImage(client.ViewType, client.LoggerType);
				toolStripItem.ToolTipText = current.FilePath;
				if (current.HasErrors(client.ViewType))
				{
					ToolStripItem expr_9C = toolStripItem;
					expr_9C.ToolTipText = expr_9C.ToolTipText + Environment.NewLine + current.GetErrorsText(client.ViewType);
				}
				else if (current.HasWarnings(client.ViewType))
				{
					ToolStripItem expr_D5 = toolStripItem;
					expr_D5.ToolTipText = expr_D5.ToolTipText + Environment.NewLine + current.GetWarningsText(client.ViewType);
					if (current.HasInfos(client.ViewType, client.LoggerType))
					{
						ToolStripItem expr_10F = toolStripItem;
						expr_10F.ToolTipText = expr_10F.ToolTipText + Environment.NewLine + current.GetInfosText(client.ViewType, client.LoggerType);
					}
				}
				else if (current.HasInfos(client.ViewType, client.LoggerType))
				{
					ToolStripItem expr_154 = toolStripItem;
					expr_154.ToolTipText = expr_154.ToolTipText + Environment.NewLine + current.GetInfosText(client.ViewType, client.LoggerType);
				}
			}
			if (itemCollection.Count > 0)
			{
				itemCollection.Add(new ToolStripSeparator());
			}
			itemCollection.Add(Resources.FileConversionProfileLoadAdditionalProfile);
		}

		public static bool OnDropDownItemClicked(ToolStripItemClickedEventArgs e, IFileConversionParametersClient client, bool continueWithConversion)
		{
			return FileConversionProfileManager.OnDropDownItemClicked(e.ClickedItem, client, continueWithConversion);
		}

		public static bool OnDropDownItemClicked(ToolStripItem clickedItem, IFileConversionParametersClient client, bool continueWithConversion)
		{
			if (client == null)
			{
				return true;
			}
			if (clickedItem.Owner != null)
			{
				clickedItem.Owner.Hide();
			}
			if (clickedItem.Text == Resources.FileConversionProfileLoadAdditionalProfile)
			{
				return FileConversionProfileManager.Instance.ApplyTo(client, false);
			}
			FileConversionProfile fileConversionProfile = clickedItem.Tag as FileConversionProfile;
			return fileConversionProfile != null && FileConversionProfileManager.Instance.ApplyTo(fileConversionProfile, client, continueWithConversion) && client.CanConvert;
		}

		public void InitFromPersistence()
		{
			this.mProfiles.Clear();
			if (this.IsInitialized)
			{
				foreach (string current in this.AppDataAccess.AppDataRoot.FileConversionProfileList.Profiles)
				{
					this.mProfiles.Add(new FileConversionProfile(current));
				}
			}
			this.mHasChanged = false;
		}

		public void SaveToPersistence()
		{
			if (!this.IsInitialized || !this.HasChanged)
			{
				return;
			}
			this.AppDataAccess.AppDataRoot.FileConversionProfileList.Profiles.Clear();
			foreach (FileConversionProfile current in this.mProfiles)
			{
				current.Save();
				this.AppDataAccess.AppDataRoot.FileConversionProfileList.Profiles.Add(current.FilePath);
			}
			this.mHasChanged = false;
		}

		public void LoadProfile(EnumViewType viewType, LoggerType loggerType)
		{
			FileConversionProfile profile;
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = Resources_Files.FileFilterFileConversionProfile;
				if (DialogResult.OK != openFileDialog.ShowDialog())
				{
					return;
				}
				profile = this.GetProfile(openFileDialog.FileName);
			}
			if (!profile.HasErrors(viewType))
			{
				this.AddProfile(profile, false);
			}
			this.DisplayErrors(profile, viewType, loggerType, false);
		}

		public void RemoveProfile(FileConversionProfile profile, bool saveToPersistence)
		{
			this.mProfiles.Remove(profile);
			this.mHasChanged = true;
			if (saveToPersistence)
			{
				this.SaveToPersistence();
			}
		}

		public ReadOnlyCollection<FileConversionProfile> GetProfiles()
		{
			return new ReadOnlyCollection<FileConversionProfile>(this.mProfiles);
		}

		public void SaveFrom(IFileConversionParametersClient client)
		{
			if (client == null)
			{
				return;
			}
			FileConversionProfile profile;
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Filter = Resources_Files.FileFilterFileConversionProfile;
				if (DialogResult.OK != saveFileDialog.ShowDialog())
				{
					return;
				}
				profile = this.GetProfile(saveFileDialog.FileName);
			}
			using (SaveFileConversionProfileDialog saveFileConversionProfileDialog = new SaveFileConversionProfileDialog())
			{
				saveFileConversionProfileDialog.DisplayName = ((!string.IsNullOrEmpty(profile.DisplayName)) ? profile.DisplayName : Path.GetFileNameWithoutExtension(profile.FilePath));
				IFileConversionParametersExClient fileConversionParametersExClient = client as IFileConversionParametersExClient;
				saveFileConversionProfileDialog.MarkersAvailable = (fileConversionParametersExClient != null && fileConversionParametersExClient.GetMarkerTypeSelection().Any<string>());
				saveFileConversionProfileDialog.HasMarkers = (saveFileConversionProfileDialog.MarkersAvailable && profile.HasMarkers);
				saveFileConversionProfileDialog.TriggersAvailable = (fileConversionParametersExClient != null && fileConversionParametersExClient.GetTriggerTypeSelection().Any<string>());
				saveFileConversionProfileDialog.HasTriggers = (saveFileConversionProfileDialog.TriggersAvailable && profile.HasTriggers);
				if (DialogResult.OK != saveFileConversionProfileDialog.ShowDialog())
				{
					return;
				}
				profile.DisplayName = saveFileConversionProfileDialog.DisplayName;
				profile.HasMarkers = saveFileConversionProfileDialog.HasMarkers;
				profile.HasTriggers = saveFileConversionProfileDialog.HasTriggers;
			}
			profile.SaveFrom(client);
			this.AddProfile(profile, true);
		}

		public bool ApplyTo(IFileConversionParametersClient client, bool continueWithConversion)
		{
			FileConversionProfile profile;
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = Resources_Files.FileFilterFileConversionProfile;
				if (DialogResult.OK != openFileDialog.ShowDialog())
				{
					return false;
				}
				profile = this.GetProfile(openFileDialog.FileName);
			}
			bool result = this.ApplyTo(profile, client, continueWithConversion);
			if (!profile.HasErrors(client.ViewType))
			{
				this.AddProfile(profile, true);
			}
			return result;
		}

		public bool ApplyTo(FileConversionProfile profile, IFileConversionParametersClient client, bool continueWithConversion)
		{
			if (client == null)
			{
				return false;
			}
			profile.ApplyTo(client);
			return this.DisplayErrors(profile, client.ViewType, client.LoggerType, continueWithConversion);
		}

		public void MoveTop(FileConversionProfile profile)
		{
			if (profile == null || !this.mProfiles.Contains(profile))
			{
				return;
			}
			this.mProfiles.Remove(profile);
			this.mProfiles.Insert(0, profile);
		}

		public void MoveUp(FileConversionProfile profile)
		{
			if (profile == null || !this.mProfiles.Contains(profile))
			{
				return;
			}
			int num = this.mProfiles.IndexOf(profile) - 1;
			if (num < 0)
			{
				return;
			}
			this.mProfiles.Remove(profile);
			this.mProfiles.Insert(num, profile);
		}

		public void MoveDown(FileConversionProfile profile)
		{
			if (profile == null || !this.mProfiles.Contains(profile))
			{
				return;
			}
			int num = this.mProfiles.IndexOf(profile) + 1;
			if (num > this.mProfiles.Count - 1)
			{
				return;
			}
			this.mProfiles.Remove(profile);
			this.mProfiles.Insert(num, profile);
		}

		public void MoveBottom(FileConversionProfile profile)
		{
			if (profile == null || !this.mProfiles.Contains(profile))
			{
				return;
			}
			this.mProfiles.Remove(profile);
			this.mProfiles.Add(profile);
		}

		private bool DisplayErrors(FileConversionProfile profile, EnumViewType viewType, LoggerType loggerType, bool continueWithConversion)
		{
			if (profile.HasErrors(viewType))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(Resources.FileConversionProfileErrorsText);
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append(profile.GetErrorsText(viewType));
				if (this.mProfiles.Contains(profile))
				{
					stringBuilder.Append(Environment.NewLine);
					stringBuilder.Append(Environment.NewLine);
					stringBuilder.Append(Resources.FileConversionProfileQuestionRemoveFile);
					if (DialogResult.Yes == InformMessageBox.Show(EnumInfoType.Error, EnumQuestionType.Question, stringBuilder.ToString()))
					{
						this.RemoveProfile(profile, true);
					}
				}
				else
				{
					InformMessageBox.Error(stringBuilder.ToString());
				}
				return false;
			}
			if (profile.HasWarnings(viewType))
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.Append(Resources.FileConversionProfileWarningsAndInfosText);
				stringBuilder2.Append(Environment.NewLine);
				stringBuilder2.Append(Environment.NewLine);
				stringBuilder2.Append(profile.GetWarningsText(viewType));
				if (profile.HasInfos(viewType, loggerType))
				{
					stringBuilder2.Append(Environment.NewLine);
					stringBuilder2.Append(profile.GetInfosText(viewType, loggerType));
				}
				stringBuilder2.Append(Environment.NewLine);
				stringBuilder2.Append(Environment.NewLine);
				stringBuilder2.Append(Resources.FileConversionProfileHintVerifySettings);
				profile.RemoveVolatileErrors();
				if (continueWithConversion)
				{
					stringBuilder2.Append(Environment.NewLine);
					stringBuilder2.Append(Environment.NewLine);
					stringBuilder2.Append(Resources.FileConversionProfileQuestionContinueWithConversion);
					return DialogResult.Yes == InformMessageBox.Show(EnumInfoType.Warning, EnumQuestionType.QuestionDefaultNo, stringBuilder2.ToString());
				}
				InformMessageBox.Warning(stringBuilder2.ToString());
				return false;
			}
			else
			{
				if (!profile.HasInfos(viewType, loggerType))
				{
					return continueWithConversion;
				}
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.Append(Resources.FileConversionProfileInfosText);
				stringBuilder3.Append(Environment.NewLine);
				stringBuilder3.Append(Environment.NewLine);
				stringBuilder3.Append(profile.GetInfosText(viewType, loggerType));
				stringBuilder3.Append(Environment.NewLine);
				stringBuilder3.Append(Environment.NewLine);
				stringBuilder3.Append(Resources.FileConversionProfileHintVerifySettings);
				profile.RemoveVolatileErrors();
				if (continueWithConversion)
				{
					stringBuilder3.Append(Environment.NewLine);
					stringBuilder3.Append(Environment.NewLine);
					stringBuilder3.Append(Resources.FileConversionProfileQuestionContinueWithConversion);
					return DialogResult.Yes == InformMessageBox.Show(EnumInfoType.Info, EnumQuestionType.QuestionDefaultNo, stringBuilder3.ToString());
				}
				InformMessageBox.Info(stringBuilder3.ToString());
				return false;
			}
		}

		private void AddProfile(FileConversionProfile profile, bool saveToPersistence)
		{
			if (this.mProfiles.Contains(profile))
			{
				return;
			}
			this.mProfiles.Add(profile);
			this.mHasChanged = true;
			if (saveToPersistence)
			{
				this.SaveToPersistence();
			}
		}

		private FileConversionProfile GetProfile(string filePath)
		{
			return this.mProfiles.FirstOrDefault((FileConversionProfile t) => t.FilePath.Equals(filePath, StringComparison.InvariantCulture)) ?? new FileConversionProfile(filePath);
		}
	}
}
