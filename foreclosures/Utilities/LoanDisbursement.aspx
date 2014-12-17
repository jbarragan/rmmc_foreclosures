<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoanDisbursement.aspx.cs" Inherits="lossmitigation_Collections_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Utilities: <%= ( disbursement.loan_id > 0 ? "Edit" : "New") + " Disbursement for Loan #" + loan.loan_id.ToString() + " - " + loan.loan_name %></title>
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
        <p><%= ( disbursement.loan_id > 0 ? "Edit" : "New") + " Disbursement for Loan #" + loan.loan_id.ToString() + " - " + loan.loan_name %></p>
    </div>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.sub_main_nav %>
    <%= com.sp.rmmc.foreclosures.views.NewNavigations.utilities_nav%>
    <!-- Example row of columns -->
    <div class="row">
        <div class="span12">
        <h2><%= ( disbursement.loan_id > 0 ? "Edit" : "New") + " Disbursement" %></h2>
        <table class="table table-striped" border="0">
            <tr>
                <td>Ordered Date:</td>
                <td>
                  <div id="datetimepicker1" class="input-append date">
                    <asp:TextBox runat="server" ID="txtOrderedDate" Width="100px" Enabled="true" />
                    <span class="add-on">
                        <i data-time-icon="icon-time" data-date-icon="icon-calendar"></i>
                    </span>
                  </div>
                </td>
            </tr>
            <tr>
                <td>Completed Date:</td>
                <td>
                  <div id="datetimepicker2" class="input-append date">
                    <asp:TextBox runat="server" ID="txtCompletedDate" Width="100px" Enabled="true" />
                    <span class="add-on">
                        <i data-time-icon="icon-time" data-date-icon="icon-calendar"></i>
                    </span>
                  </div>
                </td>
            </tr>
            <tr>
                <td>Paid Date:</td>
                <td>
                  <div id="datetimepicker3" class="input-append date">
                    <asp:TextBox runat="server" ID="txtPaidDate" Width="100px" Enabled="true" />
                    <span class="add-on">
                        <i data-time-icon="icon-time" data-date-icon="icon-calendar"></i>
                    </span>
                  </div>
                </td>
            </tr>
            <tr>
                <td>Preservation Code</td>
                <td><asp:DropDownList runat="server" ID="cmbPreservationCode" Width="800px"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Actual Amount</td>
                <td><asp:TextBox runat="server" ID="txtActual" MaxLength="10" Width="100px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Claimable Amount</td>
                <td><asp:TextBox runat="server" ID="txtClaimable" MaxLength="10" Width="100px"></asp:TextBox></td>
             </tr>
             <tr>
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server"
                                Text="Save"
                                CssClass="btn btn-success" onclick="btnSave_Click" />
                    <asp:Button ID="btnDelete" runat="server"
                                Text="Delete"
                                OnClientClick="return confirm('Are you sure you want to remove this Disbursement?');"
                                CssClass="btn btn-danger" onclick="btnDelete_Click" />
                   <a href="LoanDisbursements.aspx?loan_id=<%= loan.loan_id.ToString() %>">Back to Loan Disbursements</a>
                <p class="text-error"><asp:Label ID="lblError" runat="server" Text="" ></asp:Label></p>
                <p class="text-success"><asp:Label ID="lblSuccess" runat="server" Text="" ></asp:Label></p>
                </td>
             </tr>
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
    <script type="text/javascript" src="../../assets/js/jquery.js"></script>
    <script type="text/javascript" src="../../assets/js/bootstrap.js"></script>
    <script type="text/javascript" src="../../assets/js/bootstrap-datetimepicker.min.js"></script>
    <script type="text/javascript">
        $('#datetimepicker1').datetimepicker({
            pickTime: false//format: 'dd/MM/yyyy hh:mm:ss'
        });
        $('#datetimepicker2').datetimepicker({
            pickTime: false, //format: 'dd/MM/yyyy hh:mm:ss'
            endDate: new Date()
        });
        $('#datetimepicker3').datetimepicker({
            pickTime: false, //format: 'dd/MM/yyyy hh:mm:ss'
            endDate: new Date()
        });
    </script>
</body>
</html>
