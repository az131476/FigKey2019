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
        private RadGridView gridViewCan1;
        private RadGridView gridViewCan2;
        //private DataTable dataSource;
        //GridViewCheckBoxColumn segMentCan1;
        GridViewCheckBoxColumn _10msCan1;
        GridViewCheckBoxColumn _100msCan1;
        GridViewCheckBoxColumn _dbcSelectCan1;
        GridViewCheckBoxColumn _10msCan2;
        GridViewCheckBoxColumn _100msCan2;
        GridViewCheckBoxColumn _dbcSelectCan2;

        public GridViewControl(RadGridView view1,RadGridView view2)
        {
            this.gridViewCan1 = view1;
            this.gridViewCan2 = view2;
        }

        public void InitGridView()
        {
            SetRadGridView();
        }

        #region 设置视图属性
        /// <summary>
        /// 设置gridview属性
        /// </summary>
        private void SetRadGridView()
        {
            gridViewCan1.EnableGrouping = false;
            gridViewCan1.AllowDrop = true;
            gridViewCan1.AllowRowReorder = true;
            ///显示每行前面的标记
            gridViewCan1.AddNewRowPosition = SystemRowPosition.Bottom;
            gridViewCan1.ShowRowHeaderColumn = true;
            gridViewCan1.AutoSizeRows = true;
            gridViewCan1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            gridViewCan1.AllowAutoSizeColumns = true;

            //gridView.AutoScrollMinSize = new System.Drawing.Size(8,20);
            gridViewCan1.ReadOnly = false;
            //gridView.ColumnChooserSortOrder = RadSortOrder.Ascending;


            //can2
            gridViewCan2.EnableGrouping = false;
            gridViewCan2.AllowDrop = true;
            gridViewCan2.AllowRowReorder = true;
            ///显示每行前面的标记
            gridViewCan2.AddNewRowPosition = SystemRowPosition.Bottom;
            gridViewCan2.ShowRowHeaderColumn = true;
            gridViewCan2.AutoSizeRows = true;
            gridViewCan2.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            gridViewCan2.AllowAutoSizeColumns = true;

            //gridView.AutoScrollMinSize = new System.Drawing.Size(8,20);
            gridViewCan2.ReadOnly = false;
            //gridView.ColumnChooserSortOrder = RadSortOrder.Ascending;
            //设置虚模式
            //radGridView1.VirtualMode = true;
            //radGridView1.ColumnCount = dataSource.Columns.Count;
            //radGridView1.RowCount = dataSource.Rows.Count;


        }
        #endregion

        #region 添加列
        /// <summary>
        /// 添加table 列
        /// </summary>
        public DataTable GetDataSource()
        {
            //名称+描述+单位+数据类型+数据长度+是否摩托罗拉+开始地址+截取长度+数据地址+系数+偏移量
            DataTable dataSource = new DataTable();
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
            return dataSource;
        }
        #endregion

        #region 设置列
        public void SetRadViewColumnCheckCAN1(AgreementType agreementType)
        {
            if (agreementType == AgreementType.CCP || agreementType == AgreementType.XCP)
            {
                if (_10msCan1 == null)
                {
                    _10msCan1 = new GridViewCheckBoxColumn();
                    _10msCan1.DataType = typeof(int);
                    _10msCan1.Name = "_10ms";
                    _10msCan1.FieldName = "_10ms";
                    _10msCan1.HeaderText = "10ms";
                    gridViewCan1.MasterTemplate.Columns.Add(_10msCan1);
                    //gridView.Columns[13].BestFit();
                }
                if (_100msCan1 == null)
                {
                    _100msCan1 = new GridViewCheckBoxColumn();
                    _100msCan1.DataType = typeof(int);
                    _100msCan1.Name = "_100ms";
                    _100msCan1.FieldName = "_100ms";
                    _100msCan1.HeaderText = "100ms";
                    gridViewCan1.MasterTemplate.Columns.Add(_100msCan1);
                    //gridView.Columns[14].BestFit();
                }
                foreach (GridViewDataRowInfo row in this.gridViewCan1.Rows)
                {
                    row.Cells[12].Value = 0;
                    row.Cells[13].Value = 0;
                }
            }
            else if (agreementType == AgreementType.DBC)
            {
                if (_dbcSelectCan1 == null)
                {
                    _dbcSelectCan1 = new GridViewCheckBoxColumn();
                    _dbcSelectCan1.DataType = typeof(int);
                    _dbcSelectCan1.Name = "_dbcSelectCan1";
                    _dbcSelectCan1.FieldName = "_dbcSelectCan1";
                    _dbcSelectCan1.HeaderText = "选择";
                    gridViewCan1.MasterTemplate.Columns.Add(_dbcSelectCan1);
                }
                foreach (GridViewDataRowInfo row in this.gridViewCan2.Rows)
                {
                    row.Cells[12].Value = 0;
                }
            }
        }

        public void SetRadViewColumnCheckCAN2(AgreementType agreementType)
        {
            if (agreementType == AgreementType.CCP || agreementType == AgreementType.XCP)
            {
                if (_10msCan2 == null)
                {
                    _10msCan2 = new GridViewCheckBoxColumn();
                    _10msCan2.DataType = typeof(int);
                    _10msCan2.Name = "_10ms";
                    _10msCan2.FieldName = "_10ms";
                    _10msCan2.HeaderText = "10ms";
                    gridViewCan2.MasterTemplate.Columns.Add(_10msCan2);
                    //gridView.Columns[13].BestFit();
                }
                if (_100msCan2 == null)
                {
                    _100msCan2 = new GridViewCheckBoxColumn();
                    _100msCan2.DataType = typeof(int);
                    _100msCan2.Name = "_100ms";
                    _100msCan2.FieldName = "_100ms";
                    _100msCan2.HeaderText = "100ms";
                    gridViewCan2.MasterTemplate.Columns.Add(_100msCan2);
                    //gridView.Columns[14].BestFit();
                }
                foreach (GridViewDataRowInfo row in this.gridViewCan2.Rows)
                {
                    row.Cells[12].Value = 0;
                    row.Cells[13].Value = 0;
                }
            }
            else if (agreementType == AgreementType.DBC)
            {
                if (_dbcSelectCan2 == null)
                {
                    _dbcSelectCan2 = new GridViewCheckBoxColumn();
                    _dbcSelectCan2.DataType = typeof(int);
                    _dbcSelectCan2.Name = "_dbcSelectCan2";
                    _dbcSelectCan2.FieldName = "_dbcSelectCan2";
                    _dbcSelectCan2.HeaderText = "选择";
                    gridViewCan2.MasterTemplate.Columns.Add(_dbcSelectCan2);
                }
                foreach (GridViewDataRowInfo row in this.gridViewCan2.Rows)
                {
                    row.Cells[12].Value = 0;
                }
            }
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
                gridViewCan1.Columns[i].ReadOnly = true;
            }
            for (int i = 0; i < 11; i++)
            {
                gridViewCan1.Columns[i].BestFit();
            }
            //can2
            for (int i = 0; i < 8; i++)
            {
                gridViewCan2.Columns[i].ReadOnly = true;
            }
            for (int i = 0; i < 11; i++)
            {
                gridViewCan2.Columns[i].BestFit();
            }
        }
        #endregion

        #region 绑定数据
        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="data">数据源</param>
        public DataTable BindRadGridView(List<AnalysisSignal> dataList)
        {
            DataTable dataSource = GetDataSource();
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
            return dataSource;
        }
        #endregion

        #region 查询gridview
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="filter">查询条件</param>
        public void SearchRadGridView(RadGridView gridData, string filter)
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
