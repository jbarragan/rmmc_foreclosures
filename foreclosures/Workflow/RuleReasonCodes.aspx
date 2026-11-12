<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RuleReasonCodes.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - Process - Business Rules - Reason Codes Rules</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosure Process - Business Rules</h1>
        <h2>Reason Codes Rules</h2>
        <p></p>
    </div>
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Workflow", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.workflow_list_nav_select("Process", version)%>
    <div class="row">
        <div class="span12">
            <h2>Reason Codes Rules</h2>
            Determines if Default Reason Code and Last Memo Reason Code are equal and current.
            <br />
            Sections:
            <ol>
            <li><a href="#diagram">Diagram</a></li>
            <li><a href="#sources">Data Sources</a></li>
            <li><a href="#loans">Loans that trigger this Rule</a></li>
            </ol>
            <br />

            <a id="diagram"></a><b>Diagram</b><br />
            <img src="images/different_reason_codes.png" /><br />
            
            <br /><br /><a id="sources"></a><b>Data Sources</b><br />
            <br /><u>Default Reason Code</u><br /><br />
            <img src="images/default_reason_code.png" /><br />
            <i>DB: (select top 1 ms_credit_information.default_reason_code from ms_credit_information where ms_credit_information.loan_id = loan_id_column order by ms_credit_information.default_reason_code ) as reason_code_events_reason_code</i><br />
            <br /><u>Last Memo Reason Code</u><br /><br />
            <img src="images/last_memo_reason_code.png" /><br />
            <i>DB: (select top 1 ms_memo_categories.memo_category_desc from ms_loan_memo memo, ms_memo_categories, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_category_id = ms_memo_categories.memo_category_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_categories.memo_category_desc like '0%' and ms_memo_categories.memo_category_desc not like '031%' and (ms_memo_types.memo_type_desc = 'BC-Promised to Pay' or ms_memo_types.memo_type_desc = 'BC-First Right Party Contact')  order by memo_create_dt desc ) as reason_code_events_last_memo_reason_code</i><br />
            <br /><u>Last Memo Reason Code Date</u><br /><br />
            <img src="images/last_memo_reason_code_date.png" /><br />
            <i>DB: (select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_categories, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_category_id = ms_memo_categories.memo_category_id and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_categories.memo_category_desc like '0%' and ms_memo_categories.memo_category_desc not like '031%' and (ms_memo_types.memo_type_desc = 'BC-Promised to Pay' or ms_memo_types.memo_type_desc = 'BC-First Right Party Contact')  order by memo_create_dt desc ) as reason_code_events_memo_create_dt</i><br />

            

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
