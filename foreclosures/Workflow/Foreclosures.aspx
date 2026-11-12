<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Foreclosures.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - Process - Foreclosures</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosure Process - Foreclosures</h1>
        <h2>Loans in Foreclosure stage.</h2>
        <p></p>
    </div>
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Workflow", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.workflow_list_nav_select("Process", version)%>
    <div class="row">
        <div class="span12">
            <h2>Foreclosures</h2>
            Foreclosure is the third stage in the foreclosure process.
            <br />
            Sections:
            <ol>
            <li><a href="#diagram">Diagram</a></li>
            <li><a href="#sources">Data Sources</a></li>
            <li><a href="#rules">Business Rules/Events</a></li>
            <li><a href="#loans">Loans in Foreclosure Stage</a></li>
            </ol>
            <br />

            <a id="diagram"></a><b>Diagram</b><br />
            <img src="images/foreclosures.png" /><br />
            
            <br /><br /><a id="sources"></a><b>Data Sources</b><br />
            <br /><u>Demand Letter Due Date</u><br /><br />
            <img src="images/demand_letter_due_date.png" /><br />
            <i>DB: (select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10020 and misc.loan_id = base.loan_id order by misc.misc_dt_value ) as demand_letter_due_date</i><br />
            
            <br /><u>FC Foreclosure Stopped Memo</u><br /><br />
            <img src="images/fc_foreclosure_stopped_memo.png" /><br />
            <i>DB: (select top 1 memo.memo_notify_dt from ms_loan_memo memo join ms_memo_types on memo.memo_type_id = ms_memo_types.memo_type_id where memo.loan_id = base.loan_id and ms_memo_types.memo_type_desc like 'FC-Foreclosure Stopped%' order by memo.memo_notify_dt desc )</i><br />

            <br /><u>Papers Send To Attorney Date</u><br /><br />
            <img src="images/papers_to_attorney_date.png" /><br />
            <i>DB: (select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1000 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_date ) as foreclosure_mortgage_status_events_papers_to_attorney_date</i><br />
            
            <br /><u>FCL Type</u><br /><br />
            <img src="images/fcl_type.png" /><br />
            <i>DB: (select top 1 ms_fcl_info.fcl_type from ms_fcl_info where ms_fcl_info.loan_id = base.loan_id order by ms_fcl_info.fcl_type) as fcl_type</i><br />
            
            <br /><u>Sale Date</u><br /><br />
            <img src="images/sale_date.png" /><br />
            <i>DB: (select top 1 mfia1.fcl_item_date from ms_fcl_item_amts_1 mfia1 where mfia1.item_id = 1600 and mfia1.loan_id = base.loan_id order by mfia1.fcl_item_date ) as sale_date</i><br />
            
            <br /><u>PreForeclosure Conditions</u><br /><br />
            <img src="images/preforeclosure_checklist.png" /><br />
            <i>Visit Checklist link on preforeclosure list below.</i><br />

            <br /><br /><a id="rules"></a><b>Business Rules</b><br />
            <ol>
            <li><a href="RuleAppraisalBPO.aspx">Foreclosure Appraisal/BPO Report Rules</a></li>
            <li><a href="RuleVAAppraisalNOV.aspx">Foreclosure VA Appraisal NOV Rules</a></li>
            <li><a href="RuleDemandRequestExtension.aspx">Demand Request Extension Rules</a></li>
            <li><a href="RuleMortgageStatus.aspx">Foreclosure Mortgage Status Rules (Not 42 if FHA, Mortgage Status with Papers Send to Attorney, Mortgage Status with Posting Date)</a></li>
            <li><a href="RuleFannieMaeCalls.aspx">Fannie Mae Calls Rules</a></li>
            <li><a href="RuleValeriSale.aspx">Sale Valeri Rules</a></li>
            <li><a href="RuleHomeInsurance.aspx">Foreclosure Home Insurance Rules</a></li>
            <li>PreForeclosure FNMA Review Rules</li>
            <li>Send Papers to Attorney Rules</li>
            <li>Foreclosure Proceeding Started Rules</li>
            <li>Probate Rules</li>
            <li><a href="RuleInspections.aspx">Inspections (Not Ordered, Expired, Not Received) Rules</a></li>
            <li><a href="RuleReasonCodes.aspx">Different Reason Code Rules</a></li>
            </ol>

            <br /><br /><a id="loans"></a><b>Loans in Foreclosure Stage (<%= bfs.Count%>)</b><br /><br />
                <div class="row">
                    <div class="span12">
                    <table class="table table-striped">
                        <tr>
                            <th>Loan#</th>
                            <th>Exceptions</th>
                            <th>Loan Type</th>
                            <th>Loan Name</th>
                            <th>Due Date of next payment</th>
                            <th>UPB</th>
                            <th>Interest Rate</th>
                            <th>Occupancy</th>
                            <th>Papers To Attorney Date</th>
                            <th>Posting Date</th>
                            <th>Foreclosure Date</th>
                            <th>Officer(s)</th>
                            <th>Checklist</th>
                            <th>GL1806 Balance</th>
                        </tr>
                        <% 
                           foreach (com.sp.rmmc.foreclosures.models.BaseForeclosure bf in bfs)
                           {
                        %>
                        <!--<tr class="<%= bf.getEventStatusColor() %>">-->
                        <tr>
                            <td><%= bf.loan_id.ToString()%></td>
                            <%  int events_count = 0; if ((events_count = bf.getEvents().Count) > 0)
                                { %>
                                <td><a href="Events.aspx?loan_id=<%=bf.loan_id %>&workflow=<%=workflow %>&version=<%=version %>"><%= events_count.ToString()%></a></td>
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
                            <td><%= bf.loan.occupancy_code %></td>
                            <td><%= bf.papers_to_attorney_date.ToShortDateString()%></td>
                            <td><%= bf.posting_date.ToShortDateString()%></td>
                            <td><span class="text-error"><%= bf.foreclosure_date.ToShortDateString()%></span></td>
                            <td><%= bf.foreclosure_officers%></td>
                            <td><a href="PreforeclosureEvents.aspx?loan_id=<%=bf.loan_id.ToString() %>&version=<%=version %>">Checklist</a></td>
                            <td>
                                <% if( bf.gl1806_details != null )
                                   { 
                                %>
                                    <a href="../Management/Gl1806Details.aspx?loan_id=<%=bf.loan_id.ToString() %>"><%= bf.gl1806_details.amount.ToString("C") %></a>
                                <% }else{ %>
                                    NA
                                <% } %>
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
    </div>
    </form>
</body>
</html>
