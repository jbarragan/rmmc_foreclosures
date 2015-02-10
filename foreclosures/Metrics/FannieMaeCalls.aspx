<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FannieMaeCalls.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - Fannie Mae Calls</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Fannie Mae Calls </h1>
        <h2><%= (this.history == null) ?  "Current" : "History: " + history.history_text %></h2>
        <p></p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Metrics", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.metrics_list_nav_select("Fannie Mae Calls")%>
    <!-- Example row of columns -->    
    <div class="row">
        <div class="span4">
            Loan Officer(s):&nbsp;&nbsp; 
            <asp:DropDownList ID="ddlLoanOfficer" runat="server" AutoPostBack="true"
                onselectedindexchanged="ddlLoanOfficer_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="span4" style="display:none">
            Loan Type:&nbsp;&nbsp; 
            <asp:DropDownList ID="ddlLoanType" runat="server" AutoPostBack="true"
                onselectedindexchanged="ddlLoanType_SelectedIndexChanged"></asp:DropDownList>
        </div>
    </div>
    
    <%if (this.workflow == "Demands" || true)
      {
    %>
    <div class="row">
        <div class="span12">
        <h2>Demands</h2>
        <table class="table table-striped">
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
                <!-- 
                <th>Reason Code</th>
                <th>Occupancy Code</th>
                -->
                <th>Last Inspection Received</th>                
                <!-- 
                <th>Letters Sent</th>                
                -->
            </tr>
            <% foreach (com.sp.rmmc.foreclosures.models.BaseForeclosure bf in bfs)
               {
                   if (bf.type != com.sp.rmmc.foreclosures.models.BaseForeclosure.DEMAND_TYPE) continue;
            %>
            <tr >
                <td><%= bf.loan_id.ToString()%></td>
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
                
             </tr>
             <tr><td colspan="10"><%=( ( bf.fannie_mae_calls_events != null && bf.fannie_mae_calls_events.events.Count > 0 ) ? bf.fannie_mae_calls_events.events[0].description : "") %></td></tr>
            <%
               }
            %>
        </table>
        </div>
    </div>    
    <%
        }
    %>
    
    <%if (this.workflow == "PreForeclosures" || true)
      {
    %>
    <div class="row">
        <div class="span12">
        <h2>PreForeclosures</h2>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Papers To Attorney Date</th>
                <th>Posting Date</th>
                <th>Foreclosure Date</th>
                <th>Officer(s)</th>
            </tr>
            <% 
               foreach (com.sp.rmmc.foreclosures.models.BaseForeclosure bf in bfs)
               {
                   if (bf.type != com.sp.rmmc.foreclosures.models.BaseForeclosure.PREFORECLOSURE_TYPE) continue;
            %>
            <tr>
                <td><%= bf.loan_id.ToString()%></td>                
                <td><%= bf.loan.loan_type%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= bf.loan.prin_bal.ToString("C")%></td>
                <td><%= bf.loan.int_rate.ToString("0.000")%></td>
                <td><%= bf.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= bf.posting_date.ToShortDateString()%></td>
                <td><span class="text-error"><%= bf.foreclosure_date.ToShortDateString()%></span></td>
                <td><%= bf.foreclosure_officers%></td>
             </tr>
             <tr><td colspan="10"><%=( ( bf.fannie_mae_calls_events != null && bf.fannie_mae_calls_events.events.Count > 0 ) ? bf.fannie_mae_calls_events.events[0].description : "") %></td></tr>
            <%
               }
            %>
        </table>
        </div>
    </div>    
    <%
        }
    %>
    
    <%if (this.workflow == "Foreclosures" || true)
      {
    %>
    <div class="row">
        <div class="span12">
        <h2>Foreclosures</h2>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <th>Loan Type</th>
                <th>Loan Name</th>
                <th>Due Date of next payment</th>
                <th>UPB</th>
                <th>Interest Rate</th>
                <th>Papers To Attorney Date</th>
                <th>Posting Date</th>
                <th>Foreclosure Date</th>
                <th>Officer(s)</th>
            </tr>
            <% 
               foreach (com.sp.rmmc.foreclosures.models.BaseForeclosure bf in bfs)
               {
                   if (bf.type != com.sp.rmmc.foreclosures.models.BaseForeclosure.FORECLOSURE_TYPE) continue;
            %>
            <tr>
                <td><%= bf.loan_id.ToString()%></td>
                <td><%= bf.loan.loan_type%></td>
                <td><%= bf.loan.loan_name%></td>
                <td><%= bf.loan.due_date_next_payment.ToShortDateString()%></td>
                <td><%= bf.loan.prin_bal.ToString("C")%></td>
                <td><%= bf.loan.int_rate.ToString("0.000")%></td>
                <td><%= bf.papers_to_attorney_date.ToShortDateString()%></td>
                <td><%= bf.posting_date.ToShortDateString()%></td>
                <td><span class="text-error"><%= bf.foreclosure_date.ToShortDateString()%></span></td>
                <td><%= bf.foreclosure_officers%></td>
             </tr>
             <tr><td colspan="10"><%=( ( bf.fannie_mae_calls_events != null && bf.fannie_mae_calls_events.events.Count > 0 ) ? bf.fannie_mae_calls_events.events[0].description : "") %></td></tr>
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
