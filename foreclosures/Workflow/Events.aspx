<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Events.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - Events</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Events</h1>
        <p></p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Workflow", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.workflow_list_nav_select("PreforeclosureEvents")%>
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
            Loan Number:
            <div class="input-append">
                <asp:TextBox ID="txtLoanId" runat="server" Width="100px"></asp:TextBox>  
                <asp:Button ID="btnSearch" 
                    runat="server"
                    Text="Find"
                    CssClass="btn btn-primary"
                    autoPostBack="true" onclick="btnSearch_Click"/><br />
            </div>
            <p class="text-error"><asp:Label ID="lblError" runat="server" Text="" ></asp:Label></p>        
        </div>
    </div>
    
    <div class="row">
        <div class="span12">
        <h2>Events (<%= events.Count %>)</h2>
        <table class="table table-striped">
            <tr>
                <th>Loan#</th>
                <!--<th>Officer(s)</th>-->
                <th>Event Name</th>
                <th>Event Type</th>
                <th>Event Description</th>
            </tr>
            <% foreach (com.sp.rmmc.common.models.events.Event e in events)
               {
            %>
            <tr class="<%= e.getEventStatusColor() %>">
                <td><%= e.loan.loan_id.ToString()%></td>
                <!-- Foreclosure Officer<td></td> -->
                <td><%= e.name %></td>
                <td><%= e.type %></td>
                <td><%= e.description %></td>                
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
