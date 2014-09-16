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
        <h2>Active Foreclosures (<%= recent_foreclosure_activated_all.Count%>)
        <a class="btn btn-primary" onclick="var e = document.getElementById('recent_foreclosure_activated_table'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a>
        <a class="btn btn-inverse" onclick="var e = document.getElementById('recent_foreclosure_activated_conditions'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-info-sign icon-white"></i> Conditions</a></h2>
        <span id="recent_foreclosure_activated_conditions" style="display:none"><%=com.sp.rmmc.foreclosures.models.Foreclosure.recent_foreclosure_activated_conditions()%></span>
        <div id="recent_foreclosure_activated_table" style="display:none">
        <h3>FHA (<%= recent_foreclosure_activated_fha.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('recent_foreclosure_activated_fha'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="recent_foreclosure_activated_fha" style="display:none">
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
                <td><%= foreclosure.demand_letter_due_date.ToShortDateString()%></td>
                <td><%= foreclosure.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= foreclosure.posting_date.ToShortDateString()%></td>
                <td><%= foreclosure.foreclosure_officers%></td>
                <td><a href="PreforeclosureEvents.aspx?loan_id=<%=foreclosure.loan_id.ToString() %>">Checklist</a></td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        <h3>VA (<%= recent_foreclosure_activated_va.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('recent_foreclosure_activated_va'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="recent_foreclosure_activated_va" style="display:none">
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
                <td><%= foreclosure.demand_letter_due_date.ToShortDateString()%></td>
                <td><%= foreclosure.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= foreclosure.posting_date.ToShortDateString()%></td>
                <td><%= foreclosure.foreclosure_officers%></td>
                <td><a href="PreforeclosureEvents.aspx?loan_id=<%=foreclosure.loan_id.ToString() %>">Checklist</a></td>
             </tr>
            <%
                }
            %>
        </table>
        </div>
        <h3>CONV (<%= recent_foreclosure_activated_conv.Count%>)<a class="btn btn-primary" onclick="var e = document.getElementById('recent_foreclosure_activated_conv'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></h3>
        <div id="recent_foreclosure_activated_conv" style="display:none">
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
                <td><%= foreclosure.demand_letter_due_date.ToShortDateString()%></td>
                <td><%= foreclosure.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= foreclosure.posting_date.ToShortDateString()%></td>
                <td><%= foreclosure.foreclosure_officers%></td>
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
