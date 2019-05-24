using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common
{
	public class GenericOpenFileDialog
	{
		private static OpenFileDialog _openFileDialog;

		private static Dictionary<FileType, string> FileFilter;

		public static string FileName
		{
			get
			{
				return GenericOpenFileDialog.GetOpenFileDialog().FileName;
			}
			set
			{
				GenericOpenFileDialog.GetOpenFileDialog().FileName = value;
			}
		}

		public static string InitialDirectory
		{
			get
			{
				return GenericOpenFileDialog.GetOpenFileDialog().InitialDirectory;
			}
			set
			{
				GenericOpenFileDialog.GetOpenFileDialog().InitialDirectory = value;
			}
		}

		public static string ProjectFolder
		{
			get
			{
				OpenFileDialog openFileDialog = GenericOpenFileDialog.GetOpenFileDialog();
				if (openFileDialog.CustomPlaces.Count > 0)
				{
					return openFileDialog.CustomPlaces[0].Path;
				}
				return string.Empty;
			}
			set
			{
				OpenFileDialog openFileDialog = GenericOpenFileDialog.GetOpenFileDialog();
				openFileDialog.CustomPlaces.Clear();
				if (!string.IsNullOrEmpty(value))
				{
					openFileDialog.CustomPlaces.Add(value);
				}
			}
		}

		private static OpenFileDialog GetOpenFileDialog()
		{
			if (GenericOpenFileDialog._openFileDialog == null)
			{
				GenericOpenFileDialog._openFileDialog = new OpenFileDialog();
			}
			return GenericOpenFileDialog._openFileDialog;
		}

		static GenericOpenFileDialog()
		{
			GenericOpenFileDialog.FileFilter = new Dictionary<FileType, string>();
			GenericOpenFileDialog.FileFilter.Add(FileType.ProjectFile, Vocabulary.FileFilterGLC + "|" + Vocabulary.FileFilterVLC);
			GenericOpenFileDialog.FileFilter.Add(FileType.LTLFile, Resources_Files.FileFilterLTL);
			GenericOpenFileDialog.FileFilter.Add(FileType.CODFile, Resources_Files.FileFilterCOD);
			GenericOpenFileDialog.FileFilter.Add(FileType.AnyDatabase, Resources_Files.FileFilterDatabaseAll);
			GenericOpenFileDialog.FileFilter.Add(FileType.AnyCANDatabase, Resources_Files.FileFilterDatabaseCAN);
			GenericOpenFileDialog.FileFilter.Add(FileType.AnyCANorLINDatabase, Resources_Files.FileFilterDatabaseCANorLIN);
			GenericOpenFileDialog.FileFilter.Add(FileType.DBCorLDFDatabase, Resources_Files.FileFilterDatabaseCANorLIN);
			GenericOpenFileDialog.FileFilter.Add(FileType.DBCorXMLDatabase, Resources_Files.FileFilterDatabaseDbcAndXML);
			GenericOpenFileDialog.FileFilter.Add(FileType.DBCDatabase, Resources_Files.FileFilterDBC);
			GenericOpenFileDialog.FileFilter.Add(FileType.LDFDatabase, Resources_Files.FileFilterLDF);
			GenericOpenFileDialog.FileFilter.Add(FileType.XMLDatabase, Resources_Files.FileFilterDatabaseXML);
			GenericOpenFileDialog.FileFilter.Add(FileType.SKBFile, Resources_Files.FileFilterSKB);
			GenericOpenFileDialog.FileFilter.Add(FileType.AnyDiagDesc, Resources_Files.FileFilterDiagnosticsDb);
			GenericOpenFileDialog.FileFilter.Add(FileType.CDDDiagDesc, Resources_Files.FileFilterDiagnosticsDbCDD);
			GenericOpenFileDialog.FileFilter.Add(FileType.PDXDiagDesc, Resources_Files.FileFilterDiagnosticsDbPDX);
			GenericOpenFileDialog.FileFilter.Add(FileType.ODXDiagDesc, Resources_Files.FileFilterDiagnosticsDbODX);
			GenericOpenFileDialog.FileFilter.Add(FileType.MDXDiagDesc, Resources_Files.FileFilterDiagnosticsDbMDX);
			GenericOpenFileDialog.FileFilter.Add(FileType.INCFile, Resources_Files.FileFilterIncludeFile);
			GenericOpenFileDialog.FileFilter.Add(FileType.LICFile, Resources_Files.FileFilterLIC);
			GenericOpenFileDialog.FileFilter.Add(FileType.CSVFile, Resources_Files.FileFilterSignalList);
			GenericOpenFileDialog.FileFilter.Add(FileType.ASCorBLFLogFiles, string.Concat(new string[]
			{
				Resources_Files.FileFilterASC,
				"|",
				Resources_Files.FileFilterBLF,
				"|",
				Resources_Files.FileFilterAllFiles
			}));
			GenericOpenFileDialog.FileFilter.Add(FileType.SplitFile, Resources_Files.FileFilterSplit);
			GenericOpenFileDialog.FileFilter.Add(FileType.ZIPFile, Resources_Files.FileFilterZIP);
			GenericOpenFileDialog.FileFilter.Add(FileType.ARXMLDatabase, "arxml");
			GenericOpenFileDialog.FileFilter.Add(FileType.DBCorA2LDatabase, Resources_Files.FileFilterDatabaseDbcAndA2L);
			GenericOpenFileDialog.FileFilter.Add(FileType.DBCorA2LorXMLDatabase, Resources_Files.FileFilterDatabaseDbcAndA2LAndXML);
			GenericOpenFileDialog.FileFilter.Add(FileType.XCPSignalImport, Resources_Files.FileFilterXCPSignal);
			GenericOpenFileDialog.FileFilter.Add(FileType.WebDisplayIndex, Vocabulary.FileFilterWebDisplayIndex);
			GenericOpenFileDialog.FileFilter.Add(FileType.A2LDatabase, Resources_Files.FileFilterA2L);
			GenericOpenFileDialog.FileFilter.Add(FileType.PackAndGoFile, Resources_Files.FileFilterPackAndGo);
			GenericOpenFileDialog.FileFilter.Add(FileType.AnalysisPackage, Resources_Files.FileFilterAnalysisPackage);
		}

		public static DialogResult ShowDialog(FileType fileType)
		{
			return GenericOpenFileDialog.ShowDialog("", fileType);
		}

		public static DialogResult ShowDialog(string titleText, FileType fileType)
		{
			return GenericOpenFileDialog.ShowDialog(titleText, GenericOpenFileDialog.FileFilter[fileType]);
		}

		public static DialogResult ShowDialog(string titleText, string filter)
		{
			OpenFileDialog openFileDialog = GenericOpenFileDialog.GetOpenFileDialog();
			openFileDialog.Title = titleText;
			openFileDialog.Filter = filter;
			DialogResult result = openFileDialog.ShowDialog();
			openFileDialog.InitialDirectory = "";
			return result;
		}
	}
}
