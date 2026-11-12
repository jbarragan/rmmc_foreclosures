<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Process.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures - Process</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosure Process</h1>
        <h2>Loan Different Stages during Foreclosure Process</h2>
        <p></p>
    </div>
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Workflow", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.workflow_list_nav_select("Process", version)%>
    <div class="row">
        <div class="span12">
            <h2>Foreclosure Process</h2>
            This Foreclosure Application categories loans in Foreclosure in the following categories:
            <ol>
            <li><a href="Demands.aspx">Demands</a></li>
            <li><a href="PreForeclosures.aspx">Pre-Foreclosures</a></li>
            <li><a href="Foreclosures.aspx">Foreclosures</a></li>
            <li>Foreclosed</li>
            </ol>
            <br />
        </div>
    </div>
    </div>
    </form>
</body>
</html>
