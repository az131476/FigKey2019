<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="毕业设计_农产品追溯_.View" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>浙林农产品追溯网欢迎您&nbsp&nbsp&nbsp&nbsp&nbsp  </title>
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
        img{ float:right;
             
            
             }
    </style>
</head>
<body style="background-color:Silver">
    <form id="form1" runat="server">
    <div>
    
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataById" 
            TypeName="毕业设计_农产品追溯_.DsProductTableAdapters.T_ProductsTableAdapter">
            <SelectParameters>
                <asp:QueryStringParameter Name="Id" QueryStringField="Id" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    
    </div><div style="text-align:center">
    <asp:FormView ID="FormView1" runat="server" DataKeyNames="Id" 
        DataSourceID="ObjectDataSource1">
        <EditItemTemplate>
            Id:
            <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
            <br />
            Name:
            <asp:TextBox ID="NameTextBox" runat="server" Text='<%# Bind("Name") %>' />
            <br />
            ProducingArea:
            <asp:TextBox ID="ProducingAreaTextBox" runat="server" 
                Text='<%# Bind("ProducingArea") %>' />
            <br />
            PluckingTime:
            <asp:TextBox ID="PluckingTimeTextBox" runat="server" 
                Text='<%# Bind("PluckingTime") %>' />
            <br />
            Transportation:
            <asp:TextBox ID="TransportationTextBox" runat="server" 
                Text='<%# Bind("Transportation") %>' />
            <br />
            Process:
            <asp:TextBox ID="ProcessTextBox" runat="server" Text='<%# Bind("Process") %>' />
            <br />
            Sell:
            <asp:TextBox ID="SellTextBox" runat="server" Text='<%# Bind("Sell") %>' />
            <br />
            Image:
            <asp:TextBox ID="ImageTextBox" runat="server" Text='<%# Bind("Image") %>' />
            <br />
            FiledProcess:
            <asp:TextBox ID="FiledProcessTextBox" runat="server" 
                Text='<%# Bind("FiledProcess") %>' />
            <br />
            <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
                CommandName="Update" Text="更新" />
            &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" 
                CausesValidation="False" CommandName="Cancel" Text="取消" />
        </EditItemTemplate>
        <InsertItemTemplate>
            Id:
            <asp:TextBox ID="IdTextBox" runat="server" Text='<%# Bind("Id") %>' />
            <br />
            Name:
            <asp:TextBox ID="NameTextBox" runat="server" Text='<%# Bind("Name") %>' />
            <br />
            ProducingArea:
            <asp:TextBox ID="ProducingAreaTextBox" runat="server" 
                Text='<%# Bind("ProducingArea") %>' />
            <br />
            PluckingTime:
            <asp:TextBox ID="PluckingTimeTextBox" runat="server" 
                Text='<%# Bind("PluckingTime") %>' />
            <br />
            Transportation:
            <asp:TextBox ID="TransportationTextBox" runat="server" 
                Text='<%# Bind("Transportation") %>' />
            <br />
            Process:
            <asp:TextBox ID="ProcessTextBox" runat="server" Text='<%# Bind("Process") %>' />
            <br />
            Sell:
            <asp:TextBox ID="SellTextBox" runat="server" Text='<%# Bind("Sell") %>' />
            <br />
            Image:
            <asp:TextBox ID="ImageTextBox" runat="server" Text='<%# Bind("Image") %>' />
            <br />
            FiledProcess:
            <asp:TextBox ID="FiledProcessTextBox" runat="server" 
                Text='<%# Bind("FiledProcess") %>' />
            <br />
            <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" 
                CommandName="Insert" Text="插入" />
            &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" 
                CausesValidation="False" CommandName="Cancel" Text="取消" />
        </InsertItemTemplate>
        <ItemTemplate>
         
            <%--<asp:Label ID="ImageLabel" runat="server" Text= />--%>
            <asp:Image ID="Image2"  CssClass="img" runat="server" ImageUrl='<%# Bind("Image") %>' />
              <asp:TextBox ID="TextBox1" runat="server" Visible="false" Text='<%# Bind("Image") %>' />
        
           <br /><br /><br /><br /><br /><br />
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
        
        <div style="width:600px">
            田间作业:
            <asp:Label ID="FiledProcessLabel" runat="server" 
                Text='<%# Bind("FiledProcess") %>' />
              </div>
            <br />
         
        </ItemTemplate>
    </asp:FormView>
    </div>
    </form>
</body>
</html>
