using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common
{
	public class GenericSaveFileDialog
	{
		private static SaveFileDialog _saveAsDialog;

		private static Dictionary<FileType, string> FileFilter;

		private static Dictionary<FileType, string> Extension;

		private static SaveFileDialog SaveAsDialog
		{
			get
			{
				if (GenericSaveFileDialog._saveAsDialog == null)
				{
					GenericSaveFileDialog._saveAsDialog = new SaveFileDialog();
				}
				return GenericSaveFileDialog._saveAsDialog;
			}
			set
			{
				if (value != null)
				{
					GenericSaveFileDialog._saveAsDialog = value;
				}
			}
		}

		public static string FileName
		{
			get
			{
				return GenericSaveFileDialog.SaveAsDialog.FileName;
			}
		}

		static GenericSaveFileDialog()
		{
			GenericSaveFileDialog.FileFilter = new Dictionary<FileType, string>();
			GenericSaveFileDialog.FileFilter.Add(FileType.ProjectFile, Vocabulary.FileFilterGLC);
			GenericSaveFileDialog.FileFilter.Add(FileType.LTLFile, Resources_Files.FileFilterLTL);
			GenericSaveFileDialog.FileFilter.Add(FileType.CODFile, Resources_Files.FileFilterCOD);
			GenericSaveFileDialog.FileFilter.Add(FileType.INCFile, Resources_Files.FileFilterIncludeFile);
			GenericSaveFileDialog.FileFilter.Add(FileType.ZIPFile, Resources_Files.FileFilterZIP);
			GenericSaveFileDialog.FileFilter.Add(FileType.CompiledCAPLFile, Resources_Files.FileFilterCompiledCAPL);
			GenericSaveFileDialog.FileFilter.Add(FileType.PackAndGoFile, Resources_Files.FileFilterPackAndGo);
			GenericSaveFileDialog.Extension = new Dictionary<FileType, string>();
			GenericSaveFileDialog.Extension.Add(FileType.ProjectFile, "glc");
			GenericSaveFileDialog.Extension.Add(FileType.LTLFile, "ltl");
			GenericSaveFileDialog.Extension.Add(FileType.CODFile, "cod");
			GenericSaveFileDialog.Extension.Add(FileType.INCFile, "inc");
			GenericSaveFileDialog.Extension.Add(FileType.ZIPFile, "zip");
			GenericSaveFileDialog.Extension.Add(FileType.CompiledCAPLFile, "bin");
			GenericSaveFileDialog.Extension.Add(FileType.PackAndGoFile, "zip");
		}

		public static DialogResult ShowDialog(FileType fileType, string projectFilename, string loggerName, string pathToProjectPlace)
		{
			GenericSaveFileDialog.SaveAsDialog.FileName = GenericSaveFileDialog.GetSuggestedFilename(fileType, projectFilename, loggerName);
			GenericSaveFileDialog.SaveAsDialog.Filter = GenericSaveFileDialog.FileFilter[fileType];
			GenericSaveFileDialog.SaveAsDialog.CustomPlaces.Clear();
			if (!string.IsNullOrEmpty(pathToProjectPlace))
			{
				GenericSaveFileDialog.SaveAsDialog.CustomPlaces.Add(pathToProjectPlace);
				GenericSaveFileDialog.SaveAsDialog.InitialDirectory = pathToProjectPlace;
			}
			return GenericSaveFileDialog.SaveAsDialog.ShowDialog();
		}

		public static string GetSuggestedFilename(FileType fileType, string projectFilename, string loggerName)
		{
			string result;
			if (fileType == FileType.CompiledCAPLFile)
			{
				result = Vocabulary.FileNameCompiledCAPL;
			}
			else if (projectFilename.Length > 0)
			{
				result = Path.ChangeExtension(projectFilename, GenericSaveFileDialog.Extension[fileType]);
			}
			else if (fileType == FileType.INCFile)
			{
				result = string.Format(Resources.DefaultFileNameFormatINC, loggerName, GenericSaveFileDialog.Extension[fileType]);
			}
			else
			{
				result = string.Format(Resources.DefaultFileNameFormat, loggerName, GenericSaveFileDialog.Extension[fileType]);
			}
			return result;
		}
	}
}
