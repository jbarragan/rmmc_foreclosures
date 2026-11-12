<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RuleHomeInsurance.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - Process - Business Rules - Home Insurance Rules</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosure Process - Business Rules</h1>
        <h2>Home Insurance Rules</h2>
        <p></p>
    </div>
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Workflow", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.workflow_list_nav_select("Process", version)%>
    <div class="row">
        <div class="span12">
            <h2>Home Insurance Business Rules</h2>
            Determines if Home Insurance is current for Pre-Foreclosures and Foreclosures without any sale or cancellation event.
            <br />
            Sections:
            <ol>
            <li><a href="#diagram">Diagram</a></li>
            <li><a href="#sources">Data Sources</a></li>
            <li><a href="#loans">Loans that trigger this Rule</a></li>
            </ol>
            <br />

            <a id="diagram"></a><b>Diagram</b><br />
            <img src="images/home_inspection_events.png" /><br />
            
            <br /><br /><a id="sources"></a><b>Data Sources</b><br />
            <br /><u>Third Party Sale Date</u><br /><br />
            <img src="images/third_party_sale_date.png" /><br />
            <i>DB: (select top 1 memo.memo_create_dt  from ms_loan_memo memo join ms_memo_types on memo.memo_type_id = ms_memo_types.memo_type_id where memo.loan_id = loan_id_column and ms_memo_types.memo_type_desc like 'FC-3rd%' order by memo.memo_create_dt desc ) as foreclosure_home_insurance_events_third_party_sale_date</i><br />
            
            

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
