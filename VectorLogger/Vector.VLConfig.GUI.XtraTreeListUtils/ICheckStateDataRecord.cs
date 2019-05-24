using System;
using System.Windows.Forms;

namespace Vector.VLConfig.GUI.XtraTreeListUtils
{
	internal interface ICheckStateDataRecord
	{
		bool CanCheck
		{
			get;
		}

		CheckState CheckState
		{
			get;
			set;
		}
	}
}
