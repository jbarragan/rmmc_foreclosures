<%@ Page Language="C#" AutoEventWireup="true" CodeFile="No Inspection Ordered.aspx.cs" Inherits="lossmitigation_Default" %>

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
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Workflow", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <b>Inspection Events</b>
            </h3>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine if Inspections have been ordered, and if ordered inspections have been received.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/inspections_events.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Inspection Ordered Date</b><br />
            DB: (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = loan_id_column and lower(memo.memo_subject) like '%property inspection report%' order by memo.memo_create_dt desc ) as inspection_events_last_ordered_date<br />
            <img src="images/inspection_ordered_date.png" alt="inspection_ordered_date"></img><br />
            <br /><br /><br />
            <b>Inspection Received Date</b><br />
            DB: (select top 1 memo.memo_notify_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'PI-%' order by memo.memo_notify_dt desc ) as inspection_events_last_received_notify_date<br />
            <img src="images/inspection_received_date.png" alt="inspection_received_date"></img><br />
        </p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
