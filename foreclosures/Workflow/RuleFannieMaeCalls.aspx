<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RuleFannieMaeCalls.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - Process - Business Rules - Fannie Mae Calls Business Rules</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosure Process - Business Rules</h1>
        <h2>Fannie Mae Calls Business Rules</h2>
        <p></p>
    </div>
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Workflow", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.workflow_list_nav_select("Process", version)%>
    <div class="row">
        <div class="span12">
            <h2>Fannie Mae Call Business Rules</h2>
            Determines if Fannie Mae Calls memos are current and correct.
            <br />
            Sections:
            <ol>
            <li><a href="#diagram">Diagram</a></li>
            <li><a href="#sources">Data Sources</a></li>
            <li><a href="#loans">Loans that trigger this Rule</a></li>
            </ol>
            <br />

            <a id="diagram"></a><b>Diagram</b><br />
            <img src="images/fannie_mae_loan_last_call_warning.png" /><br />
            
            <br /><br /><a id="sources"></a><b>Data Sources</b><br />
            <br /><u>Last BC Call Memo Date</u><br /><br />
            <img src="images/last_bc_call_memo_date.png" /><br />
            <i>DB: (select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'BC%' order by memo_id desc ) as fannie_mae_calls_events_last_bc_call_memo_date</i><br />
            <br /><u>Last BC Promised To Pay Memo Date</u><br /><br />
            <img src="images/last_bc_promised_to_pay_date.png" /><br />
            <i>DB: (select top 1 memo.memo_notify_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc = 'BC-Promised to Pay' order by memo_notify_dt desc ) as fannie_mae_calls_events_last_bc_promised_to_pay_date</i><br />

            

            <br /><br /><a id="loans"></a><b>Loans that trigger this Rule(<%= events.Count%>)</b><br /><br />
                <div class="row">
                    <div class="span12">
                        <table class="table table-striped">
                            <tr>
                                <th>Loan#</th>
                                <th>Event Name</th>
                                <th>Stage</th>
                                <th>Event Type</th>
                                <th>Event Description</th>
                            </tr>
                            <% foreach (com.sp.rmmc.common.models.events.Event e in events)
                               {
                            %>
                            <tr>
                                <td><%= e.loan.loan_id.ToString()%></td>
                                <td><%= e.name %></td>
                                <td><%= e.parent_type %></td>
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
