<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformationEdit.aspx.cs" Inherits="毕业设计_农产品追溯_.InformationEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>浙林农产品追溯网欢迎您&nbsp&nbsp&nbsp&nbsp&nbsp</title>

    <script src="js/jquery-1.4.2.js" type="text/javascript"></script>
  <script type="text/javascript">
        function roll() {
            var title = document.title;
            var firstch = title.charAt(0);
            var  laststr= title.substring(1, title.length);
            document.title = laststr + firstch;
        }
        setInterval("roll()", 300);
        $(function() {
            $("#search").click(function() {
                var str = $("#id").val();
                window.location.href = 'DataEditUI.aspx?action=edit&&Id=' + str;

            });

        });
        function SelectAll() {

            var inputs = document.getElementsByTagName("CheckBox");

            for (var i = 0; i < inputs.length; i++) {
                var input = inputs[i];
                if (input.type == "checkbox") {
                    input.checked = "checked";
                }
            }
        }
        $(function() {
            $("#selectall").click(function() {
                var check = $(".test");
                for (var i = 0; i < check.length; i++) {
                    if (check.type == "checkbox") {
                        check.checked = true;

                    }
                }
            });
        });
         
      
       
       
    </script>
    <style type="text/css"> 
.body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}
.STYLE1 {
	font-size: 12px;
	color: #000000;
}
.STYLE5 {font-size: 12}
.STYLE7 {font-size: 12px; color: #FFFFFF; }
.STYLE7 a{font-size: 12px; color: #FFFFFF; }
a img {
	border:none;
}

        .style1
        {
            width: 128%;
        }

    </style>
</head>
<body>










    <form id="form1" runat="server">




<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td  background="images/main_03.gif"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td width="378" height="57" background="images/main_01.gif">&nbsp;</td>
        <td>&nbsp;</td>
        <td width="281" valign="bottom"><table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="33" height="27"><img src="images/main_05.gif" width="33" height="27" /></td>
            <td width="248" background="images/main_06.gif"><table width="225" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td height="17"><div align="right"><a href="AlterPassword.aspx" target="rightFrame"><img src="images/pass.gif" width="69" height="17" /></a></div></td>
                <td><div align="right"><a href="Homepage.aspx" target="_parent"><img src="images/quit.gif" alt=" " width="69" height="17" /></a></div></td>
              </tr>
            </table></td>
          </tr>
        </table></td>
      </tr>
    </table></td>
  </tr>
  <tr>
    <td height="40" background="images/main_10.gif"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><table border="0" cellspacing="0" cellpadding="0" class="style1">
          <tr>
            <td width="21"><img src="images/main_13.gif" width="19" height="14" /></td>
            <td width="35" class="STYLE7"><div align="center"><a href="Homepage.aspx" target="rightFrame">首页</a></div></td>
            <td width="21" class="STYLE7"><img src="images/main_15.gif" width="19" height="14" /></td>
            <td width="35" class="STYLE7"><div align="center"><a href="javascript:history.go(-1);">后退</a></div></td>
            <td width="21" class="STYLE7"><img src="images/main_17.gif" width="19" height="14" /></td>
            <td width="35" class="STYLE7"><div align="center"><a href="javascript:history.go(1);">前进</a></div></td>
            <td width="21" class="STYLE7"><img src="images/main_19.gif" width="19" height="14" /></td>
            <td width="35" class="STYLE7"><div align="center"><a href="javascript:window.parent.location.reload();">刷新</a></div></td>
            <td>&nbsp;</td>
          </tr>
        </table></td>
        
      </tr>
    </table></td>
  </tr>
  <tr>
    <td height="30" background="images/main_31.gif"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td width="8" height="30"><img src="images/main_28.gif" width="8" height="30" /></td>
        <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td height="20" valign="bottom"><span class="STYLE1"><span lang="zh-cn">&nbsp;
                </span>当前登录用户：<span lang="zh-cn">lzx</span> &nbsp;用户角色：管理员<span 
                    lang="zh-cn">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</span></span></td>
          </tr>
        </table></td>
        <td width="17"><img src="images/main_32.gif" width="17" height="30" /></td>
      </tr>
    </table></td>
  </tr>
</table>










    <input type="text" id="id"/><input type="button" id="search" value="搜索ID" onclick=" window.location.href = 'DataEditUI.aspx?action=edit&&Id='"/>&nbsp;
    <a href="DataEditUI.aspx?action=addnew" style="float:right">新增产品信息</a>
    <div >
    
        <span lang="zh-cn">&nbsp;</span><asp:ObjectDataSource ID="OdsData" runat="server" DeleteMethod="Delete" 
            InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" 
            SelectMethod="GetData" 
            TypeName="毕业设计_农产品追溯_.DsProductTableAdapters.T_ProductsTableAdapter" 
            UpdateMethod="Update">
            <DeleteParameters>
                <asp:Parameter Name="Original_Id" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="Name" Type="String" />
                <asp:Parameter Name="ProducingArea" Type="String" />
                <asp:Parameter Name="PluckingTime" Type="DateTime" />
                <asp:Parameter Name="Transportation" Type="String" />
                <asp:Parameter Name="Process" Type="String" />
                <asp:Parameter Name="Sell" Type="String" />
                <asp:Parameter Name="Image" Type="String" />
                <asp:Parameter Name="FiledProcess" Type="String" />
                <asp:Parameter Name="Original_Id" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="Id" Type="String" />
                <asp:Parameter Name="Name" Type="String" />
                <asp:Parameter Name="ProducingArea" Type="String" />
                <asp:Parameter Name="PluckingTime" Type="DateTime" />
                <asp:Parameter Name="Transportation" Type="String" />
                <asp:Parameter Name="Process" Type="String" />
                <asp:Parameter Name="Sell" Type="String" />
                <asp:Parameter Name="Image" Type="String" />
                <asp:Parameter Name="FiledProcess" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    
    <span lang="zh-cn">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
</span>
  
        <br />
    
    </div>
    <div>
    
        &nbsp;
     </div>
  
    <span lang="zh-cn">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:UpdatePanel 
    ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div style="margin-left:67px;text-align:center">
            <asp:ListView ID="LvData" runat="server" DataKeyNames="Id" 
                DataSourceID="OdsData">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:CheckBox ID="CheckBox1" runat="server" class="test" />
                        </td>
                        <td>
                            <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                        </td>
                        <td>
                            <asp:Label ID="NameLabel" runat="server" Text='<%# Eval("Name") %>' />
                        </td>
                        <td>
                            <asp:Label ID="ProducingAreaLabel" runat="server" 
                                Text='<%# Eval("ProducingArea") %>' />
                        </td>
                        <td>
                            <asp:Label ID="PluckingTimeLabel" runat="server" 
                                Text='<%# Eval("PluckingTime") %>' />
                        </td>
                        <td>
                            <asp:Label ID="TransportationLabel" runat="server" 
                                Text='<%# Eval("Transportation") %>' />
                        </td>
                        <td>
                            <asp:Label ID="ProcessLabel" runat="server" Text='<%# Eval("Process") %>' />
                        </td>
                        <td>
                            <asp:Label ID="SellLabel" runat="server" Text='<%# Eval("Sell") %>' />
                        </td>
                        <td>
                            <%-- <asp:Button ID="Button1" runat="server" CommandName="Delete" Text="删除" OnClientClick="return confirm('真的要删除吗？')"/>--%>
                            <a href='DataEditUI.aspx?action=view&amp;Id=<%# Eval("Id") %>'>查看</a>|
                            <a href='DataEditUI.aspx?action=edit&amp;Id=<%# Eval("Id") %>'>编辑</a>
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <table runat="server" 
                        style="background-color: #FFFFFF;border-collapse: collapse;border-color: #999999;border-style:none;border-width:1px;">
                        <tr>
                            <td>
                                没有数据。</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table runat="server">
                        <tr runat="server">
                            <td runat="server">
                                <table ID="itemPlaceholderContainer" runat="server" border="1" 
                                    style="background-color: #FFFFFF;border-collapse: collapse;border-color: #999999;border-style:none;border-width:1px;font-family: Verdana, Arial, Helvetica, sans-serif;">
                                    <tr runat="server" style="background-color:Gray;color: #333333;">
                                        <th ID="Th4" runat="server">
                                        </th>
                                        <th runat="server">
                                            追溯编号</th>
                                        <th runat="server">
                                            名称</th>
                                        <th runat="server">
                                            产地</th>
                                        <th runat="server">
                                            采摘时间</th>
                                        <th ID="Th1" runat="server">
                                            运输记录</th>
                                        <th ID="Th2" runat="server">
                                            加工过程</th>
                                        <th ID="Th3" runat="server">
                                            销售方式</th>
                                        <th runat="server">
                                        </th>
                                    </tr>
                                    <tr ID="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr ID="Tr1" runat="server">
                            <td ID="Td1" runat="server" 
                                style="text-align: center;background-color:Gray;font-family: Verdana, Arial, Helvetica, sans-serif;color: #333333;">
                                <asp:DataPager ID="DataPager1" runat="server">
                                    <Fields>
                                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" 
                                            ShowNextPageButton="False" ShowPreviousPageButton="False" />
                                        <asp:NumericPagerField />
                                        <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" 
                                            ShowNextPageButton="False" ShowPreviousPageButton="False" />
                                    </Fields>
                                </asp:DataPager>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
        </div>
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" 
    Text="删除选中数据"  OnClientClick="return confirm('真的要删除吗？')"
        Width="88px" />
        <asp:Button ID="Button3" runat="server" onclick="Button3_Click" Text="全选" />
        <asp:Button ID="Button4" runat="server" onclick="Button4_Click" Text="反选" />
    </ContentTemplate>
</asp:UpdatePanel>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>
  
    </form>
</body>
</html>
