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
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Workflow", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <p>
            <h3>
            <b>Demand Request for Extension of Time Alert</b>
            </h3>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine if Foreclosure Extension is required for FHA loans.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/demand_request_for_extension.png" alt="demand_request_for_extension"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Extension Ordered Date</b><br />
            DB: (select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 10800 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_ord_req ) as demand_request_for_extension_events_extension_ordered_date<br />
            <img src="images/extension_date.png" alt="extension_date"></img><br />
            <br /><br /><br />
            <b>Foreclosure Date</b><br />
            DB: (select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1200 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_recvd_expd ) as demand_request_for_extension_events_foreclosure_date<br />
            <img src="images/foreclosure_date.png" alt="foreclosure_date"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
