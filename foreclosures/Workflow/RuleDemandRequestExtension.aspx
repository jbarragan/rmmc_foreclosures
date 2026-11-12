<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RuleDemandRequestExtension.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - Process - Business Rules - Demand Request Extension Rules</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosure Process - Business Rules</h1>
        <h2>Demand Request Extension Rules</h2>
        <p></p>
    </div>
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Workflow", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.workflow_list_nav_select("Process", version)%>
    <div class="row">
        <div class="span12">
            <h2>Demand Request Extension Rules</h2>
            Check that Demand Extension has been ordered if Foreclosure Date in more than 30 days and Extension Date not ordered for FHA loans. 
            <br />
            Sections:
            <ol>
            <li><a href="#diagram">Diagram</a></li>
            <li><a href="#sources">Data Sources</a></li>
            <li><a href="#loans">Loans that trigger this Rule</a></li>
            </ol>
            <br />

            <a id="diagram"></a><b>Diagram</b><br />
            <img src="images/demand_request_for_extension.png" /><br />
            
            <br /><br /><a id="sources"></a><b>Data Sources</b><br />
            <br /><u>Foreclosure Date</u><br /><br />
            <img src="images/foreclosure_date.png" /><br />
            <i>DB: (select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_recvd_expd ) as demand_request_for_extension_events_foreclosure_date</i><br />
            
            <br /><u>Extension Ordered Date</u><br /><br />
            <img src="images/extension_ordered_date.png" /><br />
            <i>DB: (select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10800 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_ord_req ) as demand_request_for_extension_events_extension_ordered_date</i><br />

            

            <br /><br /><a id="loans"></a><b>Loans that trigger this Rule(<%= events.Count%>)</b><br /><br />
                <div class="row">
                    <div class="span12">
                        <table class="table table-striped">
                            <tr>
                                <th>Loan#</th>
                                <th>Event Name</th>
                                <th>Event Type</th>
                                <th>Event Description</th>
                            </tr>
                            <% foreach (com.sp.rmmc.common.models.events.Event e in events)
                               {
                            %>
                            <tr>
                                <td><%= e.loan.loan_id.ToString()%></td>
                                <td><%= e.name %></td>
                                <td><%= (e.type == 0 ? "Warning" : "Alert") %></td>
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
    </div>
    </form>
</body>
</html>
