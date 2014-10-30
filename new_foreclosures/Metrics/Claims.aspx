<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Claims.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Metrics: Claims.</title>
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
        <p>Claims.</p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.sub_main_nav_select("Metrics", new ADUser(HttpContext.Current.User.Identity.Name.ToString())) %>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.metrics_list_nav_select("Claims")%>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span3">
        <h2>Claims By Month.</h2><br />
        <table class="table table-striped">
            <tr><th>Month</th><th>Number of Claims</th></tr>
            <% foreach (ClaimsByMonth claims_by_month in claims_by_month_list) { %>
             <tr>
                <td><a href="ClaimsInMonth.aspx?month=<%= claims_by_month.month.Month %>&year=<%=claims_by_month.month.Year %>"><%= claims_by_month.month.ToString("MMMM, yyyy") %></a></td>
                <td><%= claims_by_month.claims.Count %></td>
             </tr>
            <% } %>
        </table>
        </div>
        <div class="span9">
            <h3><br /><br /></h3>
            <div id="chart2" class="chart2">
            <svg class="chart"></svg>
            </div>
        </div>
        <script type="text/javascript" language="javascript">
          var margin = { top: 20, right: 55, bottom: 30, left: 40 },
          width = 1100 - margin.left - margin.right,
          height = 500 - margin.top - margin.bottom;

          var x = d3.scale.ordinal()
            .rangeRoundBands([0, width], .1);

          var y = d3.scale.linear()
            .range([height, 0]);

          var xAxis = d3.svg.axis()
            .scale(x)
            .orient("bottom");

          var yAxis = d3.svg.axis()
            .scale(y)
            .orient("left");
          
          var svg = d3.select(".chart")
                        .attr("width", width)
                        .attr("height", height);

          data = [
    <% 
        for (int i = claims_by_month_list.Count - 1; i >= 0; i--)
        {
            ClaimsByMonth claim_by_month = claims_by_month_list[i];
            string array_comma = ",";
            if (i == 0) array_comma = "";
            string link = "ClaimsInMonth.aspx?month=" + claim_by_month.month.Month + "&year=" + claim_by_month.month.Year;
     %>
        {month: "<%= claim_by_month.month.ToString("MMM yy")%>", 
         value: <%= claim_by_month.claims.Count%>,
         link: "<%= link %>"} <%=array_comma%>
     <%   
        }   
     %>
          ]
           x.domain(data.map(function (d) { return d.month; }));
           y.domain([0, d3.max(data, function (d) { return d.value; })]);

           svg.append("g")
              .attr("class", "x axis")
              .attr("transform", "translate(0," + height + ")")
              .call(xAxis)
              .selectAll("text")  
                .style("text-anchor", "end")
                .attr("dx", "-.8em")
                .attr("dy", ".15em")
                .attr("transform", function(d) { return "rotate(-65)" });

           svg.append("g")
              .attr("class", "y axis")
              .call(yAxis)
              .append("text")
                  .attr("transform", "rotate(-90)")
                  .attr("y", 6)
                  .attr("dy", ".71em")
                  .style("text-anchor", "end")
                  .text("Monthly Claims");

           svg.selectAll(".bar")
              .data(data)
              .enter()
              .append("text")
                  .attr("x", function (d) { return x(d.month) + x.rangeBand() / 2; })
                  .attr("y", function (d) { return y(d.value) - 10; })
                  .attr("dx", ".35em")
                  .style("text-anchor", "end")
                  .text(function (d) { return d.value; });

           svg.selectAll(".bar")
              .data(data)
              .enter()
              .append("svg:a").attr("xlink:href", function(d) { return d.link; })
              .append("rect")
                  .attr("class", "bar")
                  .attr("x", function (d) { return x(d.month); })
                  .attr("width", x.rangeBand())
                  .attr("y", function (d) { return y(d.value); })
                  .attr("height", function (d) { return height - y(d.value); });

            function type(d) {
                d.value = +d.value; // coerce to number
                return d;
            }
        </script>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
