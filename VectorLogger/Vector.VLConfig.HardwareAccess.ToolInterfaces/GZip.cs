using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	internal class GZip : GenericToolInterface
	{
		private string pathToFolder;

		private bool isUsingSubFolders;

		private IProgressIndicator indicator;

		private Thread decompressThread;

		private ProcessExitedDelegate threadExitedDelegate;

		public string ErrorText
		{
			get;
			set;
		}

		public GZip()
		{
			base.FileName = "gzip.exe";
			this.pathToFolder = "";
			this.isUsingSubFolders = false;
		}

		public bool DecompressFile(string filePath, out string errorText)
		{
			if (!File.Exists(filePath))
			{
				errorText = string.Format(Resources.ErrorFileWithNameNotFound, filePath);
				return false;
			}
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("-df");
			base.AddCommandLineArgument(string.Format("\"{0}\"", filePath));
			base.RunSynchronous();
			if (base.LastExitCode != 0)
			{
				errorText = string.Format(Resources.ErrorFailedToDecompressFile, Path.GetFileName(filePath));
				return false;
			}
			errorText = "";
			return true;
		}

		public void DecompressFiles(string filePathToCompressedFiles, bool useSubFolders, IProgressIndicator pi, ProcessExitedDelegate processExitedDelegate)
		{
			this.ErrorText = "";
			this.indicator = pi;
			this.threadExitedDelegate = processExitedDelegate;
			this.pathToFolder = filePathToCompressedFiles;
			this.isUsingSubFolders = useSubFolders;
			this.decompressThread = new Thread(new ThreadStart(this.DecompressThreadProc));
			this.decompressThread.Name = "DecompressFiles";
			this.decompressThread.Start();
			this.decompressThread.Join();
		}

		public void DecompressThreadProc()
		{
			if (string.IsNullOrEmpty(this.pathToFolder))
			{
				this.ErrorText = Resources.ErrorFolderNotFound;
				this.threadExitedDelegate();
				return;
			}
			List<string> list = new List<string>();
			if (this.isUsingSubFolders)
			{
				string[] directories = Directory.GetDirectories(this.pathToFolder, Constants.LogDataFolderSearchPattern, SearchOption.TopDirectoryOnly);
				string[] array = directories;
				for (int i = 0; i < array.Length; i++)
				{
					string path = array[i];
					list.AddRange(Directory.EnumerateFiles(path, "*.gz"));
				}
			}
			else
			{
				list.AddRange(Directory.EnumerateFiles(this.pathToFolder, "*.gz"));
			}
			int num = list.Count<string>();
			this.indicator.SetMinMax(0, num);
			this.indicator.SetStatusText(string.Format(Resources.NumOfTotalDecompressed, 0, num));
			this.indicator.SetValue(0);
			int num2 = 0;
			foreach (string current in list)
			{
				string errorText;
				if (!this.DecompressFile(current, out errorText))
				{
					this.ErrorText = errorText;
					this.threadExitedDelegate();
					return;
				}
				num2++;
				this.indicator.SetStatusText(string.Format(Resources.NumOfTotalDecompressed, num2, num));
				this.indicator.SetValue(num2);
			}
			this.threadExitedDelegate();
		}
	}
}
