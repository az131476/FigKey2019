using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vector.VLConfig.GUI.Helpers
{
	internal class NumericUpDownEx : NumericUpDown
	{
		private Dictionary<decimal, string> valueTable;

		public void Initialize(decimal minimum, decimal maximum, Dictionary<decimal, string> valueTable)
		{
			base.Minimum = minimum;
			base.Maximum = maximum;
			this.valueTable = valueTable;
			this.UpdateEditText();
		}

		public void Initialize(decimal minimum, decimal maximum)
		{
			this.Initialize(minimum, maximum, null);
		}

		protected override void UpdateEditText()
		{
			string text;
			if (this.valueTable != null && this.valueTable.TryGetValue(base.Value, out text))
			{
				this.Text = text;
				return;
			}
			this.Text = base.Value.ToString();
		}
	}
}
