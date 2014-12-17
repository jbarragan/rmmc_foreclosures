<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreservationCodes.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Utilities: Preservation Codes</title>
    <link href="../../assets/css/bootstrapLM.css" rel="stylesheet"/>
    <script type="text/jscript" src="../../assets/js/jquery.js" ></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container-print">
    <div class="hero-unit">
        <img src="../../assets/img/logo.png" class="img-rounded" alt="RMMC Logo" />
        <h1>Utilities</h1>
        <p>Preservation Codes</p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.sub_main_nav %>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.utilities_nav%>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h4>
         View Loan Preservation Disbursements:
         <asp:TextBox runat="server" ID="txtLoanId" MaxLength="10" Width="100px"></asp:TextBox>
         <asp:Button ID="btnLoanDisbursements" runat="server" Text="View" CssClass="btn" onclick="btnLoanDisbursements_Click" />
       </h4>
        <h4><a href="NewPreservationCode.aspx">Create New Preservation Code</a></h4>
        <table>
        <%
            foreach (TypeList currentTypeList in this.preservation_loan_types)
            {
                com.sp.rmmc.foreclosures.models.PreservationCode first_preservation_code = currentTypeList[0][0];
       %>
        <tr>
        <td width="150px"><h3><%= first_preservation_code.type %></h3></td>
        <td><a class="btn btn-primary" onclick="var e = document.getElementById('<%= first_preservation_code.type.Replace(" ","_") + "_type_level" %>'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></td>
        </tr>
        <tr>
        <td colspan="2">
            <div id="<%= first_preservation_code.type.Replace(" ","_") + "_type_level" %>" style="display:none">
            <table>
       <%
           foreach (ServiceList currentServiceList in currentTypeList)
                {
                    first_preservation_code = currentServiceList[0];
        %>
        <tr>
        <td width="600px"><h4><%= first_preservation_code.type + " - " + first_preservation_code.general_service %>(<%= currentServiceList.Count%>)</h4></td>
        <td><a class="btn btn-primary" onclick="var e = document.getElementById('<%= first_preservation_code.type.Replace(" ","_") + "_" + first_preservation_code.general_service.Replace(" ","_") %>'); e.style.display = e.style.display == 'block' ? 'none' : 'block';"><i class="icon-th-list icon-white"></i> Data</a></td>
        </tr>
        <tr>
        <td colspan="2">
            <div id="<%= first_preservation_code.type.Replace(" ","_") + "_" + first_preservation_code.general_service.Replace(" ","_") %>" style="display:none">
                <table class="table table-striped">
                    <tr>
                        <th>Code</th>
                        <th>Type</th>
                        <th>Description</th>
                        <th>Service</th>
                        <th>Allowance</th>
                        <th>Enabled</th>
                    </tr>
                    <% foreach (com.sp.rmmc.foreclosures.models.PreservationCode preservation_code in currentServiceList)
                        {   
                    %>
                    <tr>
                        <td><a href="editPreservationCode.aspx?code=<%= preservation_code.code.ToString()%>"><%= preservation_code.code.ToString()%></a></td>
                        <td><%= preservation_code.type %></td>
                        <td><%= preservation_code.description %></td>
                        <td><%= preservation_code.general_service %></td>
                        <td><%= preservation_code.allowance.ToString("C") %></td>
                        <td><%= preservation_code.enabled.ToString() %></td>
                        </tr>
                    <%
                        }
                    %>
                </table>
            </div>
            </td>
            </tr>
        <%
                }
        %>
            </table>
            </div>
        </td>
        </tr>
        <%
            }
        %>
        </table>
        </div>
     </div>
    <br />
    <br />
    <div class="row">
        <div class="span12">
            <div><a href="../Default.aspx">Back to Home</a></div>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
