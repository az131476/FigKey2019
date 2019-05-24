using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	internal class ZipArchiveDirectory : IDirectory, IDisposable
	{
		private string path;

		private string file;

		private string entry;

		private string singleRootDirName;

		private ZipFile Zip;

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

		public bool IgnoreSingleRoot
		{
			get
			{
				return ZipArchiveDirectory.ignoreSingleRoot;
			}
			set
			{
				ZipArchiveDirectory.ignoreSingleRoot = value;
			}
		}

		string IDirectory.Path
		{
			get
			{
				return this.file;
			}
		}

		public ZipArchiveDirectory(string zipPath, string zipFile, string zipEntry)
		{
			this.singleRootDirName = "";
			this.path = zipPath;
			this.file = zipFile;
			this.entry = zipEntry;
			this.Zip = ZipFile.Read(this.file);
			if (ZipArchiveDirectory.ignoreSingleRoot)
			{
				this.CheckForSingleRoot();
			}
		}

		public ZipArchiveDirectory(string zipPath)
		{
			this.singleRootDirName = "";
			this.path = zipPath;
			ZipArchiveDirectory.SplitZipPath(this.path, out this.file, out this.entry);
			this.Zip = ZipFile.Read(this.file);
			if (ZipArchiveDirectory.ignoreSingleRoot)
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
		}

		bool IDirectory.Exists()
		{
			bool flag = File.Exists(this.file);
			if (this.Zip != null && flag && !string.IsNullOrEmpty(this.entry))
			{
				foreach (ZipEntry current in this.Zip)
				{
					if (current.FileName.StartsWith(this.ZipEntryName))
					{
						return true;
					}
				}
				return false;
			}
			return flag;
		}

		string[] IDirectory.GetFiles()
		{
			return ((IDirectory)this).GetFiles("*.*");
		}

		string[] IDirectory.GetFiles(string searchPattern)
		{
			List<string> list = new List<string>();
			if (this.Zip != null)
			{
				string selectionCriteria = "name = '" + this.ZipEntryName + searchPattern + "' AND type = F";
				ICollection<ZipEntry> collection = this.Zip.SelectEntries(selectionCriteria, this.ZipEntryName);
				if (collection.Count > 0)
				{
					using (IEnumerator<ZipEntry> enumerator = collection.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ZipEntry current = enumerator.Current;
							string item = (this.file + Path.DirectorySeparatorChar + current.FileName.Remove(0, this.singleRootDirName.Length)).Replace('/', Path.DirectorySeparatorChar);
							list.Add(item);
						}
						goto IL_C0;
					}
				}
				this.GetNestedZipFiles(searchPattern, list);
			}
			IL_C0:
			return list.ToArray();
		}

		string[] IDirectory.GetFiles(string searchPattern, SearchOption searchOption)
		{
			if (searchOption == SearchOption.TopDirectoryOnly)
			{
				return ((IDirectory)this).GetFiles(searchPattern);
			}
			List<string> list = new List<string>();
			if (this.Zip != null)
			{
				string selectionCriteria = string.Concat(new string[]
				{
					"name = '",
					this.singleRootDirName,
					this.entry,
					searchPattern,
					"' AND type = F"
				});
				ICollection<ZipEntry> collection = this.Zip.SelectEntries(selectionCriteria, null);
				if (collection.Count > 0)
				{
					using (IEnumerator<ZipEntry> enumerator = collection.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ZipEntry current = enumerator.Current;
							string item = (this.file + Path.DirectorySeparatorChar + current.FileName.Remove(0, this.singleRootDirName.Length)).Replace('/', Path.DirectorySeparatorChar);
							list.Add(item);
						}
						goto IL_FF;
					}
				}
				this.GetNestedZipFiles(searchPattern, list);
			}
			IL_FF:
			return list.ToArray();
		}

		private HashSet<string> GetDirectoriesInternal()
		{
			HashSet<string> hashSet = new HashSet<string>();
			if (this.Zip != null)
			{
				string pattern = Regex.Escape(this.ZipEntryName).Replace("/", "\\/") + ".*\\/";
				foreach (ZipEntry current in this.Zip)
				{
					Match match = Regex.Match(current.FileName, pattern);
					if (match.Success)
					{
						string value = match.Value;
						string item = (this.file + Path.DirectorySeparatorChar + value.Remove(0, this.singleRootDirName.Length).TrimEnd(new char[]
						{
							'/'
						})).Replace('/', Path.DirectorySeparatorChar);
						hashSet.Add(item);
					}
				}
			}
			return hashSet;
		}

		private HashSet<string> GetDirectoriesInternal(string searchPattern)
		{
			HashSet<string> hashSet = new HashSet<string>();
			if (this.Zip != null)
			{
				string pattern;
				if (searchPattern == "*.*")
				{
					pattern = Regex.Escape(this.ZipEntryName).Replace("/", "\\/") + ".*\\/";
				}
				else
				{
					pattern = Regex.Escape(this.ZipEntryName).Replace("/", "\\/") + Regex.Escape(searchPattern).Replace("\\*", ".*").Replace("\\?", ".") + "\\/";
				}
				foreach (ZipEntry current in this.Zip)
				{
					Match match = Regex.Match(current.FileName, pattern);
					if (match.Success)
					{
						string value = match.Value;
						string item = (this.file + Path.DirectorySeparatorChar + value.Remove(0, this.singleRootDirName.Length).TrimEnd(new char[]
						{
							'/'
						})).Replace('/', Path.DirectorySeparatorChar);
						hashSet.Add(item);
					}
				}
			}
			return hashSet;
		}

		private HashSet<string> GetDirectoriesInternal(string searchPattern, SearchOption searchOption)
		{
			if (searchOption == SearchOption.TopDirectoryOnly)
			{
				return this.GetDirectoriesInternal(searchPattern);
			}
			HashSet<string> hashSet = new HashSet<string>();
			if (this.Zip != null)
			{
				string pattern;
				if (searchPattern == "*.*")
				{
					pattern = Regex.Escape(this.ZipEntryName).Replace("/", "\\/") + "(.*\\/)*.*\\/";
				}
				else
				{
					pattern = Regex.Escape(this.ZipEntryName).Replace("/", "\\/") + "(.*\\/)*" + Regex.Escape(searchPattern).Replace("\\*", ".*").Replace("\\?", ".") + "\\/";
				}
				foreach (ZipEntry current in this.Zip)
				{
					Match match = Regex.Match(current.FileName, pattern);
					if (match.Success)
					{
						string value = match.Value;
						string item = (this.file + Path.DirectorySeparatorChar + value.Remove(0, this.singleRootDirName.Length).TrimEnd(new char[]
						{
							'/'
						})).Replace('/', Path.DirectorySeparatorChar);
						hashSet.Add(item);
					}
				}
			}
			return hashSet;
		}

		string[] IDirectory.GetDirectories()
		{
			return this.GetDirectoriesInternal().ToArray<string>();
		}

		string[] IDirectory.GetDirectories(string searchPattern)
		{
			return this.GetDirectoriesInternal(searchPattern).ToArray<string>();
		}

		string[] IDirectory.GetDirectories(string searchPattern, SearchOption searchOption)
		{
			return this.GetDirectoriesInternal(searchPattern, searchOption).ToArray<string>();
		}

		IEnumerable<string> IDirectory.EnumerateDirectories()
		{
			return this.GetDirectoriesInternal().AsEnumerable<string>();
		}

		IEnumerable<string> IDirectory.EnumerateDirectories(string searchPattern)
		{
			return this.GetDirectoriesInternal(searchPattern).AsEnumerable<string>();
		}

		IEnumerable<string> IDirectory.EnumerateDirectories(string searchPattern, SearchOption searchOption)
		{
			return this.GetDirectoriesInternal(searchPattern, searchOption).AsEnumerable<string>();
		}

		void IDirectory.GetAccessControl()
		{
			File.GetAccessControl(this.file);
			if (this.Zip != null)
			{
				foreach (ZipEntry current in this.Zip)
				{
					if (current.UsesEncryption)
					{
						throw new UnauthorizedAccessException();
					}
				}
			}
		}

		public static bool SplitZipPath(string path, out string zipFile, out string zipEntry)
		{
			bool flag = false;
			zipEntry = "";
			zipFile = "";
			bool flag2 = false;
			try
			{
				Uri uri = new Uri(path);
				flag2 = uri.IsUnc;
			}
			catch
			{
			}
			if (path.ToLower().EndsWith(".zip") && ZipFile.IsZipFile(path))
			{
				zipFile = path;
				flag = true;
			}
			else if (path.ToLower().Contains(".zip"))
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
							zipEntry += text;
							zipEntry += '/';
						}
					}
				}
			}
			return flag;
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
				else if (zipEntry.IsDirectory)
				{
					value = zipEntry.FileName;
				}
				if (!string.IsNullOrEmpty(value))
				{
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
		}

		private void GetNestedZipFiles(string searchPattern, List<string> fileList)
		{
			try
			{
				if (this.ZipEntryName.EndsWith(".zip/", StringComparison.OrdinalIgnoreCase))
				{
					string fileName = this.ZipEntryName.TrimEnd(new char[]
					{
						'/'
					});
					ZipEntry zipEntry = this.Zip[fileName];
					MemoryStream memoryStream = new MemoryStream(Convert.ToInt32(zipEntry.UncompressedSize));
					zipEntry.Extract(memoryStream);
					memoryStream.Seek(0L, SeekOrigin.Begin);
					if (ZipFile.IsZipFile(memoryStream, false))
					{
						memoryStream.Seek(0L, SeekOrigin.Begin);
						ZipFile zipFile = ZipFile.Read(memoryStream);
						string selectionCriteria = "name = '" + searchPattern + "' AND type = F";
						ICollection<ZipEntry> collection = zipFile.SelectEntries(selectionCriteria, "");
						foreach (ZipEntry current in collection)
						{
							string item = string.Empty;
							if (!string.IsNullOrEmpty(this.singleRootDirName) && current.FileName.IndexOf(this.singleRootDirName) == 0)
							{
								item = string.Concat(new object[]
								{
									this.file,
									Path.DirectorySeparatorChar,
									this.ZipEntryName.Replace('/', Path.DirectorySeparatorChar),
									current.FileName.Remove(0, this.singleRootDirName.Length)
								}).Replace('/', Path.DirectorySeparatorChar);
							}
							else
							{
								item = string.Concat(new object[]
								{
									this.file,
									Path.DirectorySeparatorChar,
									this.ZipEntryName.Replace('/', Path.DirectorySeparatorChar),
									current.FileName
								}).Replace('/', Path.DirectorySeparatorChar);
							}
							fileList.Add(item);
						}
						memoryStream.Close();
					}
				}
			}
			catch
			{
			}
		}
	}
}
