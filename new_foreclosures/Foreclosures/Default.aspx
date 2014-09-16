<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Foreclosures: Recent Foreclosure Activated.</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
    <script type="text/jscript" src="../../assets/js/jquery.js" />
    <script language="javascript" type="text/jscript">
        $('[data-toggle="tooltip"]').tooltip({
            'placement': 'top'
        });
        $('[data-toggle="popover"]').popover({
            trigger: 'hover',
            'placement': 'top'
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosures</h1>
        <p>Recent Loans with Foreclosure Activated.</p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.sub_main_nav %>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h2>Loans with Foreclosure Activated. (<%= recent_foreclosure_activated.Count%>)
        <a class="btn btn-inverse" onclick="var e = document.getElementById('recent_foreclosure_activated_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="recent_foreclosure_activated_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.Foreclosure.recent_foreclosure_activated_conditions()%></span>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Next Payment Due Date</th>
                <th>Demand Letter Date</th>
                <th>Papers To Attorney Date</th>
                <th>Posting Date</th>
                <th>Officer(s)</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in recent_foreclosure_activated)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                <td><%= foreclosure.loan.loan_type%></td>
                <td><%= foreclosure.loan.loan_name%></td>
                <td><%= foreclosure.loan.prin_bal.ToString("C")%></td>
                <td><%= foreclosure.loan.int_rate.ToString("0.000")%></td>
                <td><%= foreclosure.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= foreclosure.demand_letter_due_date.ToShortDateString()%></td>
                <td><%= foreclosure.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= foreclosure.posting_date.ToShortDateString()%></td>
                <td><%= foreclosure.foreclosure_officers%></td>
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
        <h2>Loans with Foreclosure Completed by Sale Date Completed. (<%= foreclosure_completed_by_sale_date_completed.Count%>)
        <a class="btn btn-inverse" onclick="var e = document.getElementById('foreclosure_completed_by_sale_date_completed_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="foreclosure_completed_by_sale_date_completed_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.Foreclosure.foreclosure_completed_by_sale_date_completed_conditions()%></span>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Next Payment Due Date</th>
                <th>Demand Letter Date</th>
                <th>Papers To Attorney Date</th>
                <th>Posting Date</th>
                <th>Officer(s)</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in foreclosure_completed_by_sale_date_completed)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                <td><%= foreclosure.loan.loan_type%></td>
                <td><%= foreclosure.loan.loan_name%></td>
                <td><%= foreclosure.loan.prin_bal.ToString("C")%></td>
                <td><%= foreclosure.loan.int_rate.ToString("0.000")%></td>
                <td><%= foreclosure.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= foreclosure.demand_letter_due_date.ToShortDateString()%></td>
                <td><%= foreclosure.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= foreclosure.posting_date.ToShortDateString()%></td>
                <td><%= foreclosure.foreclosure_officers%></td>
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
        <h2>Loans REMOVE from Foreclosure Activated by Last Regular Payment Received (<%= remove_from_recent_foreclosure_activated_by_payment_dt.Count%>)
        <a class="btn btn-inverse" onclick="var e = document.getElementById('remove_from_recent_foreclosure_activated_by_payment_dt_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="remove_from_recent_foreclosure_activated_by_payment_dt_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.Foreclosure.remove_from_recent_foreclosure_activated_by_payment_dt_conditions()%></span> 
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
                <th>Officer(s)</th>
                <th>Last Payment Received Date</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in remove_from_recent_foreclosure_activated_by_payment_dt)
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
                <td><%= foreclosure.foreclosure_officers%></td>
                <td><%= foreclosure.last_reg_paid_dt.ToShortDateString()%></td>
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
        <h2>Loans REMOVE from Foreclosure by Loss Mitigation Activity (<%= remove_from_foreclosure_by_lm_received.Count%>)</h2>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Interest Rate</th>
                <th>Demand Letter Date</th>
                <th>Papers To Attorney Date</th>
                <th>Posting Date</th>
                <th>Officer(s)</th>
                <th>Last Payment Received Date</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in remove_from_foreclosure_by_lm_received)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                <td><%= foreclosure.loan.loan_type%></td>
                <td><%= foreclosure.loan.loan_name%></td>
                <td><%= foreclosure.loan.int_rate.ToString("0.000")%></td>
                <td><%= foreclosure.demand_letter_due_date.ToShortDateString()%></td>
                <td><%= foreclosure.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= foreclosure.posting_date.ToShortDateString()%></td>
                <td><%= foreclosure.foreclosure_officers%></td>
                <td><%= foreclosure.last_reg_paid_dt.ToShortDateString()%></td>
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
        <h2>Loans REMOVE from Foreclosure by Payment Plan Activity  (<%= empty_demand_letters.Count%>)</h2>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Interest Rate</th>
                <th>Demand Letter Date</th>
                <th>Papers To Attorney Date</th>
                <th>Posting Date</th>
                <th>Officer(s)</th>
                <th>Last Payment Received Date</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in empty_demand_letters)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                <td><%= demand_letter.loan.loan_type%></td>
                <td><%= demand_letter.loan.loan_name%></td>
                <td><%= demand_letter.loan.int_rate.ToString("0.000") %></td>
                <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                <td><%= demand_letter.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= demand_letter.posting_date.ToShortDateString()%></td>
                <td><%= demand_letter.foreclosure_officers %></td>
                <td><%= demand_letter.last_reg_paid_dt.ToShortDateString()  %></td>
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
        <h2>Loans REMOVE from Foreclosure by Bankruptcy (<%= remove_from_foreclosure_by_bankruptcy.Count%>)
        <a class="btn btn-inverse" onclick="var e = document.getElementById('remove_from_foreclosure_by_bankruptcy_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="remove_from_foreclosure_by_bankruptcy_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.Foreclosure.remove_from_foreclosure_by_bankruptcy_conditions()%></span>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Interest Rate</th>
                <th>Demand Letter Date</th>
                <th>Papers To Attorney Date</th>
                <th>Posting Date</th>
                <th>Officer(s)</th>
                <th>Chapter Filed</th>
                <th>Bankruptcy Filed Date</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in remove_from_foreclosure_by_bankruptcy)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                <td><%= foreclosure.loan.loan_type%></td>
                <td><%= foreclosure.loan.loan_name%></td>
                <td><%= foreclosure.loan.int_rate.ToString("0.000")%></td>
                <td><%= foreclosure.demand_letter_due_date.ToShortDateString()%></td>
                <td><%= foreclosure.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= foreclosure.posting_date.ToShortDateString()%></td>
                <td><%= foreclosure.foreclosure_officers%></td>
                <td><%= foreclosure.chapter_filed%></td>
                <td><%= foreclosure.bankruptcy_filed_date.ToShortDateString()%></td>
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
        <h2>Loans REMOVE from Foreclosure by Sale (<%= remove_from_foreclosure_by_sale.Count%>)
        <a class="btn btn-inverse" onclick="var e = document.getElementById('remove_from_foreclosure_by_sale_date_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="remove_from_foreclosure_by_sale_date_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.Foreclosure.remove_from_foreclosure_by_sale_date_conditions()%></span>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Interest Rate</th>
                <th>Demand Letter Date</th>
                <th>Sale Date</th>
                <th>Officer(s)</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure demand_letter in remove_from_foreclosure_by_sale)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= demand_letter.loan_id.ToString()%>"><%= demand_letter.loan_id.ToString()%></a></td>
                <td><%= demand_letter.loan.loan_type%></td>
                <td><%= demand_letter.loan.loan_name%></td>
                <td><%= demand_letter.loan.int_rate.ToString("0.000") %></td>
                <td><%= demand_letter.demand_letter_due_date.ToShortDateString() %></td>
                <td><%= demand_letter.sale_date.ToShortDateString() %></td>
                <td><%= demand_letter.foreclosure_officers %></td>
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
        <h2>Loans REMOVE from Foreclosure by Deed In Lieu (<%= remove_from_foreclosure_by_deed_in_lieu.Count %>)
        <a class="btn btn-inverse" onclick="var e = document.getElementById('remove_from_foreclosure_by_deed_in_lieu_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="remove_from_foreclosure_by_deed_in_lieu_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.Foreclosure.remove_from_foreclosure_by_deed_in_lieu_conditions()%></span>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Interest Rate</th>
                <th>UPB</th>
                <th>Papers To Attorney Date</th>
                <th>Posting Date</th>
                <th>Officer(s)</th>
                <th>Next Payment Date</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.Foreclosure foreclosure in remove_from_foreclosure_by_deed_in_lieu)
               {   
            %>
            <tr>
                <td><a href="../ForeclosureLog/Default.aspx?loan_id=<%= foreclosure.loan_id.ToString()%>"><%= foreclosure.loan_id.ToString()%></a></td>
                <td><%= foreclosure.loan.loan_type%></td>
                <td><%= foreclosure.loan.loan_name%></td>
                <td><%= foreclosure.loan.int_rate.ToString("0.000")%></td>
                <td><%= foreclosure.loan.prin_bal.ToString("C")%></td>
                <td><%= foreclosure.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= foreclosure.posting_date.ToShortDateString()%></td>
                <td><%= foreclosure.foreclosure_officers%></td>
                <td><%= foreclosure.loan.due_date_next_payment.ToShortDateString()%></td>
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
