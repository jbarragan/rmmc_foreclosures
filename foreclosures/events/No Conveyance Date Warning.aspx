<%@ Page Language="C#" AutoEventWireup="true" CodeFile="No Conveyance Date Warning.aspx.cs" Inherits="lossmitigation_Default" %>

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
            <b>Conveyance Events</b>
            </h3>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine if Conveyance Date exist after Possesion Date for FHA Loans.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/conveyance_events.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Possesion Date</b><br />
            DB: (select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1900 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_date ) as sale_conveyance_events_possesion_date<br />
            <img src="images/possesion_date.png" alt="possesion_date"></img><br />
            <br /><br /><br />
            <b>Conveyance Date</b><br />
            DB: (select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11200 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_ord_req ) as sale_conveyance_events_conveyance_date<br />
            <img src="images/conveyance_date.png" alt="conveyance_date"></img><br />
            <b>FC-REO PROPERTY Date</b><br />
            DB: (select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'FC-REO%' order by memo_create_dt desc ) as sale_conveyance_events_reo_date<br />
        </p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
