<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Gl1806Details.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - Management - GL1806 details</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>GL1806 Loan - <%= this.loan_id.ToString() %> transactions</h1>
        <h2><%= (this.history == null) ?  "Current" : "History: " + history.history_text %></h2>
        <p></p>
    </div>
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Management", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.management_list_nav_select("GL1806", version)%>
   
    <div class="row">
        <div class="span12">
        <h2>GL1806 transactions for loan # <%= this.loan_id.ToString() %> (Count: <%= this.details.Count%>)</h2>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Name</th>
                <th>Transaction Date</th>
                <th>Description</th>
                <th>Invoice</th>
                <th>Check Number</th>
                <th>Amount</th>
            </tr>
            <% 
               int month = 0;
               foreach (com.sp.rmmc.foreclosures.models.Gl1806Details d in details)
               {
                   if (d.transaction_date.date.Month != month)
                   {
                       month = d.transaction_date.date.Month;
            %>
            <tr class="info"><td colspan="16"><%= d.transaction_date.date.ToString("MMMM yyyy")%></td></tr>
            <%
                   }
            %>
            <tr>
                <td><%= d.loan_id.ToString()%></td>
                <td><%= d.name %></td>
                <td><%= d.transaction_date.ToString() %></td>
                <td><%= d.description %></td>
                <td><%= d.invoice %></td>
                <td><%= d.check_number %></td>
                <td><%= d.amount.ToString("C") %></td>
             </tr>
            <%
               }
            %>
        </table>
        </div>
    </div>

    </div>
    </div>
    </form>
</body>
</html>
