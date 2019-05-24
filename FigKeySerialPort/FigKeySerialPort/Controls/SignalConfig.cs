using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using Telerik.WinControls.UI;
using CommonUtils.ByteHelper;
using CommonUtils.Logger;

namespace FigKeySerialPort.Controls
{
    public class SignalConfig
    {
        private int curGroupId;
        private RadGridView dgv;

        #region 限制输入范围
        public const string TICKS_LIMIT_HEX = "0X0110-0X0FFF";
        public const string TICKS_LIMIT_DEC = "272-4095";

        public const string QUICK_DATA1_LIMIT_HEX = "0-0X0FFF";
        public const string QUICK_DATA1_LIMIT_DEC = "0-4095";

        public const string QUICK_DATA2_LIMIT_HEX = "0-0X0FF";
        public const string QUICK_DATA2_LIMIT_DEC = "0-4095";

        public const string SLOW_GROUP_COUNT_LIMIT_HEX = "0-0X32";
        public const string SLOW_GROUP_COUNT_LIMIT_DEC = "0-50";

        #endregion

        #region 慢信号常量
        public const string GROUP_ORDER = "序号(十进制)";

        public const string GROUP_ID_HEX = "串行ID(max：FF)";
        public const string GROUP_DATA_HEX = "慢信号数据(max：0X0FFF)";

        public const string GROUP_ID_DEC = "串行ID(max：255)";
        public const string GROUP_DATA_DEC = "慢信号数据(max：4095)";
        #endregion

        #region 基础信号常量
        public const string DATA_FRAME_SHORT = "短标准串行消息";
        public const string DATA_FRAME_LONG = "长标准串行消息";

        public const string BATTERY_LOW = "空闲状态为低电平";
        public const string BATTERY_HIGH = "空闲状态为高电平";

        public const string SERIAL_MESSAGE_12 = "消息为12bits";
        public const string SERIAL_MESSAGE_16 = "消息为16bits";

        public const string TIME_FRAME_NUMBER = "0X0110";
        #endregion

        #region 快信号常量
        public const string SIGNAL_TYPE1 = "DATA1/DATA2-模拟量数据";
        public const string SIGNAL_TYPE2 = "DATA1-模拟量数据,高4位取反;DATA2帧计数";
        #endregion

        public SignalConfig(RadGridView dgv, int groupid)
        {
            this.dgv = dgv;
            this.curGroupId = groupid;
            SetRadGridView();
        }

        public enum InputStringType
        {
            HEX,
            DEC
        }

        /// <summary>
        /// 输入参数最值限制
        /// </summary>
        public enum InputLimit
        {
            SLOW_GROUP_COUNT_MAX = 50,
            SLOW_GROUP_DATA_MAX = 4095,
            SLOW_GROUP_ID_MAX = 255,
            BASE_TICKS_MIN = 272,
            BASE_TICKS_MAX = 4095,
            QUICK_DATA_MAX = 4095
        }

        /// <summary>
        /// 表头数据为十六进制
        /// </summary>
        public DataTable GetDataTatableHex
        {
            get
            {
                DataTable dxt = new DataTable();
                dxt.Columns.Add(GROUP_ORDER);
                dxt.Columns.Add(GROUP_ID_HEX);
                dxt.Columns.Add(GROUP_DATA_HEX);
                return dxt;
            }
        }

        /// <summary>
        /// 表头数据为十进制
        /// </summary>
        public DataTable GetDataTableDec
        {
            get
            {
                DataTable dct = new DataTable();
                dct.Columns.Add(GROUP_ORDER);
                dct.Columns.Add(GROUP_ID_DEC);
                dct.Columns.Add(GROUP_DATA_DEC);
                return dct;
            }
        }

        #region 初始化慢信号
        /// <summary>
        /// 初始化组数
        /// </summary>
        /// <param name="cobNum"></param>
        public void InitSlowSignal(ComboBox cobNum,RadRadioButton rdbHex,int count)
        {
            cobNum.Items.Clear();
            for (int i = 0; i < count; i++)
            {
                if (rdbHex.CheckState == CheckState.Checked)
                {
                    cobNum.Items.Add(ConvertString.ConvertToHex((i + 1)+"",2));
                }
                else
                {
                    cobNum.Items.Add(i + 1);
                }
            }
            cobNum.SelectedIndex = 0;
            if (rdbHex.CheckState == CheckState.Checked)
            {
                curGroupId = int.Parse(ConvertString.ConvertToDec(cobNum.Text));
            }
            else
            {
                if (!int.TryParse(cobNum.Text.Trim(), out curGroupId))
                {
                    
                }
            }
        }
        #endregion

        #region 初始化快信号
        /// <summary>
        /// 初始化快信号
        /// </summary>
        /// <param name="sigType"></param>
        public void InitQuickSignal(ComboBox sigType,RadCheckBox chx,Label lbx_note)
        {
            sigType.Items.Clear();
            sigType.Items.Add(SIGNAL_TYPE1);
            sigType.Items.Add(SIGNAL_TYPE2);
            sigType.SelectedIndex = 0;

            chx.Visible = true;
            lbx_note.Visible = true;
        }
        #endregion

        #region 初始化基础信号
        /// <summary>
        /// 基本信息初始化
        /// </summary>
        /// <param name="serialMsg"></param>
        /// <param name="batteryState"></param>
        /// <param name="dataframes"></param>
        /// <param name="timeFrames"></param>
        public void InitBaseSignal(ComboBox serialMsg,ComboBox batteryState,ComboBox dataframes,TextBox timeFrames)
        {
            serialMsg.Items.Clear();
            batteryState.Items.Clear();
            dataframes.Items.Clear();
            serialMsg.Items.Add(SERIAL_MESSAGE_12);
            serialMsg.Items.Add(SERIAL_MESSAGE_16);
            batteryState.Items.Add(BATTERY_LOW);
            batteryState.Items.Add(BATTERY_HIGH);
            dataframes.Items.Add(DATA_FRAME_SHORT);
            dataframes.Items.Add(DATA_FRAME_LONG);
            serialMsg.SelectedIndex = 0;
            batteryState.SelectedIndex = 0;
            dataframes.SelectedIndex = 0;
            timeFrames.Text = TIME_FRAME_NUMBER;
        }
        #endregion

        #region 加载组数据
        /// <summary>
        /// 加载慢信号组数据
        /// </summary>
        public void AddGridViewDataSource()
        {
            DataTable dt = GetDataTatableHex;
            Random rdm = new Random();
            for (int i = 0; i < curGroupId; i++)
            {
                DataRow dr = dt.NewRow();
                dr[GROUP_ORDER] = i + 1;
                dr[GROUP_ID_HEX] = ConvertString.ConvertToHex(rdm.Next(255)+"",2);
                dr[GROUP_DATA_HEX] = ConvertString.ConvertToHex(rdm.Next(4095)+"",4);
                dt.Rows.Add(dr);
            }
            dgv.DataSource = dt;
            dgv.Columns[0].BestFit();
        }
        public void AddGridViewDataSource(int num,RadGridView dgvdata,bool IsHex)
        {
            //添加行，保留之前行数
            DataTable dtv = dgvdata.DataSource as DataTable;
            int rowCount = dtv.Rows.Count;
            Random rdm = new Random();
            for (int i = 0; i < num; i++)
            {
                DataRow dr = dtv.NewRow();
                
                if (IsHex)
                {
                    dr[GROUP_ORDER] = rowCount + i + 1;
                    dr[GROUP_ID_HEX] = ConvertString.ConvertToHex(rdm.Next(255)+"",2);
                    dr[GROUP_DATA_HEX] = ConvertString.ConvertToHex(rdm.Next(255)+"",4);
                }
                else
                {
                    dr[GROUP_ORDER] = rowCount + i + 1;
                    dr[GROUP_ID_DEC] = rdm.Next(255);
                    dr[GROUP_DATA_DEC] = rdm.Next(4095);
                }
                dtv.Rows.Add(dr);
            }
            dgvdata.DataSource = dtv;
            dgvdata.Columns[0].BestFit();
        }
        #endregion

        public static void ItemValueToHex(ComboBox cbx)
        {
            cbx.Items.Clear();
            for (int i = 0; i <= 50; i++)
            {
                cbx.Items.Add(ConvertString.ConvertToHex(i+"",2));
            }
        }

        public static void ItemValueToDec(ComboBox cbx)
        {
            for (int i = 0; i <= 50; i++)
            {
                cbx.Items.Add(i);
            }
        }

        #region 设置视图属性
        /// <summary>
        /// 设置gridview属性
        /// </summary>
        private void SetRadGridView()
        {
            dgv.EnableGrouping = false;
            dgv.AllowDrop = true;
            dgv.AllowRowReorder = true;
            /////显示每行前面的标记
            dgv.AddNewRowPosition = SystemRowPosition.Bottom;
            dgv.ShowRowHeaderColumn = true;
            dgv.AutoSizeRows = true;
            dgv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            dgv.AllowAutoSizeColumns = true;
            //dgv.AutoScrollMinSize = new System.Drawing.Size(8, 20);
            dgv.ReadOnly = false;
            //gridView.ColumnChooserSortOrder = RadSortOrder.Ascending;
            //dgv.AllowRowHeaderContextMenu = false;
            
        }
        #endregion

        #region listview
        ListView listView1;
        private void AddListViewData()
        {
            this.listView1.Items.Clear();
            this.listView1.BeginUpdate();

            for (int i = 0; i < curGroupId; i++)
            {
                ListViewItem lvi = new ListViewItem();

                lvi.ImageIndex = i;     //通过与imageList绑定，显示imageList中第i项图标

                lvi.Text = i + 1 + "";

                lvi.SubItems.Add("0F");

                lvi.SubItems.Add("0E");

                this.listView1.Items.Add(lvi);
            }

            this.listView1.EndUpdate();
        }

        private void ShowListViewItem()
        {
            foreach (ListViewItem item in this.listView1.Items)
            {
                for (int i = 0; i < item.SubItems.Count; i++)
                {
                    string itemstr = item.SubItems[i].Text;
                }
            }
        }
        private void InitListView()
        {
            this.listView1.GridLines = true; //显示表格线
            this.listView1.View = System.Windows.Forms.View.Details;//显示表格细节
            //this.listView1.FullRowSelect = true;//是否可以选择行
            this.listView1.LabelEdit = true;


            //设置行高
            ImageList image = new ImageList();
            image.ImageSize = new Size(1, 25);
            this.listView1.SmallImageList = image;

            //listview
            this.listView1.Columns.Add("序号", 120, HorizontalAlignment.Left);
            this.listView1.Columns.Add("data1", 120, HorizontalAlignment.Left);
            this.listView1.Columns.Add("data2", 120, HorizontalAlignment.Left);
        }
        #endregion
    }
}
