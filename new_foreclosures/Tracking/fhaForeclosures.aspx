<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fhaForeclosures.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures: Foreclosed Tracking.</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
    <style type="text/css">
        td.alert_notification
        {
            background-color: #FF2222;
            color: White;
            font-weight: bold;
        }
        td.warning_notification
        {
            background-color: #FFFF22;
            color: Black;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosures</h1>
        <p>Foreclosed Tracking.</p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.sub_main_nav %>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h2>FHA Foreclosed Tracking</h2><br />
        <table border="1" cellpadding="5" cellspacing="0">
            <tr>
                <th>LOAN #</th>
                <th>NAME</th>
                <th>TYPE</th>
                <th>1ST LEGAL</th>
                <th>upb</th>
                <th style="background-color: Red">F/C DATE</th>
                <th>FC 3rd PARTY SALE</th>
                <th>INSPECTION ORDERED</th>
                <th>EVICITON INITIATED</th>
                <th>WRIT OF POSSESION ORDERED</th>
                <th>EVICTION STOP</th>
                <th>EVICTION COMPLETED</th>
                <th>CONVEYANCE PACKAGE RECEIVED</th>
                <th>POSSESSION DATE</th>
                <th>CONVEYANCE (30 DAYS FROM POSSESSION)</th>
                <th>HUD ADVICE OF PAYMENT PART A</th>
                <th>PAYMENT AMOUNT PART A</th>
                <th>TITLE APPROVAL (45 DAYS FROM CONVEYANCE)</th>
                <th>CLAIM (WITH IN 15 DAYS OF TITLE APPROVAL LETTER)</th>
                <th>HUD ADVICE OF PAYMENT LETTER PART B</th>
                <th>CLAIM PAYMENT AMOUNT PART B</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosed foreclosed in all_foreclosed) { %>
            <tr>
                <td><%= foreclosed.loan.loan_id %></td>
                <td><%= foreclosed.loan.loan_name %></td>
                <td><%= foreclosed.loan.loan_type %></td> 
                <td <%= foreclosed.is_attorneys_posted_date_5_months_before_due_date_alert() ? "class='alert_notification'" : "" %>>
                    <%= foreclosed.attorneys_posted_date.ToShortDateString() %>
                </td>
                <td><%= foreclosed.getUPC().ToString("C")%> </td>
                <td><span style="color:Red;"><%= foreclosed.foreclosure_date.ToShortDateString() %></span> </td>
                <td><%= foreclosed.fc_3rd_party_sale_date.ToShortDateString() %></td>
                <td <%= foreclosed.is_inspection_ordered_NOT_ordered_1_day_after_foreclosure_alert() ? "class='alert_notification'" : "" %>>
                    <%= foreclosed.pi_conveyance_inspection_dt.ToString() %>
                </td>
                <td><%= foreclosed.fs_eviction_initiated %> </td>
                <td><%= foreclosed.writ_of_posession_ordered_date.ToShortDateString() %></td>
                <td><%= foreclosed.eviction_stop_date.ToShortDateString()%></td>
                <td <%= foreclosed.is_eviction_NOT_completed_45_day_after_foreclosure_alert() ? "class='alert_notification'" : "" %>>
                    <%= foreclosed.fcl_eviction_date.ToString()%>
                 </td>
                <td><%= foreclosed.secured_date.ToShortDateString()%></td>
                <td><%= foreclosed.possesion_date.ToShortDateString()%></td>
                <td <%= foreclosed.is_conveyance_part_a_30_after_posession_alert() ? "class='alert_notification'" : "" %>>
                    <%= foreclosed.part_a_ordered_date.ToShortDateString()%>
                </td>
                <td><%= foreclosed.part_a_received_date.ToShortDateString()%></td>
                <td><%= foreclosed.part_a_payment_amount.ToString("C") %></td>
                <td <%= foreclosed.is_title_approval_45_after_conveyance_part_a_alert() ? "class='alert_notification'" : (foreclosed.is_title_approval_40_after_conveyance_part_a_warning() ? "class='warning_notification'" : "")  %>>
                    <%= foreclosed.title_approval_ordered_date.ToShortDateString()%>
                 </td>
                <td <%= foreclosed.is_claim_part_b_15_after_title_approval_alert() ? "class='alert_notification'" : (foreclosed.is_claim_part_b_10_after_title_approval_warning() ? "class='warning_notification'" : "")  %>>
                    <%= foreclosed.part_bcd_ordered_date.ToShortDateString()%>
                </td>
                <td><%= foreclosed.part_bcd_received_date.ToShortDateString()%></td>
                <td><%= foreclosed.part_bcd_payment_amount.ToString("C") %> </td>
            </tr>
            <% } %>
        </table>

        <h2>First Legal Before 5 months Alert (<%= attorneys_posted_date_5_months_before_due_date_foreclosed.Count %>)
        <a class="btn btn-primary" onclick="var e = document.getElementById('attorneys_posted_date_5_months_before_due_date_foreclosed_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
        <a class="btn btn-inverse" onclick="var e = document.getElementById('attorneys_posted_date_5_months_before_due_date_foreclosed_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="attorneys_posted_date_5_months_before_due_date_foreclosed_conditions" style="display:none">
        Attorneys Posted date is 5 months before Due date of next payment.
        </span>
        <div id="attorneys_posted_date_5_months_before_due_date_foreclosed_table" style="display:none">
        <table  class="table table-striped">
            <tr><th>LOAN</th><th>NAME</th><th>DUE DATE</th><th>1ST LEGAL</th></tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosed foreclosed in attorneys_posted_date_5_months_before_due_date_foreclosed) 
               { %>
            <tr>
                <td><%= foreclosed.loan.loan_id %></td>
                <td><%= foreclosed.loan.loan_name %></td>
                <td><%= foreclosed.loan.due_date_next_payment.ToShortDateString() %></td> 
                <td><%= foreclosed.attorneys_posted_date.ToShortDateString() %></td>
            </tr>
            <% } %>
        </table>
        </div>
        <h2>Inspection more than one day after Foreclosure Alert (<%= inspection_ordered_NOT_ordered_1_day_after_foreclosure_alert_foreclosed.Count%>)
        <a class="btn btn-primary" onclick="var e = document.getElementById('inspection_ordered_NOT_ordered_1_day_after_foreclosure_alert_foreclosed_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
        <a class="btn btn-inverse" onclick="var e = document.getElementById('inspection_ordered_NOT_ordered_1_day_after_foreclosure_alert_foreclosed_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="inspection_ordered_NOT_ordered_1_day_after_foreclosure_alert_foreclosed_conditions" style="display:none">
        Inspection ordered more than one day after Foreclosure date.
        </span>
        <div id="inspection_ordered_NOT_ordered_1_day_after_foreclosure_alert_foreclosed_table" style="display:none">
        <table  class="table table-striped">
            <tr><th>LOAN</th><th>NAME</th><th>F/C DATE</th><th>INSPECTION ORDERED</th></tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosed foreclosed in inspection_ordered_NOT_ordered_1_day_after_foreclosure_alert_foreclosed)
               {
            %>
            <tr>
                <td><%= foreclosed.loan.loan_id %></td>
                <td><%= foreclosed.loan.loan_name %></td>
                <td><%= foreclosed.foreclosure_date.ToShortDateString() %></td> 
                <td><%= foreclosed.pi_conveyance_inspection_dt.ToString() %></td>
            </tr>
            <% } %>
        </table>
        </div>
        <h2>Eviction NOT completed 45-day before Foreclosure Alert (<%= eviction_NOT_completed_45_day_after_foreclosure_alert_foreclosed.Count%>)
        <a class="btn btn-primary" onclick="var e = document.getElementById('eviction_NOT_completed_45_day_after_foreclosure_alert_foreclosed_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
        <a class="btn btn-inverse" onclick="var e = document.getElementById('eviction_NOT_completed_45_day_after_foreclosure_alert_foreclosed_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="eviction_NOT_completed_45_day_after_foreclosure_alert_foreclosed_conditions" style="display:none">
        Eviction not completed 45 days before Foreclosure date.
        </span>
        <div id="eviction_NOT_completed_45_day_after_foreclosure_alert_foreclosed_table" style="display:none">
        <table  class="table table-striped">
            <tr><th>LOAN</th><th>NAME</th><th>F/C DATE</th><th>EVICTION DATE</th></tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosed foreclosed in eviction_NOT_completed_45_day_after_foreclosure_alert_foreclosed)
               {
            %>
            <tr>
                <td><%= foreclosed.loan.loan_id %></td>
                <td><%= foreclosed.loan.loan_name %></td>
                <td><%= foreclosed.foreclosure_date.ToShortDateString() %></td> 
                <td><%= foreclosed.fcl_eviction_date.ToString()%></td>
            </tr>
            <% } %>
        </table>
        </div>
        <h2>Conveyance 30-day after Possession Alert (<%= conveyance_part_a_30_after_posession_alert_foreclosed.Count%>)
        <a class="btn btn-primary" onclick="var e = document.getElementById('conveyance_part_a_30_after_posession_alert_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
        <a class="btn btn-inverse" onclick="var e = document.getElementById('conveyance_part_a_30_after_posession_alert_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="conveyance_part_a_30_after_posession_alert_conditions" style="display:none">
        Conveyance 30 days after Possession date.
        </span>
        <div id="conveyance_part_a_30_after_posession_alert_table" style="display:none">
        <table  class="table table-striped">
            <tr><th>LOAN</th><th>NAME</th><th>POSSESSION DATE</th><th>CONVEYANCE DATE</th></tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosed foreclosed in conveyance_part_a_30_after_posession_alert_foreclosed)
               {
            %>
            <tr>
                <td><%= foreclosed.loan.loan_id %></td>
                <td><%= foreclosed.loan.loan_name %></td>
                <td><%= foreclosed.possesion_date.ToShortDateString()%></td> 
                <td><%= foreclosed.part_a_ordered_date.ToString()%></td>
            </tr>
            <% } %>
        </table>
        </div>
        <h2>Title Approval 45-day after Conveyance date Alert (<%= title_approval_45_after_conveyance_part_a_alert_foreclosed.Count%>)
        <a class="btn btn-primary" onclick="var e = document.getElementById('title_approval_45_after_conveyance_part_a_alert_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
        <a class="btn btn-inverse" onclick="var e = document.getElementById('title_approval_45_after_conveyance_part_a_alert_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="title_approval_45_after_conveyance_part_a_alert_conditions" style="display:none">
        Title Approval 45-day after Conveyance date.
        </span>
        <div id="title_approval_45_after_conveyance_part_a_alert_table" style="display:none">
        <table  class="table table-striped">
            <tr><th>LOAN</th><th>NAME</th><th>TITLE APPROVAL DATE</th><th>CONVEYANCE DATE</th></tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosed foreclosed in title_approval_45_after_conveyance_part_a_alert_foreclosed)
               {
            %>
            <tr>
                <td><%= foreclosed.loan.loan_id %></td>
                <td><%= foreclosed.loan.loan_name %></td>
                <td><%= foreclosed.title_approval_ordered_date.ToShortDateString()%></td> 
                <td><%= foreclosed.part_a_ordered_date.ToString()%></td>
            </tr>
            <% } %>
        </table>
        </div>
        <h2>Claim 15 days after Title date Alert (<%= claim_part_b_15_after_title_approval_alert_foreclosed.Count%>)
        <a class="btn btn-primary" onclick="var e = document.getElementById('claim_part_b_15_after_title_approval_alert_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
        <a class="btn btn-inverse" onclick="var e = document.getElementById('claim_part_b_15_after_title_approval_alert_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="claim_part_b_15_after_title_approval_alert_conditions" style="display:none">
        Claim 15 days after Title date.
        </span>
        <div id="claim_part_b_15_after_title_approval_alert_table" style="display:none">
        <table  class="table table-striped">
            <tr><th>LOAN</th><th>NAME</th><th>TITLE APPROVAL DATE</th><th>CLAIM DATE</th></tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosed foreclosed in claim_part_b_15_after_title_approval_alert_foreclosed)
               {
            %>
            <tr>
                <td><%= foreclosed.loan.loan_id %></td>
                <td><%= foreclosed.loan.loan_name %></td>
                <td><%= foreclosed.title_approval_ordered_date.ToShortDateString()%></td> 
                <td><%= foreclosed.part_bcd_ordered_date.ToString()%></td>
            </tr>
            <% } %>
        </table>
        </div>
        
    </div>
    </div>
    </div>
    </form>
</body>
</html>
