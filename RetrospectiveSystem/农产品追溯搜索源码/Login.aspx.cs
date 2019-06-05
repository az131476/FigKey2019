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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

        }

        protected void login_Click(object sender, EventArgs e)
        {
            T_UsersTableAdapter adapter = new T_UsersTableAdapter();
            毕业设计_农产品追溯_.DsUser.T_UsersDataTable users = adapter.GetDataByUserName(username.Text);
            if (users.Count <= 0)
            {
                wrongMsg.Visible = true;
                wrongMsg.Text = "用户名不存在";
            }
            else
            {
                毕业设计_农产品追溯_.DsUser.T_UsersRow user = users[0];
                if (user.Password == password.Text)
                {
                    Response.Redirect("InformationEdit.aspx");
                }
                else
                {
                    wrongMsg.Visible = true;
                    wrongMsg.Text = "密码错误";
                }
            }
        }
    }
}
