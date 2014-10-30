<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Demand.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Task List: Demand</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
    <script type="text/jscript" src="../../assets/js/jquery.js"></script>
    <script type="text/javascript">
        // Constants
        // Reason Codes Columns:
        var reason_code_start_column = 10;
        var reason_code_last_change_column = reason_code_start_column;
        var reason_code_last_bc_memo_date_column = reason_code_start_column + 2;

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-print">
    <div style="background:White">
    <div class="row">
        <br />
        <div class="span3">
            <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
            <h1>Task List</h1>
            <h2>Demand Queue</h2>
        </div>
        <div class="span9">
            <%= com.sp.rmmc.foreclosures.views.NewNavigations.sub_main_nav_select("Task List", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
            <%= com.sp.rmmc.foreclosures.views.NewNavigations.task_list_nav_select("Demand Queue") %>
        </div>
    </div>
    </div>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
            Loan Officer(s):&nbsp;&nbsp; 
            <asp:DropDownList ID="ddlLoanOfficer" runat="server" AutoPostBack="true"
                onselectedindexchanged="ddlLoanOfficer_SelectedIndexChanged"></asp:DropDownList>
        </div>
    </div>
    <div class="row">
        <div class="span12">
        <h2>Expired (<%= in_demand_letter_expired_mode.Count %>)
        <a class="btn btn-primary" onclick="var e = document.getElementById('in_demand_letter_expired_mode_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
        <a class="btn btn-inverse" onclick="var e = document.getElementById('in_demand_letter_expired_mode_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="in_demand_letter_expired_mode_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.DemandLetter.in_demand_letter_expired_mode_conditions() %></span>
        <div id="in_demand_letter_expired_mode_table" style="display:none">
        <h3>FHA (<%= in_demand_letter_expired_mode_fha.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('in_demand_letter_expired_mode_table_fha'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="in_demand_letter_expired_mode_table_fha" style="display:none">
        <table id="expired_fha_table" class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Demand Letter Due Date</th>
                <th>Months Demand after Delinquent</th>
                <th>Officer(s)</th>
                <th style="display: none;">Last Reason Code Change </th>
                <th>Reason Code</th>
                <th style="display: none;">Last BC Memo Date</th>
                <th style="display: none;">Last Occupancy Code Change</th>
                <th>Occupancy Code</th>
                <th style="display: none;">Last PI Memo Date</th>
                <th>Letters Sent</th>
                <th>Review Checklist</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in in_demand_letter_expired_mode_fha)
               {
            %>
            <tr>
                <td><%= demand_letter.loan_id.ToString()%></td>
                <td><%= demand_letter.loan.loan_type%></td>
                <td><%= demand_letter.loan.loan_name%></td>
                <td><%= demand_letter.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= demand_letter.loan.prin_bal.ToString("C")%></td>
                <td><%= demand_letter.loan.int_rate.ToString("0.000")%></td>
                <td><%= demand_letter.demand_letter_due_date.ToShortDateString()%></td>
                <td><span class="<%= demand_letter.months_demand_date_after_delinquent_color() %>"><%= demand_letter.months_demand_date_after_delinquent() %></span></td>
                <td><%= demand_letter.foreclosure_officers%></td>
                <td style="display: none;"><%= demand_letter.last_default_reason_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#expired_fha_table tr *:nth-child(12)').css('display') == 'block' ? $('#expired_fha_table tr *:nth-child(12)').css('display', 'none') : $('#expired_fha_table tr *:nth-child(12)').css('display', 'block');
                                $('#expired_fha_table tr *:nth-child(10)').css('display') == 'block' ? $('#expired_fha_table tr *:nth-child(10)').css('display', 'none') : $('#expired_fha_table tr *:nth-child(10)').css('display', 'block');"><span class="<%= demand_letter.default_reason_code_status_color() %>"><%= demand_letter.default_reason_code %></span></a></td>
                <td style="display: none;"><%= demand_letter.last_bc_memo_date.ToShortDateString()%></td>
                <td style="display: none;"><%= demand_letter.last_occupancy_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#expired_fha_table tr *:nth-child(13)').css('display') == 'block' ? $('#expired_fha_table tr *:nth-child(13)').css('display', 'none') : $('#expired_fha_table tr *:nth-child(13)').css('display', 'block');
                                $('#expired_fha_table tr *:nth-child(15)').css('display') == 'block' ? $('#expired_fha_table tr *:nth-child(15)').css('display', 'none') : $('#expired_fha_table tr *:nth-child(15)').css('display', 'block');"><span class="<%= demand_letter.occupancy_code_status_color() %>"><%= demand_letter.all_occupancy_codes_descriptions() %></span></a></td>
                <td style="display: none;"><%= demand_letter.last_pi_memo_date.ToShortDateString()%></td>
                <td><span class="<%= demand_letter.letters_sent_status_color() %>"><%= demand_letter.last_nl_demand_memo_date.ToString() %></span></td>
                <td>
                    <a href="../ForeclosureReviewChecklist/Default.aspx?loan_id=<%=demand_letter.loan_id.ToString() %>">Checklist</a>
                    <span class="text-success"><%= (Array.IndexOf(array_completed_checklists_loan_ids, demand_letter.loan_id) >= 0) ? "Completed" : ""%></span>
                </td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        <h3>VA (<%= in_demand_letter_expired_mode_va.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('in_demand_letter_expired_mode_table_va'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="in_demand_letter_expired_mode_table_va" style="display:none">
        <table id="expired_va_table" class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Demand Letter Due Date</th>
                <th>Months Demand after Delinquent</th>
                <th>Officer(s)</th>
                <th style="display: none;">Last Reason Code Change </th>
                <th>Reason Code</th>
                <th style="display: none;">Last BC Memo Date</th>
                <th style="display: none;">Last Occupancy Code Change</th>
                <th>Occupancy Code</th>
                <th style="display: none;">Last PI Memo Date</th>
                <th>Letters Sent</th>
                <th>Review Checklist</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in in_demand_letter_expired_mode_va)
               {
            %>
            <tr>
                <td><%= demand_letter.loan_id.ToString()%></td>
                <td><%= demand_letter.loan.loan_type%></td>
                <td><%= demand_letter.loan.loan_name%></td>
                <td><%= demand_letter.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= demand_letter.loan.prin_bal.ToString("C")%></td>
                <td><%= demand_letter.loan.int_rate.ToString("0.000")%></td>
                <td><%= demand_letter.demand_letter_due_date.ToShortDateString()%></td>
                <td><span class="<%= demand_letter.months_demand_date_after_delinquent_color() %>"><%= demand_letter.months_demand_date_after_delinquent() %></span></td>
                <td><%= demand_letter.foreclosure_officers%></td>
                <td style="display: none;"><%= demand_letter.last_default_reason_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#expired_va_table tr *:nth-child(12)').css('display') == 'block' ? $('#expired_va_table tr *:nth-child(12)').css('display', 'none') : $('#expired_va_table tr *:nth-child(12)').css('display', 'block');
                                $('#expired_va_table tr *:nth-child(10)').css('display') == 'block' ? $('#expired_va_table tr *:nth-child(10)').css('display', 'none') : $('#expired_va_table tr *:nth-child(10)').css('display', 'block');"><span class="<%= demand_letter.default_reason_code_status_color() %>"><%= demand_letter.default_reason_code %></span></a></td>
                <td style="display: none;"><%= demand_letter.last_bc_memo_date.ToShortDateString()%></td>
                <td style="display: none;"><%= demand_letter.last_occupancy_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#expired_va_table tr *:nth-child(13)').css('display') == 'block' ? $('#expired_va_table tr *:nth-child(13)').css('display', 'none') : $('#expired_va_table tr *:nth-child(13)').css('display', 'block');
                                $('#expired_va_table tr *:nth-child(15)').css('display') == 'block' ? $('#expired_va_table tr *:nth-child(15)').css('display', 'none') : $('#expired_va_table tr *:nth-child(15)').css('display', 'block');"><span class="<%= demand_letter.occupancy_code_status_color() %>"><%= demand_letter.all_occupancy_codes_descriptions() %></span></a></td>
                <td style="display: none;"><%= demand_letter.last_pi_memo_date.ToShortDateString()%></td>
                <td><span class="<%= demand_letter.letters_sent_status_color() %>"><%= demand_letter.last_nl_demand_memo_date.ToString() %></span></td>
                <td>
                    <a href="../ForeclosureReviewChecklist/Default.aspx?loan_id=<%=demand_letter.loan_id.ToString() %>">Checklist</a>
                    <span class="text-success"><%= (Array.IndexOf(array_completed_checklists_loan_ids, demand_letter.loan_id) >= 0) ? "Completed" : ""%></span>
                </td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        <h3>CONV (<%= in_demand_letter_expired_mode_conv.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('in_demand_letter_expired_mode_table_conv'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="in_demand_letter_expired_mode_table_conv" style="display:none">
        <table id="expired_conv_table" class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Demand Letter Due Date</th>
                <th>Months Demand after Delinquent</th>
                <th>Officer(s)</th>
                <th style="display: none;">Last Reason Code Change </th>
                <th>Reason Code</th>
                <th style="display: none;">Last BC Memo Date</th>
                <th style="display: none;">Last Occupancy Code Change</th>
                <th>Occupancy Code</th>
                <th style="display: none;">Last PI Memo Date</th>
                <th>Letters Sent</th>
                <th>Review Checklist</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in in_demand_letter_expired_mode_conv)
               {
            %>
            <tr>
                <td><%= demand_letter.loan_id.ToString()%></td>
                <td><%= demand_letter.loan.loan_type%></td>
                <td><%= demand_letter.loan.loan_name%></td>
                <td><%= demand_letter.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= demand_letter.loan.prin_bal.ToString("C")%></td>
                <td><%= demand_letter.loan.int_rate.ToString("0.000")%></td>
                <td><%= demand_letter.demand_letter_due_date.ToShortDateString()%></td>
                <td><span class="<%= demand_letter.months_demand_date_after_delinquent_color() %>"><%= demand_letter.months_demand_date_after_delinquent() %></span></td>
                <td><%= demand_letter.foreclosure_officers%></td>
                <td style="display: none;"><%= demand_letter.last_default_reason_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#expired_conv_table tr *:nth-child(12)').css('display') == 'block' ? $('#expired_conv_table tr *:nth-child(12)').css('display', 'none') : $('#expired_conv_table tr *:nth-child(12)').css('display', 'block');
                                $('#expired_conv_table tr *:nth-child(10)').css('display') == 'block' ? $('#expired_conv_table tr *:nth-child(10)').css('display', 'none') : $('#expired_conv_table tr *:nth-child(10)').css('display', 'block');"><span class="<%= demand_letter.default_reason_code_status_color() %>"><%= demand_letter.default_reason_code %></span></a></td>
                <td style="display: none;"><%= demand_letter.last_bc_memo_date.ToShortDateString()%></td>
                <td style="display: none;"><%= demand_letter.last_occupancy_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#expired_conv_table tr *:nth-child(13)').css('display') == 'block' ? $('#expired_conv_table tr *:nth-child(13)').css('display', 'none') : $('#expired_conv_table tr *:nth-child(13)').css('display', 'block');
                                $('#expired_conv_table tr *:nth-child(15)').css('display') == 'block' ? $('#expired_conv_table tr *:nth-child(15)').css('display', 'none') : $('#expired_conv_table tr *:nth-child(15)').css('display', 'block');"><span class="<%= demand_letter.occupancy_code_status_color() %>"><%= demand_letter.all_occupancy_codes_descriptions() %></span></a></td>
                <td style="display: none;"><%= demand_letter.last_pi_memo_date.ToShortDateString()%></td>
                <td><span class="<%= demand_letter.letters_sent_status_color() %>"><%= demand_letter.last_nl_demand_memo_date.ToString() %></span></td>
                <td>
                    <a href="../ForeclosureReviewChecklist/Default.aspx?loan_id=<%=demand_letter.loan_id.ToString() %>">Checklist</a>
                    <span class="text-success"><%= (Array.IndexOf(array_completed_checklists_loan_ids, demand_letter.loan_id) >= 0) ? "Completed" : ""%></span>
                </td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        </div>
        </div>
    </div>
    <div class="row">
        <div class="span12">
        <h2>Active (<%= active_demands.Count %>)
        <a class="btn btn-primary" onclick="var e = document.getElementById('active_demands_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
        <a class="btn btn-inverse" onclick="var e = document.getElementById('active_demands_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="active_demands_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.DemandLetter.in_pre_demand_letter_mode_conditions()%></span>
        <div id="active_demands_table" style="display:none">
        <h3>FHA (<%= active_demands_fha.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('active_demands_fha'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="active_demands_fha" style="display:none">
        <table id="active_fha_table" class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Demand Letter Due Date</th>
                <th>Months Demand after Delinquent</th>
                <th>Officer(s)</th>
                <th style="display: none;">Last Reason Code Change </th>
                <th>Reason Code</th>
                <th style="display: none;">Last BC Memo Date</th>
                <th style="display: none;">Last Occupancy Code Change</th>
                <th>Occupancy Code</th>
                <th style="display: none;">Last PI Memo Date</th>
                <th>Letters Sent</th>
                <th>Review Checklist</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in active_demands_fha)
               { 
            %>
            <tr class="<%= demand_letter.in_pre_demand_letter_tr_class() %>">
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                <td><%= demand_letter.loan.loan_type%></td>
                <td><%= demand_letter.loan.loan_name%></td>
                <td><%= demand_letter.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= demand_letter.loan.prin_bal.ToString("C")%></td>
                <td><%= demand_letter.loan.int_rate.ToString("0.000")%></td>
                <td><%= demand_letter.demand_letter_due_date.ToShortDateString()%></td>
                <td><span class="<%= demand_letter.months_demand_date_after_delinquent_color() %>"><%= demand_letter.months_demand_date_after_delinquent() %></span></td>
                <td><%= demand_letter.foreclosure_officers%></td>
                <td style="display: none;"><%= demand_letter.last_default_reason_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#active_fha_table tr *:nth-child(12)').css('display') == 'block' ? $('#active_fha_table tr *:nth-child(12)').css('display', 'none') : $('#active_fha_table tr *:nth-child(12)').css('display', 'block');
                                $('#active_fha_table tr *:nth-child(10)').css('display') == 'block' ? $('#active_fha_table tr *:nth-child(10)').css('display', 'none') : $('#active_fha_table tr *:nth-child(10)').css('display', 'block');"><span class="<%= demand_letter.default_reason_code_status_color() %>"><%= demand_letter.default_reason_code %></span></a></td>
                <td style="display: none;"><%= demand_letter.last_bc_memo_date.ToShortDateString()%></td>
                <td style="display: none;"><%= demand_letter.last_occupancy_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#active_fha_table tr *:nth-child(13)').css('display') == 'block' ? $('#active_fha_table tr *:nth-child(13)').css('display', 'none') : $('#active_fha_table tr *:nth-child(13)').css('display', 'block');
                                $('#active_fha_table tr *:nth-child(15)').css('display') == 'block' ? $('#active_fha_table tr *:nth-child(15)').css('display', 'none') : $('#active_fha_table tr *:nth-child(15)').css('display', 'block');"><span class="<%= demand_letter.occupancy_code_status_color() %>"><%= demand_letter.all_occupancy_codes_descriptions() %></span></a></td>
                <td style="display: none;"><%= demand_letter.last_pi_memo_date.ToShortDateString()%></td>
                <td><span class="<%= demand_letter.letters_sent_status_color() %>"><%= demand_letter.last_nl_demand_memo_date.ToString() %></span></td>
                <td>
                    <a href="../ForeclosureReviewChecklist/Default.aspx?loan_id=<%=demand_letter.loan_id.ToString() %>">Checklist</a>
                    <span class="text-success"><%= (Array.IndexOf(array_completed_checklists_loan_ids, demand_letter.loan_id) >= 0) ? "Completed" : ""%></span>
                </td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        <h3>VA (<%= active_demands_va.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('active_demands_va'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="active_demands_va" style="display:none">
        <table id="active_va_table" class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Demand Letter Due Date</th>
                <th>Months Demand after Delinquent</th>
                <th>Officer(s)</th>
                <th style="display: none;">Last Reason Code Change </th>
                <th>Reason Code</th>
                <th style="display: none;">Last BC Memo Date</th>
                <th style="display: none;">Last Occupancy Code Change</th>
                <th>Occupancy Code</th>
                <th style="display: none;">Last PI Memo Date</th>
                <th>Letters Sent</th>
                <th>Review Checklist</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in active_demands_va)
               { 
            %>
            <tr class="<%= demand_letter.in_pre_demand_letter_tr_class() %>">
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                <td><%= demand_letter.loan.loan_type%></td>
                <td><%= demand_letter.loan.loan_name%></td>
                <td><%= demand_letter.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= demand_letter.loan.prin_bal.ToString("C")%></td>
                <td><%= demand_letter.loan.int_rate.ToString("0.000")%></td>
                <td><%= demand_letter.demand_letter_due_date.ToShortDateString()%></td>
                <td><span class="<%= demand_letter.months_demand_date_after_delinquent_color() %>"><%= demand_letter.months_demand_date_after_delinquent() %></span></td>
                <td><%= demand_letter.foreclosure_officers%></td>
                <td style="display: none;"><%= demand_letter.last_default_reason_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#active_va_table tr *:nth-child(12)').css('display') == 'block' ? $('#active_va_table tr *:nth-child(12)').css('display', 'none') : $('#active_va_table tr *:nth-child(12)').css('display', 'block');
                                $('#active_va_table tr *:nth-child(10)').css('display') == 'block' ? $('#active_va_table tr *:nth-child(10)').css('display', 'none') : $('#active_va_table tr *:nth-child(10)').css('display', 'block');"><span class="<%= demand_letter.default_reason_code_status_color() %>"><%= demand_letter.default_reason_code %></span></a></td>
                <td style="display: none;"><%= demand_letter.last_bc_memo_date.ToShortDateString()%></td>
                <td style="display: none;"><%= demand_letter.last_occupancy_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#active_va_table tr *:nth-child(13)').css('display') == 'block' ? $('#active_va_table tr *:nth-child(13)').css('display', 'none') : $('#active_va_table tr *:nth-child(13)').css('display', 'block');
                                $('#active_va_table tr *:nth-child(15)').css('display') == 'block' ? $('#active_va_table tr *:nth-child(15)').css('display', 'none') : $('#active_va_table tr *:nth-child(15)').css('display', 'block');"><span class="<%= demand_letter.occupancy_code_status_color() %>"><%= demand_letter.all_occupancy_codes_descriptions() %></span></a></td>
                <td style="display: none;"><%= demand_letter.last_pi_memo_date.ToShortDateString()%></td>
                <td><span class="<%= demand_letter.letters_sent_status_color() %>"><%= demand_letter.last_nl_demand_memo_date.ToString() %></span></td>
                <td>
                    <a href="../ForeclosureReviewChecklist/Default.aspx?loan_id=<%=demand_letter.loan_id.ToString() %>">Checklist</a>
                    <span class="text-success"><%= (Array.IndexOf(array_completed_checklists_loan_ids, demand_letter.loan_id) >= 0) ? "Completed" : ""%></span>
                </td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        <h3>CONV (<%= active_demands_conv.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('active_demands_conv'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="active_demands_conv" style="display:none">
        <table id="active_conv_table" class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Demand Letter Due Date</th>
                <th>Months Demand after Delinquent</th>
                <th>Officer(s)</th>
                <th style="display: none;">Last Reason Code Change </th>
                <th>Reason Code</th>
                <th style="display: none;">Last BC Memo Date</th>
                <th style="display: none;">Last Occupancy Code Change</th>
                <th>Occupancy Code</th>
                <th style="display: none;">Last PI Memo Date</th>
                <th>Letters Sent</th>
                <th>Review Checklist</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in active_demands_conv)
               { 
            %>
            <tr class="<%= demand_letter.in_pre_demand_letter_tr_class() %>">
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                <td><%= demand_letter.loan.loan_type%></td>
                <td><%= demand_letter.loan.loan_name%></td>
                <td><%= demand_letter.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= demand_letter.loan.prin_bal.ToString("C")%></td>
                <td><%= demand_letter.loan.int_rate.ToString("0.000")%></td>
                <td><%= demand_letter.demand_letter_due_date.ToShortDateString()%></td>
                <td><span class="<%= demand_letter.months_demand_date_after_delinquent_color() %>"><%= demand_letter.months_demand_date_after_delinquent() %></span></td>
                <td><%= demand_letter.foreclosure_officers%></td>
                <td style="display: none;"><%= demand_letter.last_default_reason_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#active_conv_table tr *:nth-child(12)').css('display') == 'block' ? $('#active_conv_table tr *:nth-child(12)').css('display', 'none') : $('#active_conv_table tr *:nth-child(12)').css('display', 'block');
                                $('#active_conv_table tr *:nth-child(10)').css('display') == 'block' ? $('#active_conv_table tr *:nth-child(10)').css('display', 'none') : $('#active_conv_table tr *:nth-child(10)').css('display', 'block');"><span class="<%= demand_letter.default_reason_code_status_color() %>"><%= demand_letter.default_reason_code %></span></a></td>
                <td style="display: none;"><%= demand_letter.last_bc_memo_date.ToShortDateString()%></td>
                <td style="display: none;"><%= demand_letter.last_occupancy_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#active_conv_table tr *:nth-child(13)').css('display') == 'block' ? $('#active_conv_table tr *:nth-child(13)').css('display', 'none') : $('#active_conv_table tr *:nth-child(13)').css('display', 'block');
                                $('#active_conv_table tr *:nth-child(15)').css('display') == 'block' ? $('#active_conv_table tr *:nth-child(15)').css('display', 'none') : $('#active_conv_table tr *:nth-child(15)').css('display', 'block');"><span class="<%= demand_letter.occupancy_code_status_color() %>"><%= demand_letter.all_occupancy_codes_descriptions() %></span></a></td>
                <td style="display: none;"><%= demand_letter.last_pi_memo_date.ToShortDateString()%></td>
                <td><span class="<%= demand_letter.letters_sent_status_color() %>"><%= demand_letter.last_nl_demand_memo_date.ToString() %></span></td>
                <td>
                    <a href="../ForeclosureReviewChecklist/Default.aspx?loan_id=<%=demand_letter.loan_id.ToString() %>">Checklist</a>
                    <span class="text-success"><%= (Array.IndexOf(array_completed_checklists_loan_ids, demand_letter.loan_id) >= 0) ? "Completed" : ""%></span>
                </td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        </div>
        </div>
    </div>
    <div class="row">
        <div class="span12">
        <h2>That Became Current (<%= remove_demand_letter_by_last_reg_payment.Count%>)
        <a class="btn btn-primary" onclick="var e = document.getElementById('remove_demand_letter_by_last_reg_payment_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
        <a class="btn btn-inverse" onclick="var e = document.getElementById('remove_demand_letter_by_last_reg_payment_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="remove_demand_letter_by_last_reg_payment_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.DemandLetter.remove_demand_letter_by_last_reg_payment_conditions()%></span>
        <div id="remove_demand_letter_by_last_reg_payment_table" style="display:none">
        <h3>FHA (<%= remove_demand_letter_by_last_reg_payment_fha.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('remove_demand_letter_by_last_reg_payment_fha'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="remove_demand_letter_by_last_reg_payment_fha" style="display:none">
        <h4>Without Mortgage Status 20 (<%= remove_demand_letter_by_last_reg_payment_fha_mortgage_status.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('remove_demand_letter_by_last_reg_payment_fha_mortgage_status'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h4>
        <div id="remove_demand_letter_by_last_reg_payment_fha_mortgage_status" style="display:none">
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>Demand Letter Due Date</th>
                <th>Officer(s)</th>
                <th>Last Payment Received Date</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in remove_demand_letter_by_last_reg_payment_fha_mortgage_status)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                <td><%= demand_letter.loan.loan_type%></td>
                <td><%= demand_letter.loan.loan_name%></td>
                <td><%= demand_letter.loan.due_date_next_payment.ToShortDateString() %></td>
                <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                <td><%= demand_letter.foreclosure_officers %></td>
                <td><%= demand_letter.last_reg_paid_dt.ToShortDateString()  %></td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        <h4>Without Current Occupancy Letter Date (<%= remove_demand_letter_by_last_reg_payment_fha_current_occupancy_letter.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('remove_demand_letter_by_last_reg_payment_fha_current_occupancy_letter'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h4>
        <div id="remove_demand_letter_by_last_reg_payment_fha_current_occupancy_letter" style="display:none">
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>Demand Letter Due Date</th>
                <th>Officer(s)</th>
                <th>Last Payment Received Date</th>
                <th>Mortgate Status Date</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in remove_demand_letter_by_last_reg_payment_fha_current_occupancy_letter)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                <td><%= demand_letter.loan.loan_type%></td>
                <td><%= demand_letter.loan.loan_name%></td>
                <td><%= demand_letter.loan.due_date_next_payment.ToShortDateString() %></td>
                <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                <td><%= demand_letter.foreclosure_officers %></td>
                <td><%= demand_letter.last_reg_paid_dt.ToShortDateString()  %></td>
                <td><%= demand_letter.mortgage_status_date.ToShortDateString()  %></td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        <h4> With Both (<%= remove_demand_letter_by_last_reg_payment_fha_other.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('remove_demand_letter_by_last_reg_payment_fha_other'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h4>
        <div id="remove_demand_letter_by_last_reg_payment_fha_other" style="display:none">
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>Demand Letter Due Date</th>
                <th>Officer(s)</th>
                <th>Last Payment Received Date</th>
                <th>Mortgate Status Date</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in remove_demand_letter_by_last_reg_payment_fha_other)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                <td><%= demand_letter.loan.loan_type%></td>
                <td><%= demand_letter.loan.loan_name%></td>
                <td><%= demand_letter.loan.due_date_next_payment.ToShortDateString() %></td>
                <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                <td><%= demand_letter.foreclosure_officers %></td>
                <td><%= demand_letter.last_reg_paid_dt.ToShortDateString()  %></td>
                <td><%= demand_letter.mortgage_status_date.ToShortDateString()  %></td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        </div><h3>VA (<%= remove_demand_letter_by_last_reg_payment_va.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('remove_demand_letter_by_last_reg_payment_va'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="remove_demand_letter_by_last_reg_payment_va" style="display:none">
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>Demand Letter Due Date</th>
                <th>Officer(s)</th>
                <th>Last Payment Received Date</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in remove_demand_letter_by_last_reg_payment_va)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                <td><%= demand_letter.loan.loan_type%></td>
                <td><%= demand_letter.loan.loan_name%></td>
                <td><%= demand_letter.loan.due_date_next_payment.ToShortDateString() %></td>
                <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                <td><%= demand_letter.foreclosure_officers %></td>
                <td><%= demand_letter.last_reg_paid_dt.ToShortDateString()  %></td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        <h3>CONV (<%= remove_demand_letter_by_last_reg_payment_conv.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('remove_demand_letter_by_last_reg_payment_conv'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="remove_demand_letter_by_last_reg_payment_conv" style="display:none">
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>Demand Letter Due Date</th>
                <th>Officer(s)</th>
                <th>Last Payment Received Date</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in remove_demand_letter_by_last_reg_payment_conv)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                <td><%= demand_letter.loan.loan_type%></td>
                <td><%= demand_letter.loan.loan_name%></td>
                <td><%= demand_letter.loan.due_date_next_payment.ToShortDateString() %></td>
                <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                <td><%= demand_letter.foreclosure_officers %></td>
                <td><%= demand_letter.last_reg_paid_dt.ToShortDateString()  %></td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        </div>
        <div class="row">
            <div class="span12">
            <h2>Inspection Expired (<%= inspection_expired.Count%>)
            <a class="btn btn-primary" onclick="var e = document.getElementById('inspection_expired_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
            <a class="btn btn-inverse" onclick="var e = document.getElementById('inspection_expired_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
            <span id="inspection_expired_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.DemandLetter.inspections_expired_conditions() %></span>
            <div id="inspection_expired_table" style="display:none">
            <h3>FHA (<%= inspection_expired_fha.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('inspection_expired_fha'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
            <div id="inspection_expired_fha" style="display:none">
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Type</th>
                    <th>Loan Name</th>
                    <th>Demand Letter Due Date</th>
                    <th>Officer(s)</th>
                    <th>Last Notify Inspection Date</th>
                    <th>Last Inspection Date</th>
                    <th>Last Inspection Type</th>
                    <th>Last Inspection Days Ago</th>
                </tr>
                <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in inspection_expired_fha)
                   {   
                %>
                <tr>
                    <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                    <td><%= demand_letter.loan.loan_type%></td>
                    <td><%= demand_letter.loan.loan_name%></td>
                    <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                    <td><%= demand_letter.foreclosure_officers %></td>
                    <td><%= demand_letter.last_notify_inspection_memo_date.ToShortDateString() %></td>
                    <td><%= demand_letter.last_inspection_memo_date.ToShortDateString() %></td>
                    <td><%= demand_letter.last_inspection_memo_description %></td>
                    <td><%= demand_letter.last_inspection_since() %></td>
                 </tr>
                <%
                    }
                %>
            </table>
            </div>
            <h3>VA (<%= inspection_expired_va.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('inspection_expired_va'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
            <div id="inspection_expired_va" style="display:none">
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Type</th>
                    <th>Loan Name</th>
                    <th>Demand Letter Due Date</th>
                    <th>Officer(s)</th>
                    <th>Last Notify Inspection Date</th>
                    <th>Last Inspection Date</th>
                    <th>Last Inspection Type</th>
                    <th>Last Inspection Days Ago</th>
                </tr>
                <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in inspection_expired_va)
                   {   
                %>
                <tr>
                    <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                    <td><%= demand_letter.loan.loan_type%></td>
                    <td><%= demand_letter.loan.loan_name%></td>
                    <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                    <td><%= demand_letter.foreclosure_officers %></td>
                    <td><%= demand_letter.last_notify_inspection_memo_date.ToShortDateString() %></td>
                    <td><%= demand_letter.last_inspection_memo_date.ToShortDateString() %></td>
                    <td><%= demand_letter.last_inspection_memo_description %></td>
                    <td><%= demand_letter.last_inspection_since() %></td>
                 </tr>
                <%
                    }
                %>
            </table>
            </div>
            <h3>CONV (<%= inspection_expired_conv.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('inspection_expired_conv'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
            <div id="inspection_expired_conv" style="display:none">
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Type</th>
                    <th>Loan Name</th>
                    <th>Demand Letter Due Date</th>
                    <th>Officer(s)</th>
                    <th>Last Notify Inspection Date</th>
                    <th>Last Inspection Date</th>
                    <th>Last Inspection Type</th>
                    <th>Last Inspection Days Ago</th>
                </tr>
                <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in inspection_expired_conv)
                   {   
                %>
                <tr>
                    <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                    <td><%= demand_letter.loan.loan_type%></td>
                    <td><%= demand_letter.loan.loan_name%></td>
                    <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                    <td><%= demand_letter.foreclosure_officers %></td>
                    <td><%= demand_letter.last_notify_inspection_memo_date.ToShortDateString() %></td>
                    <td><%= demand_letter.last_inspection_memo_date.ToShortDateString() %></td>
                    <td><%= demand_letter.last_inspection_memo_description %></td>
                    <td><%= demand_letter.last_inspection_since() %></td>
                 </tr>
                <%
                    }
                %>
            </table>
            </div>
            </div>
            </div>
        </div>
        <div class="row">
            <div class="span12">
            <h2>That Went Into Loss Mitigation (<%= remove_lm_activity.Count%>)
            <a class="btn btn-primary" onclick="var e = document.getElementById('remove_lm_activity_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
            <a class="btn btn-inverse" onclick="var e = document.getElementById('remove_lm_activity_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
            <span id="remove_lm_activity_conditions" style="display:none">Check Loss Mitigation system for Package Received.</span>
            <div id="remove_lm_activity_table" style="display:none">
            <h3>FHA (<%= remove_lm_activity_fha.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('remove_lm_activity_fha'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
            <div id="remove_lm_activity_fha" style="display:none">
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Type</th>
                    <th>Loan Name</th>
                    <th>Demand Letter Due Date</th>
                    <th>Officer(s)</th>
                </tr>
                <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in remove_lm_activity_fha)
                   {   
                %>
                <tr>
                    <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                    <td><%= demand_letter.loan.loan_type%></td>
                    <td><%= demand_letter.loan.loan_name%></td>
                    <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                    <td><%= demand_letter.foreclosure_officers %></td>
                 </tr>
                <%
                    }
                %>
            </table>
            </div>
            <h3>VA (<%= remove_lm_activity_va.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('remove_lm_activity_va'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
            <div id="remove_lm_activity_va" style="display:none">
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Type</th>
                    <th>Loan Name</th>
                    <th>Demand Letter Due Date</th>
                    <th>Officer(s)</th>
                </tr>
                <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in remove_lm_activity_va)
                   {   
                %>
                <tr>
                    <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                    <td><%= demand_letter.loan.loan_type%></td>
                    <td><%= demand_letter.loan.loan_name%></td>
                    <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                    <td><%= demand_letter.foreclosure_officers %></td>
                 </tr>
                <%
                    }
                %>
            </table>
            </div>
            <h3>CONV (<%= remove_lm_activity_conv.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('remove_lm_activity_conv'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
            <div id="remove_lm_activity_conv" style="display:none">
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Type</th>
                    <th>Loan Name</th>
                    <th>Demand Letter Due Date</th>
                    <th>Officer(s)</th>
                </tr>
                <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in remove_lm_activity_conv)
                   {   
                %>
                <tr>
                    <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                    <td><%= demand_letter.loan.loan_type%></td>
                    <td><%= demand_letter.loan.loan_name%></td>
                    <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                    <td><%= demand_letter.foreclosure_officers %></td>
                 </tr>
                <%
                    }
                %>
            </table>
            </div>
            </div>
            </div>
        </div>
        <div class="row">
            <div class="span12">
            <h2>That Went Into Repayment Plan (<%= 0%>)</h2>
            </div>
        </div>
        <div class="row">
            <div class="span12">
            <h2>That Went Into Bankruptcy (<%= remove_from_demand_letter_by_bankruptcy.Count%>)
            <a class="btn btn-primary" onclick="var e = document.getElementById('remove_from_demand_letter_by_bankruptcy_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
            <a class="btn btn-inverse" onclick="var e = document.getElementById('remove_from_demand_letter_by_bankruptcy_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
            <span id="remove_from_demand_letter_by_bankruptcy_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.DemandLetter.remove_from_demand_letter_by_bankruptcy_conditions()%></span>
            <div id="remove_from_demand_letter_by_bankruptcy_table" style="display:none">
            <h3>FHA (<%= remove_from_demand_letter_by_bankruptcy_fha.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('remove_from_demand_letter_by_bankruptcy_fha'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
            <div id="remove_from_demand_letter_by_bankruptcy_fha" style="display:none">
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Type</th>
                    <th>Loan Name</th>
                    <th>Demand Letter Due Date</th>
                    <th>Officer(s)</th>
                    <th>Chapter Filed</th>
                    <th>Bankruptcy Filed Date</th>
                </tr>
                <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in remove_from_demand_letter_by_bankruptcy_fha)
                   {   
                %>
                <tr>
                    <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                    <td><%= demand_letter.loan.loan_type%></td>
                    <td><%= demand_letter.loan.loan_name%></td>
                    <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                    <td><%= demand_letter.foreclosure_officers %></td>
                    <td><%= demand_letter.chapter_filed%></td>
                    <td><%= demand_letter.bankruptcy_filed_date.ToShortDateString() %></td>
                 </tr>
                <%
                    }
                %>
            </table>
            </div>
            <h3>VA (<%= remove_from_demand_letter_by_bankruptcy_va.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('remove_from_demand_letter_by_bankruptcy_va'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
            <div id="remove_from_demand_letter_by_bankruptcy_va" style="display:none">
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Type</th>
                    <th>Loan Name</th>
                    <th>Demand Letter Due Date</th>
                    <th>Officer(s)</th>
                    <th>Chapter Filed</th>
                    <th>Bankruptcy Filed Date</th>
                </tr>
                <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in remove_from_demand_letter_by_bankruptcy_va)
                   {   
                %>
                <tr>
                    <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                    <td><%= demand_letter.loan.loan_type%></td>
                    <td><%= demand_letter.loan.loan_name%></td>
                    <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                    <td><%= demand_letter.foreclosure_officers %></td>
                    <td><%= demand_letter.chapter_filed%></td>
                    <td><%= demand_letter.bankruptcy_filed_date.ToShortDateString() %></td>
                 </tr>
                <%
                    }
                %>
            </table>
            </div>
            <h3>CONV (<%= remove_from_demand_letter_by_bankruptcy_conv.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('remove_from_demand_letter_by_bankruptcy_conv'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
            <div id="remove_from_demand_letter_by_bankruptcy_conv" style="display:none">
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Type</th>
                    <th>Loan Name</th>
                    <th>Demand Letter Due Date</th>
                    <th>Officer(s)</th>
                    <th>Chapter Filed</th>
                    <th>Bankruptcy Filed Date</th>
                </tr>
                <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in remove_from_demand_letter_by_bankruptcy_conv)
                   {   
                %>
                <tr>
                    <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                    <td><%= demand_letter.loan.loan_type%></td>
                    <td><%= demand_letter.loan.loan_name%></td>
                    <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                    <td><%= demand_letter.foreclosure_officers %></td>
                    <td><%= demand_letter.chapter_filed%></td>
                    <td><%= demand_letter.bankruptcy_filed_date.ToShortDateString() %></td>
                 </tr>
                <%
                    }
                %>
            </table>
            </div>
            </div>
        <div class="row">
            <div class="span12">
            <h2>Fannie Mae Calls (<%= require_fannie_mae_call.Count%>)
            <a class="btn btn-primary" onclick="var e = document.getElementById('fannie_mae_calls_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
            <a class="btn btn-inverse" onclick="var e = document.getElementById('fannie_mae_calls_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i>Conditions</a></h2>
            <span id="fannie_mae_calls_conditions" style="display:none">Last "BC-" memo removing 'BC-Promised to Pay'.</span>
            <div id="fannie_mae_calls_table" style="display:none">
            <h3>CONV (<%= require_fannie_mae_call.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('fannie_mae_calls_conv'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
            <div id="fannie_mae_calls_conv" style="display:none">
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Type</th>
                    <th>Loan Name</th>
                    <th>Borrower Phone</th>
                    <th>Demand Letter Due Date</th>
                    <th>Officer(s)</th>
                    <th>Last Call Memo Date</th>
                </tr>
                <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in require_fannie_mae_call)
                   {   
                %>
                <tr>
                    <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                    <td><%= demand_letter.loan.loan_type%></td>
                    <td><%= demand_letter.loan.loan_name%></td>
                    <td><%= demand_letter.loan.borrower_phone%></td>
                    <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                    <td><%= demand_letter.foreclosure_officers %></td>
                    <td><%= demand_letter.last_bc_call_memo_date.ToShortDateString() %></td>
                 </tr>
                <%
                    }
                %>
            </table>
            </div>
            </div>
            </div>
        </div>
    </div>
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
