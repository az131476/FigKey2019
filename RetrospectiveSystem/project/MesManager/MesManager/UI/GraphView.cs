using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.Charting;
using ZedGraph;

namespace MesManager.UI
{
    public partial class GraphView : RadForm
    {
        public GraphView()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void GraphView_Load(object sender, EventArgs e)
        {
            this.LoadData();
        }
        private void LoadData()
        {
            
        }
        private void Draw_Load(object sender, EventArgs e)
        {
            DrawPie();
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();
            GraphPane myPane = zedGraphControl1.GraphPane;
            // 画图面版标题  
            //myPane.Title.Text = "收入统计";
            //// 画图面版X标题  
            myPane.XAxis.Title.Text = "区域";
            // myPane.XAxis.Type = AxisType.Text;
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = 20;
            //myPane.YAxis.Scale.MinorStep = 10;
            //myPane.YAxis.Scale.MajorStep = 10;
            myPane.Chart.Border.IsVisible = false;
            myPane.YAxis.MajorTic.IsOpposite = false;
            //初始化数据  
            PointPairList list = new PointPairList();
            PointPairList list2 = new PointPairList();
            PointPairList list3 = new PointPairList();
            for (int i = 0; i < 100; i++)////这里的数量要和lable的一致，比如横坐标显示了5个lable，这里就要给5个  
            {
                list.Add(i, i + 10);
                list2.Add(i, i + 20);
            }
            // 画图面版Y标题  
            myPane.YAxis.Title.Text = "销售情况";
            //柱的画笔  
            BarItem myCurve = myPane.AddBar("收入1", list, Color.Blue);
            // BarItem myCurve = myPane.AddBar("收入1", null, Color.Blue);
            //BarItem myCurve1 = myPane.AddBar("收入2", list2, Color.Green);
            for (int i = 0; i < myCurve.Points.Count; i++)
            {
                //PointPair pt = myCurve.Points[i];
                //TextObj text = new TextObj(pt.Y.ToString("f2"), float.Parse(pt.X + ""), float.Parse(pt.Y + offset + ""), CoordType.AxisXYScale, AlignH.Right, AlignV.Center);
                //text.ZOrder = ZOrder.A_InFront;

                //// 隐藏标注的边框和填充
                //text.FontSpec.Border.IsVisible = false;
                //text.FontSpec.Fill.IsVisible = false;
                //// 选择标注字体90°
                ////text.FontSpec.Angle = 90;

                //myPane.GraphObjList.Add(text);

                //pt = myCurve1.Points[i];
                //text = new TextObj(pt.Y.ToString("f2"), float.Parse(pt.X + ""), float.Parse(pt.Y + offset + ""), CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
                //text.FontSpec.Border.IsVisible = false;
                //text.FontSpec.Fill.IsVisible = false;
                //myPane.GraphObjList.Add(text);
            }

            //myPane.XAxis.MajorTic.IsBetweenLabels = true;  
            //XAxis标注  
            // string[] labels = { "产品1", "产品2", "产品3", "产品4", "产品5", "产品5", "产品5", "产品1", "产品2", "产品3", "产品4", "产品5", "产品5", "产品5" };
            //  string[] labels = { "产品1" };
            //  myPane.XAxis.Scale.TextLabels = labels;
            //  //myPane.XAxis.Scale.Min = 0;
            //  myPane.XAxis.Scale.Max = 10;
            ////  myPane.XAxis.Scale.MajorStep = 50;
            // /// myPane.XAxis.Scale.MaxAuto = true;
            //  myPane.XAxis.Type = AxisType.Text;
            //图区以外的颜色  fo
            // myPane.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);  
            //背景颜色  
            //myPane.Chart.Fill = new Fill(Color.Red, Color.LightGoldenrodYellow, 45.0f);  
            //myPane.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);
            zedGraphControl1.AxisChange();
            zedGraphControl1.Refresh();


            DrawZ();

            //DataTable dt = new DataTable("cart");
            //DataColumn dc1 = new DataColumn("areaid", Type.GetType("System.String"));
            //DataColumn dc2 = new DataColumn("house", Type.GetType("System.String"));
            //DataColumn dc3 = new DataColumn("seq", Type.GetType("System.String"));
            //DataColumn dc4 = new DataColumn("remark", Type.GetType("System.String"));

            //dt.Columns.Add(dc1);
            //dt.Columns.Add(dc2);
            //dt.Columns.Add(dc3);
            //dt.Columns.Add(dc4);


            //DataRow dr = dt.NewRow();
            //dr["areaid"] = "北京";
            //dr["house"] = "北京仓库";
            //dr["seq"] = "2";
            //dr["remark"] = "货到付款";
            //dt.Rows.Add(dr);


            //DataRow dr1 = dt.NewRow();
            //dr1["areaid"] = "北京";
            //dr1["house"] = "上海仓库";
            //dr1["seq"] = "1";
            //dr1["remark"] = "货到付款";
            //dt.Rows.Add(dr1);

            //DataRow dr2 = dt.NewRow();
            //dr2["areaid"] = "上海";
            //dr2["house"] = "上海仓库";
            //dr2["seq"] = "1";
            //dr2["remark"] = "货到付款";
            //dt.Rows.Add(dr2);

            //DataRow dr3 = dt.NewRow();
            //dr3["areaid"] = "上海";
            //dr3["house"] = "北京仓库";
            //dr3["seq"] = "1";
            //dr3["remark"] = "货到付款";
            //dt.Rows.Add(dr3);


            //var query = from cus in dt.AsEnumerable()
            //            group cus by new { t1 = cus.Field<string>("areaid"), t2 = cus.Field<string>("seq") } into m
            //            select new
            //            {
            //                areaid = m.Key.t1,
            //                seq = m.Key.t2,
            //                house = m.First().Field<string>("house"),
            //                rowcount = m.Count()
            //            };


            //Console.WriteLine("区域 " + "  库房" + "   数量");
            //foreach (var item in query.ToList())
            //{
            //    if (item.rowcount > 1)
            //    {
            //        MessageBox.Show(item.areaid + "---" + item.house);
            //    }
            //    Console.WriteLine(item.areaid + "---" + item.house + "---" + item.rowcount);
            //    Console.WriteLine("\r\n");
            //}

        }
        const double offset = 0.5;
        private void DrawZ()
        {

            GraphPane myPane2 = zedGraphControl2.GraphPane;

            //设置图标标题和x、y轴标题
            myPane2.Title.Text = "机票波动情况";
            myPane2.XAxis.Title.Text = "波动日期";
            myPane2.YAxis.Title.Text = "机票价格";

            //更改标题的字
            FontSpec myFont = new FontSpec("Arial", 20, Color.Red, false, false, false);
            // 造一些数据，PointPairList里有数据对x，y的数组
            Random y = new Random();
            PointPairList list1 = new PointPairList();
            for (int i = 1; i < 37; i++)
            {
                double x = i;
                //double y1 = 1.5 + Math.Sin((double)i * 0.2);
                double y1 = y.NextDouble() * 1000;
                list1.Add(x, y1); //添加一组数据
            }

            // 用list1生产一条曲线，标注是“东航”
            LineItem myCurve = myPane2.AddCurve("东航", list1, Color.Red, SymbolType.Default);
            //LineItem    myCurve=myPane2.AddStick("东航", list1, Color.Red);
            myCurve.Line.Width = 2;
            //填充图表颜色
            myPane2.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);

            //以上生成的图标X轴为数字，下面将转换为日期的文本
            string[] labels = new string[36];
            for (int i = 0; i < 36; i++)
            {
                // labels[i] = System.DateTime.Now.AddDays(i).ToShortDateString();
                PointPair pt = myCurve.Points[i];
                TextObj text = new TextObj(pt.Y.ToString("f2"), float.Parse(pt.X + ""), float.Parse(pt.Y + 2 + ""), CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
                text.ZOrder = ZOrder.A_InFront;

                // 隐藏标注的边框和填充
                text.FontSpec.Border.IsVisible = false;
                text.FontSpec.Fill.IsVisible = false;
                // 选择标注字体90°
                text.FontSpec.Angle = 90;

                myPane2.GraphObjList.Add(text);
            }
            //     myPane2.XAxis.Scale.TextLabels = labels; //X轴文本取值
            myPane2.XAxis.Type = AxisType.Text;   //X轴类型

            //画到zedGraphControl1控件中，此句必加
            zedGraphControl2.AxisChange();

            //重绘控件
            Refresh();
        }
        private void DrawPie()
        {
            GraphPane myPane3 = this.zedGraphControl3.GraphPane;
            myPane3.Title.Text = "收支比例";
            myPane3.Title.FontSpec.IsItalic = true;
            myPane3.Title.FontSpec.Size = 24f;
            myPane3.Title.FontSpec.Family = "Times New Roman";

            myPane3.Fill = new Fill(Color.White, Color.Goldenrod, 45.0f);
            myPane3.Chart.Fill.Type = FillType.None;

            myPane3.Legend.Position = LegendPos.Float;
            myPane3.Legend.Location = new Location(0.95f, 0.15f, CoordType.PaneFraction,
                           AlignH.Right, AlignV.Top);
            myPane3.Legend.FontSpec.Size = 10f;
            myPane3.Legend.IsHStack = false;

            PieItem segment1 = myPane3.AddPieSlice(20, Color.Navy, Color.White, 45f, 0, "North");
            PieItem segment3 = myPane3.AddPieSlice(30, Color.Purple, Color.White, 45f, .0, "East");
            PieItem segment4 = myPane3.AddPieSlice(10.21, Color.LimeGreen, Color.White, 45f, 0, "West");
            PieItem segment2 = myPane3.AddPieSlice(40, Color.SandyBrown, Color.White, 45f, 0.2, "South");
            PieItem segment6 = myPane3.AddPieSlice(250, Color.Red, Color.White, 45f, 0, "Europe");
            PieItem segment7 = myPane3.AddPieSlice(1500, Color.Blue, Color.White, 45f, 0.2, "Pac Rim");
            PieItem segment8 = myPane3.AddPieSlice(400, Color.Green, Color.White, 45f, 0, "South America");
            PieItem segment9 = myPane3.AddPieSlice(50, Color.Yellow, Color.White, 45f, 0.2, "Africa");

            segment2.LabelDetail.FontSpec.FontColor = Color.Red;

            CurveList curves = myPane3.CurveList;
            double total = 0;
            for (int x = 0; x < curves.Count; x++)
                total += ((PieItem)curves[x]).Value;

            TextObj text = new TextObj("Total 2004 Sales/n" + "$" + total.ToString() + "M",
                           0.18F, 0.40F, CoordType.PaneFraction);
            text.Location.AlignH = AlignH.Center;
            text.Location.AlignV = AlignV.Bottom;
            text.FontSpec.Border.IsVisible = false;
            text.FontSpec.Fill = new Fill(Color.White, Color.FromArgb(255, 100, 100), 45F);
            text.FontSpec.StringAlignment = StringAlignment.Center;
            myPane3.GraphObjList.Add(text);

            TextObj text2 = new TextObj(text);
            text2.FontSpec.Fill = new Fill(Color.Black);
            text2.Location.X += 0.008f;
            text2.Location.Y += 0.01f;
            myPane3.GraphObjList.Add(text2);

            zedGraphControl3.AxisChange();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int intExcelNum = excelhelper.ExistsExcel();

            string strexcelname = @"C:\Users\SXS\Desktop\1111.xls";
            ArrayList arlist = new ArrayList();//{ "=SERIES(" + "\"" + "测试一" + "\"" + ",{2,12,32},{12,34,32},1)" };
            ArrayList xAxis = new ArrayList();

            arlist.Add("=SERIES(" + "\"" + "测试2" + "\"" + ",{14:20:20.222,14:30:30.333},{15,25},1)");
            xAxis.Add(Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
            //arlist.Add("=SERIES(" + "\"" + "测试1" + "\"" + ",{20,20},{80,0},2)");
            //xAxis.Add(Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
            //arlist.Add("=SERIES(" + "\"" + "" + "\"" + ",{30,30},{80,0},3)");
            //xAxis.Add(Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
            //arlist.Add("=SERIES(" + "\"" + "" + "\"" + ",{40,40},{80,0},4)");
            //xAxis.Add(Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
            //arlist.Add("=SERIES(" + "\"" + "" + "\"" + ",{50,50},{80,0},5)");
            //xAxis.Add(Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
            //arlist.Add("=SERIES(" + "\"" + "" + "\"" + ",{60,60},{80,0},6)");
            //xAxis.Add(Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
            //arlist.Add("=SERIES(" + "\"" + "" + "\"" + ",{70,70},{80,0},7)");
            //xAxis.Add(Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);

            // arlist.Add("=SERIES(" + "\"" + "测试3" + "\"" + ",30,32,1)");
            ArrayList arlistnull = new ArrayList();
            ArrayList color = new ArrayList() { 41, 45, 45, 45, 45, 45, 45, };

            string strExcel = "D";
            string[,] title = new string[1, 4];
            int nowNum = excelhelper.getTotalRowCount();
            int startRow = intExcelNum - nowNum;
            title[0, 0] = "哈哈1";
            title[0, 1] = "哈哈2";
            title[0, 2] = "哈哈3";
            title[0, 3] = "哈哈4";

            excelhelper.createExcel("123");
            excelhelper.setCurSheet("456", 1);

            excelhelper.setCellsNumberFormat("A1", strExcel + intExcelNum.ToString(), "@");
            excelhelper.setCellsValue("A1", strExcel + "1", title);
            excelhelper.setCellsColor("A1", strExcel + "1", "gray");
            excelhelper.setCellsBorder("A1" + "1", strExcel + "1");
            excelhelper.setCellsBorder("A1" + "1", strExcel + intExcelNum.ToString());
            excelhelper.setRowHeight(1, 16);
            //excelhelper.setColWidth(1, 32);
            int tempIndex = intExcelNum - 1;
            excelhelper.setCellsNumberFormat("A2", strExcel + (tempIndex + 1), "@");
            //列宽度自适应
            excelhelper.setSheetColWidthToAutoFit();

            string[,] data = new string[tempIndex, 4];
            data[0, 0] = "10";
            data[0, 1] = "2";
            data[0, 2] = "3";
            data[0, 3] = "14";
            excelhelper.addChart(1, 1, "图标", "X轴", "Y轴", 16, 0, 160, 320, arlist, xAxis, color, 0, 100, Microsoft.Office.Interop.Excel.XlChartType.xlXYScatterLinesNoMarkers);
            //excelhelper.addChart(1, 1, "图标", "X轴", "Y轴", 330,0,160, 320, arlist, arlistnull, color, 0, 100, Microsoft.Office.Interop.Excel.XlChartType.xlLine);

            excelhelper.setCellsValue("A25", strExcel + (tempIndex + 1), data);
            excelhelper.setCellsNumberFormat("A13", strExcel + (tempIndex + 1), "@");
            //列宽度自适应
            excelhelper.setSheetColWidthToAutoFit();

            excelhelper.SetActiveWorkSheet(1);
            excelhelper.saveExcel(strexcelname);

            excelhelper.CloseExcelApplication();


        }
    }
}
