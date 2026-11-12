<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoanChronology.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - Chronology</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Chronology</h1>
        <h2><%= (this.history == null) ?  "Current" : "History: " + history.history_text %></h2>
        <p></p>
    </div>
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Workflow", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.workflow_list_nav_select("Chronology", version)%>
        
    <div class="row">
        <div class="span2"></div>
        <div class="span8">
            <h2>Loan Number</h2>&nbsp;&nbsp; 
            <asp:TextBox ID="txtLoanId" runat="server"></asp:TextBox>
            <asp:Button ID="btnView" 
                runat="server"
                Text="View"
                CssClass="btn btn-primary"
                autoPostBack="true" onclick="btnView_Click" />
        </div>
        <div class="span2"></div>
    </div>
    <% if (this.foreclosure != null && this.foreclosure.loan != null && this.foreclosure.loan.loan_id > 0M)
       {
    %>
    <div class="row">
        <div class="span12">
            <h2>Loan Chronology for Loan: <%=this.foreclosure.loan.loan_id + " - " + this.foreclosure.loan.loan_name %></h2>
            <table class="table table-striped">
            <tr>
                <th>Event</th>
                <th>Date</th>
            </tr>
            <tr>
                <td>Demand Approved</td>
                <td><%= foreclosure.memo_fc_foreclosure_review_date.ToString() %></td>
            </tr>
            <tr>
                <td>Demand Initiated</td>
                <td><%= foreclosure.demand_letter_due_date.date.AddDays(-30).ToShortDateString() %></td>
            </tr>
            <tr>
                <td>Demand Current Date if Brought Current</td>
                <td>Need Help identifying source</td>
            </tr>
            <tr>
                <td>Demand Expired</td>
                <td><%= foreclosure.demand_letter_due_date.ToString() %></td>
            </tr>
            <tr>
                <td>Foreclosure Review Checklist Approved</td>
                <td><%= foreclosure_review_checklist.date.ToString() %></td>
            </tr>
            <tr>
                <td>PreForeclosure Review Checklist Date</td>
                <td><%= foreclosure.is_preforeclosure_complete() ? foreclosure.preforeclosure_events.lastest_preforeclosure_update().ToString() : "NA" %></td>
            </tr>
            <tr>
                <td>Foreclosure Referral Date</td>
                <td><%= foreclosure.fc_foreclosure_referral_date.ToString() %></td>
            </tr>
            <tr>
                <td>Foreclosure Posting Date</td>
                <td><%= foreclosure.posting_date.ToString() %></td>
            </tr>
            <tr>
                <td>Foreclosure Sale Date</td>
                <td><%= foreclosure.sale_date.ToString() %></td>
            </tr>
            <%if (this.foreclosure.loan.loan_type == "FHA")
              { %>
            <tr>
                <td>Conveyance Date</td>
                <td><%= (foreclosure.sale_conveyance != null ) ? foreclosure.sale_conveyance.conveyance_date.ToString() : ""%></td>
            </tr>
            <tr>
                <td>Claim Date Part B</td>
                <td><%= (foreclosure.sale_claim_part_b != null ) ? foreclosure.sale_claim_part_b.claim_part_b_date.ToString() : "" %></td>
            </tr>
            <% } %>
            <%if (this.foreclosure.loan.loan_type == "VA")
              { %>
            <tr>
                <td>Results of Sale</td>
                <td>Need Help identifying source</td>
            </tr>
            <tr>
                <td>Transfer of Custody Date</td>
                <td><%= (foreclosure.sale_transfer_of_custody != null ) ? foreclosure.sale_transfer_of_custody.transfer_of_custody_date.ToString() : "" %></td>
            </tr>
            <tr>
                <td>VA Claim Date</td>
                <td><%= (foreclosure.sale_basic_claim_events != null) ? foreclosure.sale_basic_claim_events.basic_claim_date.ToString() : ""%></td>
            </tr>
            <% } %>
            <%if (this.foreclosure.loan.loan_type == "CNV" || this.foreclosure.loan.loan_type == "CONV")
              { %>
            <tr>
                <td>REO Date</td>
                <td><%= (foreclosure.sale_reo_notice_events != null ) ? foreclosure.sale_reo_notice_events.reo_notice_date.ToString() : ""%></td>
            </tr>
            <tr>
                <td>Fannie Mae 571</td>
                <td><%= (foreclosure.sale_fannie_mae_claim_571_events != null) ? foreclosure.sale_fannie_mae_claim_571_events.fannie_mae_claim_571_date.ToString() : ""%></td>
            </tr>
            <% } %>
            </table>
            <p class="text-error"><asp:Label ID="lblError" runat="server" Text="" ></asp:Label></p>
            <p class="text-success"><asp:Label ID="lblSuccess" runat="server" Text="" ></asp:Label></p>
        </div>
    </div>
    <% 
       }
       else
       {
    %>
    <div class="row">
        <div class="span2"></div>
        <div class="span8">
            <h2>Loan Chronology Not found for Loan: <%=this.loan_id %></h2>
        </div>
    </div>
    <% } %>
    </div>
    </form>
</body>
</html>
