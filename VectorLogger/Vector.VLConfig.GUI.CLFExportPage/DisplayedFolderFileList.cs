using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.CLFExportPage
{
	public class DisplayedFolderFileList
	{
		private List<DisplayedFileItem> mFileList;

		private string mSourceFolder;

		private string destinationFolder;

		private List<string> sourceExtFilterList;

		private FileConversionDestFormat destinationFormat;

		private string destinationFileExtension;

		private string filePrefixOrName;

		private FileConversionFilenameFormat filenameFormat;

		public IList<DisplayedFileItem> FileList
		{
			get
			{
				return this.mFileList;
			}
		}

		public ReadOnlyCollection<DisplayedFileItem> EnabledFiles
		{
			get
			{
				return new ReadOnlyCollection<DisplayedFileItem>((from DisplayedFileItem item in this.mFileList
				where item.IsEnabled
				select item).ToList<DisplayedFileItem>());
			}
		}

		public string SourceFolder
		{
			get
			{
				return this.mSourceFolder;
			}
		}

		public DisplayedFolderFileList()
		{
			this.mFileList = new List<DisplayedFileItem>();
			this.sourceExtFilterList = new List<string>();
			this.destinationFolder = "";
			this.destinationFormat = FileConversionDestFormat.ASC;
			this.filePrefixOrName = "";
			this.filenameFormat = FileConversionFilenameFormat.UseOriginalName;
		}

		public int ReadSourceFolder(string sourceFolder)
		{
			if (string.IsNullOrEmpty(sourceFolder))
			{
				return 0;
			}
			List<DisplayedFileItem> list = new List<DisplayedFileItem>(this.mFileList);
			this.mFileList.Clear();
			this.mSourceFolder = sourceFolder;
			IEnumerable<FileInfoProxy> files = FileSystemServices.GetFiles(sourceFolder);
			IEnumerable<FileInfoProxy> enumerable = from file in files
			where this.sourceExtFilterList.Contains(file.Extension.ToLower())
			orderby file.Name
			select file;
			foreach (FileInfoProxy current in enumerable)
			{
				DisplayedFileItem displayedFileItem = new DisplayedFileItem();
				try
				{
					displayedFileItem.Filename = current.Name;
					displayedFileItem.DateTimeModified = current.LastWriteTime;
					displayedFileItem.FileSize = current.Length;
				}
				catch
				{
					continue;
				}
				displayedFileItem.DestinationFilename = "";
				displayedFileItem.IsEnabled = false;
				DisplayedFileItem displayedFileItem2;
				if (this.PopItemForFileName(displayedFileItem.Filename, list, out displayedFileItem2) && displayedFileItem2.IsEnabled)
				{
					this.EnableFileItem(displayedFileItem);
				}
				this.mFileList.Add(displayedFileItem);
			}
			list.Clear();
			return this.mFileList.Count;
		}

		public void EnableFileItem(DisplayedFileItem item)
		{
			item.IsEnabled = true;
			this.UpdateDestinationFile(item);
		}

		public void DisableFileItem(DisplayedFileItem item)
		{
			item.IsEnabled = false;
			item.DestinationFilename = "";
			item.IsDestinationFileExisting = false;
		}

		public void AddSourceFolderExtFilter(string extension)
		{
			extension = "." + extension.ToLower();
			if (!this.sourceExtFilterList.Contains(extension))
			{
				this.sourceExtFilterList.Add(extension);
			}
		}

		public void SetDestinationFolder(string destFolder)
		{
			this.destinationFolder = destFolder;
			foreach (DisplayedFileItem current in this.EnabledFiles)
			{
				this.UpdateDestinationFile(current);
			}
		}

		public void SetDestinationFileFormat(FileConversionParameters convParam)
		{
			this.destinationFormat = convParam.DestinationFormat;
			this.destinationFileExtension = FileConversionHelper.GetConfiguredDestinationFormatExtension(convParam);
			foreach (DisplayedFileItem current in this.EnabledFiles)
			{
				this.UpdateDestinationFile(current);
			}
		}

		public void SetDestinationFilenameFormat(FileConversionFilenameFormat format, string prefixOrName)
		{
			this.filePrefixOrName = prefixOrName;
			this.filenameFormat = format;
			foreach (DisplayedFileItem current in this.EnabledFiles)
			{
				this.UpdateDestinationFile(current);
			}
		}

		public void SetDestinationFileExtension(FileConversionParameters convParam)
		{
			this.destinationFileExtension = FileConversionHelper.GetConfiguredDestinationFormatExtension(convParam);
			foreach (DisplayedFileItem current in this.EnabledFiles)
			{
				this.UpdateDestinationFile(current);
			}
		}

		public void EnableAllFiles()
		{
			foreach (DisplayedFileItem current in this.mFileList)
			{
				current.IsEnabled = true;
				this.UpdateDestinationFile(current);
			}
		}

		public void DisableAllFiles()
		{
			foreach (DisplayedFileItem current in this.mFileList)
			{
				current.IsEnabled = false;
			}
		}

		private bool PopItemForFileName(string filename, List<DisplayedFileItem> itemList, out DisplayedFileItem foundItem)
		{
			foundItem = null;
			if (itemList.Count == 0)
			{
				return false;
			}
			foreach (DisplayedFileItem current in itemList)
			{
				if (current.Filename == filename)
				{
					foundItem = current;
					itemList.Remove(current);
					return true;
				}
			}
			return false;
		}

		private void UpdateDestinationFile(int index)
		{
			if (index >= 0 && index < this.mFileList.Count)
			{
				this.UpdateDestinationFile(this.mFileList[index]);
			}
		}

		private void UpdateDestinationFile(DisplayedFileItem item)
		{
			if (Directory.Exists(this.destinationFolder))
			{
				string text = item.Filename;
				if (this.filenameFormat == FileConversionFilenameFormat.AddPrefix)
				{
					bool isPrefix = this.filenameFormat == FileConversionFilenameFormat.AddPrefix;
					string text2;
					if (FileSystemServices.GetRenamedLogDataFileName(isPrefix, this.filePrefixOrName, item.Filename, out text2))
					{
						text = text2;
					}
				}
				else if (this.filenameFormat == FileConversionFilenameFormat.UseCustomName)
				{
					text = "";
				}
				if (!Path.HasExtension(text))
				{
					item.DestinationFilename = string.Format("{0}*{1}", text, this.destinationFileExtension);
				}
				else
				{
					item.DestinationFilename = string.Format("{0}*{1}", Path.GetFileNameWithoutExtension(text), this.destinationFileExtension);
				}
				string[] files = Directory.GetFiles(this.destinationFolder, item.DestinationFilename);
				if (files.Count<string>() > 0)
				{
					item.IsDestinationFileExisting = true;
					return;
				}
				item.IsDestinationFileExisting = false;
			}
		}
	}
}
