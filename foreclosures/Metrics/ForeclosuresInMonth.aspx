<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForeclosuresInMonth.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Metrics: Sale Foreclosure Completed by Month.</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Metrics</h1>
        <p>Foreclosures Sale Completed For <%= date.ToString("MMMM yyyy") %>.</p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.sub_main_nav_select("Metrics", new ADUser(HttpContext.Current.User.Identity.Name.ToString())) %>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.metrics_list_nav_select("Foreclosures By Sale Date Completed")%>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h2>Foreclosures For <%= date.ToString("MMMM yyyy") %></h2><br />
        <div id="completed_by_sale_date_time_table">
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Principal Paid</th>
                <th>Interest Rate</th>
                <th>Next Payment Due Date</th>
                <th>Papers To Attorney Date</th>
                <th>Posting Date</th>
                <th>Sale Date Completed</th>
                <th>Officer(s)</th>
                <th>Reason Code</th>
                <th>Occupancy Code</th>
                <th>Principal Paid</th>
            </tr>
            <% 
                com.sp.rmmc.foreclosures.models.Foreclosure previous = null;
                if( foreclosures.Count > 0) previous = foreclosures[0];
            %>  
                <tr><th colspan="13">Sale Completed on <%= previous.sale_date_completed.date.ToString("MMMM yyyy") %></th></tr> 
            <%
                System.Collections.Generic.List<com.sp.rmmc.foreclosures.models.Foreclosure> total_in_month = new System.Collections.Generic.List<com.sp.rmmc.foreclosures.models.Foreclosure>();
                decimal previous_total = 0M;
                int previous_count = 0;
                foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in foreclosures)
               {
            %>
            <% if (foreclosure.sale_date_completed.date.Month != previous.sale_date_completed.date.Month ||
                   foreclosure.sale_date_completed.date.Year != previous.sale_date_completed.date.Year)
               {
                   previous = foreclosure;
                   previous_total = com.sp.rmmc.foreclosures.models.Foreclosure.list_total( total_in_month );
                   previous_count = total_in_month.Count;
                   total_in_month = new System.Collections.Generic.List<com.sp.rmmc.foreclosures.models.Foreclosure>();
            %>
            <tr class="info"><td colspan="2">Count: <%= previous_count %></td><td colspan="9"></td><td colspan="2">Total: <%= previous_total.ToString("C") %></td></tr>
            <tr><th colspan="13">Sale Completed on <%= previous.sale_date_completed.date.ToString("MMMM yyyy")%></th></tr> 
            <% }
                   total_in_month.Add(foreclosure);
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                <td><%= foreclosure.loan.loan_type%></td>
                <td><%= foreclosure.loan.loan_name%></td>
                <td><%= foreclosure.non_zero_balance().ToString("C") %></td>
                <td><%= foreclosure.loan.int_rate.ToString("0.000")%></td>
                <td><%= foreclosure.demand_letter_due_date.ToShortDateString()%></td>
                <td><%= foreclosure.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= foreclosure.posting_date.ToShortDateString()%></td>
                <td><span class="text-error"><%= foreclosure.sale_date_completed.ToShortDateString()%></span></td>
                <td><%= foreclosure.foreclosure_officers%></td>
                <td><%= foreclosure.main_default_reason_code %></td>
                <td><%= foreclosure.occupancy_code %></td>
                <td><%= foreclosure.non_zero_balance().ToString("C")%></td>
             </tr>
            <%
                }
                previous_total = com.sp.rmmc.foreclosures.models.Foreclosure.list_total(total_in_month);
                previous_count = total_in_month.Count;
                if (previous_count > 0)
                {
            %>
            <tr class="info"><td colspan="2">Count: <%= previous_count%></td><td colspan="9"></td><td colspan="2">Total: <%= previous_total.ToString("C")%></td></tr>
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
