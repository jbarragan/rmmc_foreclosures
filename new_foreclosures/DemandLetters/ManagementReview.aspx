<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManagementReview.aspx.cs" Inherits="Foreclosure_Demnad_Letters_ManagementReview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Foreclosures: Demand Letter Management Review.</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosures</h1>
        <p>Loans in Demand Letter Management Review.</p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.sub_main_nav %>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h2>Loans in Demand Letter Management Review. (<%= completed_demand_letter_mode.Count%>)</h2>
        <div class="row">
          <div class="span2"></div>
          <div class="span10">
            Definitions:<br />
              <span class="text-error">5 days or less before Demand Letter Expiration Date</span><br />
              <span class="text-warning">10 days or less before Demand Letter Expiration Date</span>
          </div>
        </div>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Demand Letter Due Date</th>
                <th>Officer(s)</th>
                <th>Review Checklist</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in completed_demand_letter_mode)
               {   
            %>
            <tr class="<%= demand_letter.in_pre_demand_letter_tr_class() %>">
                <td><%= demand_letter.loan_id.ToString()%></td>
                <td><%= demand_letter.loan.loan_type%></td>
                <td><%= demand_letter.loan.loan_name%></td>
                <td><%= demand_letter.loan.prin_bal.ToString("C") %></td>
                <td><%= demand_letter.loan.int_rate.ToString("0.000") %></td>
                <td><%= demand_letter.demand_letter_due_date.ToShortDateString()%></td>
                <td><%= demand_letter.foreclosure_officers %></td>
                <td>
                    <a href="../ForeclosureReviewChecklist/Default.aspx?loan_id=<%=demand_letter.loan_id.ToString() %>">Checklist</a>
                </td>
             </tr>
            <%
                }
            %>
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
