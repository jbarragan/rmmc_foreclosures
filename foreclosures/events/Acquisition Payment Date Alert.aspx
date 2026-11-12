<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Acquisition Payment Date Alert.aspx.cs" Inherits="lossmitigation_Default" %>

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
            <b>Acquisition Payment Events</b>
            </h3>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine if Acquisition Payment Date exists after Transfer of Custody Date for VA Loans.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/acquisition_payment_date_events.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Acquisition Payment Date</b><br />
            DB: (select top 1 fcl_ud.fcl_ud_item_recvd_expd from ms_fcl_ud_item_amts_1 fcl_ud where fcl_ud.loan_id = loan_id_column and fcl_ud.fcl_ud_item_desc like '%QUISITION PAY%' order by fcl_ud_item_recvd_expd desc) as sale_acquisition_payment_events_acquisition_payment_date<br />
            <img src="images/acquisition_payment_date.png" alt="acquisition_payment_date"></img><br />
            <b>Transfer of Custody Date</b><br />
            DB: (select top 1 fcl_ud.fcl_ud_item_date from ms_fcl_ud_item_amts_1 fcl_ud where fcl_ud.loan_id = loan_id_column and fcl_ud.fcl_ud_item_desc like '%QUISITION PAY%' order by fcl_ud_item_date desc) as sale_transfer_custody_events_transfer_of_custody_date<br />
            <img src="images/transfer_of_custody_date.png" alt="transfer_of_custody"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
