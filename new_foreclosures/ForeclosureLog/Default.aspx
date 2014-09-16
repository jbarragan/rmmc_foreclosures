<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures: Foreclosure Log.</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosures</h1>
        <p>Foreclosure Log.</p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.sub_main_nav %>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h2>Foreclosure Log.</h2><br />
        <table class="table table-striped">
            <tr><th>Loan ID</th><th>Date</th><th>Section</th></tr>
            <% foreach (com.sp.rmmc.foreclosures.models.ForeclosureLog foreclosure_log in foreclosure_logs) { %>
             <tr><td><%= foreclosure_log.loan_id %></td><td><%= foreclosure_log.date.ToShortDateString() %></td><td><%= foreclosure_log.location %></td></tr>
            <% } %>
        </table>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
