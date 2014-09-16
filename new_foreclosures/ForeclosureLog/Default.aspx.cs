using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;
using com.sp.rmmc.common.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected List<ForeclosureLog> foreclosure_logs = new List<ForeclosureLog>();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string loan_id_str = Request.QueryString["loan_id"];
            decimal loan_id = Convert.ToDecimal(loan_id_str);
            foreclosure_logs = ForeclosureLog.get_loan_foreclosure_logs(loan_id);

        }
        catch (Exception)
        {
            Response.Redirect("../");
        }
    }

}