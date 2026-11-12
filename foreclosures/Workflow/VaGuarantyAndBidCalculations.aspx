<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VaGuarantyAndBidCalculations.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - VA Guaranty And Bid Calculations</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>VA Guaranty And Bid Calculations</h1>
        <h2><%= (this.history == null) ?  "Current" : "History: " + history.history_text %></h2>
        <p></p>
    </div>
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Workflow", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.workflow_list_nav_select("Foreclosures", this.version)%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.workflow_sublist_nav_select("Foreclosures", "VA Guaranty And Bid", this.version)%>
        
    <div class="row">
        <div class="span12">
            <div id="InCalculations"></div>
            <h2>Loan Number</h2>&nbsp;&nbsp; 
            <asp:TextBox ID="txtLoanId" runat="server"></asp:TextBox>
            <asp:Button ID="btnView" 
                runat="server"
                Text="View"
                CssClass="btn btn-primary"
                autoPostBack="true" onclick="btnView_Click" />
        </div>
    </div>
    <% if (this.foreclosure != null && this.foreclosure.loan != null && this.foreclosure.loan.loan_id > 0M && this.foreclosure.sale_va_guaranty_and_bid_calcs != null)
       {
    %>
    <div class="row">
        <div class="span12">
            <h2>VA Guaranty And Bid Calculations for Loan: <%=this.foreclosure.loan.loan_id + " - " + this.foreclosure.loan.loan_name %></h2>
            <h3>External Data Sources</h3>
            <table class="table table-striped">
            <tr>
                <th>DAYS OF INTEREST</th>
                <th>LIQ ESPE</th>
                <th>LIQ EXP BEFORE</th>
                <th>LIQ EXP AFTER</th>
                <th>ESCROW ADVANCES PRIOR</th>
                <th>ESCROW ADVANCES AFTER</th>
                <th>PROPERTY PRESERVATION BEFORE</th>
                <th>PROPERTY PRESERVATION AFTER</th>
                <th>APPRAISAL FEE</th>
                <th>CREDITS</th>
                <th>VA COST FACTOR</th>
                <th></th>
            </tr>
            <tr>
                <td><asp:TextBox ID="txtDaysOfInterest" runat="server" MaxLength="4" Width="50px"></asp:TextBox></td>
                <td><asp:TextBox ID="txtLiqEspe" runat="server" MaxLength="8" Width="90px"></asp:TextBox></td>
                <td><asp:TextBox ID="txtLiqExpBefore" runat="server" MaxLength="8" Width="90px"></asp:TextBox></td>
                <td><asp:TextBox ID="txtLiqExpAfter" runat="server" MaxLength="8" Width="90px"></asp:TextBox></td>
                <td><asp:TextBox ID="txtEscrowAdvancesPrior" runat="server" MaxLength="8" Width="90px"></asp:TextBox></td>
                <td><asp:TextBox ID="txtEscrowAdvancesAfter" runat="server" MaxLength="8" Width="90px"></asp:TextBox></td>
                <td><asp:TextBox ID="txtPropertyPreservationBefore" runat="server" MaxLength="8" Width="90px"></asp:TextBox></td>
                <td><asp:TextBox ID="txtPropertyPreservationAfter" runat="server" MaxLength="8" Width="90px"></asp:TextBox></td>
                <td><asp:TextBox ID="txtAppraisalFee" runat="server" MaxLength="8" Width="90px"></asp:TextBox></td>
                <td><asp:TextBox ID="txtCredits" runat="server" MaxLength="8" Width="90px"></asp:TextBox></td>
                <td><asp:TextBox ID="txtCostFactor" runat="server" MaxLength="8" Width="90px"></asp:TextBox></td>
                <td>
                    <asp:Button ID="btnRefresh" 
                            runat="server"
                            Text="Refresh"
                            CssClass="btn btn-primary"
                            autoPostBack="true" onclick="btnRefresh_Click" />
                </td>
            </tr>
            </table>
            <p class="text-error"><asp:Label ID="lblError" runat="server" Text="" ></asp:Label></p>
            <p class="text-success"><asp:Label ID="lblSuccess" runat="server" Text="" ></asp:Label></p>
        </div>
    </div>
    <div class="row">
        <div class="span12">
            <h3>VA BID CALCULATION</h3>
            <table class="table table-striped">
            <tr>
                <th>UPB AT ORIGINATION</th>
                <th>UPB AT DELIQUENCY</th>
                <th>DAYS OF INTEREST</th>
                <th>RATE</th>
                <th>ORIGINAL GUARANTY AMOUNT</th>
                <th>ACCRUED INTEREST</th>
                <th>LIQ ESPE</th>
                <th>ESCROW ADVANCES PRIOR</th>
                <th>TOTAL INDEBTEDNESS</th>
                <th>PROPERTY PRESERVATION BEFORE</th>
                <th>APPRAISAL FEE</th>
                <th>CREDITS</th>
                <th>APPRAISAL VALUE</th>
                <th>VA COST FACTOR</th>
                <th>NET VALUE</th>
                <th>BID</th>
            </tr>
            <tr>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.original_prin_balance.ToString("C") %></td>
                <td><%= this.foreclosure.loan.prin_bal.ToString("C") %></td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.days_of_interest %></td>
                <td><%= String.Format( "{0:0.00}%", this.foreclosure.loan.int_rate ) %></td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.va_guaranty_amount.ToString("C") %></td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.accured_interest.ToString("C")%></td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.liq_espe.ToString("C") %> </td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.escrow_advances_prior.ToString("C") %> </td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.total_indebtedness.ToString("C") %></td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.property_preservation_before.ToString("C") %> </td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.appraisal_fee.ToString("C")%></td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.credits.ToString("C") %> </td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.appraisal_value.ToString("C")%></td>
                <td><%= String.Format("{0:0.00}%", this.foreclosure.sale_va_guaranty_and_bid_calcs.va_cost_factor )%></td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.net_value.ToString("C")%></td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.bid.ToString("C")%></td>
            </tr>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="span12">
            <h3>VA GUARANTY CALCULATION</h3>
            <table class="table table-striped">
            <tr>
                <th>ORIGINAL GUARANTY AMOUNT</th>
                <th>MAX GUARANTY PCT</th>
                <th>GUARANTY</th>
                <th>MAX GUARANTY AMOUNT</th>
            </tr>
            <tr>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.va_guaranty_amount.ToString("C") %></td>
                <td><%= String.Format("{0:0.0000}%", this.foreclosure.sale_va_guaranty_and_bid_calcs.max_guaranty_pct * 100 )%></td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.guaranty.ToString("C") %></td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.max_guaranty_amount.ToString("C") %></td>
            </tr>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="span12">
            <h3>VA CLAIM PAYMENT</h3>
            <table class="table table-striped">
            <tr>
                <th>UPB AT ORIGINATION</th>
                <th>UPB AT DELIQUENCY</th>
                <th>DAYS OF INTEREST</th>
                <th>RATE</th>
                <th>INTEREST PAID BY VA</th>
                <th>LIQ EXP BEFORE</th>
                <th>ESCROW ADVANCES PRIOR</th>
                <th>ESCROW ADVANCES AFTER</th>
                <th>LIQ EXP AFTER</th>
                <th>PROPERTY PRESERVATION BEFORE</th>
                <th>PROPERTY PRESERVATION AFTER</th>
                <th>PRINCIPAL & INTEREST ONLY</th>
                <th>TOTAL INDEBTEDNESS</th>
                <th>PROCEEDS OF SALE</th>
                <th>CLAIM AMOUNT</th>
                <th>GROSS CLAIM AMOUNT</th>
                <th>POTENTIAL TOTAL AMOUNT PAID BY VA</th>
            </tr>
            <tr>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.original_prin_balance.ToString("C") %></td>
                <td><%= this.foreclosure.loan.prin_bal.ToString("C") %></td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.days_of_interest %></td>
                <td><%= String.Format( "{0:0.00}%", this.foreclosure.loan.int_rate ) %></td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.accured_interest.ToString("C")%></td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.liq_exp_before.ToString("C") %> </td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.escrow_advances_prior.ToString("C") %> </td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.escrow_advances_after.ToString("C") %> </td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.liq_exp_after.ToString("C") %> </td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.property_preservation_before.ToString("C") %> </td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.property_preservation_after.ToString("C") %> </td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.principal_and_interest_only.ToString("C") %> </td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.payment_total_indebtedness.ToString("C")%></td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.proceeds_of_sale.ToString("C")%></td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.claim_amount.ToString("C") %> </td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.gross_claim_amount.ToString("C") %> </td>
                <td><%= this.foreclosure.sale_va_guaranty_and_bid_calcs.potential_total_amount_paid_by_va.ToString("C")%></td>
            </tr>
            </table>
        </div>
    </div>
    <% } %>
    </div>
    </form>
</body>
</html>
