using System;
using System.Windows.Forms;
using Vector.UtilityFunctions;

namespace Vector.VLConfig.GUI.Helpers
{
	public interface ISplitButtonExClient
	{
		SplitButton SplitButton
		{
			get;
		}

		string SplitButtonEmptyDefault
		{
			get;
		}

		bool IsItemVisible(ToolStripItem item);

		void ItemClicked(ToolStripItem item);

		void DefaultActionClicked();
	}
}
