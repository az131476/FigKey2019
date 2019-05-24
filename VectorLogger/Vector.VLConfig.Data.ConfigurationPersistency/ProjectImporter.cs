using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vector.VLConfig.GeneralUtil;

namespace Vector.VLConfig.Data.ConfigurationPersistency
{
	public class ProjectImporter
	{
		public class ExternalFile
		{
			public string ExtractDirectory
			{
				get;
				set;
			}

			public string DirectoryPathInArchive
			{
				get;
				private set;
			}

			public ExternalFile(string directoryPathInArchive, string extractDirecory)
			{
				this.DirectoryPathInArchive = directoryPathInArchive;
				this.ExtractDirectory = extractDirecory;
			}
		}

		private bool projectFileExtracted;

		public event ProjectImportFileOverwriteHandler FileOverwriteHandler;

		public event ErrorHandler ErrorHandler;

		private string ImportFileName
		{
			get;
			set;
		}

		public string BaseDirectory
		{
			get;
			private set;
		}

		public string GLCFilePath
		{
			get;
			private set;
		}

		private string Password
		{
			get;
			set;
		}

		public bool IsPasswordProtected
		{
			get;
			private set;
		}

		public bool IsValidProjectImportFile
		{
			get;
			private set;
		}

		public bool MatchesWaterMark
		{
			get;
			private set;
		}

		public Dictionary<string, ProjectImporter.ExternalFile> ExternalFiles
		{
			get;
			private set;
		}

		public ProjectImporter(string importFileName, string baseDirectory, string waterMark)
		{
			this.ImportFileName = importFileName;
			this.BaseDirectory = baseDirectory;
			this.ExternalFiles = new Dictionary<string, ProjectImporter.ExternalFile>();
			this.IsPasswordProtected = false;
			this.IsValidProjectImportFile = false;
			this.MatchesWaterMark = false;
			using (ZipFile zipFile = ZipFile.Read(this.ImportFileName))
			{
				this.MatchesWaterMark = (zipFile.Comment == waterMark);
				foreach (ZipEntry current in zipFile)
				{
					if (current.UsesEncryption && !this.IsPasswordProtected)
					{
						this.IsPasswordProtected = true;
					}
					if (current.FileName.StartsWith("GLC/") && current.FileName.EndsWith(Vocabulary.FileExtensionDotGLC))
					{
						this.IsValidProjectImportFile = true;
					}
				}
			}
		}

		public bool CheckAndSetPassword(string password)
		{
			if (ZipFile.CheckZipPassword(this.ImportFileName, password))
			{
				this.Password = password;
				return true;
			}
			return false;
		}

		public void ProjectImporterProgress(object sender, ExtractProgressEventArgs e)
		{
			ZipEntry currentEntry = e.CurrentEntry;
			if (e.EventType != ZipProgressEventType.Extracting_ExtractEntryWouldOverwrite)
			{
				if (e.EventType == ZipProgressEventType.Extracting_EntryBytesWritten && Path.GetExtension(currentEntry.FileName) == Vocabulary.FileExtensionDotGLC && (e.TotalBytesToTransfer == e.BytesTransferred || e.TotalBytesToTransfer == -1L))
				{
					this.projectFileExtracted = true;
				}
				return;
			}
			bool flag = false;
			if (this.FileOverwriteHandler != null)
			{
				this.FileOverwriteHandler(e.ExtractLocation + Path.DirectorySeparatorChar + Path.GetFileName(currentEntry.FileName), ref flag);
			}
			if (flag)
			{
				currentEntry.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
				return;
			}
			currentEntry.ExtractExistingFile = ExtractExistingFileAction.DoNotOverwrite;
		}

		public bool Import()
		{
			bool result = false;
			if (this.IsValidProjectImportFile)
			{
				using (ZipFile zipFile = new ZipFile(this.ImportFileName))
				{
					zipFile.FlattenFoldersOnExtract = true;
					zipFile.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(this.ProjectImporterProgress);
					zipFile.ExtractExistingFile = ExtractExistingFileAction.InvokeExtractProgressEvent;
					zipFile.Password = this.Password;
					foreach (ZipEntry current in zipFile)
					{
						if (current.FileName.StartsWith("GLC/") && current.FileName.EndsWith(Vocabulary.FileExtensionDotGLC))
						{
							string selectionCriteria = string.Format("name='{0}'", Path.GetFileName(current.FileName));
							try
							{
								zipFile.ExtractSelectedEntries(selectionCriteria, "GLC", this.BaseDirectory, ExtractExistingFileAction.InvokeExtractProgressEvent);
								this.GLCFilePath = this.BaseDirectory + Path.DirectorySeparatorChar + Path.GetFileName(current.FileName);
								result = this.projectFileExtracted;
								continue;
							}
							catch (Exception ex)
							{
								if (this.ErrorHandler != null)
								{
									this.ErrorHandler(this.GLCFilePath, ex.Message, true);
								}
								result = false;
								break;
							}
						}
						if (current.FileName.StartsWith("GLC/"))
						{
							string selectionCriteria2 = string.Format("name='{0}'", Path.GetFileName(current.FileName));
							string directoryName = Path.GetDirectoryName(current.FileName);
							string text = this.BaseDirectory + Path.DirectorySeparatorChar;
							string text2 = current.FileName.Substring(4).Replace('/', Path.DirectorySeparatorChar);
							if (text2.Contains(Path.DirectorySeparatorChar))
							{
								text += Path.GetDirectoryName(text2);
							}
							try
							{
								zipFile.ExtractSelectedEntries(selectionCriteria2, directoryName, text, ExtractExistingFileAction.InvokeExtractProgressEvent);
								continue;
							}
							catch (Exception ex2)
							{
								if (this.ErrorHandler != null)
								{
									this.ErrorHandler(text + Path.GetFileName(current.FileName), ex2.Message, false);
								}
								continue;
							}
						}
						if (current.FileName.StartsWith("Externals/"))
						{
							if (this.ExternalFiles == null)
							{
								this.ExternalFiles = new Dictionary<string, ProjectImporter.ExternalFile>();
							}
							string.Format("name='{0}'", Path.GetFileName(current.FileName));
							string directoryName2 = Path.GetDirectoryName(current.FileName);
							string text3 = current.FileName.Substring(10).Replace('/', Path.DirectorySeparatorChar);
							string text4 = "";
							int num = text3.IndexOf(Path.DirectorySeparatorChar);
							if (num == 1)
							{
								text4 = text3.Substring(0, 1) + Path.VolumeSeparatorChar + text3.Substring(1);
							}
							else if (num > 1)
							{
								text4 = Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString() + text3;
							}
							this.ExternalFiles.Add(text4, new ProjectImporter.ExternalFile(directoryName2, Path.GetDirectoryName(text4)));
						}
					}
				}
			}
			return result;
		}

		public bool ImportFile(string fileName, string directoryInArchive, string extractDirectory)
		{
			bool result = false;
			using (ZipFile zipFile = new ZipFile(this.ImportFileName))
			{
				zipFile.FlattenFoldersOnExtract = true;
				zipFile.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(this.ProjectImporterProgress);
				zipFile.ExtractExistingFile = ExtractExistingFileAction.InvokeExtractProgressEvent;
				zipFile.Password = this.Password;
				string selectionCriteria = string.Format("name='{0}'", fileName);
				try
				{
					zipFile.ExtractSelectedEntries(selectionCriteria, directoryInArchive, extractDirectory, ExtractExistingFileAction.InvokeExtractProgressEvent);
					result = true;
				}
				catch (Exception ex)
				{
					if (this.ErrorHandler != null)
					{
						this.ErrorHandler(extractDirectory + fileName, ex.Message, false);
					}
				}
			}
			return result;
		}
	}
}
