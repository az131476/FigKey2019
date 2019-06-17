<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="毕业设计_农产品追溯_.Homepage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>浙林农产品追溯网欢迎您&nbsp&nbsp&nbsp&nbsp&nbsp      </title>
     <script type="text/javascript">
        function roll() {
            var title = document.title;
            var firstch = title.charAt(0);
            var laststr = title.substring(1, title.length);
            document.title = laststr + firstch;
        }
        setInterval("roll()", 300);
    </script>
    <style type="text/css">
        .style2
        {
            height: 110px;
            margin-top: 66px;
            text-align:center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="float:right"> <a href="Login.aspx">管理员登陆</a></div>
   <div class="style2" >
       <img src="images/标题.png" />
       </div>
    <div style="text-align:center;margin-top:90px" >
    &nbsp;<fieldset style="width: 422px;height:88px;text-align:center" >
        <legend>请输入追溯编码</legend>
        <br />
        <asp:TextBox ID="productid" runat="server" Width="235px"></asp:TextBox>
    </fieldset><br />
&nbsp;<asp:Button ID="Button1" runat="server" Text="提交追溯" onclick="Button1_Click" 
            Width="120px" />
    </div>
    </form>
</body>
</html>
