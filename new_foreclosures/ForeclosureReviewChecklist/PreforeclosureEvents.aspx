<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreforeclosureEvents.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Foreclosures: Foreclosure Review Checklist.</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosures</h1>
        <p>Foreclosure Review Checklist.</p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.sub_main_nav %>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h2>Foreclosure Review Checklist.</h2><br />
        <asp:Label ID="lblComplete" runat="server" Text="<h3 class='text-success'>Checklist is Complete, Click Incomplete to change status</h3>"></asp:Label>
        <asp:Label ID="lblIncomplete" runat="server" Text="<h3 class='text-error'>Checklist is Incomplete, Click Complete to change status</h3>"></asp:Label>
        <asp:Button ID="btnCompleteTop" 
            runat="server" 
            Text="Checklist Complete" 
            CssClass="btn btn-success"
            Width="250px"
            autoPostBack="true" onclick="btnComplete_Click"/>
        <asp:Button ID="btnIncompleteTop" 
            runat="server" 
            Text="Checklist Incomplete" 
            CssClass="btn btn-danger"
            Width="250px"
            autoPostBack="true" onclick="btnIncomplete_Click"/><br /><br />
        <b>Date: </b><%= DateTime.Today.ToShortDateString() %><br />
        <br />
        <b>RMMC LN#: </b><%= demand_letter.loan.loan_id %><br />
        <b>BORROWERS NAME: </b><%= demand_letter.loan.loan_name %><br />
        <b>PROPERTY ADDRESS: </b><%= demand_letter.loan.property_address %> <%= demand_letter.loan.property_city %>, <%= demand_letter.loan.property_state %> <%= demand_letter.loan.property_zip %><br />
        <br />
        1.  WAS A PROPERTY INSPECTION ORDERED? <b><%= (demand_letter.review_checklist.pi_text.Length > 0) ? "YES, On " + demand_letter.review_checklist.pi_dt.ToShortDateString() : "NO"%></b><br /><br />
        2.  WAS THERE CONTACT ON INSPECTION WITH BORROWER AND WHAT WAS THE OUTCOME? <br />
        <b><i><%= demand_letter.review_checklist.pi_dt.ToShortDateString() %></i><br /><%= demand_letter.review_checklist.pi_text %></b><br /><br />
        3.  DID THE BORROWER SUBMIT ANY FINANCIAL FORM FOR LOSS MITIGATION REVIEW? <br /><b>NA</b><br /><br />
        4.  DID BORROWER DISCUSS THAT HE OR SHE NEEDED HELP AND WAS GOING TO SUBMIT A FINANCIAL FORM? <b><%= (demand_letter.review_checklist.bc_first_right_party_contact_text.Length > 0) ? "YES" : "NO" %></b><br />
        <b><i><%= demand_letter.review_checklist.bc_first_right_party_contact_dt.ToShortDateString() %></i><br />
        <%= (demand_letter.review_checklist.bc_first_right_party_contact_text.Length > 100) ? 
            demand_letter.review_checklist.bc_first_right_party_contact_text.Substring(0, 100) : 
            demand_letter.review_checklist.bc_first_right_party_contact_text %></b><br /><br />
        
        5.  DID BORROWER PROMISE TO PAY BUT DID NOT COMPLY AND WHY? <b><%= (demand_letter.review_checklist.bc_promised_to_pay_text.Length > 0) ? "YES" : "NO" %></b><br />
        <b><i><%= demand_letter.review_checklist.bc_promised_to_pay_dt.ToShortDateString()%></i><br /><%= demand_letter.review_checklist.bc_promised_to_pay_text %></b><br /><br />
        6.  WAS THE PROPERTY VACANT OR ABANDONED AND WAS IT SECURED?  <b><%= (demand_letter.review_checklist.pi_initial_vacant_inspection_text.Length > 0) ? "YES" : "NO" %></b><br />
        <b><i><%= demand_letter.review_checklist.pi_initial_vacant_inspection_dt.ToShortDateString()%></i><br /><%= demand_letter.review_checklist.pi_initial_vacant_inspection_text %></b><br /><br />
        7.  DID WE HAVE A REASON DILENQUENCY AND WAS THE PROPER REASON CODE POPULATED ON MS?  <b><%= (demand_letter.review_checklist.default_reason_code.Length > 0) ? "YES" : "NO" %></b><br />
        <b><%= demand_letter.review_checklist.default_reason_code %> - <%= demand_letter.review_checklist.bc_reason_for_deliquency_text %></b><br /><br />
        8.  IF BORROWER WAS RELEASED FROM BANKRUPTCY AND IT'S AN FHA ACCOUNT DO WE HAVE TO REQUEST AN EXTENSION OF TIME TO FORECLOSURE? IS THE MORTGAGE STATUS CORRECT ON MS FOR HUB REPORTING? <b>NA</b><br /><br />
        9.  HAVE TAXES AND INSURANCE PAID TO DATE? <b>LAST TAXES PAID DUE DATE: <%= demand_letter.review_checklist.last_taxes_paid_date.ToShortDateString() %></b><br />
             <b>LAST INSURANCE PAID DUE DATE: <%= demand_letter.review_checklist.last_insurance_paid_date.ToShortDateString() %></b><br /><br />
        10.  DO WE HAVE DIFFERENT MAILING ADDRESS?  <b><%= (demand_letter.review_checklist.corr_name.Length > 0) ? "YES" : "NO" %></b><br />
        <% if (demand_letter.review_checklist.corr_name.Length > 0)
           {  %>
            <b><%= demand_letter.review_checklist.corr_name%></b><br />
            <b><%= demand_letter.review_checklist.corr_address2%></b><br />
            <b><%= demand_letter.review_checklist.corr_address1%></b><br />
            <b><%= demand_letter.review_checklist.corr_city%>, <%= demand_letter.review_checklist.corr_state%> <%= demand_letter.review_checklist.corr_zip%></b><br /><br />
        <% } %>
        <br />
        COMMENTS:<br />
        <b><i><%= demand_letter.review_checklist.fc_other_fc_action_dt.ToShortDateString()%></i><br /><%= demand_letter.review_checklist.fc_other_fc_action_text %></b><br /><br />
        REVIEWED BY: <b><%= demand_letter.foreclosure_officers %></b><br /><br />
        APPROVED BY: <b>__________________________________________</b></b><br /><br />
        <asp:Button ID="btnComplete" 
            runat="server" 
            Text="Checklist Complete" 
            CssClass="btn btn-success"
            Width="250px"
            autoPostBack="true" onclick="btnComplete_Click"/>
        <asp:Button ID="btnIncomplete" 
            runat="server" 
            Text="Checklist Incomplete" 
            CssClass="btn btn-danger"
            Width="250px"
            autoPostBack="true" onclick="btnIncomplete_Click"/>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
