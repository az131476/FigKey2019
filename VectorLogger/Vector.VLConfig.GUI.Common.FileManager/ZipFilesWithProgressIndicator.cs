using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class ZipFilesWithProgressIndicator : IDisposable
	{
		private ProgressIndicatorForm pi;

		private List<string> FileList
		{
			get;
			set;
		}

		private string Destination
		{
			get;
			set;
		}

		public ZipFilesWithProgressIndicator()
		{
			this.pi = new ProgressIndicatorForm();
			this.pi.ConfirmCancel = true;
		}

		public void Dispose()
		{
			this.pi.Dispose();
		}

		public bool Zip(List<string> fileList, string destination)
		{
			this.FileList = fileList;
			this.Destination = destination;
			this.pi.Text = string.Format(Resources.ZipCreatingArchive, Path.GetFileName(destination));
			Thread thread = new Thread(new ThreadStart(this.DoZip));
			thread.CurrentUICulture = new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name);
			thread.Start();
			this.pi.ShowDialog();
			if (this.pi.Cancelled() && thread.IsAlive)
			{
				thread.Abort();
			}
			return !this.pi.Cancelled();
		}

		private void DoZip()
		{
			if (!string.IsNullOrWhiteSpace(this.Destination) && this.FileList.Count<string>() > 0)
			{
				using (ZipFile zipFile = new ZipFile(this.Destination))
				{
					if (!this.pi.Cancelled())
					{
						this.PrepareZip(zipFile);
					}
					if (!this.pi.Cancelled())
					{
						this.SaveZip(zipFile);
					}
					if (!this.pi.Cancelled())
					{
						this.pi.ProcessExited();
					}
				}
			}
		}

		private void PrepareZip(ZipFile zip)
		{
			this.pi.SetStatusText(Resources.ZipPreparingData);
			this.pi.SetValue(-1);
			if (zip != null)
			{
				if (zip.Entries.Any<ZipEntry>())
				{
					List<ZipEntry> entriesToRemove = new List<ZipEntry>(zip.Entries);
					zip.RemoveEntries(entriesToRemove);
					zip.Save();
				}
				if (this.pi.Cancelled())
				{
					return;
				}
				foreach (string current in this.FileList)
				{
					if (ZipFile.IsZipFile(current))
					{
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(current);
						ZipFile zipFile = new ZipFile(current);
						using (IEnumerator<ZipEntry> enumerator2 = zipFile.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								ZipEntry current2 = enumerator2.Current;
								if (this.pi.Cancelled())
								{
									return;
								}
								zip.AddEntry(fileNameWithoutExtension + "\\" + current2.FileName, current2.OpenReader());
							}
							continue;
						}
					}
					if (this.pi.Cancelled())
					{
						return;
					}
					if (current.EndsWith("*"))
					{
						zip.AddFile(current.TrimEnd(new char[]
						{
							'*'
						}));
					}
					else
					{
						zip.AddFile(current);
					}
				}
			}
			this.pi.SetMinMaxInvoked(0, (zip == null) ? 0 : zip.Entries.Count<ZipEntry>());
		}

		private void SaveZip(ZipFile zip)
		{
			zip.SaveProgress += delegate(object sender, SaveProgressEventArgs e)
			{
				if (this.pi.Cancelled())
				{
					e.Cancel = true;
				}
				if (e.EventType == ZipProgressEventType.Saving_BeforeWriteEntry)
				{
					this.pi.SetStatusText(string.Format(Resources.ZipAddingFile, e.CurrentEntry.FileName));
					this.pi.SetValue(e.EntriesSaved);
				}
			};
			zip.ParallelDeflateThreshold = -1L;
			try
			{
				zip.Save();
			}
			catch (SystemException ex)
			{
				InformMessageBox.Show(EnumInfoType.Error, Resources.ErrorFailedToZipLoggingData, new string[]
				{
					ex.Message
				});
			}
		}
	}
}
