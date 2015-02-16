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
    <div class="hero-unit">
        <img src="../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Foreclosures</h1>
        <p>Home</p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_main_nav_select("Main Home", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span4"></div>
        <div class="span8">
            <h2>Current Foreclosure Events</h2>
            <div id="chart"></div>
        </div>
    </div>

    <div class="row">
        <div class="span12">
            <p><a href="http://localrmmc/devintranet/lossmitigation/Foreclosures/Default.aspx">LM Foreclosures</a></p>
            <p class="text-error"><asp:Label ID="e" runat="server" Text="" ></asp:Label></p>
        </div>
    </div>
    </div>
    </div>
    </form>
    <script>
        var w = 400;
        var h = 400;
        var r = h / 2;
        var color = d3.scale.ordinal()
                .range(["#d62728", "#fcf8e3", "#ffffff"]);

        var data = [{ "label": "Alerts(<%= this.alert_events.Count %>)", "value": <%= this.alert_events.Count %>, "link": "Workflow/Events.aspx?workflow=ALL&loan_officer=ALL&loan_id=ALL&loan_type=ALL&event_type=1"},
		          { "label": "Warnings(<%= this.warning_events.Count %>)", "value": <%= this.warning_events.Count %>, "link": "Workflow/Events.aspx?workflow=ALL&loan_officer=ALL&loan_id=ALL&loan_type=ALL&event_type=0"}];


        var vis = d3.select('#chart').append("svg:svg").data([data]).attr("width", w).attr("height", h).append("svg:g").attr("transform", "translate(" + r + "," + r + ")");
        var pie = d3.layout.pie().value(function (d) { return d.value; });

        // declare an arc generator function
        var arc = d3.svg.arc().outerRadius(r);

        // select paths, use arc generator to draw
        var slice_group = vis.selectAll("g.slice").data(pie).enter()
            .append("svg:a").attr("xlink:href", function(d) { 
                return d.data.link; 
             });
        var arcs = slice_group.append("svg:g").attr("class", "slice");
        
        arcs.append("svg:path")
        .attr("fill", function (d, i) {
            return color(i);
        });
    
        // add border
        arcs.append("path")
            .attr("fill", function (d, i) {
                return color(i);
            })
            .attr("d", arc).style('stroke', 'black')
            .style('stroke-width', 1);


        // add the text
        arcs.append("svg:text").attr("transform", function (d) {
            d.innerRadius = 0;
            d.outerRadius = r;
            return "translate(" + arc.centroid(d) + ")";
        }).attr("text-anchor", "middle").text(function (d, i) {
            return data[i].label;
        }
		);
        
        //arcs.append("svg:a").attr("xlink:href", function(d) { return d.link; });

    </script>
</body>
</html>
