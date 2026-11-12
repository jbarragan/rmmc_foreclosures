<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - Quarter Reports - Loss Mitigation In Process</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Quarter Reports</h1>
        <h2>Loss Mitigation In Process</h2>
        <p></p>
    </div>
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Quarter Reports", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.quarter_reports_list_nav_select("Loss Mitigation In Process", "current")%>
    <!-- Example row of columns -->    
    
    <div class="row">
        <div class="span12">
        <h2>Loss Mitigation In Process (<%= bfs.Count%>)</h2>
        <asp:LinkButton ID="LinkButton1" runat="server" Text="Export" onclick="lbExport_Click"></asp:LinkButton>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>UPB</th>
                <th>Mortgage Status</th>
                <th>Mortgage Status Date</th>
                <th>Trial Payment Stop Code</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.LMInProcess bf in bfs)
               {
            %>
            <tr>
                <td><%= bf.loan_id.ToString()%></td>
                <td><%= bf.loan.loan_type%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.loan.prin_bal.ToString("C")%></td>
                <td><%= bf.mortgage_status %></td>
                <td><%= bf.mortgage_status_date.ToShortDateString() %></td>
                <td><%= bf.stop_code_description %></td>
             </tr>
            <%
               }
            %>
        </table>
        </div>
    </div>
    <div class="row">
        <div class="span12">
            <%= this.debug_msg %>
        </div>
    </div>

    </div>
    </div>
    </form>
</body>
</html>
