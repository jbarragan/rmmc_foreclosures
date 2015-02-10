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
    <div style="background:White">
    <div class="row">
        <br />
            <div class="span3">
            <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
            <h1>Foreclosures</h1>
        </div>
        <div class="span9">
        <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Workflow", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
        <%= com.sp.rmmc.foreclosures.views.NewNavigations.workflow_list_nav_select("", version)%>
        </div>
    </div>
    </div>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h2>Pre-Foreclosure Checklist.</h2><br />
        <table>
            <tr>
                <td><b>Loan Number:</b></td>
                <td><%= foreclosure.loan.loan_id %></td>
            </tr>
            <tr>
                <td><b>Loan Name:</b></td>
                <td><%= foreclosure.loan.loan_name %></td>
            </tr>
        </table>
        <table class="table table-striped">
            <tr>
                <th>Description</th>
                <th>Value</th>
                <th>Date</th>
                <th>Info</th>
            </tr>
            <tr>
                <td>Foreclosure Referral (Referral)</td>
                <td><asp:CheckBox runat="server" ID="chkFcForeclosureReferral" Enabled="false" /></td>
                <td><%= foreclosure.fc_foreclosure_referral_date.ToShortDateString() %></td>
                <td width="300px">
                    <a class="btn btn-inverse" onclick="var e = document.getElementById('fc_foreclosure_referral_date_info'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a>
                    <span id="fc_foreclosure_referral_date_info" style="display:none">Based on FC-Foreclosure Referral Memo</span>
                </td>
            </tr>
            <tr>
                <td>Foreclosure Referral (Papers to Attorney)</td>
                <td><asp:CheckBox runat="server" ID="chkPapersToAttorney" Enabled="false" /></td>
                <td><%= foreclosure.papers_to_attorney_date.ToShortDateString() %></td>
                <td width="300px">
                    <a class="btn btn-inverse" onclick="var e = document.getElementById('papers_to_attorney_date_info'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a>
                    <span id="papers_to_attorney_date_info" style="display:none">Based on Item Id 1000: Papers to Attorney</span>
                </td>
            </tr>
            <tr>
                <td>Payoff</td>
                <td style="background: 'gray';"><asp:CheckBox runat="server" ID="chkFcPayoff" Enabled="false" /></td>
                <td><%= foreclosure.fc_payoff_date.ToShortDateString() %></td>
                <td width="300px">
                    <a class="btn btn-inverse" onclick="var e = document.getElementById('fc_payoff_date_info'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a>
                    <span id="fc_payoff_date_info" style="display:none">Based on FC-Payoff Memo</span>
                </td>
            </tr>
            <tr>
                <td>Copy of Demand Letters</td>
                <td><asp:CheckBox runat="server" ID="chkCopyDemandLetters" Enabled="true" /></td>
                <td><%= foreclosure.preforeclosure_events.copy_demand_letters_date.ToShortDateString() %></td>
                <td></td>
            </tr>
            <tr>
                <td>Copy of Notes</td>
                <td><asp:CheckBox runat="server" ID="chkCopyNotes" Enabled="true" /></td>
                <td><%= foreclosure.preforeclosure_events.copy_notes_date.ToShortDateString() %></td>
                <td></td>
            </tr>
            <tr>
                <td>Copy of Deed of Trust</td>
                <td><asp:CheckBox runat="server" ID="chkCopyDeedOfTrust" Enabled="true" /></td>
                <td><%= foreclosure.preforeclosure_events.copy_deed_of_trust_date.ToShortDateString() %></td>
                <td></td>
            </tr>
            <tr>
                <td>Copy of Bankruptcy</td>
                <td><asp:CheckBox runat="server" ID="chkCopyBankruptcy" Enabled="true" /></td>
                <td><%= foreclosure.preforeclosure_events.copy_bankruptcy_date.ToShortDateString() %></td>
                <td></td>
            </tr>
            <tr>
                <td>Copy of Title Policy</td>
                <td><asp:CheckBox runat="server" ID="chkCopyTitlePolicy" Enabled="true" /></td>
                <td><%= foreclosure.preforeclosure_events.copy_title_policy_date.ToShortDateString() %></td>
                <td></td>
            </tr>
            <% if (foreclosure.loan.loan_type != "FHA")
               {
             %>
            <tr>
                <td> <%= (foreclosure.loan.loan_type == "VA") ? "Order Appraisal" : "Brokers Price Options" %></td>
                <td><asp:CheckBox runat="server" ID="chkAppraisalOrdered" Enabled="false" /></td>
                <td><%= foreclosure.appraisal_ordered_date.ToShortDateString()%></td>
                <td width="300px">
                    <a class="btn btn-inverse" onclick="var e = document.getElementById('appraisal_ordered_date_info'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a>
                    <span id="appraisal_ordered_date_info" style="display:none">Based on Item Id 1100: Appraisal Ordered</span>
                </td>
            </tr>
            <% if( foreclosure.loan.loan_type == "VA" ) { %>
            <tr>
                <td>Notify Mortgagor of Appraisal</td>
                <td><asp:CheckBox runat="server" ID="chkNotifyAppraisal" Enabled="true" /></td>
                <td><%= foreclosure.preforeclosure_events.notify_appraisal_date.ToShortDateString()%></td>
                <td></td>
            </tr>
            <% } %>
            <% } %>
        </table>
        <asp:Button ID="btnSave" 
            runat="server" 
            Text="Save" 
            CssClass="btn btn-success"
            Width="250px"
            autoPostBack="true" onclick="btnSave_Click"/>
                <p class="text-error"><asp:Label ID="lblError" runat="server" Text="" ></asp:Label></p>
                <p class="text-success"><asp:Label ID="lblSuccess" runat="server" Text="" ></asp:Label></p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
