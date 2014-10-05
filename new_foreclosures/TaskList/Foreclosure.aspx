<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Foreclosure.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Task List: Foreclosure</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
    <script type="text/jscript" src="../../assets/js/jquery.js" ></script>
</head>
<body>
    <form id="form2" runat="server">
    <div>
    <div class="container-print">
    <div style="background:White">
    <div class="row">
        <br />
            <div class="span3">
            <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
            <h1>Task List</h1>
            <h2>Foreclosures</h2>
        </div>
        <div class="span9">
            <%= com.sp.rmmc.foreclosures.views.NewNavigations.sub_main_nav_select("Task List") %>
            <%= com.sp.rmmc.foreclosures.views.NewNavigations.task_list_nav_select("Foreclosure Queue") %>
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
        <h2>Active (<%= recent_foreclosure_activated_all.Count%>)
        <a class="btn btn-primary" onclick="var e = document.getElementById('recent_foreclosure_activated_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
        <a class="btn btn-inverse" onclick="var e = document.getElementById('recent_foreclosure_activated_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="recent_foreclosure_activated_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.Foreclosure.recent_foreclosure_activated_conditions()%></span>
        <div id="recent_foreclosure_activated_table" style="display:none">
        <h3>FHA (<%= recent_foreclosure_activated_fha.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('recent_foreclosure_activated_fha'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="recent_foreclosure_activated_fha" style="display:none">
        <table id="active_fha_table" class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Next Payment Due Date</th>
                <th>Foreclosure Date</th>
                <th>Papers To Attorney Date</th>
                <th>Posting Date</th>
                <th>Officer(s)</th>
                <th style="display: none;">Last Reason Code Change </th>
                <th>Reason Code</th>
                <th style="display: none;">Last BC Memo Date</th>
                <th style="display: none;">Last Occupancy Code Change</th>
                <th>Occupancy Code</th>
                <th style="display: none;">Last PI Memo Date</th>
                <th>Checklist</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in recent_foreclosure_activated_fha)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                <td><%= foreclosure.loan.loan_type%></td>
                <td><%= foreclosure.loan.loan_name%></td>
                <td><%= foreclosure.loan.prin_bal.ToString("C")%></td>
                <td><%= foreclosure.loan.int_rate.ToString("0.000")%></td>
                <td><%= foreclosure.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= foreclosure.foreclosure_date.ToShortDateString() %></td>
                <td><%= foreclosure.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= foreclosure.posting_date.ToShortDateString()%></td>
                <td><%= foreclosure.foreclosure_officers%></td>
                <td style="display: none;"><%= foreclosure.last_default_reason_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#active_fha_table tr *:nth-child(13)').css('display') == 'block' ? $('#active_fha_table tr *:nth-child(13)').css('display', 'none') : $('#active_fha_table tr *:nth-child(13)').css('display', 'block');
                                $('#active_fha_table tr *:nth-child(11)').css('display') == 'block' ? $('#active_fha_table tr *:nth-child(11)').css('display', 'none') : $('#active_fha_table tr *:nth-child(11)').css('display', 'block');"><span class="<%= foreclosure.default_reason_code_status_color() %>"><%= foreclosure.main_default_reason_code%></span></a></td>
                <td style="display: none;"><%= foreclosure.last_bc_memo_date.ToShortDateString()%></td>
                <td style="display: none;"><%= foreclosure.last_occupancy_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#active_fha_table tr *:nth-child(14)').css('display') == 'block' ? $('#active_fha_table tr *:nth-child(14)').css('display', 'none') : $('#active_fha_table tr *:nth-child(14)').css('display', 'block');
                                $('#active_fha_table tr *:nth-child(16)').css('display') == 'block' ? $('#active_fha_table tr *:nth-child(16)').css('display', 'none') : $('#active_fha_table tr *:nth-child(16)').css('display', 'block');"><span class="<%= foreclosure.occupancy_code_status_color() %>"><%= foreclosure.occupancy_code %></span></a></td>
                <td style="display: none;"><%= foreclosure.last_pi_memo_date.ToShortDateString()%></td>
                <td><a href="PreforeclosureEvents.aspx?loan_id=<%=foreclosure.loan_id.ToString() %>">Checklist</a></td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        <h3>VA (<%= recent_foreclosure_activated_va.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('recent_foreclosure_activated_va'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="recent_foreclosure_activated_va" style="display:none">
        <table id="active_va_table" class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Next Payment Due Date</th>
                <th>Foreclosure Date</th>
                <th>Papers To Attorney Date</th>
                <th>Posting Date</th>
                <th>Officer(s)</th>
                <th style="display: none;">Last Reason Code Change </th>
                <th>Reason Code</th>
                <th style="display: none;">Last BC Memo Date</th>
                <th style="display: none;">Last Occupancy Code Change</th>
                <th>Occupancy Code</th>
                <th style="display: none;">Last PI Memo Date</th>
                <th>Checklist</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in recent_foreclosure_activated_va)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                <td><%= foreclosure.loan.loan_type%></td>
                <td><%= foreclosure.loan.loan_name%></td>
                <td><%= foreclosure.loan.prin_bal.ToString("C")%></td>
                <td><%= foreclosure.loan.int_rate.ToString("0.000")%></td>
                <td><%= foreclosure.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= foreclosure.foreclosure_date.ToShortDateString() %></td>
                <td><%= foreclosure.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= foreclosure.posting_date.ToShortDateString()%></td>
                <td><%= foreclosure.foreclosure_officers%></td>
                <td style="display: none;"><%= foreclosure.last_default_reason_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#active_va_table tr *:nth-child(13)').css('display') == 'block' ? $('#active_va_table tr *:nth-child(13)').css('display', 'none') : $('#active_va_table tr *:nth-child(13)').css('display', 'block');
                                $('#active_va_table tr *:nth-child(11)').css('display') == 'block' ? $('#active_va_table tr *:nth-child(11)').css('display', 'none') : $('#active_va_table tr *:nth-child(11)').css('display', 'block');"><span class="<%= foreclosure.default_reason_code_status_color() %>"><%= foreclosure.main_default_reason_code%></span></a></td>
                <td style="display: none;"><%= foreclosure.last_bc_memo_date.ToShortDateString()%></td>
                <td style="display: none;"><%= foreclosure.last_occupancy_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#active_va_table tr *:nth-child(14)').css('display') == 'block' ? $('#active_va_table tr *:nth-child(14)').css('display', 'none') : $('#active_va_table tr *:nth-child(14)').css('display', 'block');
                                $('#active_va_table tr *:nth-child(16)').css('display') == 'block' ? $('#active_va_table tr *:nth-child(16)').css('display', 'none') : $('#active_va_table tr *:nth-child(16)').css('display', 'block');"><span class="<%= foreclosure.occupancy_code_status_color() %>"><%= foreclosure.occupancy_code %></span></a></td>
                <td style="display: none;"><%= foreclosure.last_pi_memo_date.ToShortDateString()%></td>
                <td><a href="PreforeclosureEvents.aspx?loan_id=<%=foreclosure.loan_id.ToString() %>">Checklist</a></td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        <h3>CONV (<%= recent_foreclosure_activated_conv.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('recent_foreclosure_activated_conv'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="recent_foreclosure_activated_conv" style="display:none">
        <table id="active_conv_table" class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Next Payment Due Date</th>
                <th>Foreclosure Date</th>
                <th>Papers To Attorney Date</th>
                <th>Posting Date</th>
                <th>Officer(s)</th>
                <th style="display: none;">Last Reason Code Change </th>
                <th>Reason Code</th>
                <th style="display: none;">Last BC Memo Date</th>
                <th style="display: none;">Last Occupancy Code Change</th>
                <th>Occupancy Code</th>
                <th style="display: none;">Last PI Memo Date</th>
                <th>Checklist</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in recent_foreclosure_activated_conv)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                <td><%= foreclosure.loan.loan_type%></td>
                <td><%= foreclosure.loan.loan_name%></td>
                <td><%= foreclosure.loan.prin_bal.ToString("C")%></td>
                <td><%= foreclosure.loan.int_rate.ToString("0.000")%></td>
                <td><%= foreclosure.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= foreclosure.foreclosure_date.ToShortDateString() %></td>
                <td><%= foreclosure.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= foreclosure.posting_date.ToShortDateString()%></td>
                <td><%= foreclosure.foreclosure_officers%></td>
                <td style="display: none;"><%= foreclosure.last_default_reason_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#active_conv_table tr *:nth-child(13)').css('display') == 'block' ? $('#active_conv_table tr *:nth-child(13)').css('display', 'none') : $('#active_conv_table tr *:nth-child(13)').css('display', 'block');
                                $('#active_conv_table tr *:nth-child(11)').css('display') == 'block' ? $('#active_conv_table tr *:nth-child(11)').css('display', 'none') : $('#active_conv_table tr *:nth-child(11)').css('display', 'block');"><span class="<%= foreclosure.default_reason_code_status_color() %>"><%= foreclosure.main_default_reason_code%></span></a></td>
                <td style="display: none;"><%= foreclosure.last_bc_memo_date.ToShortDateString()%></td>
                <td style="display: none;"><%= foreclosure.last_occupancy_code_change_date.ToShortDateString()%></td>
                <td><a onclick="$('#active_conv_table tr *:nth-child(14)').css('display') == 'block' ? $('#active_conv_table tr *:nth-child(14)').css('display', 'none') : $('#active_conv_table tr *:nth-child(14)').css('display', 'block');
                                $('#active_conv_table tr *:nth-child(16)').css('display') == 'block' ? $('#active_conv_table tr *:nth-child(16)').css('display', 'none') : $('#active_conv_table tr *:nth-child(16)').css('display', 'block');"><span class="<%= foreclosure.occupancy_code_status_color() %>"><%= foreclosure.occupancy_code %></span></a></td>
                <td style="display: none;"><%= foreclosure.last_pi_memo_date.ToShortDateString()%></td>
                <td><a href="PreforeclosureEvents.aspx?loan_id=<%=foreclosure.loan_id.ToString() %>">Checklist</a></td>
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
            <h2>Inspection Expired (<%= inspection_expired.Count%>)
            <a class="btn btn-primary" onclick="var e = document.getElementById('inspection_expired_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
            <a class="btn btn-inverse" onclick="var e = document.getElementById('inspection_expired_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
            <span id="inspection_expired_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.Foreclosure.inspections_expired_conditions() %></span>
            <div id="inspection_expired_table" style="display:none">
            <h3>FHA (<%= inspection_expired_fha.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('inspection_expired_fha'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
            <div id="inspection_expired_fha" style="display:none">
            <table class="table table-striped">
                <tr>
                    <th>Loan#</th>
                    <th>Loan Type</th>
                    <th>Loan Name</th>
                    <th>Foreclosure Date</th>
                    <th>Officer(s)</th>
                    <th>Last Inspection Date</th>
                    <th>Last Inspection Type</th>
                    <th>Last Inspection Days Ago</th>
                </tr>
                <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in inspection_expired_fha)
                   {   
                %>
                <tr>
                    <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                    <td><%= foreclosure.loan.loan_type%></td>
                    <td><%= foreclosure.loan.loan_name%></td>
                    <td><%= foreclosure.foreclosure_date.ToShortDateString() %></td>
                    <td><%= foreclosure.foreclosure_officers %></td>
                    <td><%= foreclosure.last_inspection_memo_date.ToShortDateString() %></td>
                    <td><%= foreclosure.last_inspection_memo_description %></td>
                    <td><%= foreclosure.last_inspection_since() %></td>
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
                    <th>Foreclosure Date</th>
                    <th>Officer(s)</th>
                    <th>Last Inspection Date</th>
                    <th>Last Inspection Type</th>
                    <th>Last Inspection Days Ago</th>
                </tr>
                <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in inspection_expired_va)
                   {   
                %>
                <tr>
                    <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                    <td><%= foreclosure.loan.loan_type%></td>
                    <td><%= foreclosure.loan.loan_name%></td>
                    <td><%= foreclosure.foreclosure_date.ToShortDateString() %></td>
                    <td><%= foreclosure.foreclosure_officers %></td>
                    <td><%= foreclosure.last_inspection_memo_date.ToShortDateString() %></td>
                    <td><%= foreclosure.last_inspection_memo_description %></td>
                    <td><%= foreclosure.last_inspection_since() %></td>
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
                    <th>Foreclosure Date</th>
                    <th>Officer(s)</th>
                    <th>Last Inspection Date</th>
                    <th>Last Inspection Type</th>
                    <th>Last Inspection Days Ago</th>
                </tr>
                <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in inspection_expired_conv)
                   {   
                %>
                <tr>
                    <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                    <td><%= foreclosure.loan.loan_type%></td>
                    <td><%= foreclosure.loan.loan_name%></td>
                    <td><%= foreclosure.foreclosure_date.ToShortDateString() %></td>
                    <td><%= foreclosure.foreclosure_officers %></td>
                    <td><%= foreclosure.last_inspection_memo_date.ToShortDateString() %></td>
                    <td><%= foreclosure.last_inspection_memo_description %></td>
                    <td><%= foreclosure.last_inspection_since() %></td>
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
                    <th>Foreclosure Date</th>
                    <th>Officer(s)</th>
                    <th>Last Call Memo Date</th>
                </tr>
                <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in require_fannie_mae_call)
                   {   
                %>
                <tr>
                    <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                    <td><%= foreclosure.loan.loan_type%></td>
                    <td><%= foreclosure.loan.loan_name%></td>
                    <td><%= foreclosure.loan.borrower_phone %></td>
                    <td><%= foreclosure.foreclosure_date.ToShortDateString() %></td>
                    <td><%= foreclosure.foreclosure_officers %></td>
                    <td><%= foreclosure.last_bc_call_memo_date.ToShortDateString() %></td>
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
        <h2>Completed by Sale Date (<%= completed_by_sale_date_all.Count%>)
        <a class="btn btn-primary" onclick="var e = document.getElementById('completed_by_sale_date_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
        <a class="btn btn-inverse" onclick="var e = document.getElementById('completed_by_sale_date_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="completed_by_sale_date_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.Foreclosure.foreclosure_completed_by_sale_date_completed_conditions()%></span>
        <div id="completed_by_sale_date_table" style="display:none">
        <h3>FHA (<%= completed_by_sale_date_fha.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('completed_by_sale_date_fha'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="completed_by_sale_date_fha" style="display:none">
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>UPB</th>
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
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in completed_by_sale_date_fha)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                <td><%= foreclosure.loan.loan_type%></td>
                <td><%= foreclosure.loan.loan_name%></td>
                <td><%= foreclosure.loan.prin_bal.ToString("C")%></td>
                <td><%= foreclosure.loan.int_rate.ToString("0.000")%></td>
                <td><%= foreclosure.loan.due_date_next_payment.ToShortDateString()%></td>
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
            %>
        </table>
        </div>
        <h3>VA (<%= completed_by_sale_date_va.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('completed_by_sale_date_va'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="completed_by_sale_date_va" style="display:none">
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>UPB</th>
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
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in completed_by_sale_date_va)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                <td><%= foreclosure.loan.loan_type%></td>
                <td><%= foreclosure.loan.loan_name%></td>
                <td><%= foreclosure.loan.prin_bal.ToString("C")%></td>
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
            %>
        </table>
        </div>
        <h3>CONV (<%= completed_by_sale_date_conv.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('completed_by_sale_date_conv'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="completed_by_sale_date_conv" style="display:none">
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>UPB</th>
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
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in completed_by_sale_date_conv)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                <td><%= foreclosure.loan.loan_type%></td>
                <td><%= foreclosure.loan.loan_name%></td>
                <td><%= foreclosure.loan.prin_bal.ToString("C")%></td>
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
            %>
        </table>
        </div>
        </div>
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
