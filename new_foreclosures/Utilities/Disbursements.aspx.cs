using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected void btnLoanDisbursements_Click(object sender, EventArgs e)
    {

        Response.Redirect("LoanDisbursements.aspx?loan_id=" + this.txtLoanId.Text);
    }
}