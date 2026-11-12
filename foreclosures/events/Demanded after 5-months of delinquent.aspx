<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Demanded after 5-months of delinquent.aspx.cs" Inherits="lossmitigation_Default" %>

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
            <b>Demanded after 5-months of delinquent</b>
            </h3>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine if CNV loan was demanded 5 months after its delinquent date.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/demanded_after_5-months_of_delinquent.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Demand Due Date</b><br />
            DB: (select top 1 misc.misc_dt_value from ms_loan_misc_fields misc where misc_field_id = 10020 and misc.loan_id = base.loan_id order by misc.misc_dt_value ) as demand_letter_due_date<br />
            <img src="images/demand_due_date.png" alt="demand_due_date"></img><br />
            <br /><br /><br />
            <b>Delinquent Date</b><br />
            DB: (select top 1 ms_loan_info.due_date_next_payment from ms_loan_information ms_loan_info where ms_loan_info.loan_id = base.loan_id order by ms_loan_info.due_date_next_payment ) as ms_loan_due_date_next_payment<br />
            <img src="images/delinquent_date.png" alt="delinquent_date"></img><br />
        </p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
