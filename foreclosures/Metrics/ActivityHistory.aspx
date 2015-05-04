<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActivityHistory.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Metrics: Foreclosure Activity.</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
    <style type="text/css">
        .bar {
            fill: steelblue;
        }

        .bar:hover {
            fill: brown;
        }

        .axis {
            font: 14px sans-serif;
        }

        .axis path,
        .axis line {
            fill: none;
            stroke: #000;
            shape-rendering: crispEdges;
        }

        .x.axis path {
            display: none;
        }
    </style>
    <script src="../../assets/js/d3.js" type="text/javascript" language="javascript"></script>
    <script src="../../assets/js/jquery_d3.js" type="text/javascript" language="javascript"></script>
    <script src="../../assets/js/bootstrap_d3.js" type="text/javascript" language="javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Metrics</h1>
        <p>Foreclosure Activity.</p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.foreclosure_sub_main_nav_select("Metrics", new ADUser(HttpContext.Current.User.Identity.Name.ToString()))%>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.metrics_list_nav_select("Activity")%>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span3">
        <h2>Activity.</h2><br />
        <table class="table table-striped">
            <tr><th>Month</th><th>Demands</th><th>Foreclosures</th><th>Foreclosed</th><th>DeedInLieu</th><th>Total</th></tr>
            <% int year = 0;
               ActivityByMonth activity_by_year = null;
               foreach (ActivityByMonth activity_by_month in activity_by_month_list)
               {
                   if (activity_by_month.date.Year != year)
                   {
                       if (year != 0 && activity_by_year != null)
                       {
            %>
             <tr class="info">
                <td><%= (activity_by_year.date.Year == DateTime.Today.Year) ? "YTD" : activity_by_year.date.Year.ToString()%></a></td>
                <td><%= activity_by_year.demands.Count%></td>
                <td><%= activity_by_year.pre_and_foreclosures.Count%></td>
                <td><%= activity_by_year.foreclosed.Count%></td>
                <td><%= activity_by_year.deed_in_lieu.Count%></td>
                <td><%= activity_by_year.total.Count%></td>
             </tr>      
            <%
                       }
                       year = activity_by_month.date.Year;
                       activity_by_year = new ActivityByMonth(activity_by_month.date, "");
                       activity_by_year.add(activity_by_month);
                   }
                   else
                   {
                       activity_by_year.add(activity_by_month);
                   }
             %>
             <tr>
                <td><a href="../Workflow/Default.aspx?version=<%= activity_by_month.history_id %>&workflow=Demands"><%= activity_by_month.activity_title %></a></td>
                <td><%= activity_by_month.demands.Count %></td>
                <td><%= activity_by_month.pre_and_foreclosures.Count %></td>
                <td><%= activity_by_month.foreclosed.Count %></td>
                <td><%= activity_by_month.deed_in_lieu.Count%></td>
                <td><%= activity_by_month.total.Count%></td>
             </tr>
            <% 
                }
                if (year != 0 && activity_by_year != null)
                {
            %>
             <tr class="info">
                <td><%= (activity_by_year.date.Year == DateTime.Today.Year) ? "YTD" : activity_by_year.date.Year.ToString()%></a></td>
                <td><%= activity_by_year.demands.Count%></td>
                <td><%= activity_by_year.pre_and_foreclosures.Count%></td>
                <td><%= activity_by_year.foreclosed.Count%></td>
                <td><%= activity_by_year.deed_in_lieu.Count%></td>
                <td><%= activity_by_year.total.Count%></td>
             </tr>      
            <%
                }
             %>

        </table>
        </div>
        <div class="span9">
            <h3><br /><br /></h3>
            <div id="chart2" class="chart2">
            <svg class="chart"></svg>
            </div>
        </div>        
        <script type="text/javascript" language="javascript">
            var margin = { top: 20, right: 45, bottom: 30, left: 220 },
                           width = 1100 - margin.left - margin.right,
                           height = 500 - margin.top - margin.bottom;

            var x0 = d3.scale.ordinal()
                .rangeRoundBands([0, width], .1);
            
            var x1 = d3.scale.ordinal();

            var y = d3.scale.linear()
                .range([height, 0]);

            var color = d3.scale.ordinal()
                .range(["#0000FF", "#309E32", "#FF0000", "#FFa500", "#000000"]);
            
            var text_high = d3.scale.ordinal()
                .range([-15, -25, -35, -45, -55]);

            var xAxis = d3.svg.axis()
                .scale(x0)
                .orient("bottom");

            var yAxis = d3.svg.axis()
                .scale(y)
                .orient("left")
                .tickFormat(d3.format(".2s"));

            var svg = d3.select(".chart")
                .attr("width", width + margin.left + margin.right)
                .attr("height", height + margin.top + margin.bottom)
                .append("g")
                .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

            var seriesNames = ["demands", "foreclosures", "foreclosed", "deed in lieu"];
            data = [
            <% 
            for (int i = 0 ; i < activity_by_month_list.Count; i++)
            //for (int i = activity_by_month_list.Count - 1 ; i >= 0 ; i--)
            {
                ActivityByMonth activity_by_month = activity_by_month_list[i];
                string array_comma = ",";
                if (i == activity_by_month_list.Count - 1) array_comma = "";
                //if (i == 0) array_comma = "";
                string link = "../Workflow/Default.aspx?version=" + activity_by_month.history_id + "&workflow=";
            %>     
                {Month: "<%= activity_by_month.activity_title%>", 
                 values: [{name: "demands", value: <%= activity_by_month.demands.Count %>, link: "<%= link %>Demands" },
                               {name: "foreclosures", value: <%= activity_by_month.pre_and_foreclosures.Count %>, link: "<%= link %>Foreclosures" },
                               {name: "foreclosed", value: <%= activity_by_month.foreclosed.Count %>, link: "<%= link %>Foreclosed" },
                               {name: "deed in lieu", value: <%= activity_by_month.deed_in_lieu.Count %>, link: "<%= link %>Deed In Lieu&sublist=In Process" }],
                 link: "<%= link %>"} <%=array_comma%>
            <%   
            }   
            %>
            ];
            x0.domain(data.map(function (d) { return d.Month; }));
            x1.domain(seriesNames).rangeRoundBands([0, x0.rangeBand()]);
            y.domain([0, d3.max(data, function (d) { return d3.max(d.values, function (d) { return d.value; }); })]);

            svg.append("g")
                .attr("class", "x axis")
                .attr("transform", "translate(0," + height + ")")
                .call(xAxis);

            svg.append("g")
                .attr("class", "y axis")
                .call(yAxis)
                .append("text")
                .attr("transform", "rotate(-90)")
                .attr("y", 6)
                .attr("dy", ".71em")
                .style("text-anchor", "end")
                .text("Activity");
                
            var month = svg.selectAll(".month")
                .data(data)
                .enter()      
                //.append("svg:a").attr("xlink:href", function(d) { return d.link; })
                .append("g")
                .attr("class", "g")
                .attr("transform", function (d) { return "translate(" + x0(d.Month) + ",0)"; });

            month.selectAll("rect")
                .data(function (d) { return d.values; })
                .enter()
                .append("svg:a")
                    .attr("xlink:href", function(d) { return d.link; })
                .append("rect")
                .attr("class", "bar")
                .attr("width", x1.rangeBand())
                .attr("x", function (d) { return x1(d.name); })
                .attr("y", function (d) { return y(d.value); })
                .attr("height", function (d) { return height - y(d.value); })
                .style("fill", function (d) { return color(d.name); })
                .append("svg:title")
                .style("fill", function (d) { return color(d.name); })
                .text(function (d) { return d.value; });
          
            var legend = svg.selectAll(".legend")
                .data(seriesNames.slice().reverse())
                .enter().append("g")
                .attr("class", "legend")
                .attr("transform", function (d, i) { return "translate(0," + i * 20 + ")"; });

            legend.append("rect")
                .attr("x", width - 18)
                .attr("width", 18)
                .attr("height", 18)
                .style("fill", color);

            legend.append("text")
                .attr("x", width - 24)
                .attr("y", 9)
                .attr("dy", ".35em")
                .style("text-anchor", "end")
                .text(function (d) { return d; });
        </script>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
