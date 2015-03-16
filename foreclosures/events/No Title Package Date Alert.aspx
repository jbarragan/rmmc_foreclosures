<%@ Page Language="C#" AutoEventWireup="true" CodeFile="No Title Package Date Alert.aspx.cs" Inherits="lossmitigation_Default" %>

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
            <b>No Title Package Events</b>
            </h3>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine if Title Package Date exist after Conveyance Date for FHA Loans.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/title_package_date_events.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Conveyance Date</b><br />
            DB: (select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11200 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_ord_req ) as sale_conveyance_events_conveyance_date<br />
            <img src="images/conveyance_date.png" alt="conveyance_date"></img><br />
            <b>Title Package Date</b><br />
            DB: (select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11500 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_ord_req ) as sale_title_package_events_title_package_date<br />
            <img src="images/title_package_date.png" alt="title_package_date"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
