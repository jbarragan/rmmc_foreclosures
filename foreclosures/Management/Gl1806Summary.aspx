<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Gl1806Summary.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - Management - GL1806 summary</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>GL1806 last loans with transactions</h1>
        <h2><%= (this.history == null) ?  "Current" : "History: " + history.history_text %></h2>
        <p></p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Management", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.management_list_nav_select("GL1806", version)%>
   
    <div class="row">
        <div class="span12">
        <h2>GL1806 last loans with transactions (<%= this.summaries.Count%>)</h2>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Name</th>
                <th>Latest Transaction Date</th>
                <th>Amount Balance</th>
                <th>Last description</th>
                <th>Last amount</th>
            </tr>
            <% 
               int month = 0;
               foreach (com.sp.rmmc.foreclosures.models.Gl1806Summary s in summaries)
               {
                   if (s.latest_date.date.Month != month)
                   {
                       month = s.latest_date.date.Month;
            %>
            <tr class="info"><td colspan="16"><%= s.latest_date.date.ToString("MMMM yyyy") %></td></tr>
            <%
                   }
            %>
            <tr>
                <td><a href="Gl1806Details.aspx?loan_id=<%=s.loan_id %>&version=<%=version %>"><%= s.loan_id.ToString()%></a></td>
                <td><%= s.name %></td>
                <td><%= s.latest_date.ToString() %></td>
                <td><%= s.amount_balance.ToString("C") %></td>
                <td><%= s.last_description %></td>
                <td><%= s.last_amount.ToString("C") %></td>
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
