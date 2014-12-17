<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - <%= workflow %></title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1><%= workflow %></h1>
        <p></p>
    </div>    
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Workflow", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.workflow_list_nav_select("Demands")%>
    <!-- Example row of columns -->    
    <div class="row">
        <div class="span6">
            Loan Officer(s):&nbsp;&nbsp; 
            <asp:DropDownList ID="ddlLoanOfficer" runat="server" AutoPostBack="true"
                onselectedindexchanged="ddlLoanOfficer_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="span6">
            Loan Type:&nbsp;&nbsp; 
            <asp:DropDownList ID="ddlLoanType" runat="server" AutoPostBack="true"
                onselectedindexchanged="ddlLoanType_SelectedIndexChanged"></asp:DropDownList>
        </div>
    </div>
    
    <div class="row">
        <div class="span12">
        <h2><%= this.workflow %> (<%= bfs.Count %>)</h2>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Exceptions</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Demand Letter Due Date</th>
                <th>Months Demand after Delinquent</th>
                <th>Officer(s)</th>
                <!-- 
                <th>Reason Code</th>
                <th>Occupancy Code</th>
                -->
                <th>Last Inspection Received</th>                
                <!-- 
                <th>Letters Sent</th>                
                -->
                <th>Review Checklist</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.BaseForeclosure bf in bfs)
               {
            %>
            <tr class="<%= bf.getEventStatusColor() %>">
                <td><%= bf.loan_id.ToString()%></td>
                <%  int events_count = 0; if ( (events_count = bf.getEvents().Count) > 0)
                    { %>
                    <td><a href="Events.aspx?loan_id=<%=bf.loan_id %>"><%= events_count.ToString() %></a></td>
                <%  }
                    else
                    { %>
                    <td>0</td>
                <%  } %>
                <td><%= bf.loan.loan_type%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= bf.loan.prin_bal.ToString("C")%></td>
                <td><%= bf.loan.int_rate.ToString("0.000")%></td>
                <td><%= bf.demand_letter_due_date.ToShortDateString()%></td>
                <td><span class="<%= bf.months_demand_date_after_delinquent_color() %>"><%= bf.months_demand_date_after_delinquent() %></span></td>
                <td><%= bf.foreclosure_officers%></td>
                <!--bf.default_reason_code -->
                <!--bf.all_occupancy_codes_descriptions()-->
                <td><%= bf.inspection.last_received_notify_date.ToShortDateString() %></td>
                <!--bf.last_nl_demand_memo_date.ToString()-->
                <td>
                    <a href="../ForeclosureReviewChecklist/Default.aspx?loan_id=<%=bf.loan_id.ToString() %>">Checklist</a>
                    <span class="text-success"><%= (Array.IndexOf(array_completed_checklists_loan_ids, bf.loan_id) >= 0) ? "Completed" : ""%></span>
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
    </form>
</body>
</html>
