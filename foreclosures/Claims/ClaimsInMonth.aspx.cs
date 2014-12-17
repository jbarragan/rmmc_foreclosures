using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.sp.rmmc.foreclosures.models;
using com.sp.rmmc.common.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected List<Claim> claims = new List<Claim>();
    protected Claim claims_total = new Claim();
    protected DateTime date = new DateTime();

    protected void Page_Load(object sender, EventArgs e)
    {
        populateClaims();
    }

    protected void populateClaims()
    {
        try
        {
            string month_s = Request.QueryString["month"];
            int month = Convert.ToInt32(month_s);

            string year_s = Request.QueryString["year"];
            int year = Convert.ToInt32(year_s);
            claims = Claim.getMonthlyClaims(month, year);
            claims_total = Claim.getClaimsTotals(claims);
            date = new DateTime(year, month, 1);

        }
        catch (Exception)
        {
            Response.Redirect("../");
        }
    }

}