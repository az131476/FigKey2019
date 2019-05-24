using Ionic.Zip;
using System;
using System.IO;
using System.Linq;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	internal class ZipArchiveFile : IFile, IDisposable
	{
		private string path;

		private string file;

		private string entry;

		private string singleRootDirName;

		private string password;

		private ZipFile Zip;

		private ZipFile nestedZip;

		private MemoryStream nestedZipStream;

		private static bool ignoreSingleRoot = true;

		public bool HasSingleRoot
		{
			get
			{
				return !string.IsNullOrEmpty(this.singleRootDirName);
			}
		}

		public string ZipEntryName
		{
			get
			{
				if (!this.HasSingleRoot)
				{
					return this.entry;
				}
				return this.singleRootDirName + this.entry;
			}
		}

		public string Name
		{
			get
			{
				return Path.GetFileName(this.path);
			}
		}

		public string FullName
		{
			get
			{
				return ((IFile)this).Path;
			}
		}

		public string DirectoryName
		{
			get
			{
				return Path.GetDirectoryName(this.path);
			}
		}

		public string Extension
		{
			get
			{
				return Path.GetExtension(this.path);
			}
		}

		public bool Exists
		{
			get
			{
				return ((IFile)this).Exists();
			}
		}

		public long Length
		{
			get
			{
				ZipEntry zipEntry = (this.Zip != null) ? this.Zip[this.ZipEntryName] : null;
				if (zipEntry == null)
				{
					zipEntry = this.GetNestedZipEntry();
				}
				if (zipEntry != null)
				{
					return (long)Convert.ToInt32(zipEntry.UncompressedSize);
				}
				return -1L;
			}
		}

		public DateTime LastWriteTime
		{
			get
			{
				ZipEntry zipEntry = (this.Zip != null) ? this.Zip[this.ZipEntryName] : null;
				if (zipEntry == null)
				{
					zipEntry = this.GetNestedZipEntry();
				}
				if (zipEntry != null)
				{
					return zipEntry.ModifiedTime.ToLocalTime();
				}
				return default(DateTime);
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public bool IgnoreSingleRoot
		{
			get
			{
				return ZipArchiveFile.ignoreSingleRoot;
			}
			set
			{
				ZipArchiveFile.ignoreSingleRoot = value;
			}
		}

		string IFile.Path
		{
			get
			{
				return this.path;
			}
		}

		public ZipArchiveFile(string zipPath, string zipFile, string zipEntry, string zipPassword = "")
		{
			this.singleRootDirName = "";
			this.path = zipPath;
			this.file = zipFile;
			this.entry = zipEntry;
			this.Zip = ZipFile.Read(this.file);
			if (!string.IsNullOrEmpty(zipPassword))
			{
				this.Zip.Password = zipPassword;
				this.password = zipPassword;
			}
			if (ZipArchiveFile.ignoreSingleRoot)
			{
				this.CheckForSingleRoot();
			}
		}

		public ZipArchiveFile(string zipPath, string zipPassword = "")
		{
			this.singleRootDirName = "";
			this.path = zipPath;
			ZipArchiveFile.SplitZipPath(this.path, out this.file, out this.entry);
			this.Zip = ZipFile.Read(this.file);
			if (!string.IsNullOrEmpty(zipPassword))
			{
				this.Zip.Password = zipPassword;
			}
			if (ZipArchiveFile.ignoreSingleRoot)
			{
				this.CheckForSingleRoot();
			}
		}

		public void Dispose()
		{
			if (this.Zip != null)
			{
				this.Zip.Dispose();
				this.Zip = null;
			}
			if (this.nestedZip != null)
			{
				this.nestedZip.Dispose();
				this.nestedZip = null;
			}
			if (this.nestedZipStream != null)
			{
				this.nestedZipStream.Dispose();
				this.nestedZipStream = null;
			}
		}

		bool IFile.Exists()
		{
			bool flag = File.Exists(this.file);
			bool flag2 = false;
			if (this.Zip != null && flag && !string.IsNullOrEmpty(this.entry))
			{
				flag2 = (this.Zip.ContainsEntry(this.ZipEntryName) || this.GetNestedZipEntry() != null);
			}
			return flag && flag2;
		}

		StreamReader IFile.OpenText()
		{
			StreamReader result = null;
			ZipEntry zipEntry = (this.Zip != null) ? this.Zip[this.ZipEntryName] : null;
			if (zipEntry == null)
			{
				zipEntry = this.GetNestedZipEntry();
			}
			if (zipEntry != null)
			{
				Stream stream = new MemoryStream();
				zipEntry.Extract(stream);
				result = new StreamReader(stream);
				stream.Seek(0L, SeekOrigin.Begin);
			}
			return result;
		}

		Stream IFile.OpenRead()
		{
			Stream stream = null;
			ZipEntry zipEntry = (this.Zip != null) ? this.Zip[this.ZipEntryName] : null;
			if (zipEntry == null)
			{
				zipEntry = this.GetNestedZipEntry();
			}
			if (zipEntry != null)
			{
				stream = new MemoryStream();
				zipEntry.Extract(stream);
				stream.Seek(0L, SeekOrigin.Begin);
			}
			return stream;
		}

		void IFile.Copy(string destFileName)
		{
			if (File.Exists(destFileName))
			{
				throw new IOException();
			}
			((IFile)this).Copy(destFileName, true);
		}

		void IFile.Copy(string destFileName, bool overwrite)
		{
			if (!overwrite && File.Exists(destFileName))
			{
				throw new IOException();
			}
			ZipEntry zipEntry = (this.Zip != null) ? this.Zip[this.ZipEntryName] : null;
			if (zipEntry == null)
			{
				zipEntry = this.GetNestedZipEntry();
			}
			if (zipEntry != null)
			{
				FileStream fileStream = null;
				try
				{
					fileStream = File.OpenWrite(destFileName);
					zipEntry.Extract(fileStream);
					fileStream.Close();
				}
				catch (Exception ex)
				{
					if (fileStream != null)
					{
						fileStream.Close();
					}
					throw ex;
				}
			}
		}

		public static bool SplitZipPath(string path, out string zipFile, out string zipEntry)
		{
			bool flag = false;
			zipFile = "";
			zipEntry = "";
			bool flag2 = false;
			try
			{
				Uri uri = new Uri(path);
				flag2 = uri.IsUnc;
			}
			catch
			{
			}
			if (path.ToLower().Contains(".zip"))
			{
				string[] array = path.Split(new char[]
				{
					Path.DirectorySeparatorChar
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i];
					if (!string.IsNullOrWhiteSpace(text))
					{
						if (!flag)
						{
							if (!string.IsNullOrEmpty(zipFile))
							{
								zipFile += Path.DirectorySeparatorChar;
							}
							else if (flag2)
							{
								zipFile = "\\\\";
							}
							zipFile += text;
							if (text.ToLower().EndsWith(".zip") && ZipFile.IsZipFile(zipFile))
							{
								flag = true;
							}
						}
						else
						{
							if (!string.IsNullOrEmpty(zipEntry))
							{
								zipEntry += '/';
							}
							zipEntry += text;
						}
					}
				}
			}
			return flag && !string.IsNullOrEmpty(zipEntry);
		}

		private void CheckForSingleRoot()
		{
			ZipEntry zipEntry = (this.Zip != null) ? this.Zip.EntriesSorted.FirstOrDefault<ZipEntry>() : null;
			if (zipEntry != null)
			{
				string[] array = zipEntry.FileName.Split(new char[]
				{
					'/'
				});
				string value = "";
				if (array.Count<string>() > 1)
				{
					value = array[0] + "/";
				}
				else
				{
					value = zipEntry.FileName;
				}
				foreach (ZipEntry current in this.Zip.EntriesSorted)
				{
					if (!current.FileName.StartsWith(value))
					{
						return;
					}
				}
				if (!this.entry.StartsWith(value))
				{
					this.singleRootDirName = value;
				}
			}
		}

		private ZipEntry GetNestedZipEntry()
		{
			ZipEntry result = null;
			try
			{
				if (this.ZipEntryName.LastIndexOf(".zip/", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					string fileName = this.ZipEntryName.Remove(this.ZipEntryName.LastIndexOf('/'));
					ZipEntry zipEntry = this.Zip[fileName];
					if (zipEntry != null)
					{
						this.nestedZipStream = new MemoryStream(Convert.ToInt32(zipEntry.UncompressedSize));
						zipEntry.Extract(this.nestedZipStream);
						this.nestedZipStream.Seek(0L, SeekOrigin.Begin);
						this.nestedZip = ZipFile.Read(this.nestedZipStream);
						this.nestedZip.Password = this.password;
						string fileName2 = this.ZipEntryName.Remove(0, this.ZipEntryName.LastIndexOf('/') + 1);
						result = this.nestedZip[fileName2];
					}
				}
			}
			catch
			{
			}
			return result;
		}
	}
}
