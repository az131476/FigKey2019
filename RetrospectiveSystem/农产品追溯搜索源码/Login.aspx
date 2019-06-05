<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="毕业设计_农产品追溯_.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>浙林农产品追溯网欢迎您&nbsp&nbsp&nbsp&nbsp&nbsp</title>

    <script src="js/jquery-1.4.2.js" type="text/javascript"></script>

    <script src="js/jquery.cookie.js" type="text/javascript"></script>
    <script type="text/javascript">

 
        function roll() {
            var title = document.title;
            var firstch = title.charAt(0);
            var laststr = title.substring(1, title.length);
            document.title = laststr + firstch;
        }
        setInterval("roll()", 300);

        $(function() {
             $("#username").mouseout(function() { 
             if ($.cookie("Password") != '' ) {

                if ($.cookie("Username") == $("#username").val())
                    $("#password").val($.cookie("Password"));
            }
            });
            $("#remember").click(function() {
                $.cookie("Username", $("#username").val());
                $.cookie("Password", $("#password").val());
            });
          
        });
    </script>

 
</head>
<body  style="background-image:url('图片/果蔬.jpg');background-position:center;background-repeat:no-repeat">
  
    
    <form id="form1" runat="server">
    
    <div style=" margin-top:300px;text-align:center;" >
    <table>
    <tr><td>
    <label for="username">用户名：</label></td><td><asp:TextBox ID="username"  runat="server" Width="150px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="username" ErrorMessage="*"></asp:RequiredFieldValidator>
    </td></tr>
    <tr><td>
    <label for="password">密码：</label></td><td> <asp:TextBox ID="password" runat="server" 
                            TextMode="Password" Width="150px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ControlToValidate="password" ErrorMessage="*"></asp:RequiredFieldValidator>
    </td></tr>
    <tr><td>
   
     <%--<asp:CheckBox ID="remember" runat="server" OnCheckedChanged="rememberPassword()" />   --%>
    <label for="remember">记住密码</label> <input type="checkbox" id="remember" onchange="rememberPassword() " />
     
     
     </td>
     <td> 
      
  <asp:Button runat="server" Text="登录" Width="75px" ID="login" onclick="login_Click"  /> 
        </td></tr>
        <tr><td colspan="2">  
        <asp:Label ID="wrongMsg" runat="server" ForeColor="#FF3300" Text="用户名或密码错误" Visible="False"></asp:Label></td></tr>
    </table>

    </div>     
    </form>
 
</body>
</html>
