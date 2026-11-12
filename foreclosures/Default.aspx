<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="lossmitigation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foreclosures.</title>
    <link href="../assets/css/bootstrapLM.css" rel="stylesheet" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <script src="../assets/js/d3.js" type="text/javascript" language="javascript"></script>
    <script src="../assets/js/jquery_d3.js" type="text/javascript" language="javascript"></script>
    <script src="../assets/js/bootstrap_d3.js" type="text/javascript" language="javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit noprint">
        <img src="../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosures</h1>
        <p>Home</p>
    </div>
    <%= com.sp.rmmc.common.views.CommonNav.main_nav_select("Foreclosures", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_main_nav_select("Main Home", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span4"></div>
        <div class="span8">
            <h2>Foreclosures</h2>
        </div>
    </div>

    <div class="row">
        <div class="span12">
            <p><a href="Graph.aspx">Foreclosures Events Graph</a></p>            
            <p><a href="http://localrmmc/devintranet/lossmitigation/Foreclosures/Default.aspx">LM Foreclosures</a></p>
            <p class="text-error"><asp:Label ID="e" runat="server" Text="" ></asp:Label></p>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
