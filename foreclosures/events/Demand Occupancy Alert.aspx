<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Demand Occupancy Alert.aspx.cs" Inherits="lossmitigation_Default" %>

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
            <b><%= this.fileName %></b>
            </h3>
        </p>
        <p>
            <b>Purpose:</b><br />
            Raise alert when a new Inspection (PI) Memo is generated and the Occupancy Date is not updated for FHA loan only.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/demand_occupancy_alert.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>FHA Occupancy Date</b><br />
            DB: ms_loan_occupancy.fcl_hud_notice_of_occ_sent<br />
            <img src="images/credit_information_default_reason_code.png" alt="FHA Occupancy Date"></img><br />
            <br /><br /><br />
            <b>Notify Date From Last PI Memo</b><br />
            DB: (select top 1 memo.memo_notify_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and (ms_memo_types.memo_type_desc like 'PI-%') order by memo_notify_dt desc ) as demand_occupancy_events_last_pi_memo_date<br />
            <img src="images/demand_occupancy_PI_memo_notify_date.png" alt="Notify Date From Last PI Memo"></img><br />
        </p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
