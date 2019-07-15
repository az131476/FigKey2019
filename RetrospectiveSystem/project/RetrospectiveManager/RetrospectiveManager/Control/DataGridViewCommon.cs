using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace RetrospectiveManager.Control
{
    class DataGridViewCommon
    {
        public static void SetRadGridViewProperty(RadGridView radGridView,bool allowAddNewRow)
        {
            radGridView.EnableGrouping = false;
            radGridView.AllowDrop = true;
            radGridView.AllowRowReorder = true;
            //显示新行
            radGridView.AddNewRowPosition = SystemRowPosition.Bottom;
            radGridView.ShowRowHeaderColumn = true;
            radGridView.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            radGridView.ReadOnly = false;
            //gridView.ColumnChooserSortOrder = RadSortOrder.Ascending;
            //dgv.AllowRowHeaderContextMenu = false;
            radGridView.ShowGroupPanel = false;
            radGridView.MasterTemplate.EnableGrouping = false;
            radGridView.MasterTemplate.AllowAddNewRow = allowAddNewRow;
            radGridView.EnableHotTracking = true;
            radGridView.MasterTemplate.SelectLastAddedRow = false;
            //radRadioDataReader.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
        }
    }
}
