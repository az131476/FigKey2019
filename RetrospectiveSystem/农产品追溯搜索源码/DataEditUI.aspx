<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataEditUI.aspx.cs" Inherits="毕业设计_农产品追溯_.DataEditUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>浙林农产品追溯网&nbsp&nbsp&nbsp&nbsp&nbsp</title>
    <link href="css/jquery-ui-1.8.15.custom.css" rel="stylesheet" type="text/css" />

    <script src="js/jquery-1.4.2.js" type="text/javascript"></script>

    <script src="js/jquery-ui-1.8.15.custom.min.js" type="text/javascript"></script>
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
    <td height="57" background="images/main_03.gif"><table width="100%" border="0" cellspacing="0" cellpadding="0">
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
                </span>当前登录用户：<span lang="zh-cn">lzxsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</span></span></td>
          </tr>
        </table></td>
        <td width="17"><img src="images/main_32.gif" width="17" height="30" /></td>
      </tr>
    </table></td>
  </tr>
</table>
    
    
    <div>
    
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            DeleteMethod="Delete" InsertMethod="Insert" 
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataById" 
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
            <SelectParameters>
                <asp:QueryStringParameter Name="Id" QueryStringField="Id" Type="String" />
            </SelectParameters>
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
    
    </div>
    <asp:FormView ID="FormView1" runat="server" DataKeyNames="Id" 
        DataSourceID="ObjectDataSource1" oniteminserted="FormView1_ItemInserted" 
        onitemupdated="FormView1_ItemUpdated" 
        onitemcreated="FormView1_ItemCreated"  
        oniteminserting="FormView1_ItemInserting" 
        onitemupdating="FormView1_ItemUpdating">
        <EditItemTemplate>
            追溯编码:
            <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
            <br />
            名称:
            <asp:TextBox ID="NameTextBox" runat="server" Text='<%# Bind("Name") %>' />
            <br />
            产地:
            <asp:TextBox ID="ProducingAreaTextBox" runat="server" 
                Text='<%# Bind("ProducingArea") %>' />
            <br />
            采摘时间:
            <asp:TextBox ID="PluckingTimeTextBox" runat="server" 
                Text='<%# Bind("PluckingTime") %>' />
                 <script type="text/javascript">
                     $('#<asp:Literal ID="litId" runat="server"></asp:Literal>').datepicker({

                 });
                 
                 </script>
            <br />
            运输记录:
            <asp:TextBox ID="TransportationTextBox" runat="server" 
                Text='<%# Bind("Transportation") %>'  />
     
            <br />
            加工过程:
            <asp:TextBox ID="ProcessTextBox" runat="server" Text='<%# Bind("Process") %>' />
            <br />
            销售方式:
            <asp:TextBox ID="SellTextBox" runat="server" Text='<%# Bind("Sell") %>' />
            <br />
            图片:<asp:FileUpload ID="FileUpload2" runat="server" />
            <asp:TextBox ID="ImageTextBox" runat="server" Visible="false" Text='<%# Bind("Image") %>' />
            <br />
            田间作业:
          <%--  <asp:TextBox ID="FiledProcessTextBox" runat="server" 
                Text='<%# Bind("FiledProcess") %>' />--%>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("FiledProcess") %>' TextMode="MultiLine"></asp:TextBox>
            <br />
            <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
                CommandName="Update" Text="更新" />
            &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" 
                CausesValidation="False" CommandName="Cancel" Text="取消" />
        </EditItemTemplate>
        <InsertItemTemplate>
            追溯编码:
            <asp:TextBox ID="IdTextBox" runat="server" Text='<%# Bind("Id") %>' />
            <br />
            名称:
            <asp:TextBox ID="NameTextBox" runat="server" Text='<%# Bind("Name") %>' />
            <br />
            产地:
            <asp:TextBox ID="ProducingAreaTextBox" runat="server" 
                Text='<%# Bind("ProducingArea") %>' />
            <br />
            采摘时间:
            <asp:TextBox ID="PluckingTimeTextBox" runat="server" 
                Text='<%# Bind("PluckingTime") %>' />
                <script type="text/javascript">
                    $('#<asp:Literal ID="litId" runat="server"></asp:Literal>').datepicker({
                    
                    });
                </script>
                
            <br />
            运输记录:
            <asp:TextBox ID="TransportationTextBox" runat="server" 
                Text='<%# Bind("Transportation") %>' />
            <br />
            加工过程:
            <asp:TextBox ID="ProcessTextBox" runat="server" Text='<%# Bind("Process") %>' />
            <br />
            销售方式:
            <asp:TextBox ID="SellTextBox" runat="server" Text='<%# Bind("Sell") %>' />
            <br />
            图片:
            <asp:FileUpload ID="FileUpload1" runat="server" />
        
           <asp:TextBox ID="ImageTextBox" runat="server" Visible="false" Text='<%# Bind("Image") %>' />
           
            <br />
            田间作业:
           <%-- <asp:TextBox ID="FiledProcessTextBox" runat="server" 
                Text='<%# Bind("FiledProcess") %>' />--%>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("FiledProcess") %>' TextMode="MultiLine"></asp:TextBox>
            <br />
            <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" 
                CommandName="Insert" Text="插入" />
            &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" 
                CausesValidation="False" CommandName="Cancel" Text="取消" />
        </InsertItemTemplate>
        <ItemTemplate>
            追溯编码:
            <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            <br />
            名称:
            <asp:Label ID="NameLabel" runat="server" Text='<%# Bind("Name") %>' />
            <br />
            产地:
            <asp:Label ID="ProducingAreaLabel" runat="server" 
                Text='<%# Bind("ProducingArea") %>' />
            <br />
            采摘时间:
            <asp:Label ID="PluckingTimeLabel" runat="server" 
                Text='<%# Bind("PluckingTime") %>' />
            <br />
            运输记录:
            <asp:Label ID="TransportationLabel" runat="server" 
                Text='<%# Bind("Transportation") %>' />
            <br />
            加工过程:
            <asp:Label ID="ProcessLabel" runat="server" Text='<%# Bind("Process") %>' />
            <br />
            销售方式:
            <asp:Label ID="SellLabel" runat="server" Text='<%# Bind("Sell") %>' />
            <br />
            图片:
            <%--<asp:Label ID="ImageLabel" runat="server" Text= />--%>
            <asp:Image ID="Image1" runat="server" BorderStyle="Solid" ImageUrl='<%# Bind("Image") %>' />
              <asp:TextBox ID="ImageTextBox" runat="server" Visible="false" Text='<%# Bind("Image") %>' />
            <br />
            
            田间作业:
            <asp:Label ID="FiledProcessLabel" runat="server" 
                Text='<%# Bind("FiledProcess") %>' />
              
            <br />
          
        </ItemTemplate>
    </asp:FormView>
    </form>
</body>
</html>
