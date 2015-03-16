<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Removed Demand Mortgage Status not 20 Warning.aspx.cs" Inherits="lossmitigation_Default" %>

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
            <b>Removed Demand Mortgage Status Events</b>
            </h3>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine if Removed Demand Mortgage Status is 20
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/removed_demand_mortgage_not_20.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Mortgage Status</b><br />
            DB: (select top 1 ms_credit_information.mortgage_status from ms_credit_information where ms_credit_information.loan_id = loan_id_column order by ms_credit_information.mortgage_status ) as xdemand_mortgage_status_events_mortgage_status<br />
            <img src="images/mortgage_status.png" alt="mortgage_status"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
