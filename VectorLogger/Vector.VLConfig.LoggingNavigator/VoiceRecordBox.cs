using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using Vector.VLConfig.LoggingNavigator.Data;
using Vector.VLConfig.LoggingNavigator.Properties;

namespace Vector.VLConfig.LoggingNavigator
{
	internal class VoiceRecordBox : PictureBox
	{
		private VoiceRecord mVoiceRecord;

		private double mRecordDuration;

		private SoundPlayer mSoundPlayer;

		private TimeLineControl mTimeLine;

		private bool mIsPlaying;

		private Timer mRecordLengthTimer = new Timer();

		public VoiceRecord VoiceRecord
		{
			get
			{
				return this.mVoiceRecord;
			}
		}

		public bool IsPlaying
		{
			get
			{
				return this.mIsPlaying;
			}
			set
			{
				this.mIsPlaying = value;
			}
		}

		public string Tooltip
		{
			get
			{
				TimeSpan timeSpan = TimeSpan.FromSeconds(this.mRecordDuration);
				return string.Concat(new string[]
				{
					this.mVoiceRecord.Tooltip,
					"\n",
					Resources.Duration,
					": ",
					string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds)
				});
			}
		}

		public VoiceRecordBox(TimeLineControl timeLineControl, VoiceRecord vr)
		{
			this.mTimeLine = timeLineControl;
			this.mVoiceRecord = vr;
			this.BackColor = Color.Transparent;
			base.Width = 13;
			base.Height = 13;
			this.mSoundPlayer = new SoundPlayer();
			this.mIsPlaying = false;
			if (FileAccessManager.GetInstance().ExistsOnFilesystem(this.mVoiceRecord.Path))
			{
				long filesSize = FileAccessManager.GetInstance().GetFilesSize(vr.Path);
				int num;
				using (Stream wavFileAsStream = FileAccessManager.GetInstance().GetWavFileAsStream(vr.Path))
				{
					if (wavFileAsStream.Length <= 32L)
					{
						this.mVoiceRecord.FileError = true;
						this.mRecordDuration = 0.0;
						this.RefreshIcon();
						return;
					}
					wavFileAsStream.Seek(28L, SeekOrigin.Begin);
					byte[] array = new byte[4];
					wavFileAsStream.Read(array, 0, 4);
					num = BitConverter.ToInt32(array, 0);
				}
				if (num > 0)
				{
					this.mRecordDuration = (double)(filesSize / (long)num);
					this.mRecordLengthTimer.Interval = ((int)this.mRecordDuration + 2) * 1000;
					this.mRecordLengthTimer.Tick += new EventHandler(this.RecordLengthTimer_Tick);
					this.mRecordLengthTimer.Enabled = true;
					base.MouseClick += new MouseEventHandler(this.TooglePlay);
					goto IL_19F;
				}
				this.mVoiceRecord.FileError = true;
				this.mRecordDuration = 0.0;
				this.RefreshIcon();
				return;
			}
			this.mVoiceRecord.FileError = true;
			this.mRecordDuration = 0.0;
			IL_19F:
			this.RefreshIcon();
		}

		public void RefreshIcon()
		{
			if (this.mVoiceRecord.FileError)
			{
				base.Image = Resources.speaker_error.ToBitmap();
			}
			else if (this.mIsPlaying)
			{
				base.Image = Resources.speaker_stop.ToBitmap();
			}
			else
			{
				base.Image = Resources.speaker_play.ToBitmap();
			}
			this.Refresh();
		}

		private void TooglePlay(object sender, EventArgs e)
		{
			if (!(sender is VoiceRecordBox))
			{
				return;
			}
			VoiceRecordBox voiceRecordBox = sender as VoiceRecordBox;
			bool isPlaying = voiceRecordBox.IsPlaying;
			if (this.mTimeLine != null)
			{
				this.mTimeLine.StopAllSounds();
			}
			if (!isPlaying)
			{
				if (FileAccessManager.GetInstance().ExistsOnFilesystem(this.mVoiceRecord.Path))
				{
					voiceRecordBox.IsPlaying = true;
					try
					{
						this.mSoundPlayer.Stream = FileAccessManager.GetInstance().GetWavFileAsStream(this.mVoiceRecord.Path);
						this.mSoundPlayer.Play();
					}
					catch (Exception)
					{
						this.mVoiceRecord.FileError = true;
						if (this.mSoundPlayer.Stream != null)
						{
							this.mSoundPlayer.Stream.Close();
						}
					}
					this.mRecordLengthTimer.Start();
				}
				else
				{
					this.mVoiceRecord.FileError = true;
				}
				voiceRecordBox.RefreshIcon();
			}
		}

		public void Stop()
		{
			this.mIsPlaying = false;
			if (this.mSoundPlayer != null)
			{
				this.mSoundPlayer.Stop();
				if (this.mSoundPlayer.Stream != null)
				{
					this.mSoundPlayer.Stream.Close();
				}
			}
			if (this.mRecordLengthTimer != null)
			{
				this.mRecordLengthTimer.Stop();
			}
			this.RefreshIcon();
		}

		private void RecordLengthTimer_Tick(object sender, EventArgs e)
		{
			this.Stop();
		}
	}
}
