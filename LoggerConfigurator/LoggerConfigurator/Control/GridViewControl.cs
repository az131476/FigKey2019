using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.Data;
using Telerik.WinControls.Enumerations;
using FigKeyLoggerConfigurator.Model;
using System.Data;
using AnalysisAgreeMent.Model.XCP;
using System.Windows.Forms;
using CommonUtils.Logger;
using AnalysisAgreeMent.Model;

namespace FigKeyLoggerConfigurator.Control
{
    public class GridViewControl
    {
        private RadGridView gridView;
        private DataTable dataSource;
        
        public GridViewControl(RadGridView view)
        {
            this.gridView = view;
        }

        public void InitGridView()
        {
            AddCollumn();
            SetRadGridView();
        }

        #region 设置视图属性
        /// <summary>
        /// 设置gridview属性
        /// </summary>
        private void SetRadGridView()
        {
            gridView.EnableGrouping = false;
            gridView.AllowDrop = true;
            gridView.AllowRowReorder = true;
            ///显示每行前面的标记
            gridView.AddNewRowPosition = SystemRowPosition.Bottom;
            gridView.ShowRowHeaderColumn = true;
            gridView.AutoSizeRows = true;
            gridView.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            gridView.AllowAutoSizeColumns = true;
            gridView.AutoScrollMinSize = new System.Drawing.Size(8,20);
            gridView.ReadOnly = false;
            //gridView.ColumnChooserSortOrder = RadSortOrder.Ascending;
        }
        #endregion

        #region 添加列
        /// <summary>
        /// 添加table 列
        /// </summary>
        private void AddCollumn()
        {
            //名称+描述+单位+数据类型+数据长度+是否摩托罗拉+开始地址+截取长度+数据地址+系数+偏移量
            dataSource = new DataTable();
            dataSource.Columns.Add(GridViewData.GridViewColumns.ORDER);
            dataSource.Columns.Add(GridViewData.GridViewColumns.NAME);
            dataSource.Columns.Add(GridViewData.GridViewColumns.DESCRIBLE);
            dataSource.Columns.Add(GridViewData.GridViewColumns.UNIT);
            dataSource.Columns.Add(GridViewData.GridViewColumns.TYPE);
            dataSource.Columns.Add(GridViewData.GridViewColumns.DATA_LEN);
            dataSource.Columns.Add(GridViewData.GridViewColumns.BYTE_ORDER);
            dataSource.Columns.Add(GridViewData.GridViewColumns.START_INDEX);
            dataSource.Columns.Add(GridViewData.GridViewColumns.BITDATA_LEN);
            dataSource.Columns.Add(GridViewData.GridViewColumns.DATA_ADDRESS);
            dataSource.Columns.Add(GridViewData.GridViewColumns.FACTOR);
            dataSource.Columns.Add(GridViewData.GridViewColumns.OFF_SET);
        }
        #endregion

        #region 设置列
        private void RefreshRadViewColumn()
        {
            AddCheckBox();
            SetColumnsPeoperties();
            SetCheckValue();
        }
        /// <summary>
        /// 设置列宽
        /// </summary>
        private void SetColumnsPeoperties()
        {
            //for (int i = 0; i < 11; i++)
            //{
            //    gridView.Columns[i].BestFit();
            //}
            //设置只读
            for (int i = 0; i < 8; i++)
            {
                gridView.Columns[i].ReadOnly = true;
            }
            for (int i = 0; i < 11; i++)
            {
                gridView.Columns[i].BestFit();
            }
        }

        /// <summary>
        /// 添加复选框 设置限制时间
        /// </summary>
        private void AddCheckBox()
        {
            GridViewCheckBoxColumn segMent = new GridViewCheckBoxColumn();
            segMent.DataType = typeof(int);
            segMent.Name = "segMent";
            segMent.FieldName = "segMent";
            segMent.HeaderText = "segMent";
            gridView.MasterTemplate.Columns.Add(segMent);
            gridView.Columns[8].BestFit();

            GridViewCheckBoxColumn _10ms = new GridViewCheckBoxColumn();
            _10ms.DataType = typeof(int);
            _10ms.Name = "_10ms";
            _10ms.FieldName = "_10ms";
            _10ms.HeaderText = "10ms";
            gridView.MasterTemplate.Columns.Add(_10ms);
            gridView.Columns[9].BestFit();

            GridViewCheckBoxColumn _100ms = new GridViewCheckBoxColumn();
            _100ms.DataType = typeof(int);
            _100ms.Name = "_100ms";
            _100ms.FieldName = "_100ms";
            _100ms.HeaderText = "100ms";
            gridView.MasterTemplate.Columns.Add(_100ms);
            gridView.Columns[10].BestFit();
            
        }
        #endregion

        private void SetCheckValue()
        {
            foreach (GridViewDataRowInfo row in this.gridView.Rows)
            {
                row.Cells[12].Value = 0;
                row.Cells[13].Value = 0;
                row.Cells[14].Value = 0;
            }
        }

        #region 绑定数据
        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="data">数据源</param>
        public void BindRadGridView(List<AnalysisSignal> dataList)
        {
            gridView.BeginUpdate();
            gridView.DataSource = null;

            for (int i = 0; i < dataList.Count; i++)
            {
                DataRow dr = dataSource.NewRow();
                dr[GridViewData.GridViewColumns.ORDER] = i + 1;
                //显示字段：名称+描述+单位+数据类型+数据长度+是否摩托罗拉+开始地址+截取长度+数据地址+系数+偏移量
                dr[GridViewData.GridViewColumns.NAME] = dataList[i].Name;
                dr[GridViewData.GridViewColumns.DESCRIBLE] = dataList[i].Describle;
                dr[GridViewData.GridViewColumns.UNIT] = dataList[i].Unit;
                dr[GridViewData.GridViewColumns.TYPE] = dataList[i].SaveDataType;
                dr[GridViewData.GridViewColumns.DATA_LEN] = dataList[i].SaveDataLen;
                dr[GridViewData.GridViewColumns.BYTE_ORDER] = dataList[i].IsMotorola;
                dr[GridViewData.GridViewColumns.START_INDEX] = dataList[i].StartIndex;
                dr[GridViewData.GridViewColumns.BITDATA_LEN] = dataList[i].DataBitLen;
                dr[GridViewData.GridViewColumns.DATA_ADDRESS] = dataList[i].DataAddress;
                dr[GridViewData.GridViewColumns.FACTOR] = dataList[i].Factor;
                dr[GridViewData.GridViewColumns.OFF_SET] = dataList[i].OffSet;

                dataSource.Rows.Add(dr);
            }
            gridView.DataSource = dataSource;
            RefreshRadViewColumn();
            gridView.EndUpdate();
        }
        #endregion

        #region 查询gridview
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="filter">查询条件</param>
        public void SearchRadGridView(XcpData data, string filter)
        {
            //var measure = data.MeasureData;
            //var methold = data.MetholdData;
            //DataTable dtTemp = dataSource.Clone();
            //IEnumerable<MeasureMent> measureRes;
            //if (!string.IsNullOrEmpty(filter))
            //{
            //    measureRes = from v in measure
            //                 where (v.Name).Contains(filter)
            //                 || v.Name.StartsWith(filter)
            //                 || v.Name.EndsWith(filter)
            //                 || v.ReferenceMethod.Contains(filter)
            //                 || v.ReferenceMethod.StartsWith(filter)
            //                 || v.ReferenceMethod.EndsWith(filter)
            //                 || v.EcuAddress.ToString().StartsWith(filter)
            //                 || v.EcuAddress.ToString().EndsWith(filter)
            //                 select v;
            //}
            //else
            //{
            //    measureRes = from v in measure
            //              select v;
            //}
            //int i = 0;
            //gridView.BeginUpdate();
            //gridView.DataSource = null;
            //dtTemp = dataSource.Copy();
            //dataSource.Clear();
            //foreach (var item in measureRes)
            //{
            //    //显示字段：名称+描述+单位+数据类型+数据长度+是否摩托罗拉+开始地址+截取长度+数据地址+系数+偏移量
            //    DataRow dr = dataSource.NewRow();
            //    dr[GridViewData.GridViewColumns.ORDER] = i + 1;
            //    dr[GridViewData.GridViewColumns.NAME] = item.Name;
            //    dr[GridViewData.GridViewColumns.DESCRIBLE] = item.Describle;
            //    dr[GridViewData.GridViewColumns.UNIT] = item.uni;
            //    dr[GridViewData.GridViewColumns.TYPE] = item.Type;
            //    dr[GridViewData.GridViewColumns.REFERENCE_METHOLD] = item.ReferenceMethod;
            //    dr[GridViewData.GridViewColumns.DATA_ADDRESS] = item.EcuAddress;

            //    ///查询函数值
            //    ///
            //    var vm = methold.Find(tm => tm.name == item.ReferenceMethod);
                
            //    dr[GridViewData.GridViewColumns.FUN_TYPE] = vm.funType;
            //    dr[GridViewData.GridViewColumns.UNIT] = vm.unit;
            //    dr[GridViewData.GridViewColumns.COEFFS] = vm.coeffsValue;

            //    dataSource.Rows.Add(dr);
            //    i++;
            //}
            //gridView.DataSource = dataSource;
            //RefreshRadViewColumn();
            //gridView.EndUpdate();
        }
        #endregion  
    }
}
