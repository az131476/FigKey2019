using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;

namespace Vector.VLConfig.GUI.MultibusChannelsPage
{
	internal class BaudrateComboboxItem
	{
		private readonly uint baudrate;

		private readonly bool isUserDefined;

		private readonly string specialItemText;

		public BaudrateComboboxItem(uint baudrate)
		{
			this.baudrate = baudrate;
			this.isUserDefined = false;
			this.specialItemText = string.Empty;
		}

		public BaudrateComboboxItem(string specialItemText)
		{
			this.baudrate = 0u;
			this.isUserDefined = false;
			this.specialItemText = specialItemText;
		}

		private BaudrateComboboxItem(uint baudrate, bool isUserDefined)
		{
			this.baudrate = baudrate;
			this.isUserDefined = isUserDefined;
			this.specialItemText = string.Empty;
		}

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.specialItemText))
			{
				return this.specialItemText;
			}
			if (this.isUserDefined)
			{
				return GUIUtil.MapBaudrate2String(this.baudrate) + GUIUtil.UserdefBaudrateValueEntryPostfix;
			}
			return GUIUtil.MapBaudrate2String(this.baudrate);
		}

		public static void Select(ComboBox comboBox, uint baudrate)
		{
			Trace.Assert(comboBox != null);
			BaudrateComboboxItem baudrateComboboxItem = comboBox.Items.OfType<BaudrateComboboxItem>().FirstOrDefault((BaudrateComboboxItem t) => t.baudrate == baudrate);
			if (baudrateComboboxItem != null)
			{
				comboBox.SelectedItem = baudrateComboboxItem;
			}
		}

		public static uint GetSelected(ComboBox comboBox)
		{
			bool flag;
			return BaudrateComboboxItem.GetSelected(comboBox, out flag);
		}

		public static uint GetSelected(ComboBox comboBox, out bool isUserDefined)
		{
			Trace.Assert(comboBox != null);
			BaudrateComboboxItem baudrateComboboxItem = comboBox.SelectedItem as BaudrateComboboxItem;
			Trace.Assert(baudrateComboboxItem != null);
			isUserDefined = baudrateComboboxItem.isUserDefined;
			return baudrateComboboxItem.baudrate;
		}

		public static void InsertUserDefined(ComboBox comboBox, uint baudrate)
		{
			BaudrateComboboxItem.RemoveUserDefined(comboBox);
			int index = comboBox.Items.Count - 1;
			for (int i = 0; i < comboBox.Items.Count; i++)
			{
				BaudrateComboboxItem baudrateComboboxItem = comboBox.Items[i] as BaudrateComboboxItem;
				if (baudrateComboboxItem != null && string.IsNullOrEmpty(baudrateComboboxItem.specialItemText) && baudrateComboboxItem.baudrate > baudrate)
				{
					index = i;
					break;
				}
			}
			BaudrateComboboxItem item = new BaudrateComboboxItem(baudrate, true);
			comboBox.Items.Insert(index, item);
		}

		public static void RemoveUserDefined(ComboBox comboBox)
		{
			BaudrateComboboxItem baudrateComboboxItem = comboBox.Items.OfType<BaudrateComboboxItem>().FirstOrDefault((BaudrateComboboxItem t) => t.isUserDefined);
			if (baudrateComboboxItem != null)
			{
				comboBox.Items.Remove(baudrateComboboxItem);
			}
		}

		public static void SelectUserDefined(ComboBox comboBox)
		{
			BaudrateComboboxItem baudrateComboboxItem = comboBox.Items.OfType<BaudrateComboboxItem>().FirstOrDefault((BaudrateComboboxItem t) => t.isUserDefined);
			if (baudrateComboboxItem != null)
			{
				comboBox.SelectedItem = baudrateComboboxItem;
			}
		}
	}
}
