using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common
{
	public sealed class LRUFileHandler
	{
		private static readonly int NrOfToolTipItem = 10;

		private static readonly int RecentFileMenuEntryWidth = 450;

		private string[] model;

		private Dictionary<string, string> displayToFullPath;

		private Font currentFont;

		private ToolStripItemCollection collection;

		private EventHandler eventHandler;

		private int numberOfFixedMenuItems;

		public int NumberOfFixedMenuItems
		{
			set
			{
				this.numberOfFixedMenuItems = value;
			}
		}

		public LRUFileHandler(ToolStripItemCollection collection, EventHandler eventHandler, Font font)
		{
			this.model = new string[LRUFileHandler.NrOfToolTipItem];
			this.displayToFullPath = new Dictionary<string, string>();
			for (int i = 0; i < LRUFileHandler.NrOfToolTipItem; i++)
			{
				this.model[i] = string.Empty;
			}
			this.collection = collection;
			this.eventHandler = eventHandler;
			this.currentFont = font;
		}

		public void SaveToolStripItem2Settings()
		{
			int num = 1;
			string[] array = this.model;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				switch (num)
				{
				case 1:
					Settings.Default.LRU1 = text;
					break;
				case 2:
					Settings.Default.LRU2 = text;
					break;
				case 3:
					Settings.Default.LRU3 = text;
					break;
				case 4:
					Settings.Default.LRU4 = text;
					break;
				case 5:
					Settings.Default.LRU5 = text;
					break;
				case 6:
					Settings.Default.LRU6 = text;
					break;
				case 7:
					Settings.Default.LRU7 = text;
					break;
				case 8:
					Settings.Default.LRU8 = text;
					break;
				case 9:
					Settings.Default.LRU9 = text;
					break;
				case 10:
					Settings.Default.LRU10 = text;
					break;
				}
				num++;
			}
			Settings.Default.Save();
		}

		public void LoadSettings2ToolStripItem()
		{
			string lRU = Settings.Default.LRU1;
			if (lRU.Length > 0)
			{
				this.model[0] = lRU;
			}
			string lRU2 = Settings.Default.LRU2;
			if (lRU2.Length > 0)
			{
				this.model[1] = lRU2;
			}
			string lRU3 = Settings.Default.LRU3;
			if (lRU3.Length > 0)
			{
				this.model[2] = lRU3;
			}
			string lRU4 = Settings.Default.LRU4;
			if (lRU4.Length > 0)
			{
				this.model[3] = lRU4;
			}
			string lRU5 = Settings.Default.LRU5;
			if (lRU5.Length > 0)
			{
				this.model[4] = lRU5;
			}
			string lRU6 = Settings.Default.LRU6;
			if (lRU6.Length > 0)
			{
				this.model[5] = lRU6;
			}
			string lRU7 = Settings.Default.LRU7;
			if (lRU7.Length > 0)
			{
				this.model[6] = lRU7;
			}
			string lRU8 = Settings.Default.LRU8;
			if (lRU8.Length > 0)
			{
				this.model[7] = lRU8;
			}
			string lRU9 = Settings.Default.LRU9;
			if (lRU9.Length > 0)
			{
				this.model[8] = lRU9;
			}
			string lRU10 = Settings.Default.LRU10;
			if (lRU10.Length > 0)
			{
				this.model[9] = lRU10;
			}
			this.MapDataModel2GUICollection();
		}

		public void AddFileToLRUList(string fileName)
		{
			FileInfo fileInfo = new FileInfo(fileName);
			if (this.model[0] == fileName)
			{
				return;
			}
			string shortenedPath = PathUtil.GetShortenedPath(fileName, this.currentFont, LRUFileHandler.RecentFileMenuEntryWidth, false);
			if (this.displayToFullPath.ContainsKey(shortenedPath))
			{
				string fileName2 = this.displayToFullPath[shortenedPath];
				this.RemoveFileFromLRUList(fileName2);
			}
			for (int i = 0; i < LRUFileHandler.NrOfToolTipItem; i++)
			{
				if (this.model[i] == fileName)
				{
					for (int j = i; j >= 1; j--)
					{
						this.model[j] = this.model[j - 1];
					}
					this.model[0] = fileName;
					this.MapDataModel2GUICollection();
					return;
				}
			}
			for (int k = LRUFileHandler.NrOfToolTipItem - 1; k > 0; k--)
			{
				this.model[k] = this.model[k - 1];
			}
			this.model[0] = fileInfo.ToString();
			this.MapDataModel2GUICollection();
		}

		public bool RemoveFileFromLRUList(string fileName)
		{
			int num = -1;
			for (int i = 0; i < LRUFileHandler.NrOfToolTipItem; i++)
			{
				if (this.model[i] == fileName)
				{
					num = i;
					break;
				}
			}
			if (num < 0)
			{
				return false;
			}
			for (int j = num; j < LRUFileHandler.NrOfToolTipItem - 1; j++)
			{
				this.model[j] = this.model[j + 1];
			}
			this.model[LRUFileHandler.NrOfToolTipItem - 1] = "";
			this.MapDataModel2GUICollection();
			return true;
		}

		public string GetFilenameForMenuItem(string menuText)
		{
			int num = menuText.IndexOf(' ', 0);
			if (num < 0)
			{
				return string.Empty;
			}
			if (num + 1 < menuText.Length)
			{
				string key = menuText.Substring(num + 1);
				if (this.displayToFullPath.ContainsKey(key))
				{
					return this.displayToFullPath[key];
				}
			}
			return string.Empty;
		}

		public bool GetMostRecentlyUsedFile(out string fileName)
		{
			fileName = "";
			if (this.model.Length > 0 && !string.IsNullOrEmpty(this.model[0]))
			{
				fileName = this.model[0];
				return true;
			}
			return false;
		}

		private void MapDataModel2GUICollection()
		{
			ToolStripItem value = this.collection[this.collection.Count - 1];
			ToolStripItem value2 = this.collection[this.collection.Count - 2];
			for (int i = this.collection.Count - 1; i > this.numberOfFixedMenuItems; i--)
			{
				this.collection.RemoveAt(i);
			}
			this.displayToFullPath.Clear();
			for (int j = 0; j < this.model.Length; j++)
			{
				if (this.model[j].Length > 0)
				{
					string shortenedPath = PathUtil.GetShortenedPath(this.model[j], this.currentFont, LRUFileHandler.RecentFileMenuEntryWidth, false);
					this.displayToFullPath.Add(shortenedPath, this.model[j]);
					this.collection.Add(string.Format("&{0} {1}", j + 1, shortenedPath), null, this.eventHandler);
				}
			}
			this.collection.Add(value2);
			this.collection.Add(value);
		}
	}
}
