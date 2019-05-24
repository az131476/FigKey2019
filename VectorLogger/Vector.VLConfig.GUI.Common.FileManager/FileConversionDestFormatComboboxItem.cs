using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class FileConversionDestFormatComboboxItem
	{
		public FileConversionDestFormat Format
		{
			get;
			private set;
		}

		public FileConversionDestFormatComboboxItem(FileConversionDestFormat format)
		{
			this.Format = format;
		}

		public override string ToString()
		{
			return FileConversionHelper.FileConversionDestFormat2String(this.Format);
		}

		public static bool IsContainedIn(ComboBox combobox, FileConversionDestFormat format)
		{
			IList<FileConversionDestFormatComboboxItem> source = combobox.Items.OfType<FileConversionDestFormatComboboxItem>().ToList<FileConversionDestFormatComboboxItem>();
			return source.Any((FileConversionDestFormatComboboxItem t) => t.Format == format);
		}

		public static FileConversionDestFormat SelectItem(ComboBox combobox, FileConversionDestFormat format)
		{
			FileConversionDestFormat result = format;
			FileConversionDestFormatComboboxItem fileConversionDestFormatComboboxItem = FileConversionDestFormatComboboxItem.GetItemFrom(combobox, format);
			if (fileConversionDestFormatComboboxItem != null)
			{
				result = fileConversionDestFormatComboboxItem.Format;
				combobox.SelectedItem = fileConversionDestFormatComboboxItem;
			}
			else if (combobox.Items.Count > 0)
			{
				fileConversionDestFormatComboboxItem = (combobox.Items[0] as FileConversionDestFormatComboboxItem);
				if (fileConversionDestFormatComboboxItem != null)
				{
					result = fileConversionDestFormatComboboxItem.Format;
				}
				combobox.SelectedIndex = 0;
			}
			return result;
		}

		public static FileConversionDestFormat GetSelectedFormat(ComboBox combobox, FileConversionDestFormat defaultFormat)
		{
			FileConversionDestFormatComboboxItem fileConversionDestFormatComboboxItem = combobox.SelectedItem as FileConversionDestFormatComboboxItem;
			if (fileConversionDestFormatComboboxItem == null)
			{
				return defaultFormat;
			}
			return fileConversionDestFormatComboboxItem.Format;
		}

		private static FileConversionDestFormatComboboxItem GetItemFrom(ComboBox combobox, FileConversionDestFormat format)
		{
			if (!FileConversionDestFormatComboboxItem.IsContainedIn(combobox, format))
			{
				return null;
			}
			IList<FileConversionDestFormatComboboxItem> source = combobox.Items.OfType<FileConversionDestFormatComboboxItem>().ToList<FileConversionDestFormatComboboxItem>();
			return source.First((FileConversionDestFormatComboboxItem t) => t.Format == format);
		}
	}
}
