<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoanDisbursements.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Utilities: Disbursements for Loan <%= loan_id.ToString() + " - " + loan.loan_name %></title>
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
        <p>Disbursements for Loan <%= loan_id.ToString() + " - " + loan.loan_name%></p>
    </div>
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Utilities", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.utilities_nav_select("Disbursements")%>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h2>Over Paid Disbursements</h2>
        <table class="table table-striped">
            <tr>
                <th>Description</th>
                <th>Variance</th>
                <th>Ordered Date</th>
                <th>Completed Date</th>
                <th>Paid Date</th>
                <th>Actual Amount</th>
                <th>Claimable Amount</th>
                <th>Code Max Allowance</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.LoanPreservationDisbursement disbursement in with_more_amount_disbursements)
               {   
            %>
            <tr>
                <td><%= disbursement.fcl_misc_description.ToString()%></td>
                <td><span class="text-error"><%= (disbursement.preservation_code.allowance - disbursement.fcl_misc_actual_amt).ToString("C")%></span></td>
                <td><%= disbursement.fcl_misc_ordered_date.ToShortDateString() %></td>
                <td><%= disbursement.fcl_misc_completed_date.ToShortDateString() %></td>
                <td><%= disbursement.fcl_misc_paid_date.ToShortDateString() %></td>
                <td><%= disbursement.fcl_misc_actual_amt.ToString("C")%></td>
                <td><%= disbursement.fcl_misc_claimable_amt.ToString("C")%></td>
                <td><span class="text-error"><%= disbursement.preservation_code.allowance.ToString("C")%></span></td>
             </tr>
            <%
                }
            %>
        </table>
        <h2>Under Paid Disbursements</h2>
        <table class="table table-striped">
            <tr>
                <th>Description</th>
                <th>Variance</th>
                <th>Ordered Date</th>
                <th>Completed Date</th>
                <th>Paid Date</th>
                <th>Actual Amount</th>
                <th>Claimable Amount</th>
                <th>Code Max Allowance</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.LoanPreservationDisbursement disbursement in with_less_amount_disbursements)
               {   
            %>
            <tr>
                <td><%= disbursement.fcl_misc_description.ToString()%></td>
                <td><span class="text-warning"><%= (disbursement.preservation_code.allowance - disbursement.fcl_misc_actual_amt).ToString("C")%></span></td>
                <td><%= disbursement.fcl_misc_ordered_date.ToShortDateString() %></td>
                <td><%= disbursement.fcl_misc_completed_date.ToShortDateString() %></td>
                <td><%= disbursement.fcl_misc_paid_date.ToShortDateString() %></td>
                <td><%= disbursement.fcl_misc_actual_amt.ToString("C")%></td>
                <td><%= disbursement.fcl_misc_claimable_amt.ToString("C")%></td>
                <td><span class="text-warning"><%= disbursement.preservation_code.allowance.ToString("C")%></span></td>
             </tr>
            <%
                }
            %>
        </table>
        <h2>Paid Correct Disbursements</h2>
        <table class="table table-striped">
            <tr>
                <th>Description</th>
                <th>Variance</th>
                <th>Ordered Date</th>
                <th>Completed Date</th>
                <th>Paid Date</th>
                <th>Actual Amount</th>
                <th>Claimable Amount</th>
                <th>Code Max Allowance</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.LoanPreservationDisbursement disbursement in with_correct_amount_disbursements)
               {   
            %>
            <tr>
                <td><%= disbursement.fcl_misc_description.ToString()%></td>
                <td><span class="text-success"><%= (disbursement.preservation_code.allowance - disbursement.fcl_misc_actual_amt).ToString("C")%></span></td>
                <td><%= disbursement.fcl_misc_ordered_date.ToShortDateString() %></td>
                <td><%= disbursement.fcl_misc_completed_date.ToShortDateString() %></td>
                <td><%= disbursement.fcl_misc_paid_date.ToShortDateString() %></td>
                <td><%= disbursement.fcl_misc_actual_amt.ToString("C")%></td>
                <td><%= disbursement.fcl_misc_claimable_amt.ToString("C")%></td>
                <td><span class="text-success"><%= disbursement.preservation_code.allowance.ToString("C")%></span></td>
             </tr>
            <%
                }
            %>
        </table>
        <h2>No Code Matching Disbursements</h2>
        <table class="table table-striped">
            <tr>
                <th>Description</th>
                <th>Ordered Date</th>
                <th>Completed Date</th>
                <th>Paid Date</th>
                <th>Actual Amount</th>
                <th>Claimable Amount</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.LoanPreservationDisbursement disbursement in no_code_disbursements)
               {   
            %>
            <tr>
                <td><a href="LoanDisbursement.aspx?loan_id=<%= disbursement.loan_id %>&fcl_misc_disb_id=<%= disbursement.fcl_misc_disb_id %>"><%= disbursement.fcl_misc_description.ToString()%></a></td>
                <td><%= disbursement.fcl_misc_ordered_date.ToShortDateString() %></td>
                <td><%= disbursement.fcl_misc_completed_date.ToShortDateString() %></td>
                <td><%= disbursement.fcl_misc_paid_date.ToShortDateString() %></td>
                <td><%= disbursement.fcl_misc_actual_amt.ToString("C")%></td>
                <td><%= disbursement.fcl_misc_claimable_amt.ToString("C")%></td>
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
