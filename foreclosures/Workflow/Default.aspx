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
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.workflow_list_nav_select(workflow)%>
    <!-- Example row of columns -->    
    <div class="row">
        <div class="span4">
            Loan Officer(s):&nbsp;&nbsp; 
            <asp:DropDownList ID="ddlLoanOfficer" runat="server" AutoPostBack="true"
                onselectedindexchanged="ddlLoanOfficer_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="span4">
            Loan Type:&nbsp;&nbsp; 
            <asp:DropDownList ID="ddlLoanType" runat="server" AutoPostBack="true"
                onselectedindexchanged="ddlLoanType_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="span4">
            Event Type:&nbsp;&nbsp; 
            <asp:DropDownList ID="ddlEventType" runat="server" AutoPostBack="true"
                onselectedindexchanged="ddlEventType_SelectedIndexChanged"></asp:DropDownList>
        </div>
    </div>
    
    <%if (this.workflow == "Demands")
      {
    %>
    <div class="row">
        <div class="span12">
        <h2><%= this.workflow%> (<%= bfs.Count%>)</h2>
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
                <%  int events_count = 0; if ((events_count = bf.getEvents().Count) > 0)
                    { %>
                    <td><a href="Events.aspx?loan_id=<%=bf.loan_id %>"><%= events_count.ToString()%></a></td>
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
                <td><span class="<%= bf.months_demand_date_after_delinquent_color() %>"><%= bf.months_demand_date_after_delinquent()%></span></td>
                <td><%= bf.foreclosure_officers%></td>
                <!--bf.default_reason_code -->
                <!--bf.all_occupancy_codes_descriptions()-->
                <td><%= bf.inspection.last_received_notify_date.ToShortDateString()%></td>
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
    <%
        }
    %>
    
    <%if (this.workflow == "Removed Demands")
      {
    %>
    <div class="row">
        <div class="span12">
        <h2><%= this.workflow%> (<%= bfs.Count%>)</h2>
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
                <th>Reason</th>
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.BaseForeclosure bf in bfs)
               {
            %>
            <tr class="<%= bf.getEventStatusColor() %>">
                <td><%= bf.loan_id.ToString()%></td>
                <%  int events_count = 0; if ((events_count = bf.getEvents().Count) > 0)
                    { %>
                    <td><a href="Events.aspx?loan_id=<%=bf.loan_id %>&workflow=<%=workflow %>"><%= events_count.ToString()%></a></td>
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
                <td><span class="<%= bf.months_demand_date_after_delinquent_color() %>"><%= bf.months_demand_date_after_delinquent()%></span></td>
                <td><%= bf.foreclosure_officers%></td>
                <!--bf.default_reason_code -->
                <!--bf.all_occupancy_codes_descriptions()-->
                <td><%= bf.inspection.last_received_notify_date.ToShortDateString()%></td>
                <!--bf.last_nl_demand_memo_date.ToString()-->
                <td><%= bf.filter_reason %>
                </td>
                
             </tr>
            <%
               }
            %>
        </table>
        </div>
    </div>    
    <%
        }
    %>
    
    <%if (this.workflow == "PreForeclosures")
      {
    %>
    <div class="row">
        <div class="span12">
        <h2><%= this.workflow%> (<%= bfs.Count%>)</h2>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Exceptions</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Papers To Attorney Date</th>
                <th>Posting Date</th>
                <th>Foreclosure Date</th>
                <th>Officer(s)</th>
                <th>Checklist</th>
            </tr>
            <% 
               foreach (com.sp.rmmc.foreclosures.models.BaseForeclosure bf in bfs)
               {
            %>
            <tr class="<%= bf.getEventStatusColor() %>">
                <td><%= bf.loan_id.ToString()%></td>
                <%  int events_count = 0; if ((events_count = bf.getEvents().Count) > 0)
                    { %>
                    <td><a href="Events.aspx?loan_id=<%=bf.loan_id %>&workflow=<%=workflow %>"><%= events_count.ToString()%></a></td>
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
                <td><%= bf.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= bf.posting_date.ToShortDateString()%></td>
                <td><span class="text-error"><%= bf.foreclosure_date.ToShortDateString()%></span></td>
                <td><%= bf.foreclosure_officers%></td>
                <td><a href="PreforeclosureEvents.aspx?loan_id=<%=bf.loan_id.ToString() %>">Checklist</a></td>
             </tr>
            <%
               }
            %>
        </table>
        </div>
    </div>    
    <%
        }
    %>
    
    <%if (this.workflow == "Foreclosures")
      {
    %>
    <div class="row">
        <div class="span12">
        <h2><%= this.workflow%> (<%= bfs.Count%>)</h2>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Exceptions</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Papers To Attorney Date</th>
                <th>Posting Date</th>
                <th>Foreclosure Date</th>
                <th>Officer(s)</th>
                <th>Checklist</th>
            </tr>
            <% 
               foreach (com.sp.rmmc.foreclosures.models.BaseForeclosure bf in bfs)
               {
            %>
            <tr class="<%= bf.getEventStatusColor() %>">
                <td><%= bf.loan_id.ToString()%></td>
                <%  int events_count = 0; if ((events_count = bf.getEvents().Count) > 0)
                    { %>
                    <td><a href="Events.aspx?loan_id=<%=bf.loan_id %>&workflow=<%=workflow %>"><%= events_count.ToString()%></a></td>
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
                <td><%= bf.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= bf.posting_date.ToShortDateString()%></td>
                <td><span class="text-error"><%= bf.foreclosure_date.ToShortDateString()%></span></td>
                <td><%= bf.foreclosure_officers%></td>
                <td><a href="PreforeclosureEvents.aspx?loan_id=<%=bf.loan_id.ToString() %>">Checklist</a></td>
             </tr>
            <%
               }
            %>
        </table>
        </div>
    </div>    
    <%
        }
    %>

    
    <%if (this.workflow == "Foreclosed")
      {
    %>
    <div class="row">
        <div class="span12">
        <h2><%= this.workflow%> (<%= bfs.Count%>)</h2>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Exceptions</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Papers To Attorney Date</th>
                <th>Posting Date</th>
                <th>Sale Date</th>
                <th>Officer(s)</th>
                <th>Principal Paid</th>
                <!-- 
                <th>Reason Code</th>
                <th>Occupancy Code</th>
                -->
                <!-- 
                <th>Letters Sent</th>                
                -->
            </tr>
            <% 
               int month = 0;
               foreach (com.sp.rmmc.foreclosures.models.BaseForeclosure bf in bfs)
               {
                   if (bf.sale_date.date.Month != month)
                   {
                       month = bf.sale_date.date.Month;
            %>
            <tr class="info"><td colspan="12"><%= bf.sale_date.date.ToString("MMMM yyyy") %></td></tr>
            <%
                   }
            %>
            <tr class="<%= bf.getEventStatusColor() %>">
                <td><%= bf.loan_id.ToString()%></td>
                <%  int events_count = 0; if ((events_count = bf.getEvents().Count) > 0)
                    { %>
                    <td><a href="Events.aspx?loan_id=<%=bf.loan_id %>&workflow=<%=workflow %>"><%= events_count.ToString()%></a></td>
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
                <td><%= bf.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= bf.posting_date.ToShortDateString()%></td>
                <td><span class="text-error"><%= bf.sale_date.ToShortDateString()%></span></td>
                <td><%= bf.foreclosure_officers%></td>
                <td><%= bf.non_zero_balance().ToString("C")%></td>
                <!--bf.default_reason_code -->
                <!--bf.all_occupancy_codes_descriptions()-->
                <!--bf.last_nl_demand_memo_date.ToString()-->
                
             </tr>
            <%
               }
            %>
        </table>
        </div>
    </div>    
    <%
        }
    %>

    </div>
    </div>
    </form>
</body>
</html>
