<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures: Foreclosed Tracking.</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
</head>
<body>
    <form id="form2" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosures</h1>
        <p>Foreclosed Tracking.</p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.sub_main_nav %>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h2>Foreclosures Types</h2>
        <br /><a href="fhaForeclosures.aspx">FHA Foreclosures Dashboard</a>
        <br /><a href="vaForeclosures.aspx">VA Foreclosures Dashboard</a>
        <br /><a href="convForeclosures.aspx">Conventional Foreclosures Dashboard</a>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
