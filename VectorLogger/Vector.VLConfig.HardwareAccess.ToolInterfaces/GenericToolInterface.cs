using Nini.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	public abstract class GenericToolInterface
	{
		private Process toolProcess;

		private bool ioMonitoringInProgress;

		private int creatorThreadID;

		private ProcessExitedDelegate processExitedDelegate;

		private bool isProcessExitSynced;

		private CultureInfo initialCulture;

		protected string fileName;

		private GiNToolErrorCode ginToolErrorCode;

		protected int lastExitCode;

		protected string lastStdOut;

		protected string lastStdErr;

		private IConfigSource iniConfigSource;

		private StreamWriter stdInWriter;

		protected string FileName
		{
			get
			{
				Monitor.Enter(this);
				string result = this.fileName;
				Monitor.Exit(this);
				return result;
			}
			set
			{
				Monitor.Enter(this);
				this.fileName = value;
				Monitor.Exit(this);
			}
		}

		private IList<string> CommandLineArguments
		{
			get;
			set;
		}

		protected string LastStdOut
		{
			get
			{
				Monitor.Enter(this);
				string result = this.lastStdOut;
				Monitor.Exit(this);
				return result;
			}
			set
			{
				Monitor.Enter(this);
				this.lastStdOut = value;
				Monitor.Exit(this);
			}
		}

		protected string LastStdErr
		{
			get
			{
				Monitor.Enter(this);
				string result = this.lastStdErr;
				Monitor.Exit(this);
				return result;
			}
			set
			{
				Monitor.Enter(this);
				this.lastStdErr = value;
				Monitor.Exit(this);
			}
		}

		private IProgressIndicator CurrentProgressIndicator
		{
			get;
			set;
		}

		private IProgressIndicatorValueParser CurrentProgressIndicatorValueParser
		{
			get;
			set;
		}

		protected IConfigSource StdOutAsIniConfigSource
		{
			get
			{
				return this.iniConfigSource;
			}
		}

		protected Encoding CurrentStdOutputTextEncoding
		{
			get;
			set;
		}

		public int LastExitCode
		{
			get
			{
				Monitor.Enter(this);
				int result = this.lastExitCode;
				Monitor.Exit(this);
				return result;
			}
			set
			{
				Monitor.Enter(this);
				this.lastExitCode = value;
				Monitor.Exit(this);
			}
		}

		private bool IOMonitoringInProgress
		{
			get
			{
				Monitor.Enter(this);
				bool result = this.ioMonitoringInProgress;
				Monitor.Exit(this);
				return result;
			}
			set
			{
				Monitor.Enter(this);
				this.ioMonitoringInProgress = value;
				Monitor.Exit(this);
			}
		}

		protected GenericToolInterface()
		{
			this.ioMonitoringInProgress = false;
			this.CommandLineArguments = new List<string>();
			this.creatorThreadID = Thread.CurrentThread.ManagedThreadId;
			this.ginToolErrorCode = new GiNToolErrorCode();
			this.stdInWriter = null;
			this.isProcessExitSynced = false;
			this.initialCulture = new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name);
			this.CurrentStdOutputTextEncoding = Console.OutputEncoding;
			Encoding encoding = null;
			try
			{
				encoding = Encoding.GetEncoding(Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage);
			}
			catch
			{
			}
			if (encoding != null)
			{
				this.CurrentStdOutputTextEncoding = encoding;
			}
		}

		protected bool ParseLastStdOutAsIni()
		{
			StringReader reader = new StringReader(this.LastStdOut);
			bool result;
			try
			{
				this.iniConfigSource = new IniConfigSource(reader);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		protected void ClearIniParser()
		{
			this.iniConfigSource = null;
		}

		protected void DeleteCommandLineArguments()
		{
			Monitor.Enter(this);
			this.CommandLineArguments.Clear();
			Monitor.Exit(this);
		}

		protected void DeleteSpecificCommandLineArgument(string arg)
		{
			Monitor.Enter(this);
			for (int i = 0; i < this.CommandLineArguments.Count; i++)
			{
				if (this.CommandLineArguments[i].Contains(arg))
				{
					this.CommandLineArguments[i] = this.CommandLineArguments[i].Replace(arg, "").Trim();
					if (string.IsNullOrEmpty(this.CommandLineArguments[i]))
					{
						this.CommandLineArguments.RemoveAt(i);
					}
				}
			}
			Monitor.Exit(this);
		}

		protected void AddCommandLineArgument(string commandLineArgument)
		{
			Monitor.Enter(this);
			this.CommandLineArguments.Add(commandLineArgument);
			Monitor.Exit(this);
		}

		private string GetCompleteCommandLine()
		{
			StringBuilder stringBuilder = new StringBuilder(this.FileName);
			stringBuilder.Append(" ");
			stringBuilder.Append(this.GetCommandLineArguments());
			return stringBuilder.ToString();
		}

		private string GetCommandLineArguments()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string current in this.CommandLineArguments)
			{
				stringBuilder.Append(current);
				stringBuilder.Append(" ");
			}
			return stringBuilder.ToString();
		}

		protected int RunSynchronous()
		{
			return this.RunSynchronous(null);
		}

		protected int RunSynchronous(string workingDirectory)
		{
			this.lastStdOut = "";
			this.lastStdErr = "";
			string text = this.FileName;
			string commandLineArguments = this.GetCommandLineArguments();
			ProcessStartInfo processStartInfo = new ProcessStartInfo(text, commandLineArguments);
			processStartInfo.UseShellExecute = false;
			processStartInfo.ErrorDialog = true;
			processStartInfo.RedirectStandardError = true;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.CreateNoWindow = true;
			processStartInfo.StandardOutputEncoding = this.CurrentStdOutputTextEncoding;
			if (workingDirectory != null)
			{
				processStartInfo.WorkingDirectory = workingDirectory;
			}
			this.toolProcess = new Process();
			this.toolProcess.StartInfo = processStartInfo;
			this.toolProcess.OutputDataReceived += new DataReceivedEventHandler(this.DataReceivedInStdOut);
			this.toolProcess.ErrorDataReceived += new DataReceivedEventHandler(this.DataReceivedInErrOut);
			this.toolProcess.Start();
			this.StartIOMonitoring();
			this.toolProcess.WaitForExit();
			this.LastExitCode = this.toolProcess.ExitCode;
			return this.LastExitCode;
		}

		private void DataReceivedInStdOut(object sendingProcess, DataReceivedEventArgs outLine)
		{
			Thread.CurrentThread.CurrentUICulture = this.initialCulture;
			string text = outLine.Data;
			if (!string.IsNullOrEmpty(text))
			{
				text += Environment.NewLine;
			}
			this.LastStdOut += text;
			int value = 0;
			string statusText = "";
			if (this.CurrentProgressIndicatorValueParser != null && this.CurrentProgressIndicator != null)
			{
				bool progressBarValueFromString = this.CurrentProgressIndicatorValueParser.GetProgressBarValueFromString(text, ref value, ref statusText);
				if (progressBarValueFromString && this.IOMonitoringInProgress)
				{
					this.CurrentProgressIndicator.SetValue(value);
					this.CurrentProgressIndicator.SetStatusText(statusText);
				}
			}
		}

		protected virtual void DataReceivedInErrOut(object sendingProcess, DataReceivedEventArgs outLine)
		{
			this.LastStdErr += outLine.Data;
			this.DataReceivedInStdOut(sendingProcess, outLine);
		}

		protected int RunSynchronousWithProgressBar(IProgressIndicator progressIndicator, IProgressIndicatorValueParser progressIndicatorValueParser)
		{
			this.lastStdOut = "";
			this.lastStdErr = "";
			string text = this.FileName;
			string commandLineArguments = this.GetCommandLineArguments();
			ProcessStartInfo processStartInfo = new ProcessStartInfo(text, commandLineArguments);
			processStartInfo.UseShellExecute = false;
			processStartInfo.ErrorDialog = true;
			processStartInfo.RedirectStandardError = true;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.RedirectStandardInput = false;
			processStartInfo.CreateNoWindow = true;
			this.CurrentProgressIndicatorValueParser = progressIndicatorValueParser;
			this.CurrentProgressIndicator = progressIndicator;
			this.toolProcess = new Process();
			this.toolProcess.StartInfo = processStartInfo;
			this.toolProcess.OutputDataReceived += new DataReceivedEventHandler(this.DataReceivedInStdOut);
			this.toolProcess.ErrorDataReceived += new DataReceivedEventHandler(this.DataReceivedInErrOut);
			this.toolProcess.Start();
			this.StartIOMonitoring();
			this.toolProcess.WaitForExit();
			this.CurrentProgressIndicatorValueParser = null;
			this.CurrentProgressIndicator = null;
			this.LastExitCode = this.toolProcess.ExitCode;
			return this.LastExitCode;
		}

		protected void RunAsynchronousWithProgressBar(IProgressIndicator progressIndicator, IProgressIndicatorValueParser progressIndicatorValueParser, ProcessExitedDelegate processExitedDelegate)
		{
			this.lastStdOut = "";
			this.lastStdErr = "";
			this.processExitedDelegate = processExitedDelegate;
			string text = this.FileName;
			string commandLineArguments = this.GetCommandLineArguments();
			ProcessStartInfo processStartInfo = new ProcessStartInfo(text, commandLineArguments);
			processStartInfo.UseShellExecute = false;
			processStartInfo.ErrorDialog = true;
			processStartInfo.RedirectStandardError = true;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.RedirectStandardInput = false;
			processStartInfo.CreateNoWindow = true;
			this.CurrentProgressIndicatorValueParser = progressIndicatorValueParser;
			this.CurrentProgressIndicator = progressIndicator;
			this.toolProcess = new Process();
			this.toolProcess.StartInfo = processStartInfo;
			this.toolProcess.OutputDataReceived += new DataReceivedEventHandler(this.DataReceivedInStdOut);
			this.toolProcess.ErrorDataReceived += new DataReceivedEventHandler(this.DataReceivedInErrOut);
			this.toolProcess.Start();
			this.StartIOMonitoring();
			ThreadStart start = new ThreadStart(this.MonitorProcessExitAndCancelButton);
			new Thread(start)
			{
				Name = "Tool Process Watcher"
			}.Start();
		}

		private void StartIOMonitoring()
		{
			this.IOMonitoringInProgress = true;
			this.toolProcess.BeginOutputReadLine();
			this.toolProcess.BeginErrorReadLine();
		}

		private void CancelIOMonitoring()
		{
			this.IOMonitoringInProgress = false;
			this.toolProcess.CancelErrorRead();
			this.toolProcess.CancelOutputRead();
			this.toolProcess.OutputDataReceived -= new DataReceivedEventHandler(this.DataReceivedInStdOut);
			this.toolProcess.ErrorDataReceived -= new DataReceivedEventHandler(this.DataReceivedInErrOut);
		}

		public virtual void WaitForExitOrKillAfter(int millisecondsBeforeKill)
		{
			if (this.toolProcess == null)
			{
				return;
			}
			DateTime now = DateTime.Now;
			while (!this.toolProcess.HasExited || !this.isProcessExitSynced)
			{
				if (this.CurrentProgressIndicator.Cancelled())
				{
					int milliseconds = DateTime.Now.Subtract(now).Milliseconds;
					if (milliseconds >= millisecondsBeforeKill)
					{
						this.CancelIOMonitoring();
						this.toolProcess.Kill();
						this.toolProcess.WaitForExit();
						return;
					}
				}
				Thread.Sleep(100);
			}
			this.CancelIOMonitoring();
		}

		private void MonitorProcessExitAndCancelButton()
		{
			bool wasCancelled = false;
			while (!this.toolProcess.HasExited)
			{
				if (this.CurrentProgressIndicator.Cancelled())
				{
					this.CancelIOMonitoring();
					if (!this.toolProcess.HasExited)
					{
						this.toolProcess.Kill();
						this.toolProcess.WaitForExit();
					}
					wasCancelled = true;
					IL_6C:
					this.LastExitCode = this.toolProcess.ExitCode;
					int arg_83_0 = this.LastExitCode;
					this.processExitedDelegate();
					this.AsyncProcessExited(wasCancelled);
					return;
				}
				Thread.Sleep(200);
			}
			this.CancelIOMonitoring();
			goto IL_6C;
		}

		private string ReadStringFromOpenStream(StreamReader streamReader)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = streamReader.Peek() != -1;
			while (flag)
			{
				char value = (char)streamReader.Read();
				stringBuilder.Append(value);
				flag = (streamReader.Peek() != -1);
			}
			return stringBuilder.ToString();
		}

		protected void RunSynchronousWithInteraction(IToolOutputParser toolOutputParser)
		{
		}

		protected void WriteToStdIn(string inString)
		{
			Thread.Sleep(200);
			this.stdInWriter.WriteLine(inString);
		}

		protected string GetGinErrorCodeString(int ginErrorCode)
		{
			return this.ginToolErrorCode.GetErrorString(ginErrorCode);
		}

		protected string GetGinErrorCodeString(int ginErrorCode, string ginErrorText)
		{
			return this.ginToolErrorCode.GetErrorString(ginErrorCode, ginErrorText);
		}

		protected virtual void AsyncProcessExited(bool wasCancelled)
		{
			this.isProcessExitSynced = true;
		}

		public virtual string GetLastGinErrorCodeString()
		{
			return this.GetGinErrorCodeString(this.LastExitCode, this.lastStdErr);
		}

		public CANTransceiverType ParseTransceiverEncoding(string encoding)
		{
			return MlRtIniFile.ParseTransceiverEncoding(encoding);
		}
	}
}
