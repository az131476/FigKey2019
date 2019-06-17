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

namespace 毕业设计_农产品追溯_
{
    public partial class AlterPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            T_UsersTableAdapter adapter = new T_UsersTableAdapter();
            毕业设计_农产品追溯_.DsUser.T_UsersDataTable users = adapter.GetDataByUserName("lzx");
            毕业设计_农产品追溯_.DsUser.T_UsersRow user = users[0];
            if (user.Password == TextBox1.Text)
            {
                adapter.UpdateUser(TextBox2.Text, user.Id);
                Response.Redirect("Success.aspx");
            }
            else
            {
                wrongMsg.Visible = true;
            }
        }
    }
}
