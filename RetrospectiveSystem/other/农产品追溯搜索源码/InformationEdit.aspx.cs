using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using 毕业设计_农产品追溯_.DsProductTableAdapters;

namespace 毕业设计_农产品追溯_
{
    public partial class InformationEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //foreach (ListViewDataItem item in LvData.Items)
            //{
            //    CheckBox check=(CheckBox)item.FindControl("CheckBox1");
            //    if (check.Checked==true)
            //    {
            //        LvData.Items.Remove(item);
            //    }
            //}
            for (int i = 0; i < LvData.Items.Count; i++)
            {
               CheckBox check =(CheckBox) LvData.Items[i].FindControl("CheckBox1");
                if (check.Checked == true)
                {
                   // this.LvData.Items.Remove(this.LvData.Items[i]);
                    T_ProductsTableAdapter adapter = new T_ProductsTableAdapter();
                    Label lab =(Label) this.LvData.Items[i].FindControl("IdLabel");
                    adapter.DeleteById(lab.Text);

                   
                } 
                
            }LvData.DataBind();
            
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < LvData.Items.Count; i++)
            {
                CheckBox check = (CheckBox)LvData.Items[i].FindControl("CheckBox1");
                check.Checked = true;
            }
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < LvData.Items.Count; i++)
            {
                CheckBox check = (CheckBox)LvData.Items[i].FindControl("CheckBox1");
                if (check.Checked == true)
                {
                    check.Checked = false;
                }
                else
                {
                    check.Checked = true;
                }
            }
        }
    }
}
