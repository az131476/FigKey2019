using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.InteropServices;
using System.IO;
namespace testFlow
{
    public partial class Form1 : Form
    {
        private PointF mouseDownPointF;
        public Form1()
        {
            InitializeComponent();
        }




        private void tsb_Line_Click(object sender, EventArgs e)
        {
            //设置画线时图形的焦点区域模式（中心点模式、图形全区域模式）
            //this.addFlow1.LinkCreationMode = Lassalle.Flow.LinkCreationMode.MiddleHandle;
            //没搞明白
            this.addFlow1.LinkHandleSize = Lassalle.Flow.HandleSize.Large;
            this.addFlow1.DisplayHandles = true;



            this.addFlow1.CanDrawLink = true;
            this.addFlow1.CanDrawNode = false;
        }

        private void tsb_ORG_Click(object sender, EventArgs e)
        {
            this.addFlow1.CanDrawLink = false;
            this.addFlow1.CanDrawNode = true;



        }

        private void tsb_Save_Click(object sender, EventArgs e)
        {
            System.Xml.XmlTextWriter a = new System.Xml.XmlTextWriter("c:\\a.xml", null);

            Lassalle.XMLFlow.Serial.FlowToXML(a, this.addFlow1);
            a.Close();
        }

        private void tsb_Load_Click(object sender, EventArgs e)
        {
            System.Xml.XmlTextReader a = new System.Xml.XmlTextReader("c:\\a.xml");
            Lassalle.XMLFlow.Serial.XMLToFlow(a, this.addFlow1);
            a.Close();
        }

        private void addFlow1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.addFlow1.CanDrawNode)
            {
                this.mouseDownPointF.X = e.X;
                this.mouseDownPointF.Y = e.Y;
            }
        }

        private void addFlow1_MouseUp(object sender, MouseEventArgs e)
        {


        }

        private void addFlow1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals(""))
            {//删除 
                this.addFlow1.DeleteSel();

            }
        }

        private void tsb_Delete_Click(object sender, EventArgs e)
        {
            //不知道啥意思
            this.addFlow1.SendSelectionChangeEvent = false;
            this.addFlow1.DeleteSel();
            //不知道啥意思
            this.addFlow1.SendSelectionChangeEvent = true;
        }

        private void tsb_Line_Style1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tsb_ORG_Style1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //圆形
            //矩形
            //菱形
            //图片
            //p_判断条件
            //p_活动
            if (tsb_ORG_Style1.Text.Equals("p_判断条件"))
            {
                this.addFlow1.DefNodeProp = new Lassalle.Flow.DefNode(PC.ProductLine.Flow.NodeLib.ONode.Get_Losange_Style());
            }
            else if (tsb_ORG_Style1.Text.Equals("p_活动"))
            {
                this.addFlow1.DefNodeProp = new Lassalle.Flow.DefNode(PC.ProductLine.Flow.NodeLib.ONode.Get_Rectangle_Style());
            }
        }

        private void addFlow1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // this.addFlow1.SelectedItem.
        }

        private void addFlow1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Lassalle.Flow.Item item1 = this.addFlow1.GetItemAt(new Point(e.X, e.Y));
                if (item1 != null)
                    item1.Selected = true;

            }

        }

        private void tsb_UNDo_Click(object sender, EventArgs e)
        {
            this.addFlow1.Undo();

        }

        private void tsb_ReDo_Click(object sender, EventArgs e)
        {
            this.addFlow1.Redo();
        }
        #region Windows 拷贝、粘贴、剪切
        /// <summary>
        /// (从某例子中拷贝过来的没弄明白呢,反正是配合剪切用的)
        /// </summary>
        private void SelectionChangeHandle()
        {
            Lassalle.Flow.Item item = addFlow1.SelectedItem;
            if (item != null)
            {
                if (item is Lassalle.Flow.Node)
                {
                    Lassalle.Flow.Node node = (Lassalle.Flow.Node)item;
                    propertyGrid1.SelectedObject = node;
                    //Label1.Text = "Selected Node";
                }
                else if (item is Lassalle.Flow.Link)
                {
                    Lassalle.Flow.Link link = (Lassalle.Flow.Link)item;
                    propertyGrid1.SelectedObject = link;
                    //Label1.Text = "Selected Link";
                }
            }
            else
            {
                propertyGrid1.SelectedObject = addFlow1;
                // Label1.Text = "AddFlow control";
            }
        }
        /// <summary>
        /// 看代码应该是删除(从某例子中拷贝过来的没弄明白呢,反正是配合剪切用的)
        /// </summary>
        private void MenuItemEditDelete()
        {
            addFlow1.SendSelectionChangeEvent = false;
            addFlow1.DeleteSel();
            addFlow1.SendSelectionChangeEvent = true;
            SelectionChangeHandle();
        }
        private void MenuItemEditCut()
        {
            MenuItemEditCopy();
            MenuItemEditDelete();
        }
        /// <summary>
        /// 拷贝粘贴-----拷贝(从某例子中拷贝过来的没弄明白呢)
        /// </summary>
        private void MenuItemEditCopy()
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            this.addFlow1.WriteXml(writer, true, false);
            DataObject data = new DataObject();
            data.SetData("AddFlow.XMLFormat", stream);
            Clipboard.SetDataObject(data, true);
        }
        /// <summary>
        /// 拷贝粘贴-----粘贴(从某例子中拷贝过来的没弄明白呢)
        /// </summary>
        private void MenuItemEditPaste()
        {
            IDataObject data = Clipboard.GetDataObject();
            if (data.GetDataPresent("AddFlow.XMLFormat"))
            {
                // We first unselect the selected items
                this.addFlow1.SelectedItems.Clear();

                // Get the data from the clipboard
                MemoryStream stream = (MemoryStream)data.GetData("AddFlow.XMLFormat");

                // We paste the portion of the diagram in our AddFlow control.
                // We group all the load actions in only one action so that we could undo
                // the paste action in one time.
                this.addFlow1.BeginAction(1003);

                XmlTextReader reader = new XmlTextReader(stream);
                reader.WhitespaceHandling = WhitespaceHandling.None;
                this.addFlow1.ReadXml(reader, true, false, false);
                reader.Close();

                // We move a little each pasted node and link so that they do not recover
                // the original items.
                float dx = this.addFlow1.Grid.Size.Width;
                float dy = this.addFlow1.Grid.Size.Height;
                foreach (Lassalle.Flow.Item item in addFlow1.SelectedItems)
                {
                    if (item is Lassalle.Flow.Node)
                    {
                        Lassalle.Flow.Node node = (Lassalle.Flow.Node)item;
                        node.Location = new PointF(node.Location.X + dx, node.Location.Y + dy);
                    }
                    else
                    {
                        PointF pt;
                        Lassalle.Flow.Link link = (Lassalle.Flow.Link)item;

                        if (link.AdjustOrg)
                        {
                            pt = link.Points[0];
                            link.Points[0] = new PointF(pt.X + dx, pt.Y + dy);
                        }
                        for (int k = 1; k < link.Points.Count - 1; k++)
                        {
                            pt = link.Points[k];
                            link.Points[k] = new PointF(pt.X + dx, pt.Y + dy);
                        }
                        if (link.AdjustDst)
                        {
                            pt = link.Points[link.Points.Count - 1];
                            link.Points[link.Points.Count - 1] = new PointF(pt.X + dx, pt.Y + dy);
                        }
                    }
                }

                addFlow1.EndAction();
            }
        }
        #endregion
        private void tsb_Copy_Click(object sender, EventArgs e)
        {
            MenuItemEditCopy();
        }

        private void tsb_Post_Click(object sender, EventArgs e)
        {
            MenuItemEditPaste();
        }

        private void ttsb_Cut_Click(object sender, EventArgs e)
        {
            MenuItemEditCut();
        }
        private void tsb_MouseP_Click(object sender, EventArgs e)
        {
            this.addFlow1.CanDrawLink = false;
            this.addFlow1.CanDrawNode = false;
            // this.addFlow1.
        }

        private void tsb_Zomm150_Click(object sender, EventArgs e)
        {
            addFlow1.Zoom = new Lassalle.Flow.Zoom(1.5f, 1.5f);
        }

        private void tsb_Zomm100_Click(object sender, EventArgs e)
        {
            addFlow1.Zoom = new Lassalle.Flow.Zoom(1, 1);
        }

        private void tsb_Zomm70_Click(object sender, EventArgs e)
        {
            addFlow1.Zoom = new Lassalle.Flow.Zoom(0.7f, 0.7f);
        }

        private void tsb_Property_Click(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = this.addFlow1.SelectedItem;
        }

        private void tsb_NewFile_Click(object sender, EventArgs e)
        {
            
        }

        private void tsb_ClearNodes_Click(object sender, EventArgs e)
        {
            this.addFlow1.Nodes.Clear();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
        #region 打印相关
        private void tsb_打印预览_Click(object sender, EventArgs e)
        {
            SizeF pagesize = SizeF.Empty;
            Lassalle.PrnFlow.PrnFlow prnflow = new Lassalle.PrnFlow.PrnFlow();
            pagesize = prnflow.GetPageSize(addFlow1);

            prnflow.Preview(addFlow1);
        }

        private void tsb_打印_Click(object sender, EventArgs e)
        {
            Lassalle.PrnFlow.PrnFlow prnflow = new Lassalle.PrnFlow.PrnFlow();
            prnflow.Print(addFlow1);
        }

        private void tsb_打印设置_Click(object sender, EventArgs e)
        {
            SizeF pagesize = SizeF.Empty;
            Lassalle.PrnFlow.PrnFlow prnflow = new Lassalle.PrnFlow.PrnFlow();
            prnflow.PageSetup();
            pagesize = prnflow.GetPageSize(addFlow1);
            addFlow1.Invalidate();
        }
        #endregion

        private void tsb_附加属性_Click(object sender, EventArgs e)
        {
            //怎么就不好使呢？（执行不出错，保存的xml中没有这个属性）
            this.addFlow1.SelectedItem.Properties["扩展属性"].Value = "我扩展了";
            
        }






    }
}
