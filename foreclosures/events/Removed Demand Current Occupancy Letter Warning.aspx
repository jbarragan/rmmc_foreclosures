<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Removed Demand Current Occupancy Letter Warning.aspx.cs" Inherits="lossmitigation_Default" %>

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
            <b>Removed Demand Current Occupancy Letter Events</b>
            </h3>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine if Removed Demand Current Occupancy Letter does exist.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/removed_demand_occupancy_letter_events.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Current Occupancy Letter Date</b><br />
            DB: (select top 1 memo.memo_create_dt from ms_loan_memo memo where memo.loan_id = loan_id_column and memo.memo_type_id = 138 order by memo_id desc) as xdemand_occupancy_letter_events_nl_current_occupancy_letter_date<br />
            <img src="images/current_occupancy_letter_date.PNG" alt="current_occupancy_letter_date"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
