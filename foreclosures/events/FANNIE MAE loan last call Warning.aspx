<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FANNIE MAE loan last call Warning.aspx.cs" Inherits="lossmitigation_Default" %>

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
            <b>Fannie Mae Calls Warning</b>
            </h3>
        </p>
        <p>
            <b>Purpose:</b><br />
            Determine if Fannie Mae call is required by tracking Last Memo call and Promise to Pay dates in Conventional Loans.
        </p>
        <p>
            <b>Logic:</b><br />
            <img src="images/FANNIE_MAE_loan_last_call_warning.png" alt="event logic"></img>
        </p>
        <p>
            <b>Data Sources:</b><br />
            <b>Last BC Call Memo Created Date</b><br />
            DB: select top 1 memo.memo_create_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc like 'BC%' order by memo_id desc ) as fannie_mae_calls_events_last_bc_call_memo_date<br />
            <img src="images/FANNIE_MAE_loan_last_call_last_bc_memo.PNG" alt="FANNIE MAE loan last call last bc memo"></img><br />
            <br /><br /><br />
            <b>Last BC Promised to pay Memo Notify Date</b><br />
            DB: (select top 1 memo.memo_notify_dt from ms_loan_memo memo, ms_memo_types where memo.loan_id = loan_id_column and memo.memo_type_id = ms_memo_types.memo_type_id and ms_memo_types.memo_type_desc = 'BC-Promised to Pay' order by memo_notify_dt desc ) as fannie_mae_calls_events_last_bc_promised_to_pay_date<br />
            <img src="images/FANNIE_MAE_loan_last_call_last_bc_promised_to_pay_memo.PNG" alt="FANNIE_MAE_loan_last_call_last_bc_promised_to_pay_memo"></img><br />
        </p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
