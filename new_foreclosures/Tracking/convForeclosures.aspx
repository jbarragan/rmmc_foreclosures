<%@ Page Language="C#" AutoEventWireup="true" CodeFile="convForeclosures.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

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
        <h2>Conventional Foreclosed Tracking</h2><br />
        <table border="1" cellpadding="5" cellspacing="0">
            <tr>
                <th>LOAN #</th>
                <th>NAME</th>
                <th>TYPE</th>
                <th>1ST LEGAL</th>
                <th>upb</th>
                <th style="background-color: Red">F/C DATE</th>

                <th>REO NOTICE</th>
                <th>FANNIE MAE 571 CLAIM</th>
                <th>FANNIE MAE CLAIM CHECK</th>
                <th>CLAIM AMOUNT</th>
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

                <td><%= foreclosed.fc_reo_notification_date.ToShortDateString() %> </td>
                <td <%= foreclosed.is_fannie_mae_claim_30_after_foreclosure_date_alert() ? "class='alert_notification'" : "" %>>
                    <%= foreclosed.fannie_mae_claim_571_date.ToShortDateString()%>
                </td>
                <td><%= foreclosed.fannie_mae_claim_571_check_date.ToShortDateString()%></td>
                <td><%= foreclosed.fannie_mae_claim_571_amount.ToString("C") %> </td>
            </tr>
            <% } %>
        </table>
        </div>
    </div>
    <div class="row">
        <div class="span12">
        
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
        <h2>Fannie Mae Claim 30 days after F/C Alert (<%= fannie_mae_claim_30_after_foreclosure_date_foreclosed.Count%>)
        <a class="btn btn-primary" onclick="var e = document.getElementById('fannie_mae_claim_30_after_foreclosure_date_foreclosed_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
        <a class="btn btn-inverse" onclick="var e = document.getElementById('fannie_mae_claim_30_after_foreclosure_date_foreclosed_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="fannie_mae_claim_30_after_foreclosure_date_foreclosed_conditions" style="display:none">
        Attorneys Posted date is 5 months before Due date of next payment.
        </span>
        <div id="fannie_mae_claim_30_after_foreclosure_date_foreclosed_table" style="display:none">
        <table  class="table table-striped">
            <tr><th>LOAN</th><th>NAME</th><th>FORECLOSURE DATE</th><th>FANNIE MAE 571 CLAIML</th></tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosed foreclosed in fannie_mae_claim_30_after_foreclosure_date_foreclosed) 
               { %>
            <tr>
                <td><%= foreclosed.loan.loan_id %></td>
                <td><%= foreclosed.loan.loan_name %></td>
                <td><%= foreclosed.foreclosure_date.ToShortDateString() %></td> 
                <td><%= foreclosed.fannie_mae_claim_571_date.ToShortDateString() %></td>
            </tr>
            <% } %>
        </table>
        </div>
    </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
