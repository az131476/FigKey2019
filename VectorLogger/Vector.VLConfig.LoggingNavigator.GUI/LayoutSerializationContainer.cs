using DevExpress.XtraGrid.Views.Grid;
using System;
using System.IO;

namespace Vector.VLConfig.LoggingNavigator.GUI
{
	public class LayoutSerializationContainer
	{
		public string GridViewMeasurementsLayout;

		public string GridControlMarkerLayout;

		public string GridControlLogFilesLayout;

		public string GridControlTriggerLayout;

		public string GridControlMarkerSelectionTableLayout;

		public string GridControlTriggerSelectionTableLayout;

		public LayoutSerializationContainer()
		{
			this.GridViewMeasurementsLayout = "";
			this.GridControlMarkerLayout = "";
			this.GridControlLogFilesLayout = "";
			this.GridControlTriggerLayout = "";
			this.GridControlMarkerSelectionTableLayout = "";
			this.GridControlTriggerSelectionTableLayout = "";
		}

		public static string SerializeGridComponent(GridView control)
		{
			string result = "";
			if (control == null)
			{
				return result;
			}
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				control.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				result = Convert.ToBase64String(array, 0, array.Length);
			}
			catch
			{
				result = "";
			}
			finally
			{
				if (memoryStream != null)
				{
					memoryStream.Close();
				}
			}
			return result;
		}

		public static void DeSerializeGridComponent(GridView control, string layout)
		{
			if (control == null || string.IsNullOrEmpty(layout))
			{
				return;
			}
			MemoryStream memoryStream = null;
			try
			{
				byte[] buffer = Convert.FromBase64String(layout);
				memoryStream = new MemoryStream(buffer);
				control.RestoreLayoutFromStream(memoryStream);
			}
			catch
			{
			}
			finally
			{
				if (memoryStream != null)
				{
					memoryStream.Close();
				}
			}
		}
	}
}
