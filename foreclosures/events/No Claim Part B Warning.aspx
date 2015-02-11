<%@ Page Language="C#" AutoEventWireup="true" CodeFile="No Claim Part B Warning.aspx.cs" Inherits="lossmitigation_Default" %>

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
            <b>No Claim Part B</b>
            </h3>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine if Claim Part B has been received after Title Approval Date.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/no_claim_part_b.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Claim Part B Date</b><br />
            DB: (select top 1 ms_fcl.fcl_item_recvd_expd from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11900 and ms_fcl.loan_id = base.loan_id order by ms_fcl.fcl_item_recvd_expd ) as sale_claim_part_b_events_claim_part_b_date<br />
            <img src="images/no_claim_part_b_part_b_date.png" alt="no_claim_part_b_part_b_date"></img><br />
            <br /><br /><br />
            <b>Title Approval Date</b><br />
            DB: (select top 1 ms_fcl.fcl_item_ord_req from ms_fcl_item_amts_2 ms_fcl where ms_fcl.item_id = 11600 and ms_fcl.loan_id = loan_id_column order by ms_fcl.fcl_item_ord_req ) as sale_claim_part_b_events_title_approval_date<br />
            <img src="images/no_claim_part_b_title_approval_date.png" alt="no_claim_part_b_title_approval_date"></img><br />
        </p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
