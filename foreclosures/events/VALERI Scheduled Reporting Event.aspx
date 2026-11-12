<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Demand Request for Extension of Time Alert.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - Event - <%= this.fileName %></title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1><%= this.fileName %></h1>
        <p></p>
    </div>
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Workflow", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <b>VALERI Scheduled Reporting Event</b>
            </h3>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine if VALERI Schedule Memo exists for VA loans with posting date and Foreclosure Scheduled Action.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/valeri_scheduled_reporting_event.png" alt="valeri_scheduled_reporting_event"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Foreclosure Scheduled Default Action Date</b><br />
            DB: (select top 1 default_action_date from ms_default_actions mda where mda.loan_id = loan_id_column and mda.default_action_code = '71' order by mda.default_action_date desc ) as sale_valeri_events_default_action_date<br />
            <img src="images/foreclosure_scheduled_default_action_date.png" alt="foreclosure_scheduled_default_action_date"></img><br />
            <br /><br /><br />
            <b>Foreclosure Scheduled Default End Date</b><br />
            DB: "(select top 1 default_end_dt from ms_default_actions mda where mda.loan_id = loan_id_column and mda.default_action_code = '71' order by mda.default_end_dt desc ) as sale_valeri_events_default_end_dt<br />
            <img src="images/foreclosure_scheduled_default_end_date.png" alt="foreclosure_scheduled_default_end_date"></img><br />
            <br /><br /><br />
            <b>Posting Date</b><br />
            DB: (select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_date ) as sale_valeri_events_posting_date<br />
            <img src="images/posting_date.png" alt="posting_date"></img><br />
            <br /><br /><br />
            <b>FC-VA Scheduled Memo Created Date</b><br />
            DB: (select top 1 memo.memo_create_dt  from ms_loan_memo memo join ms_memo_types on memo.memo_type_id = ms_memo_types.memo_type_id where memo.loan_id = loan_id_column and ms_memo_types.memo_type_desc like 'FC-VA SCHEDULED%' order by memo.memo_create_dt desc ) as sale_valeri_events_fc_va_scheduled_memo_created_date<br />
            <img src="images/FC_VA_Scheduled_memo_created_date.PNG" alt="FC_VA_Scheduled_memo_created_date"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
