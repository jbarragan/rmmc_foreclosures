<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreservationCodesList.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Utilities: Preservation Codes</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
    <script type="text/jscript" src="../../assets/js/jquery.js" ></script>
</head>
<body>
    <form id="form1" runat="server" method="post" enctype="multipart/form-data">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Utilities</h1>
        <p>Preservation Codes</p>
    </div>
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Utilities", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.utilities_nav_select("Preservation Codes List")%>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
            <p class="text-error"><asp:Label ID="lbl_error" runat="server" Text="" ></asp:Label></p>
            <p class="text-success"><asp:Label ID="lbl_success" runat="server" Text="" ></asp:Label></p>
        <h4>
         View Loan Preservation Disbursements:
         <asp:TextBox runat="server" ID="txtLoanId" MaxLength="10" Width="100px"></asp:TextBox>
         <asp:Button ID="btnLoanDisbursements" runat="server" Text="View" CssClass="btn" onclick="btnLoanDisbursements_Click" />
       </h4>
        <h4><a href="NewPreservationCode.aspx">Create New Preservation Code</a></h4>
        <br />
        <h4>Upload File</h4>
        <input type="file" id="File1" name="File1" runat="server"  style="width:600px;" />
        <br />
        <br />
        <input type="submit" id="Submit1" value="Upload" runat="server" />
        <br />
        <br />
        <br />
        <h4>Preservation Code List (<%= this.all_preservation_codes.Count %>)</h4>
        <asp:LinkButton ID="lbExport" runat="server" Text="Export" onclick="lbExport_Click"></asp:LinkButton>
                <table class="table table-striped">
                    <tr>
                        <th>Code</th>
                        <th>Type</th>
                        <th>Description</th>
                        <th>Service</th>
                        <th>Allowance</th>
                        <th>Effective Date</th>
                        <th>Enabled</th>
                    </tr>
                    <% foreach (com.sp.rmmc.foreclosures.models.PreservationCode preservation_code in this.all_preservation_codes)
                        {   
                    %>
                    <tr>
                        <td><a href="editPreservationCode.aspx?code=<%= preservation_code.code.ToString()%>"><%= preservation_code.code.ToString()%></a></td>
                        <td><%= preservation_code.type %></td>
                        <td><%= preservation_code.description %></td>
                        <td><%= preservation_code.general_service %></td>
                        <td><%= preservation_code.allowance.ToString("C") %></td>
                        <td><%= preservation_code.effective_date.ToString() %></td>
                        <td><%= preservation_code.enabled.ToString() %></td>
                        </tr>
                    <%
                        }
                    %>
                </table>
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
