using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace Vector.VLConfig.GeneralUtil.GUI
{
	public abstract class GenericBackgroundWorkerJob
	{
		private readonly CultureInfo mCultureInfo;

		private int mWeight = 1;

		public int Weight
		{
			get
			{
				return this.mWeight;
			}
			protected set
			{
				this.mWeight = Math.Max(1, value);
			}
		}

		protected abstract void OnDoWork(object sender, DoWorkEventArgs e);

		protected GenericBackgroundWorkerJob()
		{
			this.mCultureInfo = new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name);
		}

		public void DoWork(object sender, DoWorkEventArgs e)
		{
			Thread.CurrentThread.CurrentUICulture = this.mCultureInfo;
			this.OnDoWork(sender, e);
		}
	}
}
