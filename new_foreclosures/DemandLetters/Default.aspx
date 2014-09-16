<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Foreclosures: Demand Letter.</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
    <script type="text/jscript" src="../../assets/js/jquery.js" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosures</h1>
        <p>Loans in Demand Letter.</p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.sub_main_nav %>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.task_list_nav %>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h2>Loans in Expired Demand Letter Queue. (<%= in_demand_letter_expired_mode.Count %>)
        <a class="btn btn-inverse" onclick="var e = document.getElementById('in_demand_letter_expired_mode_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="in_demand_letter_expired_mode_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.DemandLetter.in_demand_letter_expired_mode_conditions() %></span>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Demand Letter Due Date</th>
                <th>Officer(s)</th>
                <th>Review Checklist</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in in_demand_letter_expired_mode)
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
                <td><%= demand_letter.foreclosure_officers%></td>
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
    <br />
    <br />
    <div class="row">
        <div class="span12">
        <h2>Loans in Demand Letter Queue. (<%= in_pre_demand_letter_mode.Count %>)
        <a class="btn btn-inverse" onclick="var e = document.getElementById('in_pre_demand_letter_mode_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="in_pre_demand_letter_mode_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.DemandLetter.in_pre_demand_letter_mode_conditions()%></span>
        <div class="row">
          <div class="span2"></div>
          <div class="span10">
            Definitions:<br />
              <span class="text-error">5 days or less before Demand Letter Expiration Date</span><br />
              <span class="text-warning">10 days or less before Demand Letter Expiration Date</span>
          </div>
        </div>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Demand Letter Due Date</th>
                <th>Officer(s)</th>
                <th>Review Checklist</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in in_pre_demand_letter_mode)
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
                <td><%= demand_letter.foreclosure_officers%></td>
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
    <br />
    <br />
    <div class="row">
        <div class="span12">
        <h2>Loans REMOVE from demand letter by Last Regular Payment Received (<%= remove_demand_letter_by_last_reg_payment.Count%>)
        <a class="btn btn-inverse" onclick="var e = document.getElementById('remove_demand_letter_by_last_reg_payment_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="remove_demand_letter_by_last_reg_payment_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.DemandLetter.remove_demand_letter_by_last_reg_payment_conditions()%></span>
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
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in remove_demand_letter_by_last_reg_payment)
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
    <br />
    <br />
    <div class="row">
        <div class="span12">
        <h2>Loans REMOVE from demand letter by Foreclosure Activated (<%= remove_demand_letter_by_foreclosure_active.Count%>)
        <a class="btn btn-inverse" onclick="var e = document.getElementById('remove_demand_letter_by_foreclosure_active_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="remove_demand_letter_by_foreclosure_active_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.DemandLetter.remove_demand_letter_by_foreclosure_active_conditions()%></span>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Demand Letter Due Date</th>
                <th>Officer(s)</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in remove_demand_letter_by_foreclosure_active)
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
    <br />
    <br />
    <div class="row">
        <div class="span12">
        <h2>Loans REMOVE from demand letter by Loss Mitigation Activity (<%= remove_lm_activity.Count%>)</h2>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Demand Letter Due Date</th>
                <th>Officer(s)</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in remove_lm_activity)
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
    <br />
    <br />
    <div class="row">
        <div class="span12">
        <h2>Loans REMOVE from demand letter by Payment Plan Activity (<%= empty_demand_letters.Count%>)</h2>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Demand Letter Due Date</th>
                <th>Officer(s)</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in empty_demand_letters)
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
    <br />
    <br />
    <div class="row">
        <div class="span12">
        <h2>Loans REMOVE from demand letter by Bankruptcy Activity (<%= remove_from_demand_letter_by_bankruptcy.Count%>)
        <a class="btn btn-inverse" onclick="var e = document.getElementById('remove_from_demand_letter_by_bankruptcy_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="remove_from_demand_letter_by_bankruptcy_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.DemandLetter.remove_from_demand_letter_by_bankruptcy_conditions()%></span>
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
            <% foreach (com.sp.rmmc.foreclosures.models.DemandLetter demand_letter in remove_from_demand_letter_by_bankruptcy)
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
