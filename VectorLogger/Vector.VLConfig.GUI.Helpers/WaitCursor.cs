using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Vector.VLConfig.GUI.Helpers
{
	public class WaitCursor : IDisposable
	{
		public static bool Enabled
		{
			get
			{
				return Application.UseWaitCursor;
			}
			set
			{
				if (value == Application.UseWaitCursor)
				{
					return;
				}
				Application.UseWaitCursor = value;
				Form activeForm = Form.ActiveForm;
				if (activeForm != null)
				{
					IntPtr arg_24_0 = activeForm.Handle;
					WaitCursor.SendMessage(activeForm.Handle, 32, activeForm.Handle, (IntPtr)1);
				}
			}
		}

		public WaitCursor()
		{
			WaitCursor.Enabled = true;
		}

		public void Dispose()
		{
			WaitCursor.Enabled = false;
		}

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
	}
}
