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
using 毕业设计_农产品追溯_.DsUserTableAdapters;
using 毕业设计_农产品追溯_.DsProductTableAdapters;

namespace 毕业设计_农产品追溯_
{
    public partial class Homepage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            T_ProductsTableAdapter adapter = new T_ProductsTableAdapter();
            毕业设计_农产品追溯_.DsProduct.T_ProductsDataTable products = adapter.GetDataById(productid.Text);
            if (products.Count <= 0)
            {
                Response.Redirect("Error.aspx");
            }
            else
            {
                Response.Redirect("View.aspx?Id=" + productid.Text);
            }
        }
    }
}
