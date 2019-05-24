using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic
{
	public static class GlobalOptionsManager
	{
		public enum ListSelector
		{
			SourceFolders,
			DestinationFolders,
			ClfSourceFolders,
			ClfExportFolders,
			SourceZips
		}

		public static readonly int RecentFolderListWidth = 250;

		public static readonly int ListItemsCount = 5;

		public static GlobalOptions GlobalOptions
		{
			get;
			set;
		}

		public static List<string> GetRecentFolders(GlobalOptionsManager.ListSelector listSelector)
		{
			if (GlobalOptionsManager.GlobalOptions == null)
			{
				return null;
			}
			if (listSelector == GlobalOptionsManager.ListSelector.SourceFolders)
			{
				return GlobalOptionsManager.GlobalOptions.RecentSourceFolders;
			}
			if (listSelector == GlobalOptionsManager.ListSelector.DestinationFolders)
			{
				return GlobalOptionsManager.GlobalOptions.RecentDestinationFolders;
			}
			if (listSelector == GlobalOptionsManager.ListSelector.ClfSourceFolders)
			{
				return GlobalOptionsManager.GlobalOptions.RecentClfSourceFolders;
			}
			if (listSelector == GlobalOptionsManager.ListSelector.SourceZips)
			{
				return GlobalOptionsManager.GlobalOptions.RecentSourceZips;
			}
			return GlobalOptionsManager.GlobalOptions.RecentClfExportFolders;
		}

		public static void AddMostRecentFolder(GlobalOptionsManager.ListSelector listSelector, string path)
		{
			if (GlobalOptionsManager.GlobalOptions != null)
			{
				List<string> list;
				if (listSelector == GlobalOptionsManager.ListSelector.SourceFolders)
				{
					list = GlobalOptionsManager.GlobalOptions.RecentSourceFolders;
				}
				else if (listSelector == GlobalOptionsManager.ListSelector.DestinationFolders)
				{
					list = GlobalOptionsManager.GlobalOptions.RecentDestinationFolders;
				}
				else if (listSelector == GlobalOptionsManager.ListSelector.ClfSourceFolders)
				{
					list = GlobalOptionsManager.GlobalOptions.RecentClfSourceFolders;
				}
				else if (listSelector == GlobalOptionsManager.ListSelector.SourceZips)
				{
					list = GlobalOptionsManager.GlobalOptions.RecentSourceZips;
				}
				else
				{
					list = GlobalOptionsManager.GlobalOptions.RecentClfExportFolders;
				}
				if (string.IsNullOrEmpty(path))
				{
					return;
				}
				foreach (string current in list)
				{
					if (path == current)
					{
						list.Remove(path);
						break;
					}
				}
				list.Insert(0, path);
				while (list.Count > GlobalOptionsManager.ListItemsCount)
				{
					list.RemoveAt(GlobalOptionsManager.ListItemsCount);
				}
			}
		}

		public static void RemoveRecentFolder(GlobalOptionsManager.ListSelector listSelector, string path)
		{
			if (GlobalOptionsManager.GlobalOptions != null)
			{
				if (listSelector == GlobalOptionsManager.ListSelector.SourceFolders)
				{
					GlobalOptionsManager.GlobalOptions.RecentSourceFolders.Remove(path);
					return;
				}
				if (listSelector == GlobalOptionsManager.ListSelector.DestinationFolders)
				{
					GlobalOptionsManager.GlobalOptions.RecentDestinationFolders.Remove(path);
					return;
				}
				if (listSelector == GlobalOptionsManager.ListSelector.ClfSourceFolders)
				{
					GlobalOptionsManager.GlobalOptions.RecentClfSourceFolders.Remove(path);
					return;
				}
				if (listSelector == GlobalOptionsManager.ListSelector.SourceZips)
				{
					GlobalOptionsManager.GlobalOptions.RecentSourceZips.Remove(path);
					return;
				}
				GlobalOptionsManager.GlobalOptions.RecentClfExportFolders.Remove(path);
			}
		}

		public static bool FolderAccessible(GlobalOptionsManager.ListSelector listSelector, string path)
		{
			string text = string.Empty;
			if (string.IsNullOrEmpty(path) || !DirectoryProxy.Exists(path))
			{
				text = Resources.ErrorPathNotFound;
			}
			else if (!GUIUtil.FolderAccessible(path))
			{
				text = Resources.ErrorUnaccessiblePath;
			}
			if (text != string.Empty)
			{
				if (InformMessageBox.Show(EnumQuestionType.Question, string.Format("{0} {1}", text, Resources.QuestionDiscardPath)) == DialogResult.Yes)
				{
					Array values = Enum.GetValues(typeof(GlobalOptionsManager.ListSelector));
					foreach (GlobalOptionsManager.ListSelector listSelector2 in values)
					{
						GlobalOptionsManager.RemoveRecentFolder(listSelector2, path);
					}
				}
				return false;
			}
			return true;
		}

		public static bool FolderPathExists(GlobalOptionsManager.ListSelector listSelector, string path)
		{
			if (string.IsNullOrEmpty(path) || (listSelector == GlobalOptionsManager.ListSelector.SourceZips && !File.Exists(path)) || (listSelector != GlobalOptionsManager.ListSelector.SourceZips && !Directory.Exists(path)))
			{
				if (MessageBox.Show(string.Format("{0} {1}", Resources.ErrorPathNotFound, Resources.QuestionDiscardPath), Vocabulary.VLConfigApplicationTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					Array values = Enum.GetValues(typeof(GlobalOptionsManager.ListSelector));
					foreach (GlobalOptionsManager.ListSelector listSelector2 in values)
					{
						GlobalOptionsManager.RemoveRecentFolder(listSelector2, path);
					}
				}
				return false;
			}
			return true;
		}
	}
}
