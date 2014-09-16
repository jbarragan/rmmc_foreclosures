<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditPreservationCode.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Utilities: Edit Preservation Code</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
    <script type="text/jscript" src="../../assets/js/jquery.js" ></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Utilities</h1>
        <p>Edit Preservation Code: <%=preservation_code.code %></p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.sub_main_nav %>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.utilities_nav%>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h2>Edit Preservation Code</h2>
        <table class="table table-striped" border="0">
            <tr>
                <td>Code</td>
                <td><asp:TextBox runat="server" ID="txtCode" MaxLength="50" Width="100px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Type</td>
                <td><asp:DropDownList runat="server" ID="cmbType">
                      <asp:ListItem Text="FHA" Value="FHA"></asp:ListItem>
                      <asp:ListItem Text="VA" Value="VA"></asp:ListItem>
                      <asp:ListItem Text="CONV" Value="CONV"></asp:ListItem>
                   </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Description</td>
                <td><asp:TextBox runat="server" ID="txtDescription" MaxLength="255" Width="600px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Service</td>
                <td><asp:TextBox runat="server" ID="txtGeneralService" MaxLength="255" Width="600px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Allowance</td>
                <td><asp:TextBox runat="server" ID="txtAllowance" MaxLength="10" Width="100px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Enabled</td>
                <td><asp:CheckBox runat="server" ID="chkEnabled" Checked="true" /></td>
             </tr>
             <tr>
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server"
                                Text="Save"
                                CssClass="btn btn-success" onclick="btnSave_Click" />
                    <asp:Button ID="btnDelete" runat="server"
                                Text="Delete"
                                OnClientClick="return confirm('Are you sure you want to remove this code?');"
                                CssClass="btn btn-danger" onclick="btnDelete_Click" />
                <p class="text-error"><asp:Label ID="lblError" runat="server" Text="" ></asp:Label></p>
                <p class="text-success"><asp:Label ID="lblSuccess" runat="server" Text="" ></asp:Label></p>
                </td>
             </tr>
        </table>
        </div>
     </div>
    <br />
    <br />
    <div class="row">
        <div class="span12">
            <div><a href="../Default.aspx">Back to Home</a></div>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
