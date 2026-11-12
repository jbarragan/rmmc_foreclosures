<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Papers Not Received From Attorney Alert.aspx.cs" Inherits="lossmitigation_Default" %>

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
            <b>Papers Not Received From Attorney Alert</b>
            </h3>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine if Papers have been received from Attorney and a case number exists.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/papers_to_attorney.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Papers to Attorney Ordered Date</b><br />
            DB: (select top 1 ms_fcl.fcl_item_date from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1000 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as foreclosure_papers_to_attorney_events_papers_to_attorney_date<br />
            <img src="images/papers_to_attorney_ordered_date.png" alt="papers_to_attorney_ordered_date"></img><br />
            <br /><br /><br />
            <b>Papers to Attorney Received Date</b><br />
            DB: (select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1000 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as foreclosure_papers_to_attorney_events_received_from_attorney_date<br />
            <img src="images/papers_to_attorney_received_date.png" alt="papers_to_attorney_received_date"></img><br />
            <br /><br /><br />
            <b>Papers to Attorney Description</b><br />
            DB: (select top 1 ms_fcl.fcl_item_desc from ms_fcl_item_amts_1 ms_fcl where ms_fcl.item_id = 1000 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_date ) as foreclosure_papers_to_attorney_events_received_from_attorney_desc<br />
            <img src="images/papers_to_attorney_description.png" alt="papers_to_attorney_description"></img><br />
            <br /><br /><br />
        </p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
