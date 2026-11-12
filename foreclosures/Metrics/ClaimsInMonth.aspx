<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClaimsInMonth.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Metrics: Claims.</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Claims</h1>
        <p>Claims For <%= date.ToString("MMMM yyyy") %>.</p>
    </div>
    
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.sub_main_nav_select("Metrics", new ADUser(HttpContext.Current.User.Identity.Name.ToString())) %>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.metrics_list_nav_select("Claims")%>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h2>Claims For <%= date.ToString("MMMM yyyy") %></h2><br />
        <table class="table table-striped">
            <tr>
                <th>Loan #</th>
                <th>Name</th>
                <th>Type</th>
                <th>Pay Off Date</th>
                <th>Pay Off</th>
                <th>Not Pay Off</th>
                <th>Foreclosure Date</th>
                <th>Claim Type</th>
                <th>Claim Received</th>
                <th>Claim Amount</th>
                <th>Loss Received</th>
                <th>Loss Amount</th>
                <th>Debenture Rate</th>
                <th>Debenture Interest Paid</th>
                <th>Interest Rate</th>
                <th>Interest of Payoff</th>
                <th>Difference</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Claim claim in claims) { %>
             <tr>
                 <td><%= claim.loan_id %></td>
                 <td><%= claim.loan.loan_name %></td>
                 <td><%= claim.loan.loan_type %></td>
                 <td><%= claim.last_pay_off_date.ToShortDateString() %></td>
                 <td><%= claim.loan_paid_off.ToString("C") %></td>
                 <td><%= claim.loan_current_balance.ToString("C") %></td>
                 <td><%= claim.foreclosure_date.ToShortDateString() %></td>
                 <td width="100px"><%= claim.claim_type %></td>
                 <td><%= claim.received_date.ToShortDateString()%></td>
                 <td><%= claim.claim_amount.ToString("C") %></td>
                 <td><%= claim.loss_received_date.ToShortDateString()%></td>
                 <td><%= claim.loss_amount.ToString("C") %></td>
                 <td><%= String.Format("{0:P2}.", claim.debenture_rate/100M) %></td>
                 <td><%= claim.debenture_interest_paid.ToString("C") %></td>
                 <td><%= String.Format("{0:P2}.", claim.loan.int_rate/100M) %></td>
                 <td><%= claim.interest_of_payoff.ToString("C") %></td>
                 <td><%= (claim.interest_of_payoff - claim.debenture_interest_paid).ToString("C") %></td>
             </tr>  
            <% } %>
            <tr><td colspan="13"></td></tr>
             <tr class="info">
                 <td></td>
                 <td>TOTALS</td>
                 <td></td>
                 <td></td>
                 <td><%= claims_total.loan_paid_off.ToString("C")%></td>
                 <td><%= claims_total.loan_current_balance.ToString("C")%></td>
                 <td></td>
                 <td></td>
                 <td><%= claims_total.received_date.ToShortDateString()%></td>
                 <td><%= claims_total.claim_amount.ToString("C")%></td>
                 <td></td>
                 <td><%= claims_total.loss_amount.ToString("C")%></td>
                 <td><%= String.Format("{0:P2}.", claims_total.debenture_rate / 100M) %></td>
                 <td><%= claims_total.debenture_interest_paid.ToString("C")%></td>
                 <td><%= String.Format("{0:P2}.", claims_total.loan.int_rate / 100M)%></td>
                 <td><%= claims_total.interest_of_payoff.ToString("C")%></td>
                 <td><%= (claims_total.interest_of_payoff - claims_total.debenture_interest_paid).ToString("C")%></td>
             </tr>
        </table>
        </div>
    </div>
    </div>
    </form>
</body>
</html>
