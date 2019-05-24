using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.GeneralUtil;

namespace Vector.VLConfig.Data.ConfigurationPersistency
{
	public class ProjectExporter
	{
		private static readonly string glcZipPath = "GLC";

		private static readonly string externalsZipPath = "Externals";

		public event ErrorHandler ErrorHandler;

		private string ExportFileName
		{
			get;
			set;
		}

		private ProjectExporterParameters Parameters
		{
			get;
			set;
		}

		private string ProjectDirectory
		{
			get;
			set;
		}

		public ProjectExporter(string exportFileName, ProjectExporterParameters parameters)
		{
			this.ExportFileName = exportFileName;
			this.Parameters = parameters;
		}

		public bool Export(ref VLProject project, string waterMark)
		{
			Dictionary<string, string> dictionary;
			this.GatherProjectFiles(ref project, out dictionary);
			return this.CreateZipFile(ref dictionary, waterMark);
		}

		private bool GatherProjectFiles(ref VLProject project, out Dictionary<string, string> fileList)
		{
			fileList = new Dictionary<string, string>();
			fileList.Add(project.FilePath, ProjectExporter.glcZipPath);
			this.ProjectDirectory = Path.GetDirectoryName(project.FilePath);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(project.GetProjectFileName());
			List<IFile> list = new List<IFile>();
			foreach (IFeatureReferencedFiles current in ((IFeatureRegistration)project).FeaturesUsingReferencedFiles)
			{
				if (this.Parameters.ExportNonBusDatabases || !(current is GPSConfiguration))
				{
					list.AddRange(current.ReferencedFiles);
				}
			}
			if (!this.Parameters.ExportBusDatabases)
			{
				ReadOnlyCollection<IFile> busDatabases = this.GetBusDatabases(new ReadOnlyCollection<IFile>(list), fileNameWithoutExtension);
				list.RemoveAll(new Predicate<IFile>(busDatabases.Contains));
			}
			if (!this.Parameters.ExportNonBusDatabases)
			{
				ReadOnlyCollection<IFile> nonBusDatabases = this.GetNonBusDatabases(new ReadOnlyCollection<IFile>(list), fileNameWithoutExtension);
				list.RemoveAll(new Predicate<IFile>(nonBusDatabases.Contains));
			}
			if (!this.Parameters.ExportCCPXCPDatabases)
			{
				ReadOnlyCollection<IFile> cCPXCPDatabases = this.GetCCPXCPDatabases(new ReadOnlyCollection<IFile>(list));
				list.RemoveAll(new Predicate<IFile>(cCPXCPDatabases.Contains));
			}
			if (!this.Parameters.ExportDiagnosticDescriptions)
			{
				ReadOnlyCollection<IFile> diagnosticDescriptions = this.GetDiagnosticDescriptions(new ReadOnlyCollection<IFile>(list));
				list.RemoveAll(new Predicate<IFile>(diagnosticDescriptions.Contains));
			}
			if (!this.Parameters.ExportIncludeFiles)
			{
				ReadOnlyCollection<IFile> includeFiles = this.GetIncludeFiles(new ReadOnlyCollection<IFile>(list));
				list.RemoveAll(new Predicate<IFile>(includeFiles.Contains));
			}
			ReadOnlyCollection<IFile> webDisplayFiles = this.GetWebDisplayFiles(new ReadOnlyCollection<IFile>(list));
			if (!this.Parameters.ExportWebDisplay)
			{
				list.RemoveAll(new Predicate<IFile>(webDisplayFiles.Contains));
			}
			else
			{
				List<WebDisplayFile> list2 = new List<WebDisplayFile>();
				foreach (IFile current2 in webDisplayFiles)
				{
					if (current2 is WebDisplayFile)
					{
						list2.AddRange(this.GatherWebDisplayUserFiles(project, current2 as WebDisplayFile));
					}
				}
				list.AddRange(list2);
			}
			this.Gather<IFile>(new ReadOnlyCollection<IFile>(list), ref fileList);
			return true;
		}

		private void Gather<T>(ReadOnlyCollection<T> collection, ref Dictionary<string, string> fileList) where T : IFile
		{
			foreach (T current in collection)
			{
				string value = current.FilePath.Value;
				if (value.Length > 0)
				{
					string value2;
					string text2;
					if (FileSystemServices.IsAbsolutePath(value))
					{
						string text = ProjectExporter.externalsZipPath + Path.DirectorySeparatorChar + (Path.GetDirectoryName(value) ?? string.Empty).TrimStart(new char[]
						{
							Path.DirectorySeparatorChar
						});
						value2 = text.Replace(Path.VolumeSeparatorChar.ToString(), "");
						text2 = value;
					}
					else
					{
						value2 = ProjectExporter.glcZipPath + Path.GetDirectoryName(value);
						text2 = FileSystemServices.GetAbsolutePath(value, this.ProjectDirectory);
					}
					if (File.Exists(text2) && !fileList.ContainsKey(text2))
					{
						fileList.Add(text2, value2);
					}
				}
			}
		}

		private bool CreateZipFile(ref Dictionary<string, string> fileList, string waterMark)
		{
			using (ZipFile zipFile = new ZipFile())
			{
				try
				{
					if (this.Parameters.ProtectWithPassword && this.Parameters.Password != null && this.Parameters.Password.Length > 0)
					{
						zipFile.Password = this.Parameters.Password;
					}
					zipFile.Comment = waterMark;
					zipFile.AlternateEncoding = Encoding.UTF8;
					zipFile.AlternateEncodingUsage = ZipOption.Always;
					foreach (KeyValuePair<string, string> current in fileList)
					{
						zipFile.AddFile(current.Key, current.Value);
					}
					zipFile.Save(this.ExportFileName);
				}
				catch (Exception ex)
				{
					if (this.ErrorHandler != null)
					{
						string errorMsg = string.Format("{0} [{1}]", ex.GetType().ToString(), ex.Message);
						this.ErrorHandler(this.ExportFileName, errorMsg, false);
					}
					return false;
				}
			}
			return true;
		}

		private ReadOnlyCollection<WebDisplayFile> GatherWebDisplayUserFiles(VLProject project, WebDisplayFile index)
		{
			List<WebDisplayFile> list = new List<WebDisplayFile>();
			string path = string.Empty;
			string directoryName = Path.GetDirectoryName(project.FilePath);
			if (FileSystemServices.IsAbsolutePath(index.FilePath.Value))
			{
				path = Path.Combine(Path.GetDirectoryName(index.FilePath.Value), "userfiles");
			}
			else
			{
				path = Path.Combine(Path.GetDirectoryName(FileSystemServices.GetAbsolutePath(index.FilePath.Value, directoryName)), "userfiles");
			}
			this.GetAllFilesOfDirectory(path, ref list, project);
			return new ReadOnlyCollection<WebDisplayFile>(list);
		}

		private void GetAllFilesOfDirectory(string path, ref List<WebDisplayFile> webDisplayFiles, VLProject project)
		{
			try
			{
				Directory.GetAccessControl(path);
				string directoryName = Path.GetDirectoryName(project.FilePath);
				string[] files = Directory.GetFiles(path);
				for (int i = 0; i < files.Length; i++)
				{
					string text = files[i];
					string path2 = text;
					if (FileSystemServices.TryMakeFilePathRelativeToConfiguration(directoryName, ref path2))
					{
						webDisplayFiles.Add(new WebDisplayFile(path2));
					}
					else
					{
						webDisplayFiles.Add(new WebDisplayFile(text));
					}
				}
				string[] directories = Directory.GetDirectories(path);
				for (int j = 0; j < directories.Length; j++)
				{
					string path3 = directories[j];
					this.GetAllFilesOfDirectory(path3, ref webDisplayFiles, project);
				}
			}
			catch (Exception)
			{
			}
		}

		private ReadOnlyCollection<IFile> GetBusDatabases(ReadOnlyCollection<IFile> fileCollection, string projectName)
		{
			List<IFile> list = new List<IFile>();
			foreach (IFile current in fileCollection)
			{
				Database database = current as Database;
				if (database != null && database.IsBusDatabase)
				{
					string fileName = Path.GetFileName(database.FilePath.Value);
					string pattern = string.Format("{0}_((CAN \\d+)|GPS_CAN\\d+).dbc", projectName);
					Match match = Regex.Match(fileName, pattern);
					if (!match.Success)
					{
						list.Add(current);
					}
				}
			}
			return new ReadOnlyCollection<IFile>(list);
		}

		private ReadOnlyCollection<IFile> GetNonBusDatabases(ReadOnlyCollection<IFile> fileCollection, string projectName)
		{
			List<IFile> list = new List<IFile>();
			foreach (IFile current in fileCollection)
			{
				Database database = current as Database;
				if (database != null && database.IsBusDatabase)
				{
					string fileName = Path.GetFileName(database.FilePath.Value);
					string pattern = string.Format("{0}_((CAN \\d+)|GPS_CAN\\d+).dbc", projectName);
					Match match = Regex.Match(fileName, pattern);
					if (match.Success)
					{
						list.Add(current);
					}
				}
			}
			return new ReadOnlyCollection<IFile>(list);
		}

		private ReadOnlyCollection<IFile> GetCCPXCPDatabases(ReadOnlyCollection<IFile> fileCollection)
		{
			List<IFile> list = new List<IFile>();
			foreach (IFile current in fileCollection)
			{
				Database database = current as Database;
				if (current is CPProtection | (database != null && database.CPType.Value != CPType.None))
				{
					list.Add(current);
				}
			}
			return new ReadOnlyCollection<IFile>(list);
		}

		private ReadOnlyCollection<IFile> GetIncludeFiles(ReadOnlyCollection<IFile> fileCollection)
		{
			List<IFile> list = new List<IFile>();
			foreach (IFile current in fileCollection)
			{
				if (current is IncludeFile)
				{
					list.Add(current);
				}
			}
			return new ReadOnlyCollection<IFile>(list);
		}

		private ReadOnlyCollection<IFile> GetWebDisplayFiles(ReadOnlyCollection<IFile> fileCollection)
		{
			List<IFile> list = new List<IFile>();
			foreach (IFile current in fileCollection)
			{
				if (current is WebDisplayFile)
				{
					list.Add(current);
				}
			}
			return new ReadOnlyCollection<IFile>(list);
		}

		private ReadOnlyCollection<IFile> GetDiagnosticDescriptions(ReadOnlyCollection<IFile> fileCollection)
		{
			List<IFile> list = new List<IFile>();
			foreach (IFile current in fileCollection)
			{
				if (current is DiagnosticsDatabase)
				{
					list.Add(current);
				}
			}
			return new ReadOnlyCollection<IFile>(list);
		}
	}
}
