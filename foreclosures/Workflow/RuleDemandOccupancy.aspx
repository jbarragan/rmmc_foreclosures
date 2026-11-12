<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RuleDemandOccupancy.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - Process - Business Rules - Demand Occupancy Rules</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosure Process - Business Rules</h1>
        <h2>Demand Occupancy Rules</h2>
        <p></p>
    </div>
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Workflow", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.workflow_list_nav_select("Process", version)%>
    <div class="row">
        <div class="span12">
            <h2>Demand Occupancy Rules</h2>
            Validates that Demand Occupancy is current and correct. 
            <br />
            Sections:
            <ol>
            <li><a href="#diagram">Diagram</a></li>
            <li><a href="#sources">Data Sources</a></li>
            <li><a href="#loans">Loans that trigger this Rule</a></li>
            </ol>
            <br />

            <a id="diagram"></a><b>Diagram</b><br />
            <img src="images/demand_occupancy_alert.png" /><br />
            
            <br /><br /><a id="sources"></a><b>Data Sources</b><br />
            <br /><u>FHA Occupancy Date</u><br /><br />
            <img src="images/fha_occupancy_date.png" /><br />
            <i>DB: (select top 1 ms_loan_occupancy.fcl_hud_notice_of_occ_sent from ms_loan_occupancy where ms_loan_occupancy.loan_id = loan_id_column order by ms_loan_occupancy.fcl_hud_notice_of_occ_sent desc) as demand_occupancy_events_occupancy_fcl_hud_notice_of_occ_sent_date</i><br />
            
            <br /><u>Last PI Memo_Date</u><br /><br />
            <img src="images/last_pi_memo_date.png" /><br />
            <i>DB: (select top 1 memo.memo_notify_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and (ms_memo_types.memo_type_desc like 'PI-%') order by memo_notify_dt desc ) as demand_occupancy_events_last_pi_memo_date</i><br />

            

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
