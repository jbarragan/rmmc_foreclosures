<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Eviction Process Warning.aspx.cs" Inherits="lossmitigation_Default" %>

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
            <b>Eviction Process</b>
            </h3>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine if Eviction Process is pending.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/eviction_process.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Eviction Initiated</b><br />
            DB: (select top 1 misc.misc_field_value from ms_loan_misc_fields misc where misc_field_id = 10084 and misc.loan_id = base.loan_id order by misc.misc_field_value ) as sale_eviction_process_events_fs_eviction_initiated<br />
            <img src="images/eviction_process_fc_eviction_initiated.png" alt="eviction_process_fc_eviction_initiated"></img><br />
            <br /><br /><br />
            <b>Eviction Stop Date</b><br />
            DB: (select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10154 and misc.loan_id = base.loan_id order by misc.misc_dt_value ) as sale_eviction_process_events_eviction_stop_date<br />
            <img src="images/eviction_process_eviction_stop.png" alt="eviction_process_eviction_stop"></img><br />
        </p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
